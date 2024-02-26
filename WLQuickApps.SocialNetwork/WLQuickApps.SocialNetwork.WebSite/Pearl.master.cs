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

public partial class Pearl : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string siteTitle = SettingsWrapper.SiteTitle;
        //string pageTitle = this.Page.Title;

        //if ((String.IsNullOrEmpty(this.Page.Title)) || (this.Page.Title == "Untitled Page"))
        //{
        //    if (SiteMap.CurrentNode == null)
        //    {
        //        pageTitle = siteTitle;
        //    }
        //    else
        //    {
        //        pageTitle = SiteMap.CurrentNode.Title;
        //    }
        //}

        //if (pageTitle != siteTitle)
        //{
        //    //this._mainHeading.Text = pageTitle;
        //    pageTitle = String.Format("{0} - {1}", siteTitle, pageTitle);
        //}

        //this.Page.Title = pageTitle;
    }
}
