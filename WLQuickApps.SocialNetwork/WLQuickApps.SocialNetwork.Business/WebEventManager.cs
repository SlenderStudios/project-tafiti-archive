using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.WebEventDataSetTableAdapters;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class WebEventManager
    {
        static public WebEvent GetWebEvent(string eventID)
        {
            if (String.IsNullOrEmpty(eventID))
            {
                throw new ArgumentException("eventID is null or empty.", "eventID");
            }
            
            using (aspnet_WebEvent_EventsTableAdapter webEventTableAdapter = new aspnet_WebEvent_EventsTableAdapter())
            {
                WebEventDataSet.aspnet_WebEvent_EventsDataTable webEventTable = webEventTableAdapter.GetWebEventByEventID(eventID);
                if (webEventTable.Rows.Count != 1)
                {
                    throw new ArgumentException("The web event with the specified ID does not exist");
                }
                return (new WebEvent(webEventTable[0]));
            }
        }

        static public int GetWebEventCount()
        {
            using (aspnet_WebEvent_EventsTableAdapter webEventTableAdapter = new aspnet_WebEvent_EventsTableAdapter())
            {
                return (int)webEventTableAdapter.GetWebEventCount();
            }
        }

        static public List<WebEvent> GetWebEvents()
        {
            return WebEventManager.GetWebEvents(0, WebEventManager.GetWebEventCount());
        }

        static public List<WebEvent> GetWebEvents(int startRowIndex, int maximumRows)
        {
            List<WebEvent> list = new List<WebEvent>();

            using (aspnet_WebEvent_EventsTableAdapter webEventTableAdapter = new aspnet_WebEvent_EventsTableAdapter())
            {
                foreach (WebEventDataSet.aspnet_WebEvent_EventsRow row in webEventTableAdapter.GetWebEvents(startRowIndex, maximumRows))
                {
                    list.Add(new WebEvent(row));
                }
            }
            return list;
        }
    }
}
