using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Linq;
using System.Text;

using WLQuickApps.FieldManager.Data;
using System.Security;

namespace WLQuickApps.FieldManager.Business
{
    public class LeagueManager
    {
        static private ReadOnlyCollection<League> GetListFromTable(IEnumerable<League> items)
        {
            return (new List<League>(items)).AsReadOnly();
        }

        static public ReadOnlyCollection<League> GetAllLeagues(int startRowIndex, int maximumRows)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return LeagueManager.GetListFromTable(
                    (from items in context.Leagues select items)
                    .Skip(startRowIndex)
                    .Take(maximumRows));
            }
        }

        static public int GetAllLeaguesCount()
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return context.Leagues.Count();
            }
        }

        static public ReadOnlyCollection<League> GetLeaguesForUser(Guid userID, int startRowIndex, int maximumRows)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return LeagueManager.GetListFromTable(
                    (from items in context.Leagues
                     where items.UserLeagues.Contains(
                        (from userItem in context.UserLeagues 
                         where userItem.UserID == userID 
                         select userItem).Single())
                     select items)
                    .Skip(startRowIndex)
                    .Take(maximumRows));
            }
        }

        static public int GetLeaguesForUserCount(Guid userID)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return context.UserLeagues.Count(item => item.UserID == userID);
            }
        }



        static public League GetLeague(int leagueID)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return (from items in context.Leagues select items).Single(item => item.LeagueID == leagueID);
            }
        }

        static public bool IsLeagueAdmin(int leagueID)
        {
            if (!UserManager.UserIsLoggedIn()) { return false; }

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return (context.LeagueAdmins.Count(items => ((items.UserID == UserManager.LoggedInUser.UserID) && (items.LeagueID == leagueID))) > 0);
            }
        }

        static public bool IsLeagueUser(int leagueID)
        {
            if (!UserManager.UserIsLoggedIn()) { return false; }

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return (context.UserLeagues.Count(items => ((items.UserID == UserManager.LoggedInUser.UserID) && (items.LeagueID == leagueID))) > 0);
            }
        }

        static public League CreateLeague(string title, string description, string type)
        {
            UserManager.VerifyAUserIsLoggedIn();

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                League league = new League();
                league.Description = description;
                league.Title = title;
                league.Type = type;

                context.Leagues.InsertOnSubmit(league);
                context.SubmitChanges();

                LeagueAdmin leagueAdmin = new LeagueAdmin();
                leagueAdmin.UserID = UserManager.LoggedInUser.UserID;
                leagueAdmin.LeagueID = league.LeagueID;
                context.LeagueAdmins.InsertOnSubmit(leagueAdmin);
                context.SubmitChanges();

                LeagueManager.AddLeagueUser(league.LeagueID);

                return league;
            }
        }

        static public void AddLeagueAdmin(int leagueID, Guid userID)
        {
            LeagueManager.VerifyOwnerAction(leagueID);

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                context.UserLeagues.DeleteAllOnSubmit(
                    from items in context.UserLeagues
                    where ((items.LeagueID == leagueID) && (items.UserID == userID))
                    select items);

                context.LeagueAdmins.DeleteAllOnSubmit(
                    from items in context.LeagueAdmins
                    where ((items.LeagueID == leagueID) && (items.UserID == userID))
                    select items);
                context.SubmitChanges();

                League league = (from items in context.Leagues select items).Single(item => item.LeagueID == leagueID);
                
                LeagueAdmin leagueAdmin = new LeagueAdmin();
                leagueAdmin.UserID = userID;
                leagueAdmin.LeagueID = league.LeagueID;
                context.LeagueAdmins.InsertOnSubmit(leagueAdmin);

                UserLeague userLeague = new UserLeague();
                userLeague.UserID = userID;
                userLeague.LeagueID = league.LeagueID;
                context.UserLeagues.InsertOnSubmit(userLeague);

                context.SubmitChanges();
            }
        }

        static public void AddLeagueUser(int leagueID)
        {
            UserManager.VerifyAUserIsLoggedIn();

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                League league = (from items in context.Leagues select items).Single(item => item.LeagueID == leagueID);
                UserLeague userLeague = new UserLeague();
                userLeague.UserID = UserManager.LoggedInUser.UserID;
                userLeague.LeagueID = league.LeagueID;
                context.UserLeagues.InsertOnSubmit(userLeague);
                context.SubmitChanges();
            }
        }


        static public void UpdateLeague(int leagueID, string title, string description, string type)
        {
            LeagueManager.VerifyOwnerAction(leagueID);

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                League league = (from items in context.Leagues select items).Single(item => item.LeagueID == leagueID);

                league.Title = title;
                league.Description = description;
                league.Type = type;

                context.SubmitChanges();
            }
        }

        static public void DeleteLeague(int leagueID)
        {
            LeagueManager.VerifyOwnerAction(leagueID);

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                League league = (from items in context.Leagues select items).Single(item => item.LeagueID == leagueID);
                context.Leagues.DeleteOnSubmit(league);
                context.SubmitChanges();
            }
        }

        static public void RemoveLeague(int leagueID)
        {
            UserManager.VerifyAUserIsLoggedIn();

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                UserLeague userLeague = (from items in context.UserLeagues select items).Single(item => ((item.LeagueID == leagueID) && (item.UserID == UserManager.LoggedInUser.UserID)));
                context.UserLeagues.DeleteOnSubmit(userLeague);
                context.SubmitChanges();

                if (LeagueManager.IsLeagueAdmin(leagueID))
                {
                    if (context.LeagueAdmins.Count(items => (items.LeagueID == leagueID)) == 1)
                    {
                        LeagueManager.DeleteLeague(leagueID);
                    }
                    else
                    {
                        LeagueAdmin leagueAdmin = (from items in context.LeagueAdmins select items).Single(item => ((item.LeagueID == leagueID) && (item.UserID == UserManager.LoggedInUser.UserID)));
                        context.LeagueAdmins.DeleteOnSubmit(leagueAdmin);
                        context.SubmitChanges();
                    }
                }
            }
        }



        static internal void VerifyOwnerAction(int leagueID)
        {
            UserManager.VerifyAUserIsLoggedIn();

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                if (context.LeagueAdmins.Count(item => ((item.UserID == UserManager.LoggedInUser.UserID) && (item.LeagueID == leagueID))) == 0)
                {
                    throw new SecurityException("You must be an administrator for this league to perform this action");
                }
            }

        }

    }
}
