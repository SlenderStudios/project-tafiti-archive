using System;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;

namespace WLQuickApps.SocialNetwork.WebSite
{
    [PersistChildren(true)]
    [ToolboxData(@"<{0}:DropShadowPanel runat=""server"" Width=""125px"" Height=""50px""> </{0}:DropShadowPanel>")]
    [Designer("System.Web.UI.Design.WebControls.PanelContainerDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [ParseChildren(false)]
    public class DropShadowPanel : Panel
    {
        [Themeable(true)]
        [ToolboxItem(true)]
        public string CssClassPrefix
        {
            get { return this._cssClassPrefix; }
            set { this._cssClassPrefix = value; }
        }
        private string _cssClassPrefix = "shadow";

        [Themeable(true)]
        [ToolboxItem(true)]
        public bool DisplayShadow
        {
            get { return this._displayShadow; }
            set { this._displayShadow = value; }
        }
        private bool _displayShadow = false;

        public DropShadowPanel()
            : base()
        {
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);

            if (this._displayShadow)
            {
                this.WriteShadowTag(writer, "upperRight");
                this.WriteShadowTag(writer, "right");
                this.WriteShadowTag(writer, "lowerRight");
                this.WriteShadowTag(writer, "lower");
                this.WriteShadowTag(writer, "lowerLeft");
            }
        }

        private void WriteShadowTag(HtmlTextWriter writer, string cssSuffix)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class,
                    String.Format("{0}-{1}", this._cssClassPrefix, cssSuffix));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
        }
    }
}