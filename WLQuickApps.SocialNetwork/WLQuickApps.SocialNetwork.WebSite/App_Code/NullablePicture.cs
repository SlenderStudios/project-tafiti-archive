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
    [ParseChildren(false)]
    [ControlBuilder(typeof(HyperLinkControlBuilder))]
    [Designer("System.Web.UI.Design.WebControls.HyperLinkDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [ToolboxData(@"<{0}:NullablePicture runat=""server"">NullablePicture</{0}:NullablePicture>")]
    [DefaultProperty("Text")]
    public class NullablePicture : HyperLink
    {
        public BaseItem Item
        {
            get { return this._item; }
            set { this._item = value; }
        }
        private BaseItem _item;

        public int MaxHeight
        {
            get { return this._maxHeight; }
            set { this._maxHeight = value; }
        }
        private int _maxHeight;

        public int MaxWidth
        {
            get { return this._maxWidth; }
            set { this._maxWidth = value; }
        }
        private int _maxWidth;

        public string NullImageUrl
        {
            get { return base.ImageUrl; }
            set { base.ImageUrl = value; }
        }

        public NullablePicture()
            : base()
        {
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (this._item != null)
            {
                if (this._item.HasThumbnail)
                {
                    base.ImageUrl = WebUtilities.GetViewImageUrl(this._item.ThumbnailBaseItemID, this._maxWidth, this._maxHeight);
                }

                if (string.IsNullOrEmpty(this.Text))
                {
                    this.Text = this._item.Title;
                }

                if (string.IsNullOrEmpty(base.ImageUrl))
                {
                    this.Visible = false;
                }
            }

            base.RenderControl(writer);
        }
    }
}
