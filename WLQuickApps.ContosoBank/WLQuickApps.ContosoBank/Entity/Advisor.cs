using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class Advisors
    {
        public List<Advisor> AdvisorList { get; set; }
    }

    public class Advisor
    {
        public int ID { get; set; }
        public string AdvisorName { get; set; }
        public string CID { get; set; }
        public bool AvailableAppointment { get; set; }
        public bool IsActive { get; set; }
    }
}