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

namespace MetaliqSilverlightSDK.Controls.Mixer
{
    public partial class Mixer : UserControl
    {
        public static Mixer Instance;
        public Mixer()
        {
            InitializeComponent();
            Instance = this;
            Init();
        }
        protected void Init()
        {
            Library.Init();
            SetupDebug();
        }
        private void SetupDebug()
        {
            foreach (MixerAsset asset in Library.Items)
            {
                asset.Drag.DragStart += new EventHandler(Drag_DragStart);
                asset.Drag.DragMove += new EventHandler(Drag_DragMove);
                asset.Drag.DragDrop += new EventHandler<DragInfoEvent>(Drag_DragDrop);
                asset.Drag.DragComplete += new EventHandler(Drag_DragComplete);
                asset.Drag.DragOver += new EventHandler<DragInfoEvent>(Drag_DragOver);
                asset.Drag.DragOut += new EventHandler<DragInfoEvent>(Drag_DragOut);
            }
        }

        void Drag_DragOut(object sender, DragInfoEvent e)
        {
            string str = e.Info.Element.Name + " was dragged off: " + e.AffectedTarget.Name;
            Debug(str);
        }

        void Drag_DragOver(object sender, DragInfoEvent e)
        {
            string str = e.Info.Element.Name + " was dragged over: " + e.AffectedTarget.Name;
            Debug(str);
        }
        void DebugTargets(DragInfoEvent e)
        {
            foreach (FrameworkElement target in e.AffectedTargets)
            {
                Debug(target.Name);
            }
        }
        void Drag_DragComplete(object sender, EventArgs e)
        {
            Debug("DragComplete");
        }

        void Drag_DragDrop(object sender, DragInfoEvent e)
        {
            string str = e.Info.Element.Name + " was dropped on " + e.AffectedTargets.Count + " target";
            if (e.AffectedTargets.Count > 1)
            {
                str += "s";
            }

            if (e.AffectedTarget is MixerTimeline)
            {
                FrameworkElement clone = e.Info.Element.Clone();
                DragInfo info = DragUtil.MakeDraggable(clone, LayoutRoot);
                info.Targets.Add(VideoTrack);
                info.Targets.Add(AudioTrack);
                (e.AffectedTarget as MixerTimeline).Content.Children.Add(clone);
            }

            Debug(str);
            DebugTargets(e);
        }

        void Drag_DragMove(object sender, EventArgs e)
        {
            //Debug("DragMove");
        }

        void Drag_DragStart(object sender, EventArgs e)
        {
            Debug("DragStart");
        }
        public void Debug(string txt)
        {
            DebugTextBox.Text += txt + "\n";
        }
    }
}