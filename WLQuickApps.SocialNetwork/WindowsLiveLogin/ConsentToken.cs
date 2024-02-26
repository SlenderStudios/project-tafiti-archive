using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;

/// <summary>
/// Holds the Consent Token object corresponding to consent granted. 
/// </summary>
public class ConsentToken
{
    #region private Data
    private WindowsLiveLogin wll;
    private string returnUrl;
    private bool forceDelAuthNonProvisioned = false;
    private string appId;
    private string delegationToken;
    private string refreshToken;
    private byte[] sessionKey;
    private DateTime expiry;
    private ArrayList offers;
    private string offersString;
    private string locationID;
    private string context;
    private string decodedToken;
    private string token;
    #endregion

    #region Constructors
    public ConsentToken(WindowsLiveLogin wll, string token)
    {
        this.wll = wll;
        ProcessConsentToken(token);
    }

    public ConsentToken(WindowsLiveLogin wll, NameValueCollection query)
    {
        this.wll = wll;
        ProcessConsent(query);
    }

    public ConsentToken(WindowsLiveLogin wll, string delegationToken, string refreshToken, string sessionKey, string expiry, string offers, string locationID, string context, string decodedToken, string token)
    {
        this.wll = wll;
        setDelegationToken(delegationToken);
        setRefreshToken(refreshToken);
        setSessionKey(sessionKey);
        setExpiry(expiry);
        setOffers(offers);
        setLocationID(locationID);
        setContext(context);
        setDecodedToken(decodedToken);
        setToken(token);
    }
    #endregion

    /// <summary>
    /// Gets the Delegation token.
    /// </summary>
    public string DelegationToken { get { return delegationToken; } }

    /// <summary>
    /// Sets the Delegation token.
    /// </summary>
    /// <param name="delegationToken">Delegation token</param>
    private void setDelegationToken(string delegationToken)
    {
        if (string.IsNullOrEmpty(delegationToken))
        {
            throw new ArgumentException("Error: ConsentToken: Null delegation token.");
        }

        this.delegationToken = delegationToken;
    }

    /// <summary>
    /// Gets the refresh token.
    /// </summary>
    public string RefreshToken { get { return refreshToken; } }

    /// <summary>
    /// Sets the refresh token.
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    private void setRefreshToken(string refreshToken)
    {
        this.refreshToken = refreshToken;
    }

    /// <summary>
    /// Gets the session key.
    /// </summary>
    public byte[] SessionKey { get { return sessionKey; } }

    /// <summary>
    /// Sets the session key.
    /// </summary>
    /// <param name="sessionKey">Session key</param>
    private void setSessionKey(string sessionKey)
    {
        if (string.IsNullOrEmpty(sessionKey))
        {
            throw new ArgumentException("Error: ConsentToken: Null session key.");
        }

        this.sessionKey = Helper.u64(sessionKey);
    }

    /// <summary>
    /// Gets the expiry time of delegation token.
    /// </summary>
    public DateTime Expiry { get { return expiry; } }

    /// <summary>
    /// Sets the expiry time of delegation token.
    /// </summary>
    /// <param name="expiry">Expiry time</param>
    private void setExpiry(string expiry)
    {
        if (string.IsNullOrEmpty(expiry))
        {
            throw new ArgumentException("Error: ConsentToken: Null expiry time.");
        }

        int expiryInt;

        try
        {
            expiryInt = Convert.ToInt32(expiry);
        }
        catch (Exception)
        {
            throw new ArgumentException("Error: Consent: Invalid expiry time: "
                                        + expiry);
        }

        DateTime refTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        this.expiry = refTime.AddSeconds(expiryInt);
    }

    /// <summary>
    /// Gets the list of offers/actions for which the user granted consent.
    /// </summary>
    public ArrayList Offers { get { return offers; } }

    /// <summary>
    /// Gets the string representation of all the offers/actions for which 
    /// the user granted consent.
    /// </summary>
    public String OffersString { get { return offersString; } }

    /// <summary>
    /// Sets the offers/actions for which user granted consent.
    /// </summary>
    /// <param name="offers">Comma-delimited list of offers</param>
    private void setOffers(string offers)
    {
        if (string.IsNullOrEmpty(offers))
        {
            throw new ArgumentException("Error: ConsentToken: Null offers.");
        }

        offers = HttpUtility.UrlDecode(offers);

        this.offersString = string.Empty;
        this.offers = new ArrayList();

        string[] offersList = offers.Split(new Char[] { ';' });

        foreach (string offer in offersList)
        {
            if (!(this.offersString == string.Empty))
            {
                this.offersString += ",";
            }

            int separator = offer.IndexOf(':');
            if (separator == -1)
            {
                this.offers.Add(offer);
                this.offersString += offer;
            }
            else
            {
                string o = offer.Substring(0, separator);
                this.offers.Add(o);
                this.offersString += o;
            }
        }
    }

    /// <summary>
    /// Gets the location ID.
    /// </summary>
    public string LocationID { get { return locationID; } }

    /// <summary>
    /// Sets the location ID.
    /// </summary>
    /// <param name="locationID">Location ID</param>
    private void setLocationID(string locationID)
    {
        this.locationID = locationID;
    }

    /// <summary>
    /// Returns the application context that was originally passed 
    /// to the consent request, if any.
    /// </summary>
    public string Context { get { return context; } }

