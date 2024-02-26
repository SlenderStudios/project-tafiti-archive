//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace VESilverlight
{
    public interface ILayout
    {
        void Measure(Size availableSize);
        void Arrange(Rect finalRect);

        void InvalidateMeasure();
        void InvalidateArrange();

        LayoutStorage LayoutStorage
        {
            get;
        }
    }

    public class LayoutStorage
    {
        public Size PreviousConstraint
        {
            get { return _previousAvailableSize; }
            set { _previousAvailableSize = value; }
        }

        public Rect PreviousArrangeRect
        {
            get { return _previousArrangeRect; }
            set { _previousArrangeRect = value; }
        }

        public Rect FinalRect
        {
            get { return _finalRect; }
            set { _finalRect = value; }
        }

        public Size DesiredSize
        {
            get { return _desiredSize; }
            set { _desiredSize = value; }
        }

        public Size OriginallySpecifiedSize
        {
            get { return _originallySpecifiedSize; }
            set { _originallySpecifiedSize = value; }
        }

        public bool MeasureDirty { get { return GetFlag(LayoutFlags.MeasureDirty); } set { SetFlag(LayoutFlags.MeasureDirty, value); } }
        public bool ArrangeDirty { get { return GetFlag(LayoutFlags.ArrangeDirty); } set { SetFlag(LayoutFlags.ArrangeDirty, value); } }
        public bool MeasureInProgress { get { return GetFlag(LayoutFlags.MeasureInProgress); } set { SetFlag(LayoutFlags.MeasureInProgress, value); } }
        public bool ArrangeInProgress { get { return GetFlag(LayoutFlags.ArrangeInProgress); } set { SetFlag(LayoutFlags.ArrangeInProgress, value); } }
        public bool NeverMeasured { get { return GetFlag(LayoutFlags.NeverMeasured); } set { SetFlag(LayoutFlags.NeverMeasured, value); } }
        public bool NeverArranged { get { return GetFlag(LayoutFlags.NeverArranged); } set { SetFlag(LayoutFlags.NeverArranged, value); } }
        public bool MeasureDuringArrange { get { return GetFlag(LayoutFlags.MeasureDuringArrange); } set { SetFlag(LayoutFlags.MeasureDuringArrange, value); } }
        public bool InMeasureQueue { get { return GetFlag(LayoutFlags.InMeasureQueue); } set { SetFlag(LayoutFlags.InMeasureQueue, value); } }
        public bool InArrangeQueue { get { return GetFlag(LayoutFlags.InArrangeQueue); } set { SetFlag(LayoutFlags.InArrangeQueue, value); } }

        public bool WidthWasSpecified { get { return _originallySpecifiedSize.Width > 0; } }
        public bool HeightWasSpecified { get { return _originallySpecifiedSize.Height > 0; } }

        internal bool GetFlag(LayoutFlags flag)
        {
            return (_flags & flag) == flag;
        }

        internal void SetFlag(LayoutFlags flag, bool value)
        {
            if (value)
                _flags |= flag;
            else
                _flags &= ~flag;
        }

        private LayoutFlags _flags = LayoutFlags.NeverArranged | LayoutFlags.NeverMeasured;

        private Size _previousAvailableSize;
        private Size _desiredSize;
        private Size _originallySpecifiedSize;
        private Rect _previousArrangeRect;
        private Rect _finalRect;
    }
}
