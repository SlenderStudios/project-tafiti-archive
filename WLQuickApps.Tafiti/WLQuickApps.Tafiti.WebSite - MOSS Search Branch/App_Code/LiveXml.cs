using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using LiveSearch;

public class LiveXmlResult 
{
    public string domain;
    public string title;
    public string description;
    public string source;
    public string imageUrl;
    public string height;
    public string width;
    public string url;
}

public class LiveXmlWebResult : LiveXmlResult
{
    public string title;
    public string desc;
    public string url;
}

public class LiveXmlImageResult : LiveXmlResult
{
    public LiveXmlImageThumbnail thumbnail = new LiveXmlImageThumbnail();
    public string url;
}

public class LiveXmlImageThumbnail
{
    public string width = "0";
    public string height = "0";
    public string url = "";
}

public class LiveXmlNewsResult : LiveXmlResult
{
    public string title;
    public string desc;
    public string source;
    public string url;
}

public class LiveXmlSearchResults
{
    public LiveXmlSearchResult searchresult = new LiveXmlSearchResult();
}

public class LiveXmlSearchResult
{
    public LiveXmlDocumentSet documentset = new LiveXmlDocumentSet();
}

public class LiveXmlDocumentSet
{
    public string _source;
    public string _start;
    public string _total;
    public string _count;
    public object document; // if _count is 1, this is an instance of LiveXml...Result, otherwise it is an array
}