using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using WLQuickApps.SocialNetwork.Data;
using System.Security;

namespace WLQuickApps.SocialNetwork.Business
{
    /// <summary>
    /// Represents a user album.
    /// </summary>
    public class SearchResults
    {
        public ReadOnlyCollection<Media> Pictures
        {
            get { return this._pictures.AsReadOnly(); }
        }
        private List<Media> _pictures;

        public ReadOnlyCollection<Media> Audios
        {
            get { return this._audios.AsReadOnly(); }
        }
        private List<Media> _audios;

        public ReadOnlyCollection<Media> Videos
        {
            get { return this._videos.AsReadOnly(); }
        }
        private List<Media> _videos;

        public ReadOnlyCollection<Media> Files
        {
            get { return this._files.AsReadOnly(); }
        }
        private List<Media> _files;

        public ReadOnlyCollection<Forum> Forums
        {
            get { return this._forums.AsReadOnly(); }
        }
        private List<Forum> _forums;

        public ReadOnlyCollection<Event> Events
        {
            get
            {
                this._events.Sort();
                return this._events.AsReadOnly();
            }
        }
        private List<Event> _events;

        public ReadOnlyCollection<User> Users
        {
            get { return this._users.AsReadOnly(); }
        }
        private List<User> _users;

        public ReadOnlyCollection<Group> Groups
        {
            get { return this._groups.AsReadOnly(); }
        }
        private List<Group> _groups;

        public ReadOnlyCollection<Collection> Collections
        {
            get { return this._collections.AsReadOnly(); }
        }
        private List<Collection> _collections;

        public ReadOnlyCollection<Event> GetSpecialEvents(string type)
        {
            if (!this._specialEvents.ContainsKey(type.ToUpper()))
            {
                return (new List<Event>()).AsReadOnly();
            }
            return this._specialEvents[type.ToUpper()].AsReadOnly();
        }
        private Dictionary<string, List<Event>> _specialEvents;

        public ReadOnlyCollection<Group> GetSpecialGroups(string type)
        {
            if (!this._specialGroups.ContainsKey(type.ToUpper()))
            {
                return (new List<Group>()).AsReadOnly();
            }
            return this._specialGroups[type.ToUpper()].AsReadOnly();
        }
        private Dictionary<string, List<Group>> _specialGroups;

        internal SearchResults()
        {
            this._audios = new List<Media>();
            this._events = new List<Event>();
            this._files = new List<Media>();
            this._groups = new List<Group>();
            this._collections = new List<Collection>();
            this._pictures = new List<Media>();
            this._users = new List<User>();
            this._videos = new List<Media>();
            this._forums = new List<Forum>();

            this._specialEvents = new Dictionary<string, List<Event>>();
            this._specialGroups = new Dictionary<string, List<Group>>();
        }

        public void AddMedia(int baseItemID)
        {
            Media media;

            try
            {
                media = MediaManager.GetMedia(baseItemID);
            }
            catch (SecurityException)
            {
                return;
            }

            if (!media.CanView) { return; }

            switch (media.MediaType)
            {
                case MediaType.Picture: this._pictures.Add(media); break;
                case MediaType.Audio: this._audios.Add(media); break;
                case MediaType.Video: this._videos.Add(media); break;
            }
        }

        public void AddGroup(int baseItemID)
        {
            Group group;

            try
            {
                group = BaseItemManager.GetBaseItem(baseItemID) as Group;
            }
            catch (SecurityException)
            {
                return;
            }

            if (!group.CanView) { return; }

            if (group.SubType == string.Empty)
            {
                if (group is Event)
                {
                    this._events.Add(EventManager.GetEvent(baseItemID));
                }
                else
                {
                    this._groups.Add(group);
                }
            }
            else
            {
                string key = group.SubType.ToUpper();

                // If this type is already associated with a group, put it in the right group bucket.
                if (this._specialGroups.ContainsKey(key))
                {
                    this._specialGroups[key].Add(group);
                }
                else
                {
                    // If this type wasn't associated with a group, try to get it as an event. If it isn't an event,
                    // create a group bucket and add it there. While this is a bit of a hack, each event type miss only
                    // happens once per search, so it shouldn't be bad (unless there are a ton of custom groups).
                    Event eventItem;
                    try
                    {
                        eventItem = EventManager.GetEvent(baseItemID);
                        if (!this._specialEvents.ContainsKey(key))
                        {
                            this._specialEvents.Add(key, new List<Event>());
                        }
                        this._specialEvents[key].Add(eventItem);
                    }
                    catch
                    {
                        this._specialGroups.Add(key, new List<Group>());
                        this._specialGroups[key].Add(group);
                    }
                }
            }
        }

        public void AddUser(int baseItemID)
        {
            User user;

            try
            {
                user = UserManager.GetUserByBaseItemID(baseItemID);
            }
            catch (SecurityException)
            {
                return;
            }

            if (!user.CanView) { return; }
            this._users.Add(user);
        }

        public void AddForum(int baseItemID)
        {
            Forum forum;

            try
            {
                forum = ForumManager.GetForum(baseItemID);
            }
            catch (SecurityException)
            {
                return;
            }

            if (!forum.CanView) { return; }
            this._forums.Add(forum);
        }

        public void AddCollection(int baseItemID)
        {
            Collection collection;

            try
            {
                collection = CollectionManager.GetCollection(baseItemID);
            }
            catch (SecurityException)
            {
                return;
            }

            if (!collection.CanView) { return; }
            this._collections.Add(collection);
        }

    }
}
