using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using WLQuickApps.ContosoBank.Entity;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class FeaturedEventControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FeaturedEventRepeater.DataSource = EventLogic.GetFeaturedEvents();
                FeaturedEventRepeater.DataBind();
            }
        }

        protected void FeaturedEventRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Item)
            {
                LocalEvent localEvent = (LocalEvent) e.Item.DataItem;

                Label tempLabel = (Label) e.Item.FindControl("SubjectLabel");
                string subject = localEvent.EventName;
                tempLabel.Text = (subject.Length > 50) ? subject.Substring(0, 47) + "..." : subject;

                tempLabel = (Label) e.Item.FindControl("locationLabel");
                tempLabel.Text = localEvent.Location + ",";

                tempLabel = (Label) e.Item.FindControl("eventDateLabel");
                tempLabel.Text = localEvent.EventDate.ToShortDateString();

                tempLabel = (Label) e.Item.FindControl("EventDateTimeLabel");
                tempLabel.Text = localEvent.EventDate.ToLongDateString() + ", " + localEvent.EventDuration;
            }
        }
    }
}