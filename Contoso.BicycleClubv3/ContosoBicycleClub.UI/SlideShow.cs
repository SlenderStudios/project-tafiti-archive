using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Collections;

namespace WLQuickApps.ContosoBicycleClub.UI
{
	public class SlideShow
	{
		public static AjaxControlToolkit.Slide[] GetSlides(string request)
		{
			// Spin up a new XML document to house the RSS FEED.
			System.Xml.XmlDocument xmlDocument = new XmlDocument();

			// Execute the request using the Reader
			XmlReader xmlReader = XmlReader.Create(request);

			// Load the XML Document with the reader output
			xmlDocument.Load(xmlReader);

			// Fetch the node list from the RSS see. http://cyber.law.harvard.edu/rss/rss.html#ltenclosuregtSubelementOfLtitemgt 
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/rss/channel/item/enclosure");

			// Create arraylist to house the slides
			ArrayList slides = new ArrayList();


			// Enumerate the enclosures
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				// fetch the description
				string description = HttpUtility.HtmlEncode(xmlNode.ParentNode.FirstChild.InnerText) ;

				// Add the slide (URI to IMAGE, NAME (using Description), DESCRIPTION)
				string url = HttpUtility.HtmlEncode(xmlNode.Attributes["url"].Value); //Fix Bug: 170719

				slides.Add(new AjaxControlToolkit.Slide(url, description, description));
			}

			// Return the slides.
			return (AjaxControlToolkit.Slide[])slides.ToArray(typeof(AjaxControlToolkit.Slide));

		}
	}
}
