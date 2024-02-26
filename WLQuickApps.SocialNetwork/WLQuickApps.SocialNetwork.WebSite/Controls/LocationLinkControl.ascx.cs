using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class LocationLinkControl : UserControl
{
    public Location LocationItem
    {
        get
        {
            return Location.Empty;
        }
        set
        {
            if ((value == null) || (value.Name.Length == 0))
            {
                this._showLocationCaption = false;
            }
            else
            {
                this._locationHyperLink.NavigateUrl = string.Format("~/Search.aspx?name={0}&address1={1}&address2={2}&city={3}&region={4}&country={5}&postalCode={6}",
                    value.Name, value.Address1, value.Address2, value.City, value.Region, value.Country, value.PostalCode);
            }

            if ((value != null) && (value != Location.Empty))
            {
                Location startAddress = Location.Empty;
                if (UserManager.IsUserLoggedIn())
                {
                    startAddress = UserManager.LoggedInUser.Location;
                }

                this._directionsHyperLink.NavigateUrl = string.Format("~/Directions.aspx?startAddress={0}&endAddress={1}",
                    HttpUtility.UrlEncode(startAddress.GetAddressText()), HttpUtility.UrlEncode(value.GetAddressText()));

                this._locationHyperLink.Text = value.Name;
                this._locationHyperLink.Visible = true;
            }
            else
            {
                this._locationHyperLink.Visible = false;
                this._directionsHyperLink.Visible = false;
            }
        }
    }

    public bool ShowLocationCaption
    {
        get { return this._showLocationCaption; }
        set { this._showLocationCaption = value; }
    }
    private bool _showLocationCaption = true;

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.RegisterRequiresControlState(this);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this._locationHyperLink.Visible = this._showLocationCaption;
        this._unknownLocationLabel.Visible = !this._showLocationCaption;
    }

    protected override void LoadControlState(object savedState)
    {
        Dictionary<string, object> stateData = (Dictionary<string, object>)savedState;

        base.LoadControlState(stateData[WebConstants.ControlStateVariables.BaseState]);
        this._showLocationCaption = (bool)stateData[WebConstants.ControlStateVariables.ShowLocationCaption];
    }

    protected override object SaveControlState()
    {
        Dictionary<string, object> stateData = new Dictionary<string, object>(2);

        stateData[WebConstants.ControlStateVariables.BaseState] = base.SaveControlState();
        stateData[WebConstants.ControlStateVariables.ShowLocationCaption] = this._showLocationCaption;

        return stateData;
    }
}
