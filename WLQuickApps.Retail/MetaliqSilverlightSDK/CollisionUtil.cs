using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace MetaliqSilverlightSDK
{
    public class CollisionUtil
    {
        public static bool CheckCollision(FrameworkElement ControlA, FrameworkElement ControlB)
        {
            // first see if sprite rectangles collide
            Rect BoundingBoxA = ControlA.GetBounds();
            Rect BoundingBoxB = ControlA.GetBounds();
            BoundingBoxA.Intersect(BoundingBoxB);
            if (BoundingBoxA == Rect.Empty)
            {
                // no collision - GET OUT!
                return false;
            }
            else
            {
                bool bCollision = false;
                Point ptCheck = new Point();

                // now we do a more accurate pixel hit test
                for (int x = Convert.ToInt32(BoundingBoxA.X); x < Convert.ToInt32(BoundingBoxA.X + BoundingBoxA.Width); x++)
                {
                    for (int y = Convert.ToInt32(BoundingBoxA.Y); y < Convert.ToInt32(BoundingBoxA.Y + BoundingBoxA.Height); y++)
                    {
                        ptCheck.X = x;
                        ptCheck.Y = y;

                        List<UIElement> hits = (List<UIElement>)VisualTreeHelper.FindElementsInHostCoordinates(ptCheck, ControlA);
                        if (hits.Contains(ControlA))
                        {
                            // we have a hit on the first control elem,
                            // now see if the second elem has a similar hit
                            List<UIElement> hits2 = (List<UIElement>)VisualTreeHelper.FindElementsInHostCoordinates(ptCheck, ControlB);
                            if (hits2.Contains(ControlB))
                           {
                                bCollision = true;
                                break;
                            }
                        }
                    }
                    if (bCollision) break;
                }
                return bCollision;
            }
       }
   }
}
