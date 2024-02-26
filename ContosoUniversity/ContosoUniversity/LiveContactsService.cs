using System;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Configuration;

/// <summary>
/// Simple web service that acts a a proxy to the Live Contacts REST API
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class LiveContactsService : System.Web.Services.WebService
{

    private string addressBookOwnerHandle = WebConfigurationManager.AppSettings["AddressBookOwnerHandle"];
    private string addressBookOwnerAuthToken = WebConfigurationManager.AppSettings["AddressBookOwnerAuthToken"];

    /// <summary>
    /// Saves a lat/long to a contact record in the master address book
    /// </summary>
    /// <param name="ownerHandle">Windows Live ID of the contact</param>
    /// <param name="latitude">Latitude</param>
    /// <param name="longitude">Longitude</param>
    /// <param name="locationText">Description of user's current location</param>
    /// <returns>Textual description of success</returns>
    [ScriptMethod]
    [WebMethod(Description = "Save Location", EnableSession = true)]
    public string SaveMyLocation(double latitude, double longitude, string locationText)
    {
        // Create a new live contacts object.
        LiveContacts lc = new LiveContacts();

        // Save the location to site administrators address book
        return lc.SaveMyLocation(addressBookOwnerHandle, addressBookOwnerAuthToken, latitude, longitude, locationText);
    }
}

