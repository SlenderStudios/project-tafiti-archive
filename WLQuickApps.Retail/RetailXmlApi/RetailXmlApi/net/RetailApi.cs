using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using MetaliqSilverlightSDK.net;
using RetailXmlApi.model;


namespace RetailXmlApi.net
{
    public enum Brand
    {
        Cortefiel,
        Milano,
        PdH,
        Springfield,
        WomanSecret
    }
    public class RetailApi : XMLLoader
    {
        public string _VideoPath;
        public string _StaticAssetURL;
        private static RetailApi _Instance;
        private RetailApi()
        {
            string foo = "Bar";
            Application.Current.Startup += new StartupEventHandler(Current_Startup);
            App.Current.UnhandledException += new EventHandler<ApplicationUnhandledExceptionEventArgs>(Current_UnhandledException);
        }

        void Current_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void Current_Startup(object sender, StartupEventArgs e)
        {
            _VideoPath = "videos/";//e.InitParams["video"];
        }
        protected Dictionary<string, List<Product>> _ProductsByBrand = new Dictionary<string, List<Product>>();
        //public string videoPath = RetailSiteKit.App._VideoPath;
        protected List<Product> _ProductList;

        public static RetailApi Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new RetailApi();
                }
                return _Instance;
            }
        }
        public Dictionary<string, List<Product>> ProductsByBrand
        {
            get
            {
                return _ProductsByBrand;
            }
        }
        /*
        public List<Product> GetProductsByBrand(Brand brand)
        {
            return new List<Product>();
        }
        */
        public Product GetProductById(int Id, string brand)
        {
            return _ProductsByBrand[brand][Id];
        }
        public static Brand GetBrand(string str)
        {
            Brand result = Brand.Cortefiel;
            if (str.ToLower() == Brand.Cortefiel.ToString().ToLower())
            {
                result = Brand.Cortefiel;
            }
            else if (str.ToLower() == Brand.Milano.ToString().ToLower())
            {
                result = Brand.Milano;
            }
            else if (str.ToLower() == Brand.PdH.ToString().ToLower())
            {
                result = Brand.PdH;
            }
            else if (str.ToLower() == Brand.Springfield.ToString().ToLower())
            {
                result = Brand.Springfield;
            }
            else if (str.ToLower() == Brand.WomanSecret.ToString().ToLower())
            {
                result = Brand.WomanSecret;
            }
            else
            {
                throw new Exception("Invalid Brand Passed to RetailXmlApi.RetailApi.GetBrand", new Exception(str + " is not a valid brand"));
            }

            return result;
        }
        protected override void ParseXML(XmlReader Reader)
        {
            try
            {
                /*
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new Uri("http://contoso.com"));
                request.BeginGetResponse(new AsyncCallback(ResponseCallback), request);
                 
                ...
                 
                // AsyncCallback called back on UIThread
                private void ResponseCallback(IAsyncResult asyncResult)
                {
                    HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                 
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
                    Stream content = response.GetResponseStream();
 
                    using (XmlReader reader = XmlReader.Create(new StreamReader(content)))
                    {
                 * */

                //HttpWebResponse response = (HttpWebResponse)((HttpWebRequest)result.AsyncState).EndGetResponse(result);
                ////if (response.StatusCode != HttpStatusCode.OK)
                ////    throw new ApplicationException("HttpStatusCode " +
                ////      response.StatusCode.ToString() + " was returned.");
                //if (response.StatusCode != HttpStatusCode.OK)
                //    throw new Exception("HttpStatusCode " +
                //      response.StatusCode.ToString() + " was returned.");

                //StreamReader responseReader = new StreamReader(response.GetResponseStream());

                //string rawResponse = responseReader.ReadToEnd();

                //response.Close();
                //XmlReader Reader = XmlReader.Create(new StringReader(rawResponse));
                /*
                    _ProductsByBrand.Add(BrandReader.GetAttribute("id"), _ProductList);
                 */
                string brandId = "lkasdfa";
                while (Reader.Read())
                {
                    //*
                    if (Reader.LocalName.ToLower() == "brand")
                    {
                        _ProductList = new List<Product>();
                        brandId = Reader.GetAttribute("id");
                        _ProductsByBrand[brandId] = new List<Product>();
                        using (XmlReader BrandReader = Reader.ReadSubtree())
                        {
                            while (BrandReader.Read())
                            {
                                //*/
                                //XmlReader BrandReader = Reader;
                                if (BrandReader.LocalName.ToLower() == "product")
                                {
                                    #region PRODUCT
                                    Product product = new Product();
                                    product.Id = BrandReader.GetAttribute("id");
                                    product.Gender = BrandReader.GetAttribute("gender").ToLower() == "male" ? GenderType.MALE : GenderType.FEMALE;
                                    product.Price = Convert.ToDouble(BrandReader.GetAttribute("us_price"));
                                    //product.Color = BrandReader.GetAttribute("color");
                                    using (XmlReader ProductReader = BrandReader.ReadSubtree())
                                    {
                                        while (ProductReader.Read())
                                        {
                                            if (ProductReader.LocalName.ToLower() == "videothumb")
                                            {
                                                string videoThumbId = ProductReader.GetAttribute("id");
                                                string videoThumbUrl = ProductReader.GetAttribute("url");
                                                product.VideoThumb = new AssetData(videoThumbId, new Uri(this._StaticAssetURL+"/"+videoThumbUrl, UriKind.RelativeOrAbsolute));
                                            }
                                            if (ProductReader.LocalName.ToLower() == "video")
                                            {
                                                string videoId = ProductReader.GetAttribute("id");
                                                string videoUrl = ProductReader.GetAttribute("url");
                                                product.Video = new AssetData(videoId, new Uri(this._VideoPath + videoUrl, UriKind.RelativeOrAbsolute));
                                                string smallVideoPath = videoUrl.Substring(0, videoUrl.LastIndexOf(".")) + "a" + "." + videoUrl.Substring(videoUrl.LastIndexOf(".") + 1, videoUrl.Length - videoUrl.LastIndexOf(".") - 1);
                                                product.VideoSmall = new AssetData(videoId, new Uri(this._VideoPath + smallVideoPath, UriKind.RelativeOrAbsolute));
                                            }
                                            XmlReader LanguageReader;
                                            if (ProductReader.LocalName.ToLower() == "sizes")
                                            {
                                                using (LanguageReader = ProductReader.ReadSubtree())
                                                {
                                                    LocaleString size = new LocaleString();
                                                    while (LanguageReader.Read())
                                                    {
                                                        switch (LanguageReader.LocalName.ToLower())
                                                        {
                                                            // add more languages here
                                                            case "us_en":
                                                                size.AddTranslation(LanguageReader.ReadElementContentAsString(), Language.AMERICAN_ENGLISH);
                                                                product.Size = size;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (ProductReader.LocalName.ToLower() == "colors")
                                            {
                                                using (LanguageReader = ProductReader.ReadSubtree())
                                                {
                                                    LocaleString color = new LocaleString();
                                                    while (LanguageReader.Read())
                                                    {
                                                        switch (LanguageReader.LocalName.ToLower())
                                                        {
                                                            // add more languages here
                                                            case "us_en":
                                                                color.AddTranslation(LanguageReader.ReadElementContentAsString(), Language.AMERICAN_ENGLISH);
                                                                product.Color = color;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (ProductReader.LocalName.ToLower() == "descriptions")
                                            {
                                                using (LanguageReader = ProductReader.ReadSubtree())
                                                {
                                                    LocaleString description = new LocaleString();
                                                    while (LanguageReader.Read())
                                                    {
                                                        switch (LanguageReader.LocalName.ToLower())
                                                        {
                                                            // add more languages here
                                                            case "us_en":
                                                                description.AddTranslation(LanguageReader.ReadElementContentAsString(), Language.AMERICAN_ENGLISH);
                                                                product.Description = description;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (ProductReader.LocalName.ToLower() == "compositions")
                                            {
                                                using (LanguageReader = ProductReader.ReadSubtree())
                                                {
                                                    LocaleString composition = new LocaleString();
                                                    while (LanguageReader.Read())
                                                    {
                                                        switch (LanguageReader.LocalName.ToLower())
                                                        {
                                                            // add more languages here
                                                            case "us_en":
                                                                composition.AddTranslation(LanguageReader.ReadElementContentAsString(), Language.AMERICAN_ENGLISH);
                                                                product.Composition = composition;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (ProductReader.LocalName.ToLower() == "moreinfothumbs")
                                            {
                                                using (XmlReader MoreInfoThumbReader = ProductReader.ReadSubtree())
                                                {
                                                    while (MoreInfoThumbReader.Read())
                                                    {
                                                        if (MoreInfoThumbReader.LocalName.ToLower() == "image")
                                                        {
                                                            string thumbId = MoreInfoThumbReader.GetAttribute("id");
                                                            string thumbUrl = MoreInfoThumbReader.GetAttribute("url");
                                                            AssetData thumbnail = new AssetData(thumbId, new Uri(this._StaticAssetURL + "/" + thumbUrl, UriKind.RelativeOrAbsolute));
                                                            product.MoreInfoThumbs.Add(thumbnail);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    _ProductList.Add(product);
                                    _ProductsByBrand[brandId].Add(product);
                                    #endregion
                                }
                                //*
                            }
                        }
                    }
                    //*/
                    // end of brand loop
                }
            }
            catch (Exception e)
            {
                Exception ex = e;
            }
            int foogle = _ProductList.Count;
            int minorityReport = _ProductsByBrand.Count;
            string chunk = "chaz.";
        }
    }
}
