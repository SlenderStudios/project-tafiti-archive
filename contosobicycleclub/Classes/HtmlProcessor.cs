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
/// The HtmlProcessor is a set of utility methods which are used
/// to parse HTML.
/// </summary>
public class HtmlProcessor
{
    // Extracts a maps.live.com collection id
    public static string ExtractMapCid(string html)
    {
        // Create the RegEx (conditions are find anything (including URLs) which have cid={THECIDVALUE}
        Regex regex = new Regex(";cid=(.*?)&amp");

        // Execute a search/match against the regex
        Match match = regex.Match(html);

        if (match.Groups.Count > 1)
            return match.Groups[1].Value;
        else return "";
    }

    // Extracts the first line
    public static string FirstLine(string html)
    {
        Regex regex = new Regex("(.*?)\\.");

        Match match = regex.Match(html);

        if (match.Groups.Count > 1)
            return match.Groups[1].Value;
        else return "";
    }

    // Extracts an image reference
    public static string ExtractImageUrl(string html)
    {
        // Create a RegEx to find the Image
        Regex regex = new Regex("<img.*?src.*?=.*?\"(.*?)\"", RegexOptions.IgnoreCase);

        // Execute the search/match.
        Match match = regex.Match(html);

        if (match.Groups.Count > 1)
            return match.Groups[1].Value;
        else return "";
    }

    // Extracts a Spaces Photo Album feed
    public static string ExtractPhotoAlbumFeed(string html)
    {
        // Create a regex to find a spaces photoalbum url.
        Regex regex = new Regex("http://(.*?).spaces.live.com/.*?PhotoAlbum.*?(cns!.*?)&");

        Match match = regex.Match(html);

        if (match.Groups.Count > 2)
            return string.Format("http://{0}.spaces.live.com/photos/{1}/feed.rss", match.Groups[1].Value, match.Groups[2].Value);
        else return "";
    }

    // Extracts an anchor reference
    public static string ExtractLink(string html)
    {
        Regex regex = new Regex("<a\\s+href=\"(.*?)\">", RegexOptions.IgnoreCase);

        Match match = regex.Match(html);

        if (match.Groups.Count > 1)
            return match.Groups[1].Value;
        else return "";
    }

    // Removes all tags from HTML
    public static string RemoveTags(string html)
    {
        Regex regex = new Regex("\\<.*?\\>");

        return (regex.Replace(html, ""));

    }
}