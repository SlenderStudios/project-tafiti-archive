/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: PrimaryPlaceListItem.cs
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
using System.Windows.Media.Imaging;

namespace VESilverlight.Primary
{
    /// <summary>
    /// Represent a single list item found in the various tool bars and panels
    /// </summary>
    public partial class PrimaryPlaceListItem : UserControl
    {
        protected Attraction attraction;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attraction">Attraction object</param>
        public PrimaryPlaceListItem(Attraction attraction)
        {
            this.InitializeComponent();

            this.Loaded += new RoutedEventHandler(PrimaryPlaceListItem_Loaded);

            this.attraction = attraction;
        }

        void PrimaryPlaceListItem_Loaded(object sender, RoutedEventArgs e)
        {
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


            this.MouseLeftButtonDown += new MouseButtonEventHandler(PrimaryPlaceListItem_MouseLeftButtonDown);
            this.MouseEnter += new MouseEventHandler(PrimaryPlaceListItem_MouseEnter);
        }

        /// <summary>
        /// Displays popup image on mouseover
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PrimaryPlaceListItem_MouseEnter(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(this);
            Point g = e.GetPosition(null);
            Controller.GetInstance().ShowPlaceListHover(attraction, (int)(g.X - p.X), (int)(g.Y - p.Y));
        }

        /// <summary>
        /// Opens tour popup item with clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PrimaryPlaceListItem_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            Controller.GetInstance().StopTour();
            Controller.GetInstance().ShowTourPopup(attraction);
        }
    }
}
