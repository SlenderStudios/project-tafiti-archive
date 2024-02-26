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
    public partial class EventsControl : System.Web.UI.UserControl
    {
        protected string ErrorMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            EventsDataList.DataSource =  eventsDataSource;
            try
            {
                DataBind();
            }
            catch (Exception ex)
            {
                ErrorDiv.Visible = true;
                ErrorMessage = ex.Message;
            }
        }
    }
}