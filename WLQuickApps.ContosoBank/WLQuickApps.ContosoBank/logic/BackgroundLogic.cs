using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class BackgroundLogic
    {
        public static List<Background> GetBackgrounds()
        {
            if (HttpContext.Current.Cache["Backgrounds"] == null)
            {
                XDocument backgroundsXML = XDocument.Load(getBackgroundLocation());
                var temp = from feed in backgroundsXML.Descendants("Background")
                           orderby feed.Element("Name").Value
                           select new Background
                                      {
                                          ID = Convert.ToInt32(feed.Element("ID").Value),
                                          Name = feed.Element("Name").Value,
                                          Location = feed.Element("Location").Value,
                                          IsDefault = Convert.ToBoolean(feed.Element("IsDefault").Value),
                                      };

                HttpContext.Current.Cache.Add("Backgrounds", temp.ToList(), new CacheDependency(getBackgroundLocation()),
                                              Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0),
                                              CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["Backgrounds"] as List<Background>;
        }

        public static Background GetDefaultBackground()
        {
            if (HttpContext.Current.Cache["DefaultBackground"] == null)
            {
                XDocument backgroundsXML = XDocument.Load(getBackgroundLocation());
                var temp = from feed in backgroundsXML.Descendants("Background")
                           where Convert.ToBoolean(feed.Element("IsDefault").Value)
                           select new Background
                                      {
                                          ID = Convert.ToInt32(feed.Element("ID").Value),
                                          Name = feed.Element("Name").Value,
                                          Location = feed.Element("Location").Value,
                                          IsDefault = Convert.ToBoolean(feed.Element("IsDefault").Value),
                                      };

                HttpContext.Current.Cache.Add("DefaultBackground", temp.ToList(),
                                              new CacheDependency(getBackgroundLocation()), Cache.NoAbsoluteExpiration,
                                              new TimeSpan(2, 0, 0), CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["DefaultBackground"] as Background;
        }

        public static Background GetBackgroundByName(string backgroundName)
        {
            XDocument backgroundsXML = XDocument.Load(getBackgroundLocation());
            var temp = from feed in backgroundsXML.Descendants("Background")
                       where feed.Element("Name").Value == backgroundName
                       select new Background
                                  {
                                      ID = Convert.ToInt32(feed.Element("ID").Value),
                                      Name = feed.Element("Name").Value,
                                      Location = feed.Element("Location").Value,
                                      IsDefault = Convert.ToBoolean(feed.Element("IsDefault").Value),
                                  };

            return temp.FirstOrDefault();
        }

        private static string getBackgroundLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\Backgrounds.xml";
        }
    }
}