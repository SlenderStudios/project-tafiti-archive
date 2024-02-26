using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.IO;
using System.Text;
namespace WLQuickApps.SocialNetwork.Business
{
    public class Album2
    {
        string text;
        string value;
        string caption;

        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public Album2(string tex, string val, string cap)
        {
            text = tex;
            value = val;
            caption = cap;
        }

    }
    public class PhotoAlbum
    {
        NameTable nt;
        XmlNamespaceManager xmlnsm;
        string ownerHandle = "";
        string domainAuthToken = "";
        string strRoot = "";
        //string responseCode = "";
        public PhotoAlbum(string Handle, string AuthToken)
        {
            ownerHandle = Handle;
            domainAuthToken = AuthToken;

            nt = new NameTable();
            xmlnsm = null;
            strRoot = "https://cumulus.services.live.com/" + ownerHandle + "/SpacesPhotos/";
            xmlnsm = new XmlNamespaceManager(nt);
            xmlnsm.AddNamespace("D", "DAV:");
            xmlnsm.AddNamespace("c", "http://storage.msn.com/DAV/");
        }

       

        public string GetCaption(string path)
        {
            XmlDocument xmld = null;
            HttpWebRequest request = MakeRequest(path, "PROPFIND");
            request.Headers.Add("Depth", "0");
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                xmld = new XmlDocument();
                xmld.LoadXml((new StreamReader(response.GetResponseStream()).ReadToEnd()).ToString());
                response.Close();
            }
            catch
            {
                return "";
            }
            if (null != xmld)
            {
                XmlNodeList xmlnl = xmld.SelectNodes("/D:multistatus/D:response/D:propstat/D:prop", xmlnsm);
                foreach (XmlNode xmlnode in xmlnl)
                {
                    XmlNode capnode = xmlnode.SelectSingleNode("c:Caption", xmlnsm);
                    if (capnode.LastChild != null) return capnode.LastChild.Value;
                }
            }
            return "";

        }


        public List<Album2> ListAlbum(ref string Error)
        {
            return GetPartlist(strRoot, ref Error);
        }

