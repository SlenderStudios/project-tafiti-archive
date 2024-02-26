using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class Backgrounds
    {
        public List<Background> BackgroundList { get; set; }
    }

    public class Background
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsDefault { get; set; }
    }
}