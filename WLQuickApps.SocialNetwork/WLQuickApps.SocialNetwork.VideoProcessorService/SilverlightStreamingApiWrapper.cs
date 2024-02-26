using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    public class SilverlightStreamingApiWrapper
    {
        static string _serviceRoot = string.Empty;

        static SilverlightStreamingApiWrapper()
        {
            _serviceRoot = SilverlightStreamingApiSettingsWrapper.ServiceRoot;
        }

        public static string ListApplications(SilverlightStreamingAccount account)
        {
            if (account == null)
            { throw new ArgumentNullException("account"); }

            string accountId = account.Id;
            string accountKey = account.Key;

            string apiUri = _serviceRoot + account.Id;
            
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(apiUri);
            req.Credentials = new NetworkCredential(accountId, accountKey);

            return GetXmlResponse(req);
        }

        public static string AddApplication(SilverlightStreamingAccount account, string applicationName, string zipFile)
        {
            if (account == null)
            { throw new ArgumentNullException("account"); }
            if (applicationName == null)
            { throw new ArgumentNullException("applicationName"); }
            if (zipFile == null)
            { throw new ArgumentNullException("zipFile"); }

            string accountId = account.Id;
            string accountKey = account.Key;

            string apiUri = _serviceRoot + account.Id + "/" + applicationName;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(apiUri);
            req.Credentials = new NetworkCredential(accountId, accountKey);

            req.Method = "POST";
            req.ContentType = "application/zip";
            
            // without updating, the following timeout properties will default to
            // 300 seconds and 100 seconds respectively (not enough for large file
            // uploads).
            int timeoutInMinutes = Constants.MediaUploadTimeout * 1000 * 60;
            req.Timeout = timeoutInMinutes;
            req.ReadWriteTimeout = timeoutInMinutes;

            using (FileStream fs = new FileStream(zipFile, FileMode.Open, FileAccess.Read))
            {
                req.ContentLength = fs.Length;

                Stream reqStr = req.GetRequestStream();

                StreamUtils.StreamCopy(fs, reqStr);
            }

            return GetXmlResponse(req);
        }

        public static string DeleteApplication(SilverlightStreamingAccount account, string applicationName)
        {
            if (account == null)
            { throw new ArgumentNullException("account"); }
            if (applicationName == null)
            { throw new ArgumentNullException("applicationName"); }

            string accountId = account.Id;
            string accountKey = account.Key;

            string apiUri = _serviceRoot + account.Id + "/" + applicationName;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(apiUri);
            req.Credentials = new NetworkCredential(accountId, accountKey);
            req.Method = "DELETE";

            return GetXmlResponse(req);
        }

        public static string ListApplicationFiles(SilverlightStreamingAccount account, string applicationName)
        {
            if (account == null)
            { throw new ArgumentNullException("account"); }
            if (applicationName == null)
            { throw new ArgumentNullException("applicationName"); }

            string accountId = account.Id;
            string accountKey = account.Key;

            string apiUri = _serviceRoot + account.Id + "/" + applicationName;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(apiUri);
            req.Credentials = new NetworkCredential(accountId, accountKey);

            return GetXmlResponse(req);
        }

        private static string GetXmlResponse(HttpWebRequest req)
        {
            if (req == null)
            { throw new ArgumentNullException("req"); }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream strm = resp.GetResponseStream();

            StreamReader rdr = new StreamReader(strm);

            string xmlResponse = rdr.ReadToEnd();
            return xmlResponse;
        }
    }
}
