using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MetaliqSilverlightSDK;
using RetailXmlApi.net;
using RetailXmlApi.model;
using System.Collections.Generic;

namespace RetailSiteKit
{


	public partial class CartControl2 : UserControl
	{

        public List<CartItem> CartList;
        double pos = 0;
        int numberInCart = 0;
        int thisQuantity;
        string thisSize;
        protected int _selectedItem;
        protected bool itemsUp = false;
        public double subtotalAmount = 0;

        public event EventHandler removeClick;
        public event EventHandler updateAmountsUp;
        public event EventHandler updateAmountsDown;

		public CartControl2()
		{
			// Required to initialize variables
			InitializeComponent();

            CartList = new List<CartItem>();

            foreach (CartItem cartItem in CartList)
            {
                cartItem.removeClick += new EventHandler(cartItem_removeClick);
                cartItem.updateAmountsUp += new EventHandler(cartItem_updateAmountsUp);
                cartItem.updateAmountsDown += new EventHandler(cartItem_updateAmountsDown);
            }
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
        void cartItem_removeClick(object sender, EventArgs e)
        {
            _selectedItem = ((CartItem)sender).itemNumber;
            CartList.Remove((CartItem)sender);
            LayoutCart.Children.Remove((CartItem)sender);
            numberInCart -= 1;
            subtotalAmount -= RetailApi.Instance.GetProductById(_selectedItem, Page.app.currentBrand).Price * Convert.ToDouble(((CartItem)sender).itemQuantity.Text); 
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
            cartItem.itemCartPrice.Text = "$" + (RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).Price * thisQuantity).ToString();
            cartItem.itemSize.Text = thisSize;
            cartItem.itemSizeShadow.Text = thisSize;
            //cartItem.itemCartImage.Source = RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).MoreInfoThumbs[0].Url;
            cartItem.itemCartImage.SetValue(Image.SourceProperty, RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).MoreInfoThumbs[0].Url);
            cartItem.removeClick += new EventHandler(cartItem_removeClick);
            cartItem.updateAmountsUp +=new EventHandler(cartItem_updateAmountsUp);
            cartItem.updateAmountsDown += new EventHandler(cartItem_updateAmountsDown);
            LayoutCart.Children.Add(cartItem);
            CartList.Add(cartItem);
            numberInCart += 1;
            cartItem.orderNumber = numberInCart;
            subtotalAmount += RetailApi.Instance.GetProductById(selectedItem, Page.app.currentBrand).Price * thisQuantity; 
            // subtotalAmount += RetailApi.Instance.GetProductById(DetailVideos.SelectedItem).Price;
            // cartItemsNumber.Text = numberInCart.ToString();
            // shoppingInCart.Text = numberInCart.ToString();
            // subtotalCart.Text = "$" + subtotalAmount.ToString();
        }
        public void arrangeLayout(string style)
        {
            if (style == "flyout")
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Colors.White;

                foreach (CartItem cartItem in  CartList)
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
                itemsUp = true;
            }
        }
       
	}
}