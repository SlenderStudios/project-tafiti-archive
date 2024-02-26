using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections;
using System.Web;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;

namespace WLQuickApps.ContosoBicycleClub.UI
{
    public partial class WindowsLiveLogin
    {
       
        public static string GetWindowsLiveCID()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            else
            {
                return (HttpContext.Current.Session["WindowsLiveCID"] as string);
            }
        }

        public static bool IsUserAuthenticated()
        {
            return (!string.IsNullOrEmpty(WindowsLiveLogin.GetWindowsLiveCID()));
        }

        public string GetOnlinePresenceInvitationUrl(string returnurl, string privacyurl)
        {           
            return string.Format("http://settings.messenger.live.com/applications/websignup.aspx?returnurl={0}&privacyurl={1}", returnurl, privacyurl);
        }
      
    }
}
