using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetaliqSilverlightSDK.Controls.Mixer.Model
{
    public class VideoComposition : MixInfo
    {
        public MediaTrack AudioTrack{get;set;}
        public MediaTrack VideoTrack{get;set;}
        public VideoComposition(int ID, DateTime DateCreated, int Views, MediaTrack AudioTrack, MediaTrack VideoTrack)
            :base(ID, DateCreated, Views)
        {
            this.AudioTrack = AudioTrack;
            this.VideoTrack = VideoTrack;
        }
        public VideoComposition(int ID, DateTime DateCreated, int Views, string Title, Uri Uri, DateTime DateModified, int Rating, Visibility Visible, DateTime LastPlayed, MediaTrack AudioTrack, MediaTrack VideoTrack)
            : base(ID, DateCreated, Views, Title, Uri, DateModified, Rating, Visible, LastPlayed)
        {
            this.AudioTrack = AudioTrack;
            this.VideoTrack = VideoTrack;
        }
    }
}
