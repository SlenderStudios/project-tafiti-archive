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
using MetaliqSilverlightSDK.Controls.Mixer.Model;
using MetaliqSilverlightSDK.Controls.Mixer.Model.Media;

namespace MetaliqSilverlightSDK.Controls.Mixer
{
    public partial class MixerTimeline : UserControl
    {
        protected MediaTrack _Data = new MediaTrack();

        public MixerTimeline()
        {
            InitializeComponent();
        }
        public MediaTrackKeyframe this[int index]
        {
            get
            {
                return _Data[index];
            }
            set
            {
                _Data[index] = value;
            }
        }
        public int Count
        {
            get 
            { 
                return _Data.Count;
            }
        }
    }
}
