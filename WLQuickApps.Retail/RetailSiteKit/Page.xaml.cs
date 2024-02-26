using System;
using System.Linq;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Media.Imaging;
//using System.Windows.Browser.Net;
using System.Collections.Generic;
//using RetailSiteKit.components;
using RetailXmlApi.net;
using RetailXmlApi.model;
using MetaliqSilverlightSDK.net;


namespace RetailSiteKit
{
    public partial class Page : UserControl
    {
        public string currentBrand = Brand.Cortefiel.ToString();
        public static Page app;
        // Global Variables
        public int currQuantity = 1;
        public int currSize = 0;
        public string currSizeString;
        bool loggedIn = false;
        bool LiveIdloggedIn;
        string slideOutState;
        string CurrentPage = "home";
        string altPage = "home";
        string currentCatalog = "ClientBin/xmlFiles/allData.xml";
        int videoSource = 0;
        int thumbSource = 0;
        VideoDetailsGrid DetailVideos;
        VideoGrid Videos;
        CartControl cartControl;
        public TextBlock codeDebug;
        public int numberInCart;
        double pos = 0;
        double subtotalAmount = 0;
        bool checkoutOpen = false;
        public string _StaticAssetURL = string.Empty;
        public string _Appid = string.Empty;
        public string _VideoPath = string.Empty;

        public Page(string staticAssetURL, string videoPath, string appid)
        {
            InitializeComponent();
            app = this;
            _Appid = appid;
            _VideoPath = videoPath;
            _StaticAssetURL = staticAssetURL;
            this.Loaded += this.Page_Loaded;
        }

