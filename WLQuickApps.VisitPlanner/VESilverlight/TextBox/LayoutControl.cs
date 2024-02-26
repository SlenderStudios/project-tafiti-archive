//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace VESilverlight
{
    public abstract class LayoutControl : ControlBase, ILayout, IGrid
    {
        public LayoutControl()
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
            FocusManager.FocusedElement = this;
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
        }

        protected virtual Size MeasureCore(Size availableSize)
        {
            return Size.Empty;
        }

        public void Arrange(Rect finalRect)
        {
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

        public void SetFocus()
        {
            FocusManager.FocusedElement = this;
        }

        public virtual void OnGotFocus() { }
        public virtual void OnLostFocus() { }

        public virtual void OnKeyDown(object sender, KeyboardEventArgs e) {}
        public virtual void OnKeyUp(object sender, KeyboardEventArgs e) {}

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

        public string Margin
        {
            get { return _margin.ToString(); }
            set { _margin = new Thickness(value); }
        }

        public Thickness _Margin
        {
            get { return _margin; }
            set { _margin = value; }
        }

        private Thickness _margin = new Thickness();
    }

    public class Thickness
    {
        public Thickness() { }

        public Thickness(string value)
        {
            string[] values = value.Split(',');

            if (values.Length == 1)
            {
                Left = Bottom = Top = Right = Int32.Parse(values[0]);
            }
            else if (values.Length == 2)
            {
                Left = Int32.Parse(values[0]);
                Top = Int32.Parse(values[1]);
                Right = Bottom = 0;
            }
            else if (values.Length == 4)
            {
                Left = Int32.Parse(values[0]);
                Top = Int32.Parse(values[1]);
                Right = Int32.Parse(values[2]);
                Bottom = Int32.Parse(values[3]);
            }
            else
                throw new ArgumentException("Must have 1, 2 or 4 comma-separated integer values with no whitespace");
        }

        public override string ToString()
        {
            if ((Top == Left) && (Top == Bottom) && (Left == Right))
            {
                return Top.ToString();
            }
            else if ((Bottom == Right) && (Bottom == 0))
            {
                return Left.ToString() + "," + Top.ToString();
            }
            else
            {
                return Left.ToString() + "," + Top.ToString() + "," + Right.ToString() + "," + Bottom.ToString();
            }
        }

        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }

        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public int Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        public int Right
        {
            get { return _right; }
            set { _right = value; }
        }

        private int _top, _left, _bottom, _right;
    }
}
