using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace RetailSiteKitAction
{
    public partial class Page : UserControl
    {
        string AssetsURL = string.Empty;
        bool moving = false;
        bool Playing = true;
        Storyboard moveLeft = new Storyboard();
        Storyboard moveRight = new Storyboard();

        public Page(string assetsURL)
        {
            InitializeComponent();
            AssetsURL=assetsURL;
            DoubleAnimation myDoubleRight = new DoubleAnimation();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.2));
            myDoubleRight.Duration = duration;
            moveRight.Duration = duration;
            moveRight.Children.Add(myDoubleRight);
            Storyboard.SetTarget(myDoubleRight, Thumbs);
            Storyboard.SetTargetProperty(myDoubleRight, new PropertyPath("(Canvas.Left)"));
            myDoubleRight.By = -440;

            DoubleAnimation myDoubleLeft = new DoubleAnimation();
            myDoubleLeft.Duration = duration;
            moveLeft.Duration = duration;
            moveLeft.Children.Add(myDoubleLeft);
            Storyboard.SetTarget(myDoubleLeft, Thumbs);
            Storyboard.SetTargetProperty(myDoubleLeft, new PropertyPath("(Canvas.Left)"));
            myDoubleLeft.By = 440;

            // LayoutRoot.Resources.Add(moveLeft);
            // LayoutRoot.Resources.Add(moveRight);
            LayoutRoot.Resources.Add("Left", moveLeft);
            LayoutRoot.Resources.Add("Right", moveRight);

            PlayVideo("Cortefiel_Men_1");

            MediaControl.MouseLeave += new MouseEventHandler(MediaControl_MouseLeave);
            MediaControl.MouseEnter += new MouseEventHandler(MediaControl_MouseEnter);
            MediaControl.MouseLeftButtonDown += new MouseButtonEventHandler(MediaControl_MouseLeftButtonDown);
            MediaControl.MouseLeftButtonUp += new MouseButtonEventHandler(MediaControl_MouseLeftButtonUp);
            ScrollLeft.MouseLeftButtonDown += new MouseButtonEventHandler(ScrollLeft_MouseLeftButtonDown);
            ScrollRight.MouseLeftButtonDown += new MouseButtonEventHandler(ScrollRight_MouseLeftButtonDown);
            ScrollRight.MouseEnter += new MouseEventHandler(ScrollRight_MouseEnter);
            ScrollRight.MouseLeave += new MouseEventHandler(ScrollRight_MouseLeave);
            ScrollLeft.MouseEnter += new MouseEventHandler(ScrollLeft_MouseEnter);
            ScrollLeft.MouseLeave += new MouseEventHandler(ScrollLeft_MouseLeave);
            Movie.MediaEnded += new RoutedEventHandler(Movie_MediaEnded);
            moveLeft.Completed += new EventHandler(move_Completed);
            moveRight.Completed += new EventHandler(move_Completed);
            move_Completed(this, new EventArgs());

            Share.MouseLeftButtonDown += new MouseButtonEventHandler(Share_MouseLeftButtonDown);
        }

        void Share_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                HtmlPage.Window.Invoke("shareVideo", CurrentVideoName);
            }
            catch (Exception ex)
            {
                string foo = ex.Message;
            }
        }

        void ScrollLeft_MouseLeave(object sender, MouseEventArgs e)
        {
            ScrollLeftOff.Begin();
        }

        void ScrollLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((double)Thumbs.GetValue(Canvas.LeftProperty) < 0)
            {
                ScrollLeftOn.Begin();
            }
        }

        void ScrollRight_MouseLeave(object sender, MouseEventArgs e)
        {

            ScrollRightOff.Begin();
        }

        void ScrollRight_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((double)Thumbs.GetValue(Canvas.LeftProperty) > -800)
            {
                ScrollRightOn.Begin();
            }
        }

        void MediaControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PlayDown.Begin();
        }
        
        void MediaControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Playing)
            {
                Movie.Pause();
                Playing = false;
                PlaySymbol.Visibility = Visibility.Visible;
                PauseSymbol.Visibility = Visibility.Collapsed;
            }
            else
            {
                Movie.Play();
                Playing = true;
                PlaySymbol.Visibility = Visibility.Collapsed;
                PauseSymbol.Visibility = Visibility.Visible;
                
            }
        }

        void MediaControl_MouseLeave(object sender, MouseEventArgs e)
        {
            PlayOff.Begin();
        }

        void MediaControl_MouseEnter(object sender, MouseEventArgs e)
        {
            PlayOn.Begin();
        }

        void move_Completed(object sender, EventArgs e)
        {
            moving = false;
            if ((double)Thumbs.GetValue(Canvas.LeftProperty) < 0)
            {
                ScrollLeft.Opacity = 1;
                ScrollLeft.IsHitTestVisible = true;
            }
            else
            {
                ScrollLeft.Opacity = .5;
                ScrollLeftOff.Begin();
                ScrollLeft.IsHitTestVisible = false;
            }
            if ((double)Thumbs.GetValue(Canvas.LeftProperty) < -800)
            {
                ScrollRight.Opacity = .5;
                ScrollRightOff.Begin();
                ScrollRight.IsHitTestVisible = false;
            }
            else
            {
                ScrollRight.Opacity = 1;
                ScrollRight.IsHitTestVisible = true;
            }
        }
        [ScriptableMember()]
        public void PlayVideo(string name)
        {
            CurrentVideoName = name;
            //Uri updatedSource = MetaliqSilverlightSDK.net.NetUtil.ToAbsoluteUri("videos/" + CurrentVideoName + ".wmv");
            Uri updatedSource = new Uri(AssetsURL + "/" + CurrentVideoName + ".wmv", UriKind.Absolute);
            Movie.Source = updatedSource;
        }
        protected string CurrentVideoName;
        void ThumbDown(object sender, EventArgs e)
        {
            Image image = sender as Image;
            BitmapImage bmi = (BitmapImage)image.Source;
            //newImage.Source = new BitmapImage(bmi.UriSource);
            CurrentVideoName = bmi.UriSource.ToString();
            int trimAt = CurrentVideoName.LastIndexOf("_");
            CurrentVideoName = CurrentVideoName.Substring(0, trimAt);
            CurrentVideoName = CurrentVideoName.Substring(7);
            PlayVideo(CurrentVideoName);
            LoaderCanvas.Visibility = Visibility.Visible;
            LoaderFadeIn.Begin();
            LoadingAnimation.Begin();
            Movie.MediaOpened += new RoutedEventHandler(Movie_MediaOpened);
        }

        void Movie_MediaOpened(object sender, RoutedEventArgs e)
        {
            LoaderCanvas.Visibility = Visibility.Collapsed;
            LoadingAnimation.Seek(new TimeSpan(0, 0, 0, 0, 0));
            LoadingAnimation.Stop();
        }

        void ScrollRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!moving)
            {
                if ((double)Thumbs.GetValue(Canvas.LeftProperty) > -800)
                {
                    //ScrollRightAnim.Begin();
                    moveRight.Begin();
                    moving = true;
                }
            }
        }

        void ScrollLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!moving)
            {
                if ((double)Thumbs.GetValue(Canvas.LeftProperty) < 0)
                {
                    //ScrollLeftAnim.Begin();
                    moveLeft.Begin();
                    moving = true;
                }
            }
        }

        void Movie_MediaEnded(object sender, RoutedEventArgs e)
        {
            Movie.Position = new TimeSpan(0);

            Movie.Play();
        }

        
    }
}
