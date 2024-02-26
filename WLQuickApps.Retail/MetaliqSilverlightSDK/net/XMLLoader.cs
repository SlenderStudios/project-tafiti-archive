using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Browser;
using System.Net;
using System.Xml;
using System.IO;

namespace MetaliqSilverlightSDK.net
{
    public class XMLLoader
    {
        public event EventHandler Error;
        public event EventHandler Loaded;
        public string m_proxyType = "aspx";
        protected bool m_isLoaded = false;
        protected bool m_isLoading = false;
        protected string m_initUrl;

        public XMLLoader()
        {
            string bar = "foo";
        }
        public void Load(Uri uri)
        {
            Load(uri.ToString());
        }
        public void Load(string uri)
        {
            try
            {
                m_isLoading = true;
                m_isLoaded = false;

                uri = HttpUtility.HtmlEncode(uri);
                m_initUrl = uri;
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(NetUtil.ToAbsoluteUri(m_initUrl));
                string foo = "Bar";
                //IAsyncResult result = request.BeginGetResponse(new AsyncCallback(OnXMLSuccess), request);

                WebClient client = new WebClient();
                client.DownloadStringAsync(new Uri(HtmlPage.Document.DocumentUri, "ClientBin/xmlFiles/allData.xml"));
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            }
            catch (Exception ex)
            {
                //error handling
                Error(this, null);
            }
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            m_isLoaded = true;
            m_isLoading = false;
            XmlReader reader = XmlReader.Create(new StringReader(e.Result));
            ParseXML(reader);

            if (Loaded != null)
            {
                Loaded(this, null);
            }
        }

        protected void OnXmlError(object sender, EventArgs e)
        {
            m_isLoading = false;
        }

        protected void OnXMLSuccess(IAsyncResult result)
        {
            m_isLoaded = true;
            m_isLoading = false;
            ParseXML(result);

            if (Loaded != null)
            {
                Loaded(this, null);
            }
        }
        protected virtual void ParseXML(XmlReader Reader)
        { }
        protected virtual void ParseXML(IAsyncResult result)
        {
        }
        public string initUrl
        {
            get { return m_initUrl; }
        }
        public string proxyType
        {
            get { return m_proxyType; }
            set { m_proxyType = value; }
        }
        public bool isLoaded
        {
            get { return m_isLoaded; }
        }
        public bool isLoading
        {
            get { return m_isLoading; }
        }
    }
}
