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

public partial class RideReports : System.Web.UI.Page
{
    private DataList rideReportsDataList;
    private XmlDataSource rideReportsDataSource;
    private string DataBindingErrorMsg;
    private Panel errorItem;  

    protected void Page_Load(object sender, EventArgs e)
    {
        rideReportsDataList.DataSource = rideReportsDataSource;
        try
        {
            DataBind();
        }
        catch (Exception ex)
        {
            // failure to bind the data, enable the error row and show the underlying cause
            errorItem.Visible = true;
            DataBindingErrorMsg = ex.Message;
        }
    }
}
