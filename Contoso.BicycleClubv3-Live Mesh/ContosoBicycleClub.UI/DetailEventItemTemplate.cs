using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WLQuickApps.ContosoBicycleClub.UI
{
	/// <summary>
	/// Fix bug 
	/// 170707, 170708
	/// 
	/// </summary>
	public class DetailEventItemTemplate : ITemplate
	{
		private string urlFormat = "EventInfo.aspx?EventId={0}"; 
		private bool isShowDate = false;
		private int imageHeight;
		private int imageWidth;


		public DetailEventItemTemplate(bool isShowDate, int imageHeight, int imageWidth, string urlFormat)
		{
			this.isShowDate = isShowDate;
			this.imageHeight = imageHeight;
			this.imageWidth = imageWidth;
			this.urlFormat = urlFormat;
		}

		public void InstantiateIn(Control container)
		{
			Image img = new Image();
			img.ID = "EventImage";
			img.CssClass = "thumbnail";
			img.Width = Unit.Pixel(imageWidth);
			img.Height = Unit.Pixel(imageHeight);

			img.DataBinding += new EventHandler(this.Image_DataBinding);

			HyperLink link = new HyperLink();
			link.ID = "HyperLink";
			link.Controls.Add(img);
			link.DataBinding += new EventHandler(this.HyperLink_DataBinding);

			HyperLink titleLink = new HyperLink();
			titleLink.ID = "titleLink";
			titleLink.DataBinding += new EventHandler(this.TitleLink_DataBinding);

			HtmlGenericControl h3 = new HtmlGenericControl("h3");
			h3.Attributes.Add("class", "summary");
			h3.Controls.Add(titleLink);

			Label ownerLabel = new Label();
			ownerLabel.CssClass = "fn nickname";
			ownerLabel.ID = "OwnerLabel";
			ownerLabel.DataBinding += new EventHandler(this.OwnerLabel_DataBinding);

			HtmlGenericControl div = new HtmlGenericControl("div");
			div.Attributes.Add("class", "vcard");
			div.Controls.Add(ownerLabel);

			Label eventdateLabel = new Label();
			eventdateLabel.ID = "EventdateLabel";
			eventdateLabel.DataBinding += new EventHandler(this.EventdateLabel_DataBinding);
			
			HtmlGenericControl abbr = new HtmlGenericControl("abbr");
			abbr.ID = "Abbr1";
			abbr.Attributes.Add("class", "dtstart");
			abbr.DataBinding += new EventHandler(this.Abbr_DataBinding);
			abbr.Controls.Add(eventdateLabel);

			HtmlGenericControl li = new HtmlGenericControl("li");
			li.Attributes.Add("class", "vevent");

			li.Controls.Add(link);
			li.Controls.Add(h3);
			li.Controls.Add(div);

			if (isShowDate)
			{
				li.Controls.Add(abbr);
			}

			container.Controls.Add(li);			
		}

		void Abbr_DataBinding(object sender, EventArgs e)
		{
			HtmlGenericControl item = (HtmlGenericControl)sender;
			RepeaterItem container = (RepeaterItem)item.NamingContainer;
			string eventDate = HttpUtility.HtmlEncode((string)DataBinder.GetPropertyValue(container.DataItem, "EventDate", "{0:yyyy-MM-dd}"));
			item.Attributes.Add("title", eventDate);
		}

		void EventdateLabel_DataBinding(object sender, EventArgs e)
		{
			Label item = (Label)sender;
			RepeaterItem container = (RepeaterItem)item.NamingContainer;
			item.Text = HttpUtility.HtmlEncode((string)DataBinder.GetPropertyValue(container.DataItem, "EventDate", "{0:MMMM dd, yyyy}"));
		}


		void OwnerLabel_DataBinding(object sender, EventArgs e)
		{
			Label item = (Label)sender;
			RepeaterItem container = (RepeaterItem)item.NamingContainer;
			item.Text = HttpUtility.HtmlEncode((string)DataBinder.GetPropertyValue(container.DataItem, "OwnerName"));
		}

		public void Image_DataBinding(object sender, EventArgs e)
		{
			Image item = (Image)sender;
			RepeaterItem container = (RepeaterItem) item.NamingContainer;
			item.ImageUrl = HttpUtility.HtmlEncode((string)DataBinder.GetPropertyValue(container.DataItem, "PhotoLink"));
		}

		public void HyperLink_DataBinding(object sender, EventArgs e)
		{
			HyperLink item = (HyperLink)sender;
			RepeaterItem container = (RepeaterItem)item.NamingContainer;
			item.NavigateUrl = (string)DataBinder.GetPropertyValue(container.DataItem, "RideId", urlFormat);
		}

		public void TitleLink_DataBinding(object sender, EventArgs e)
		{
			HyperLink item = (HyperLink)sender;
			RepeaterItem container = (RepeaterItem)item.NamingContainer;
			item.NavigateUrl = (string)DataBinder.GetPropertyValue(container.DataItem, "RideId", urlFormat);
			item.Text = HttpUtility.HtmlEncode((string)DataBinder.GetPropertyValue(container.DataItem, "Title"));
		}

	}
}
