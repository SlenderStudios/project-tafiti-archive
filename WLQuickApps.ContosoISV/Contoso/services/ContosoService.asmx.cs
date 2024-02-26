/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Contoso.Common.Entity;
using Contoso.Common.Logic;

namespace Contoso.services
{
    /// <summary>
    /// Summary description for ContosoService
    /// </summary>
    [WebService(Namespace = "http://www.contoso.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ContosoService : WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public string SaveFavorite(string type, string sID)
        {
            Dictionary<string, string> favorites = (Dictionary<string, string>)HttpContext.Current.Session["Favs"];
            if (favorites == null)
            {
                favorites = new Dictionary<string, string>();
            }

               
            //Add to Session
            if (!favorites.ContainsKey(sID))
            {
                favorites.Add(sID, sID);
            }
            HttpContext.Current.Session.Add("Favs", favorites);

            StringBuilder str = new StringBuilder();
            foreach (string title in favorites.Values)
            {
                str.Append("<div class=\"FavoriteItemContent\">");
                str.Append(title);
                str.Append("</div><div class=\"FavoriteItemMore\"></div><hr />");
            }

            return str.ToString();
        }

        private static List<NewsItem> getFavItems(Dictionary<int, int> favorites)
        {
            List<NewsItem> items = new List<NewsItem>();
            List<NewsItem> newsItems = NewsBL.getNewsItems();
            foreach (int item in favorites.Keys)
            {
                items.Add(newsItems[item]);
            }
            return items;
        }
    }
}