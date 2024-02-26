using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class ContosoCupDraw
    {
        public List<Round> Rounds { get; set; }
    }

    public class Round
    {
        public int RoundNumber { get; set; }
        public string PlayerOfWeek { get; set; }
    }
}