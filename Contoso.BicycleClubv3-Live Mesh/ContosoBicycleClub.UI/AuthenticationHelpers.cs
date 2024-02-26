//  *********************************************************************************
//  File:	AuthenticationHelpers.cs
//  Version: 1.1
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.
//  *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// 1.1
using System.Net;
using System.IO;
using System.Web.Script.Serialization;


/// <summary>
/// Contains code for managing authentication tokens.
/// </summary>
namespace WLQuickApps.ContosoBicycleClub.UI
{
    public class AuthenticationHelperClass
    {
        // Create a new instance of WindowsLiveLogin.
        WindowsLiveLogin wl_login = new WindowsLiveLogin(true);

        public AuthenticationHelperClass()
        {
        }

        //Get Renewed ConsentToken Object with RefreshToken
        public WindowsLiveLogin.ConsentToken getNewTokenwithRefreshToken(string offerString, string refreshToken, string delToken)
        {
            try
            {
                string refreshTokenURL = wl_login.GetRefreshConsentTokenUrl(offerString, refreshToken, delToken);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(refreshTokenURL);
                request.Method = "GET";
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                string jsonResult = streamReader.ReadToEnd();

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                string renewedRawTokenString = jsonSerializer.Deserialize<refreshTokenObj>(jsonResult).ConsentToken;
                return wl_login.ProcessConsentToken(renewedRawTokenString);
            }
            catch
            {
                return null;
            }
        }

        //ConsentToken Info Object to be Deserialized
        class refreshTokenObj
        {
            public string ConsentToken { get; set; }
        }

        // Create a cookie and assign keys to each piece of token data.
        public void CreateTokenCookie(WindowsLiveLogin.ConsentToken consentToken)
        {
            HttpCookie tokenCookie = new HttpCookie("consentToken");
            HttpResponse Response = HttpContext.Current.Response;
            tokenCookie["delToken"] = consentToken.DelegationToken;
            tokenCookie["refreshToken"] = consentToken.RefreshToken;
            tokenCookie["sessionKey"] = consentToken.SessionKey.ToString();
            tokenCookie["expiry"] = consentToken.Expiry.ToString();
            tokenCookie["offersString"] = consentToken.OffersString;
            tokenCookie["locationId"] = consentToken.LocationID;
            tokenCookie["context"] = consentToken.Context;
            tokenCookie["decodedtoken"] = consentToken.DecodedToken;
            tokenCookie["token"] = consentToken.Token;
            //tokenCookie.Expires = DateTime.Now.AddDays(30d);
            tokenCookie.Expires = consentToken.Expiry.AddDays(30d);
            Response.Cookies.Add(tokenCookie);
        }

        // This is a helper method to delete a cookie, any cookie.
        // Use for testing purposes.
        public void DeleteCookie(Cookie cookie)
        {
            if (cookie != null)
            {
                HttpCookie myCookie = new HttpCookie(cookie.Name);
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                HttpResponse Response = HttpContext.Current.Response;
                Response.Cookies.Add(myCookie);
            }
        }

    }
}