using System.Collections.Generic;
using Contoso.Common.Entity; 
namespace Contoso.Common.Logic
{
    public static class NewsBL
    {

        public static List<NewsItem> getNewsItems()
        {
            return getNewsItems(int.MaxValue);
        }

        public static List<NewsItem> getNewsItems(int maxrecords)
        {
            List<NewsItem> Items = new List<NewsItem>();
            Items.Add(
                new NewsItem(Resource.NewsItem1_Title,
                             Resource.NewsItem1_Description,
                             Resource.NewsItem1_Image,
                             Resource.NewsItem1_Author,
                             Resource.NewsItem1_AuthorPrecenseID
                             ));
            Items.Add(
                new NewsItem(Resource.NewsItem2_Title,
                             Resource.NewsItem2_Description,
                             Resource.NewsItem2_Image,
                             Resource.NewsItem2_Author,
                             Resource.NewsItem2_AuthorPrecenseID
                             ));
            Items.Add(
                new NewsItem(Resource.NewsItem3_Title,
                             Resource.NewsItem3_Description,
                             Resource.NewsItem3_Image,
                             Resource.NewsItem3_Author,
                             Resource.NewsItem3_AuthorPrecenseID
                             ));
            
            //only return the required number of records
            if (Items.Count > maxrecords)
            {
                Items.RemoveRange(0, Items.Count - maxrecords);
            }
            return Items;
        }
    }
}