//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;

namespace VESilverlight {

    // Implements a ListBox control. The ListBox is a Control
    // that has a collection of Items - FrameworkElements. All Items have
    // equal height and are positioned vertically. 
    public partial class Repeater : UserControl
    {

        #region Public Methods

        // Default ListBox ctor - sets the Content Canvas. We need that for scrolling.
        public Repeater()
        {
            this.InitializeComponent();

            content = new Canvas();
            //must be bellow every thing else
            this.LayoutRoot.Children.Insert(0, content);

            // TODO: RootLeave issue
            //RootLeave += new EventHandler(OnRootLeave);

            this.Loaded += new RoutedEventHandler(Repeater_Loaded);


        }

        void Repeater_Loaded(object sender, RoutedEventArgs e)
        {
            this.OnLoaded(sender, e);
        }

        void nextPage_Click(object sender, EventArgs e)
        {
            NextPage();
        }

        void prevPage_Click(object sender, EventArgs e)
        {
            PreviousPage();
        }

        public void PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;

                OnScrollChanged(null, null);
            }
        }

        public void NextPage()
        {
            if (currentPage < pageCount)
            {
                currentPage++;

                OnScrollChanged(null, null);
            }
        }

        //Updates the content children with the current items in the list
        public virtual void UpdateItems()
        {
            //remove all the children and start from empty
            content.Children.Clear();

            this.ControlCanvas.Opacity = 0;

            //place the items and add a scrollbar if needed
            if (items.Count < 1) {
                return;
            }

            itemHeight = items[0].Height;
            if (itemHeight <= 0) {
                throw new InvalidOperationException("items height must be specified");
            }

            //first check if all have the same Height
            foreach (FrameworkElement item in items) {
                if (item.Height != itemHeight) {
                    throw new InvalidOperationException("different items height is not supported");
                }
            }

            //attach the items
            double pos = 0;
            foreach (FrameworkElement item in items) {
                item.SetValue(Canvas.LeftProperty, 0.0);
                item.SetValue(Canvas.TopProperty, pos);
                content.Children.Add(item);
                pos += itemHeight;
            }

            itemsPerPage = (int)Math.Floor((Height - this.ControlCanvas.Height) / itemHeight);
            pageCount = (int)Math.Ceiling((double)Items.Count / (double)itemsPerPage);
            
            //check if we need to show the page controls
            if (pos > Height) {
                //change the rectangle width if needed
                if (background != null) {
                    background.Height = Height - this.ControlCanvas.Height;
                }
                this.ControlCanvas.Opacity = 100;
                this.ControlCanvas.SetValue(Canvas.TopProperty, background.Height);
                this.ControlCanvas.SetValue(Canvas.LeftProperty, Width - this.ControlCanvas.Width);
                OnScrollChanged(this, new EventArgs());
            } else {
                this.ControlCanvas.Opacity = 0;
                //change the rectangle width if needed
                if (background != null) {
                    background.Height = Height;
                }
            }

        }

        #endregion Public Methods

        #region Public Properties

        // The collection of items for the ListBox
        public ICollection<FrameworkElement> Items
        {
            get { return items; }
        }

        // Currently selected item - null if no selection
        public FrameworkElement SelectedItem
        {
            get { return selectedItem; }
        }

        #endregion Public Properties

        #region Public Events

        // Fired when the selected item changes
        public event EventHandler SelectionChanged;

        #endregion Public Events

        #region Protected Methods

        // CaptureMouse
        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            if (CheckMousePosition(args)) {
                this.CaptureMouse();
            }
        }

        // If the mouse is on the ListBox find on which item and select it.
        protected void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            this.ReleaseMouseCapture();

            //do thing only if we are outside the ScrollBar
            if (CheckMousePosition(args)) {
                Point pt = args.GetPosition(this);
                if ((items.Count > 0) && (pt.X < Width) && (pt.Y < Height)) {
                    FrameworkElement newSelection = GetItemByPosition(pt);
                    if (selectedItem != newSelection) {
                        Select(newSelection);
                        if (SelectionChanged != null) {
                            SelectionChanged(this, null);
                        }
                    }
                }
            }
        }

        protected FrameworkElement GetItemByPosition(Point pt)
        {
            if (items.Count == 0) return null;
            double contentY = pt.Y - (double)(content.GetValue(Canvas.TopProperty));
            int itemNumber = (int)(contentY / itemHeight);
            return ( itemNumber < items.Count ? items[itemNumber] : null );
        }

        //scroll by whole pages
        protected void OnScrollChanged(object sender, EventArgs args)
        {
            if (currentPage < 10)
            {
                this.CurrentPage.Text = " ";
            }
            this.CurrentPage.Text += currentPage.ToString();
            if (pageCount < 10)
            {
                this.TotalPages.Text = " ";
            }
            this.TotalPages.Text += pageCount.ToString();
            
            content.SetValue(Canvas.TopProperty, -((currentPage - 1)* itemsPerPage * itemHeight));

            int topVisible = (currentPage - 1)* itemsPerPage;

            int bottomVisible = currentPage * itemsPerPage - 1;

            for (int x = 0; x < items.Count; x++)
            {
                FrameworkElement item = items[x];

                if (x <= bottomVisible && x >= topVisible)
                {
                    item.Visibility = Visibility.Visible;
                }
                else
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }

        // When the mouse leaves the root visual we loose capture
        protected void OnRootLeave(object sender, EventArgs args)
        {
            this.ReleaseMouseCapture();
        }

        //Attaches the items to ListBox
        protected void OnLoaded(object sender, RoutedEventArgs args)
        {
            UpdateItems();
        }

        //Sets the Scrollbar size and position if any is attached
        protected void UpdateLayout()
        {
            base.UpdateLayout();
            //change the frame size too
            if (background != null) {
                background.Width = Width;

                //only do this if control buttons visible (more than one page)
                if (this.ControlCanvas.Visibility == Visibility.Visible)
                    background.Height = Height - this.ControlCanvas.Height;
            }

            //update clipping area
            RectangleGeometry clip = new RectangleGeometry();

            clip.Rect = new Rect(0, 0, Width, Height);
           
            Clip = clip;
        }

        #endregion Protected Methods

        #region Protected Prperties

        #endregion Protected Properties

        #region Private Methods

        // saves the selected item
        protected void Select(FrameworkElement item)
        {
            selectedItem = item;
        }

        

        // Returns true if the mouse is outside of the ScrollBar area
        private bool CheckMousePosition(MouseEventArgs args)
        {
            bool onTheList = true;
            return onTheList;
        }

        #endregion Private Methods

        #region Data

        protected Canvas content;  //contains the items so they can be scrolled if needed
        private double itemHeight; //all items must have equal height to allow correct scrolling
        private List<FrameworkElement> items = new List<FrameworkElement>();
        private FrameworkElement selectedItem = null;

        private int pageCount = 1;
        private int itemsPerPage = 0;
        private int currentPage = 1;

        #endregion Data
    }
}
