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
    public class MixInfo
    {
        public readonly DateTime DateCreated;
        public readonly int Views;
        public readonly int ID;

        public MixInfo(int ID, DateTime DateCreated, int Views)
        {
            this.ID = ID;
            this.DateCreated = DateCreated;
            this.Views = Views;
        }
        public MixInfo(int ID, DateTime DateCreated, int Views, string Title, Uri Uri, DateTime DateModified, int Rating, Visibility Visible, DateTime LastPlayed)
        {
            this.ID = ID;
            this.DateCreated = DateCreated;
            this.Views = Views;
            this.Title = Title;
            this.Uri = Uri;
            this.DateModified = DateModified;
            this.Rating = Rating;
            this.Visible = Visible;
            this.LastPlayed = LastPlayed;
        }
        public string Title { get; set; }
        public Uri Uri { get; set; }
        public DateTime DateModified { get; set; }
        public TimeSpan Duration { get; set; }
        public int Rating { get; set; }
        public Visibility Visible { get; set; }
        public DateTime LastPlayed { get; set; }
    }
}
