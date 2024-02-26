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
using WindowsLive;
using System.Web.Configuration;
namespace ContosoUniversity
{
    public partial class Default : System.Web.UI.Page
    {
        protected string AppId = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            // Setup the contacts control
            if (Session["ConsentToken"] != null)
            {
                ContactsControl1.ConsentToken = (WindowsLiveLogin.ConsentToken)Session["ConsentToken"];
                ContactsControl1.ShowMapLinks = false;
            }
            AppId = WebConfigurationManager.AppSettings["wll_appid"];
            pnlWhereAretheyNow.Visible = (Request.Cookies["webauthtoken"] != null);

        }
    }
}
