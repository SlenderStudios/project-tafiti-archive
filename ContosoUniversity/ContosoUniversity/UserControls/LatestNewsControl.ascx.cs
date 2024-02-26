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
    public partial class LatestNewsControl : System.Web.UI.UserControl
    {
        protected string errorMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            latestNewsDataList.DataSource = latestNewsDataSource;
            try
            {
                DataBind();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                ErrorDiv.Visible = true;
            }
        }
    }
}