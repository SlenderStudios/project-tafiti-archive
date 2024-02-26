//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media;

namespace VESilverlight
{
    public static class LayoutManager
    {
        static LayoutManager()
        {
            BrowserHost.Resize += new EventHandler(BrowserHost_Resize);
            BrowserHost.FullScreenChange += new EventHandler(BrowserHost_FullScreenChange);
        }

        static void BrowserHost_Resize(object sender, EventArgs e)
        {
            bool rootWasNull = _root == null;
            _root = sender as ILayout;
            if (_root != null && BrowserHost.ActualWidth > 0 && BrowserHost.ActualHeight > 0)
            {
                if (rootWasNull)
                    ((UIElement)_root).Visibility = Visibility.Visible;

                LayoutFromRoot();
            }
        }

        static void BrowserHost_FullScreenChange(object sender, EventArgs e)
        {
            LayoutFromRoot();
        }

        private static void LayoutFromRoot()
        {
            Reset();
            _measure.Enqueue(_root);
            _arrange.Enqueue(_root);
            ((UIElement)_root).Visibility = Visibility.Visible;
            Update();
        }

        public static void Initialize()
        {
        }

        public static LayoutQueue MeasureQueue
        {
            get { return _measure; }
        }

        public static LayoutQueue ArrangeQueue
        {
            get { return _arrange; }
        }

        internal static LayoutQueue GetQueue(QueueSpecifier queue)
        {
            return queue == QueueSpecifier.Measure ? MeasureQueue : ArrangeQueue;
        }

        public static void Update()
        {
            uint iterationsRemaining = 42;

            while (IsDirty && iterationsRemaining-- > 0)
            {
                //Debug.WriteLine("Layout iteration " + (42 - iterationsRemaining).ToString());
                while (!_measure.IsEmpty)
                {
                    ILayout item = _measure.Dequeue();
                    //Debug.WriteLine("\tMeasuring " + item.Name);
                    Measure(item);
                }

                while (!_arrange.IsEmpty && _measure.IsEmpty)
                {
                    ILayout item = _arrange.Dequeue();
                    //Debug.WriteLine("\tArranging " + item.Name);
                    Arrange(item);
                }
            }

            if (iterationsRemaining == 0)
            {
                Reset();
            }

            if (_root != null)
                DumpTree((FrameworkElement)_root, 0);
        }

        private static void DumpTree(FrameworkElement node, int depth)
        {
            string output = string.Empty;
            for (int tab = 0; tab < depth; ++tab)
                output = "\t" + output;
            output += " " + node.GetType().Name + "(" + node.Name + ") @ ";
            output += node.GetValue(Canvas.LeftProperty) + "," + node.GetValue(Canvas.TopProperty);
            output += " " + node.Width + "," + node.Height;

            Debug.WriteLine(output);

            Panel panel = node as Panel;
            if (panel != null)
            {
                foreach (FrameworkElement child in panel.Children)
                    DumpTree(child, depth + 1);
            }
        }

        public static bool CloseEnough(Rect r0, Rect r1)
        {
            return 
                CloseEnough(r0.X, r1.X) && 
                CloseEnough(r0.Y, r1.Y) && 
                CloseEnough(r0.Width, r1.Width) && 
                CloseEnough(r0.Height, r1.Height);
        }

        public static bool CloseEnough(Size s0, Size s1)
        {
            return CloseEnough(s0.Width, s1.Width) && CloseEnough(s0.Height, s1.Height);
        }

        public static bool CloseEnough(double d0, double d1)
        {
            // This function is only "good enough".
            const double Delta = 0.0001;
            return Math.Abs(d0 - d1) < Delta;
        }

        private static void Measure(ILayout item)
        {
            Size available = ((FrameworkElement)item).Parent == null ? new Size(BrowserHost.ActualWidth, BrowserHost.ActualHeight) : item.LayoutStorage.PreviousConstraint;
            
            item.Measure(available);
        }

        private static void Arrange(ILayout item)
        {
            Rect arrangeRect = item.LayoutStorage.PreviousArrangeRect;

            if (((FrameworkElement)item).Parent == null)
            {
                arrangeRect.X = arrangeRect.Y = 0;
                //if (double.IsPositiveInfinity(item.LayoutStorage.PreviousConstraint.Width))
                    arrangeRect.Width = item.LayoutStorage.DesiredSize.Width;

                //if (double.IsPositiveInfinity(item.LayoutStorage.PreviousConstraint.Height))
                    arrangeRect.Height = item.LayoutStorage.DesiredSize.Height;
            }

            item.Arrange(arrangeRect);
        }

        private static bool IsDirty
        {
            get { return !_measure.IsEmpty || !_arrange.IsEmpty; }
        }

        private static void Reset()
        {
            _measure.Clear();
            _arrange.Clear();
        }

        private static ILayout _root = null;

        private static LayoutQueue _measure = new LayoutQueue(LayoutFlags.InMeasureQueue);
        private static LayoutQueue _arrange = new LayoutQueue(LayoutFlags.InArrangeQueue);
    }
}
