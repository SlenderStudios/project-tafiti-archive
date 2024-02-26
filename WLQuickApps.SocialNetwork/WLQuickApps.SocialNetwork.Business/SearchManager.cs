using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.BaseItemDataSetTableAdapters;

namespace WLQuickApps.SocialNetwork.Business
{
    public class SearchManager
    {
        static private string _emptyTagSearchString;
        static private string _emptyLocationSearchString;

        static SearchManager()
        {
            SearchManager._emptyTagSearchString = TagManager.GetTagSearchList(string.Empty);
            SearchManager._emptyLocationSearchString = Location.BuildSearchString(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public SearchManager() { }

        static public SearchResults SearchNetwork(string name, string address1, string address2, string city, string region, string country, string postalCode, string tagSearchString)
        {
            return SearchManager.SearchNetwork(string.Empty, name, address1, address2, city, region, country, postalCode, tagSearchString);
        }

        static public SearchResults SearchNetwork(string baseItemType, string name, string address1, string address2, string city, string region, string country, string postalCode, string tagSearchString)
        {
            SearchResults searchResults = new SearchResults();

            if (name == null) { name = string.Empty; }
            if (address1 == null) { address1 = string.Empty; }
            if (address2 == null) { address2 = string.Empty; }
            if (city == null) { city = string.Empty; }
            if (region == null) { region = string.Empty; }
            if (country == null) { country = string.Empty; }
            if (postalCode == null) { postalCode = string.Empty; }
            if (tagSearchString == null) { tagSearchString = string.Empty; }

            tagSearchString = TagManager.GetTagSearchList(tagSearchString);
            string locationSearchString = Location.BuildSearchString(name, address1, address2, city, region, country, postalCode);

            if ((tagSearchString == SearchManager._emptyTagSearchString) && (locationSearchString == SearchManager._emptyLocationSearchString))
            {
                return searchResults;
            }

            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                foreach (BaseItemDataSet.BaseItemRow row in baseItemTableAdapter.SearchBySubItemType(Constants.BaseItemTypes.Media, Constants.MediaTypes.Picture, 
                    tagSearchString, locationSearchString, 0, 50))
                {
                    searchResults.AddMedia(row.BaseItemID);
                }

                foreach (BaseItemDataSet.BaseItemRow row in baseItemTableAdapter.SearchBySubItemType(Constants.BaseItemTypes.Media, Constants.MediaTypes.Audio,
                    tagSearchString, locationSearchString, 0, 50))
                {
                    searchResults.AddMedia(row.BaseItemID);
                }

                foreach (BaseItemDataSet.BaseItemRow row in baseItemTableAdapter.SearchBySubItemType(Constants.BaseItemTypes.Media, Constants.MediaTypes.Video,
                    tagSearchString, locationSearchString, 0, 50))
                {
                    searchResults.AddMedia(row.BaseItemID);
                }

                foreach (BaseItemDataSet.BaseItemRow row in baseItemTableAdapter.SearchByItemType(Constants.BaseItemTypes.Group,
                    tagSearchString, locationSearchString, 0, 50))
                {
                    searchResults.AddGroup(row.BaseItemID);
                }

                foreach (BaseItemDataSet.BaseItemRow row in baseItemTableAdapter.SearchByItemType(Constants.BaseItemTypes.User,
                    tagSearchString, locationSearchString, 0, 50))
                {
                    searchResults.AddUser(row.BaseItemID);
                }

                foreach (BaseItemDataSet.BaseItemRow row in baseItemTableAdapter.SearchByItemType(Constants.BaseItemTypes.Collection,
                    tagSearchString, locationSearchString, 0, 50))
                {
                    searchResults.AddCollection(row.BaseItemID);
                }

                foreach (BaseItemDataSet.BaseItemRow row in baseItemTableAdapter.SearchByItemType(Constants.BaseItemTypes.Forum,
                    tagSearchString, locationSearchString, 0, 50))
                {
                    searchResults.AddForum(row.BaseItemID);
                }

            }

            return searchResults;
        }

    }
}
