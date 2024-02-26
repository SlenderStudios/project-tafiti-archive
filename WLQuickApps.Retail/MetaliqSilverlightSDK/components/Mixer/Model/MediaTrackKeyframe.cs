using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MetaliqSilverlightSDK.Controls.Mixer.Model.Media;

namespace MetaliqSilverlightSDK.Controls.Mixer.Model
{
    public class MediaTrackKeyframe
    {
        public TimeSpan Position { get; set; }
        public MediaSegment Segment;
    }
}
