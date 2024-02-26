/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: FloatingPin.xaml.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Browser;
using System.Windows.Media.Imaging;
using VESilverlight.Primary;

namespace VESilverlight.Secondary
{
    /// <summary>
    /// Implements the floating pin on the map when a user drags a pushpin
    /// </summary>
    [ScriptableType]
    public partial class FloatingPin : Canvas
    {
        /// <summary>
        /// Constructor - registers object as scriptable
        /// </summary>
		public FloatingPin()
		{
            this.InitializeComponent();

            HtmlPage.RegisterScriptableObject("FloatingPin", this);
		}

        /// <summary>
        /// Page load - gets a handle to the SilverLight controls
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void Page_Loaded(object o, EventArgs e)
        {
            // Required to initialize variables
            InitializeComponent();
        }

        /// <summary>
        /// Sets floating pin image according to attraction list type
        /// </summary>
        /// <param name="serialText"></param>
        [ScriptableMember]
        public void Initialize(string serialText)
        {
            Attraction attraction = Attraction.Deserialize(serialText);

            //Controller.GetInstance().GetAttractionById(attractionID,
            //    delegate(Attraction attraction)
            //    {
                    string url = "";

                    switch (attraction.List)
                    {
                        case Attraction.ListType.Concierge:
                            url = "/images/Btn_PushPin_Faded.png";
                            break;
                        case Attraction.ListType.User:
                            url = "/images/Btn_PushPin_Faded.png";
                            break;
                        case Attraction.ListType.Shared:
                            url = "/images/Btn_PushPin_saved.png";
                            break;
                        case Attraction.ListType.PushpinSearch:
                            url = "/images/Btn_PushPin_saved.png";
                            break;
                    }
                    image.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
                //});
        }
    }
}