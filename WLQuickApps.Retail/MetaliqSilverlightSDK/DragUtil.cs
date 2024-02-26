using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace MetaliqSilverlightSDK
{
    public class DragInfoEvent : EventArgs
    {
        public readonly DragInfo Info;
        public readonly List<FrameworkElement> AffectedTargets = new List<FrameworkElement>();

        public DragInfoEvent(DragInfo _Info, List<FrameworkElement> Targets)
        {
            AffectedTargets = Targets;
            Info = _Info;
        }
        public DragInfoEvent(DragInfo _Info, FrameworkElement Target)
        {
            AffectedTargets.AddUnique(Target);
            Info = _Info;
        }
        public DragInfoEvent(DragInfo _Info)
        {
            Info = _Info;
        }
        public void AddAffectedTarget(FrameworkElement Target)
        {
            AffectedTargets.AddUnique(Target);
        }
        public FrameworkElement AffectedTarget
        {
            get
            {
                return AffectedTargets[0];
            }
        }
    }
    public enum DragEventType
    {
        DragMove, DragStart, DragComplete, DragDrop, DragOver, DragOut
    }
    public class DragInfo
    {
        public event EventHandler DragMove;
        public event EventHandler DragStart;
        public event EventHandler DragComplete;
        public event EventHandler<DragInfoEvent> DragDrop;
        public event EventHandler<DragInfoEvent> DragOver;
        public event EventHandler<DragInfoEvent> DragOut;

        public List<FrameworkElement> Targets = new List<FrameworkElement>();
        public bool Active = true;
        public bool SnapBack = false;
        protected Point StartPosition;
        protected Panel StartParent;
        public readonly FrameworkElement Element;
        public readonly Panel Context;
        protected Dictionary<FrameworkElement, bool> TargetHitMap = new Dictionary<FrameworkElement, bool>();
        public DragInfo(FrameworkElement DragItem, Panel DragContext)
        {
            Element = DragItem;
            Context = DragContext;
        }
        public DragInfo(FrameworkElement DragItem, Panel DragContext, List<FrameworkElement> DragTargets)
        {
            Element = DragItem;
            Context = DragContext;
            Targets = DragTargets;
        }
        public void DispatchDragEvent(DragEventType Type)
        {
            DispatchDragEvent(Type, null);
        }
        public void DispatchDragEvent(DragEventType Type, DragInfoEvent Info)
        {
            switch (Type)
            {
                case DragEventType.DragComplete:
                    if (DragComplete != null)
                    {
                        DragComplete(this, Info);
                    }
                    break;
                case DragEventType.DragDrop:
                    if (DragDrop != null)
                    {
                        DragDrop(this, Info);
                    }
                    break;
                case DragEventType.DragMove:
                    if (DragMove != null)
                    {
                        DragMove(this, Info);
                    }
                    break;
                case DragEventType.DragOut:
                    if (DragOut != null)
                    {
                        DragOut(this, Info);
                    }
                    break;
                case DragEventType.DragOver:
                    if (DragOver != null)
                    {
                        DragOver(this, Info);
                    }
                    break;
                case DragEventType.DragStart:
                    if (DragStart != null)
                    {
                        StartPosition = new Point(Element.GetX(), Element.GetY());
                        StartParent = (Panel)Element.Parent;
                        DragStart(this, Info);
                    }
                    break;
            }
        }
        public void AddTargetHit(FrameworkElement HitTarget)
        {
            TargetHitMap[HitTarget] = true;
        }
        public void RemoveTargetHit(FrameworkElement HitTarget)
        {
            TargetHitMap[HitTarget] = false;
        }
        public bool IsOverTarget(FrameworkElement CandidateTarget)
        {
            bool result = false;
            try
            {
                result = TargetHitMap[CandidateTarget];
            }
            catch (KeyNotFoundException ex)
            {
            }
            return result;
        }
        public void ResetDropTargetState()
        {
            foreach (FrameworkElement target in TargetHitMap.Keys)
            {
                TargetHitMap[target] = false;
            }
        }
        public List<FrameworkElement> DroppedTargets
        {
            get
            {
                List<FrameworkElement> result = new List<FrameworkElement>();
                foreach (FrameworkElement target in TargetHitMap.Keys)
                {
                    if (TargetHitMap[target] == true)
                    {
                        result.Add(target);
                    }
                }
                return result;
            }
        }
    }
    public class DragUtil
    {
        protected static Point MousePosition = new Point();
        protected static Dictionary<FrameworkElement, DragInfo> DragInfoCollection = new Dictionary<FrameworkElement, DragInfo>();
        protected static Dictionary<FrameworkElement, bool> MouseCaptureMap = new Dictionary<FrameworkElement, bool>();
        protected static Dictionary<FrameworkElement, Panel> DragContextMap = new Dictionary<FrameworkElement, Panel>();

        public static DragInfo GetInfo(FrameworkElement item)
        {
            return DragInfoCollection[item];
        }

        public static DragInfo MakeDraggable(FrameworkElement Element, Panel Context)
        {
            DragInfo info = null;
            if (DragInfoCollection.ContainsKey(Element) == false)
            {
                Element.CaptureMouse();
                Element.MouseMove += new MouseEventHandler(Element_MouseMove);
                Element.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
                Element.MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
                DragContextMap[Element] = Context;
                info = new DragInfo(Element, Context);
                DragInfoCollection[Element] = info;
                MouseCaptureMap[Element] = false;
            }
            else
            {
                info = DragInfoCollection[Element];
            }

            return info;
        }

        static void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            UserInterfaceUtil.ChangeParentKeepPosition(item, DragContextMap[item], e);
            MousePosition.X = e.GetPosition(null).X;
            MousePosition.Y = e.GetPosition(null).Y;
            MouseCaptureMap[item] = true;
            item.CaptureMouse(); // this will invoke a MouseMove Event
            // dispatch event
            GetInfo(item).DispatchDragEvent(DragEventType.DragStart);
        }

        static void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            DragInfo info = GetInfo(item);
            if (info.DroppedTargets.Count > 0)
            {
                DragInfoEvent DragEvent = new DragInfoEvent(info, info.DroppedTargets);
                info.DispatchDragEvent(DragEventType.DragDrop, DragEvent);
            }
            MouseCaptureMap[item] = false;
            item.ReleaseMouseCapture();
            info.DispatchDragEvent(DragEventType.DragComplete);
        }

        static void Element_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            DragInfo info = DragInfoCollection[item];

            if (info.Active)
            {
                var IsMouseCaptured = MouseCaptureMap[item];
                if (IsMouseCaptured)
                {
                    Double deltaH = e.GetPosition(null).X - MousePosition.X;
                    Double deltaV = e.GetPosition(null).Y - MousePosition.Y;
                    item.SetX(deltaH + item.GetX());
                    item.SetY(deltaV + item.GetY());
                }
                MousePosition.X = e.GetPosition(null).X;
                MousePosition.Y = e.GetPosition(null).Y;
                item.SetValue(Canvas.ZIndexProperty, 2000);

                DragInfoEvent TargetEvent;
                foreach (FrameworkElement element in info.Targets)
                {
                    TargetEvent = new DragInfoEvent(info, element);
                    if (CollisionUtil.CheckCollision(item, element))
                    {
                        if (info.IsOverTarget(element) == false)
                        {
                            info.AddTargetHit(element);
                            info.DispatchDragEvent(DragEventType.DragOver, TargetEvent);
                        }
                    }
                    else
                    {
                        if (info.IsOverTarget(element) == true)
                        {
                            info.DispatchDragEvent(DragEventType.DragOut, TargetEvent);
                        }
                        info.RemoveTargetHit(element);
                        element.Opacity = 1;
                    }
                }
                info.DispatchDragEvent(DragEventType.DragMove);
            }
        }
    }
}
