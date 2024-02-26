using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.Caching;

using WLQuickApps.FieldManager.Business.WeatherService;

namespace WLQuickApps.FieldManager.Business
{
    public class WeatherManager
    {
        static public Weather[] GetWeather(double latitude, double longitude)
        {
            int days = 6;
            string latLongString = string.Format("{0:0.0},{1:0.0}", latitude, longitude);
            Weather[] weathers = HttpContext.Current.Cache[latLongString] as Weather[];
            if (weathers == null)
            {
                weathers = new Weather[days];
                for (int lcv = 0; lcv < days; lcv++)
                {
                    weathers[lcv] = new Weather();
                    weathers[lcv].Day = DateTime.Now.AddDays(lcv).DayOfWeek.ToString();
                }

                XmlDocument xmlDocument;
                try
                {
                    using (ndfdXML service = new ndfdXML())
                    {
                        string xmlData = service.NDFDgenByDay((decimal)latitude, (decimal)longitude, DateTime.Now, days.ToString(), "24 hourly");

                        xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(xmlData);
                    }
                }
                catch (Exception e)
                {
                    // TODO: Log error?
                    return weathers;
//                    throw;
                }

                XmlNode parametersNode = xmlDocument.SelectSingleNode("dwml/data/parameters");

                XmlNodeList maxTempNodes = xmlDocument.SelectNodes("//temperature[@type='maximum']/value");
                for (int lcv = 0; lcv < days; lcv++)
                {
                    if (maxTempNodes.Count < lcv) { break; }
                    int.TryParse(maxTempNodes[lcv].InnerText, out weathers[lcv].MaxTemperature);
                }

                XmlNodeList minTempNodes = xmlDocument.SelectNodes("//temperature[@type='minimum']/value");
                for (int lcv = 0; lcv < days; lcv++)
                {
                    if (minTempNodes.Count < lcv) { break; }
                    int.TryParse(minTempNodes[lcv].InnerText, out weathers[lcv].MinTemperature);
                }

                XmlNodeList weatherNodes = xmlDocument.SelectNodes("//weather/weather-conditions");
                if ((weatherNodes != null) && (weatherNodes.Count == days))
                {
                    for (int lcv = 0; lcv < days; lcv++)
                    {
                        if (weatherNodes[lcv].Attributes["weather-summary"] != null)
                        {
                            weathers[lcv].WeatherSummary = weatherNodes[lcv].Attributes["weather-summary"].InnerText;
                        }
                    }
                }

                HttpContext.Current.Cache.Add(latLongString, weathers, null, DateTime.Now.AddHours(6), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }

            return weathers;
        }
    }
}
