using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;


/// <summary>
/// Summary description for CalendarService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class CalendarService : System.Web.Services.WebService {

    public class Event
    {
        public string Uid { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Location { get; set; }
        public string Url { get; set; }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public List<Event> GetEvents(DateTime contextKey)
    {
        Cache cache = HttpRuntime.Cache;

        // Try to retrieve the calendar feed from the cache
        List<Event> events = (List<Event>)cache["Calendar"];

        if (events != null)
        {
            List<Event> results = (from e in events
                                   where e.DateStart.Month == contextKey.Month &&
                                         e.DateStart.Year == contextKey.Year
                                   orderby e.DateStart
                                   select e).ToList<Event>();
            return results;
        }
        else
        {
            return null;
        }
    }    
}

