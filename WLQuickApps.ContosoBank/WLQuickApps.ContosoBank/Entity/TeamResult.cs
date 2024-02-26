using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class TeamResults
    {
        public List<TeamResult> TeamResultList { get; set; }
    }

    public class TeamResult
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public int Draw { get; set; }
    }
}