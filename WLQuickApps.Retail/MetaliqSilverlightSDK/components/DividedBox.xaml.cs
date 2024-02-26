using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetaliqSilverlightSDK.Controls
{
    public class DividedBox : Control
    {
        public DividedBox()
        {
            System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream("MetaliqSilverlightSDK.Controls.DividedBox.xaml");
            this.InitializeFromXaml(new System.IO.StreamReader(s).ReadToEnd());
        }
    }
}
