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

namespace MetaliqSilverlightSDK.Controls.Mixer
{
    public partial class MixerAsset : UserControl
    {
        public DragInfo Drag;
        public MixerAsset()
        {
            InitializeComponent();
            BackgroundRectangle.Width = Width;
            BackgroundRectangle.Height = Height;
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            BackgroundRectangle.Width = availableSize.Width;
            BackgroundRectangle.Height = availableSize.Height;
            return base.MeasureOverride(availableSize);
        }
        public Color Color
        {
            get
            {
                return (Color)GetValue(RectangleColorProperty);
            }
            set
            {
                BackgroundRectangle.Fill = new SolidColorBrush(value);
                SetValue(RectangleColorProperty, value);
            }
        }
        public static readonly DependencyProperty RectangleColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(MixerAsset), null);
    }
}
