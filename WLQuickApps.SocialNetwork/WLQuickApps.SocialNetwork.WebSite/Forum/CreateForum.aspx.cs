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
using System.Net;
using System.Drawing;
using System.IO;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Forum_CreateForum : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (SettingsWrapper.SpecialForums.Length > 0)
            {
                this._forumPanel.Visible = true;
                foreach (string forum in SettingsWrapper.SpecialForums)
                {
                    this._forumDropDownList.Items.Add(new ListItem(forum, forum));
                }
            }
        }
    }

    protected void _createForum_Click(object sender, EventArgs e)
    {
        Forum forum = ForumManager.CreateForum(this._nameTextBox.Text, this._forumDropDownList.SelectedValue);
        Response.Redirect(WebUtilities.GetViewItemUrl(forum));
    }
}
