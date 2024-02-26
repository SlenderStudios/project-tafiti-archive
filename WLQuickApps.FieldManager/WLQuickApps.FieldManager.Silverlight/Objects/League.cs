using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using WLQuickApps.FieldManager.Silverlight.SiteService;

namespace WLQuickApps.FieldManager.Silverlight
{
    public class League
    {
        private LeagueItem _league;

        // We need to use LeagueID as the Tag for the UI controls we databind to, but
        // it throws an exception unless we bind to a string. As a result, this
        // whole class exists just to provide the ID as a string.
        public string LeagueID { get { return this._league.LeagueID.ToString(); } }
        public string Description { get { return this._league.Description; } }
        public string Title { get { return this._league.Title; } }
        public string Type { get { return this._league.Type; } }
        public string TitleAndDescription
        {
            get
            {
                if (string.IsNullOrEmpty(this.Description))
                {
                    return this.Title;
                }
                return string.Format("{0} ({1})", this.Title, this.Description);
            }
        }

        public League()
        {
        }

        static public League CreateFromLeague(LeagueItem leagueItem)
        {
            League league = new League();
            league._league = leagueItem;
            return league;
        }

        static public League CreateMyFieldsLeague()
        {
            League league = new League();
            league._league = new LeagueItem();
            league._league.LeagueID = -1;
            league._league.Title = "<View My Fields>";
            return league;
        }

        static public List<League> CreateFromLeagueList(IEnumerable<LeagueItem> leagueItems)
        {
            List<League> leagues = new List<League>();
            foreach (LeagueItem leagueItem in leagueItems)
            {
                leagues.Add(League.CreateFromLeague(leagueItem));
            }
            return leagues;
        }
    }
}
