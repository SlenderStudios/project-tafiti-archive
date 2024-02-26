using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class AvatarLogic
    {
        public static List<Avatar> GetAvatars()
        {
            if (HttpContext.Current.Cache["Avatars"] == null)
            {
                XDocument avatarsXML = XDocument.Load(getAvatarLocation());
                var temp = from feed in avatarsXML.Descendants("Avatar")
                           orderby feed.Element("Name").Value
                           select new Avatar
                                      {
                                          ID = Convert.ToInt32(feed.Element("ID").Value),
                                          Name = feed.Element("Name").Value,
                                          Location = feed.Element("Location").Value,
                                          IsDefault = Convert.ToBoolean(feed.Element("IsDefault").Value),
                                      };

                HttpContext.Current.Cache.Add("Avatars", temp.ToList(), new CacheDependency(getAvatarLocation()),
                                              Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0),
                                              CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["Avatars"] as List<Avatar>;
        }

        public static Avatar GetDefaultAvatar()
        {
            if (HttpContext.Current.Cache["DefaultAvatar"] == null)
            {
                XDocument avatarsXML = XDocument.Load(getAvatarLocation());
                var temp = from feed in avatarsXML.Descendants("Avatar")
                           where Convert.ToBoolean(feed.Element("IsDefault").Value)
                           select new Avatar
                                      {
                                          ID = Convert.ToInt32(feed.Element("ID").Value),
                                          Name = feed.Element("Name").Value,
                                          Location = feed.Element("Location").Value,
                                          IsDefault = Convert.ToBoolean(feed.Element("IsDefault").Value),
                                      };

                HttpContext.Current.Cache.Add("DefaultAvatar", temp.ToList(), new CacheDependency(getAvatarLocation()),
                                              Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0),
                                              CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["DefaultAvatar"] as Avatar;
        }


        public static Avatar GetAvatarByLocation(string location)
        {
            XDocument avatarsXML = XDocument.Load(getAvatarLocation());
            var temp = from feed in avatarsXML.Descendants("Avatar")
                       where feed.Element("Location").Value == location
                       select new Avatar
                                  {
                                      ID = Convert.ToInt32(feed.Element("ID").Value),
                                      Name = feed.Element("Name").Value,
                                      Location = feed.Element("Location").Value,
                                      IsDefault = Convert.ToBoolean(feed.Element("IsDefault").Value),
                                  };

            return temp.FirstOrDefault();
        }

        public static Avatar GetAvatarByID(int id)
        {
            XDocument avatarsXML = XDocument.Load(getAvatarLocation());
            var temp = from feed in avatarsXML.Descendants("Avatar")
                       where Convert.ToInt32(feed.Element("ID").Value) == id
                       select new Avatar
                       {
                           ID = Convert.ToInt32(feed.Element("ID").Value),
                           Name = feed.Element("Name").Value,
                           Location = feed.Element("Location").Value,
                           IsDefault = Convert.ToBoolean(feed.Element("IsDefault").Value),
                       };

            return temp.FirstOrDefault();
        }

        private static string getAvatarLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\Avatars.xml";
        }
    }

    /// <summary>
    /// Public interface to allow the setting of the data object on the user control
    /// </summary>
    public interface IAvatarData
    {
        Avatar Item { get; set; }
    }
}