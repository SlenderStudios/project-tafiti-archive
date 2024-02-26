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
using System.Web.Configuration;
using System.Text.RegularExpressions;
using WindowsLive;
namespace ContosoUniversity
{
    public partial class WhereAreTheyNow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set up the contacts control to read the user's address book
            if (Session["ConsentToken"] != null)
            {
                ContactsControl1.ConsentToken = (WindowsLiveLogin.ConsentToken)Session["ConsentToken"];
            }

            // Bind the location dropdown to a spaces list of points of interest
            if (!IsPostBack)
            {
                try
                {
                    // Read the RSS feed into a dataset
                    string url = WebConfigurationManager.AppSettings["CampusLocationsList"];
                    DataSet ds = new DataSet();
                    ds.ReadXml(url);

                    string regex_p = "<p>[-+]?[0-9]*.[0-9]*,[-+]?[0-9]*.[0-9]*</p>";
                    Regex ex_p = new Regex(regex_p, RegexOptions.IgnoreCase);

                    // Loop through the rows and extract the lat/long from the description element
                    foreach (DataRow row in ds.Tables["item"].Rows)
                    {
                        string s = row["description"].ToString();
                        MatchCollection m = ex_p.Matches(s);

                        if (m.Count == 1)
                        {
                            char[] TrimStart = new char[] { ' ', '<', 'p', '>' };
                            char[] TrimEnd = new char[] { '<', '/', 'p', '>', ' ' };
                            row["description"] = m[0].Value.TrimStart(TrimStart).TrimEnd(TrimEnd);
                        }
                        else
                        {
                            row["description"] = "0,0";
                        }
                    }

                    // Set the dropdown list binding properties
                    Locations.DataTextField = ds.Tables[6].Columns[0].ToString();
                    Locations.DataValueField = ds.Tables[6].Columns[2].ToString();
                    Locations.DataSource = ds.Tables[6];
                    Locations.DataBind();
                }
                catch { }
            }
        }

    }
}