        public bool AddAlbum(string name, ref string Error)
        {
            HttpWebRequest request = MakeRequest(strRoot + "/" + name, "MKCOL");
            bool status = false;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.Created) status = true;
                response.Close();
            }
            catch (WebException ex)
            {
                Error = ex.Message;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return status;
        }

        public bool AddPhoto(string path, string name, HttpPostedFile file, ref string Error)
        {
            HttpWebRequest request = MakeRequest(path + "/" + name, "PUT");
            bool status = false;
            try
            {
                using (Stream newStream = request.GetRequestStream())
                {
                    CopyStream(file.InputStream, newStream);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.Created) status = true;
                response.Close();
            }
            catch (WebException ex)
            {
                Error = ex.Message;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return status;
        }

        public bool AddPhoto(string path, string name, byte[] bits, ref string Error)
        {
            HttpWebRequest request = MakeRequest(path + "/" + name, "PUT");
            bool status = false;
            try
            {
                using (Stream newStream = request.GetRequestStream())
                {
                    newStream.Write(bits, 0, bits.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.Created) status = true;
                response.Close();
            }
            catch (WebException ex)
            {
                Error = ex.Message;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return status;
        }


        private void CopyStream(Stream istream, Stream ostream)
        {
            byte[] buffer = new byte[2048];
            int bytes;
            while (0 < (bytes = istream.Read(buffer, 0, buffer.Length)))
                ostream.Write(buffer, 0, bytes);
        }

        public bool UpdateAlbum(string path, string name, ref string Error)
        {
            return UpdateItem(path, name, ref Error);
        }

        public string VerifyAddAlbum(string aname, ref string Error)
        {
            List<Album2> lst = ListAlbum(ref Error);
            bool flag = false;
            if (lst == null)
            {
                if (AddAlbum(aname, ref Error)) return strRoot + "/" + aname;
                return string.Empty;
            }
            for (int index = 0; index < lst.Count; index++)
            {
                if (lst[index].Text == aname) { flag = true; break; }
            }
            if (flag) return strRoot + "/" + aname;
            if (AddAlbum(aname, ref Error)) return strRoot + "/" + aname;
            return string.Empty;

        }


        public string VerifyAddPhoto(string path, string name, byte[] bits, ref string Error)
        {
            List<Album2> lst = ListPhoto(path, ref Error);
            bool flag = false;
            if (lst == null)
            {
                if (AddPhoto(path, name, bits, ref Error)) return path + "/" + name;
                return string.Empty;
            }
            for (int index = 0; index < lst.Count; index++)
            {
                if (lst[index].Text == name) { flag = true; break; }
            }
            if (flag) return path + "/" + name;
            if (AddPhoto(path, name, bits, ref Error)) return path + "/" + name;
            return string.Empty;
        }

        public bool UpdatePhoto(string path, string name, ref string Error)
        {
            return UpdateItem(path, name, ref Error);
        }

        public static bool UpdatePhoto(string path, string name, string domainAuthToken)
        {
            string strCaption = "<c:Caption>" + name + "</c:Caption>";
            string updateXml = "<D:propertyupdate xmlns:D=\"DAV:\" xmlns:c=\"http://storage.msn.com/DAV/\"><D:set><D:prop>" + strCaption + "</D:prop></D:set></D:propertyupdate>";

            // Create the request object.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            request.UserAgent = "Microsoft Windows Live Photo Demo";
            request.Method = "PROPPATCH";
            request.Headers.Add("Authorization", "DomainAuthToken at=\"" + domainAuthToken + "\"");

            //Copy the XML to the request stream.
            request.ContentType = "text/xml";
            request.ContentLength = updateXml.Length;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                writer.Write(updateXml);

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch { }
            return false;
        }

        public static bool DeletePhoto(string path, string domainAuthToken)
        {
            bool status = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
                request.UserAgent = "Microsoft Windows Live Photo Demo";
                request.Method = "DELETE";
                request.Headers.Add("Authorization", "DomainAuthToken at=\"" + domainAuthToken + "\"");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.NoContent) status = true;
                response.Close();
            }
            catch { }
            return status;
        }

        private bool UpdateItem(string url, string name, ref string Error)
        {
            string path = "";
            string strCaption = "<c:Caption>" + name + "</c:Caption>";
            path = url;
            string updateXml = "<D:propertyupdate xmlns:D=\"DAV:\" xmlns:c=\"http://storage.msn.com/DAV/\"><D:set><D:prop>" + strCaption + "</D:prop></D:set></D:propertyupdate>";

            HttpWebRequest request = MakeRequest(url, "PROPPATCH");
            //Copy the XML to the request stream.
            request.ContentType = "text/xml";
            request.ContentLength = updateXml.Length;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                writer.Write(updateXml);

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode < HttpStatusCode.MultipleChoices) return true; // 207 Not available
            }
            catch (WebException ex)
            {
                //The HTTP request failed, display what happened.
                Error = "Request failed - " + Environment.NewLine + ex.Message;
            }
            return false;
        }

        public bool DeleteAlbum(string name, ref string Error)
        {
            return DeleteItem(name, ref Error);
        }

        public bool DeletePhoto(string name, ref string Error)
        {
            return DeleteItem(name, ref Error);
        }

        private bool DeleteItem(string name, ref string Error)
        {
            HttpWebRequest request = MakeRequest(name, "DELETE");
            bool status = false;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.NoContent) status = true;
                response.Close();
            }
            catch (WebException ex)
            {
                Error = ex.Message;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return status;
        }

        public List<Album2> ListPhoto(string AlbumPath, ref string Error)
        {
            return GetPartlist(AlbumPath, ref Error);
        }

        private List<Album2> GetPartlist(string path, ref string Error)
        {
            List<Album2> photoAlbums = new List<Album2>();
            XmlDocument xmld = null;
            HttpWebRequest request = MakeRequest(path, "PROPFIND");
            request.Headers.Add("Depth", "1");
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                xmld = new XmlDocument();
                xmld.LoadXml((new StreamReader(response.GetResponseStream()).ReadToEnd()).ToString());
                response.Close();
            }
            catch (WebException ex)
            {
                Error = ex.Message;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            if (null != xmld)
            {
                XmlNodeList xmlnl = xmld.SelectNodes("/D:multistatus/D:response/D:propstat/D:prop", xmlnsm);
                bool first = true;
                foreach (XmlNode xmlnode in xmlnl)
                {
                    if (first)
                    {
                        // Ignore first item
                        first = false;
                        continue;
                    }

                    XmlNode href = xmlnode.SelectSingleNode("D:href", xmlnsm);
                    XmlNode name = xmlnode.SelectSingleNode("D:displayname", xmlnsm);
                    XmlNode capnode = xmlnode.SelectSingleNode("c:Caption", xmlnsm);
                    string caption = "";
                    if (capnode.LastChild != null) caption = capnode.LastChild.Value;
                    photoAlbums.Add(new Album2(name.LastChild.Value, href.LastChild.Value, caption));
                }
            }
            return photoAlbums;

        }


        private HttpWebRequest MakeRequest(string reqPath, string method)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(reqPath);
            request.UserAgent = "Microsoft Windows Live Photo Demo";
            request.Method = method;
            request.Headers.Add("Authorization", "DomainAuthToken at=\"" + domainAuthToken + "\"");
            return request;
        }



       
    }
}
