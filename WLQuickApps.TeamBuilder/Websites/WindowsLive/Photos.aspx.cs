using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using WindowsLive;

public partial class Photos : System.Web.UI.Page
{
    string _lid; // [Location ID]
    long _llid;
    string _delegationToken; // [Delegation Token]

    static XNamespace Atom = "http://www.w3.org/2005/Atom";
    static XNamespace LP = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
    static XNamespace LivePhotos = "http://dev.live.com/photos";

    protected void Page_Load(object sender, EventArgs e)
    {
        // Check for authenticated user
        if (User.Identity.IsAuthenticated)
        {
            UploadPanel.Visible = true;
        }
        else
        {
            UploadPanel.Visible = false;
        }

        // Get the lid and delegation token.
        // We have saved the consent token to appSettings, all we 
        // need to do is extract the values from the consent token.
        WindowsLiveLogin.ConsentToken ct = Utilities.GetAppConsentToken();

        if (ct != null && ct.IsValid())
        {
            // Get the data values.            
            _lid = ct.LocationID;
            _llid = Int64.Parse(_lid, System.Globalization.NumberStyles.HexNumber);
            _delegationToken = ct.DelegationToken;

            if (!IsPostBack)
            {
                try
                {

                    // Construct the request URI
                    string uri = string.Format("https://cumulus.services.live.com/@C@{0}/AtomSpacesPhotos/Folders", _lid);

                    // Create a WebClient and add the delegation token to the header
                    WebClient client = new WebClient();
                    client.Headers["Authorization"] = "DelegatedToken dt=\"" + _delegationToken + "\"";

                    XDocument document = null;

                    if (Cache["AlbumsList"] != null)
                    {
                        document = (XDocument)Cache["AlbumsList"];
                    }
                    else
                    {
                        XmlReader reader;
                        
                        reader = XmlReader.Create(client.OpenRead(uri));
                        
                        document = XDocument.Load(reader);
                        
                        Cache.Insert("AlbumsList", document);
                    }

                    var data = from entry in document.Descendants(Atom + "entry")
                               let related = (from link in entry.Elements(Atom + "link")
                                              where link.Attribute("rel").Value == "related"
                                              select link).First()
                               orderby DateTime.Parse(entry.Element(Atom + "updated").Value) descending
                               select new
                               {
                                   Title = entry.Element(Atom + "title").Value,
                                   Link = related.Attribute("href").Value
                               };

                    AlbumDropDown.DataSource = data;
                    AlbumDropDown.DataBind();
                }
                catch (Exception)
                {
                }
            }
        }
    }

    protected void UploadButton_Click(object sender, EventArgs e)
    {
        if (PhotoUpload.HasFile)
        {
            // Get the select album
            string uri = AlbumDropDown.SelectedValue;

            // Load image and convert to Jpeg
            System.Drawing.Image image =
                System.Drawing.Image.FromStream(PhotoUpload.FileContent);

            // Create a WebClient and add the delegation token to the header
            WebClient client = new WebClient();
            client.Headers["Authorization"] = "DelegatedToken dt=\"" + _delegationToken + "\"";
            client.Headers["Content-Type"] = "image/jpeg";
            client.Headers["Slug"] = PhotoUpload.FileName;

            try
            {
                // Open a stream to write the image to
                Stream stream = client.OpenWrite(uri);

                // Save image to stream
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Close stream and release its resources
                stream.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                Response.Redirect("~/Photos.aspx");
            }
        }
    }
}
