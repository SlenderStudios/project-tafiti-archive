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
using System.ComponentModel;
using WLQuickApps.SocialNetwork.Business;
using AjaxControlToolkit;
using System.Collections.Generic;
using WLQuickApps.SocialNetwork.WebSite;

public partial class MediaRating : System.Web.UI.UserControl
{
    [Bindable(true)]
    [ToolboxItem(true)]
    public int MediaID
    {
        get { return this._mediaID; }
        set { this._mediaID = value; }
    }
    private int _mediaID;

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.RegisterRequiresControlState(this);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this._rating.CurrentRating = (int)Math.Round(MediaManager.GetMedia(this._mediaID).AverageRating, 1);
    }

    protected override void LoadControlState(object savedState)
    {
        Dictionary<string, object> stateData = (Dictionary<string, object>)savedState;

        base.LoadControlState(stateData[WebConstants.ControlStateVariables.BaseState]);
        this._mediaID = (int)stateData[WebConstants.ControlStateVariables.BaseItemID];
    }

    protected override object SaveControlState()
    {
        Dictionary<string, object> stateData = new Dictionary<string, object>(2);

        stateData[WebConstants.ControlStateVariables.BaseState] = base.SaveControlState();
        stateData[WebConstants.ControlStateVariables.BaseItemID] = this._mediaID;

        return stateData;
    }

    protected void _rating_Changed(object sender, RatingEventArgs e)
    {
        if (UserManager.IsUserLoggedIn())
        {
            MediaManager.GetMedia(this._mediaID).Rate(Int32.Parse(e.Value));
        }
    }
}
