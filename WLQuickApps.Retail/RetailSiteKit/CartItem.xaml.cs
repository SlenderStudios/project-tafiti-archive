using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RetailSiteKit
{
    public partial class CartItem : UserControl
    {
        FrameworkElement root;
        public double itemPrice;
        public int itemNumber;
        public int orderNumber;
        //public TextBlock itemQuantityShadow;
        //public TextBlock itemQuantity;
        //public TextBlock itemSize;
        //public TextBlock itemSizeShadow;
        //public Canvas removeBtn;
        //public Storyboard slideUp;
        //public Storyboard slideDown;
        //public Storyboard slideUpAdjust;
        //public Storyboard slideDownAdjust;
        //public Storyboard smallUpAdjust;
        //public Storyboard smallDownAdjust;
        //public Storyboard removeOver;
        //public Storyboard removeOut;
        public Canvas checkoutFiles;
        public TextBlock itemCartPrice;
        public TextBlock itemCartName;
        public Image itemCartImage;
        //public Canvas upperLine;
        public string sizeArray;
        public int currSize;
        public int currQuantity;
        protected bool _inCart = true;
        public double top;

        public event EventHandler removeClick;
        public event EventHandler updateAmountsUp;
        public event EventHandler updateAmountsDown;
        public event EventHandler updateQuantityUp;
        public event EventHandler updateQuantityDown;

        public CartItem()
        {
            InitializeComponent();
        }
        public CartItem(double top)
        {
            InitializeComponent();
            // Stream s = this.GetType().Assembly.GetManifestResourceStream("RetailSiteKit.CartItem.xaml");
            // root = this.InitializeFromXaml(new StreamReader(s).ReadToEnd());
            //Application.LoadComponent(this, new Uri("RetailSiteKit.CartItem.xaml", UriKind.Relative));

            //itemSize = ((TextBlock)root.FindName("itemSize"));
            //itemSizeShadow = ((TextBlock)root.FindName("itemSizeShadow"));
            //itemQuantityShadow = ((TextBlock)root.FindName("itemQuantityShadow"));
            //itemQuantity = ((TextBlock)root.FindName("itemQuantity"));
            itemCartPrice = cartItemPrice;
            itemCartName = cartItemName;
            itemCartImage = cartItemImage;
            checkoutFiles = CheckoutFiles;
            //upperLine = ((Canvas)root.FindName("upperLine"));

            //removeBtn = ((Canvas)root.FindName("removeBtn"));
            removeBtn.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(removeBtn_MouseLeftButtonDown);

            //removeOut = ((Storyboard)root.FindName("removeOut"));
            //removeOver = ((Storyboard)root.FindName("removeOver"));
            //slideUp = ((Storyboard)root.FindName("slideUp"));
            //slideDown = ((Storyboard)root.FindName("slideDown"));
            //slideUpAdjust = ((Storyboard)root.FindName("slideUpAdjust"));
            //slideDownAdjust = ((Storyboard)root.FindName("slideDownAdjust"));
            //smallUpAdjust = ((Storyboard)root.FindName("smallUpAdjust"));
            //smallDownAdjust = ((Storyboard)root.FindName("smallDownAdjust"));
            removeBtn.MouseEnter += new System.Windows.Input.MouseEventHandler(removeBtn_MouseEnter);
            removeBtn.MouseLeave += new System.Windows.Input.MouseEventHandler(removeBtn_MouseLeave);
            //string animation = string.Format(ANIMATION); 
            //Storyboard storyboard = XamlReader.Load(animation) as Storyboard;

            this.SetValue(Canvas.TopProperty, top);
        }

        void removeBtn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            removeOut.Begin();
        }

        void removeBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            removeOver.Begin();
        }
        //private const string ANIMATION = "<Storyboard x:Name=\"adjustItem\">"
        //+ "<DoubleAnimation Storyboard.TargetName=\"CartItem\" Storyboard.TargetProperty=\"(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.Y)\" By=\"-60\"  Duration=\"0:0:00.3\"  />"
        //+ "</Storyboard>";


        public bool InCart
        {

            get
            {
                return _inCart;
            }
            set
            {
                _inCart = value;
                if (_inCart)
                {
                    //RedBorderSmall_In.Begin();
                    //IndexChanged(this, new EventArgs());


                }
                else
                {
                    //RedBorderSmall_Out.Begin();
                    //itemMouseOver_Out.Begin();
                    removeClick(this, new EventArgs());
                }
            }

        }

        void removeBtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            InCart = false;
        }
        void onMouseEnter(object sender, EventArgs e)
        {
            ((FrameworkElement)sender).Opacity = 1;
        }
        void onMouseLeave(object sender, EventArgs e)
        {
            ((FrameworkElement)sender).Opacity = .5;
        }
        void updateItemQuantity(object sender, EventArgs e)
        {
            if (((FrameworkElement)sender).Name == "itemQuantityUp")
            {

                currQuantity = currQuantity + 1;
                itemQuantity.Text = currQuantity.ToString();
                itemQuantityShadow.Text = currQuantity.ToString();
                updateAmountsUp(this, new EventArgs());
                updateQuantityUp(this, new EventArgs());
            }
            else
            {

                currQuantity = currQuantity - 1;
                itemQuantity.Text = currQuantity.ToString();
                itemQuantityShadow.Text = currQuantity.ToString();
                updateAmountsDown(this, new EventArgs());
                updateQuantityDown(this, new EventArgs());
            }
            updateItemPrice();
        }
        //uncomment to have the cartItems update thier price to reflect the quantity of items 

        void updateItemPrice()
        {
            //itemCartPrice.Text = "$" + (currQuantity * itemPrice).ToString();
        }

        void updateItemSize(object sender, EventArgs e)
        {

            string[] itemSizeArray = sizeArray.Split('|');


            if (((FrameworkElement)sender).Name == "itemSizeUp")
            {
                if (currSize < itemSizeArray.Length - 1)
                {
                    currSize = currSize + 1;
                    itemSize.Text = itemSizeArray[currSize];
                    itemSizeShadow.Text = itemSizeArray[currSize];
                }
            }
            else
            {
                if (currSize > 0)
                {
                    currSize = currSize - 1;
                    itemSize.Text = itemSizeArray[currSize];
                    itemSizeShadow.Text = itemSizeArray[currSize];
                }
            }
        }
    }
}
