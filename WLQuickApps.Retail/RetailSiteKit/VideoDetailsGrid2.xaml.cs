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
using System.Windows.Threading;

namespace RetailSiteKit
{
	public partial class VideoDetailsGrid2 : UserControl
	{
        protected int invocationCount = 0;
        protected TextBlock debug;
        protected Storyboard Timer;
        protected Dictionary<int, int> iterationMap;

        public event EventHandler VideoPlayStart;
        public event EventHandler VideoPlayStop;
        public event EventHandler IndexChanged;
        //public event EventHandler XMLLoaded;

        public delegate void OnXMLLoaded();
        public event OnXMLLoaded XMLLoaded;
        public List<videoItem> VideoList;
        public int SelectedItem = 0;
        protected videoItem _SelectedItem;

        protected DispatcherTimer dt;

		public VideoDetailsGrid2()
		{
			// Required to initialize variables
			InitializeComponent();

            RetailApi.Instance.Error += new EventHandler(Instance_Error);
            RetailApi.Instance.Loaded += new EventHandler(Instance_Loaded);

            CreateIterationMap();

            dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 100); // 100 Milliseconds
            dt.Tick += new EventHandler(dt_Tick);
            //dt.Start();


            //Timer = (Storyboard)XamlReader.Load("<Storyboard xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"TimerClick\" Duration=\"00:00:00.1\" />");
            //Add a completed delegate to run code after the storyboard has completed
            //Timer.Completed += new EventHandler(Timer_Completed);
            //Add the storyboard to the resources of the page
            //this.Resources.Add(Timer);
		}
        protected void CreateIterationMap()
        {
            iterationMap = new Dictionary<int, int>();
            iterationMap[0] = 3;
            iterationMap[1] = 2;
            iterationMap[2] = 1;
            iterationMap[3] = 0;
            iterationMap[4] = 4;
            iterationMap[5] = 8;
            iterationMap[6] = 12;
            iterationMap[7] = 13;
            iterationMap[8] = 14;
            iterationMap[9] = 15;
            iterationMap[10] = 11;
            iterationMap[11] = 7;
        }
        public void UpdateVideos(List<Product> productList)
        {
            // update the video item's info about the video
            for (int j = 0; j < productList.Count; j++)
            {
                GetVideo(j).isThumbnailVisible = false;
                //GetVideo(iterationMap[j]).ItemIndex = iterationMap[j];
            }
            for (int i = 0; i < 12; i++)
            {
                GetVideo(iterationMap[i]).product = productList[i];
            }
            // show the first image
            GetVideo(iterationMap[0]).isThumbnailVisible = true;
            // start iteration chain
            //CreateTimer();
            dt.Start();
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
                LayoutDetails.Children.Add(Item);
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
        protected void VideoDetailsGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }
        void dt_Tick(object sender, EventArgs e)
        {
            invocationCount++;
            if (invocationCount < 12)
            {
                GetVideo(iterationMap[invocationCount]).isThumbnailVisible = true;
                //CreateTimer();
            }
            else
            {
                //todo - ResetThumbs(); // (hide all thumbs)
                // reset the invocation count to the position of the first image we want to show in our iteration chain.
                invocationCount = 0;
            }
        }
        /*
        protected void CreateTimer()
        {
            
                try
                {
                    Timer.Begin();
                }
                catch (NullReferenceException ex)
                {
                    
                    //Timer.Begin();
                }
            
        }
        

        protected void  Timer_Completed(object sender, EventArgs e)
        {
            invocationCount++;
            if (invocationCount < 12)
            {
                GetVideo(iterationMap[invocationCount]).isThumbnailVisible = true;
                CreateTimer();
            }
            else
            {
                //todo - ResetThumbs(); // (hide all thumbs)
                // reset the invocation count to the position of the first image we want to show in our iteration chain.
                invocationCount = 0;
            }
        }
        */
        protected void Item_VideoPlayStop(object sender, EventArgs e)
        {
            VideoPlayStop(this, e);
        }

        protected void Item_VideoPlayStart(object sender, EventArgs e)
        {
            VideoPlayStart(this, e);
        }
        protected void Item_IndexChanged(object sender, EventArgs e)
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
            SelectedItem = ((videoItem)sender).ItemIndex - 1;
            IndexChanged(this, e);
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
                    LayoutDetails.Children.Remove(vid);
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