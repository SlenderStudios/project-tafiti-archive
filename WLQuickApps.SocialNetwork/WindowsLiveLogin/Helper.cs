using System;
using System.Net;
using System.Collections.Specialized;
using System.Web;
using System.IO;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Provides auxilliary functions for the other classes.
/// </summary>
class Helper
{
    /// <summary>
    /// Fetches the contents given a URL.
    /// </summary>
    internal static string fetch(string url)
    {
        string body = null;
        try
        {
            WebRequest req = HttpWebRequest.Create(url);
            req.Method = "GET";
            WebResponse res = req.GetResponse();
            using (StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
            {
                body = sr.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            debug("Error: fetch: Failed to get the document: " + url + ", " + e);
        }
        return body;
    }

    /// <summary>
    /// Base64-encodes and URL-escapes a byte array.
    /// </summary>
    internal static string e64(byte[] b)
    {
        string s = null;
        if (b == null) { return s; }

        try
        {
            s = Convert.ToBase64String(b);
            s = HttpUtility.UrlEncode(s);
        }
        catch (Exception e)
        {
            debug("Error: e64: Base64 conversion error: " + e);
        }

        return s;
    }

    /// <summary>
    /// URL-unescapes and Base64-decodes a string.
    /// </summary>
    internal static byte[] u64(string s)
    {
        byte[] b = null;
        if (s == null) { return b; }
        s = HttpUtility.UrlDecode(s);

        try
        {
            b = Convert.FromBase64String(s);
        }
        catch (Exception e)
        {
            debug("Error: u64: Base64 conversion error: " + s + ", " + e);
        }
        return b;
    }

    /// <summary>
    /// Parses query string and return a table representation of 
    /// the key and value pairs.  Similar to 
    /// HttpUtility.ParseQueryString, except that no URL decoding
    /// is done and only the last value is considered in the case
    /// of multiple values with one key.
    /// </summary>
    internal static NameValueCollection parse(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            debug("Error: parse: Null input.");
            return null;
        }

        NameValueCollection pairs = new NameValueCollection();

        string[] kvs = input.Split(new Char[] { '&' });
        foreach (string kv in kvs)
        {
            int separator = kv.IndexOf('=');

            if ((separator == -1) || (separator == kv.Length))
            {
                debug("Warning: parse: Ignoring pair: " + kv);
                continue;
            }

            pairs[kv.Substring(0, separator)] = kv.Substring(separator + 1);
        }

        return pairs;
    }

    /// <summary>
    /// Generates a timestamp suitable for the application
    /// verifier token.
    /// </summary>
    internal static string getTimestamp()
    {
        DateTime refTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan ts = DateTime.UtcNow - refTime;
        return ((uint)ts.TotalSeconds).ToString();
    }

    /// <summary>
    /// Derives the key, given the secret key and prefix as described in the 
    /// Web Authentication SDK documentation.
    /// </summary>
    private static byte[] derive(string secret, string prefix)
    {
        using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
        {
            const int keyLength = 16;
            byte[] data = Encoding.Default.GetBytes(prefix + secret);
            byte[] hashOutput = hashAlg.ComputeHash(data);
            byte[] byteKey = new byte[keyLength];
            Array.Copy(hashOutput, byteKey, keyLength);
            return byteKey;
        }
    }

    /// <summary>
    /// Derives the signature or encryption key, given the secret key 
    /// and prefix as described in the SDK documentation.
    /// </summary>
    internal static byte[] DeriveKey(string prefix, string secret)
    {
        using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
        {
            int keyLength = 16;

            byte[] data = System.Text.Encoding.Default.GetBytes(prefix + secret);
            byte[] hashOutput = hashAlg.ComputeHash(data);
            byte[] byteKey = new byte[keyLength];

            Array.Copy(hashOutput, byteKey, keyLength);
            return byteKey;
        }
    }

    /// <summary>
    /// Decodes and validates the raw token with appropriate crypt key 
    /// and sign key.
    /// </summary>
    /// <param name="token">Raw token.</param>
    /// <param name="cryptKey">Crypt key.</param>
    /// <param name="signKey">Sign key.</param>
    /// <returns></returns>
    internal static string DecodeAndValidateToken(string token, string secret)
    {
        byte[] cryptKey = derive(secret, "ENCRYPTION");
        byte[] signKey = derive(secret, "SIGNATURE");

        string stoken = DecodeToken(token, cryptKey);

        if (!string.IsNullOrEmpty(stoken))
        {
            stoken = ValidateToken(stoken, signKey);
        }

        return stoken;
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
    /// <param name="token">Raw token.</param>
    /// <param name="cryptKey">Crypt key.</param>
    /// <returns>Decoded token.</returns>
    private static string DecodeToken(string token, byte[] cryptKey)
    {
        if (cryptKey == null || cryptKey.Length == 0)
        {
            throw new InvalidOperationException("Error: DecodeToken: Secret key was not set. Aborting.");
        }

        if (string.IsNullOrEmpty(token))
        {
            debug("Error: DecodeToken: Null token input.");
            return null;
        }

        const int ivLength = 16;
        byte[] ivAndEncryptedValue = u64(token);

        if ((ivAndEncryptedValue == null) ||
            (ivAndEncryptedValue.Length <= ivLength) ||
            ((ivAndEncryptedValue.Length % ivLength) != 0))
        {
            debug("Error: DecodeToken: Attempted to decode invalid token.");
            return null;
        }

        Rijndael aesAlg = null;
        MemoryStream memStream = null;
        CryptoStream cStream = null;
        StreamReader sReader = null;
        string decodedValue = null;

        try
        {
            aesAlg = new RijndaelManaged();
            aesAlg.KeySize = 128;
            aesAlg.Key = cryptKey;
            aesAlg.Padding = PaddingMode.PKCS7;
            memStream = new MemoryStream(ivAndEncryptedValue);
            byte[] iv = new byte[ivLength];
            memStream.Read(iv, 0, ivLength);
            aesAlg.IV = iv;
            cStream = new CryptoStream(memStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read);
            sReader = new StreamReader(cStream, Encoding.ASCII);
            decodedValue = sReader.ReadToEnd();
        }
        catch (Exception e)
        {
            debug("Error: DecodeToken: Decryption failed: " + e);
            return null;
        }
        finally
        {
            try
            {
                if (sReader != null) { sReader.Close(); }
                if (cStream != null) { cStream.Close(); }
                if (memStream != null) { memStream.Close(); }
                if (aesAlg != null) { aesAlg.Clear(); }
            }
            catch (Exception e)
            {
                debug("Error: DecodeToken: Failure during resource cleanup: " + e);
            }
        }

        return decodedValue;
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
    internal static string DecodeToken(string token, string secret)
    {
        byte[] cryptKey = Helper.DeriveKey(WebConstants.WindowsLiveVariables.EncryptionKeyPrefix, secret);
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

        return ValidateToken(decodedValue, secret);
    }

    /// <summary>
    /// Extracts the signature from the token and validates it by using the 
    /// signature key.
    /// </summary>
    private static string ValidateToken(string token, byte[] signKey)
    {
        if (string.IsNullOrEmpty(token))
        {
            debug("Error: ValidateToken: Null token.");
            return null;
        }

        string[] s = { "&sig=" };
        string[] bodyAndSig = token.Split(s, StringSplitOptions.None);

        if (bodyAndSig.Length != 2)
        {
            debug("Error: ValidateToken: Invalid token: " + token);
            return null;
        }

        byte[] sig = u64(bodyAndSig[1]);

        if (sig == null)
        {
            debug("Error: ValidateToken: Could not extract the signature from the token.");
            return null;
        }

        byte[] sig2 = SignToken(bodyAndSig[0], signKey);

        if (sig2 == null)
        {
            debug("Error: ValidateToken: Could not generate a signature for the token.");
            return null;
        }

        if (sig.Length == sig2.Length)
        {
            for (int i = 0; i < sig.Length; i++)
            {
                if (sig[i] != sig2[i]) { goto badSig; }
            }

            return token;
        }

    badSig:
        debug("Error: ValidateToken: Signature did not match.");
        return null;
    }

    /// <summary>
    /// Extracts the signature from the token and validates it.
    /// </summary>
    private static string ValidateToken(string token, string secret)
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

        byte[] sig2 = Helper.SignToken(bodyAndSig[0], secret);

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
    internal static byte[] SignToken(string token, string secret)
    {
        byte[] signKey = Helper.DeriveKey(WebConstants.WindowsLiveVariables.SignatureKeyPrefix, secret);

        using (HashAlgorithm hashAlgorithm = new HMACSHA256(signKey))
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(token);
            byte[] hash = hashAlgorithm.ComputeHash(data);

            return hash;
        }
    }

    /// <summary>
    /// Creates a signature for the given string by using the
    /// signature key.
    /// </summary>
    private static byte[] SignToken(string token, byte[] signKey)
    {
        if (signKey == null || signKey.Length == 0)
        {
            throw new InvalidOperationException("Error: SignToken: Secret key was not set. Aborting.");
        }

        if (string.IsNullOrEmpty(token))
        {
            debug("Attempted to sign null token.");
            return null;
        }

        using (HashAlgorithm hashAlg = new HMACSHA256(signKey))
        {
            byte[] data = Encoding.Default.GetBytes(token);
            byte[] hash = hashAlg.ComputeHash(data);
            return hash;
        }
    }
    
    /// <summary>
    /// Stub implementation for logging debug output. You can run
    /// a tool such as 'dbmon' to see the output.
    /// </summary>
    internal static void debug(string msg)
    {
        System.Diagnostics.Debug.WriteLine(msg);
        System.Diagnostics.Debug.Flush();
    }
}