    /// <summary>
    /// Sets the application context.
    /// </summary>
    /// <param name="context">Application context</param>
    private void setContext(string context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets the decoded token.
    /// </summary>
    public string DecodedToken { get { return decodedToken; } }

    /// <summary>
    /// Sets the decoded token.
    /// </summary>
    /// <param name="decodedToken">Decoded token</param>
    private void setDecodedToken(string decodedToken)
    {
        this.decodedToken = decodedToken;
    }

    /// <summary>
    /// Gets the raw token.
    /// </summary>
    public string Token { get { return token; } }

    /// <summary>
    /// Sets the raw token.
    /// </summary>
    /// <param name="token">Raw token</param>
    private void setToken(string token)
    {
        this.token = token;
    }

    /// <summary>
    /// Indicates whether the delegation token is set and has not expired.
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        if (string.IsNullOrEmpty(DelegationToken))
        {
            return false;
        }

        if (DateTime.UtcNow.AddSeconds(-300) > Expiry)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Processes the POST response from the Delegated Authentication 
    /// service after a user has granted consent. The processConsent
    /// function extracts the consent token string and returns the result 
    /// of invoking the processConsentToken method. 
    /// </summary>
    /// <param name="query">Response from the Delegated Authentication service.</param>
    /// <returns>ConsentToken</returns>
    private void ProcessConsent(NameValueCollection query)
    {
        if (query == null)
        {
            Helper.debug("Error: ProcessConsent: Invalid query.");
            return;
        }

        string action = query["action"];

        if (action != "delauth")
        {
            Helper.debug("Warning: ProcessConsent: query action ignored: " + action);
            return;
        }

        if (query["ResponseCode"] != "RequestApproved")
        {
            Helper.debug("Error: ProcessConsent: Consent was not successfully granted: " + query["ResponseCode"]);
            return;
        }

        ProcessConsentToken(query["ConsentToken"]);
    }

    /// <summary>
    /// Processes the consent token string that is returned in the POST 
    /// response by the Delegated Authentication service after a 
    /// user has granted consent.
    /// </summary>
    /// <param name="token">Raw token.</param>
    /// response for site-specific use.</param>
    /// <returns></returns>
    private void ProcessConsentToken(string token)
    {
        string decodedToken = token;

        if (string.IsNullOrEmpty(token))
        {
            Helper.debug("Error: ProcessConsentToken: Null token.");
            return;
        }

        NameValueCollection parsedToken = Helper.parse(HttpUtility.UrlDecode(token));

        if (!string.IsNullOrEmpty(parsedToken["eact"]))
        {
            decodedToken = Helper.DecodeAndValidateToken(parsedToken["eact"], WindowsLiveLogin.Secret);
            if (string.IsNullOrEmpty(decodedToken))
            {
                Helper.debug("Error: ProcessConsentToken: Failed to decode/validate token: " + token);
                return;
            }

            parsedToken = Helper.parse(decodedToken);
            decodedToken = HttpUtility.UrlEncode(decodedToken);
        }

        try
        {
            setDelegationToken(parsedToken["delt"]);
            setRefreshToken(parsedToken["reft"]);
            setSessionKey(parsedToken["skey"]);
            setExpiry(parsedToken["exp"]);
            setOffers(parsedToken["offer"]);
            setLocationID(parsedToken["lid"]);
            setContext(context);
            setDecodedToken(decodedToken);
            setToken(token);
        }
        catch (Exception e)
        {
            Helper.debug("Error: ProcessConsentToken: Contents of token considered invalid: " + e);
        }
    }

    /// <summary>
    /// Returns the URL to use to download a new consent token, given the 
    /// offers and refresh token.
    /// </summary>
    /// <param name="offers">Comma-delimited list of offers.</param>
    /// <param name="refreshToken">Refresh token.</param>
    /// <returns>Refresh consent token URL</returns>
    /// <param name="ru">The registered/configured return URL will be 
    /// overridden by 'ru' specified here.</param>
    /// <returns>Refresh consent token URL</returns>
    public string GetRefreshConsentTokenUrl(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentException("Error: GetRefreshConsentTokenUrl: Invalid refresh token.");
        }

        return WindowsLiveLogin.GetRefreshConsentUrl(refreshToken);
    }

    /// <summary>
    /// Sets or gets the return URL--the URL on your site to which the consent 
    /// service redirects users (along with the action, consent token, 
    /// and application context) after they have successfully provided 
    /// consent information for Delegated Authentication. 
    /// 
    /// This value will override the return URL specified during 
    /// registration.
    /// </summary>
    public string ReturnUrl
    {
        set
        {
            if (string.IsNullOrEmpty(value) && ForceDelAuthNonProvisioned)
            {
                throw new ArgumentNullException("value");
            }

            returnUrl = value;
        }

        get
        {
            if (string.IsNullOrEmpty(returnUrl) && ForceDelAuthNonProvisioned)
            {
                throw new InvalidOperationException("Error: ReturnUrl: Return URL must be specified in a delegated auth non-provisioned scenario. Aborting.");
            }

            return returnUrl;
        }
    }

    /// <summary>
    /// Sets or gets a flag that indicates whether Delegated Authentication
    /// is non-provisioned (i.e. does not use an application ID or secret
    /// key).
    /// </summary>
    public bool ForceDelAuthNonProvisioned
    {
        set { forceDelAuthNonProvisioned = value; }

        get { return forceDelAuthNonProvisioned; }
    }

    /// <summary>
    /// Gets or sets the application ID.
    /// </summary>
    public string AppId
    {
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                if (ForceDelAuthNonProvisioned)
                {
                    return;
                }

                throw new ArgumentNullException("value");
            }

            Regex re = new Regex(@"^\w+$");
            if (!re.IsMatch(value))
            {
                throw new ArgumentException("Error: AppId: Application ID must be alphanumeric: " + value);
            }

            appId = value;
        }

        get
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new InvalidOperationException("Error: AppId: Application ID was not set. Aborting.");
            }

            return appId;
        }
    }
}

