using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class EventLogic
    {
        public static List<LocalEvent> GetEvents()
        {
            XDocument eventsXml = XDocument.Load(getEventLocation());
            var temp = from feed in eventsXml.Descendants("LocalEvent")
                       orderby Convert.ToDateTime(feed.Element("EventDate").Value) descending
                       select new LocalEvent
                                  {
                                      ID = Convert.ToInt32(feed.Element("ID").Value),
                                      EventDate = Convert.ToDateTime(feed.Element("EventDate").Value),
                                      EventDescription = feed.Element("EventDescription").Value,
                                      EventName = feed.Element("EventName").Value,
                                      Latitude = Convert.ToDouble(feed.Element("Latitude").Value),
                                      Longitude = Convert.ToDouble(feed.Element("Longitude").Value),
                                      Location = feed.Element("Location").Value
                                  };
            return temp.ToList();
        }

        public static List<LocalEvent> GetFeaturedEvents()
        {
            XDocument eventsXml = XDocument.Load(getEventLocation());
            var temp = from feed in eventsXml.Descendants("LocalEvent")
                       where Convert.ToBoolean(feed.Element("Featured").Value)
                       orderby Convert.ToDateTime(feed.Element("EventDate").Value) descending
                       select new LocalEvent
                                  {
                                      ID = Convert.ToInt32(feed.Element("ID").Value),
                                      EventDate = Convert.ToDateTime(feed.Element("EventDate").Value),
                                      EventDescription = feed.Element("EventDescription").Value,
                                      EventName = feed.Element("EventName").Value,
                                      Latitude = Convert.ToDouble(feed.Element("Latitude").Value),
                                      Longitude = Convert.ToDouble(feed.Element("Longitude").Value),
                                      Location = feed.Element("Location").Value,
                                      Address = feed.Element("Address").Value,
                                      ContactDetails = feed.Element("ContactDetails").Value,
                                      EventDuration = feed.Element("EventDuration").Value
                                  };
            return temp.ToList();
        }

        public static LocalEvent GetEventByID(int eventID)
        {
            XDocument eventsXml = XDocument.Load(getEventLocation());
            var temp = from feed in eventsXml.Descendants("LocalEvent")
                       where Convert.ToInt32(feed.Element("ID").Value) == eventID
                       select new LocalEvent
                                  {
                                      ID = Convert.ToInt32(feed.Element("ID").Value),
                                      EventDate = Convert.ToDateTime(feed.Element("EventDate").Value),
                                      EventDescription = feed.Element("EventDescription").Value,
                                      EventName = feed.Element("EventName").Value,
                                      Latitude = Convert.ToDouble(feed.Element("Latitude").Value),
                                      Longitude = Convert.ToDouble(feed.Element("Longitude").Value),
                                      Location = feed.Element("Location").Value,
                                      Address = feed.Element("Address").Value,
                                      ContactDetails = feed.Element("ContactDetails").Value,
                                      EventDuration = feed.Element("EventDuration").Value,
                                  };
            return temp.FirstOrDefault();
        }

        private static string getEventLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\events.xml";
        }
    }
}