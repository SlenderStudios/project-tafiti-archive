using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Browser;
using System.Windows.Shapes;
using System.Windows.Input;

namespace VESilverlight.Primary
{
    [ScriptableType]
    public partial class Checkbox : UserControl
    {
        private bool value = false;
        
        public Checkbox()
		{
            this.InitializeComponent();

            this.Loaded += new RoutedEventHandler(Checkbox_Loaded);            
		}

        void Checkbox_Loaded(object sender, RoutedEventArgs e)
        {
            this.OnLoaded(sender, e);
        }
       
        /// <summary>
        /// OnLoaded override
        /// </summary>
        /// <param name="e">event args</param>
        protected void OnLoaded(object sender, RoutedEventArgs args)
        {            
            this.MouseLeftButtonDown += new MouseButtonEventHandler(this_MouseLeftButtonDown);
        }

        void this_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            value = !value;

            if (value)
            {
                this.Check.Visibility = Visibility.Visible;
            }
            else
            {
                this.Check.Visibility = Visibility.Collapsed;
            }

            ValueChanged(this, new ValueChangedEventArgs(value));
        }

        public class ValueChangedEventArgs : EventArgs{
            private bool value = false;
            
            public ValueChangedEventArgs(bool newValue)
            {
                value = newValue;
            }

            public bool Value
            {
                get { return value; }
            }
        }

        public bool Value
        {
            get { return value; }
            set
            {
                if (this.value != value && ValueChanged != null)
                    ValueChanged(this, new ValueChangedEventArgs(value));
                
                this.value = value;
                if (value)
                {
                    this.Check.Visibility = Visibility.Visible;
                }
                else
                {
                    this.Check.Visibility = Visibility.Collapsed;
                }
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        
    }
}