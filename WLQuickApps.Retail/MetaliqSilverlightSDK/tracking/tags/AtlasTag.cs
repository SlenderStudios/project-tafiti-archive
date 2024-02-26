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
    public class AtlasTag : BaseTrackingTag
    {
        new public readonly int Id;
        new public readonly string Description;
        public readonly string Action;  	
        public readonly string ActionTag;  	
        public readonly string ImageCode;

        public AtlasTag(int _Id, string _Description, string _Action, string _ActionTag, string _ImageCode)
        {
            Id = _Id;
            Description = _Description;
            Action = _Action;
            ActionTag = _ActionTag;
            ImageCode = _ImageCode;
        }
    }
}
