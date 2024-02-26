using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for EventTests
    /// </summary>
    [TestClass]
    public class EventTests
    {
        public EventTests()
        {
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Utilities.SwitchToOwnerUser();
            foreach (Event eventItem in EventManager.GetAllEvents())
            {
                eventItem.Delete();
            }
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Utilities.SwitchToOwnerUser();
            foreach (Event eventItem in EventManager.GetAllEvents())
            {
                eventItem.Delete();
            }
        }
        #endregion

        [TestMethod]
        public void TestCreateEvent()
        {
            Utilities.SwitchToOwnerUser();

            DateTime startDateTime = DateTime.Now;
            DateTime endDateTime = DateTime.Now.AddHours(1);

            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, startDateTime, endDateTime, Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);

            Assert.AreEqual(Utilities.OwnerUser, eventItem.Owner); 
            Assert.AreEqual(Constants.Strings.EventDescription, eventItem.Description);
            Assert.AreEqual(endDateTime.ToString(), eventItem.EndDateTime.ToString());
            Assert.AreEqual(Constants.Strings.EventName, eventItem.Title);
            Assert.AreEqual(startDateTime.ToString(), eventItem.StartDateTime.ToString());

            eventItem = EventManager.GetEvent(eventItem.BaseItemID);

            Assert.AreEqual(Utilities.OwnerUser, eventItem.Owner);
            
            Assert.AreEqual(Constants.Strings.EventDescription, eventItem.Description);
            Assert.AreEqual(endDateTime.ToString(), eventItem.EndDateTime.ToString());
            Assert.AreEqual(Constants.Strings.EventName, eventItem.Title);
            Assert.AreEqual(startDateTime.ToString(), eventItem.StartDateTime.ToString());
            
            eventItem.Description = "New Description";
            eventItem.ChangeEventTime(startDateTime.AddHours(1), endDateTime.AddHours(1));
            eventItem.Title = "New Name";

            EventManager.UpdateEvent(eventItem);
            Event updatedEvent = EventManager.GetEvent(eventItem.BaseItemID);
            Assert.AreEqual(eventItem.Description, updatedEvent.Description);
            Assert.AreEqual(eventItem.EndDateTime.ToString(), updatedEvent.EndDateTime.ToString());
            Assert.AreEqual(eventItem.Title, updatedEvent.Title);
            Assert.AreEqual(eventItem.StartDateTime.ToString(), updatedEvent.StartDateTime.ToString());

            eventItem.Delete();

            try
            {
                EventManager.GetEvent(eventItem.BaseItemID);
                Assert.Fail("GetEvent did not throw an exception when a deleted item was requested");
            }
            catch (AssertFailedException) { throw; }
            catch (Exception)
            {
            }
        }

        [TestMethod]
        public void TestEventRetrieval()
        {
            Utilities.SwitchToOwnerUser();
            User user = Utilities.OwnerUser;

            Assert.AreEqual(0, EventManager.GetEventsForUser(user.UserName, UserGroupStatus.Joined).Count);

            // Create new event
            string name = Constants.Strings.EventName;
            string description = Constants.Strings.EventDescription;
            DateTime startDateTime = new DateTime(2007, 10, 25, 15, 30, 30);
            DateTime endDateTime = new DateTime(2007, 10, 25, 16, 45, 0);

            Event anEvent = EventManager.CreateEvent(name,
                description, startDateTime, endDateTime, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);

            // Number of events user created
            Assert.AreEqual(1, EventManager.GetEventsForUser(user.UserName, UserGroupStatus.Joined).Count);

            // Retrieve event by creatorID
            ReadOnlyCollection<Event> events = EventManager.GetEventsForUser(user.UserName, UserGroupStatus.Joined);
            foreach (Event eventItem in events)
            {
                Assert.AreEqual(user, eventItem.Owner);
                Assert.AreEqual(description, eventItem.Description);
                Assert.AreEqual(endDateTime.ToString(), eventItem.EndDateTime.ToString());
                Assert.AreEqual(name, eventItem.Title);
                Assert.AreEqual(startDateTime.ToString(), eventItem.StartDateTime.ToString());
                Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
                eventItem.Delete();
            }
        }

        [TestMethod]
        public void TestEventRetrievalPaged()
        {
            Utilities.SwitchToOwnerUser();
            User user = Utilities.OwnerUser;

            // Create new event
            string name = Constants.Strings.EventName;
            string description = Constants.Strings.EventDescription;
            DateTime startDateTime = new DateTime(2007, 10, 25, 15, 30, 30);
            DateTime endDateTime = new DateTime(2007, 10, 25, 16, 45, 0);

            int eventIndex;
            List<Event> myEvents = new List<Event>(10);

            for (eventIndex = 0; eventIndex < 10; eventIndex++)
            {
                Event anEvent = EventManager.CreateEvent(name,
                    description, startDateTime, endDateTime, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);
                myEvents.Add(anEvent);
            }

            Assert.AreEqual(10, EventManager.GetEventsForUserCount(user.UserName, startDateTime.AddHours(-1),
                endDateTime.AddHours(1), UserGroupStatus.Joined));

            // Retrieve event by creatorID
            ReadOnlyCollection<Event> events;
            
            events = EventManager.GetEventsForUser(user.UserName, startDateTime.AddHours(-1), endDateTime.AddHours(1),
                UserGroupStatus.Joined, 0, 5);
            for (eventIndex = 0; eventIndex < 5; eventIndex++)
            {
                Assert.AreEqual(myEvents[eventIndex], events[eventIndex]);
            }

            events = EventManager.GetEventsForUser(user.UserName, startDateTime.AddHours(-1), endDateTime.AddHours(1),
                UserGroupStatus.Joined, 5, 5);
            for (eventIndex = 0; eventIndex < 5; eventIndex++)
            {
                Assert.AreEqual(myEvents[eventIndex + 5], events[eventIndex]);
            }
        }

        [TestMethod]
        public void TestEventRetrievalByDateRange()
        {
            Utilities.SwitchToOwnerUser();
            User user = Utilities.OwnerUser;

            Assert.AreEqual(0, EventManager.GetEventsForUser(user.UserName, UserGroupStatus.Joined).Count);

            DateTime in23Hours = DateTime.Now.AddHours(23);
            DateTime in24Hours = DateTime.Now.AddHours(24);
            DateTime in25Hours = DateTime.Now.AddHours(25);
            DateTime in26Hours = DateTime.Now.AddHours(26);
            DateTime in27Hours = DateTime.Now.AddHours(27);

            // Create new event
            string name = Constants.Strings.EventName;
            string description = Constants.Strings.EventDescription;
            DateTime startDateTime = in24Hours;
            DateTime endDateTime = in26Hours;
            
            Event anEvent = EventManager.CreateEvent(name,
                description, startDateTime, endDateTime, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);

            // Number of events user created
            int eventsInDateRangeCount = EventManager.GetEventsForUserCount(user.UserName, in23Hours, in27Hours, UserGroupStatus.Joined);
            Assert.AreEqual(eventsInDateRangeCount, 1);

            // Retrieve event by creatorID
            ReadOnlyCollection<Event> events = EventManager.GetEventsForUser(user.UserName, in23Hours, in27Hours, UserGroupStatus.Joined, 0, eventsInDateRangeCount + 10);
            Assert.AreEqual(eventsInDateRangeCount, events.Count);

            foreach (Event eventItem in events)
            {
                Assert.AreEqual(user, eventItem.Owner);
                Assert.AreEqual(description, eventItem.Description);
                Assert.AreEqual(endDateTime.ToString(), eventItem.EndDateTime.ToString());
                Assert.AreEqual(name, eventItem.Title);
                Assert.AreEqual(startDateTime.ToString(), eventItem.StartDateTime.ToString());
                Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
            }

            Assert.AreEqual(0, EventManager.GetEventsForUserByStartDateCount(user.UserName, in25Hours, in27Hours, UserGroupStatus.Joined));
            
            int eventsInStartRangeCount = EventManager.GetEventsForUserByStartDateCount(user.UserName, in23Hours, in25Hours, UserGroupStatus.Joined);
            Assert.AreEqual(eventsInStartRangeCount, 1);

            // Retrieve event by creatorID
            events = EventManager.GetEventsForUserByStartDate(user.UserName, in23Hours, in25Hours, UserGroupStatus.Joined, 0, eventsInStartRangeCount + 10);
            Assert.AreEqual(eventsInStartRangeCount, events.Count);

            foreach (Event eventItem in events)
            {
                Assert.AreEqual(user, eventItem.Owner);
                Assert.AreEqual(description, eventItem.Description);
                Assert.AreEqual(endDateTime.ToString(), eventItem.EndDateTime.ToString());
                Assert.AreEqual(name, eventItem.Title);
                Assert.AreEqual(startDateTime.ToString(), eventItem.StartDateTime.ToString());
                Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NonOwnerDeleteEvent()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, start, end, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(Constants.Strings.EventName, eventItem.Title);
            Assert.AreEqual(Constants.Strings.EventDescription, eventItem.Description);
            Assert.AreEqual(start.ToString(), eventItem.StartDateTime.ToString());
            Assert.AreEqual(end.ToString(), eventItem.EndDateTime.ToString());
            Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
            Utilities.SwitchToNonOwnerUser();
            eventItem.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousDeleteEvent()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, start, end, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(Constants.Strings.EventName, eventItem.Title);
            Assert.AreEqual(Constants.Strings.EventDescription, eventItem.Description);
            Assert.AreEqual(start.ToString(), eventItem.StartDateTime.ToString());
            Assert.AreEqual(end.ToString(), eventItem.EndDateTime.ToString());
            Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
            Utilities.SwitchToAnonymousUser();
            eventItem.Delete();
        }

        [TestMethod]
        public void OwnerDeleteEvent()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, start, end, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(Constants.Strings.EventName, eventItem.Title);
            Assert.AreEqual(Constants.Strings.EventDescription, eventItem.Description);
            Assert.AreEqual(start.ToString(), eventItem.StartDateTime.ToString());
            Assert.AreEqual(end.ToString(), eventItem.EndDateTime.ToString());
            Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
            eventItem.Delete();
            Assert.IsFalse(EventManager.EventExists(eventItem.BaseItemID));
        }

        [TestMethod]
        public void AdminDeleteEvent()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, start, end, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(Constants.Strings.EventName, eventItem.Title);
            Assert.AreEqual(Constants.Strings.EventDescription, eventItem.Description);
            Assert.AreEqual(start.ToString(), eventItem.StartDateTime.ToString());
            Assert.AreEqual(end.ToString(), eventItem.EndDateTime.ToString());
            Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
            Utilities.SwitchToAdminUser();
            eventItem.Delete();
            Assert.IsFalse(EventManager.EventExists(eventItem.BaseItemID));
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NonOwnerUpdateEvent()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, start, end, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(Constants.Strings.EventName, eventItem.Title);
            Assert.AreEqual(Constants.Strings.EventDescription, eventItem.Description);
            Assert.AreEqual(start.ToString(), eventItem.StartDateTime.ToString());
            Assert.AreEqual(end.ToString(), eventItem.EndDateTime.ToString());
            Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
            eventItem.Title = "New event name";
            eventItem.Description = "New event description";
            start = DateTime.Now.AddDays(1);
            end = start.AddHours(2);
            Utilities.SwitchToNonOwnerUser();
            EventManager.UpdateEvent(eventItem);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousUpdateEvent()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, start, end, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(Constants.Strings.EventName, eventItem.Title);
            Assert.AreEqual(Constants.Strings.EventDescription, eventItem.Description);
            Assert.AreEqual(start.ToString(), eventItem.StartDateTime.ToString());
            Assert.AreEqual(end.ToString(), eventItem.EndDateTime.ToString());
            Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
            eventItem.Title = "New event name";
            eventItem.Description = "New event description";
            start = DateTime.Now.AddDays(1);
            end = start.AddHours(2);
            Utilities.SwitchToAnonymousUser();
            EventManager.UpdateEvent(eventItem);
        }

        [TestMethod]
        public void OwnerUpdateEvent()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, start, end, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(Constants.Strings.EventName, eventItem.Title);
            Assert.AreEqual(Constants.Strings.EventDescription, eventItem.Description);
            Assert.AreEqual(start.ToString(), eventItem.StartDateTime.ToString());
            Assert.AreEqual(end.ToString(), eventItem.EndDateTime.ToString());
            Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
            eventItem.Title = "New event name";
            eventItem.Description = "New event description";
            start = DateTime.Now.AddDays(1);
            end = start.AddHours(2);
            eventItem.ChangeEventTime(start, end);
            eventItem.Location = Location.Empty;
            EventManager.UpdateEvent(eventItem);
            Assert.AreEqual("New event name", EventManager.GetEvent(eventItem.BaseItemID).Title);
            Assert.AreEqual("New event description", EventManager.GetEvent(eventItem.BaseItemID).Description);
            Assert.AreEqual(Location.Empty, EventManager.GetEvent(eventItem.BaseItemID).Location);
            Assert.AreEqual(start.ToString(), EventManager.GetEvent(eventItem.BaseItemID).StartDateTime.ToString());
            Assert.AreEqual(end.ToString(), EventManager.GetEvent(eventItem.BaseItemID).EndDateTime.ToString());
        }

        [TestMethod]
        public void AdminUpdateEvent()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, start, end, Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(Constants.Strings.EventName, eventItem.Title);
            Assert.AreEqual(Constants.Strings.EventDescription, eventItem.Description);
            Assert.AreEqual(start.ToString(), eventItem.StartDateTime.ToString());
            Assert.AreEqual(end.ToString(), eventItem.EndDateTime.ToString());
            Assert.AreEqual(Utilities.TestLocation, eventItem.Location);
            Utilities.SwitchToAdminUser();
            eventItem.Title = "New event name";
            eventItem.Description = "New event description";
            start = DateTime.Now.AddDays(1);
            end = start.AddHours(2);
            eventItem.ChangeEventTime(start, end);
            eventItem.Location = Location.Empty;
            EventManager.UpdateEvent(eventItem);
            Assert.AreEqual("New event name", EventManager.GetEvent(eventItem.BaseItemID).Title);
            Assert.AreEqual("New event description", EventManager.GetEvent(eventItem.BaseItemID).Description);
            Assert.AreEqual(Location.Empty, EventManager.GetEvent(eventItem.BaseItemID).Location);
            Assert.AreEqual(start.ToString(), EventManager.GetEvent(eventItem.BaseItemID).StartDateTime.ToString());
            Assert.AreEqual(end.ToString(), EventManager.GetEvent(eventItem.BaseItemID).EndDateTime.ToString());
        }

        [TestMethod]
        public void AssociateEventWithGroup()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), Utilities.TestLocation, PrivacyLevel.Public, Constants.Strings.EventType);
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            group.Associate(eventItem);

            Assert.AreEqual(1, group.Events.Count);
            Assert.AreEqual(eventItem, group.Events[0]);

            group.RemoveBaseItemAssociation(eventItem, true);

            Assert.AreEqual(0, group.Events.Count);

            group.Associate(eventItem);

            Assert.AreEqual(1, group.Events.Count);
            Assert.AreEqual(eventItem, group.Events[0]);

            eventItem.Delete();

            Assert.AreEqual(0, group.Events.Count);

            group.Delete();
        }

    }
}
