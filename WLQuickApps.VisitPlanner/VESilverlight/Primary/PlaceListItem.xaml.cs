/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: PlaceListItem.xaml.cs
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
using VESilverlight;
using System.Windows.Media.Imaging;

namespace VESilverlight.Primary
{
    /// <summary>
    /// Implements a single attraction list item
    /// </summary>
    public partial class PlaceListItem : UserControl
    {
        protected Attraction attraction;

        /// <summary>
        /// Constructor. Get a handle to Silverlight controls.  Set events
        /// </summary>
		public PlaceListItem(Attraction attraction)
		{
            this.InitializeComponent();

            this.attraction = attraction;
            titleText.Text = attraction.Title;

            //Displays the category icon
            switch (attraction.Category)
            {
                case Attraction.Categories.Food:
                    typeIcon.Source = new BitmapImage(new Uri("/images/food.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Movie:
                    typeIcon.Source = new BitmapImage(new Uri("/images/movie.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Music:
                    typeIcon.Source = new BitmapImage(new Uri("/images/music.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Art:
                    typeIcon.Source = new BitmapImage(new Uri("/images/other.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Search:
                    typeIcon.Source = new BitmapImage(new Uri("/images/Btn_PushPin_searchSaved.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Accomodation:
                    typeIcon.Source = new BitmapImage(new Uri("/images/hotel_sym_yellow.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Custom:
                    typeIcon.Source = new BitmapImage(new Uri("/images/Btn_PushPin_searchSaved.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Misc:
                    typeIcon.Source = new BitmapImage(new Uri("/images/Btn_PushPin_search.png", UriKind.Relative));
                    break;
            }
		}
    }
}