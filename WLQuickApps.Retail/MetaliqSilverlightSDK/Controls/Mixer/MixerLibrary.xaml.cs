using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace MetaliqSilverlightSDK.Controls.Mixer
{
    public partial class MixerLibrary : UserControl
    {
        public MixerLibrary()
        {
            InitializeComponent();
        }
        public void Init()
        {
            DragInfo info;
            foreach (MixerAsset asset in Items)
            {
                info = DragUtil.MakeDraggable(asset, Mixer.Instance.LayoutRoot);
                info.Targets.AddUnique(LibraryBackground);
                info.Targets.AddUnique(Mixer.Instance.VideoTrack);
                asset.Drag = info;
            }
        }
        public List<MixerAsset> Items
        {
            get
            {
                List<MixerAsset> items = new List<MixerAsset>();
                foreach (FrameworkElement element in Content.Children)
                {
                    if (element is MixerAsset)
                    {
                        MixerAsset child = element as MixerAsset;
                        items.Add(child);
                    }
                }
                return items;
            }
        }
    }
}
