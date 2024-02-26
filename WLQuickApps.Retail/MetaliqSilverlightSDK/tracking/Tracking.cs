using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MetaliqSilverlightSDK.tracking.tags;
using MetaliqSilverlightSDK.net;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Net;
using System.Windows.Browser;

namespace MetaliqSilverlightSDK.tracking
{
    class Nested
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nested()
        {
        }

        internal static readonly TrackingInstance instance = new TrackingInstance();
    }
    class TrackingInstance : Tracking
    { 
    }

    public enum TrackingType
    {
        WebTrends,
        Atlas,
        Generic
    }
    public abstract class Tracking : XMLLoader
    {
        protected List<WebTrendsTag> WebtrendsTags;
        protected List<AtlasTag> AtlasTags;
        protected static string _XML;
        public static string XML
        {
            set
            {
                _XML = MetaliqSilverlightSDK.net.NetUtil.ToAbsoluteUri(value).ToString();
                // Load XML
                Nested.instance.Load(_XML);
            }
            get
            {
                return _XML;
            }
        }
        protected override void ParseXML(XmlReader Reader)
        {
            //   - create WebTrendsTag objects
            //   - load webtrendstag objects into collection
            //   - dispatch event signalling completion
            try
            {
                AtlasTags = new List<AtlasTag>();
                WebtrendsTags = new List<WebTrendsTag>();
                //HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                //HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                //if (response.StatusCode != HttpStatusCode.OK)
                //    throw new Exception("HttpStatusCode " +
                //      response.StatusCode.ToString() + " was returned.");

                //StreamReader responseReader = new StreamReader(response.GetResponseStream());

                //string rawResponse = responseReader.ReadToEnd();

                //response.Close();
                //XmlReader Reader = XmlReader.Create(new StringReader(rawResponse));

                while (Reader.Read())
                {
                    if (Reader.LocalName.ToLower() == "tracking")
                    {
                        using (XmlReader TrackingTagReader = Reader.ReadSubtree())
                        {
                            while (TrackingTagReader.Read())
                            {
                                if (TrackingTagReader.LocalName.ToLower() == "atlas")
                                {
                                    AtlasTag Tag = new AtlasTag(Int32.Parse(TrackingTagReader.GetAttribute("id")), 
                                        TrackingTagReader.GetAttribute("description"),
                                        TrackingTagReader.GetAttribute("action"),
                                        TrackingTagReader.GetAttribute("actionTag"),
                                        TrackingTagReader.GetAttribute("imageCode"));
                                    AtlasTags.Add(Tag);
                                }
                                if (TrackingTagReader.LocalName.ToLower() == "webtrends")
                                {
                                    WebTrendsTag Tag = new WebTrendsTag(Int32.Parse(TrackingTagReader.GetAttribute("id")),
                                        TrackingTagReader.GetAttribute("description"),
                                        TrackingTagReader.GetAttribute("dcsuri"),
                                        TrackingTagReader.GetAttribute("ti"),
                                        TrackingTagReader.GetAttribute("cg_n"),
                                        TrackingTagReader.GetAttribute("dac"),
                                        TrackingTagReader.GetAttribute("customTag"),
                                        TrackingTagReader.GetAttribute("customTagValue"));
                                    WebtrendsTags.Add(Tag);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Exception ex = e;
            }
        }
        public BaseTrackingTag GetTag(int TagId, TrackingType type)
        {
            BaseTrackingTag result = null;
            if (type == TrackingType.WebTrends)
            {
                result = WebtrendsTags[TagId];
            }
            else if (type == TrackingType.Atlas)
            {
                result = AtlasTags[TagId];
            }

            return result;
        }
        protected static void OnTrackResult(IAsyncResult result)
        { 
        }
        public static void Track(int TagId, TrackingType type)
        {
            Track(Nested.instance.GetTag(TagId-1, type));
        }
        public static void Track(BaseTrackingTag Tag)
        {
            if (_XML == null)
            {
                return;
            }
            if(Tag is WebTrendsTag)
            {
                string foo;
                if (Tag.Id == 4)
                {
                     foo = "Bar";
                }
                WebTrendsTag WebTrends = (WebTrendsTag)Tag;
                string[] parameters = WebTrends.customTag == String.Empty || WebTrends.customTagValue == String.Empty
                                   ? new string[] { "WT.ti", WebTrends.ti,
                                                    "WT.dl", "6",
                                                    "WT.cg_n", WebTrends.cg_n,
                                                    "DCS.dcsuri",  WebTrends.dcsuri,
                                                    "DCSext.dac", WebTrends.dac
                                                  } 
                                   : new string[] { "WT.ti", WebTrends.ti,
                                                    "WT.dl", "6",
                                                    "WT.cg_n", WebTrends.cg_n,
                                                    "DCS.dcsuri",  WebTrends.dcsuri,
                                                    WebTrends.customTag, WebTrends.customTagValue
                                                  };
                try
                {
                    ScriptObject trackRequest = HtmlPage.Window.CreateInstance("track", parameters);
                }
                catch(Exception retard)
                {
                }
            }
            else if(Tag is AtlasTag)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(NetUtil.ToAbsoluteUri(((AtlasTag)Tag).ImageCode));
                request.BeginGetResponse(new AsyncCallback(OnTrackResult), Nested.instance);
            }
        }
        protected static BaseTrackingTag GetTagById(int TagId, TrackingType type)
        {
            return Nested.instance.GetTag(TagId, type);
        }

    }
}
