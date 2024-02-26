using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.TestSuite.Properties;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    internal class Utilities
    {
        static internal Location TestLocation
        {
            get
            {
                return LocationManager.CreateLocation("WLQuickApps", "16625 Redmond Way", "Suite M PMB 206", "Redmond", "WA", "USA", "98052", string.Empty);
            }
        }

        static internal Location TestSearchLocation
        {
            get
            {
                Utilities.SwitchToAdminUser();
                return LocationManager.CreateLocation("WLQuickApps", "16625 Redmond Way", "Suite M PMB 206", "Redmond", "WA", "USA", "98052", "UnitTestLocation");
            }
        }

        static internal byte[] TestPictureBits
        {
            get
            {
                if (Utilities._testPictureBits == null)
                {
                    Utilities._testPictureBits =
                        Business.Utilities.ConvertImageToBytes(Resources.TestPicture);
                }
                return Utilities._testPictureBits;
            }
        }
        static private byte[] _testPictureBits;

        static internal byte[] TestPictureBits2
        {
            get
            {
                if (Utilities._testPictureBits2 == null)
                {
                    Utilities._testPictureBits2 =
                        Business.Utilities.ConvertImageToBytes(Resources.TestPicture2);
                }
                return Utilities._testPictureBits2;
            }
        }
        static private byte[] _testPictureBits2;

        static internal byte[] TestPictureBitsBad
        {
            get
            {
                if (Utilities._testPictureBitsBad == null)
                {
                    Utilities._testPictureBitsBad =
                        Resources.bad;
                }
                return Utilities._testPictureBitsBad;
            }
        }
        static private byte[] _testPictureBitsBad;

        static internal byte[] TestAudioBits
        {
            get
            {
                if (Utilities._testAudioBits == null)
                {
                    Utilities._testAudioBits = Resources.TestAudio;
                }
                return Utilities._testAudioBits;
            }
        }
        static private byte[] _testAudioBits;

        static internal byte[] TestVideoBits
        {
            get
            {
                if (Utilities._testVideoBits == null)
                {
                    Utilities._testVideoBits = Resources.TestVideo;
                }
                return Utilities._testVideoBits;
            }
        }
        static private byte[] _testVideoBits;

        static internal void SwitchToAdminUser()
        {
            if (Utilities._adminPrincipal == null)
            {
                Utilities._adminPrincipal =
                    new GenericPrincipal(
                        new FormsIdentity(
                            new FormsAuthenticationTicket("TestAdministrator", false, 0)), null);
            }
            Thread.CurrentPrincipal = Utilities._adminPrincipal;
        }

        static internal void SwitchToCustomUser(string userName)
        {
            Thread.CurrentPrincipal =
                    new GenericPrincipal(
                        new FormsIdentity(
                            new FormsAuthenticationTicket(userName, false, 0)), null);
        }


        static internal User AdminUser
        {
            get
            {
                Utilities.SwitchToAdminUser();
                return UserManager.LoggedInUser;
            }
        }
        static private IPrincipal _adminPrincipal;

        static internal void SwitchToOwnerUser()
        {
            if (Utilities._ownerPrincipal == null)
            {
                Utilities._ownerPrincipal =
                    new GenericPrincipal(
                        new FormsIdentity(
                            new FormsAuthenticationTicket("TestOwnerUser", false, 0)), null);
            }
            Thread.CurrentPrincipal = Utilities._ownerPrincipal;
        }

        static internal User OwnerUser
        {
            get
            {
                Utilities.SwitchToOwnerUser();
                return UserManager.LoggedInUser;
            }
            set
            {
                Utilities._ownerPrincipal = 
                    new GenericPrincipal(
                        new FormsIdentity(
                            new FormsAuthenticationTicket(value.UserName, false, 0)), null);
            }
        }
        static private IPrincipal _ownerPrincipal;

        static internal void SwitchToNonOwnerUser()
        {
            if (Utilities._nonOwnerPrincipal == null)
            {
                Utilities._nonOwnerPrincipal =
                    new GenericPrincipal(
                        new FormsIdentity(
                            new FormsAuthenticationTicket("TestNonOwnerUser", false, 0)), null);
            }
            Thread.CurrentPrincipal = Utilities._nonOwnerPrincipal;
        }

        static internal User NonOwnerUser
        {
            get
            {
                Utilities.SwitchToNonOwnerUser();
                return UserManager.LoggedInUser;
            }
            set
            {
                Utilities._nonOwnerPrincipal =
                    new GenericPrincipal(
                        new FormsIdentity(
                            new FormsAuthenticationTicket(value.UserName, false, 0)), null);
            }
        }
        static private IPrincipal _nonOwnerPrincipal;

        static internal void SwitchToAnonymousUser()
        {
            Thread.CurrentPrincipal = null;
        }

        static internal User AnonymousUser
        {
            get
            {
                Utilities.SwitchToAnonymousUser();
                return UserManager.LoggedInUser;
            }
        }

        static internal bool ByteArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1.LongLength != array2.LongLength)
            {
                return false;
            }

            for (long i = 0; i < array1.LongLength; i++)
            {

                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        internal static bool ImagesEqual(Image image1, Image image2)
        {
            if (image1.Width != image2.Width)
            {
                return false;
            }
            if (image1.Height != image2.Height)
            {
                return false;
            }

            return true;
            //ImageConverter converter = new ImageConverter();

            //byte[] bytes1 = new byte[1];
            //byte[] bytes2 = new byte[1];

            //bytes1 = (byte[])converter.ConvertTo(image1, bytes1.GetType());
            //bytes2 = (byte[])converter.ConvertTo(image2, bytes2.GetType());

            //return ByteArraysEqual(bytes1, bytes2);
        }

        private static void DeleteAllAlbumsForCurrentUser()
        {
            foreach (Album album in AlbumManager.GetAllAlbums())
            {
                album.Delete();
            }
        }

        private static void DeleteAllCollectionsForCurrentUser()
        {
            foreach (Collection collection in CollectionManager.GetCollectionsByUser(UserManager.LoggedInUser.UserName, Constants.Strings.CollectionType))
            {
                collection.Delete();
            }
        }

        private static void DeleteAllEventsForCurrentUser()
        {
            foreach (Event eventItem in EventManager.GetEventsForUser(UserManager.LoggedInUser.UserName, UserGroupStatus.Joined))
            {
                eventItem.Delete();
            }
        }

        private static void DeleteAllFriendsForCurrentUser()
        {
            foreach (User friend in FriendManager.GetFriends())
            {
                FriendManager.RemoveFriendship(friend);
            }

            foreach (User friend in FriendManager.GetPendingFriendInvitations())
            {
                FriendManager.RemoveFriendship(friend);
            }

            foreach (User friend in FriendManager.GetPendingFriendRequests())
            {
                FriendManager.RemoveFriendship(friend);
            }
        }

        internal static void DeleteAllAlbumsForTestUsers()
        {
            Utilities.SwitchToAdminUser();
            DeleteAllAlbumsForCurrentUser();

            Utilities.SwitchToOwnerUser();
            DeleteAllAlbumsForCurrentUser();

            Utilities.SwitchToNonOwnerUser();
            DeleteAllAlbumsForCurrentUser();
        }

        internal static void DeleteAllCollectionsForTestUsers()
        {
            Utilities.SwitchToAdminUser();
            Utilities.DeleteAllCollectionsForCurrentUser();

            Utilities.SwitchToNonOwnerUser();
            Utilities.DeleteAllCollectionsForCurrentUser();

            Utilities.SwitchToOwnerUser();
            Utilities.DeleteAllCollectionsForCurrentUser();
        }

        internal static void DeleteAllEventsForTestUsers()
        {
            Utilities.SwitchToAdminUser();
            DeleteAllEventsForCurrentUser();

            Utilities.SwitchToOwnerUser();
            DeleteAllEventsForCurrentUser();

            Utilities.SwitchToNonOwnerUser();
            DeleteAllEventsForCurrentUser();
        }

        internal static void DeleteAllFriendsForTestUsers()
        {
            Utilities.SwitchToAdminUser();
            Utilities.DeleteAllFriendsForCurrentUser();

            Utilities.SwitchToNonOwnerUser();
            Utilities.DeleteAllFriendsForCurrentUser();

            Utilities.SwitchToOwnerUser();
            Utilities.DeleteAllFriendsForCurrentUser();
        }

    }
}
