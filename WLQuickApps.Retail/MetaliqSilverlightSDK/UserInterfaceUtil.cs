using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetaliqSilverlightSDK
{
    public class UserInterfaceUtil
    {
        public static void ChangeParentKeepPosition(FrameworkElement item, Panel newParent, MouseButtonEventArgs e)
        {
            FrameworkElement parent = item.Parent as FrameworkElement;
            Point newTopLeft = TranslatePoint(new Point(item.GetX(), item.GetY()), parent, newParent, e);
            item.SetX(newTopLeft.X);
            item.SetY(newTopLeft.Y);
            (item.Parent as Panel).Children.Remove(item);
            newParent.Children.Add(item);
        }
        static Point TranslatePoint(Point point, UIElement from, UIElement to, MouseEventArgs e)
        {
            Point fromPoint = e.GetPosition(from);
            Point toPoint = e.GetPosition(to);
            Point delta = new Point(fromPoint.X - toPoint.X, fromPoint.Y - toPoint.Y);
            Point result = new Point(point.X - delta.X, point.Y - delta.Y);
            return result;
        }
    }
}
