using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class Avatars
    {
        public List<Avatar> AvatarList { get; set; }
    }

    public class Avatar
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsDefault { get; set; }
    }
}