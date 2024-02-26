using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MetaliqSilverlightSDK
{
    public class Timer
    {
        protected DispatcherTimer _Timer;
        public EventHandler Tick;
        protected int _Interval;

        public Timer(int Milliseconds)
        {
            Init(Milliseconds);
        }
        public Timer()
        {
            Init(100);
        }
        protected void Init(int Milliseconds)
        {
            _Interval = Milliseconds;
            _Timer = new System.Windows.Threading.DispatcherTimer();
            _Timer.Interval = new TimeSpan(0, 0, 0, 0, _Interval); // 100 Milliseconds
            _Timer.Tick += new EventHandler(_Tick);
            Start();
        }

        public void Start()
        {
            _Timer.Start();
        }
        public void Stop()
        {
            _Timer.Stop();
        }
        public int Interval
        {
            get
            {
                return _Interval;
            }
        }
        // Fires every n milliseconds while the Timer is active.
        protected void _Tick(object o, EventArgs sender)
        {
            if (Tick != null)
            {
                Tick(sender, null);
            }
        }
    }
}
