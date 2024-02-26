//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace VESilverlight
{
    public class LayoutQueue
    {
        internal LayoutQueue(LayoutFlags flag)
        {
            _flag = flag;
        }

        internal ILayout Dequeue()
        {
            if (!IsEmpty)
            {
                ILayout item = _items[0];
                _items.RemoveAt(0);
                item.LayoutStorage.SetFlag(_flag, false);
                return item;
            }

            return null;
        }

        internal void Enqueue(ILayout item)
        {
            if (!item.LayoutStorage.GetFlag(_flag))
            {
                _items.Add(item);
                item.LayoutStorage.SetFlag(_flag, true);

                Panel panel = item as Panel;
                if (panel != null)
                {
                    foreach (Visual child in panel.Children)
                    {
                        ILayout layoutChild = child as ILayout;
                        if (layoutChild != null)
                        {
                            Remove(layoutChild);
                        }
                    }
                }
            }
        }

        internal bool Remove(ILayout item)
        {
            if (item.LayoutStorage.GetFlag(_flag))
            {
                _items.Remove(item);
                item.LayoutStorage.SetFlag(_flag, false);
                return true;
            }

            return false;            
        }

        internal void Clear()
        {
            foreach (ILayout item in _items)
            {
                item.LayoutStorage.SetFlag(_flag, false);
            }

            _items.Clear();
        }

        internal bool IsEmpty
        {
            get { return _items.Count == 0; }
        }

        private LayoutFlags _flag;
        private List<ILayout> _items = new List<ILayout>();
    }
}
