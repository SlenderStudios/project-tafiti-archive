using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MetaliqSilverlightSDK;
using RetailXmlApi.net;
using RetailXmlApi.model;
using System.Collections.Generic;

namespace RetailSiteKit
{


    public partial class CartControl : UserControl
    {
        bool scrolling = false;
        double lastPos;
        double canvasCurrentPos;
        double scrollHit;

        public List<CartItem> CartList;
        double pos = 0;
        int numberInCart = 0;
        int thisQuantity;
        string thisSize;
        protected int _selectedItem;
        protected bool itemsUp = false;
        public double subtotalAmount = 0;
        public double totalItemsInCart = 0;
        double trackTop;
        double trackBottom;
        public event EventHandler removeClick;
        public event EventHandler updateAmountsUp;
        public event EventHandler updateAmountsDown;
        public event EventHandler updateQuantityUp;
        public event EventHandler updateQuantityDown;

        public CartControl()
        {
            // Required to initialize variables
            InitializeComponent();

            CartList = new List<CartItem>();

            foreach (CartItem cartItem in CartList)
            {
                cartItem.removeClick += new EventHandler(cartItem_removeClick);
                cartItem.updateAmountsUp += new EventHandler(cartItem_updateAmountsUp);
                cartItem.updateAmountsDown += new EventHandler(cartItem_updateAmountsDown);
                cartItem.updateQuantityUp += new EventHandler(cartItem_updateQuantityUp);
                cartItem.updateQuantityDown += new EventHandler(cartItem_updateQuantityDown);

            }
            LayoutCartNav.Height = 0;
            //init LayoutCartNav's left margin
            LayoutCartNav.SetValue(Canvas.LeftProperty, 220.00);
            trackTop = (double)LayoutCart.GetValue(Canvas.TopProperty);
            //trackBottom = (double)LayoutCartNav.GetValue(Canvas.TopProperty) + (LayoutCartNav.Height);
            LayoutCart.MouseMove += new MouseEventHandler(LayoutCartNav_MouseMove);
            LayoutCart.MouseLeftButtonDown += new MouseButtonEventHandler(LayoutCart_MouseLeftButtonDown);
            LayoutCart.MouseLeftButtonUp += new MouseButtonEventHandler(LayoutCart_MouseLeftButtonUp);
            LayoutCart.MouseLeave += new MouseEventHandler(LayoutCart_MouseLeave);

        }

        void cartItem_updateQuantityDown(object sender, EventArgs e)
        {
            totalItemsInCart -= 1;
            updateQuantityDown(this, e);
        }

        void cartItem_updateQuantityUp(object sender, EventArgs e)
        {
            totalItemsInCart += 1;
            updateQuantityUp(this, e);
        }

        void cartItem_updateAmountsDown(object sender, EventArgs e)
        {
            subtotalAmount -= ((CartItem)sender).itemPrice;
            updateAmountsDown(this, e);

        }

        void cartItem_updateAmountsUp(object sender, EventArgs e)
        {
            subtotalAmount += ((CartItem)sender).itemPrice;
            updateAmountsUp(this, e);
        }

        void LayoutCart_MouseLeave(object sender, MouseEventArgs e)
        {
            scrolling = false;
        }

        void LayoutCart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrolling = false;
            lastPos = e.GetPosition(LayoutCart).Y;

        }

