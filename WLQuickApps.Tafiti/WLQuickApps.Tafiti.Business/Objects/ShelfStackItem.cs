using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.Tafiti.Data;

namespace WLQuickApps.Tafiti.Business
{
    public class ShelfStackItem
    {
        public int ShelfStackItemID { get { return this._shelfStackItemID; } }
        private int _shelfStackItemID;

        public string Title { get { return this._title; } }
        private string _title;

        public string Description { get { return this._description; } }
        private string _description;

        public string Url { get { return this._url; } }
        private string _url;

        public string ImageUrl { get { return this._imageUrl; } }
        private string _imageUrl;

        public string Source { get { return this._source; } }
        private string _source;

        public string Domain { get { return this._domain; } }
        private string _domain;

        public int Height { get { return this._height; } }
        private int _height;

        public int Width { get { return this._width; } }
        private int _width;

        public User Owner 
        {
            get
            {
                if (this._owner == null)
                {
                    this._owner = UserManager.GetUserByID(this._userID);
                }
                return this._owner;
            }
        }
        private User _owner;
        private string _userID;

        public ShelfStack ParentShelf
        {
            get
            {
                if (this._parentShelf == null)
                {
                    this._parentShelf = ShelfStackManager.GetShelfStackByID(this._parentShelfID);
                }
                return this._parentShelf;
            }
        }
        private ShelfStack _parentShelf;
        private Guid _parentShelfID;

        public DateTime Timestamp { get { return this._timestamp; } }
        private DateTime _timestamp;

        public ShelfStackItem(ShelfStackItemDataSet.ShelfStackItemRow row)
        {
            this._description = row.Description;
            this._domain = row.Domain;
            this._height = row.Height;
            this._imageUrl = row.ImageUrl;
            this._parentShelfID = row.ShelfStackID;
            this._shelfStackItemID = row.ShelfStackItemID;
            this._source = row.Source;
            this._timestamp = row.AddedTimestamp;
            this._title = row.Title;
            this._url = row.Url;
            this._userID = row.AddedBy;
            this._width = row.Width;
        }

    }
}
