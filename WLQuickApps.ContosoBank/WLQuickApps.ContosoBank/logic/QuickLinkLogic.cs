using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class QuickLinkLogic
    {
        public static List<QuickLink> GetQuickLinks()
        {
            if (HttpContext.Current.Cache["QuickLinks"] == null)
            {
                XDocument advisorssXML = XDocument.Load(getQuickLinksLocation());
                var temp = from feed in advisorssXML.Descendants("QuickLink")
                           select new QuickLink
                                      {
                                          Link = feed.Element("Link").Value,
                                          Title = feed.Element("Title").Value,
                                      };

                HttpContext.Current.Cache.Add("QuickLinks", temp.ToList(), new CacheDependency(getQuickLinksLocation()),
                                              Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0),
                                              CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["QuickLinks"] as List<QuickLink>;
        }

        private static string getQuickLinksLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\QuickLinks.xml";
        }
    }
}