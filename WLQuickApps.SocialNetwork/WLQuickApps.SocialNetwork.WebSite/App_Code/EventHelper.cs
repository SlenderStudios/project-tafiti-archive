using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.WebSite
{
    public struct EventSummary
    {
        public string DisplayName
        {
            get { return this._displayName; }
            set { return; }
        }
        private string _displayName;

        public int BaseItemID
        {
            get { return this._baseItemID; }
            set { return; }
        }
        private int _baseItemID;

        public EventSummary(Event eventItem)
        {
            this._baseItemID = eventItem.BaseItemID;

            string subType = "";
            if (eventItem.SubType.Length > 0) { subType = eventItem.SubType + " - "; }
            this._displayName = String.Format("{0}{1} ({2})", subType, eventItem.Title, eventItem.StartDateTime.ToString("f"));
        }
    }

    /// <summary>
    /// Returns a version of the Event object that a checkboxlist can consume visually
    /// </summary>
    public class EventHelper
    {
        public EventHelper() { }

        static public List<EventSummary> GetEventSummary(string userName)
        {
            return GetEventSummary(EventManager.GetEventsForUserByStartDate(userName, DateTime.Today, DateTime.MaxValue,
                    UserGroupStatus.Joined, 0, EventManager.GetEventsForUserByStartDateCount(userName, DateTime.Today,
                    DateTime.MaxValue, UserGroupStatus.Joined)));
        }

        static public List<EventSummary> GetEventSummary(ReadOnlyCollection<Event> events)
        {
            List<EventSummary> result = new List<EventSummary>();
            foreach (Event eventItem in events)
            {
                result.Add(new EventSummary(eventItem));
            }

            return result;
        }
    }
}
