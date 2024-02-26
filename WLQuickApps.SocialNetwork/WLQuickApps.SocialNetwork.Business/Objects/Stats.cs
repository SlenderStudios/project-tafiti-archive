using System;
using System.Collections.Generic;
using System.Text;
using WLQuickApps.SocialNetwork.Data;

namespace WLQuickApps.SocialNetwork.Business
{
    public struct Stats
    {
        public int AlbumCount
        {
            get { return this._albumCount; }
        }
        private int _albumCount;

        public int ActiveUserCount
        {
            get { return this._activeUserCount; }
        }
        private int _activeUserCount;

        public int AudioCount
        {
            get { return this._audioCount; }
        }
        private int _audioCount;

        public int CollectionCount
        {
            get { return this._collectionCount; }
        }
        private int _collectionCount;

        public int CommentCount
        {
            get { return this._commentCount; }
        }
        private int _commentCount;

        public int EventCount
        {
            get { return this._eventCount; }
        }
        private int _eventCount;

        public int FileCount
        {
            get { return this._fileCount; }
        }
        private int _fileCount;

        public int FriendConfirmedCount
        {
            get { return this._friendConfirmedCount; }
        }
        private int _friendConfirmedCount;

        public int FriendRequestedCount
        {
            get { return this._friendRequestedCount; }
        }
        private int _friendRequestedCount;

        public int GroupCount
        {
            get { return this._groupCount; }
        }
        private int _groupCount;

        public int PictureCount
        {
            get { return this._pictureCount; }
        }
        private int _pictureCount;

        public int RatingCount
        {
            get { return this._ratingCount; }
        }
        private int _ratingCount;

        public int TagCount
        {
            get { return this._tagCount; }
        }
        private int _tagCount;

        public int UserCount
        {
            get { return this._userCount; }
        }
        private int _userCount;

        public int VideoCount
        {
            get { return this._videoCount; }
        }
        private int _videoCount;

        internal Stats(StatisticsDataSet.StatisticsRow row)
        {
            this._activeUserCount = row.ActiveUserCount;
            this._albumCount = row.AlbumCount;
            this._audioCount = row.AudioCount;
            this._collectionCount = row.CollectionCount;
            this._commentCount = row.CommentCount;
            this._eventCount = row.EventCount;
            this._fileCount = row.FileCount;
            this._friendConfirmedCount = row.FriendConfirmedCount;
            this._friendRequestedCount = row.FriendRequestCount;
            this._groupCount = row.GroupCount;
            this._pictureCount = row.PictureCount;
            this._ratingCount = row.RatingCount;
            this._tagCount = row.TagCount;
            this._userCount = row.UserCount;
            this._videoCount = row.VideoCount;
        }
    }
}
