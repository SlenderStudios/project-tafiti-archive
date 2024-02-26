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

namespace ContosoUniversity.UserControls
{
    public partial class LiveSearch : System.Web.UI.UserControl
    {
    // There are issues with the Live Search javasript when running in debug mode in Visual Studio
    // This mechanism simply hides the html when debugging with a local web server (Cassini)
        protected void Page_Load(object sender, EventArgs e)
        {
            bool local = Request.Url.AbsoluteUri.Contains("http://localhost");
            Panel1.Visible = !local;
            Label1.Visible = local;
        }
    }
}