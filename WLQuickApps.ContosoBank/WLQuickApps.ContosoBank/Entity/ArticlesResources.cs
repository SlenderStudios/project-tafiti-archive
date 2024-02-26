using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class ArticlesResources
    {
        public List<Resource> ArticlesResourcesList { get; set; }
    }

    public class Resource
    {
        public string Title { get; set; }
        public string ResourceType { get; set; }
        public string Area { get; set; }
        public string Link { get; set; }
    }
}