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

/// <summary>
/// Summary description for Spaces
/// </summary>
public class Spaces
{
	public Spaces()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Get the latest item for the RSS Feed.
    /// </summary>
    /// <param name="feed"></param>
    /// <returns></returns>
    public static string LatestTitle(string feed)
    {
        XmlDocument xmlDocument = new XmlDocument();
        try
        {
            XmlReader xmlReader = XmlReader.Create(feed);

            xmlDocument.Load(xmlReader);
        }
        catch(Exception ex)
        {
            return "Error: " + ex.Message;
        }
        
        if(xmlDocument.SelectSingleNode("rss/channel/item/title")!= null)
        {
            return(xmlDocument.SelectSingleNode("rss/channel/item/title").InnerText);
        } 
        else
        {
            return("Error: Title not found.");
        }
    }

    /// <summary>
    /// Get the latest description from the feed.
    /// </summary>
    /// <param name="feed"></param>
    /// <returns></returns>
    public static string LatestDescription(string feed)
    {
        XmlDocument xmlDocument = new XmlDocument();

        XmlReader xmlReader = XmlReader.Create(feed);

        xmlDocument.Load(xmlReader);
        
        // Does the node exist
        if (xmlDocument.SelectSingleNode("rss/channel/item/description") != null)
        {
            // Yes - return
            return (xmlDocument.SelectSingleNode("rss/channel/item/description").InnerText);
        }
        else
        {
            return ("Error: Description not found.");
        }

       

    }
}
