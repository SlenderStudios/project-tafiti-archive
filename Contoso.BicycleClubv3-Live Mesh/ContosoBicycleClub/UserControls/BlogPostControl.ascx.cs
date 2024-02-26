using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WLQuickApps.ContosoBicycleClub.Business.MetaWeblogApi;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub.UserControls
{
	public partial class BlogPostControl : System.Web.UI.UserControl
	{
		//public string Description;
		public string BlogId { get; set; }
		public string Title { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			TitleLabel.InnerText = this.Title;

			try
			{
				MsnSpacesMetaWeblogService weblogSvc = new MsnSpacesMetaWeblogService();
				Post currentPost = weblogSvc.GetPost(BlogId);								
				//The html come from Windows Live Space and should be safe.
				//HttpUtility.HtmlEncode is used and un-encode only the known good tags
				//We do want to show the HTML
				DescriptionLabel.Text = AntiXssHelper.HtmlEncode(currentPost.description, AntiXssHelper.AllowedTags); 
			}
			catch (Exception ex)
			{
				DescriptionLabel.Text = ex.Message;
			}

		}
	}
}