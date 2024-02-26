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

public partial class Default2 : System.Web.UI.Page
{
    static User userHandler = new User();
    protected string Name, IM_ID, WlKey, Subject, Desc;
    protected string[] info = new string[5];

    protected bool submited = false;
    protected void Page_Load(object sender, EventArgs e)
    {
  
        //Handle Form POST
        if (TextBox_Name.Text != null && TextBox_Name.Text != "")
        {
            Name = TextBox_Name.Text;
            WlKey = TextBox_WlKey.Text;
            IM_ID = TextBox_IM_ID.Text;
            Subject = TextBox_sub.Text;
            Desc = TextBox_desc.Text;

            if (userHandler.AddInfo(Name, IM_ID, WlKey, Subject, Desc))
            {
                submited = true;
            }
        }

        //check WL KEY
        if (Session["WlKey"] == null)
        {
           Session["Refer_link"] = "join/join.aspx";
           Response.Redirect("~/login.aspx",false);
        }
        else
        {
          

                if (Request.QueryString["id"] == null)
                {

                    Response.Redirect("http://settings.messenger.live.com/applications/websignup.aspx?returnurl=http://www.kcptest.com/join/join.aspx&privacyurl=http://www.kcptest.com/join/privacy.htm");
                }
                else
                {
                    TextBox_WlKey.Text = Convert.ToString(Session["WlKey"]);
                    TextBox_IM_ID.Text = Request.QueryString["id"];

                }
          
        }
        
    }


}
