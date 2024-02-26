using System;
using System.Collections.Generic;
using System.Text;

namespace WLQuickApps.Tafiti.WebSite
{
    public static class Json
    {
        public static string JScriptEscape(string s)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                string sub;
                if (_escape.TryGetValue(s[i], out sub))
                    sb.Append(sub);
                else
                    sb.Append(s[i]);
            }
            return sb.ToString();
        }

        public static string ToJson(string[] s)
        {
            string[] result = Array.ConvertAll<string, string>(s, new Converter<string, string>(
                delegate(string t)
                {
                    return "\"" + Json.JScriptEscape(t) + "\"";
                }));

            StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[ ");
            sb.Append(string.Join(", ", result));
            sb.Append("] ");
            return sb.ToString();
        }

        private static Dictionary<char, string> _escape = new Dictionary<char, string>();

        static Json()
        {
            _escape['\t'] = @"\t";
            _escape['\n'] = @"\n";
            _escape['\r'] = @"\r";
            _escape['\f'] = @"\f";
            _escape['\b'] = @"\b";
            _escape['"'] = "\\\"";
            _escape['\\'] = @"\\";
        }
    }
}