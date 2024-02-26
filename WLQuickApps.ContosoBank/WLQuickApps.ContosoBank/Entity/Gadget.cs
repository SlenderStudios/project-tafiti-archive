using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class Gadgets
    {
        public List<Gadget> GadgetList { get; set; }
    }

    public class Gadget
    {
        public int ID { get; set; }
        public string GadgetName { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Thumbnail { get; set; }
    }
}