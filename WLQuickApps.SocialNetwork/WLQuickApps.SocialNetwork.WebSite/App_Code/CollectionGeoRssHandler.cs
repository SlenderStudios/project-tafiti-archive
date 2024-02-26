using System;
using System.Web;
using System.Xml;
using WLQuickApps.SocialNetwork.Business;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Caching;

namespace WLQuickApps.SocialNetwork.WebSite
{
    public class CollectionGeoRssHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            int collectionID = int.Parse(System.IO.Path.GetFileNameWithoutExtension(context.Request.Path));
            Collection collection = CollectionManager.GetCollection(collectionID);

            context.Response.ContentType = "text/xml";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            XmlTextWriter xtw = new XmlTextWriter(context.Response.Output);

            xtw.Formatting = Formatting.Indented;
            xtw.WriteStartDocument();
            xtw.WriteStartElement("rss");
            xtw.WriteAttributeString("version", "2.0");
            xtw.WriteStartElement("channel");
            xtw.WriteElementString("title", collection.Title);
            xtw.WriteStartElement("link");
            xtw.WriteFullEndElement();
            xtw.WriteElementString("description", collection.Description);
            xtw.WriteElementString("mappointIntlCode", "cht");

            foreach (CollectionItem item in collection.Items)
            {
                WLQuickApps.SocialNetwork.Business.Location location = item.Location;

                if ((location.Latitude == 0) && (location.Longitude == 0))
                {
                    continue;
                }

                StringWriter descriptionWriter = new StringWriter();
                using (HtmlTextWriter htw = new HtmlTextWriter(descriptionWriter))
                {
                    CollectionGeoRssHandler.RenderDescriptionPanel(item, htw);
                }

                xtw.WriteStartElement("item");
                xtw.WriteElementString("title", item.Title);
                xtw.WriteElementString("description", descriptionWriter.ToString());
                xtw.WriteElementString("geo", "lat", "http://virtualearth.msn.com/apis/annotate#", location.Latitude.ToString());
                xtw.WriteElementString("geo", "long", "http://virtualearth.msn.com/apis/annotate#", location.Longitude.ToString());
                xtw.WriteEndElement();
            }

            xtw.WriteEndDocument();
        }

        public static void RenderDescriptionPanel(CollectionItem item, HtmlTextWriter htw)
        {
            NullablePicture thumbnail = new NullablePicture();
            Label descriptionLabel = new Label();
            Panel mediaPanel = new Panel();
            HyperLink directionsLink = new HyperLink();

            List<Media> medias = MediaManager.GetMediaForLocation(item.Location, 0, 10);

            thumbnail.MaxHeight = 64;
            thumbnail.MaxWidth = 64;
            thumbnail.Style.Add(HtmlTextWriterStyle.MarginRight, Unit.Pixel(10).ToString());

            if (item.HasThumbnail)
            {
                // Use the collection's thumbnail.
                thumbnail.Item = item;
            }
            else if (medias.Count > 0)
            {
                // The collection doesn't have a thumbnail. Use the first media
                // item associated with the location instead.
                thumbnail.Item = medias[0];
                thumbnail.NavigateUrl = WebUtilities.GetViewItemUrl(thumbnail.Item);

                medias.RemoveAt(0);
            }
            else
            {
                // The collection dooesn't have a thumbnail and there is no media
                // for this location.
                thumbnail.Visible = false;
            }

            descriptionLabel.Style.Add(HtmlTextWriterStyle.VerticalAlign, VerticalAlign.Top.ToString());
            descriptionLabel.Text = item.Description;

            // Render each of the media items associated with this location.
            foreach (Media media in medias)
            {
                NullablePicture mediaThumbnail = new NullablePicture();
                mediaThumbnail.MaxHeight = 32;
                mediaThumbnail.MaxWidth = 32;
                mediaThumbnail.Style[HtmlTextWriterStyle.MarginRight] = Unit.Pixel(5).ToString();

                mediaThumbnail.Item = media;
                mediaThumbnail.NavigateUrl = WebUtilities.GetViewItemUrl(media);

                mediaPanel.Controls.Add(mediaThumbnail);
            }

            Location startAddress = Location.Empty;
            if (UserManager.IsUserLoggedIn())
            {
                startAddress = UserManager.LoggedInUser.Location;
            }

            directionsLink.NavigateUrl = string.Format("~/Directions.aspx?startAddress={0}&endAddress={1}",
                HttpUtility.UrlEncode(startAddress.GetAddressText()), HttpUtility.UrlEncode(item.Location.GetAddressText()));
            directionsLink.Style.Add(HtmlTextWriterStyle.VerticalAlign, VerticalAlign.Bottom.ToString());
            directionsLink.Style.Add(HtmlTextWriterStyle.TextAlign, TextAlign.Right.ToString());
            directionsLink.Text = "Directions";

            thumbnail.RenderControl(htw);
            descriptionLabel.RenderControl(htw);
            mediaPanel.RenderControl(htw);
            directionsLink.RenderControl(htw);
        }

        #endregion
    }
}
