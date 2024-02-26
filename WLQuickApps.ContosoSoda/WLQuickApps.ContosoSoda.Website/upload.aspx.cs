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

public partial class Default2 : System.Web.UI.Page
{
    static User userHandler = new User();
    static Video VideoHandler = new Video();

    protected string Name, PostDate, Desc,Im, header_img;
    protected bool submited, no_user_found = false;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        string[] info = new string[5];
        DateTime d = DateTime.Now;
         //check WL KEY
        if (Session["WlKey"] == null)
        {
           Session["Refer_link"] = "upload.aspx";
           Response.Redirect("~/login.aspx",false);
        }
        else
        {
      
            info = userHandler.GetUsers(Convert.ToString(Session["WlKey"]));
            if (info[0] == "no_user_found")
            {
/*
                if (Request.QueryString["id"] == null)
                {
                    Response.Redirect("http://settings.messenger.live.com/applications/websignup.aspx?returnurl=http://www.dq.hk:8080/upload.aspx&privacyurl=http://www.dq.hk:8080/join/privacy.htm");
                }
                else
                {
                    TextBox_IM_ID.Text = Request.QueryString["id"];
                }
             */
            
            }
          
  
        }
    }

    protected void UploadButton_Click(object sender, EventArgs e)
    {
        //Demo only, no need upload real file.
        /*
         //String savePath = @"C:\Documents and Settings\Administrator\My Documents\Visual Studio 2008\WebSites\WebSite2\upload\";
        String savePath = @"e:\";
              //Get the name of the file to upload.
             String fileName = FileUpload1.FileName;
            
             savePath += fileName;

             FileUpload1.SaveAs(savePath);
         */
        DateTime d = DateTime.Now;
            Name = TextBox_User.Text;
            PostDate = string.Format("{0:yyyy/MM/dd/ HH:mm:ss}", d);
            Desc = TextBox_Desc.Text;
            Im = TextBox_IM_ID.Text;
            VideoHandler.AddInfo(Name, Convert.ToString(Session["WlKey"]), "0", PostDate, Desc, "fileName", Im);
            submited = true;
        
         
    }
}
