using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using RetailXmlApi.net;
using RetailXmlApi.model;
using System.Collections.Generic;

namespace RetailSiteKit
{
	public partial class VideoGrid2 : UserControl
	{
        public event EventHandler VideoPlayStart;
        public event EventHandler VideoPlayStop;
        public event EventHandler IndexChanged;
        //public event EventHandler XMLLoaded;

        public delegate void OnXMLLoaded();
        public event OnXMLLoaded XMLLoaded;
        public List<videoItem> VideoList;
        public int SelectedItem = 0;
        protected videoItem _SelectedItem;


		public VideoGrid2()
		{
			// Required to initialize variables
			InitializeComponent();

            RetailApi.Instance.Error += new EventHandler(Instance_Error);
            RetailApi.Instance.Loaded += new EventHandler(Instance_Loaded);
		}
        public virtual void UpdateVideos(List<Product> productList)
        {
            for (int i = 0; i < 16; i++)
            {
                GetVideo(i).product = productList[i];
            }
        }
        public virtual void LoadData(string url)
        {
            RetailApi.Instance.Load(url);
        }
        protected virtual void Instance_Error(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        protected virtual void Instance_Loaded(object sender, EventArgs e)
        {
            VideoList = new List<videoItem>();
            videoItem Item;
            int thisRow = 0;
            double ColumnOffset;

            Dictionary<string, List<Product>> allProducts = RetailApi.Instance.ProductsByBrand;
            List<Product> cortProducts = allProducts[Brand.Cortefiel.ToString()];



            for (int i = 0; i < cortProducts.Count; i++)
            {
                Product product = RetailApi.Instance.GetProductById(i, Brand.Cortefiel.ToString());
                ColumnOffset = i % 4;
                if (i % 4 == 0)
                {
                    thisRow++;
                }
                ColumnOffset = 1 + ColumnOffset;
                Item = new videoItem(i, product);
                Item.Row = thisRow;
                Item.Column = Convert.ToInt32(ColumnOffset);
                //Item.root.FindName("VideoDisplay").
                Item.VideoPlayStart += new EventHandler(Item_VideoPlayStart);
                Item.VideoPlayStop += new EventHandler(Item_VideoPlayStop);
                Item.IndexChanged += new EventHandler(Item_IndexChanged);
                VideoList.Add(Item);
                Layout.Children.Add(Item);
            }
            try
            {
                XMLLoaded();//this, new EventArgs());
            }
            catch (NullReferenceException ex)
            {
                string foo = "bar";
            }
        }

        protected virtual void Item_IndexChanged(object sender, EventArgs e)
        {
            videoItem oldItem = _SelectedItem;
            try
            {
                _SelectedItem = sender as videoItem;
                oldItem.Selected = false;
                oldItem.Stop();
            }
            catch (NullReferenceException nullEx)
            {
                string bar = "foo";
            }
            catch (Exception ex)
            {
                string foo = "Bar";
            }
            SelectedItem = ((videoItem)sender).ItemIndex;
            IndexChanged(this, e);

        }


        void Item_VideoPlayStop(object sender, EventArgs e)
        {
            VideoPlayStop(this, e);
        }

        void Item_VideoPlayStart(object sender, EventArgs e)
        {
            VideoPlayStart(this, e);
        }
        public videoItem GetVideo(int id)
        {
            return VideoList[id];
        }
        public videoItem GetVideo(int Row, int Column)
        {
            videoItem result = null;
            foreach (videoItem vid in VideoList)
            {
                if (vid.Row == Row && vid.Column == Column)
                {
                    result = vid;
                    break;
                }
            }
            return result;
        }
        public void Clear()
        {
            try
            {
                foreach (videoItem vid in VideoList)
                {
                    Layout.Children.Remove(vid);
                }
            }
            catch (NullReferenceException ex)
            {
            }
            VideoList.Clear();

        }
        public void DeselectAll()
        {
            if (VideoList != null)
            {
                foreach (videoItem vid in VideoList)
                {
                    if (vid.Selected)
                    {
                        vid.Selected = false;
                    }
                }
            }
        }
        new public virtual Visibility Visibility
        {
            get
            {
                return base.Visibility;
            }
            set
            {
                base.Visibility = value;
                if (value == Visibility.Collapsed)
                {
                    DeselectAll();
                }
            }
        }
	}
}