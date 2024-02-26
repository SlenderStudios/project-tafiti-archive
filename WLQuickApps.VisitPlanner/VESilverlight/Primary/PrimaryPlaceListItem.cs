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

namespace VESilverlight.Primary
{
    /// <summary>
    /// Represent a single list item found in the various tool bars and panels
    /// </summary>
    public class PrimaryPlaceListItem : PlaceListItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attraction">Attraction object</param>
        public PrimaryPlaceListItem(Attraction attraction)
            : base(attraction)
        {
            this.MouseLeftButtonDown += new System.Windows.Input.MouseEventHandler(PrimaryPlaceListItem_MouseLeftButtonDown);
            this.MouseEnter += new System.Windows.Input.MouseEventHandler(PrimaryPlaceListItem_MouseEnter);
        }

        /// <summary>
        /// Displays popup image on mouseover
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PrimaryPlaceListItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
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
        void PrimaryPlaceListItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Controller.GetInstance().StopTour();
            Controller.GetInstance().ShowTourPopup(attraction);
        }
    }
}
