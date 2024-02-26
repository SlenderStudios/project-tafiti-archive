using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Browser;

namespace WLQuickApps.FieldManager.Silverlight
{
    public class WeatherToImagePathConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string summary;
            if (!(value is string))
            {
                return Utilities.AppUrlRoot + "/Images/Weather/na.png";
                
            }
            summary = ((string)value).ToLower();

            if (summary.Contains("mostly cloudy")) { return new BitmapImage(new Uri(Utilities.AppUrlRoot + "/Images/Weather/mcloudy.png")); }
            if (summary.Contains("cloud")) { return new BitmapImage(new Uri(Utilities.AppUrlRoot + "/Images/Weather/pcloudy.png")); }
            if (summary.Contains("sunny")) { return new BitmapImage(new Uri(Utilities.AppUrlRoot + "/Images/Weather/sunny.png")); }
            if (summary.Contains("rain")) { return new BitmapImage(new Uri(Utilities.AppUrlRoot + "/Images/Weather/rain.png")); }
            if (summary.Contains("snow")) { return new BitmapImage(new Uri(Utilities.AppUrlRoot + "/Images/Weather/snow.png")); }
            if (summary.Contains("wintry mix")) { return new BitmapImage(new Uri(Utilities.AppUrlRoot + "/Images/Weather/snow.png")); }
            if (summary.Contains("breezy")) { return new BitmapImage(new Uri(Utilities.AppUrlRoot + "/Images/Weather/wind.png")); }
            if (summary.Contains("fog")) { return new BitmapImage(new Uri(Utilities.AppUrlRoot + "/Images/Weather/fog.png")); }
            if (summary.Contains("thunder")) { return new BitmapImage(new Uri(Utilities.AppUrlRoot + "/Images/Weather/tstorm.png")); }
            if (summary.Contains("hazy")) { return new BitmapImage(new Uri(Utilities.AppUrlRoot + "/Images/Weather/hazy.png")); }

            if (summary.Length > 0)
            {
                HtmlPage.Window.Invoke("alert", "DEBUG - Weather summary without recognized token:\n" + summary);
            }

            return Utilities.AppUrlRoot + "/Images/Weather/na.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "Not Implemented";
            //throw new NotImplementedException();
        }

        #endregion
    }
}
