using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Live.ServerControls;
using WindowsLive;
using System.Collections;

public partial class Invite : System.Web.UI.Page
{
    string lid = string.Empty;
    string delegationToken = string.Empty;
    long llid = 0;

    // Initialize the WindowsLiveLogin module.
    static WindowsLiveLogin wll = new WindowsLiveLogin(true);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Get the lid and delegation token.
            // We have saved the consent token to session state, all we 
            // need to do is extract the values from the consent token.
            if ((WindowsLiveLogin.ConsentToken)Session["ConsentToken"] != null)
            {
                WindowsLiveLogin.ConsentToken ct = (WindowsLiveLogin.ConsentToken)Session["ConsentToken"];

                // If a consent token is found and is stale, try to refresh it and store  
                // it in persistent storage.
                if (!ct.IsValid())
                {
                    if (ct.Refresh() && ct.IsValid())
                    {
                        Session["ConsentToken"] = ct.Token;
                    }
                }

                if (ct.IsValid())
                {
                    // Get the data values.            
                    lid = ct.LocationID;
                    llid = Int64.Parse(lid, System.Globalization.NumberStyles.HexNumber);
                    delegationToken = ct.DelegationToken;
                    InviteRequest.Visible = false;
                    PostBack.Visible = true;
                    GetContactList();
                }
                else
                {
                    InviteRequest.Visible = true;
                    AuthLink.NavigateUrl = GetConsentURI();
                    return;
                }
            }
            else
            {
                InviteRequest.Visible = true;
                AuthLink.NavigateUrl = GetConsentURI();
                return;
            }
        }
    }

    private string GetConsentURI()
    {
        //HACK: If you run this in localhost you need to use a FQDN - in this case we've replaced it with AngusTest1.com
        string fqdnURIbase = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace("localhost", "angustest1.com").ToLower();
        string returnURI = System.Web.HttpUtility.UrlEncode(fqdnURIbase.Replace("invite.aspx", "webauth-handler.aspx"));
        string privacyURI = System.Web.HttpUtility.UrlEncode(fqdnURIbase.Replace("invite.aspx", "privacy.aspx"));

        return string.Format("https://consent.live.com/Delegation.aspx?ps=Contacts.Invite&appctx=Invite.aspx&ru={0}&pl={1}", returnURI, privacyURI);
    }

    protected void PostBack_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < ListView.Items.Count; i++)
        {
            // Iterate through list items
            ListViewDataItem item = ListView.Items[i];

            // Get form values
            CheckBox checkBox = (CheckBox)item.FindControl("CheckBox");

            bool selected = checkBox.Checked;

            if (selected)
            {
                // Get data keys
                DataKey key = ListView.DataKeys[i];

                string name = (string)key["Name"];
                string email = (string)key["Email"];
                string owner = (string)key["Owner"];

                // Add friend to database
                if (Membership.GetUserNameByEmail(email) == null)
                {
                    // Does the invited members list exist?
                    if (Session["invitedMembers"] == null)
                    {
                        // no create it in session
                        Session["invitedMembers"] = new System.Collections.ArrayList();
                    }

                    // add the member to the session variable
                    ((System.Collections.ArrayList)Session["invitedMembers"]).Add(email);
                }
            }
        }

        Response.Redirect("~/Invite.aspx");
    }

    protected void GetContactList()
    {
        // Don't continue of we don't have the right credentials
        if (lid == string.Empty && delegationToken == string.Empty)
        {
            return;
        }

        // Construct the request URI
        string uri = string.Format("https://livecontacts.services.live.com/users/@C@{0}/rest/invitationsbyemail", llid);

        // Create a WebClient and add the delegation token to the header
        WebClient client = new WebClient();

        client.Headers["Authorization"] = "DelegatedToken dt=\"" + delegationToken + "\"";

        try
        {
            using (XmlReader reader = XmlReader.Create(client.OpenRead(uri)))
            {
                XDocument document = XDocument.Load(reader);

                // Get the owner of the contact list
                string owner = document.Root.Element("Owner").Element("WindowsLiveID").Value;

                // Create a data structure for displaying in the list view
                var withName = from contact in document.Descendants("Contact")
                               let personal = contact.Element("Profiles").Element("Personal")
                               where personal.HasElements &&
                                     personal.Element("FirstName") != null &&
                                     !personal.Element("FirstName").IsEmpty &&
                                     personal.Element("LastName") != null &&
                                     !personal.Element("LastName").IsEmpty
                               select new
                               {
                                   Name = personal.Element("FirstName").Value + " " +
                                          personal.Element("LastName").Value,
                                   Email = contact.Element("PreferredEmail").Value
                               };

                var mailOnly = from contact in document.Descendants("Contact")
                               select new
                               {
                                   Name = contact.Element("PreferredEmail").Value,
                                   Email = contact.Element("PreferredEmail").Value
                               };

                var contacts = from m in mailOnly
                               join n in withName on m.Email equals n.Email into mn
                               from item in mn.DefaultIfEmpty(new { Name = m.Name, Email = m.Email })
                               orderby item.Name
                               select new
                               {
                                   Name = item.Name, // get the name
                                   Email = item.Email, // get the email
                                   Enabled = !this.InvitedMembers.Contains(item.Email), //check if the current user has been invited in this session
                                   AlreadyMember = (Membership.GetUser(item.Email) != null), // check if the user is registered in the database
                                   PresenceID = UserPresenceInformation(item.Email), // if the user is registered and is sharing presence get the ID
                                   Owner = owner
                               };

                // Bind data to list view
                ListView.DataSource = contacts;
                ListView.DataBind();
            }
        }
        catch (Exception)
        {
        }
    }

    public string UserPresenceInformation(string username)
    {
        ProfileCommon _profile = Profile.GetProfile(username);

        if (_profile != null)
        {
            return (_profile.Presence.ID);
        }
        else
        {
            return (string.Empty);
        }
    }

    /// <summary>
    /// This property wraps the session object.
    /// </summary>
    public System.Collections.ArrayList InvitedMembers
    {
        get
        {
            if (Session["invitedMembers"] == null)
            {
                return new ArrayList();
            }
            else
            {
                return (ArrayList)Session["invitedMembers"];
            }
        }
    }
}