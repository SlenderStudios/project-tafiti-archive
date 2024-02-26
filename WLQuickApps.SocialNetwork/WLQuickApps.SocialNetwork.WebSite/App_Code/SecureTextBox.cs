using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Extends the ASP TextBox control to return HTML-Encoded text to prevent XSS attacks
/// </summary>

namespace WLQuickApps.SocialNetwork.WebSite
{
    [ToolboxData(@"<{0}:SecureTextBox runat=""server"" />")]
    public class SecureTextBox : TextBox
    {
        public override string Text
        {
            get
            {
                return HttpUtility.HtmlEncode(base.Text);
            }
            set
            {
                base.Text = value;
            }
        }
    }
}
