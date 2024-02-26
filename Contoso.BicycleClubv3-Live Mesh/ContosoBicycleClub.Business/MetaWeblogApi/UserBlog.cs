using System;
using CookComputing.XmlRpc;
using System.Net;

namespace WLQuickApps.ContosoBicycleClub.Business.MetaWeblogApi
{
    /// <summary> 
    /// This struct represents information about a user's blog. 
    /// </summary> 
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct UserBlog
    {
        public string url;
        public string blogid;
        public string blogName;
    }

}
