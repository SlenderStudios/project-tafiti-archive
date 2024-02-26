using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class ArticlesResourcesLogic
    {
        public static List<Resource> GetArticlesResourcesByType(string resourceType, int items)
        {
            if (HttpContext.Current.Cache["ArticlesResources" + resourceType] == null)
            {
                XDocument advisorssXML = XDocument.Load(getArticlesResourcesLocation());
                var temp = from feed in advisorssXML.Descendants("Resource")
                           where feed.Element("ResourceType").Value == resourceType
                           select new Resource
                                      {
                                          Link = feed.Element("Link").Value,
                                          Title = feed.Element("Title").Value,
                                          ResourceType = feed.Element("ResourceType").Value,
                                          Area = feed.Element("Area").Value,
                                      };
                HttpContext.Current.Cache.Add("ArticlesResources" + resourceType, temp.ToList(),
                                              new CacheDependency(getArticlesResourcesLocation()),
                                              Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0),
                                              CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["ArticlesResources" + resourceType] as List<Resource>;
        }

        private static string getArticlesResourcesLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\ArticlesResources.xml";
        }
    }
}