        private void Page_Loaded(object sender, EventArgs args)
        {
            // Required to initialize variables.  Needs to be done from loaded event so FindName works properly. 
            InitializeComponent();

            //MetaliqSilverlightSDK.AuthenticationState.StateChange += new EventHandler(AuthenticationState_StateChange);

            //LiveIdloggedIn = MetaliqSilverlightSDK.AuthenticationState.IsLoggedIn;

            // test
            string foo = "Blue|Grey|Beige";
            string[] stringArray = foo.Split('|');

            for (int i = 0; i < stringArray.Length; i++)
            {
                string item = stringArray[i];
            }
            string loginCookie = null;
            try
            {
                loginCookie = MetaliqSilverlightSDK.net.CookieUtil.GetCookie("webauthtoken");
            }
            catch (Exception ex)
            { }

            LiveIdloggedIn = loginCookie != null;

            if (LiveIdloggedIn == true)
            {
                
                loginBtnCanvas.Visibility = Visibility.Collapsed;
                logoutBtnCanvas.Visibility = Visibility.Visible;
                logoutText.Visibility = Visibility.Visible;
                loginText.Visibility = Visibility.Collapsed;
                loggedIn = true;
                loginHeaderText.Text = "Thank you for logging in";
                WLIDText.Text = "Windows Live ID Sign - out";
            }
            else
            {
                loginBtnCanvas.Visibility = Visibility.Visible;
                logoutBtnCanvas.Visibility = Visibility.Collapsed;
                logoutText.Visibility = Visibility.Collapsed;
                loginText.Visibility = Visibility.Visible;
                loggedIn = false;
                loginHeaderText.Text = "Please Login";
                WLIDText.Text = "Windows Live ID Sign - in";
            }
                
            
            //testG = new VideoGrid();
            //selectedItemDetails.Children.Add(testG);
            codeDebug = new TextBlock();
            cartControl = new CartControl();
            shoppingCart.Children.Add(cartControl);
            cartControl.removeClick += new EventHandler(cartControl_removeClick);
            cartControl.updateAmountsUp += new EventHandler(cartControl_updateAmountsUp);
            cartControl.updateAmountsDown += new EventHandler(cartControl_updateAmountsDown);
            cartControl.updateQuantityUp += new EventHandler(cartControl_updateQuantityUp);
            cartControl.updateQuantityDown += new EventHandler(cartControl_updateQuantityDown);
            // Insert code required on object creation below this point.
            /*
            RetailApi.Instance.Error += new EventHandler(Instance_Error);
            RetailApi.Instance.Loaded += new EventHandler(Instance_Loaded);
            RetailApi.Instance.Load("data.xml");
            */
            numberInCart = 0;
            // Global Navigation Event Handlers
            returnBtn.MouseLeave += new MouseEventHandler(returnBtn_MouseLeave);
            returnBtn.MouseEnter += new MouseEventHandler(returnBtn_MouseEnter);
            purchaseBtn.MouseLeave += new MouseEventHandler(purchaseBtn_MouseLeave);
            purchaseBtn.MouseEnter += new MouseEventHandler(purchaseBtn_MouseEnter);
            checkoutBtn.MouseLeave += new MouseEventHandler(checkoutBtn_MouseLeave);
            checkoutBtn.MouseEnter += new MouseEventHandler(checkoutBtn_MouseEnter);
            editCartBtn.MouseLeave += new MouseEventHandler(editCartBtn_MouseLeave);
            editCartBtn.MouseEnter += new MouseEventHandler(editCartBtn_MouseEnter);
            cartDown.MouseLeftButtonDown += new MouseButtonEventHandler(cartDown_MouseLeftButtonDown);
            cartUp.MouseLeftButtonDown += new MouseButtonEventHandler(cartUp_MouseLeftButtonDown);
            homeBtn.MouseLeftButtonUp += new MouseButtonEventHandler(homeBtn_MouseLeftButtonUp);
            homeBtn.MouseLeave += new MouseEventHandler(homeBtn_MouseLeave);
            homeBtn.MouseEnter += new System.Windows.Input.MouseEventHandler(homeBtn_MouseEnter);
            catalogBtn.MouseLeave += new MouseEventHandler(catalogBtn_MouseLeave);
            catalogBtn.MouseEnter += new System.Windows.Input.MouseEventHandler(catalogBtn_MouseEnter);
            //catalogBtn.MouseLeftButtonUp += new MouseEventHandler(catalogBtn_MouseLeftButtonUp);
            accountBtn.MouseLeave += new MouseEventHandler(accountBtn_MouseLeave);
            accountBtn.MouseEnter += new System.Windows.Input.MouseEventHandler(accountBtn_MouseEnter);
            loginBtn.MouseLeave += new MouseEventHandler(loginBtn_MouseLeave);
            loginBtn.MouseEnter += new System.Windows.Input.MouseEventHandler(loginBtn_MouseEnter);
            //loginBtnButton.MouseLeftButtonUp += new MouseButtonEventHandler(loginBtnButton_MouseLeftButtonUp);
            cartBtn.MouseLeave += new MouseEventHandler(cartBtn_MouseLeave);
            cartBtn.MouseEnter += new System.Windows.Input.MouseEventHandler(cartBtn_MouseEnter);
            timerSB.Completed += new EventHandler(timerSB_Completed);
            selectedItemDetails.MouseEnter += new MouseEventHandler(selectedItemDetails_MouseEnter);
            selectedItemDetails.MouseLeave += new MouseEventHandler(selectedItemDetails_MouseLeave);
            advSearchBtn.MouseLeftButtonUp += new MouseButtonEventHandler(advSearchBtn_MouseLeftButtonUp);
            searchCloseBtn.MouseLeftButtonUp += new MouseButtonEventHandler(searchCloseBtn_MouseLeftButtonUp);
            // Videos.ItemClicked += new EventHandler(Videos_ItemClicked);
            purchaseBtn.MouseLeftButtonDown += new MouseButtonEventHandler(purchaseBtn_MouseLeftButtonDown);
            editCartBtn.MouseLeftButtonDown += new MouseButtonEventHandler(editCartBtn_MouseLeftButtonDown);
            checkoutBtn.MouseLeftButtonDown += new MouseButtonEventHandler(checkoutBtn_MouseLeftButtonDown);
            returnBtn.MouseLeftButtonDown += new MouseButtonEventHandler(returnBtn_MouseLeftButtonDown);

            MessengerBtn.MouseLeftButtonDown += new MouseButtonEventHandler(MessengerBtn_MouseLeftButtonDown);

            VideoWall.Stop();
            VideoWall.MediaOpened += new RoutedEventHandler(VideoWall_MediaOpened);
            //
            pdhLogo.MouseLeftButtonUp += new MouseButtonEventHandler(pdhLogo_MouseLeftButtonUp);
            milanoLogo.MouseLeftButtonUp += new MouseButtonEventHandler(milanoLogo_MouseLeftButtonUp);
            secretLogo.MouseLeftButtonUp += new MouseButtonEventHandler(secretLogo_MouseLeftButtonUp);
            springfieldLogo.MouseLeftButtonUp += new MouseButtonEventHandler(springfieldLogo_MouseLeftButtonUp);
            cortefielLogo.MouseLeftButtonUp += new MouseButtonEventHandler(cortefielLogo_MouseLeftButtonUp);

            pdhLogo.MouseEnter += new MouseEventHandler(pdhLogo_MouseEnter);
            milanoLogo.MouseEnter += new MouseEventHandler(milanoLogo_MouseEnter);
            secretLogo.MouseEnter += new MouseEventHandler(secretLogo_MouseEnter);
            springfieldLogo.MouseEnter += new MouseEventHandler(springfieldLogo_MouseEnter);
            cortefielLogo.MouseEnter += new MouseEventHandler(cortefielLogo_MouseEnter);

            pdhLogo.MouseLeave += new MouseEventHandler(pdhLogo_MouseLeave);
            milanoLogo.MouseLeave += new MouseEventHandler(milanoLogo_MouseLeave);
            secretLogo.MouseLeave += new MouseEventHandler(secretLogo_MouseLeave);
            springfieldLogo.MouseLeave += new MouseEventHandler(springfieldLogo_MouseLeave);
            cortefielLogo.MouseLeave += new MouseEventHandler(cortefielLogo_MouseLeave);

            #region stuff
            hover_account.Visibility = Visibility.Collapsed;
            hover_cart.Visibility = Visibility.Collapsed;
            hover_login.Visibility = Visibility.Collapsed;
            hover_catalog.Visibility = Visibility.Collapsed;
            login.Visibility = Visibility.Collapsed;
            companyLogos.Visibility = Visibility.Collapsed;
            accountInfo.Visibility = Visibility.Collapsed;
            selectedItemDetails.Visibility = Visibility.Collapsed;

            //logoutBtnCanvas.Visibility = Visibility.Collapsed;
            //logoutText.Visibility = Visibility.Collapsed;

            searchCloseBtn.Visibility = Visibility.Collapsed;

            advancedSearchShadow.IsHitTestVisible = false;
            checkoutPage.IsHitTestVisible = false;
            confirmation.IsHitTestVisible = false;
            shoppingCart.IsHitTestVisible = false;
            
            
            //debug.Text = WebApplication.Current.StartupArguments["video"];
            
            loadVideoGrid();
            
            loadVideoDetailsGrid();
            DetailVideos.Visibility = Visibility.Collapsed;

            home_MouseOver.Begin();
            #endregion
        }

