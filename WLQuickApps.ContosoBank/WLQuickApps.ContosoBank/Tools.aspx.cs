/*****************************************************************************
 * Tools.apx
 * Notes: Page to provide community with useful tools
 * **************************************************************************/

using System;

namespace WLQuickApps.ContosoBank
{
    public partial class Tools : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            // User choose the Business Search - redirect them to the search
            // results page
            Session["SearchString"] = BusinessSearchTextBox.Text;
            Session["SearchPage"] = 0;
            Response.Redirect("SearchResults.aspx");
        }
    }
}