using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Transactions;
using System.Security;

using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.EventDataSetTableAdapters;

namespace WLQuickApps.SocialNetwork.Business
{
    /// <summary>
    /// Provides data access methods for an event.
    /// </summary>
    public static class EventManager
    {
        static private ReadOnlyCollection<Event> GetEventsFromTable(EventDataSet.EventDataTable eventDataTable)
        {
            List<Event> list = new List<Event>();
            foreach (EventDataSet.EventRow row in eventDataTable)
            {
                Event eventItem = new Event(row);
                if (eventItem.CanView)
                {
                    list.Add(eventItem);
                }
            }
            return list.AsReadOnly();
        }

        static public Event GetEvent(int baseItemID)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                EventDataSet.EventDataTable eventTable = eventTableAdapter.GetEvent(baseItemID);

                if (eventTable.Rows.Count != 1)
                {
                    throw new ArgumentException("The event with the specified ID does not exist");
                }

                Event eventItem = new Event(eventTable[0]);
                if (!eventItem.CanView) { throw new SecurityException("The current user does not have permission to access this item"); }
                return eventItem;
            }
        }

        static public bool EventExists(int baseItemID)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return (eventTableAdapter.GetEvent(baseItemID).Rows.Count != 0);
            }
        }

        static public Event CreateEvent(string name, string description, DateTime startDateTime, DateTime endDateTime, Location location,
            PrivacyLevel privacyLevel)
        {
            return EventManager.CreateEvent(name, description, startDateTime, endDateTime, location, privacyLevel, string.Empty);
        }

        /// <summary>
        /// Create an event in the database.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        static public Event CreateEvent(string name, string description, DateTime startDateTime, DateTime endDateTime,
            Location location, PrivacyLevel privacyLevel, string eventType)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("Name cannot be null or empty"); }
            if (description == null) { description = string.Empty; }
            if (endDateTime <= startDateTime) { throw new ArgumentException("Event cannot end before it begins"); }
            if (string.IsNullOrEmpty(eventType)) { eventType = Constants.GroupTypes.GenericEvent; }

            int baseItemID = -1;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                baseItemID = GroupManager.CreateGroup(name, description, eventType, location, privacyLevel).BaseItemID;
                eventTableAdapter.CreateEvent(baseItemID, startDateTime, endDateTime);

                transactionScope.Complete();
            }

            Event eventItem = EventManager.GetEvent(baseItemID);
            eventItem.Update();
            return eventItem;
        }

        /// <summary>
        /// Delete an event and associated comments.
        /// </summary>
        /// <param name="baseItemID"></param>
        static internal void DeleteEvent(Event eventItem)
        {
            EventManager.VerifyOwnerActionOnEvent(eventItem);

            BaseItemManager.DeleteBaseItem(eventItem);
        }

        /// <summary>
        /// Update an event.
        /// </summary>
        /// <param name="eventItem"></param>
        static public void UpdateEvent(Event eventItem)
        {
            if (eventItem == null) { throw new ArgumentNullException("eventItem"); }

            EventManager.VerifyOwnerActionOnEvent(eventItem);

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                eventTableAdapter.UpdateEvent(eventItem.StartDateTime, eventItem.EndDateTime, eventItem.BaseItemID);
                GroupManager.UpdateGroup(eventItem);

                transaction.Complete();
            }
        }

        static public ReadOnlyCollection<Event> GetEventsForUser(UserGroupStatus status)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return EventManager.GetEventsForUser(UserManager.LoggedInUser.UserName, status, 0, EventManager.GetEventsForUserCount(status));
        }

        static public ReadOnlyCollection<Event> GetEventsForUser(UserGroupStatus status, int startRowIndex, int maximumRows)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return EventManager.GetEventsForUser(UserManager.LoggedInUser.UserName, status, startRowIndex, maximumRows);
        }

        static public int GetEventsForUserCount(UserGroupStatus status)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return EventManager.GetEventsForUserCount(UserManager.LoggedInUser.UserName, status);
        }

        static public ReadOnlyCollection<Event> GetEventsForUser(string userName, UserGroupStatus status)
        {
            return EventManager.GetEventsForUser(userName, status, 0, EventManager.GetEventsForUserCount(userName, status));
        }

        static public ReadOnlyCollection<Event> GetEventsForUser(string userName, UserGroupStatus status, int startRowIndex, int maximumRows)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return EventManager.GetEventsFromTable(eventTableAdapter.GetEventsForUser(UserManager.GetUserByUserName(userName).UserID, (status != UserGroupStatus.Invited), (status != UserGroupStatus.WaitingForApproval), startRowIndex, maximumRows));
            }
        }

        static public int GetEventsForUserCount(string userName, UserGroupStatus status)
        {
            bool isInviteAccepted;
            bool isApprovedByOwner;
            GroupManager.PersistUserGroupStatus(status, out isInviteAccepted, out isApprovedByOwner);

            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return (int)eventTableAdapter.GetEventsForUserCount(UserManager.GetUserByUserName(userName).UserID, isInviteAccepted, isApprovedByOwner);
            }
        }

        static public ReadOnlyCollection<Event> GetAllEventsInSystem(int startRowIndex, int maximumRows)
        {
            using (EventTableAdapter tableAdapter = new EventTableAdapter())
            {
                return EventManager.GetEventsFromTable(tableAdapter.GetAllEvents(startRowIndex, maximumRows));
            }
        }

        static public int GetAllEventsInSystemCount()
        {
            using (EventTableAdapter tableAdapter = new EventTableAdapter())
            {
                return (int)tableAdapter.GetAllEventsCount();
            }
        }

        static public ReadOnlyCollection<Event> GetAllEvents()
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return EventManager.GetEventsForUser(UserManager.LoggedInUser.UserName, new DateTime(2000, 1, 1), DateTime.MaxValue, UserGroupStatus.Joined);
        }

        static public ReadOnlyCollection<Event> GetFutureEventsForUser(string userName, int startRowIndex, int maximumRows)
        {
            return EventManager.GetEventsForUser(userName, DateTime.Now, DateTime.MaxValue, UserGroupStatus.Joined, startRowIndex, maximumRows);
        }

        static public int GetFutureEventsForUserCount(string userName)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return (int)eventTableAdapter.GetAllEventsForUserForDateRangeCount(UserManager.GetUserByUserName(userName).UserID, DateTime.Now, DateTime.MaxValue, true, true);
            }
        }

        static public ReadOnlyCollection<Event> GetPastEventsForUser(string userName, int startRowIndex, int maximumRows)
        {
            return EventManager.GetEventsForUser(userName, new DateTime(2000, 1, 1), DateTime.Now, UserGroupStatus.Joined, startRowIndex, maximumRows);
        }

        static public int GetPastEventsForUserCount(string userName)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return (int)eventTableAdapter.GetAllEventsForUserForDateRangeCount(UserManager.GetUserByUserName(userName).UserID, new DateTime(2000, 1, 1), DateTime.Now, true, true);
            }
        }

        static public ReadOnlyCollection<Event> GetEventsForUser(string userName, DateTime startDateTime, DateTime endDateTime, UserGroupStatus status)
        {
            return EventManager.GetEventsForUser(userName, startDateTime, endDateTime, status, 0, EventManager.GetEventsForUserCount(userName, startDateTime, endDateTime, status));
        }

        static public int GetEventsForUserCount(string userName, DateTime startDateTime, DateTime endDateTime, UserGroupStatus status)
        {
            bool isInviteAccepted;
            bool isApprovedByOwner;
            GroupManager.PersistUserGroupStatus(status, out isInviteAccepted, out isApprovedByOwner);

            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return (int)eventTableAdapter.GetAllEventsForUserForDateRangeCount(UserManager.GetUserByUserName(userName).UserID, startDateTime, endDateTime, isInviteAccepted, isApprovedByOwner);
            }
        }

        static public int GetEventsForUserByStartDateCount(string userName, DateTime searchRangeStart, DateTime searchRangeEnd, UserGroupStatus status)
        {
            bool isInviteAccepted;
            bool isApprovedByOwner;
            GroupManager.PersistUserGroupStatus(status, out isInviteAccepted, out isApprovedByOwner);

            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return (int)eventTableAdapter.GetAllEventsForUserStartingInDateRangeCount(UserManager.GetUserByUserName(userName).UserID, searchRangeStart, searchRangeEnd,
                    isInviteAccepted, isApprovedByOwner);
            }
        }

        static public ReadOnlyCollection<Event> GetEventsForUser(string userName, DateTime startDateTime, DateTime endDateTime,
                UserGroupStatus status, int startRowIndex, int maximumRows)
        {
            bool isInviteAccepted;
            bool isApprovedByOwner;
            GroupManager.PersistUserGroupStatus(status, out isInviteAccepted, out isApprovedByOwner);

            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return EventManager.GetEventsFromTable(eventTableAdapter.GetAllEventsForUserForDateRange(
                    UserManager.GetUserByUserName(userName).UserID, startDateTime, endDateTime, isInviteAccepted,
                    isApprovedByOwner, startRowIndex, maximumRows));
            }
        }

        static public ReadOnlyCollection<Event> GetEventsForUserByStartDate(string userName, DateTime searchRangeStart, DateTime searchRangeEnd,
                UserGroupStatus status, int startRowIndex, int maximumRows)
        {
            bool isInviteAccepted;
            bool isApprovedByOwner;
            GroupManager.PersistUserGroupStatus(status, out isInviteAccepted, out isApprovedByOwner);

            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return EventManager.GetEventsFromTable(eventTableAdapter.GetAllEventsForUserStartingInDateRange(
                    UserManager.GetUserByUserName(userName).UserID, searchRangeStart, searchRangeEnd, isInviteAccepted,
                    isApprovedByOwner, startRowIndex, maximumRows));
            }
        }

        static public ReadOnlyCollection<Event> SearchEventsWithDateRange(DateTime startDateTime, DateTime endDateTime,
            string name, string address1, string address2, string city, string region, string country, string postalCode,
            string tagSearchString)
        {
            return EventManager.SearchEventsWithDateRange(startDateTime, endDateTime, name, address1, address2, city, region, country, postalCode, tagSearchString, 0,
                EventManager.SearchEventsWithDateRangeCount(startDateTime, endDateTime, name, address1, address2, city, region, country, postalCode, tagSearchString));
        }

        static public int SearchEventsWithDateRangeCount(DateTime startDateTime, DateTime endDateTime,
            string name, string address1, string address2, string city, string region, string country, string postalCode,
            string tagSearchString)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return (int)eventTableAdapter.SearchEventsWithDateRangeCount(startDateTime, endDateTime,
                    TagManager.GetTagSearchList(tagSearchString), Location.BuildSearchString(name, address1, address2, city, region, country, postalCode));
            }
        }

        static public ReadOnlyCollection<Event> SearchEventsWithDateRange(DateTime startDateTime, DateTime endDateTime,
            string name, string address1, string address2, string city, string region, string country, string postalCode,
            string tagSearchString, int startRowIndex, int maximumRows)
        {
            if (string.IsNullOrEmpty(name + address1 + address2 + city + region + country + postalCode + tagSearchString) &&
                (endDateTime == DateTime.MaxValue))
            {
                return new List<Event>().AsReadOnly();
            }

            if (name == null) { name = string.Empty; }
            if (address1 == null) { address1 = string.Empty; }
            if (address2 == null) { address2 = string.Empty; }
            if (city == null) { city = string.Empty; }
            if (region == null) { region = string.Empty; }
            if (country == null) { country = string.Empty; }
            if (postalCode == null) { postalCode = string.Empty; }
            if (tagSearchString == null) { tagSearchString = string.Empty; }

            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return EventManager.GetEventsFromTable(eventTableAdapter.SearchEventsWithDateRange(startDateTime, endDateTime,
                    TagManager.GetTagSearchList(tagSearchString), Location.BuildSearchString(name, address1, address2, city, region, country, postalCode),
                    startRowIndex, maximumRows));
            }
        }

        static public ReadOnlyCollection<Event> GetUpcomingEventsForToday()
        {
            return EventManager.GetUpcomingEventsForToday(0, EventManager.GetUpcomingEventsForTodayCount());
        }

        static public ReadOnlyCollection<Event> GetUpcomingEventsForToday(int startRowIndex, int maximumRows)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return EventManager.GetEventsFromTable(eventTableAdapter.GetAllUpcomingEventsForToday(startRowIndex, maximumRows));
            }
        }

        static public int GetUpcomingEventsForTodayCount()
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return (int)eventTableAdapter.GetAllUpcomingEventsForTodayCount();
            }
        }

        static public ReadOnlyCollection<Event> GetEventsByBaseItemID(int baseItemID)
        {
            return EventManager.GetEventsByBaseItemID(baseItemID, 0, GetEventsByBaseItemIDCount(baseItemID));
        }

        static public int GetEventsByBaseItemIDCount(int baseItemID)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return (int)eventTableAdapter.GetEventsByBaseItemIDCount(baseItemID);
            }
        }

        static public ReadOnlyCollection<Event> GetEventsByBaseItemID(int baseItemID, int startRowIndex, int maximumRows)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return EventManager.GetEventsFromTable(eventTableAdapter.GetEventsByBaseItemID(baseItemID, startRowIndex, maximumRows));
            }
        }

        static public ReadOnlyCollection<Event> GetEventsByBaseItemID(int baseItemID, DateTime startDateTime, DateTime endDateTime, int startRowIndex, int maximumRows)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return EventManager.GetEventsFromTable(eventTableAdapter.GetEventsByBaseItemIDForDateRange(baseItemID, startDateTime, endDateTime, startRowIndex, maximumRows));
            }
        }

        static public int GetEventsByBaseItemIDCount(int baseItemID, DateTime startDateTime, DateTime endDateTime)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return (int)eventTableAdapter.GetEventsByBaseItemIDForDateRangeCount(baseItemID, startDateTime, endDateTime);
            }
        }

        static public ReadOnlyCollection<Event> GetPastEventsByBaseItemID(int baseItemID, int startRowIndex, int maximumRows)
        {
            return EventManager.GetEventsByBaseItemID(baseItemID, new DateTime(2000, 1, 1), DateTime.Now, startRowIndex, maximumRows);
        }

        static public int GetPastEventsByBaseItemIDCount(int baseItemID)
        {
            using (EventTableAdapter eventTableAdapter = new EventTableAdapter())
            {
                return EventManager.GetEventsByBaseItemIDCount(baseItemID, new DateTime(2000, 1, 1), DateTime.Now);
            }
        }

        static internal void VerifyOwnerActionOnEvent(Event eventItem)
        {
            if (!EventManager.CanModifyEvent(eventItem.BaseItemID))
            {
                throw new SecurityException("Event cannot be modified because it does not belong to the logged in user");
            }
        }

        static public bool CanModifyEvent(int baseItemID)
        {
            Event eventItem = EventManager.GetEvent(baseItemID);
            return EventManager.CanModifyEvent(eventItem);
        }

        static internal bool CanModifyEvent(Event eventItem)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            return ((eventItem.Owner == UserManager.LoggedInUser) || UserManager.LoggedInUser.IsAdmin);
        }

        public static void RemoveUserFromEvent(User user, Event eventToLeave)
        {
            if (user == null) { throw new ArgumentNullException("user"); }
            if (eventToLeave == null) { throw new ArgumentNullException("eventToLeave"); }

            GroupManager.RemoveUserFromGroup(user, eventToLeave);
        }
    }
}
