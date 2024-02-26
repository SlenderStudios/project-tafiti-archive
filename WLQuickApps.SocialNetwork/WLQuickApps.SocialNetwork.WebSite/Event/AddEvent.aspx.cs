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
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Event_AddEvent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) { }

    protected void _addEventForm_Save(object sender, EventArgs e)
    {
        Event newEvent = EventManager.CreateEvent(this._addEventForm.Name, this._addEventForm.Description,
            this._addEventForm.StartTime, this._addEventForm.EndTime, this._addEventForm.Location,
            this._addEventForm.PrivacyLevel, this._addEventForm.Type);

        if (this._addEventForm.PictureBits != null)
        {
            newEvent.SetThumbnail(this._addEventForm.PictureBits);
            EventManager.UpdateEvent(newEvent);
        }

        if (!String.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]))
        {
            Group group = GroupManager.GetGroup(Int32.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]));
            group.Associate(newEvent);
            Response.Redirect(WebUtilities.GetViewItemUrl(group));
        }

        Response.Redirect(WebUtilities.GetViewItemUrl(newEvent));
    }
}
