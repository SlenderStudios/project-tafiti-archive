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

namespace MetaliqSilverlightSDK
{
    public partial class UIControlBase : Canvas
    {
        public Canvas root;

        public UIControlBase()
        {
            setup();
            Loaded += new RoutedEventHandler(PageLoaded);
        }
        protected virtual void PageLoaded(object o, RoutedEventArgs e)
        {

        }
        protected virtual void setup()
        {
        }
        protected Canvas init(string xamlFile)
        {
            return this;
        }
        public double X
        {
            set
            {
                this.SetValue(Canvas.LeftProperty, value);
            }
            get
            {
                return (double)this.GetValue(Canvas.LeftProperty);
            }
        }
        public double Y
        {
            set
            {
                this.SetValue(Canvas.TopProperty, value);
            }
            get
            {
                return (double)this.GetValue(Canvas.TopProperty);
            }
        }
    }
}
