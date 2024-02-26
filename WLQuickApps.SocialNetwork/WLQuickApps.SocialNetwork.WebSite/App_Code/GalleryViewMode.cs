using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WLQuickApps.SocialNetwork.WebSite
{
    /// <summary>
    /// Defines possible views for the MetaGalleryControl
    /// </summary>
    public enum GalleryViewMode
    {
        Square = 0,
        List = 1,
        Icon = 2,
        Album = 3,
        Event = 4,
        Group = 5,
        Media = 6,
        User = 7,
        Text = 8,
        Thumbnail = 9,
        Mobile = 10,
    }
}

