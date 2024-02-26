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
using System.Windows.Browser;
using System.Diagnostics;

namespace VESilverlight
{
    public partial class App : Application
    {
        public string Mode { get { return this._mode; } }
        private string _mode;

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this._mode = e.InitParams["mode"];
            switch (this.Mode)
            {
                case "main":
                    this.RootVisual = new Primary.Page();
                    break;
                case "tour":
                    this.RootVisual = new Secondary.TourControl();
                    break;
                case "touritem":
                    this.RootVisual = new Secondary.TourItem();
                    break;
                case "popup":
                    this.RootVisual = new Secondary.PopupItem();
                    break;
                case "floatingpin":
                    this.RootVisual = new Secondary.FloatingPin();
                    break;
                case "placelist":
                    this.RootVisual = new Secondary.PlaceListHover();
                    break;
                case "dropbox":
                    this.RootVisual = new Secondary.AttractionDropBox();
                    break;
                case "directionsbutton":
                    this.RootVisual = new Secondary.DirectionsButton();
                    break;
                case "directionsdialog":
                    this.RootVisual = new Secondary.DirectionsDialog();
                    break;                    
            }
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            Debug.Assert(false);
        }
    }
}