        void LayoutCart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            trackBottom = (double)LayoutCartNav.GetValue(Canvas.TopProperty) + (LayoutCartNav.Height);
            scrolling = true;
            canvasCurrentPos = e.GetPosition(this).Y;
            scrollHit = e.GetPosition(LayoutCartNav).Y;
            LayoutCartNav.CaptureMouse();
            Page.app.debug.Text = "canvastop" + LayoutCartNav.GetValue(Canvas.TopProperty).ToString() + " : " + "height" + (LayoutCartNav.Height).ToString() + " =" + trackBottom.ToString();
        }

        void LayoutCartNav_MouseMove(object sender, MouseEventArgs e)
        {


            if (scrolling == true)
            {
                double y = e.GetPosition(LayoutCart).Y;
                double newOrigin = new double();



                newOrigin = scrollHit - y;
                if (newOrigin < trackTop) newOrigin = trackTop;

                if (newOrigin > trackBottom) newOrigin = trackBottom;
                LayoutCartNav.SetValue(Canvas.TopProperty, (-newOrigin));

                Page.app.debug.Text = "scrollHit" + scrollHit.ToString() + "-" + "y" + y.ToString() + " = " + newOrigin.ToString();
            }
            //Page.app.debug.Text = newor
        }


        void cartItem_removeClick(object sender, EventArgs e)
        {
            _selectedItem = ((CartItem)sender).itemNumber;
            CartList.Remove((CartItem)sender);
            LayoutCartNav.Children.Remove((CartItem)sender);
            numberInCart -= 1;
            subtotalAmount -= RetailApi.Instance.GetProductById(_selectedItem, Page.app.currentBrand).Price * Convert.ToDouble(((CartItem)sender).itemQuantity.Text);
            totalItemsInCart -= Convert.ToDouble(((CartItem)sender).itemQuantity.Text);
            removeClick(this, e);
            foreach (CartItem cartItem in CartList)
            {
                if (((CartItem)sender).orderNumber < cartItem.orderNumber)
                {
                    cartItem.slideUpAdjust.Begin();
                    //cartItem.adjust.by
                    cartItem.orderNumber -= 1;
                    cartItem.top -= 93;

                }
            }
            if (93 * CartList.Count >= 279)
                LayoutCartNav.Height = 93 * CartList.Count - 279;
            if (CartList.Count < 4)
            {
                LayoutCartNav.SetValue(Canvas.TopProperty, pos);
            }
        }

        public void addItemToCart(int selectedItem)
        {
            thisQuantity = Page.app.currQuantity;
            thisSize = Page.app.currSizeString;

            pos = 93 * numberInCart;
            CartItem cartItem = new CartItem(pos);
            cartItem.itemPrice = RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).Price;
            cartItem.currQuantity = Page.app.currQuantity;
            cartItem.currSize = Page.app.currSize;
            cartItem.sizeArray = RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).Size.ToString();
            cartItem.itemNumber = selectedItem;
            cartItem.itemCartName.Text = RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).Description.ToString();
            if (cartItem.itemCartName.Text.Length > 22)
            {
                cartItem.itemCartName.Text = cartItem.itemCartName.Text.Substring(0, 18) + "...";
            }
            cartItem.itemQuantity.Text = thisQuantity.ToString();
            cartItem.itemCartPrice.Text = "$" + (RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).Price).ToString();
            cartItem.itemSize.Text = thisSize;
            cartItem.itemSizeShadow.Text = thisSize;
            //cartItem.itemCartImage.Source = RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).MoreInfoThumbs[0].Url;
            //cartItem.itemCartImage.SetValue(Image.SourceProperty, RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).MoreInfoThumbs[0].Url);
            cartItem.itemCartImage.Source = new BitmapImage(RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).MoreInfoThumbs[0].Url);
            
            cartItem.removeClick += new EventHandler(cartItem_removeClick);
            cartItem.updateAmountsUp += new EventHandler(cartItem_updateAmountsUp);
            cartItem.updateAmountsDown += new EventHandler(cartItem_updateAmountsDown);
            cartItem.updateQuantityUp += new EventHandler(cartItem_updateQuantityUp);
            cartItem.updateQuantityDown += new EventHandler(cartItem_updateQuantityDown);
            LayoutCartNav.Children.Add(cartItem);
            CartList.Add(cartItem);
            numberInCart += 1;
            cartItem.orderNumber = numberInCart;
            subtotalAmount += RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).Price * thisQuantity;
            totalItemsInCart += Page.app.currQuantity;
            // subtotalAmount += RetailApi.Instance.GetProductById(DetailVideos.SelectedItem).Price;
            // cartItemsNumber.Text = numberInCart.ToString();
            // shoppingInCart.Text = numberInCart.ToString();
            // subtotalCart.Text = "$" + subtotalAmount.ToString();
            if (93 * CartList.Count >= 279)
                LayoutCartNav.Height = 93 * CartList.Count - 279;

        }
        public void arrangeLayout(string style)
        {
            if (style == "flyout")
            {
                if (itemsUp == true)
                {
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Colors.White;

                    foreach (CartItem cartItem in CartList)
                    {
                        cartItem.upperLine.Visibility = Visibility.Visible;
                        cartItem.checkoutFiles.Visibility = Visibility.Collapsed;
                        if (itemsUp == true)
                        {
                            cartItem.smallDownAdjust.Begin();

                        }

                        //cartItem.itemCartName.Foreground = brush;
                        //cartItem.itemCartPrice.Foreground = brush;
                    }
                    MaskEditClose.Begin();
                }
                itemsUp = false;
            }
            else if (style == "checkout")
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Colors.Black;

                foreach (CartItem cartItem in CartList)
                {
                    cartItem.checkoutFiles.Visibility = Visibility.Visible;
                    cartItem.upperLine.Visibility = Visibility.Collapsed;

                    cartItem.smallUpAdjust.Begin();
                    //cartItem.itemCartName.Foreground = brush;
                    //cartItem.itemCartPrice.Foreground = brush;
                }
                MaskEditOpen.Begin();
                itemsUp = true;
            }

            //LayoutCartNav.SetValue(Canvas.TopProperty, 0);
            LayoutCartNav.SetValue(Canvas.TopProperty, 110.0);

        }

    }
}