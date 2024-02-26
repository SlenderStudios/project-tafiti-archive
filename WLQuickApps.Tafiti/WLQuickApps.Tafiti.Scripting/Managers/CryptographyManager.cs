using System;
using System.DHTML;
using ScriptFX;
using System.XML;

namespace WLQuickApps.Tafiti.Scripting
{
    public class CryptographyManager
    {
        // I like things that rhyme.
        static private Dictionary HashCache
        {
            get
            {
                if (CryptographyManager._hashCache == null)
                {
                    CryptographyManager._hashCache = new Dictionary();
                }
                return CryptographyManager._hashCache;
            }
        }
        static private Dictionary _hashCache;

        static public string GetMD5Hash(string input)
        {
            if (CryptographyManager.HashCache.ContainsKey(input))
            {
                return (string)CryptographyManager.HashCache[input];
            }
            
            uint a = 0x67452301;
            uint b = 0xEFCDAB89;
            uint c = 0x98BADCFE;
            uint d = 0x10325476;
            byte s11 = 7;
            byte s12 = 12;
            byte s13 = 17; 
            byte s14 = 22;
            byte s21 = 5; 
            byte s22 = 9;
            byte s23 = 14;
            byte s24 = 20;
            byte s31 = 4;
            byte s32 = 11;
            byte s33 = 16;
            byte s34 = 23;
            byte s41 = 6;
            byte s42 = 10;
            byte s43 = 15;
            byte s44 = 21;

            UInt32[] words = CryptographyManager.ConvertToWordArray(input);
       
	        for (int lcv = 0; lcv < words.Length; lcv += 16)
            {
		        uint lastA = a;
                uint lastB = b;
                uint lastC = c;
                uint lastD = d;
		        a = FF(a, b, c, d, words[lcv + 0], s11, 0xD76AA478);
		        d = FF(d, a, b, c, words[lcv + 1], s12, 0xE8C7B756);
		        c = FF(c, d, a, b, words[lcv + 2], s13, 0x242070DB);
		        b = FF(b, c, d, a, words[lcv + 3], s14, 0xC1BDCEEE);
		        a = FF(a, b, c, d, words[lcv + 4], s11, 0xF57C0FAF);
		        d = FF(d, a, b, c, words[lcv + 5], s12, 0x4787C62A);
		        c = FF(c, d, a, b, words[lcv + 6], s13, 0xA8304613);
		        b = FF(b, c, d, a, words[lcv + 7], s14, 0xFD469501);
		        a = FF(a, b, c, d, words[lcv + 8], s11, 0x698098D8);
		        d = FF(d, a, b, c, words[lcv + 9], s12, 0x8B44F7AF);
		        c = FF(c, d, a, b, words[lcv + 10], s13, 0xFFFF5BB1);
		        b = FF(b, c, d, a, words[lcv + 11], s14, 0x895CD7BE);
		        a = FF(a, b, c, d, words[lcv + 12], s11, 0x6B901122);
		        d = FF(d, a, b, c, words[lcv + 13], s12, 0xFD987193);
		        c = FF(c, d, a, b, words[lcv + 14], s13, 0xA679438E);
		        b = FF(b, c, d, a, words[lcv + 15], s14, 0x49B40821);
		        a = GG(a, b, c, d, words[lcv + 1], s21, 0xF61E2562);
		        d = GG(d, a, b, c, words[lcv + 6], s22, 0xC040B340);
		        c = GG(c, d, a, b, words[lcv + 11], s23, 0x265E5A51);
		        b = GG(b, c, d, a, words[lcv + 0], s24, 0xE9B6C7AA);
		        a = GG(a, b, c, d, words[lcv + 5], s21, 0xD62F105D);
		        d = GG(d, a, b, c, words[lcv + 10], s22, 0x2441453);
		        c = GG(c, d, a, b, words[lcv + 15], s23, 0xD8A1E681);
		        b = GG(b, c, d, a, words[lcv + 4], s24, 0xE7D3FBC8);
		        a = GG(a, b, c, d, words[lcv + 9], s21, 0x21E1CDE6);
		        d = GG(d, a, b, c, words[lcv + 14], s22, 0xC33707D6);
		        c = GG(c, d, a, b, words[lcv + 3], s23, 0xF4D50D87);
		        b = GG(b, c, d, a, words[lcv + 8], s24, 0x455A14ED);
		        a = GG(a, b, c, d, words[lcv + 13], s21, 0xA9E3E905);
		        d = GG(d, a, b, c, words[lcv + 2], s22, 0xFCEFA3F8);
		        c = GG(c, d, a, b, words[lcv + 7], s23, 0x676F02D9);
		        b = GG(b, c, d, a, words[lcv + 12], s24, 0x8D2A4C8A);
		        a = HH(a, b, c, d, words[lcv + 5], s31, 0xFFFA3942);
		        d = HH(d, a, b, c, words[lcv + 8], s32, 0x8771F681);
		        c = HH(c, d, a, b, words[lcv + 11], s33, 0x6D9D6122);
		        b = HH(b, c, d, a, words[lcv + 14], s34, 0xFDE5380C);
		        a = HH(a, b, c, d, words[lcv + 1], s31, 0xA4BEEA44);
		        d = HH(d, a, b, c, words[lcv + 4], s32, 0x4BDECFA9);
		        c = HH(c, d, a, b, words[lcv + 7], s33, 0xF6BB4B60);
		        b = HH(b, c, d, a, words[lcv + 10], s34, 0xBEBFBC70);
		        a = HH(a, b, c, d, words[lcv + 13], s31, 0x289B7EC6);
		        d = HH(d, a, b, c, words[lcv + 0], s32, 0xEAA127FA);
		        c = HH(c, d, a, b, words[lcv + 3], s33, 0xD4EF3085);
		        b = HH(b, c, d, a, words[lcv + 6], s34, 0x4881D05);
		        a = HH(a, b, c, d, words[lcv + 9], s31, 0xD9D4D039);
		        d = HH(d, a, b, c, words[lcv + 12], s32, 0xE6DB99E5);
		        c = HH(c, d, a, b, words[lcv + 15], s33, 0x1FA27CF8);
		        b = HH(b, c, d, a, words[lcv + 2], s34, 0xC4AC5665);
		        a = II(a, b, c, d, words[lcv + 0], s41, 0xF4292244);
		        d = II(d, a, b, c, words[lcv + 7], s42, 0x432AFF97);
		        c = II(c, d, a, b, words[lcv + 14], s43, 0xAB9423A7);
		        b = II(b, c, d, a, words[lcv + 5], s44, 0xFC93A039);
		        a = II(a, b, c, d, words[lcv + 12], s41, 0x655B59C3);
		        d = II(d, a, b, c, words[lcv + 3], s42, 0x8F0CCC92);
		        c = II(c, d, a, b, words[lcv + 10], s43, 0xFFEFF47D);
		        b = II(b, c, d, a, words[lcv + 1], s44, 0x85845DD1);
		        a = II(a, b, c, d, words[lcv + 8], s41, 0x6FA87E4F);
		        d = II(d, a, b, c, words[lcv + 15], s42, 0xFE2CE6E0);
		        c = II(c, d, a, b, words[lcv + 6], s43, 0xA3014314);
		        b = II(b, c, d, a, words[lcv + 13], s44, 0x4E0811A1);
		        a = II(a, b, c, d, words[lcv + 4], s41, 0xF7537E82);
		        d = II(d, a, b, c, words[lcv + 11], s42, 0xBD3AF235);
		        c = II(c, d, a, b, words[lcv + 2], s43, 0x2AD7D2BB);
		        b = II(b, c, d, a, words[lcv + 9], s44, 0xEB86D391);
		        a = CryptographyManager.SafeAdd(a, lastA);
                b = CryptographyManager.SafeAdd(b, lastB);
                c = CryptographyManager.SafeAdd(c, lastC);
                d = CryptographyManager.SafeAdd(d, lastD);
	        }

            string hash = CryptographyManager.WordToHex(a) + CryptographyManager.WordToHex(b) +
                CryptographyManager.WordToHex(c) + CryptographyManager.WordToHex(d);

            CryptographyManager.HashCache[input] = hash;

            return hash;
        }

