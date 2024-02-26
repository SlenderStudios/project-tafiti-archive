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
    public class AudioSegment : MediaSegment
    {
        public AudioSegment(int ID, float Volume):base(ID)
        {
            this.Volume = Volume;
        }
        public AudioSegment(int ID, string Title, Uri Uri, TimeSpan Duration, TimeSpan StartTime, TimeSpan EndTime, Uri Thumbnail, MediaSegmentType Type, float Volume)
            : base(ID, Title, Uri, Duration, StartTime, EndTime, Thumbnail, Type)
        {
            this.Volume = Volume;
        }
        public AudioSegment(int ID, string Title, Uri Uri, TimeSpan Duration, TimeSpan StartTime, TimeSpan EndTime, Uri Thumbnail, MediaSegmentType Type)
            : base(ID, Title, Uri, Duration, StartTime, EndTime, Thumbnail, Type)
        {
        }
        public float Volume { get; set; }
    }
}
