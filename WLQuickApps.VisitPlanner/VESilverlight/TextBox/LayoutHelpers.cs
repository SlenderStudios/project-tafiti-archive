//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace VESilverlight
{
    public static class LayoutHelpers
    {
        internal static void SetClipRect(UIElement element, Rect clip)
        {
            RectangleGeometry rg = new RectangleGeometry();
            rg.Rect = clip;
            element.SetValue(UIElement.ClipProperty, rg);
        }

        internal static Rect DecreaseForMargin(Rect rect, Thickness margin)
        {
            return new Rect(
                rect.Left + margin.Left,
                rect.Top + margin.Top,
                rect.Width - margin.Left - margin.Right,
                rect.Height - margin.Top - margin.Bottom);
        }

        internal static Size IncreaseForMargin(Size size, Thickness margin)
        {
            return new Size(
                margin.Left + margin.Right + size.Width,
                margin.Top + margin.Bottom + size.Height);
        }
    }
}
