using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

public partial class Controls_LatestVideoControl : System.Web.UI.UserControl
{
    public class Manifest
    {
        public Manifest()
        {
            MediaData = new MediaData();
        }

        public MediaData MediaData { get; set; }
    }

    public class MediaData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
    }

    static XNamespace Media = "http://tempuri.org/Media";

    string accountId = "";
    string accountKey = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection settings = ConfigurationManager.AppSettings;
        accountId = settings["silverlight_accountid"];
        accountKey = settings["silverlight_accountkey"];

        if (!IsPostBack)
        {
            // Construct the request URI
            string uri = string.Format("https://silverlight.services.live.com/{0}", accountId);

            // Authorization header
            byte[] userPass = Encoding.Default.GetBytes(accountId + ":" + accountKey);

            // Create a WebClient and add the basic authentication header
            WebClient client = new WebClient();
            client.Headers["Authorization"] = "Basic " + Convert.ToBase64String(userPass);

            try
            {
                using (XmlReader reader = XmlReader.Create(client.OpenRead(uri)))
                {
                    XDocument document = XDocument.Load(reader);

                    
                    //HACK: Better validation of when the fileset is empty
                    if (document.ToString() != "<fileSets />")
                    {
                        var data = (from fileSet in document.Descendants("fileSet")
                                    where fileSet.Attribute("isApp") != null && fileSet.Attribute("isApp").Value == "true"
                                    orderby fileSet.Attribute("name").Value descending
                                    let fileSetName = fileSet.Attribute("name").Value
                                    let manifest = GetManifest(fileSetName)
                                    select new
                                    {
                                        Title = manifest.MediaData.Title,
                                        Description = manifest.MediaData.Description,
                                        Thumbnail = string.Format("~/Video/ImageHandler.ashx?image={0}/{1}/thumbnail.jpg", accountId, fileSetName),
                                    }).First();
                        
                        if (data != null)
                        {
                            ThumbnailImage.ImageUrl = data.Thumbnail;
                            Title.Text = data.Title;
                            Description.Text = data.Description;
                        }
                    }

                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get the latest video from Silverlight Streaming.", ex);
            }
        }
    }

    private Manifest GetManifest(string fileSet)
    {
        // Construct the request URI
        string uri = string.Format("https://silverlight.services.live.com/{0}/{1}/manifest.xml", accountId, fileSet);

        // Authorization header
        byte[] userPass = Encoding.Default.GetBytes(accountId + ":" + accountKey);

        // Create a WebClient and add the basic authentication header
        WebClient client = new WebClient();
        client.Headers["Authorization"] = "Basic " + Convert.ToBase64String(userPass);

        using (XmlReader reader = XmlReader.Create(client.OpenRead(uri)))
        {
            XDocument document = XDocument.Load(reader);

            // Get the mediaData element and parse it
            XElement data = document.Descendants(Media + "mediaData").First();
            XElement title = data.Element(Media + "title");
            XElement description = data.Element(Media + "description");
            XElement tags = data.Element(Media + "tags");

            // Create a Manifest object
            Manifest manifest = new Manifest();
            manifest.MediaData.Title = title.Value;
            manifest.MediaData.Description = description.Value;
            manifest.MediaData.Tags = tags.Value;

            // Return
            return manifest;
        }
    }
}
