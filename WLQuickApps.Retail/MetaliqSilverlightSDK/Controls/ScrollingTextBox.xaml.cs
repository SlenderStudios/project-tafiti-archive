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

namespace MetaliqSilverlightSDK.Controls
{
    public partial class ScrollingTextBox : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                tb.Text = value;
            }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string),
                               typeof(ScrollingTextBox), null);


        public ScrollingTextBox()
        {
            InitializeComponent();
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (hiddenTextBlock != null)
            {
                hiddenTextBlock.Text = tb.Text;
                tb.Width = hiddenTextBlock.ActualWidth;
                tb.Height = hiddenTextBlock.ActualHeight;
            }
        }
    }
}
