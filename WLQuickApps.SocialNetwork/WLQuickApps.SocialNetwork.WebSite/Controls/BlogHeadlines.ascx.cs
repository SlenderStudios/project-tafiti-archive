using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using RssToolkit;

public partial class BlogHeadlines : System.Web.UI.UserControl
{
    public string RssUrl
    {
        get { return this._rssDataSource.Url; }
        set { this._rssDataSource.Url = value; }
    }

    public string ErrorText
    {
        get { return this._errorLabel.Text; }
        set { this._errorLabel.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void DataBindChildren()
    {
        try
        {
            this._rssDataSource.DataBind();
            this._blogHeadlines.DataSource = this._rssDataSource.Channel.Items;

            base.DataBindChildren();

            this._blogHeadlinesPanel.Visible = true;
            this._errorLabel.Visible = false;
        }
        catch
        {
            this._blogHeadlinesPanel.Visible = false;
            this._errorLabel.Visible = true;
        }
    }

    protected string GetRssItemTitle(object rssItem)
    {
        GenericRssElement element = (GenericRssElement)rssItem;

        if (!element.Attributes.ContainsKey("Title"))
        {
            return "Untitled";
        }
        else
        {
            string title = element.Attributes["Title"];

            if (title.Length > 70)
            {
                title = title.Substring(0, 65) + "...";
            }

            return this.Server.HtmlEncode(title);
        }
    }
}
