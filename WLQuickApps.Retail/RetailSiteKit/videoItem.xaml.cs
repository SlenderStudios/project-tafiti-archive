using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using MetaliqSilverlightSDK;
using RetailXmlApi.model;


namespace RetailSiteKit
{
    public partial class videoItem : UserControl
    {
        public videoItem()
        {
            InitializeComponent();
        }
        public event EventHandler VideoPlayStart;
        public event EventHandler VideoPlayStop;
        public event EventHandler IndexChanged;

        protected static Point StartingPoint = new Point(133, 29);
        protected static double CELLSPACING = 6;
        protected static double COLUMNWIDTH = 197.5;
        protected static double ROWHEIGHT = 121;

        protected bool _Selected = false;
        protected bool _Playing = false;
        protected int _Column;
        protected int _Row;
        protected bool _isThumbnailVisible;
        public LocaleString descriptions;
        public int ItemIndex;
        public int _ItemIndex;
        public Uri Source;
        protected Uri myImageSource;
        protected Product _product;
        FrameworkElement root;
        protected MediaElement VideoDisplay;

        public videoItem(int itemIndex, Product theProduct)
        {
            InitializeComponent();

            this.product = theProduct;
            VideoDisplayHitTarget.Opacity = 0;
            darkBackground.Visibility = Visibility.Visible;

            VideoItemCanvas.MouseEnter += new MouseEventHandler(videoItem_MouseEnter);
            VideoItemCanvas.MouseLeave += new MouseEventHandler(videoItem_MouseLeave);
            VideoItemCanvas.MouseLeftButtonUp += new MouseButtonEventHandler(videoItem_MouseLeftButtonUp);
            _ItemIndex = itemIndex;
            //hide thumb by default
            isThumbnailVisible = true;

        }
        public Product product
        {
            set
            {
                _product = value;
                ItemIndex = Int32.Parse(_product.Id);
                description.Text = _product.Description.ToString();

                if (description.Text.Length > 35)
                {
                    description.Text = description.Text.Substring(0, 35) + "...";
                }
                price.Text = _product.Price.ToString();
                myImageSource = new Uri(_product.VideoThumb.Url.ToString(), UriKind.RelativeOrAbsolute);
                Source = new Uri(_product.VideoSmall.Url.ToString(), UriKind.RelativeOrAbsolute);
                thumbImage.Source = new BitmapImage(myImageSource);
                descriptions = _product.Description;
                price.Text = _product.Price.ToString();
            }
        }
        public void Play()
        {
            VideoDisplay.Visibility = Visibility.Visible;
            VideoDisplay.Source = Source;
            VideoDisplay.Play();
            VideoPlayStart(this, new EventArgs());
        }
        public void Stop()
        {
            if (!_Selected)
            {
                VideoDisplay.Pause();
                VideoDisplay.Visibility = Visibility.Collapsed;
                VideoPlayStop(this, new EventArgs());
            }
        }
        public bool Selected
        {

            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
                if (_Selected)
                {
                    redBorderSmall_In.Begin();
                    IndexChanged(this, new EventArgs());
                }
                else
                {
                    redBorderSmall_Out.Begin();
                    itemMouseOver_Out.Begin();
                    LoaderCanvas.Visibility = Visibility.Collapsed;
                    LoaderCanvas.Opacity = 0;
                    VideoItemCanvas.Children.Remove(VideoDisplay);
                }
            }

        }

        public bool isThumbnailVisible
        {
            get
            {
                return _isThumbnailVisible;
            }
            set
            {
                _isThumbnailVisible = value;
                if (_isThumbnailVisible)
                {
                    selectedSmall.Visibility = Visibility.Visible;
                    thumbImage.Visibility = Visibility.Visible;
                    darkForeground.Visibility = Visibility.Collapsed;
                    darkBackground.Opacity = 1;
                }
                else
                {
                    selectedSmall.Visibility = Visibility.Collapsed;
                    thumbImage.Visibility = Visibility.Collapsed;
                    darkForeground.Visibility = Visibility.Visible;
                    darkBackground.Opacity = 0;
                }
            }
        }


        void videoItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Selected = true;
        }

        void videoItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_Selected)
            {
                itemMouseOver_Out.Begin();
                Stop();
                darkForeground.Opacity = 0;
                _Playing = false;
                VideoItemCanvas.Children.Remove(VideoDisplay);
                LoaderCanvas.Visibility = Visibility.Collapsed;
                LoaderCanvas.Opacity = 0;
            }
        }

        void videoItem_MouseEnter(object sender, MouseEventArgs e)
        {
            VideoDisplay = new MediaElement();
            VideoDisplay.Source = Source;
            VideoDisplay.Height = 119.5;
            VideoDisplay.Width = 196;
            VideoItemCanvas.Children.Add(VideoDisplay);
            //VideoDisplay.
            LoaderFadeIn.Begin();
            LoadingAnimation.Begin();
            itemMouseOver_In.Begin();
            LoaderCanvas.Visibility = Visibility.Visible;

            Play();
            _Playing = true;
            VideoDisplay.MediaOpened += new RoutedEventHandler(VideoDisplay_MediaOpened);
            VideoDisplay.MediaEnded += new RoutedEventHandler(VideoDisplay_MediaEnded);
        }

        void VideoDisplay_MediaOpened(object sender, RoutedEventArgs e)
        {
            LoadingAnimation.Seek(new TimeSpan(0, 0, 0, 0, 0));
            foregroundFade.Begin();
            LoadingAnimation.Stop();
            LoaderCanvas.Visibility = Visibility.Collapsed;
        }
        public int Row
        {
            set
            {
                if (value > 8 || value < 1)
                {
                    System.Console.WriteLine(value.ToString());
                    throw new Exception("Invalid row value assigned to videoItem");
                }
                else
                {
                    _Row = value;
                    Y = StartingPoint.Y + ((_Row - 1) * (ROWHEIGHT + CELLSPACING));
                }
            }
            get
            {
                return _Row;
            }
        }
        public int Column
        {
            set
            {
                if (value > 4 || value < 1)
                {
                    throw new Exception("Invalid column value assigned to videoItem");
                }
                else
                {
                    _Column = value;
                    X = StartingPoint.X + ((_Column - 1) * (COLUMNWIDTH + CELLSPACING));
                }
            }
            get
            {
                return _Column;
            }
        }
        void VideoDisplay_MediaEnded(object sender, EventArgs e)
        {
            if (_Playing)
            {
                MediaElement video = (MediaElement)sender; video.Position = new TimeSpan(0);
                video.Play();
            }
        }
        void onHomePage(object sender, EventArgs e)
        {
            selectedSmall.Visibility = Visibility.Collapsed;
            darkForeground.Visibility = Visibility.Visible;

        }
        public double X
        {
            set
            {
                this.SetValue(Canvas.LeftProperty, value);
            }
            get
            {
                return (double)this.GetValue(Canvas.LeftProperty);
            }
        }
        public double Y
        {
            set
            {
                this.SetValue(Canvas.TopProperty, value);
            }
            get
            {
                return (double)this.GetValue(Canvas.TopProperty);
            }
        }
    }
}
