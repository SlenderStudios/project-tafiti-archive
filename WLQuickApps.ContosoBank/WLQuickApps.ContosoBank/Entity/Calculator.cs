using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class Calculators
    {
        public List<Calculator> CalculatorList { get; set; }
    }

    public class Calculator
    {
        public int ID { get; set; }
        public string CalculatorName { get; set; }
        public string Description { get; set; }
        public string DownloadLink { get; set; }
    }
}