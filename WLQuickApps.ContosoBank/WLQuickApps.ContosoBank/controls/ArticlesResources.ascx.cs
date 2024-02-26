using System;
using System.Web.UI;

using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class ArticlesResources : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ArticlesRepeater.DataSource = ArticlesResourcesLogic.GetArticlesResourcesByType("Article", 5);
                ArticlesRepeater.DataBind();

                ResourcesRepeater.DataSource = ArticlesResourcesLogic.GetArticlesResourcesByType("Resources", 5);
                ResourcesRepeater.DataBind();
            }
        }
    }
}