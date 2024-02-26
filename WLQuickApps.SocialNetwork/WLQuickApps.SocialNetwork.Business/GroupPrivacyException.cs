using System;
using System.Collections.Generic;
using System.Text;
using System.Security;

namespace WLQuickApps.SocialNetwork.Business
{
    public class GroupPrivacyException : System.Security.SecurityException
    {
        public int BaseItemID
        {
            get { return this._BaseItemID;  }
        }
        private int _BaseItemID;

        public string GroupName
        {
            get { return this._groupName; }
        }
        private string _groupName;

        public GroupPrivacyException(int BaseItemID, string groupName)
        {
            this._BaseItemID = BaseItemID;
            this._groupName = groupName;
        }
    }
}
