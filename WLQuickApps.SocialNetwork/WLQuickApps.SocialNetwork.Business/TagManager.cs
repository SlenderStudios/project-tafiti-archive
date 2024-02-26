using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.TagDataSetTableAdapters;
using WLQuickApps.SocialNetwork.Data.BaseItemDataSetTableAdapters;
using System.Transactions;
using System.Text.RegularExpressions;
using System.Security;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class TagManager
    {
        static private List<Tag> GetTagsFromTable(TagDataSet.TagDataTable tagDataTable)
        {
            List<Tag> list = new List<Tag>();
            foreach (TagDataSet.TagRow row in tagDataTable)
            {
                list.Add(new Tag(row));
            }
            return list;
        }

        static public List<Tag> GetTags(int baseItemID)
        {
            return TagManager.GetTags(BaseItemManager.GetBaseItem(baseItemID));
        }

        static public List<Tag> GetTags(BaseItem baseItem)
        {
            using (TagTableAdapter tagTableAdapter = new TagTableAdapter())
            {
                return TagManager.GetTagsFromTable(tagTableAdapter.GetTagsByBaseItemID(baseItem.BaseItemID));
            }
        }
        
        static public Tag GetTagByTagName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("Name cannot be null or empty"); }

            using (TagTableAdapter tagTableAdapter = new TagTableAdapter())
            {
                TagDataSet.TagDataTable tagDataTable = tagTableAdapter.GetTagByName(name);

                if (tagDataTable.Rows.Count == 0) { throw new ArgumentException("Tag name does not exist"); }

                return new Tag(tagDataTable[0]);
            }
        }

        static public Tag GetTagByTagID(int tagID)
        {
            using (TagTableAdapter tagTableAdapter = new TagTableAdapter())
            {
                TagDataSet.TagDataTable tagDataTable = tagTableAdapter.GetTagByTagID(tagID);

                if (tagDataTable.Rows.Count == 0) { throw new ArgumentException("Tag ID does not exist"); }

                return new Tag(tagDataTable[0]);
            }
        }

        static public bool TagExists(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("Name cannot be null or empty"); }

            using (TagTableAdapter tagTableAdapter = new TagTableAdapter())
            {
                return (tagTableAdapter.GetTagByName(name).Rows.Count != 0);
            }
        }

        static public Tag CreateTag(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("Name cannot be null or empty"); }

            if (TagManager.TagExists(name))
            {
                return TagManager.GetTagByTagName(name);
            }

            using (TagTableAdapter tagTableAdapter = new TagTableAdapter())
            {
                int tagID = Convert.ToInt32(tagTableAdapter.CreateTag(name));
                return TagManager.GetTagByTagID(tagID);
            }
        }

        static public void AddTagToBaseItem(string tag, BaseItem baseItem)
        {
            if (string.IsNullOrEmpty(tag)) { throw new ArgumentException("Tag cannot be null or empty"); }

            if (!BaseItemManager.CanModifyBaseItem(baseItem))
            {
                throw new SecurityException("Currently logged-in user cannot modify the tags for this item.");
            }

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (TagTableAdapter tagTableAdapter = new TagTableAdapter())
            {
                tagTableAdapter.AddTagToBaseItemID(TagManager.CreateTag(tag).TagID, baseItem.BaseItemID);
                baseItem.Update();

                transaction.Complete();
            }
        }

        static public void RemoveTagFromBaseItem(string tag, BaseItem baseItem)
        {
            if (string.IsNullOrEmpty(tag)) { throw new ArgumentException("Tag cannot be null or empty"); }

            if (!BaseItemManager.CanModifyBaseItem(baseItem))
            {
                throw new SecurityException("Currently logged-in user cannot modify the tags for this item.");
            }

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (TagTableAdapter tagTableAdapter = new TagTableAdapter())
            {
                tagTableAdapter.RemoveTagFromBaseItemID(TagManager.CreateTag(tag).TagID, baseItem.BaseItemID);
                baseItem.Update();

                transaction.Complete();
            }
        }

        static internal string GetTagList(List<Tag> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException("tags");
            }
            
            List<string> tagList = tags.ConvertAll<string>(new Converter<Tag, string>(TagManager.GetTagName));
            tagList.Sort();

            return String.Format(",{0},", String.Join(",,", tagList.ToArray()));
        }

        static internal string GetTagSearchList(string tags)
        {
            if (tags == null) { throw new ArgumentNullException("tags"); }

            tags = Regex.Replace(tags, "[^a-z0-9]", " ", RegexOptions.IgnoreCase);

            if (tags.Trim().Length == 0) { return "%"; }

            List<string> tagList = new List<string>(tags.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            tagList.Sort();

            return String.Format("%,{0},%", String.Join(",%,", tagList.ToArray()));
        }

        static private string GetTagName(Tag tag)
        {
            return tag.Name;
        }
    }
}
