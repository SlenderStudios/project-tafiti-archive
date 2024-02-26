using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class GadgetLogic
    {
        public static List<Gadget> GetGadgets()
        {
            if (HttpContext.Current.Cache["Gadgets"] == null)
            {
                XDocument gadgetsXML = XDocument.Load(getGadgetLocation());
                var temp = from feed in gadgetsXML.Descendants("Gadget")
                           select new Gadget
                                      {
                                          ID = Convert.ToInt32(feed.Element("ID").Value),
                                          GadgetName = feed.Element("GadgetName").Value,
                                          Description = feed.Element("Description").Value,
                                          Link = feed.Element("Link").Value,
                                          Thumbnail = feed.Element("Thumbnail").Value,
                                      };

                HttpContext.Current.Cache.Add("Gadgets", temp.ToList(), new CacheDependency(getGadgetLocation()),
                                              Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0),
                                              CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["Gadgets"] as List<Gadget>;
        }

        private static string getGadgetLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\Gadgets.xml";
        }
    }
}