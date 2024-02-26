using System;
using System.Web;

using WLQuickApps.Tafiti.WebSite;
using WLQuickApps.Tafiti.Business;

public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Utility.VerifyIsSelfRequest();

        if (ShelfStackManager.GetShelfStackByID(new Guid(this.Request.QueryString[Constants.QueryKeys.SnapshotID])) != null)
        {
            this.Response.StatusCode = 200;
        }
        else
        {
            this.Response.StatusCode = 404;
        }

//        string snapshotId = SnapshotUtility.GetSnapshotId(Request);
        //string shelfSlotContents = SnapshotUtility.LoadSnapshot(snapshotId);

        //Response.StatusCode = 200;
        //if (!string.IsNullOrEmpty(shelfSlotContents))
        //    Response.Write(shelfSlotContents);
        //Response.End();
    }
}
