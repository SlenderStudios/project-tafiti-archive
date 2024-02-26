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

namespace MetaliqSilverlightSDK.Controls.Mixer.Model.Media
{
    public class VideoSegment : MediaSegment
    {
        public Dictionary<string, string> Metadata = new Dictionary<string,string>();
        public VideoSegment(int ID)
            : base(ID)
        {
        }
        public VideoSegment(int ID, string Title, Uri Uri, TimeSpan Duration, TimeSpan StartTime, TimeSpan EndTime, Uri Thumbnail, MediaSegmentType Type, Dictionary<string, string> Metadata)
            : base(ID, Title, Uri, Duration, StartTime, EndTime, Thumbnail, Type)
        {
            this.Metadata = Metadata;
        }
        public VideoSegment(int ID, string Title, Uri Uri, TimeSpan Duration, TimeSpan StartTime, TimeSpan EndTime, Uri Thumbnail, MediaSegmentType Type)
            : base(ID, Title, Uri, Duration, StartTime, EndTime, Thumbnail, Type)
        {
        }
    }
}
