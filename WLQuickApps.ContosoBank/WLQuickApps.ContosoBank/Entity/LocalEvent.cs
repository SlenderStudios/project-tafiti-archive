using System;
using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class Events
    {
        public List<LocalEvent> LocalEvent { get; set; }
    }

    public class LocalEvent
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Location { get; set; }
        public string EventDuration { get; set; }
        public string Address { get; set; }
        public string ContactDetails { get; set; }
        public bool Featured { get; set; }
    }
}