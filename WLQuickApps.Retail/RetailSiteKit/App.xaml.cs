using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RetailSiteKit
{
    public partial class App : Application
    {
        public string _VideoPath;
        public string _StaticAssetURL = string.Empty;
        public string _Appid = string.Empty;
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Load the main control
            _VideoPath = e.InitParams["video"].ToString();
            _Appid = e.InitParams["Appid"].ToString();
            _StaticAssetURL = e.InitParams["StaticAssetsURL"].ToString();
            this.RootVisual = new Page(_StaticAssetURL, _VideoPath, _Appid);
            
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {

        }
    }
}