        static private uint RotateLeft(uint value, byte bitsToShift)
        {
            return (value << bitsToShift) | (value >> (32 - bitsToShift)); 
        }

        static private uint SafeAdd(uint first, uint second) 
        {
	        uint x4 = first & 0x40000000;
            uint y4 = second & 0x40000000;
            ulong x8 = first & 0x80000000;
            ulong y8 = second & 0x80000000;
            ulong result = (first & 0x3FFFFFFF) + (second & 0x3FFFFFFF);
            if ((x4 & y4) != 0)
            {
                return (uint)(result ^ 0x80000000 ^ x8 ^ y8);
            }
            if ((x4 | y4) != 0)
            {
                if ((result & 0x40000000) != 0)
                {
                    return (uint)(result ^ 0xC0000000 ^ x8 ^ y8);
                }
                else
                {
                    return (uint)(result ^ 0x40000000 ^ x8 ^ y8);
                }
            }
            else
            {
                return (uint)(result ^ x8 ^ y8);
            }
         }

        static private uint F(uint x, uint y, uint z) { return (x & y) | ((~x) & z); }
        static private uint G(uint x, uint y, uint z) { return (x & z) | (y & (~z)); }
        static private uint H(uint x, uint y, uint z) { return (x ^ y ^ z); }
        static private uint I(uint x, uint y, uint z) { return (y ^ (x | (~z))); }
        static private uint FF(uint a, uint b, uint c, uint d, UInt32 x, byte s, uint ac)
        {
            a = CryptographyManager.SafeAdd(a, CryptographyManager.SafeAdd(CryptographyManager.SafeAdd(F(b, c, d), x), ac));
            return CryptographyManager.SafeAdd(CryptographyManager.RotateLeft(a, s), b);
        }

