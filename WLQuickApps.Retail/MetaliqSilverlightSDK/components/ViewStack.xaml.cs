using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Markup;

namespace MetaliqSilverlightSDK.Controls
{
    public class ViewStack : Canvas
    {
        public delegate void StateChangedEvent(int updatedIndex, int oldIndex);
        public event StateChangedEvent StateChanged;

        public ViewStack()
        {
            System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream("MetaliqSilverlightSDK.Controls.ViewStack.xaml");
            this.InitializeFromXaml(new System.IO.StreamReader(s).ReadToEnd());
            Loaded += new RoutedEventHandler(PageLoaded);
        }
        protected void PageLoaded(object sender, RoutedEventArgs e)
        {
            VisibleIndex = 0;
        }
        protected FrameworkElement InitializeFromXaml(string xaml)
        {
            FrameworkElement control = (FrameworkElement)XamlReader.Load(xaml);
            this.Children.Add(control);
            return control;
        }
        protected int _VisibleIndex;
        public int VisibleIndex
        {
            get { return _VisibleIndex; }
            set
            {
                int oldVisibleIndex = _VisibleIndex;
                _VisibleIndex = value;
                for (int i = 0; i < Children.Count; i++)
                {
                    FrameworkElement element = (FrameworkElement)Children[i];
                    // todo - figure out why this is off by 1
                    if (i == value+1)
                    {
                        element.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                }
                StateChanged(_VisibleIndex, oldVisibleIndex);
            }
        }
    }
}
