<%@ WebHandler Language="C#" Class="DataHandler" %>

using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WindowsLive;

public class DataHandler : IHttpHandler
{
    [XmlRoot(ElementName = "data")]
    public class Data
    {
        [XmlAttribute(AttributeName = "startAlbumIndex")]
        public int StartAlbumIndex { get; set; }
        [XmlAttribute(AttributeName = "transition")]
        public string Transition { get; set; }
        [XmlElement(ElementName = "album")]
        public Album[] Albums { get; set; }
    }

    [XmlRoot(ElementName = "album")]
    public class Album
    {
        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "image")]
        public string Image { get; set; }
        [XmlElement(ElementName = "slide")]
        public Slide[] Slides { get; set; }
    }

    [XmlRoot(ElementName = "slide")]
    public class Slide
    {
        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "image")]
        public string Image { get; set; }
        [XmlAttribute(AttributeName = "thumbnail")]
        public string Thumbnail { get; set; }
    }

    string _lid; // [Location ID]
    long _llid;
    string _delegationToken; // [Delegation Token]

    static XNamespace Atom = "http://www.w3.org/2005/Atom";
    static XNamespace LP = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
    static XNamespace LivePhotos = "http://dev.live.com/photos";

    public void ProcessRequest(HttpContext context)
    {
        // Construct a Slide.Show data file in xml
        Data data = new Data();

        try
        {
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

                // Construct the request URI
                string uri = string.Format("https://cumulus.services.live.com/@C@{0}/AtomSpacesPhotos/Folders", _lid);

                // Add the albums to the data object
                data.Albums = GetAlbums(uri);
            }
        }
        catch (Exception)
        {
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // Return an empty data object to avoid JavaScript errors
        }
        finally
        {
            // Serialize data to xml
            XmlSerializer serializer = new XmlSerializer(typeof(Data));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // No namespace

            // Ouptut serialized data
            context.Response.ContentType = "text/xml";
            serializer.Serialize(context.Response.Output, data, ns);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private Album[] GetAlbums(string url)
    {
        string albumCacheKey = "Albums:" + url;
        if (HttpContext.Current.Cache[albumCacheKey] != null)
        {
            return (Album[])HttpContext.Current.Cache[albumCacheKey];
        }
        else
        {
            // Create a WebClient and add the delegation token to the header
            WebClient client = new WebClient();
            client.Headers["Authorization"] = "DelegatedToken dt=\"" + _delegationToken + "\"";

            using (XmlReader reader = XmlReader.Create(client.OpenRead(url)))
            {
                XDocument document = XDocument.Load(reader);
                Album[] _albums = (from entry in document.Descendants(Atom + "entry")
                                   let related = (from link in entry.Elements(Atom + "link")
                                                  where link.Attribute("rel").Value == "related"
                                                  select link).First()
                                   orderby DateTime.Parse(entry.Element(Atom + "updated").Value) descending
                                   select new Album
                                   {
                                       Title = entry.Element(Atom + "title").Value,
                                       Slides = GetSlides(related.Attribute("href").Value)
                                   }).ToArray<Album>();

                HttpContext.Current.Cache.Insert(albumCacheKey, _albums);

                return (_albums);
            }
        }
    }

    private Slide[] GetSlides(string url)
    {
         string slidesCacheKey = "Slides:" + url;
        
         if (HttpContext.Current.Cache[slidesCacheKey] != null)
         {
             return (Slide[])HttpContext.Current.Cache[slidesCacheKey];
         }
         else
         {
             // Create a WebClient and add the delegation token to the header
             WebClient client = new WebClient();
             client.Headers["Authorization"] = "DelegatedToken dt=\"" + _delegationToken + "\"";

             using (XmlReader reader = XmlReader.Create(client.OpenRead(url)))
             {
                 XDocument document = XDocument.Load(reader);
                 Slide[] _slides = (from entry in document.Descendants(Atom + "entry")
                                    let related = (from link in entry.Elements(Atom + "link")
                                                   where link.Attribute("rel").Value == "related"
                                                   select link).First()
                                    let handler = "../Photos/ImageHandler.ashx?url=" + related.Attribute("href").Value
                                    select new Slide
                                    {
                                        Title = entry.Element(Atom + "title").Value,
                                        Image = handler + "(1)/$value", // WebReady stream = 1
                                        Thumbnail = handler + "(2)/$value" // Thumbnail stream = 2
                                    }).ToArray<Slide>();

                 HttpContext.Current.Cache.Insert(slidesCacheKey, _slides);

                 return (_slides);
             }
         }
    }
}