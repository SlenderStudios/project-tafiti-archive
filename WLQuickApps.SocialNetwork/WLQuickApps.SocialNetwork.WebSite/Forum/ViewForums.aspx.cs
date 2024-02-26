using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WLQuickApps.SocialNetwork.Business;
using AjaxControlToolkit;
using System.IO;
using System.Drawing.Imaging;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Forum_ViewForums : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) 
    {        
    }

    protected void _specialForumTopics_Load(object sender, EventArgs e)
    {
        DataList _specialForumTopics = (DataList)sender;

        _specialForumTopics.DataSource = SettingsWrapper.SpecialForums;
        _specialForumTopics.DataBind();
    }

}
