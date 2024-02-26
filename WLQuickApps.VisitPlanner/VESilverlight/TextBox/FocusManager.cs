//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace VESilverlight
{
    public static class FocusManager
    {
        #region MouseHasLeftControl
        public static event EventHandler MouseHasLeftControl
        {
            add
            {
                _mouseHasLeftControlHandlers.Add(new WeakReference(value));
                CleanupEventHandlers();
            }

            remove
            {
                for (int index = 0; index < _mouseHasLeftControlHandlers.Count; ++index)
                {
                    WeakReference wr = _mouseHasLeftControlHandlers[index];

                    if (wr.IsAlive)
                    {
                        EventHandler eh = (EventHandler)wr.Target;
                        if (eh.Equals(value))
                        {
                            _mouseHasLeftControlHandlers.RemoveAt(index);
                            return;
                        }
                    }
                }
                CleanupEventHandlers();
            }
        }

        private static void CleanupEventHandlers()
        {
            int index = 0;

            while (index < _mouseHasLeftControlHandlers.Count)
            {
                WeakReference wr = _mouseHasLeftControlHandlers[index];

                if (!wr.IsAlive)
                {
                    _mouseHasLeftControlHandlers.RemoveAt(index);
                }
                else
                    ++index;
            }
        }

        private static List<WeakReference> _mouseHasLeftControlHandlers = new List<WeakReference>();

        internal static void RaiseMouseHasLeftControl()
        {
            foreach (WeakReference wr in _mouseHasLeftControlHandlers)
            {
                if (wr.IsAlive)
                {
                    EventHandler eh = (EventHandler)wr.Target;
                    eh.Invoke(null, new EventArgs());
                }
            }
        }
        #endregion // MouseHasLeftControl

        public static LayoutControl FocusedElement
        {
            get { return _focusedElement; }
            set
            {
                if (_focusedElement == value)
                    return;

                if (_focusedElement != null)
                    _focusedElement.OnLostFocus();
                
                _focusedElement = value;

                if (_focusedElement != null)
                    _focusedElement.OnGotFocus();
            }
        }

        private static LayoutControl _focusedElement;

        internal static void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            if (_focusedElement != null)
                _focusedElement.OnKeyDown(sender, e);
        }

        internal static void OnKeyUp(object sender, KeyboardEventArgs e)
        {
            if (_focusedElement != null)
                _focusedElement.OnKeyUp(sender, e);
        }
    }

    public class WeakReferenceEventHandlerList<T> where T : EventArgs
    {
        public void Add(T value)
        {
            _handlers.Add(new WeakReference(value));
            CleanupEventHandlers();
        }

        public void Remove(T value)
        {
            for (int index = 0; index < _handlers.Count; ++index)
            {
                WeakReference wr = _handlers[index];

                if (wr.IsAlive)
                {
                    T eh = (T)wr.Target;
                    if (eh.Equals(value))
                    {
                        _handlers.RemoveAt(index);
                        return;
                    }
                }
            }
            CleanupEventHandlers();
        }

        public void Raise(object sender, T value)
        {
            foreach (WeakReference wr in _handlers)
            {
                if (wr.IsAlive)
                {
                    EventHandler eh = (EventHandler)wr.Target;
                    eh.Invoke(null, value);
                }
            }
        }

        private void CleanupEventHandlers()
        {
            int index = 0;

            while (index < _handlers.Count)
            {
                WeakReference wr = _handlers[index];

                if (!wr.IsAlive)
                {
                    _handlers.RemoveAt(index);
                }
                else
                    ++index;
            }
        }

        List<WeakReference> _handlers = new List<WeakReference>();
    }
}
