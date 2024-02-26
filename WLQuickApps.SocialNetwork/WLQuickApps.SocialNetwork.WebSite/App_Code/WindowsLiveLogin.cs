using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Principal;
using WLQuickApps.SocialNetwork.Business;
using System.Web.SessionState;
using System.IO;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace WLQuickApps.SocialNetwork.WebSite
{
    /// <summary>
    /// Provides forms authentication via the Windows Live ID
    /// Web Authentication service.
    /// </summary>
    public class WindowsLiveLogin
    {
        public static bool AuthenticateUser(out string windowsLiveUUID)
        {
            windowsLiveUUID = null;
            string action = HttpContext.Current.Request[WebConstants.WindowsLiveVariables.Action];

            if (action == WebConstants.WindowsLiveVariables.LogoutAction)
            {
                WindowsLiveLogin.LogoutUser();
                HttpContext.Current.Response.Redirect(FormsAuthentication.DefaultUrl);

                return false;
            }
            else if (action == WebConstants.WindowsLiveVariables.ClearCookieAction)
            {
                WindowsLiveLogin.LogoutUser();
                HttpContext.Current.Response.Redirect("~/Images/signout_good.gif");

                return false;
            }
            else
            {
                string token = HttpContext.Current.Request[WebConstants.WindowsLiveVariables.Token];
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                token = WindowsLiveLogin.DecodeToken(token);
                if (string.IsNullOrEmpty(token))
                {
                    HealthMonitoringManager.LogWarning("Unable to decode Windows Live token.");
                    return false;
                }

                NameValueCollection parsedToken = HttpUtility.ParseQueryString(token);
                if (parsedToken == null || parsedToken.Count < 3)
                {
                    HealthMonitoringManager.LogWarning("Unable to parse Windows Live token.");
                    return false;
                }

                string appID = parsedToken[WebConstants.WindowsLiveVariables.AppID];
                if (appID != SettingsWrapper.WindowsLiveAppID)
                {
                    HealthMonitoringManager.LogError("Received Windows Live token with incorrect AppID.");
                    return false;
                }

                windowsLiveUUID = parsedToken[WebConstants.WindowsLiveVariables.UID];
                HttpContext.Current.Session[WebConstants.SessionVariables.WindowsLiveUUID] = windowsLiveUUID;

                return true;
            }
        }

        public static string GetWindowsLiveUUID()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            else
            {
                return (HttpContext.Current.Session[WebConstants.SessionVariables.WindowsLiveUUID] as string);
            }
        }

        public static bool IsUserAuthenticated()
        {
            return (!string.IsNullOrEmpty(WindowsLiveLogin.GetWindowsLiveUUID()));
        }

        /// <summary>
        /// Decode the given token. Returns null on failure.
        /// </summary>
        ///
        /// <list type="number">
        /// <item>First, the string is URL unescaped and base64
        /// decoded.</item>
        /// <item>Second, the IV is extracted from the first 16 bytes
        /// of the string.</item>
        /// <item>Finally, the string is decrypted by using the
        /// encryption key.</item> 
        /// </list>
        private static string DecodeToken(string token)
        {
            byte[] cryptKey = WindowsLiveLogin.DeriveKey(WebConstants.WindowsLiveVariables.EncryptionKeyPrefix);
            if (cryptKey == null || cryptKey.Length == 0)
            {
                return null;
            }

            const int ivLength = 16;
            byte[] ivAndEncryptedValue = Convert.FromBase64String(HttpUtility.UrlDecode(token));

            if ((ivAndEncryptedValue == null) ||
                (ivAndEncryptedValue.Length <= ivLength) ||
                ((ivAndEncryptedValue.Length % ivLength) != 0))
            {
                return null;
            }

            Rijndael aesAlg = new RijndaelManaged();
            aesAlg.KeySize = 128;
            aesAlg.Key = cryptKey;
            aesAlg.Padding = PaddingMode.PKCS7;

            string decodedValue;
            using (MemoryStream memoryStream = new MemoryStream(ivAndEncryptedValue))
            {
                byte[] iv = new byte[ivLength];
                memoryStream.Read(iv, 0, ivLength);
                aesAlg.IV = iv;

                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(cryptoStream, System.Text.Encoding.ASCII))
                {
                    decodedValue = reader.ReadToEnd();
                }

                aesAlg.Clear();
            }

            return WindowsLiveLogin.ValidateToken(decodedValue);
        }

        /// <summary>
        /// Extracts the signature from the token and validates it.
        /// </summary>
        private static string ValidateToken(string token)
        {
            if (token == null || token.Length == 0)
            {
                return null;
            }

            string[] s = { string.Format("&{0}=", WebConstants.WindowsLiveVariables.Signature) };
            string[] bodyAndSig = token.Split(s, StringSplitOptions.None);

            if (bodyAndSig.Length != 2)
            {
                return null;
            }

            byte[] sig = Convert.FromBase64String(HttpUtility.UrlDecode(bodyAndSig[1]));

            if (sig == null)
            {
                return null;
            }

            byte[] sig2 = WindowsLiveLogin.SignToken(bodyAndSig[0]);

            if (sig2 == null)
            {
                return null;
            }

            if (sig.Length == sig2.Length)
            {
                for (int i = 0; i < sig.Length; i++)
                {
                    if (sig[i] != sig2[i])
                    {
                        return null;
                    }
                }

                return token;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Creates a signature for the given string by using the
        /// signature key.
        /// </summary>
        private static byte[] SignToken(string token)
        {
            byte[] signKey = WindowsLiveLogin.DeriveKey(WebConstants.WindowsLiveVariables.SignatureKeyPrefix);

            using (HashAlgorithm hashAlgorithm = new HMACSHA256(signKey))
            {
                byte[] data = System.Text.Encoding.Default.GetBytes(token);
                byte[] hash = hashAlgorithm.ComputeHash(data);

                return hash;
            }
        }

        /// <summary>
        /// Derives the signature or encryption key, given the secret key 
        /// and prefix as described in the SDK documentation.
        /// </summary>
        private static byte[] DeriveKey(string prefix)
        {
            using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
            {
                int keyLength = 16;

                byte[] data = System.Text.Encoding.Default.GetBytes(prefix + SettingsWrapper.WindowsLiveSecret);
                byte[] hashOutput = hashAlg.ComputeHash(data);
                byte[] byteKey = new byte[keyLength];

                Array.Copy(hashOutput, byteKey, keyLength);
                return byteKey;
            }
        }

        /// <summary>
        /// Signs out of Windows Live ID and forms authentication.
        /// </summary>
        public static void LogoutUser()
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // Sign out of forms auth.
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        public static string GetLoginUrl()
        {
            return string.Format("http://login.live.com/wlogin.srf?{0}={1}&{2}={3}&alg=wsignin1.0",
                WebConstants.WindowsLiveVariables.AppID, SettingsWrapper.WindowsLiveAppID,
                WebConstants.WindowsLiveVariables.AppContext, HttpUtility.UrlEncode(WindowsLiveLogin.GetReturnUrl()));
        }

        public static string GetLogoutUrl()
        {
            return string.Format("http://login.live.com/logout.srf?{0}={1}", WebConstants.WindowsLiveVariables.AppID,
                SettingsWrapper.WindowsLiveAppID);
        }

        private static string GetReturnUrl()
        {
            HttpContext context = HttpContext.Current;

            if (!string.IsNullOrEmpty(context.Request.QueryString[WebConstants.QueryVariables.ReturnURL]))
            {
                return context.Request.QueryString[WebConstants.QueryVariables.ReturnURL];
            }
            else
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Url.PathAndQuery))
                {
                    return HttpContext.Current.Request.Url.PathAndQuery;
                }
                else
                {
                    return FormsAuthentication.DefaultUrl;
                }
            }
        }

        public static void LoginUser(string userName)
        {
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(userName, "FormsAuthentication"), null);
            FormsAuthentication.SetAuthCookie(userName, false);

            // Redirect user to original destination.
            string returnUrl = HttpContext.Current.Request[WebConstants.WindowsLiveVariables.AppContext];
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = FormsAuthentication.DefaultUrl;
            }

            // TODO: Determine if there's a better way to figure out an absolute URL given a virtual path.
            // For example, we get /Media/AddPicture.aspx?albumID=2 and want 
            // http://localhost/AppPath/Media/AddPicture.aspx?albumID=2 if the app is configured to root at
            // http://localhost/AppPath.
            // Note that VirtualPathUtility doesn't support query strings, so segments like
            // /AddPicture.aspx?albumID=2 throw exceptions.
            Uri nextUrl = new Uri(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + returnUrl);
            UriBuilder returnUrlBuilder = new UriBuilder(HttpContext.Current.Request.Url);
            returnUrlBuilder.Query = nextUrl.Query;
            returnUrlBuilder.Path = nextUrl.AbsolutePath;

            string alertsUrl = string.Empty;

            // Get the Alerts Signup URL
            alertsUrl = LiveAlertsWrapper.GetAlertsSignupUrl(returnUrlBuilder.ToString());

            if (alertsUrl.Length > 0)
            {
                HttpContext.Current.Response.Redirect(alertsUrl);
            }
            else
            {
                HttpContext.Current.Response.Redirect(returnUrl);
            }
        }
    }
}
