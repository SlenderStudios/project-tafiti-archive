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
using System.Collections.Generic;
using System.ComponentModel;
using WLQuickApps.SocialNetwork.WebSite;

public partial class LocationControl : System.Web.UI.UserControl
{
    [Bindable(true)]
    public Location LocationItem
    {
        get 
        {
            return LocationManager.CreateLocation(this._nameTextBox.Text, this._address1TextBox.Text, this._address2TextBox.Text, this._cityTextBox.Text,
                this._regionTextBox.Text, this._countryDropDown.SelectedValue, this._postalCodeTextBox.Text);
        }
        set
        {
            Location location = value;
            if (location == null)
            {
                location = Location.Empty;
            }

            this._initialNameLabel.Text = location.Name;
            this._nameTextBox.Text = location.Name;
            this._address1TextBox.Text = location.Address1;
            this._address2TextBox.Text = location.Address2;
            this._cityTextBox.Text = location.City;
            this._countryDropDown.SelectedValue = location.Country;
            this._postalCodeTextBox.Text = location.PostalCode;
            this._regionTextBox.Text = location.Region;
        }
    }

    public string DataSourceName
    {
        set 
        {
            MapPointWrapper._DataSourceName = this._countryDropDown.SelectedValue;
        }
    }

    public string Name
    {
        get { return this._nameTextBox.Text; }
        set { this._nameTextBox.Text = value; }
    }

    public string Address1
    {
        get { return this._address1TextBox.Text; }
        set { this._address1TextBox.Text = value; }
    }

    public string Address2
    {
        get { return this._address2TextBox.Text; }
        set { this._address2TextBox.Text = value; }
    }

    public string City
    {
        get { return this._cityTextBox.Text; }
        set { this._cityTextBox.Text = value; }
    }

    public string Region
    {
        get { return this._regionTextBox.Text; }
        set { this._regionTextBox.Text = value; }
    }

    public string Country
    {
        get { return this._countryDropDown.SelectedValue; }
        set { this._countryDropDown.SelectedValue = value; }
    }

    public string PostalCode
    {
        get { return this._postalCodeTextBox.Text; }
        set { this._postalCodeTextBox.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        MapPointWrapper._DataSourceName = this._countryDropDown.SelectedValue;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this._chooseLocation.Text = this.Server.HtmlEncode("Choose Location >>");
    }

    protected void _chooseLocation_Click(object sender, EventArgs e)
    {
        this._locationMiniPanel.Visible = false;
        this._currentLocationPanel.Visible = true;
    }

    protected void _clearButton_Click(object sender, EventArgs e)
    {
        this._address1TextBox.Text = string.Empty;
        this._address2TextBox.Text = string.Empty;
        this._nameTextBox.Text = string.Empty;
        this._cityTextBox.Text = string.Empty;
        this._regionTextBox.Text = string.Empty;
        this._postalCodeTextBox.Text = string.Empty;
        this._countryDropDown.SelectedIndex = 0;
        
    }
}