        static private uint GG(uint a, uint b, uint c, uint d, UInt32 x, byte s, uint ac)
        {
            a = CryptographyManager.SafeAdd(a, CryptographyManager.SafeAdd(CryptographyManager.SafeAdd(G(b, c, d), x), ac));
            return CryptographyManager.SafeAdd(CryptographyManager.RotateLeft(a, s), b);
        }

        static private uint HH(uint a, uint b, uint c, uint d, UInt32 x, byte s, uint ac)
        {
            a = CryptographyManager.SafeAdd(a, CryptographyManager.SafeAdd(CryptographyManager.SafeAdd(H(b, c, d), x), ac));
            return CryptographyManager.SafeAdd(CryptographyManager.RotateLeft(a, s), b);
         }

        static private uint II(uint a, uint b, uint c, uint d, UInt32 x, byte s, uint ac)
        {
            a = CryptographyManager.SafeAdd(a, CryptographyManager.SafeAdd(CryptographyManager.SafeAdd(I(b, c, d), x), ac));
	        return CryptographyManager.SafeAdd(RotateLeft(a, s), b);
        }

        static private UInt32[] ConvertToWordArray(string text) 
        {
	        int wordCount;
	        int messageLength = text.Length;
	        int numberOfWords_temp1 = messageLength + 8;
	        int numberOfWords_temp2 = (numberOfWords_temp1 - (numberOfWords_temp1 % 64))/64;
	        int numberOfWords = (numberOfWords_temp2+1)*16;
            UInt32[] wordArray = new UInt32[numberOfWords - 1];
	        int bytePosition = 0;
            int byteCount = 0;
	        while (byteCount < messageLength)
            {
		        wordCount = (byteCount - (byteCount % 4)) / 4;
		        bytePosition = (byteCount % 4) * 8;
		        wordArray[wordCount] = (UInt32)(wordArray[wordCount] | (text.CharCodeAt(byteCount) << bytePosition));
		        byteCount++;
	        }
	        wordCount = (byteCount - (byteCount % 4)) / 4;
	        bytePosition = (byteCount % 4) * 8;
            wordArray[wordCount] = (UInt32)(wordArray[wordCount] | (0x80 << bytePosition));
            wordArray[numberOfWords - 2] = (UInt32)(messageLength << 3);
            wordArray[numberOfWords - 1] = (UInt32)(messageLength >> 29);
	        return wordArray;
         }

        static private string WordToHex(uint word) 
        {
            StringBuilder stringBuilder = new StringBuilder();
            byte byteValue;
	        for (int lcv = 0; lcv <= 3; lcv++) 
            {
		        byteValue = (byte) ((word >> (lcv * 8)) & 255);
                string temp = "0" + byteValue.ToString(16);
                stringBuilder.Append(temp.Substr(temp.Length - 2, 2));
	        }
            return stringBuilder.ToLocaleString();
         }


    }
}
