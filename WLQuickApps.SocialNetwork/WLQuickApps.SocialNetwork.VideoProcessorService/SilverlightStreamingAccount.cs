using System;
using System.Collections.Generic;
using System.Text;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    public class SilverlightStreamingAccount
    {
        string _id;
        string _key;

        public SilverlightStreamingAccount(string id, string key)
        {
            if (id == null || key == null)
            { throw new ArgumentNullException((id == null) ? "id" : "key"); }

            this._id = id;
            this._key = key;
        }

        private bool IsKeyValid()
        {
            // TODO: implement?
            return true;
        }

        private bool IsIdValid()
        {
            // TODO: implement?
            return true;
        }

        public string Id
        {
            get
            {
                return this._id;
            }
        }

        public string Key
        {
            get
            {
                return this._key;
            }
        }
    }
}
