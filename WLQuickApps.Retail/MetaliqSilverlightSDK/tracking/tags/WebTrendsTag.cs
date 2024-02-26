using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetaliqSilverlightSDK.tracking.tags
{
    public class WebTrendsTag : BaseTrackingTag
    {
        public readonly string dcsuri; 	
        public readonly string ti; 	
        public readonly string cg_n;
        public readonly string dac;
        public readonly string customTag;
        public readonly string customTagValue;
        new public readonly int Id;
        new public readonly string Description;

        public WebTrendsTag(int _Id, string _Description, string _dcsuri, string _ti, string _cg_n, string _dac, string _customTag, string _customTagValue)
        {
            Id = _Id;
            Description = _Description;
            dcsuri = _dcsuri;
            ti = _ti;
            cg_n = _cg_n;
            dac = _dac;
            customTag = (_customTag == null) ? "" : _customTag;
            customTagValue = (_customTagValue == null) ? "" : _customTagValue;
        }
    }
}
