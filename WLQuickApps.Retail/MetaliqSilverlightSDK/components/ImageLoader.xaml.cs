using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetaliqSilverlightSDK.components
{

    public class ImageLoader : UIControlBase
    {
        //public delegate void CompleteHandler(object sender);
        //public event CompleteHandler complete;
        protected Image img;
        public string imageUri;

        public ImageLoader()
        {
            img = (Image)root.FindName("img");
        }

        protected override void setup()
        {
            init("MetaliqSilverlightSDK.components.ImageLoader.xaml");
        }

        public void Load(string p_uri)
        {
            // Temporary so we can load images locally
            imageUri = p_uri;
            Uri uri = new Uri(p_uri, UriKind.RelativeOrAbsolute);
            img.Width = Width;
            img.Height = Height;
            img.Stretch = Stretch.Fill;
            img.Source = new BitmapImage(uri);
        }

        // Need to override Width and Height to resize Image
        new public double Width
        {
            set
            {
                base.Width = value;
                img.Width = value;
            }
            get
            {
                return base.Width;
            }
        }

        new public double Height
        {
            set
            {
                base.Height = value;
                img.Height = value;
            }
            get
            {
                return base.Height;
            }
        }
    }
}
