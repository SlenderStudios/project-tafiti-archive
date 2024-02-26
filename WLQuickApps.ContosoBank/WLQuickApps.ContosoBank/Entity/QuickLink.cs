using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class QuickLinks
    {
        public List<Advisor> QuickLinkList { get; set; }
    }

    public class QuickLink
    {
        public string Title { get; set; }
        public string Link { get; set; }
    }
}