using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

/// <summary>
/// HTML processing methods
/// </summary>
public class HtmlProcessor
{
	public HtmlProcessor()
	{
	}

    /// <summary>
    /// A HTML Chunk will be passed in and the CID will be extracted with RegEx.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string ExtractMapCid(string html)
    {
        Regex r = new Regex(";cid=(.*?)&amp");

        Match m = r.Match(html);

        if (m.Groups.Count > 1)
            return m.Groups[1].Value;
        else return "";
    }

    public static string FirstLine(string html)
    {
        Regex r = new Regex("(.*?)\\.");

        Match m = r.Match(html);

        if (m.Groups.Count > 1)
            return m.Groups[1].Value;
        else return "";
    }

    /// <summary>
    /// Extract the Image URL from the HTML Chunk using RegEx.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string ExtractImageUrl(string html)
    {
        Regex r = new Regex("<img.*?src.*?=.*?\"(.*?)\"", RegexOptions.IgnoreCase);

        Match m = r.Match(html);

        if (m.Groups.Count > 1)
            return m.Groups[1].Value;
        else return "";
    }

    public static string ExtractPhotoAlbumFeed(string html)
    {
        Regex r = new Regex("http://(.*?).spaces.live.com/.*?PhotoAlbum.*?(cns!.*?)&");

        Match m = r.Match(html);

        if (m.Groups.Count > 2)
            return string.Format("http://{0}.spaces.live.com/photos/{1}/feed.rss",m.Groups[1].Value,m.Groups[2].Value);
        else return "";
    }


    public static string ExtractLink(string html)
    {
        Regex r = new Regex("<a\\s+href=\"(.*?)\">",RegexOptions.IgnoreCase);

        Match m = r.Match(html);

        if (m.Groups.Count > 1)
            return m.Groups[1].Value;
        else return "";
    }

    public static string ExtractVideo(string html)
    {
        Regex r = new Regex("/(streaming:/.*?)\"");

        Match m = r.Match(html);

        if (m.Groups.Count > 1)
        {
             string v =  m.Groups[1].Value;
            return v;
        }
        else return "";
    }

    public static string RemoveTags(string html)
    {
        Regex regex = new Regex("\\<.*?\\>");

        string s = regex.Replace(html, "");

        return s;
    }

}