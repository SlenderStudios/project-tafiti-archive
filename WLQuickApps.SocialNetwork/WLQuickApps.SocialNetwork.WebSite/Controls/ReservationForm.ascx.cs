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

public partial class ReservationForm : System.Web.UI.UserControl
{
    [ToolboxItem(true)]
    public string DefaultValue
    {
        get { return this._defaultValue; }
        set { this._defaultValue = value; }
    }
    private string _defaultValue;

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((!String.IsNullOrEmpty(this._defaultValue)) && (!this.Page.IsPostBack))
        {
            foreach (ListItem listItem in this._destinationList.Items)
            {
                listItem.Selected = false;
                if (listItem.Text == this._defaultValue)
                {
                    listItem.Selected = true;
                }
            }
        }
        this.SetCurrentImage();
    }

    protected void _destinationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.SetCurrentImage();
    }

    protected void _destinationRequired_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !(this._destinationList.SelectedIndex == 0);
    }

    private void SetCurrentImage()
    {
        this._sideImage.ImageUrl = String.Format("~/Images/reservationPics/{0}.jpg", this._destinationList.SelectedValue);
    }
}
