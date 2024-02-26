/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
namespace Contoso.Common.Entity
{
    public class NewsItem
    {
        private string _title;
        private string _description;
        private string _imageUrl;
        private string _author;
        private string _authorPresenceID;

        public NewsItem(string title, string description, string imageURL, string author, string authorPresenceID)
        {
            _title = title;
            _description = description;
            _author = author;
            _authorPresenceID = authorPresenceID;
            _imageUrl = imageURL;
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string ImageURL
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string AuthorPresenceID
        {
            get { return _authorPresenceID; }
            set { _authorPresenceID = value; }
        }
    }
}