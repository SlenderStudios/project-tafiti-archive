using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace MetaliqSilverlightSDK
{
    public class EnterFrameEmulator
    {
        private Canvas _canvas;
        private Storyboard _storyBoard;
        private DateTime _lastUpdate;
        private TimeSpan _elapsed;

        public delegate void UpdateDelegate(TimeSpan ElapsedTime);
        public event UpdateDelegate Update;

        public EnterFrameEmulator(Canvas canvas)
        {
            _canvas = canvas;
            _storyBoard = new Storyboard();
            _storyBoard.SetValue(Storyboard.TargetNameProperty, "storyBoard");
            _canvas.Resources.Add("Key", _storyBoard);
            _storyBoard.Completed += new EventHandler(_storyBoard_Completed);
            _lastUpdate = DateTime.Now;
            _storyBoard.Begin();
        }
        protected void _storyBoard_Completed(object sender, EventArgs e)
        {
            _elapsed = DateTime.Now - _lastUpdate;
            Update(_elapsed);
            _storyBoard.Begin();
            _lastUpdate = DateTime.Now;
        }
    }
}