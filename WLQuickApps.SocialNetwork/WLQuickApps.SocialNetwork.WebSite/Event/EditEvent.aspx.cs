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

public partial class Event_EditEvent : System.Web.UI.Page
{
    private int _eventID;

    protected void Page_Load(object sender, EventArgs e)
    {
        this._eventID = Int32.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]);
        if (!this.IsPostBack)
        {
            this._editEventForm.GroupItem = EventManager.GetEvent(this._eventID);
        }
    }

    protected void _editEventForm_Save(object sender, EventArgs e)
    {
        Event eventItem = EventManager.GetEvent(this._eventID);

        eventItem.Title = this._editEventForm.Name;
        eventItem.Description = this._editEventForm.Description;
        eventItem.Location = this._editEventForm.Location;
        eventItem.ChangeEventTime(this._editEventForm.StartTime, this._editEventForm.EndTime);
        eventItem.PrivacyLevel = this._editEventForm.PrivacyLevel;
        if (this._editEventForm.PictureBits != null)
        {
            eventItem.SetThumbnail(this._editEventForm.PictureBits);
        }

        EventManager.UpdateEvent(eventItem);

        //GridView gridView = (GridView)this._editEventForm.FindControl("_associationsGridView");
        //foreach (GridViewRow row in gridView.Rows)
        //{
        //    if (!((CheckBox)row.FindControl("_keepCheckBox")).Checked)
        //    {
        //        BaseItem baseItem = BaseItemManager.GetBaseItem(Convert.ToInt32(((Label)row.FindControl("_baseItemIDLabel")).Text));

        //        eventItem.RemoveBaseItemAssociation(baseItem, false);
        //    }
        //}

        //GridView gridViewParents = (GridView)this._editEventForm.FindControl("_associationsParentsGridView");
        //foreach (GridViewRow row in gridViewParents.Rows)
        //{
        //    if (!((CheckBox)row.FindControl("_keepCheckBox")).Checked)
        //    {
        //        BaseItem baseItem = BaseItemManager.GetBaseItem(Convert.ToInt32(((Label)row.FindControl("_baseItemIDLabel")).Text));

        //        if (baseItem is Group)
        //        {
        //            ((Group)baseItem).RemoveBaseItemAssociation(eventItem, true);
        //        }
        //    }
        //}

        Response.Redirect(WebUtilities.GetViewItemUrl(EventManager.GetEvent(this._eventID)));
    }
}
