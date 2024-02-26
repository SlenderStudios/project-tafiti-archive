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
/// Summary description for Video
/// </summary>
namespace VideoManagement
{
    public class Video
    {
        protected string file_name = ConfigurationManager.AppSettings["XmlPath"] + "video_data.xml";
        public Video()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string[] GetVideoTemp()
        {
            string[] video_info = new string[7];
            DataSet dsVideos = new DataSet();
            dsVideos.ReadXml(file_name);
           
            DataView dvVideos = new DataView(dsVideos.Tables["Video"]);
            dvVideos.Sort = "PostDate DESC";

            video_info[0] = dvVideos[0]["ID"].ToString();
            video_info[1] = dvVideos[0]["DisplayName"].ToString();
            video_info[2] = dvVideos[0]["Votes"].ToString();
            video_info[3] = dvVideos[0]["PostDate"].ToString();
            video_info[4] = dvVideos[0]["Desc"].ToString();
            video_info[5] = dvVideos[0]["UserID"].ToString();
            video_info[6] = dvVideos[0]["Im"].ToString();
            return video_info;
        }

        public DataView GetVideos(string ID)
        {
            DataSet dsVideos = new DataSet();
            dsVideos.ReadXml(file_name);

            DataView dvVideos = new DataView(dsVideos.Tables["Video"]);
            dvVideos.Sort = "WlKey";

            int rowIndex = dvVideos.Find(ID);

            if (rowIndex == -1)
            {
                //no Video find
            }
            else
            {

            }
            return dvVideos;
        }

        public void Vote(string ID)
        {

            DataSet dsVideos = new DataSet();
            dsVideos.ReadXml(file_name);

            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = dsVideos.Tables["Video"].Columns["ID"];
            dsVideos.Tables["Video"].PrimaryKey = PrimaryKeyColumns;
            DataRow dr = dsVideos.Tables["Video"].Rows.Find(ID);
            DataGrid DataGrid1 = new DataGrid();
            if (  dr == null)
            {

            }
            else
            {
                dr["Votes"] = Convert.ToString((Convert.ToInt32(dr["Votes"]) + 1));
DataGrid1.DataSource = dsVideos;
DataGrid1.DataBind();
dsVideos.WriteXml(file_name); 
            }
         
        }

        public bool AddInfo(string name, string usID, string votes, string postDate, string desc, string path, string im)
        {
            DataSet dsVideos = new DataSet();
            dsVideos.ReadXml(file_name);

            DataView dvVideos = new DataView(dsVideos.Tables["Video"]);
            int videoId = dvVideos.Count + 1;
              XmlDocument xmlDoc=new XmlDocument();
              xmlDoc.Load(file_name);
              XmlNode root = xmlDoc.SelectSingleNode("Videos");
              XmlElement xe1 = xmlDoc.CreateElement("Video");
              xe1.SetAttribute("id", "0");

              XmlElement xesub7 = xmlDoc.CreateElement("ID");
              xesub7.InnerText = videoId.ToString();
              xe1.AppendChild(xesub7);

              XmlElement xesub6 = xmlDoc.CreateElement("DisplayName");
              xesub6.InnerText = name;
              xe1.AppendChild(xesub6);

              XmlElement xesub1 = xmlDoc.CreateElement("UserID");
              xesub1.InnerText = usID;
              xe1.AppendChild(xesub1);

              XmlElement xesub2 = xmlDoc.CreateElement("Votes");
              xesub2.InnerText = votes;
              xe1.AppendChild(xesub2);

              XmlElement xesub3 = xmlDoc.CreateElement("PostDate");
              xesub3.InnerText = postDate;
              xe1.AppendChild(xesub3);

              XmlElement xesub4 = xmlDoc.CreateElement("Desc");
              xesub4.InnerText = desc;
              xe1.AppendChild(xesub4);

              XmlElement xesub5 = xmlDoc.CreateElement("Path");
              xesub5.InnerText = path;
              xe1.AppendChild(xesub5);

              XmlElement xesub8 = xmlDoc.CreateElement("Im");
              xesub8.InnerText = im;
              xe1.AppendChild(xesub8);

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

            if (AddInfo("billy", "d0e02a8c03d6cce87b6dc498178a1a83", "5510", "2008/03/13/ 16:10:31", "Longest Line of People to Pass a Ping Pong Ball on Spoons", "video6.wmv","null"))
            {
                //success
            }
        }
    }
}
