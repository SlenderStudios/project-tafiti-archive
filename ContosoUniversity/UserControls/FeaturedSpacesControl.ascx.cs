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

namespace ContosoUniversity.UserControls
{
    public partial class FeaturedSpacesControl : System.Web.UI.UserControl
    {
        private bool textMode = false;

        public bool TextMode
        {
            get { return textMode; }
            set { textMode = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Panel1.Visible = !textMode;
            Panel2.Visible = textMode;
        }

    }
}