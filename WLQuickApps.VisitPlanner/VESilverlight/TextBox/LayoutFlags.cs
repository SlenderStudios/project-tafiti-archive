//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace VESilverlight
{
    internal static class QueueFlags
    {
        internal static LayoutFlags GetDirty(QueueSpecifier queue)
        {
            return queue == QueueSpecifier.Measure ? LayoutFlags.MeasureDirty : LayoutFlags.ArrangeDirty;
        }

        internal static LayoutFlags GetInProgress(QueueSpecifier queue)
        {
            return queue == QueueSpecifier.Measure ? LayoutFlags.MeasureInProgress : LayoutFlags.ArrangeInProgress;
        }

        internal static LayoutFlags GetNever(QueueSpecifier queue)
        {
            return queue == QueueSpecifier.Measure ? LayoutFlags.NeverMeasured : LayoutFlags.NeverArranged;
        }

        internal static LayoutFlags GetInQueue(QueueSpecifier queue)
        {
            return queue == QueueSpecifier.Measure ? LayoutFlags.InMeasureQueue : LayoutFlags.InArrangeQueue;
        }
    }


    [Flags]
    internal enum LayoutFlags
    {
        MeasureDirty = 0x1,
        ArrangeDirty = 0x2,
        MeasureInProgress = 0x4,
        ArrangeInProgress = 0x8,
        NeverMeasured = 0x10,
        NeverArranged = 0x20,
        MeasureDuringArrange = 0x40,
        InMeasureQueue = 0x80,
        InArrangeQueue = 0x100,
    }

    internal enum QueueSpecifier
    {
        Measure,
        Arrange
    }
}