        void MessengerBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //HtmlPage.Window.Navigate(new Uri("http://messenger.msn.com/Resource/games.aspx?appID=99995719"));
            HtmlPage.Window.Invoke("CheckP4Launch", _Appid);
        }

        
        void AuthenticationState_StateChange(object sender, EventArgs e)
        {
            string foo = "Bar";
        }

        void LiveIDLogin(object sender, MouseButtonEventArgs e)
        {
            if (LiveIdloggedIn == false)
            {
                HtmlPage.Window.Navigate(new Uri("http://login.live.com/wlogin.srf?appid=" + _Appid + "&alg=wsignin1.0"));
            }
            else
            {
                HtmlPage.Window.Navigate(new Uri("http://login.live.com/logout.srf?appid=" + _Appid + ""));
            }
        }

        void returnBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            returnOut.Begin();
        }

        void returnBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            returnOver.Begin();
        }

        void purchaseBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            purchaseOut.Begin();
        }

        void purchaseBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            purchaseOver.Begin();
        }

        void checkoutBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            checkOutOut.Begin();
        }

        void checkoutBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            checkOutOver.Begin();
        }

        void editCartBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            editCartOut.Begin();
        }

        void editCartBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            editCartOver.Begin();
        }

        void cartDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void cartUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void returnBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CurrentPage = "cart";
            cartBtn_MouseLeave(this, e);
        }

        void checkoutBtn_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            confirmation.Visibility = Visibility.Visible;
            CurrentPage = "cartConfirmation";
            checkout_FadeOut.Begin();
            shoppingCart_FadeOut.Begin();
            confirm_FadeIn.Begin();
        }

        void VideoWall_MediaOpened(object sender, RoutedEventArgs e)
        {
            MediaElement video = sender as MediaElement;
            VideoFadeIn.Begin();
            VideoWall.Play();
        }
        void loadVideoGrid()
        {
            // Add Video Grid

            Videos = new VideoGrid(_StaticAssetURL,_VideoPath);
            Videos.VideoPlayStart += new EventHandler(Videos_VideoPlayStart);
            Videos.VideoPlayStop += new EventHandler(Videos_VideoPlayStop);
            Videos.IndexChanged += new EventHandler(Videos_Item_IndexChanged);
            Videos.Loaded += new RoutedEventHandler(OnVideoGridLoaded);
            Videos.XMLLoaded += new VideoGrid.OnXMLLoaded(Videos_XMLLoaded);
            gridCanvas.Children.Add(Videos);
        }

        void Videos_XMLLoaded()
        {
            VideoWall.Source = RetailApi.Instance.GetProductById(0, currentBrand).Video.Url;
        }
        void loadVideoDetailsGrid()
        {
            
            // Add video grid for detail page(s)
            DetailVideos = new VideoDetailsGrid();
            DetailVideos.VideoPlayStart += new EventHandler(DetailVideos_VideoPlayStart);
            DetailVideos.VideoPlayStop += new EventHandler(DetailVideos_VideoPlayStop);
            DetailVideos.IndexChanged += new EventHandler(DetailVideos_IndexChanged);
            DetailVideos.Loaded += new RoutedEventHandler(OnVideoDetailsGridLoaded);
            //DetailVideos.XMLLoaded += new VideoGrid.OnXMLLoaded(Videos_XMLLoaded);
            gridCanvas.Children.Add(DetailVideos);
        }
        void cartControl_removeClick(object sender, EventArgs e)
        {
            setTotals();
            foreach (CartItem cartItem in cartControl.CartList)
            {
                //cartItem.slideDown.Begin();
            }
        }
        void cartControl_updateAmountsUp(object sender, EventArgs e)
        {
            setTotals();
        }
        void cartControl_updateAmountsDown(object sender, EventArgs e)
        {
            setTotals();
        }

        void cartControl_updateQuantityDown(object sender, EventArgs e)
        {
            setTotals();
        }

        void cartControl_updateQuantityUp(object sender, EventArgs e)
        {
            setTotals();
        }


        void purchaseBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            purchase.Text = "In Cart";
            purchaseShadow.Text = "In Cart";
            cartControl.addItemToCart(DetailVideos.SelectedItem);
            subtotalAmount += RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).Price * currQuantity;
            setTotals();
        }
        void setTotals()
        {
            cartItemsNumber.Text = cartControl.totalItemsInCart.ToString();
            shoppingInCart.Text = cartControl.totalItemsInCart.ToString();
            subtotalCart.Text = "$" + cartControl.subtotalAmount.ToString();
            checkoutSubtotal.Text = "$" + cartControl.subtotalAmount.ToString();
            checkoutItemsInCart.Text = cartControl.totalItemsInCart.ToString();
        }

        # region MouseEvents
        void pdhLogo_MouseLeave(object sender, MouseEventArgs e)
        {
            pdhLogo1.Visibility = Visibility.Collapsed;
        }

        void cortefielLogo_MouseLeave(object sender, MouseEventArgs e)
        {
            cortefielLogo1.Visibility = Visibility.Collapsed;
        }

        void springfieldLogo_MouseLeave(object sender, MouseEventArgs e)
        {
            springfieldLogo1.Visibility = Visibility.Collapsed;
        }

        void secretLogo_MouseLeave(object sender, MouseEventArgs e)
        {
            secretLogo1.Visibility = Visibility.Collapsed;
        }

        void milanoLogo_MouseLeave(object sender, MouseEventArgs e)
        {
            milanoLogo1.Visibility = Visibility.Collapsed;
        }

        void cortefielLogo_MouseEnter(object sender, MouseEventArgs e)
        {
            cortefielLogo1.Visibility = Visibility.Visible;
        }

        void springfieldLogo_MouseEnter(object sender, MouseEventArgs e)
        {
            springfieldLogo1.Visibility = Visibility.Visible;
        }

        void secretLogo_MouseEnter(object sender, MouseEventArgs e)
        {
            secretLogo1.Visibility = Visibility.Visible;
        }

        void milanoLogo_MouseEnter(object sender, MouseEventArgs e)
        {
            milanoLogo1.Visibility = Visibility.Visible;
        }

        void pdhLogo_MouseEnter(object sender, MouseEventArgs e)
        {

            pdhLogo1.Visibility = Visibility.Visible;
        }

        void cortefielLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentCatalog = "xmlFiles/cortefiel.xml";
            currentBrand = Brand.Cortefiel.ToString();
            CatalogClick(sender, e);
        }

        void springfieldLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentCatalog = "xmlFiles/springfield.xml";
            currentBrand = Brand.Springfield.ToString();
            CatalogClick(sender, e);
        }

        void secretLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentCatalog = "xmlFiles/secret.xml";
            currentBrand = Brand.WomanSecret.ToString();
            CatalogClick(sender, e);
        }

        void milanoLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentCatalog = "xmlFiles/milano.xml";
            currentBrand = Brand.Milano.ToString();
            CatalogClick(sender, e);
        }

        void pdhLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentCatalog = "xmlFiles/pdh.xml";
            currentBrand = Brand.PdH.ToString();
            CatalogClick(sender, e);
        }
        # endregion

        protected void CatalogClick(object sender, MouseButtonEventArgs e)
        {
            altPage = "catalog";
            CurrentPage = "catalog";
            VideoWall.Source = RetailApi.Instance.GetProductById(0, currentBrand).Video.Url;
            VideoWall.Visibility = Visibility.Collapsed;
            VideoWall.Stop();
            DetailVideos.Visibility = Visibility.Collapsed;
            Videos.Visibility = Visibility.Collapsed;
            Videos_Item_IndexChanged(sender, e);
            Videos.UpdateVideos(RetailApi.Instance.ProductsByBrand[currentBrand]);
            DetailVideos.UpdateVideos(RetailApi.Instance.ProductsByBrand[currentBrand]);
            catalogBtn_MouseLeave(sender, e);
            itemQuantity.Text = "1";
            itemQuantityShadow.Text = "1";
            currQuantity = 1;
            account_MouseOut.Begin();
            cart_MouseOut.Begin();
            home_MouseOut.Begin();
            login_MouseOut.Begin();
            logout_MouseOut.Begin();
        }

        void OnVideoGridLoaded(object sender, RoutedEventArgs e)
        {
            Videos.LoadData(currentCatalog);
        }

        void OnVideoDetailsGridLoaded(object sender, RoutedEventArgs e)
        {
            DetailVideos.LoadData(currentCatalog);
        }

        void DetailVideos_IndexChanged(object sender, EventArgs e)
        {
            detailsInfo.Visibility = Visibility.Visible;
            VideoDetails.Visibility = Visibility.Visible;
            VideoDetails.Source = RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).Video.Url;
            itemName.Text = RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).Description.ToString();
            if (itemName.Text.Length > 40)
            {
                itemName.Text = itemName.Text.Substring(0, 40) + "...";
            }
            currSize = 0;
            currSizeString = "";
            itemSize.Text = currSizeString;
            itemSizeShadow.Text = currSizeString;
            itemPrice.Text = "$" + RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).Price.ToString();
            //itemImageDetail.Source = RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs[0].Url;
            //itemImageDetail.SetValue(Image.SourceProperty, RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs[0].Url);
            itemImageDetail.Source = new BitmapImage(RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs[0].Url);

            thumbSource = 0;
            currentThumb.Text = (thumbSource + 1).ToString();
            maxThumb.Text = RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs.Count.ToString();
            itemQuantity.Text = "1";
            itemQuantityShadow.Text = "1";
            currQuantity = 1;
            updateSize(this, e);
            purchase.Text = "purchase";
            purchaseShadow.Text = "purchase";
        }

        void Videos_Item_IndexChanged(object sender, EventArgs e)
        {

            //Videos.Clear();
            //loadVideoGrid();
            //loadVideoDetailsGrid();

            Videos.DeselectAll();
            DetailVideos.DeselectAll();
            if (((FrameworkElement)sender).Name == "pdhLogo" || ((FrameworkElement)sender).Name == "milanoLogo" || ((FrameworkElement)sender).Name == "secretLogo" || ((FrameworkElement)sender).Name == "springfieldLogo" || ((FrameworkElement)sender).Name == "cortefielLogo")
            {
                DetailVideos.SelectedItem = Convert.ToInt32(RetailApi.Instance.GetProductById(Videos.SelectedItem, currentBrand).Id);
            }
            else
            {
                DetailVideos.SelectedItem = Convert.ToInt32(RetailApi.Instance.GetProductById(Videos.SelectedItem, currentBrand).Id) - 2;
            }
            selectedItemDetails.Visibility = Visibility.Visible;
            detailsInfo.Visibility = Visibility.Visible;
            VideoDetails.Visibility = Visibility.Visible;
            itemPrice.Text = "$" + RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).Price.ToString();
            // itemImageDetail.SetValue(Image.SourceProperty, RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs[0].Url);
            //itemImageDetail.SetValue(Image.SourceProperty, new Uri(RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs[0].Url.ToString()));
            itemImageDetail.Source = new BitmapImage(new Uri(RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs[0].Url.ToString(), UriKind.RelativeOrAbsolute));

            thumbSource = 0;
            currentThumb.Text = (thumbSource + 1).ToString();
            maxThumb.Text = RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs.Count.ToString();
            currSize = 0;
            currSizeString = "";
            itemSize.Text = currSizeString;
            itemSizeShadow.Text = currSizeString;
            // todo - determine: is the following line broken?
            // DetailVideos.LoadData(currentCatalog);
            Videos.Visibility = Visibility.Collapsed;
            DetailVideos.Visibility = Visibility.Visible;
            DetailsImage.Visibility = Visibility.Collapsed;
            VideoDetails.Source = RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).Video.Url;
            itemName.Text = RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).Description.ToString();
            if (itemName.Text.Length > 25)
            {
                itemName.Text = itemName.Text.Substring(0, 25) + "...";
            }
            DetailVideos.UpdateVideos(RetailApi.Instance.ProductsByBrand[currentBrand]);
            VideoWall.Stop();
            VideoWall.Visibility = Visibility.Collapsed;
            reflectionVideoLarge.Visibility = Visibility.Collapsed;
            CurrentPage = "catalog";
            altPage = "catalog";
            itemQuantity.Text = "1";
            itemQuantityShadow.Text = "1";
            currQuantity = 1;
            updateSize(this, e);
            purchase.Text = "purchase";
            purchaseShadow.Text = "purchase";
            
        }
        void onMouseEnter(object sender, EventArgs e)
        {
            ((FrameworkElement)sender).Opacity = 1;
        }
        void onMouseLeave(object sender, EventArgs e)
        {
            ((FrameworkElement)sender).Opacity = .5;
        }
        void updateQuantity(object sender, EventArgs e)
        {
            if (((FrameworkElement)sender).Name == "quantityUp")
            {
                currQuantity = currQuantity + 1;
                itemQuantity.Text = currQuantity.ToString();
                itemQuantityShadow.Text = currQuantity.ToString();
            }
            else
            {
                if (currQuantity > 0)
                {
                    currQuantity = currQuantity - 1;
                    itemQuantity.Text = currQuantity.ToString();
                    itemQuantityShadow.Text = currQuantity.ToString();
                }
                else
                {
                    return;
                }

            }
        }
        void updateSize(object sender, EventArgs e)
        {
            string size = RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).Size.ToString();
            string[] sizeArray = size.Split('|');

            
            itemSize.Text = sizeArray[0];
            currSizeString = sizeArray[0];
            for (int i = 0; i < sizeArray.Length; i++)
            {
                string item = sizeArray[i];
            }
            if (((FrameworkElement)sender).Name == "sizeUp")
            {
                if (currSize < sizeArray.Length - 1)
                {
                    currSize = currSize + 1;
                    itemSize.Text = sizeArray[currSize];
                    currSizeString = sizeArray[currSize];
                }
            }
            else
            {
                if (currSize > 0)
                {
                    currSize = currSize - 1;
                    itemSize.Text = sizeArray[currSize];
                    currSizeString = sizeArray[currSize];
                }
            }
        }
        void NextThumb(object sender, EventArgs e)
        {
            if (thumbSource < RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs.Count - 1)
            {
                //itemImageDetail.SetValue(Image.SourceProperty, RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs[thumbSource+1].Url);
                itemImageDetail.Source = new BitmapImage(RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs[thumbSource + 1].Url);
                thumbSource++;
                currentThumb.Text = (thumbSource + 1).ToString();
                debug.Text = RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).Color.ToString();
            }
        }
        void PrevThumb(object sender, EventArgs e)
        {
            if (thumbSource > 0)
            {
                debug.Text = thumbSource.ToString();
                //itemImageDetail.SetValue(Image.SourceProperty, RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs[thumbSource-1].Url);
                itemImageDetail.Source = new BitmapImage(RetailApi.Instance.GetProductById(DetailVideos.SelectedItem, currentBrand).MoreInfoThumbs[thumbSource - 1].Url);

                thumbSource--;
                currentThumb.Text = (thumbSource + 1).ToString();
                debug.Text = thumbSource.ToString();
            }
        }

        void DetailVideos_VideoPlayStop(object sender, EventArgs e)
        {
            videoFade_Out.Begin();
        }
        void DetailVideos_VideoPlayStart(object sender, EventArgs e)
        {
            videoFade_In.Begin();
        }

        


        void Instance_Error(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void Instance_Loaded(object sender, EventArgs e)
        {
            /*
            RetailApi baz = RetailApi.Instance;
            string bar = "foo";
            for (int i = 0; i < RetailApi.Instance.Count; i++)
            {
                Product product = RetailApi.Instance.GetProductById(i);
                string foo = product.Id;
            }
             */
            // debug.Text = RetailApi.Instance.Count.ToString();
        }


        void searchCloseBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            advancedSearchDropdown_Up.Begin();
            searchCloseBtn.Visibility = Visibility.Collapsed;
        }

        void advSearchBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            advancedSearchDropdown.Begin();
            searchCloseBtn.Visibility = Visibility.Visible;
        }

        void Videos_VideoPlayStop(object sender, EventArgs e)
        {
            videoFade_Out.Begin();
        }
        void selectedItemDetails_MouseLeave(object sender, EventArgs e)
        {
            selectedItemDetail_MouseOut.Begin();
        }
        void Videos_VideoPlayStart(object sender, EventArgs e)
        {
            videoFade_In.Begin();

        }
        void selectedItemDetails_MouseEnter(object sender, MouseEventArgs e)
        {
            selectedItemDetail_MouseOver.Begin();
        }
        // Global Navigation Events
        void homeBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            catalog_MouseOut.Begin();
            account_MouseOut.Begin();
            cart_MouseOut.Begin();
            login_MouseOut.Begin();
            logout_MouseOut.Begin();
            DetailVideos.Visibility = Visibility.Collapsed;
            Videos.Visibility = Visibility.Visible;
            CurrentPage = "home";
            altPage = "home";
            selectedItemDetails.Visibility = Visibility.Collapsed;
            VideoWall.Visibility = Visibility.Visible;
            VideoWall.Play();
            reflectionVideoLarge.Visibility = Visibility.Visible;
        }
        /*
        private object Canvas(DependencyObject dependencyObject)
        {
            //throw new NotImplementedException();
        }
        */
        void homeBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            if (CurrentPage == "home")
            {
                
            }
            else
            {
                home_MouseOut.Begin();
            }
        }

        void homeBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            home_MouseOver.Begin();
            largeSlideOut_Out.Begin();
            slideOutState = "home";
        }

        void catalogBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            if (CurrentPage == "catalog")
            {
                
            }
            else
            {
                catalog_MouseOut.Begin();
            }
            timerSB.Begin();
            companyLogos_FadeOut.Begin();
            accountBtn.IsHitTestVisible = true;
            cartBtn.IsHitTestVisible = true;
            loginBtn.IsHitTestVisible = true;
            catalogBtn.IsHitTestVisible = true;
            companyLogos.Visibility = Visibility.Collapsed;
        }

        void catalogBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            timerSB.Stop();
            companyLogos_FadeIn.Begin();
            catalog_MouseOver.Begin();
            smallSlideOut_Over.Begin();
            largeSlideOut_Out.Begin();
            slideOutState = "catalog";
            hover_catalog.Visibility = Visibility.Visible;
            hover_account.Visibility = Visibility.Collapsed;
            hover_cart.Visibility = Visibility.Collapsed;
            hover_login.Visibility = Visibility.Collapsed;
            companyLogos.Visibility = Visibility.Visible;
            accountBtn.IsHitTestVisible = false;
            cartBtn.IsHitTestVisible = false;
            loginBtn.IsHitTestVisible = false;
        }

        void catalogBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectedItemDetails.Visibility = Visibility.Visible;
            foreach (videoItem Item in Videos.VideoList)
            {
                Item.isThumbnailVisible = false;
            }
            CurrentPage = "catalog";
            account_MouseOut.Begin();
            cart_MouseOut.Begin();
            home_MouseOut.Begin();
            login_MouseOut.Begin();
            logout_MouseOut.Begin();
        }


        void accountBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            
            if (CurrentPage == "account")
            {

            }
            else
            {
                account_MouseOut.Begin();
            }
            timerSB.Begin();
            accountInfo_FadeOut.Begin();
            accountBtn.IsHitTestVisible = true;
            cartBtn.IsHitTestVisible = true;
            loginBtn.IsHitTestVisible = true;
            catalogBtn.IsHitTestVisible = true;
            accountInfo.Visibility = Visibility.Collapsed;
            login_FadeOut.Begin();
        }

        void accountBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (LiveIdloggedIn)
            {
                
                
                accountInfo_FadeIn.Begin();
                largeSlideOut_Out.Begin();
                
                slideOutState = "account";
                accountInfo.Visibility = Visibility.Visible;
                
                hover_cart.Visibility = Visibility.Collapsed;
                hover_login.Visibility = Visibility.Collapsed;
                hover_catalog.Visibility = Visibility.Collapsed;
                catalogBtn.IsHitTestVisible = false;
                cartBtn.IsHitTestVisible = false;
                loginBtn.IsHitTestVisible = false;
            }
            else
            {
                login_FadeIn.Begin();
            }
            timerSB.Stop();
            account_MouseOver.Begin();
            smallSlideOut_Over.Begin();
            hover_account.Visibility = Visibility.Visible;
            login.Visibility = Visibility.Visible;

        }

        void loginBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            
            logout_MouseOut.Begin();
            login_MouseOut.Begin();
            
            login_FadeOut.Begin();

            timerSB.Begin();
            accountBtn.IsHitTestVisible = true;
            cartBtn.IsHitTestVisible = true;
            loginBtn.IsHitTestVisible = true;
            catalogBtn.IsHitTestVisible = true;
            login.Visibility = Visibility.Collapsed;

        }

        void loginBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            timerSB.Stop();
            login_FadeIn.Begin();
            logout_MouseOver.Begin();
            login_MouseOver.Begin();
            smallSlideOut_Over.Begin();
            largeSlideOut_Out.Begin();
            slideOutState = "login";
            hover_login.Visibility = Visibility.Visible;
            hover_account.Visibility = Visibility.Collapsed;
            hover_cart.Visibility = Visibility.Collapsed;
            hover_catalog.Visibility = Visibility.Collapsed;
            login.Visibility = Visibility.Visible;
            accountBtn.IsHitTestVisible = false;
            cartBtn.IsHitTestVisible = false;
            catalogBtn.IsHitTestVisible = false;
              
        }

        

        void editCartBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            shoppingCart_FadeIn.Begin();
            checkout_FadeIn.Begin();
            largeSlideOut_In.Begin();
            smallSlideOut_Out.Begin();
            cartControl.arrangeLayout("checkout");
            shoppingCartText.Visibility = Visibility.Collapsed;
            shoppingCart.Visibility = Visibility.Visible;
            checkoutPage.Visibility = Visibility.Visible;
            confirmation.IsHitTestVisible = false;
            checkoutPage.IsHitTestVisible = true;
            checkoutOpen = true;
            setTotals();
            CurrentPage = "cartCheckout";
            catalog_MouseOut.Begin();
            account_MouseOut.Begin();
            home_MouseOut.Begin();
            login_MouseOut.Begin();
            logout_MouseOut.Begin();
        }

        void cartBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            if (CurrentPage != "cartCheckout")
            {
                if (altPage == "home")
                {
                    CurrentPage = "home";
                    home_MouseOver.Begin();
                    cart_MouseOut.Begin();
                }
                else
                {
                    CurrentPage = "catalog";
                    catalog_MouseOver.Begin();
                    cart_MouseOut.Begin();
                }
                //largeSlideOut_Out.Begin();
                timerSB.Begin();
                shoppingCart_FadeOut.Begin();

                confirm_FadeOut.Begin();
                homeBtn.IsHitTestVisible = true;
                accountBtn.IsHitTestVisible = true;
                cartBtn.IsHitTestVisible = true;
                loginBtn.IsHitTestVisible = true;
                catalogBtn.IsHitTestVisible = true;
                shoppingCart.Visibility = Visibility.Collapsed;
                checkoutPage.Visibility = Visibility.Collapsed;
                confirmation.Visibility = Visibility.Collapsed;
            }
            else
            {
                
            }

        }

        void cartBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            timerSB.Stop();

            foreach (CartItem cartItem in cartControl.CartList)
            {
                cartItem.Visibility = Visibility.Visible;
                cartItem.Opacity = 1;
            }

            if (checkoutOpen == false)
            {
                shoppingCart.Visibility = Visibility.Visible;
                shoppingCartText.Visibility = Visibility.Visible;
                checkoutPage.IsHitTestVisible = false;
                cart_MouseOver.Begin();
                smallSlideOut_Over.Begin();
                //largeSlideOut_Out.Begin();
                shoppingCart_FadeIn.Begin();
                cartControl.arrangeLayout("flyout");
            }
            else
            {
                cart_MouseOver.Begin();
                largeSlideOut_In.Begin();
                checkout_FadeIn.Begin();
                shoppingCart_FadeIn.Begin();
                shoppingCart.Visibility = Visibility.Visible;
                checkoutPage.Visibility = Visibility.Visible;
                //confirmation.IsHitTestVisible = true;
                checkoutPage.IsHitTestVisible = true;
                //cartControl.arrangeLayout("checkout");
            }
            slideOutState = "cart";
            hover_cart.Visibility = Visibility.Visible;
            hover_account.Visibility = Visibility.Collapsed;
            hover_login.Visibility = Visibility.Collapsed;
            hover_catalog.Visibility = Visibility.Collapsed;
            accountBtn.IsHitTestVisible = false;
            catalogBtn.IsHitTestVisible = false;
            loginBtn.IsHitTestVisible = false;
            shoppingCart.IsHitTestVisible = true;
            homeBtn.IsHitTestVisible = false;

            setTotals();
        }


        void onMediaEnded(object sender, EventArgs e)
        {
            VideoFadeOut.Begin();
            MediaElement video = (MediaElement)sender;
            video.Position = TimeSpan.FromMilliseconds(0);
            if (videoSource < RetailApi.Instance.ProductsByBrand[currentBrand].Count - 1)
            {
                videoSource++;
            }
            else
            {
                currentBrand = GetNextBrand(currentBrand);
                videoSource = 0;
            }

            video.Source = RetailApi.Instance.GetProductById(videoSource, currentBrand).Video.Url;
            video.Play();
            Videos.UpdateVideos(RetailApi.Instance.ProductsByBrand[currentBrand]);
        }
        public string GetNextBrand(string p_currentBrand)
        {
            Brand result = Brand.Cortefiel;
            if (p_currentBrand.ToLower() == Brand.Cortefiel.ToString().ToLower())
            {
                result = Brand.Milano;
            }
            else if (p_currentBrand.ToLower() == Brand.Milano.ToString().ToLower())
            {
                result = Brand.PdH;
            }
            else if (p_currentBrand.ToLower() == Brand.PdH.ToString().ToLower())
            {
                result = Brand.Springfield;
            }
            else if (p_currentBrand.ToLower() == Brand.Springfield.ToString().ToLower())
            {
                result = Brand.WomanSecret;
            }
            else if (p_currentBrand.ToLower() == Brand.WomanSecret.ToString().ToLower())
            {
                result = Brand.Cortefiel;
            }
            else
            {
                throw new Exception("Invalid Brand Passed to Page.GetNextBrand", new Exception(p_currentBrand + " is not a valid brand"));
            }

            // refresh videolist aqui

            return result.ToString();
        }
        void onMediaEndedSmall(object sender, EventArgs e)
        {
            //VideoFadeOut.Begin();
            MediaElement video = (MediaElement)sender;
            video.Stop();
            video.Position = new TimeSpan(0);
            video.Play();
        }

        void timerSB_Completed(object sender, EventArgs e)
        {
            
            largeSlideOut_Out.Begin();
            
            smallSlideOut_Out.Begin();
            
            hover_account.Visibility = Visibility.Collapsed;
            hover_cart.Visibility = Visibility.Collapsed;
            hover_login.Visibility = Visibility.Collapsed;
            hover_catalog.Visibility = Visibility.Collapsed;
            checkoutOpen = false;
            checkout_FadeOut.Begin();
            confirm_FadeOut.Begin();
            cartControl.arrangeLayout("flyout");
            
        }
        protected void api_Loaded(object sender, EventArgs e)
        {
            Dictionary<string, List<Product>> allProducts = RetailApi.Instance.ProductsByBrand;
            List<Product> brandProducts;

            foreach (KeyValuePair<string, List<Product>> keyValuePair in allProducts)
            {
                brandProducts = allProducts[keyValuePair.Key];
                string monkey = keyValuePair.Key;
                for (int i = 0; i < brandProducts.Count; i++)
                {
                    Product p = brandProducts[i];
                    string foo = p.Id;
                }
            }
        }
    }
}
