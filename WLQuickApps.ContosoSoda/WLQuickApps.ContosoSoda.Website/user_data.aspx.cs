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
    static Video videoHandler = new Video();
    protected string user_xml, video_xml;
    protected void Page_Load(object sender, EventArgs e)
    {
        user_xml = userHandler.GetUserList2();
        string[] video_info = new string[6];
        string[] user_info = new string[5];
        video_info = videoHandler.GetVideoTemp();
        user_info = userHandler.GetUsers(video_info[5]);
        string im_id = user_info[2];
       // video_xml = "<info id=\"" +  + "\" votes=\"" +  + "\" date=\"" +  + "\" vlink=\"vote/vote.aspx?id=" + video_info[0] + "\" link=\"chat.aspx?invitee=" + im_id + "\" icon=\"http://messenger.services.live.com/users/" + im_id + "/presenceimage?mkt=en-hk\">" +  + "</info>";
        video_xml = "'" + video_info[1] + "', '" + video_info[2] + "', '" + video_info[3] + "', '" + video_info[4] + "'";
    }
}
