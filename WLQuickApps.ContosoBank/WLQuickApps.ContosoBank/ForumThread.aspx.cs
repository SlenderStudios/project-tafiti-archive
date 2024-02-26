/*****************************************************************************
 * ForumThread.aspx
 * Notes: Page to display supplied forum thread
 * **************************************************************************/

using System;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank
{
    public partial class ForumThread : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && Request.QueryString["ID"] != null)
            {
                ReplyForumReply.BindData(ForumLogic.GetReply(Convert.ToInt32(Request.QueryString["ID"])));
            }
        }
    }
}