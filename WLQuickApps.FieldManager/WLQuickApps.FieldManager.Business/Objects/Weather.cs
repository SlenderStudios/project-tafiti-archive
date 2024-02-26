using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using WLQuickApps.FieldManager.Business.WeatherService;

namespace WLQuickApps.FieldManager.Business
{
    [DataContract]
    public class Weather
    {
        [DataMember] public string Day = string.Empty;
        [DataMember] public int MaxTemperature;
        [DataMember] public int MinTemperature;
        [DataMember] public string WeatherSummary = string.Empty;
    }
}
