/*****************************************************************************
 * SearchResults.apx
 * Notes: Page to display the results of the user's business search
 * **************************************************************************/

using System;

namespace WLQuickApps.ContosoBank
{
    public partial class SearchResults : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["SearchPage"] != null && Session["SearchString"] != null)
                {
                    businessSearchResultsControl.BindData(Session["SearchString"].ToString(), 0);
                }
            }
        }
    }
}