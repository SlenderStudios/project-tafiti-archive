using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using UserManagement;
using VideoManagement;

public partial class Reset : System.Web.UI.Page
{
    static User userHandler = new User();
    static Video videoHandler = new Video();
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        userHandler.Reset();
        videoHandler.Reset();
        Response.Redirect("default.aspx");
    }
}
