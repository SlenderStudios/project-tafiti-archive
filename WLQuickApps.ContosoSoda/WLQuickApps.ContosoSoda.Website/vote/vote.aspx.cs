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
using System.Xml;
using System.Web.Mail;
using VideoManagement;

public partial class Default2 : System.Web.UI.Page
{
    static Video VideoHandler = new Video();
    protected bool submited = false;
    protected string fd_list;
    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (Request.QueryString["fd_list"] != null)
        {
            TextBox_fd.Text = Request.QueryString["fd_list"];
        }

        if (TextBox_fd.Text != null && TextBox_fd.Text != "" && TextBox_name.Text != null && TextBox_name.Text != "" & TextBox_mail.Text != "" & TextBox_mail.Text.IndexOf('@') != -1 & TextBox_mail.Text.IndexOf('.') != -1)
        {
            MailMessage Message = new MailMessage();
            fd_list = TextBox_fd.Text;
            Message.To = TextBox_fd.Text;
            Message.From = TextBox_mail.Text;
            Message.Subject = "Check out this video and we could go to Beijing";
            Message.Body = TextBox_msg.Text;
            SmtpMail.SmtpServer = "smtp2.incnets.com";
            SmtpMail.Send(Message);
            submited = true;
        }
        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "") 
            {
                VideoHandler.Vote(Request.QueryString["id"]);
            }

        TextBox_msg.Text = "Here's a cool site with lots of amazing videos:\r\n";
        TextBox_msg.Text += "http://www.kcptest.com/default.aspx?id=" + Request.QueryString["id"] + "\r\n";
        TextBox_msg.Text += "You can vote for your favorite to win a trip to Beijing. Check it out.";
    }
}
