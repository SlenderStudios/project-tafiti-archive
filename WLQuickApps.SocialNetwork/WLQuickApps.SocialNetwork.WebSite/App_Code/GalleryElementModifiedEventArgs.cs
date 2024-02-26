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
    public class GalleryElementModifiedEventArgs : EventArgs
    {
        static public new GalleryElementModifiedEventArgs Empty;

        static GalleryElementModifiedEventArgs()
        {
            GalleryElementModifiedEventArgs.Empty = new GalleryElementModifiedEventArgs();
        }

        private GalleryElementModifiedEventArgs()
            : base()
        {
        }
    }
}
