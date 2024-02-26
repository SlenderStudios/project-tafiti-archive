using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Data;

namespace WLQuickApps.SocialNetwork.Business
{
    public class Tag
    {
        public string Name
        {
            get { return this._name; }
        }
        private string _name;

        public int TagID
        {
            get { return this._tagID; }
        }
        private int _tagID;

        public Tag(int tagID, string name)
        {
            this._tagID = tagID;
            this._name = name;
        }

        public Tag(TagDataSet.TagRow row)
            : this(row.TagID, row.Name)
        {
        }

        public override bool Equals(object obj)
        {
            Tag otherTag = obj as Tag;
            if (((object)otherTag) == null) { return false; }

            return (this.TagID == otherTag.TagID);
        }

        public override int GetHashCode()
        {
            return this.TagID.GetHashCode();
        }

        public static bool operator ==(Tag first, Tag second)
        {
            if (((object)first) == null)
            {
                return (((object)second) == null);
            }

            return first.Equals(second);
        }

        public static bool operator !=(Tag first, Tag second)
        {
            return !(first == second);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
