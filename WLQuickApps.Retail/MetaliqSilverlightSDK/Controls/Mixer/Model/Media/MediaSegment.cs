using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetaliqSilverlightSDK.Controls.Mixer.Model.Media
{
    public enum MediaSegmentType
    {
        Audio, Video, Image
    }
    public class MediaSegment
    {
        public string Title { get; set; }
        public Uri Uri { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Uri Thumbnail { get; set; }
        public MediaSegmentType Type { get; set; }
        public readonly int ID;

        public MediaSegment(int ID)
        {
            this.ID = ID;
        }
        public MediaSegment(int ID, string Title, Uri Uri, TimeSpan Duration, TimeSpan StartTime, TimeSpan EndTime, Uri Thumbnail, MediaSegmentType Type)
        {
            this.ID = ID;
            this.Title = Title;
            this.Uri = Uri;
            this.Duration = Duration;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Thumbnail = Thumbnail;
            this.Type = Type;
        }
    }
}
