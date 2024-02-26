using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class AdvisorLogic
    {
        public static List<Advisor> GetActiveAdvisors()
        {
            if (HttpContext.Current.Cache["Advisors"] == null)
            {
                XDocument advisorssXML = XDocument.Load(getAdvisorLocation());
                var temp = from feed in advisorssXML.Descendants("Advisor")
                           where Convert.ToBoolean(feed.Element("IsActive").Value)
                           select new Advisor
                                      {
                                          ID = Convert.ToInt32(feed.Element("ID").Value),
                                          AdvisorName = feed.Element("AdvisorName").Value,
                                          CID = feed.Element("CID").Value,
                                          IsActive = Convert.ToBoolean(feed.Element("IsActive").Value),
                                          AvailableAppointment =
                                              Convert.ToBoolean(feed.Element("AvailableAppointment").Value),
                                      };

                HttpContext.Current.Cache.Add("Advisors", temp.ToList(), new CacheDependency(getAdvisorLocation()),
                                              Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0),
                                              CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["Advisors"] as List<Advisor>;
        }

        public static Advisor GetAdvisorByID(int id)
        {
            var temp = from feed in GetActiveAdvisors()
                       where feed.ID == id
                       select new Advisor
                                  {
                                      ID = feed.ID,
                                      AdvisorName = feed.AdvisorName,
                                      CID = feed.CID,
                                      IsActive = feed.IsActive,
                                      AvailableAppointment = feed.AvailableAppointment,
                                  };

            return temp.FirstOrDefault();
        }

        private static string getAdvisorLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\Advisors.xml";
        }
    }
}