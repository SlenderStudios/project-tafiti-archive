using System;
using CookComputing.XmlRpc;
using System.Net;
namespace WLQuickApps.ContosoBicycleClub.Business.MetaWeblogApi
{
    /// <summary> 
    /// This struct represents the information about a category that could be returned by the 
    /// getCategories() method. 
    /// </summary> 
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Category
    {
        public string description;
        public string title;
    }

}
