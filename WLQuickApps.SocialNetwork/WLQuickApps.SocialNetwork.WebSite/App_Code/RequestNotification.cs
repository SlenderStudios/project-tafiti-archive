using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WLQuickApps.SocialNetwork.Business;
using System.ComponentModel;

namespace WLQuickApps.SocialNetwork.WebSite
{
    [ControlBuilder(typeof(HyperLinkControlBuilder))]
    [Designer("System.Web.UI.Design.WebControls.HyperLinkDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [ToolboxData(@"<{0}:RequestNotification runat=""server""></{0}:RequestNotification>")]
    [PersistChildren(false)]
    [ParseChildren(true)]
    public class RequestNotification : HyperLink
    {
        [Bindable(true)]
        [Category("Behavior")]
        [Localizable(false)]
        [Themeable(false)]
        public int RequestCount
        {
            get
            {
                int? i = this.ViewState[WebConstants.ViewStateVariables.RequestCount] as int?;
                return i.GetValueOrDefault(0);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("RequestCount must be nonnegative.", "RequestCount");
                }
                this.ViewState[WebConstants.ViewStateVariables.RequestCount] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(false)]
        [Themeable(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style RequestCountStyle
        {
            get
            {
                return this._requestCountStyle;
            }
        }
        private Style _requestCountStyle;

        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(false)]
        [Themeable(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style RequestsLabelStyle
        {
            get
            {
                return this._requestsLabelStyle;
            }
        }
        private Style _requestsLabelStyle;

        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(true)]
        [Themeable(false)]
        public string RequestsLabelText
        {
            get
            {
                string s = this.ViewState[WebConstants.ViewStateVariables.RequestsLabelText] as string;
                if (s == null)
                {
                    return string.Empty;
                }
                else
                {
                    return s;
                }
            }
            set
            {
                this.ViewState[WebConstants.ViewStateVariables.RequestsLabelText] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(true)]
        [Themeable(false)]
        public string SingularRequestsLabelText
        {
            get
            {
                string s = this.ViewState[WebConstants.ViewStateVariables.SingularRequestsLabelText] as string;
                if (s == null)
                {
                    return this.RequestsLabelText;
                }
                else
                {
                    return s;
                }
            }
            set
            {
                this.ViewState[WebConstants.ViewStateVariables.SingularRequestsLabelText] = value;
            }
        }

        private Label _requestCountLabel;
        private Label _requestsLabel;

        public RequestNotification()
        {
            this._requestCountStyle = new Label().ControlStyle;
            this._requestsLabelStyle = new Label().ControlStyle;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this._requestCountLabel = new Label();
            this.Controls.Add(this._requestCountLabel);
            this._requestsLabel = new Label();
            this.Controls.Add(this._requestsLabel);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.RequestCount > 0)
            {
                this._requestCountLabel.Text = String.Format("{0}&nbsp;", this.RequestCount);
                this._requestCountLabel.ApplyStyle(this._requestCountStyle);

                if (this.RequestCount == 1)
                {
                    this._requestsLabel.Text = string.Format("{0}<br />", this.SingularRequestsLabelText);
                }
                else
                {
                    this._requestsLabel.Text = string.Format("{0}<br />", this.RequestsLabelText);
                }
                this._requestsLabel.ApplyStyle(this._requestsLabelStyle);
            }
            else
            {
                this.Visible = false;
            }

            base.OnPreRender(e);
        }
    }
}
