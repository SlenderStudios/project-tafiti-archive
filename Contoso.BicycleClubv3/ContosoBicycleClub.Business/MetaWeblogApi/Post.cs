using System;
using CookComputing.XmlRpc;
using System.Net;

namespace WLQuickApps.ContosoBicycleClub.Business.MetaWeblogApi
{
    /// <summary> 
    /// This struct represents the information about a post that could be returned by the 
    /// editPost(), getRecentPosts() and getPost() methods. 
    /// </summary> 
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        public DateTime dateCreated;
        public string description;
        public string title;
        public string postid;
        public string[] categories;
    }
}
