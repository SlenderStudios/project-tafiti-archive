using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class ContosoCupLogic
    {
        public static ContosoCup GetLeaderBoard()
        {
            ContosoCup leaderBoard = new ContosoCup
                                         {
                                             Ladder = TeamResultLogic.GetTeamResults(),
                                             PlayerOfWeek = GetLatestRound().PlayerOfWeek
                                         };
            return leaderBoard;
        }

        public static Round GetLatestRound()
        {
            if (HttpContext.Current.Cache["ContosoCupDraw"] == null)
            {
                XDocument roundsXml = XDocument.Load(getContosoCupDrawLocation());
                var temp = from feed in roundsXml.Descendants("Round")
                           orderby Convert.ToInt32(feed.Element("RoundNumber").Value) descending
                           select new Round
                                      {
                                          RoundNumber = Convert.ToInt32(feed.Element("RoundNumber").Value),
                                          PlayerOfWeek = feed.Element("PlayerOfWeek").Value,
                                      };


                HttpContext.Current.Cache.Add("ContosoCupDraw", temp.FirstOrDefault(),
                                              new CacheDependency(getContosoCupDrawLocation()),
                                              Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0),
                                              CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["ContosoCupDraw"] as Round;
        }

        private static string getContosoCupDrawLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\ContosoCupDraw.xml";
        }
    }
}