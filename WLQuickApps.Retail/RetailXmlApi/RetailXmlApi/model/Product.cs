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

namespace RetailXmlApi.model
{
    public class Product
    {
        public AssetData VideoThumb;
        public AssetData Video;
        public AssetData VideoSmall;
        public List<AssetData> MoreInfoThumbs = new List<AssetData>();
        public LocaleString Size;
        public LocaleString Description;
        public LocaleString Composition;
        public string Id;
        public double Price;
        public LocaleString Color;
        public GenderType Gender;
    }
    public enum GenderType
    {
        MALE,
        FEMALE
    }
}
