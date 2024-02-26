using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;

using WLQuickApps.FieldManager.Business;
using WLQuickApps.FieldManager.Data;

namespace WLQuickApps.FieldManager.WebSite
{
    /// <summary>
    /// Summary description for LeagueItem
    /// </summary>
    [DataContract]
    public class LeagueItem
    {
        [DataMember] public string Description;
        [DataMember] public int LeagueID;
        [DataMember] public string Title;
        [DataMember] public string Type;

        public LeagueItem() { }

        static public LeagueItem CreateFromLeague(League item)
        {
            LeagueItem leagueItem = new LeagueItem();

            leagueItem.Description = item.Description;
            leagueItem.LeagueID = item.LeagueID;
            leagueItem.Title = item.Title;
            leagueItem.Type = item.Type;

            return leagueItem;
        }

        static public LeagueItem[] CreateFromLeagueList(IEnumerable<League> list)
        {
            List<LeagueItem> leagueItems = new List<LeagueItem>();
            foreach (League league in list)
            {
                leagueItems.Add(LeagueItem.CreateFromLeague(league));
            }
            return leagueItems.ToArray();
        }

    }
}