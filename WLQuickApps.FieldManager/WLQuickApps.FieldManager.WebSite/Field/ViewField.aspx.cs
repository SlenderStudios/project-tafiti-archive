using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using WLQuickApps.FieldManager.Business;
using WLQuickApps.FieldManager.Data;
using Microsoft.Security.Application;

namespace WLQuickApps.FieldManager.WebSite
{
    public partial class Field_ViewField : System.Web.UI.Page
    {
        private Field _field;

        public string Address { get { return AntiXss.JavaScriptEncode(this._field.Address); } }
        public int FieldID { get { return this._field.FieldID; } }
        public double Latitude { get { return this._field.Latitude; } }
        public double Longitude { get { return this._field.Longitude; } }
        public string MyFields 
        {
            get
            {
                if (!UserManager.UserIsLoggedIn()) { return string.Empty; }

                StringBuilder stringBuilder = new StringBuilder();
                foreach (Field field in FieldsManager.GetFieldsForUser(UserManager.LoggedInUser.UserID, 0, FieldsManager.GetFieldsForUserCount(UserManager.LoggedInUser.UserID)))
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.AppendFormat("{0}: true", field.FieldID);
                }

                return stringBuilder.ToString();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            try
            {                
                this._field = FieldsManager.GetField(Convert.ToInt32(this.Request.QueryString["fieldID"]));
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Default.aspx");
            }

            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this._adminPanel.Visible = FieldsManager.IsFieldAdmin(this._field.FieldID);

            this._addressTextBox.Text = this._field.Address;
            this._descriptionTextBox.Text = this._field.Description;
            this._isOpenCheckBox.Checked = this._field.IsOpen;
            this._numberOfFieldsTextBox.Text = this._field.NumberOfFields.ToString();
            this._parkingLotTextBox.Text = this._field.ParkingLot;
            this._phoneNumberTextBox.Text = this._field.PhoneNumber;
            this._statusTextBox.Text = this._field.Status;
            this._titleTextBox.Text = this._field.Title;
        }

        protected void _deleteClick(object sender, EventArgs e)
        {
            FieldsManager.DeleteField(this._field.FieldID);
            this.Response.Redirect("~/User/Default.aspx");
        }

        protected void _addAdminByEmailButtonClick(object sender, EventArgs e)
        {
            try
            {
                FieldsManager.AddFieldAdmin(this._field.FieldID, (Guid)Membership.GetUser(Membership.GetUserNameByEmail(this._addAdminByEmailTextBox.Text)).ProviderUserKey);
                this._addAdminByEmailTextBox.Text = string.Empty;
            }
            catch { }
        }

    }
}