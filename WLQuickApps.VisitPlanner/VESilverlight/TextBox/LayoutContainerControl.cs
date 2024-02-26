//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace VESilverlight
{
    public class LayoutContainerControl : Canvas, ILayout, IGrid
    {
        public LayoutContainerControl()
        {
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseLeftButtonUp += OnMouseLeftButtonUp;
            MouseMove += OnMouseMove;
            FocusManager.MouseHasLeftControl += OnMouseHasLeftControl;
        }

        #region EventHandlers
        private void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OnMouseEnterCore(sender, e);
        }
        protected virtual void OnMouseEnterCore(object sender, System.Windows.Input.MouseEventArgs e) { }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            OnMouseLeaveCore(sender, e);
        }
        protected virtual void OnMouseLeaveCore(object sender, EventArgs e) { }

        private void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OnMouseLeftButtonDownCore(sender, e);
        }
        protected virtual void OnMouseLeftButtonDownCore(object sender, System.Windows.Input.MouseEventArgs e) { }

        private void OnMouseLeftButtonUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OnMouseLeftButtonUpCore(sender, e);
        }
        protected virtual void OnMouseLeftButtonUpCore(object sender, System.Windows.Input.MouseEventArgs e) {}

        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OnMouseMoveCore(sender, e);
        }
        protected virtual void OnMouseMoveCore(object sender, System.Windows.Input.MouseEventArgs e) { }

        void OnMouseHasLeftControl(object sender, EventArgs e)
        {
            OnMouseHasLeftControlCore(sender, e);
        }
        protected virtual void OnMouseHasLeftControlCore(object sender, EventArgs e){}

        #endregion

        #region Layout

        public void Measure(Size availableSize) 
        {
            LayoutManager.MeasureQueue.Remove(this);

            if (LayoutStorage.NeverMeasured)
            {
                LayoutStorage.OriginallySpecifiedSize = new Size(Width, Height);
            }

            if (Visibility == Visibility.Collapsed)
            {
                if (LayoutManager.CloseEnough(availableSize, LayoutStorage.PreviousConstraint))
                {
                    LayoutStorage.PreviousConstraint = availableSize;
                    LayoutStorage.MeasureDirty = true;
                }
                return;
            }

            if (!LayoutStorage.MeasureDirty && !LayoutStorage.NeverMeasured && LayoutManager.CloseEnough(availableSize, LayoutStorage.PreviousConstraint))
            {
                return;
            }

            LayoutStorage.NeverMeasured = false;
            InvalidateArrange();
            LayoutStorage.MeasureInProgress = true;

            Size previousSize = LayoutStorage.DesiredSize;

            Size desiredSize = MeasureCore(availableSize);

            LayoutStorage.MeasureInProgress = false;
            LayoutStorage.MeasureDirty = false;

            LayoutStorage.PreviousConstraint = availableSize;
            LayoutStorage.DesiredSize = desiredSize;

            if (!LayoutStorage.MeasureDuringArrange && !LayoutManager.CloseEnough(previousSize, desiredSize))
            {
                ILayout parent = Parent as ILayout;
                if (Parent != null && !parent.LayoutStorage.MeasureInProgress)
                {
                    if (!parent.LayoutStorage.MeasureDirty)
                    {
                        parent.InvalidateMeasure();
                    }
                }
            }
            //Debug.WriteLine("\t\tEnd Measure " + this.Name + " " + _desiredSize.Width + "," + _desiredSize.Height);
        }

        protected virtual Size MeasureCore(Size availableSize)
        {
            return Size.Empty;
        }

        public void Arrange(Rect finalRect)
        {
            //Debug.WriteLine("\t\tBegin Arrange " + this.Name + " " + finalRect);

            LayoutManager.ArrangeQueue.Remove(this);

            if (Visibility == Visibility.Collapsed)
            {
                LayoutStorage.FinalRect = finalRect;
            }

            if (LayoutStorage.MeasureDirty || LayoutStorage.NeverMeasured)
            {
                LayoutStorage.MeasureDuringArrange = true;
                if (LayoutStorage.NeverMeasured)
                    Measure(finalRect.Size);
                else
                    Measure(LayoutStorage.PreviousConstraint);
                LayoutStorage.MeasureDuringArrange = false;
            }

            if (!LayoutStorage.ArrangeDirty && !LayoutStorage.NeverArranged && LayoutManager.CloseEnough(finalRect, LayoutStorage.FinalRect))
                return;

            LayoutStorage.NeverArranged = false;
            LayoutStorage.ArrangeInProgress = true;

            ArrangeCore(finalRect);

            LayoutStorage.ArrangeInProgress = false;
            LayoutStorage.ArrangeDirty = false;
        }

        protected virtual void ArrangeCore(Rect finalRect)
        {
            //Debug.WriteLine("\t\tBegin ArrangeCore " + this.Name + " " + finalRect);
            SetValue(Canvas.LeftProperty, finalRect.Left);
            SetValue(Canvas.TopProperty, finalRect.Top);
            Width = finalRect.Width;
            Height = finalRect.Height;
            LayoutStorage.PreviousArrangeRect = finalRect;
        }

        public void InvalidateMeasure()
        {
            Invalidate(QueueSpecifier.Measure);
        }

        public void InvalidateArrange()
        {
            Invalidate(QueueSpecifier.Arrange);
        }

        private void Invalidate(QueueSpecifier queue)
        {
            if (!LayoutStorage.GetFlag(QueueFlags.GetDirty(queue)) && !(LayoutStorage.GetFlag(QueueFlags.GetInProgress(queue))))
            {
                if (!LayoutStorage.GetFlag(QueueFlags.GetNever(queue)))
                {
                    LayoutManager.GetQueue(queue).Enqueue(this);
                }
                LayoutStorage.SetFlag(QueueFlags.GetDirty(queue), true);
            }
        }

        public LayoutStorage LayoutStorage
        {
            get
            {
                if (_layoutStorage == null)
                    _layoutStorage = new LayoutStorage();
                return _layoutStorage;
            }
        }

        private LayoutStorage _layoutStorage;

        #endregion // Layout

        #region Helpers

        protected static Color GetColor(byte r, byte g, byte b)
        {
            return GetColor(byte.MaxValue, r, g, b);
        }

        protected static Color GetColor(byte a, byte r, byte g, byte b)
        {
            Color color = new Color();
            color.A = a;
            color.R = r;
            color.G = g;
            color.B = b;

            return color;
        }

        #endregion // Helpers

        #region IGrid Members

        public int GridColumn
        {
            get { return _gridColumn; }
            set { _gridColumn = value; }
        }

        public int GridRow
        {
            get { return _gridRow; }
            set { _gridRow = value; }
        }

        int _gridColumn = Grid.Default, _gridRow = Grid.Default;

        #endregion
    }
}
