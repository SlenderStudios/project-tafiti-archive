using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

public partial class Video : System.Web.UI.Page
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
            // Check for authenticated user
            if (User.Identity.IsAuthenticated)
            {
                UploadPanel.Visible = true;
            }
            else
            {
                UploadPanel.Visible = false;
            }

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
                    var data = from fileSet in document.Descendants("fileSet")
                               where fileSet.Attribute("isApp") != null && fileSet.Attribute("isApp").Value == "true"
                               orderby fileSet.Attribute("name").Value descending
                               let fileSetName = fileSet.Attribute("name").Value
                               let manifest = GetManifest(fileSetName)
                               select new
                               {
                                   Title = manifest.MediaData.Title,
                                   Description = manifest.MediaData.Description,
                                   Thumbnail = string.Format("Video/ImageHandler.ashx?image={0}/{1}/thumbnail.jpg", accountId, fileSetName),
                                   MediaSource = string.Format("streaming:/{0}/{1}/video.wmv", accountId, fileSetName),
                                   FileSet = fileSetName,
                               };

                    if (data.Count() > 0)
                        MediaPlayer.MediaSource = data.First().MediaSource;

                    FileSetList.DataSource = data;
                    FileSetList.DataBind();
                }
            }
            catch (Exception)
            {
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

    protected void FileSetList_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            try
            {
                //ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                //DataKey dataKey = FileSetList.DataKeys[currentItem.DataItemIndex];

                // Get the FileSet data key
                //string fileSet = dataKey["FileSet"].ToString();
                string fileSet = (string)e.CommandArgument;

                // Construct the request URI
                string uri = string.Format("https://silverlight.services.live.com/{0}/{1}", accountId, fileSet);

                // Authorization header
                byte[] userPass = Encoding.Default.GetBytes(accountId + ":" + accountKey);

                // Create a WebClient and add the basic authentication header
                WebClient client = new WebClient();
                client.Headers["Authorization"] = "Basic " + Convert.ToBase64String(userPass);

                // Delete the fileset
                client.UploadString(uri, "DELETE", "");

                // Redirect to back to page (avoids further postbacks)
                Response.Redirect("~/Video.aspx");
            }
            catch (Exception)
            {
            }
        }
    }

    protected void UploadButton_Click(object sender, EventArgs e)
    {
        if (FileUpload.HasFile)
        {
            // Generate a unique file set name
            //string fileSet = Guid.NewGuid().ToString();

            // Use current date time instead for sorting most recent
            string fileSet = DateTime.Now.Ticks.ToString();

            // Save the file to a temp folder
            string tempFolder = Server.MapPath("~/App_Data/SilverlightStreaming/");
            string fileSetFolder = tempFolder + fileSet + "/";
            string fileName = fileSetFolder + "video.wmv";

            // Create the required folders if they do not exist
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            if (!Directory.Exists(fileSetFolder))
                Directory.CreateDirectory(fileSetFolder);

            // Save the uploaded file locally
            FileUpload.SaveAs(fileName);

            // Create a zip file to package the video file
            string zipFileName = Path.ChangeExtension(fileName, ".zip");

            using (Package package = Package.Open(zipFileName, FileMode.Create))
            {
                // Add the video file to the package
                AddFileToPackage(package, fileName, "video/x-ms-wmv");

                // Create manifest with source=dummy.xaml
                // See: http://msdn2.microsoft.com/en-us/library/bb802532.aspx
                // We also include user data to be used by this application
                XNamespace ns = "http://tempuri.org/Media";
                XDocument manifest =
                    new XDocument(new XElement("SilverlightApp",
                        new XAttribute(XNamespace.Xmlns + "media", ns.NamespaceName),
                        new XElement("source", new XText("dummy.xaml")),
                        new XElement(ns + "mediaData",
                            new XElement(ns + "title", new XText(TitleTextBox.Text)),
                            new XElement(ns + "description", new XText(DescTextBox.Text)),
                            new XElement(ns + "tags", new XText("")))));

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Write manifest to memory stream
                    manifest.Save(new StreamWriter(memoryStream));

                    // Reposition memory stream
                    memoryStream.Position = 0;

                    // Add the manifest to the package
                    AddStreamToPackage(package, memoryStream, "manifest.xml", "text/xml");
                }

                // Add the thumbnail if one was selected
                if (ThumbUpload.HasFile)
                {
                    // Load image
                    System.Drawing.Image original =
                        System.Drawing.Image.FromStream(ThumbUpload.FileContent);

                    // Convert to thumbnail size (100x75)
                    System.Drawing.Image.GetThumbnailImageAbort thumbCallback =
                        new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                    System.Drawing.Image thumbnail =
                        original.GetThumbnailImage(100, 75, thumbCallback, IntPtr.Zero);

                    // Save thumbnail as Jpeg
                    string thumbFileName = fileSetFolder + "thumbnail.jpg";
                    thumbnail.Save(thumbFileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                    // Add the thumbnail file to the package
                    AddFileToPackage(package, thumbFileName, "image/jpeg");
                }
                else
                {
                    // Use a default thumbnail
                    string thumbFileName = "~/Images/thumbnail.jpg";
                    AddFileToPackage(package, Server.MapPath(thumbFileName), "image/jpeg");
                }
            }

            // Construct the request URI with file set info
            string uri = string.Format("https://silverlight.services.live.com/{0}/{1}", accountId, fileSet);

            // Authorization header
            byte[] userPass = Encoding.Default.GetBytes(accountId + ":" + accountKey);

            // Create a WebClient and add the basic authentication header
            WebClient client = new WebClient();
            client.Headers["Authorization"] = "Basic " + Convert.ToBase64String(userPass);
            client.Headers["Content-Type"] = "application/zip";

            try
            {
                client.UploadData(uri, File.ReadAllBytes(zipFileName));
            }
            catch (Exception)
            {
            }
            finally
            {
                Directory.Delete(fileSetFolder, true);
                Response.Redirect("~/Video.aspx");
            }
        }
    }

    public bool ThumbnailCallback()
    {
        return false;
    }

    private void AddFileToPackage(Package package, string fileName, string contentType)
    {
        // Copy the file to the package
        using (Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            AddStreamToPackage(package, fileStream, fileName, contentType);
        }
    }

    private void AddStreamToPackage(Package package, Stream fileStream, string fileName, string contentType)
    {
        // Construct a uri for the file being added
        string partName = ".\\" + Path.GetFileName(fileName);
        Uri partUri = PackUriHelper.CreatePartUri(new Uri(partName, UriKind.Relative));

        // Create a package part to add to the zip file
        PackagePart packagePart = package.CreatePart(partUri, contentType);

        // Copy the stream to the package
        using (Stream partStream = packagePart.GetStream())
        {
            CopyStream(fileStream, partStream);
        }
    }

    private void CopyStream(Stream input, Stream output)
    {
        const int size = 4096;
        byte[] bytes = new byte[4096];
        int numBytes;
        while ((numBytes = input.Read(bytes, 0, size)) > 0)
            output.Write(bytes, 0, numBytes);
    }
}
