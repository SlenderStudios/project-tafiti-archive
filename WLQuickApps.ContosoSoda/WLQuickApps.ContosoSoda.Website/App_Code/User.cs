using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
/// <summary>
/// Summary description for User
/// </summary>
namespace UserManagement
{
    public class User
    {

        protected string file_name = ConfigurationManager.AppSettings["XmlPath"] + "user_data.xml";
        public User()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //
        public string GetUserList()
        {
            string rs_text ="";
            int count, total_user;
            DataSet dsUsers = new DataSet();
            dsUsers.ReadXml(file_name);

            DataView dvUsers = new DataView(dsUsers.Tables["User"]);
            dvUsers.Sort = "ID desc";
            total_user = dvUsers.Count;
            if (total_user > 4){
                total_user = 4;
            }
            for (count = 0; count < total_user; count++)
            {

                rs_text = rs_text + "<item id=\"" + dvUsers[count]["Name"].ToString() + "\" link=\"chat.aspx?invitee=" + dvUsers[count]["ImId"].ToString() + "\" icon=\"http://messenger.services.live.com/users/" + dvUsers[count]["ImId"].ToString() + "/presenceimage?mkt=en-hk\">" + dvUsers[count]["Subject"].ToString() + "</item>";
            }
            
            return rs_text;
        }

        public string GetUserList2()
        {
            string team_link = "";
            string team_subject = "";
            string team_message = "";
            string team_owner = "";
            string owner_icon = "";
            string rs_text = "";
            int count, total_user;
            DataSet dsUsers = new DataSet();
            dsUsers.ReadXml(file_name);

            DataView dvUsers = new DataView(dsUsers.Tables["User"]);
            dvUsers.Sort = "ID desc";
            total_user = dvUsers.Count;
            if (total_user > 4)
            {
                total_user = 4;
            }

            for (count = 0; count < total_user; count++)
            {
                team_owner = "'" + dvUsers[count]["Name"].ToString() + "', ";
                owner_icon = "'" + "chat.aspx?invitee=" + dvUsers[count]["ImId"].ToString() + "', ";
                team_link = "'" + "http://messenger.services.live.com/users/" + dvUsers[count]["ImId"].ToString() + "/presenceimage?mkt=en-hk" + "', ";
                team_subject = "'" + dvUsers[count]["Subject"].ToString() + "', ";
                team_message = "'" + dvUsers[count]["Msg"].ToString() + "'";


                rs_text = rs_text + "[";
                rs_text = rs_text + team_subject + team_owner + owner_icon + team_link + team_message;
                rs_text = rs_text + "],"+"\n";

            }

            return rs_text;
        }
        public DataView GetUserList3()
        {
            int count, total_user;
            DataSet dsUsers = new DataSet();
            dsUsers.ReadXml(file_name);

            DataView dvUsers = new DataView(dsUsers.Tables["User"]);

            return dvUsers;
        }
        public string[] GetUsers(string WlKey)
        {
            string[] user_info = new string[5];
            DataSet dsUsers = new DataSet();
            dsUsers.ReadXml(file_name);

            DataView dvUsers = new DataView(dsUsers.Tables["User"]);
            dvUsers.Sort = "WlKey";

            int rowIndex = dvUsers.Find(WlKey);

            if (rowIndex == -1)
            {
                user_info[0] = "no_user_found";
                //no user find
            }
            else
            {
                user_info[0] = dvUsers[rowIndex]["Name"].ToString();
                user_info[1] = dvUsers[rowIndex]["WlKey"].ToString();
                user_info[2] = dvUsers[rowIndex]["ImId"].ToString();
                user_info[3] = dvUsers[rowIndex]["Subject"].ToString();
                user_info[4] = dvUsers[rowIndex]["Msg"].ToString();
            }
            return user_info;
        }

        public bool AddInfo(string name, string imId, string wlKey, string subject, string msg)
        {
            DataSet dsUsers = new DataSet();
            dsUsers.ReadXml(file_name);

            DataView dvUsers = new DataView(dsUsers.Tables["User"]);
            int userId = dvUsers.Count + 1;

              XmlDocument xmlDoc=new XmlDocument();
              xmlDoc.Load(file_name);
              XmlNode root = xmlDoc.SelectSingleNode("Users");
              XmlElement xe1 = xmlDoc.CreateElement("User");

              xe1.SetAttribute("id", "0");
              XmlElement xesub6 = xmlDoc.CreateElement("ID");
              xesub6.InnerText = userId.ToString();
              xe1.AppendChild(xesub6);

              XmlElement xesub1 = xmlDoc.CreateElement("Name");
              xesub1.InnerText = name;
              xe1.AppendChild(xesub1);

              XmlElement xesub2 = xmlDoc.CreateElement("WlKey");
              xesub2.InnerText = wlKey;
              xe1.AppendChild(xesub2);

              XmlElement xesub3 = xmlDoc.CreateElement("ImId");
              xesub3.InnerText = imId;
              xe1.AppendChild(xesub3);
              root.AppendChild(xe1);

              XmlElement xesub4 = xmlDoc.CreateElement("Subject");
              xesub4.InnerText = subject;
              xe1.AppendChild(xesub4);
              root.AppendChild(xe1);

              XmlElement xesub5 = xmlDoc.CreateElement("Msg");
              xesub5.InnerText = msg;
              xe1.AppendChild(xesub5);
              root.AppendChild(xe1);

              xmlDoc.Save(file_name);

            return true;
        }
        public void Reset()
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file_name);
            XmlNode root = xmlDoc.DocumentElement;

            //Remove all attribute and child nodes.
            root.RemoveAll();

            xmlDoc.Save(file_name);

            if (AddInfo("Stephen", "bcb7942db041e607@apps.messenger.live.com", "d0e02a8c03d6cce87b6dc498178a1a83", "Most people in a Photo Sticker Booth", "Most people in a Photo Sticker Booth"))
            {
                //success
            }
            if (AddInfo("Thomas", "bcb7942db041e607@apps.messenger.live.com", "d0e02a8c03d6cce87b6dc498178a1a83", "Longest Line of People to Pass a Ping Pong Ball on Spoons", "Longest Line of People to Pass a Ping Pong Ball on Spoons"))
            {
                //success
            }
            if (AddInfo("Elvis", "bcb7942db041e607@apps.messenger.live.com", "d0e02a8c03d6cce87b6dc498178a1a83", "Longest Line of People to Pass a Ping Pong Ball on Spoons", "Longest Line of People to Pass a Ping Pong Ball on Spoons"))
            {
                //success
            }
            if (AddInfo("Helen", "bcb7942db041e607@apps.messenger.live.com", "d0e02a8c03d6cce87b6dc498178a1a83", "Most people in a Photo Sticker Booth", "Most people in a Photo Sticker Booth"))
            {
                //success
            }
        }
    }
}
