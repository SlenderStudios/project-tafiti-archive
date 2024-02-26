/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: Page.xaml.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Windows.Media.Imaging;

namespace VESilverlight.Primary
{
    /// <summary>
    /// The main Sliverlight Host container
    /// </summary>
    public partial class Page : UserControl
    {
        private static Page instance;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public Page()
        {
            this.InitializeComponent();

            instance = this;

            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        /// <summary>
        /// Initializes map panel
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void Page_Loaded(object o, RoutedEventArgs e)
        {
            // Required to initialize variables
            InitializeComponent();
            Application.Current.Host.Content.Resized += new EventHandler(BrowserHost_Resize);
        }

        /// <summary>
        /// On browser window resize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowserHost_Resize(object sender, EventArgs e)
        {
            this.Height = Application.Current.Host.Content.ActualHeight;
            this.Width = Application.Current.Host.Content.ActualWidth;
            if (this.Height > 0)  mapPanel.Resize(Width, Height);
        }

        public static Canvas RootCanvas {
            get
            {
                return instance.LayoutRoot;
            }
        }   
    }
}
