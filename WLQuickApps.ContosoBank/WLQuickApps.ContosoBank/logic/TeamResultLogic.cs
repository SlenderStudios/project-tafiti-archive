using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class TeamResultLogic
    {
        public static List<TeamResult> GetTeamResults()
        {
            if (HttpContext.Current.Cache["TeamResults"] == null)
            {
                XDocument resultsXML = XDocument.Load(getTeamResultLocation());
                var temp = from feed in resultsXML.Descendants("TeamResult")
                           orderby Convert.ToInt32(feed.Element("Won").Value) descending
                           select new TeamResult
                                      {
                                          ID = Convert.ToInt32(feed.Element("ID").Value),
                                          Name = feed.Element("Name").Value,
                                          Played = Convert.ToInt32(feed.Element("Played").Value),
                                          Won = Convert.ToInt32(feed.Element("Won").Value),
                                          Lost = Convert.ToInt32(feed.Element("Lost").Value),
                                          Draw = Convert.ToInt32(feed.Element("Draw").Value)
                                      };

                HttpContext.Current.Cache.Add("TeamResults", temp.ToList(), new CacheDependency(getTeamResultLocation()),
                                              Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0),
                                              CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["TeamResults"] as List<TeamResult>;
        }

        private static string getTeamResultLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\TeamResults.xml";
        }
    }
}