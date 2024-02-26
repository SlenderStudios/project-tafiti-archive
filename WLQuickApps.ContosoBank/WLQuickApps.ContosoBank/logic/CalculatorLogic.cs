using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class CalculatorLogic
    {
        public static List<Calculator> GetCalculators()
        {
            if (HttpContext.Current.Cache["Calculators"] == null)
            {
                XDocument calculatorsXML = XDocument.Load(getCalculatorLocation());
                var temp = from feed in calculatorsXML.Descendants("Calculator")
                           select new Calculator
                                      {
                                          ID = Convert.ToInt32(feed.Element("ID").Value),
                                          CalculatorName = feed.Element("CalculatorName").Value,
                                          Description = feed.Element("Description").Value,
                                          DownloadLink = feed.Element("DownloadLink").Value,
                                      };

                HttpContext.Current.Cache.Add("Calculators", temp.ToList(), new CacheDependency(getCalculatorLocation()),
                                              Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0),
                                              CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["Calculators"] as List<Calculator>;
        }

        private static string getCalculatorLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\Calculators.xml";
        }
    }
}