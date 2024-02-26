//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace VESilverlight
{
    public class TextBox : LayoutControl
    {
        static TextBox()
        {
            InitializeMap();
        }

        public TextBox()
        {
            _text = FindName("text") as TextBlock;
            _selection = FindName("selection") as Rectangle;
            _cursor = FindName("cursor") as Rectangle;
            _border = FindName("border") as Rectangle;
            _cursorBlink = FindName("cursorBlink") as Storyboard;
            this.Cursor = Cursors.IBeam;
            _scratch = new TextBlock();

            _shiftCanvas = FindName("ShiftCanvas") as Canvas;
        }

        public void Initialize(Canvas root){
            _rootCanvas = root;
        }


        public void Resize(double width, double height)
        {
            this.Arrange(new Rect(new Size(width, height)));
            this.ArrangeCore(new Rect(new Size(width, height)));
        }

        public override void OnGotFocus()
        {
            _rootCanvas.KeyDown += OnKeyDown;
            _rootCanvas.KeyUp += OnKeyUp;
            
            _selection.Opacity = .3;
            ShowCursor();
            _hasFocus = true;
            _border.Stroke = GetFocusBrush();
        }

        public override void OnLostFocus()
        {
            _rootCanvas.KeyDown -= OnKeyDown;
            _rootCanvas.KeyUp -= OnKeyUp;
            
            _selection.Opacity = 0;
            HideCursor();
            _hasFocus = false;
            _border.Stroke = GetBorderBrush();
        }

        public override void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == 1)
                Delete(false);
            else if (e.Key == 255 && e.PlatformKeyCode == 46)
                Delete(true);
            else
            {
                char c;
                if (GetChar(e, out c))
                {
                    InsertText(c);
                }
                else
                {
                    _gotControlKey = ProcessControlKey(e);
                }
            }
        }

        public override void OnKeyUp(object sender, KeyboardEventArgs e)
        {
            if (!_gotControlKey)
            {
                ProcessControlKey(e);
            }
            else
                _gotControlKey = false;
        }

        protected override void OnMouseLeftButtonDownCore(object sender, MouseEventArgs e)
        {
            HideCursor();

            Point p = e.GetPosition(sender as UIElement);
            CursorIndex = GetCursorIndex(p.X);

            if (e.Shift && _isSelecting)
            {
                AdjustSelection(true);
            }
            else
            {
                SelectionStart = CursorIndex;
                SelectionLength = 0;
                AdjustSelection(false);
                CaptureMouse();
                _isSelecting = true;
            }
            _mouseDown = true;
        }

        protected override void OnMouseMoveCore(object sender, MouseEventArgs e)
        {
            if (_isSelecting && _mouseDown)
            {
                Point p = e.GetPosition(sender as UIElement);
                CursorIndex = GetCursorIndex(p.X);
                AdjustSelection(true);
                _selStart = Math.Min(_dragEnd, _dragStart);
                _selLength = Math.Abs(_dragEnd - _dragStart);
            }
        }

        protected override void OnMouseLeftButtonUpCore(object sender, MouseEventArgs e)
        {
            ShowCursor();

            if (_isSelecting && !e.Shift)
            {
                ReleaseMouseCapture();
                _isSelecting = false;
            }

            _mouseDown = false;
        }

        protected override void OnMouseHasLeftControlCore(object sender, EventArgs e)
        {
            if (_isSelecting)
            {
                ReleaseMouseCapture();
                _mouseDown = false;
            }
        }

        // The resource name used to initialize the actual object
        protected override string ResourceName
        {
            get { return "TextBox.xaml"; }
        }

        public string Text
        {
            get { return _text.Text; }
            set { _text.Text = value; RecalculateText(); }
        }

        public string FontFamily
        {
            get { return _text.FontFamily; }
            set { _scratch.FontFamily = _text.FontFamily = value; RecalculateText(); InvalidateMeasure(); }
        }

        public double FontSize
        {
            get { return _text.FontSize; }
            set { _scratch.FontSize = _text.FontSize = value; RecalculateText(); InvalidateMeasure(); }
        }

        public Color FontColor
        {
            get { return ((SolidColorBrush)_text.Foreground).Color; }
            set { _scratch.Foreground = new SolidColorBrush(value); _text.Foreground = new SolidColorBrush(value); }
        }

        public int SelectionStart
        {
            get { return _selStart; }
            set { if (_selStart != value) { _selStart = value; SelectionChanged(); } }
        }

        public int SelectionLength
        {
            get { return _selLength; }
            set { if (_selLength != value) { _selLength = value; SelectionChanged(); }  }
        }

        public Brush Background
        {
            get { return this.Background; }
            set { this.Background = value; }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; _border.Stroke = GetBorderBrush(); }
        }

        public Color FocusColor
        {
            get { return _focusColor; }
            set { _focusColor = value; }
        }

        private Brush GetBorderBrush()
        {
            return new SolidColorBrush(_borderColor);
        }

        private Brush GetFocusBrush()
        {
            return new SolidColorBrush(_focusColor);
        }

        public double BorderThickness
        {
            get { return _border.StrokeThickness; }
            set { _border.StrokeThickness = value; InvalidateMeasure(); }
        }

        public bool GetChar(KeyboardEventArgs e, out char c)
        {
            if (e.Ctrl)
            {
                c = '\0';
                return false;
            }

            // Construct hash code
            int key = e.Key * 256 + e.PlatformKeyCode;
            if (!e.Shift)
                key = key * -1;

            return map.TryGetValue(key, out c);
        }       

        private void MoveCursor(int delta, bool isShift)
        {
            int newCursorIndex;
            if (delta > 0)
                newCursorIndex = Math.Min(CursorIndex + delta, Text.Length);
            else
                newCursorIndex = Math.Max(0, CursorIndex + delta);

            if (isShift)
            {
                if (!IsSelecting)
                {
                    BeginSelecting();
                }
            }
            else
            {
                _dragEnd = _dragStart = newCursorIndex;
            }

            CursorIndex = newCursorIndex;
            AdjustSelection(isShift);
        }

        private bool ProcessControlKey(KeyboardEventArgs e)
        {
            if (e.Ctrl && e.Key == 30)
            {
                SelectAll();
                return true;
            }

            switch (e.Key)
            {
                case 3: //enter
                    EnterPressed(this, new EventArgs());
                    ClearText();
                    break;
                case 12: // end
                    MoveCursor(Text.Length - CursorIndex, e.Shift);
                    break;
                case 13: // home
                    MoveCursor(-CursorIndex, e.Shift);
                    break;
                case 14:
                    MoveCursor(-1, e.Shift);
                    break;
                case 16:
                    MoveCursor(1, e.Shift);
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void SelectAll()
        {
            _dragStart = 0;
            _dragEnd = Text.Length;
            CursorIndex = Text.Length;
            AdjustSelection(true);
        }       

        private int GetCursorIndex(double x)
        {
            if (Text.Length == 0)
                return 0;

            int index;
            for (index = 0; index < _cumulativeWidths.Length; ++index)
            {
                if (x < _cumulativeWidths[index] - .5 * _widths[index])
                    break;
            }

            if (index == _cumulativeWidths.Length)
            {
                return Text.Length;
            }

            return index;
        }

        private void InsertText(char c)
        {
            string beforeSelection = Text.Substring(0, SelectionStart);
            string afterSelection = Text.Substring(SelectionStart + SelectionLength);

            Text = beforeSelection + c + afterSelection;
            CursorIndex = beforeSelection.Length + 1;

            AdjustSelection(false);
        }

        private void Delete(bool deleteForward)
        {
            string beforeSelection = Text.Substring(0, SelectionStart);
            string afterSelection = Text.Substring(SelectionStart + SelectionLength);

            if (SelectionLength == 0)
            {
                if (deleteForward)
                {
                    if (afterSelection.Length > 0)
                    {
                        Text = beforeSelection + afterSelection.Substring(1);
                    }
                }
                else
                {
                    if (SelectionStart > 0)
                    {
                        Text = beforeSelection.Substring(0, beforeSelection.Length - 1) + afterSelection;
                        --SelectionStart;
                        --CursorIndex;
                    }
                }
            }
            else
            {
                Text = beforeSelection + afterSelection;
                SelectionLength = 0;
                CursorIndex = beforeSelection.Length;
            }

            AdjustSelection(false);
        }

        #region Cursor control

        private int CursorIndex
        {
            get { return _cursorIndex; }
            set
            {
                _cursorIndex = Math.Min(Text.Length, value);
                _cursorIndex = Math.Max(0, value);
                
                double left = _cursorIndex == 0 ? 0 : _cumulativeWidths[_cursorIndex - 1];

                _cursor.SetValue(Canvas.LeftProperty, left + BorderThickness + TextOffset);

                double visibleStart = -(double)_shiftCanvas.GetValue(Canvas.LeftProperty) + 5;

                double visibleEnd = visibleStart + Width -10 ;

                if (left + BorderThickness + TextOffset < visibleStart)
                {
                    _shiftCanvas.SetValue(Canvas.LeftProperty, -(left + BorderThickness + TextOffset)+5);
                }
                else if (left + BorderThickness + TextOffset > visibleEnd)
                {
                    _shiftCanvas.SetValue(Canvas.LeftProperty, -(left + BorderThickness + TextOffset - Width)-10);
                }
            }
        }

        private void ShowCursor()
        {
            _cursor.Visibility = Visibility.Visible;
            _cursorBlink.Begin();

        }

        private void HideCursor()
        {
            _cursor.Visibility = Visibility.Collapsed;
            _cursorBlink.Stop();
        }

        #endregion

        #region Selection

        private bool IsSelecting
        {
            get { return _isSelecting; }
        }

        private void BeginSelecting()
        {
            AdjustSelection(true);
        }

        private void StopSelecting()
        {
            AdjustSelection(false);
        }

        private void AdjustSelection(bool isSelecting)
        {
            if (!isSelecting)
            {
                _dragStart = CursorIndex;
            }
            _dragEnd = CursorIndex;

            _isSelecting = isSelecting;

            int leftIndex = Math.Min(_dragStart, _dragEnd);
            int rightIndex = Math.Max(_dragStart, _dragEnd);

            double left = leftIndex == 0 ? 0 : _cumulativeWidths[leftIndex - 1];
            double right = rightIndex == 0 ? 0 : _cumulativeWidths[rightIndex - 1];

            _selection.SetValue(Canvas.LeftProperty, left + BorderThickness + TextOffset);
            _selection.Width = right - left;
            _selStart = leftIndex;
            _selLength = rightIndex - leftIndex;
        }

        #endregion

        public void ClearText()
        {
            Text = "";
            CursorIndex = 0;
        }

        private void SelectionChanged()
        {
            //AdjustSelection();
        }

        private void RecalculateText()
        {
            _widths = new double[Text.Length];
            _cumulativeWidths = new double[Text.Length];

            for (int index = 0; index < Text.Length; ++index)
            {
                _scratch.Text = Text.Substring(index, 1);
                _widths[index] = _scratch.ActualWidth;
                _cumulativeWidths[index] = _widths[index];
                if (index > 0)
                {
                    _cumulativeWidths[index] += _cumulativeWidths[index - 1];
                }
            }
        }

        protected Size MeasureCore(Size availableSize)
        {
            _scratch.Text = "Hello, world";
            double textWidth = string.IsNullOrEmpty(_text.Text) ? _scratch.ActualWidth : _text.ActualWidth;
            double textHeight = string.IsNullOrEmpty(_text.Text) ? _scratch.ActualHeight : _text.ActualHeight;

            return IncreaseForMargin(new Size(
               (LayoutStorage.WidthWasSpecified ? LayoutStorage.OriginallySpecifiedSize.Width : textWidth),
               textHeight + 2 * BorderThickness), _Margin);
        }

        protected void ArrangeCore(Rect finalRect)
        {
            finalRect = DecreaseForMargin(finalRect, _Margin);
            base.ArrangeCore(finalRect);
            SetClipRect(this, new Rect(0, 0, finalRect.Width, finalRect.Height));

            _border.Height = finalRect.Height;
            _border.Width = finalRect.Width;

            _text.SetValue(Canvas.LeftProperty, BorderThickness + TextOffset);
            _text.SetValue(Canvas.TopProperty, BorderThickness);
            _cursor.SetValue(Canvas.LeftProperty, BorderThickness + TextOffset);
            
            _selection.SetValue(Canvas.TopProperty, BorderThickness + 1);
            _cursor.SetValue(Canvas.TopProperty, BorderThickness + 1);

            this.Width = finalRect.Width;
            _text.Width =  finalRect.Width - 2 * BorderThickness - 2;
            this.Height = finalRect.Height;
            _text.Height = finalRect.Height - 2 * BorderThickness;
            _selection.Height = _cursor.Height = _text.Height - 2;
        }

        private void SetClipRect(UIElement element, Rect clip)
        {
            RectangleGeometry rg = new RectangleGeometry();
            rg.Rect = clip;
            element.SetValue(UIElement.ClipProperty, rg);
        }

        private Size IncreaseForMargin(Size size, Thickness margin)
        {
            return new Size(
                margin.Left + margin.Right + size.Width,
                margin.Top + margin.Bottom + size.Height);
        }

        private Rect DecreaseForMargin(Rect rect, Thickness margin)
        {
            return new Rect(
                rect.Left + margin.Left,
                rect.Top + margin.Top,
                rect.Width - margin.Left - margin.Right,
                rect.Height - margin.Top - margin.Bottom);
        }

        private static void InitializeMap()
        {
            map = new Dictionary<int, char>();

            map[-2336] = ' ';
            map[-5168] = '0';
            map[-5425] = '1';
            map[-5682] = '2';
            map[-5939] = '3';
            map[-6196] = '4';
            map[-6453] = '5';
            map[-6710] = '6';
            map[-6967] = '7';
            map[-7224] = '8';
            map[-7481] = '9';
            map[-7745] = 'a';
            map[-8002] = 'b';
            map[-8259] = 'c';
            map[-8516] = 'd';
            map[-8773] = 'e';
            map[-9030] = 'f';
            map[-9287] = 'g';
            map[-9544] = 'h';
            map[-9801] = 'i';
            map[-10058] = 'j';
            map[-10315] = 'k';
            map[-10572] = 'l';
            map[-10829] = 'm';
            map[-11086] = 'n';
            map[-11343] = 'o';
            map[-11600] = 'p';
            map[-11857] = 'q';
            map[-12114] = 'r';
            map[-12371] = 's';
            map[-12628] = 't';
            map[-12885] = 'u';
            map[-13142] = 'v';
            map[-13399] = 'w';
            map[-13656] = 'x';
            map[-13913] = 'y';
            map[-14170] = 'z';
            map[-65466] = ';';
            map[-65467] = '=';
            map[-65468] = ',';
            map[-65469] = '-';
            map[-65470] = '.';
            map[-65471] = '/';
            map[-65472] = '`';
            map[-65499] = '[';
            map[-65500] = '\\';
            map[-65501] = ']';
            map[-65502] = '\'';
            map[5168] = ')';
            map[5425] = '!';
            map[5682] = '@';
            map[5939] = '#';
            map[6196] = '$';
            map[6453] = '%';
            map[6710] = '&';
            map[6710] = '^';
            map[7224] = '*';
            map[7481] = '(';
            map[7745] = 'A';
            map[8002] = 'B';
            map[8259] = 'C';
            map[8516] = 'D';
            map[8773] = 'E';
            map[9030] = 'F';
            map[9287] = 'G';
            map[9544] = 'H';
            map[9801] = 'I';
            map[10058] = 'J';
            map[10315] = 'K';
            map[10572] = 'L';
            map[10829] = 'M';
            map[11086] = 'N';
            map[11343] = 'O';
            map[11600] = 'P';
            map[11857] = 'Q';
            map[12114] = 'R';
            map[12371] = 'S';
            map[12628] = 'T';
            map[12885] = 'U';
            map[13142] = 'V';
            map[13399] = 'W';
            map[13656] = 'X';
            map[13913] = 'Y';
            map[14170] = 'Z';
            map[65466] = ':';
            map[65467] = '+';
            map[65468] = '<';
            map[65469] = '_';
            map[65470] = '>';
            map[65471] = '?';
            map[65472] = '~';
            map[65499] = '{';
            map[65500] = '|';
            map[65501] = '}';
            map[65502] = '"';
        }

        const int TextOffset = 2;

        private TextBlock _text;
        private Rectangle _border, _selection, _cursor;
        private Storyboard _cursorBlink;
        private bool _hasFocus;

        private Color _borderColor = Colors.Black, _focusColor = Colors.Blue;

        private TextBlock _scratch;
        private double[] _widths;
        private double[] _cumulativeWidths;
        private int _selStart, _selLength, _cursorIndex;
        bool _isSelecting;
        int _dragStart, _dragEnd;
        bool _gotControlKey;
        bool _mouseDown;

        private Canvas _rootCanvas;

        private Canvas _shiftCanvas;

        static Dictionary<int, char> map;

        public event EventHandler EnterPressed;
    }
}
