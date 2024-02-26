using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace WLQuickApps.ContosoBicycleClub.UI
{
	public static class AntiXssHelper
	{
		//These are all the allowed tags in the description field.
		public static string[] AllowedTags = { "u", "b", "p", "li", "ol", "ul", "em", "strong", "h1", "h2", "h3" };

		/// <summary>
		/// Encode Html with exception tags given.
		/// Basically this will assume everything is evil by encoding everything
		/// and then undo encoding for the given tags.
		/// 
		/// This technique is recommended here http://msdn2.microsoft.com/en-us/library/ms998274.aspx
		/// 
		/// I can't find this functionality in AntiXssLibrary
		/// </summary>
		/// <param name="strIn"></param>
		/// <param name="tags"></param>
		/// <returns></returns>
		public static string HtmlEncode(string strIn, params string[] exceptionTags)
		{ 
			// Encode the string input
			StringBuilder sb = new StringBuilder(HttpUtility.HtmlEncode(strIn));

			// Selectively allow  <b> and <i>
			foreach (string tag in exceptionTags)
			{
				string openTag = "<" + tag + ">";
				string closeTag = "</" + tag + ">";
				string openTagEncoded = HttpUtility.HtmlEncode(openTag);
				string closeTagEncoded = HttpUtility.HtmlEncode(closeTag);

				sb.Replace(openTagEncoded, openTag);
				sb.Replace(closeTagEncoded, closeTag);			
			}	
			return sb.ToString();
		}
	}
}
