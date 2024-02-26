using System;
using System.Net;
using System.Xml.XPath;
using System.Xml;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Web;
using WindowsLive;

/// <summary>
/// Windows Live Contacts wrapper class
/// </summary>
public class LiveContacts
{
    private string template = @"
                                <Contact>
	                                <Emails>
		                                <Email>
			                                <EmailType>Personal</EmailType>
			                                <Address>#EMAILADDRESS#</Address>
			                                <IsDefault>false</IsDefault>
		                                </Email>
	                                </Emails>
	                                <Locations>
		                                <Location>
			                                <LocationType>Personal</LocationType>
			                                <CompanyName>Contoso University</CompanyName>
                                            <StreetLine>#TEXT#</StreetLine>
                                            <Latitude>#LATITUDE#</Latitude>
			                                <Longitude>#LONGITUDE#</Longitude>
			                                <IsDefault>true</IsDefault>
		                                </Location>
	                                </Locations>
                                </Contact>";

    /// <summary>
    /// Constructor
    /// </summary>
    public LiveContacts()
    {
        // Create custom SSL handler
        ServicePointManager.ServerCertificateValidationCallback +=
            delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                bool validationResult = true;
                return validationResult;
            };
    }
    /// <summary>
    /// //get the Windows Live ID of current login user.
    /// </summary>
    /// <returns>Windows Live ID</returns>
    public string GetOwnerEmailAddess()
    {
        string ownerHandle = null;
        string delegationToken = null;
        // Setup the contacts control
        if (HttpContext.Current.Session["ConsentToken"] != null)
        {
            WindowsLiveLogin.ConsentToken consentToken = (WindowsLiveLogin.ConsentToken)HttpContext.Current.Session["ConsentToken"];
            ownerHandle = (Int64.Parse(consentToken.LocationID, System.Globalization.NumberStyles.HexNumber)).ToString();
            delegationToken = consentToken.DelegationToken;
        }
        string contactsUri = string.Format("{0}/LiveContacts", ownerHandle);
        string result = SendHttpRequest(ref contactsUri, delegationToken, "GET", null);
        XmlDocument contactsXml = new XmlDocument();
        contactsXml.LoadXml(result);
        string contactNodeUri = "/LiveContacts/Owner/WindowsLiveID";
        XmlNode contactNode = contactsXml.SelectSingleNode(contactNodeUri);
        if (contactNode != null)
        {
            return contactNode.InnerText;
        }
        return null;
    }
    /// <summary>
    /// Saves a lat/long to a contact record in the master address book
    /// </summary>
    /// <param name="ownerHandle">Windows Live ID of the contact</param>
    /// <param name="latitude">Latitude</param>
    /// <param name="longitude">Longitude</param>
    /// <param name="locationText">Description of user's current location</param>
    /// <returns>Textual description of success</returns>
    public string SaveMyLocation(string addressBookOwnerHandle, string addressBookOwnerAuthToken, double latitude, double longitude, string locationText)
    {
        string ownerHandle = GetOwnerEmailAddess(); //get the Windows Live ID of current login user.

        // Get a list of contacts from administration address book
        string contactsUri = string.Format("{0}/LiveContacts", addressBookOwnerHandle); // "?filter=LiveContacts(Contact(ID,WindowsLiveID,Locations))";

        // Execute the the request and get the response back.
        string result = SendHttpRequest(ref contactsUri, addressBookOwnerAuthToken, "GET", null);

        // See if there is a contact for the current ownerHandle
        XmlDocument contactsXml = new XmlDocument();
        contactsXml.LoadXml(result);
        string contactNodeUri = string.Format("/LiveContacts/Contacts/Contact/Emails/Email[Address='{0}']", ownerHandle);
        XmlNode contactNode = contactsXml.SelectSingleNode(contactNodeUri);

        // Clean up the location text (remove any html tages & silently trim to 256 chars, the max allowed for the StreetLine field
        locationText = HtmlProcessor.RemoveTags(locationText);
        if (locationText.Length > 256) locationText = locationText.Substring(0, 256);

        // Found it
        if (contactNode != null)
        {
            //Does it have a location
            XmlNode locationNode = contactsXml.SelectSingleNode("/LiveContacts/Contacts/Contact/Locations/Location[LocationType='Personal']");

            // Yes it does, so update it
            if (locationNode != null)
            {
                string updateXmlBody = string.Format("<Location><Latitude>{0}</Latitude><Longitude>{1}</Longitude><StreetLine>{2}</StreetLine></Location>", latitude, longitude, locationText);
                string updateUri = string.Format("{0}/LiveContacts/Contacts/Contact({1})/locations/location({2})", addressBookOwnerHandle, contactNode.ParentNode.ParentNode["ID"].InnerText, locationNode["ID"].InnerText);
                result = SendHttpRequest(ref updateUri, addressBookOwnerAuthToken, "PUT", updateXmlBody);
                result = "Location node updated for " + ownerHandle;
            }
            // No, so let's add it
            else
            {
                string updateXmlBody = string.Format("<Location><LocationType>Personal</LocationType><PrimaryCity>Contoso</PrimaryCity><Latitude>{0}</Latitude><Longitude>{1}</Longitude><StreetLine>{2}</StreetLine><IsDefault>true</IsDefault></Location>", latitude, longitude, locationText);
                string updateUri = string.Format("{0}/LiveContacts/Contacts/Contact({1})/Locations", addressBookOwnerHandle, contactNode.ParentNode.ParentNode["ID"].InnerText);
                result = SendHttpRequest(ref updateUri, addressBookOwnerAuthToken, "PUT", updateXmlBody);
                result = "Location node added for " + ownerHandle;
            }
        }
        // Didn't find it, so add a new one
        else
        {
            string contactXml = template.Replace("#EMAILADDRESS#", ownerHandle).Replace("#LATITUDE#", latitude.ToString()).Replace("#LONGITUDE#", longitude.ToString()).Replace("#TEXT#", locationText);
            string updateUri = string.Format("{0}/LiveContacts/Contacts", addressBookOwnerHandle);
            result = SendHttpRequest(ref updateUri, addressBookOwnerAuthToken, "POST", contactXml);
            result = "Contact added for " + ownerHandle;
        }
        return result;
    }

    public string GetContacts(string uri, string delegatedToken)
    {
        string xml = SendHttpRequest(ref uri, delegatedToken, "GET", null);
        return xml;
    }

    public string SendHttpRequest(ref string uri, string delegatedToken, string requestMethod, string contactData)
    {
        string uriPath = string.Format("https://livecontacts.services.live.com/users/@C@{0}", uri);

        // Create HTTP request object.
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriPath);

        // Add the authentication header
        request.Headers.Add("Authorization", "DelegatedToken dt=\"" + delegatedToken + "\"");

        request.CookieContainer = new CookieContainer();

        // Set the HTTP request method. For example, this can be POST, GET, and so on.
        request.Method = requestMethod;

        request.ContentType = "application/xml; charset=utf-8";

        // For POST or PUT requests, load the xml and write it to the request stream after the header.
        if (requestMethod.Equals("POST") || requestMethod.Equals("PUT"))
        {
            XmlDocument contactXml = new XmlDocument();
            contactXml.LoadXml(contactData);
            contactXml.Save(request.GetRequestStream());

            // Close the request stream to release the resource.
            request.GetRequestStream().Close();
        }

        // Send the request and get the response
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        // If this is a contact created by POST, update uriPath.
        if (requestMethod.Equals("POST"))
            uri = response.Headers[HttpResponseHeader.Location].Substring(33);

        // Save the response text
        StreamReader readStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

        // pull pull all the response text out of the stream.
        string text = readStream.ReadToEnd();

        // Close the response to release the resource.
        response.Close();

        return text;
    }
}