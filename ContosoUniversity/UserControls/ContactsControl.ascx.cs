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
using System.Xml;
using System.Web.Configuration;
using WindowsLive;
namespace ContosoUniversity.UserControls
{
    public partial class ContactsControl : System.Web.UI.UserControl
    {
        private XmlDocument contactLocationsXml = new XmlDocument();
        private const string Offers = "ContactsSync.FullSync";

        public string GetDelegationURI
        {
            get
            {
                WindowsLiveLogin wll = new WindowsLiveLogin(true);
                return wll.GetConsentUrl(Offers);
            }
        }

        private string ownerHandle;


        public string OwnerHandle
        {
            get { return ownerHandle; }
            set { ownerHandle = value; }
        }
        private string delegationToken;

        public string DelegationToken
        {
            get { return delegationToken; }
            set { delegationToken = value; }
        }

        private bool showMapLinks = false;

        public bool ShowMapLinks
        {
            get { return showMapLinks; }
            set { showMapLinks = value; }
        }

        private WindowsLiveLogin.ConsentToken consentToken = null;
        public WindowsLiveLogin.ConsentToken ConsentToken
        {
            set
            {
                consentToken = value;
                //init 
                if (value != null)
                {
                    ownerHandle = (Int64.Parse(value.LocationID, System.Globalization.NumberStyles.HexNumber)).ToString();
                    delegationToken = value.DelegationToken;
                }
            }
        }

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        /// Handles the page load event
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // If there is a user then fetch their address book data from Live Contacts
            if (ownerHandle != null)
            {
                // Create an instance of our LiveContacts wrapper class
                LiveContacts lc = new LiveContacts();

                // Fetch the user's address book
                XmlDataSource1.Data = lc.GetContacts(string.Format("{0}/rest/LiveContacts", ownerHandle), delegationToken);
                XmlDataSource1.XPath = "/LiveContacts/Contacts/Contact[Emails/Email/EmailType='WindowsLiveID']"; //ALOGAN: This case changed from WindowsLiveId to WindowsLiveID
                //XmlDataSource1.XPath = "/LiveContacts/Contacts/Contact";

                // Bind the xml to the grid view
                GridView1.DataSourceID = "XmlDataSource1";
                GridView1.PageSize = pageSize;

                // Fetch the location nodes from the Contoso address book that holds the location of everyone
                string addressBookOwnerHandle = WebConfigurationManager.AppSettings["AddressBookOwnerHandle"];
                string addressBookOwnerAuthToken = WebConfigurationManager.AppSettings["AddressBookOwnerAuthToken"];
                if (addressBookOwnerAuthToken != null)
                {
                    contactLocationsXml.LoadXml(lc.GetContacts(string.Format("{0}/rest/LiveContacts", addressBookOwnerHandle), addressBookOwnerAuthToken));
                }
            }

            // Show or hide the current location column and 'see all' link
            GridView1.Columns[4].Visible = showMapLinks;
            SeeAllLink.Visible = showMapLinks;
        }

        /// <summary>
        /// Handles the grid view row bound event.
        /// We need to add in the location from the site administrator's address book as this is not in the bound data
        /// </summary>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Only select data rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    // Extract the user's email address (LiveID) from the row
                    DataBoundLiteralControl emailAddressControl = (DataBoundLiteralControl)e.Row.Cells[3].Controls[0];
                    int start = emailAddressControl.Text.IndexOf("mailto:") + 7;
                    int end = emailAddressControl.Text.IndexOf(">");
                    string emailAddress = emailAddressControl.Text.Substring(start, end - start - 1);

                    // Extract the First Name from the row
                    DataBoundLiteralControl firstNameControl = (DataBoundLiteralControl)e.Row.Cells[1].Controls[0];
                    string firstName = firstNameControl.Text.Replace("\r\n", "").Trim();

                    // If the First Name is empty then replace with the first part of the email address
                    if (firstName == "")
                    {
                        firstName = emailAddress.Substring(0, emailAddress.IndexOf("@"));
                        ((TableCell)e.Row.Cells[1]).Text = firstName;
                    }

                    // If we are displaying the location buttons, then get the user's location from the admin address book
                    if (showMapLinks)
                    {
                        // Create a reference to the grid cell
                        TableCell locationControl = (TableCell)e.Row.Cells[4];

                        // Fetch the location from the admin address book
                        Location location = GetLocation(emailAddress);

                        // If it's not null then create a hyperlink and add it to the cell text
                        if (location != null)
                            locationControl.Text = string.Format("<a href=\"javascript:PlotContactOnMap('{0}', {1}, {2}, '{3}');\"><img alt=\"Plot {0} on the map\" src=\"App_Themes/Default/images/location_icon.gif\" border=0 /></a>", firstName, location.Latitude, location.Longitude, location.LocationText);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Fetches a location for a user from the administrators address book
        /// </summary>
        /// <param name="windowsLiveId">A string containing the Windows Live ID</param>
        /// <returns></returns>
        private Location GetLocation(string windowsLiveId)
        {
            Location location = null;

            // Select the node that matches the id
            string xPath = string.Format("/LiveContacts/Contacts/Contact/Emails/Email[Address='{0}']", windowsLiveId);
            XmlNode contactXmlNode = contactLocationsXml.SelectSingleNode(xPath);

            if (contactXmlNode != null)
            {
                // Select the location node from the locations list
                XmlNode locationXmlNode = contactXmlNode.ParentNode.ParentNode.SelectSingleNode("Locations/Location[LocationType='Personal']");

                // Should always have a location node, but check anyway
                if (locationXmlNode != null)
                {
                    location = new Location();
                    location.Latitude = double.Parse(locationXmlNode["Latitude"].InnerText);
                    location.Longitude = double.Parse(locationXmlNode["Longitude"].InnerText);
                    location.LocationText = locationXmlNode["StreetLine"].InnerText;
                }
            }
            return location;
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (ownerHandle == null)
            {
                //TODO - investigate using Request.ApplicationPath instead of Request.Url.Host
                string siteName = "https://" + Request.Url.Host;
                output.Write(string.Format("<a href='" + GetDelegationURI + "' title='Click this link to grant permission for the Contoso University web site to display your Windows Live Contacts'><span class='ContactsMsg'>Grant this site access to your contacts</span></a>", siteName));
            }
            base.Render(output);
        }
    }
}