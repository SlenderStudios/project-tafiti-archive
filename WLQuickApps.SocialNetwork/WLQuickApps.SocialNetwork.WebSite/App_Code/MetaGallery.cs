using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WLQuickApps.SocialNetwork.Business;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WLQuickApps.SocialNetwork.WebSite
{
    [ToolboxData(@"<{0}:MetaGallery runat=""server"" />")]
    public class MetaGallery : DataList
    {
        private bool _hasRows;
        private DataSourceSelectArguments _arguments;

        #region Public properties

        [Browsable(true)]
        [Category("Paging")]
        [DefaultValue(false)]
        public virtual bool AllowPaging
        {
            get
            {
                bool? allowPaging = this.ViewState["AllowPaging"] as bool?;
                return allowPaging.GetValueOrDefault(false);
            }
            set
            {
                bool allowPaging = this.AllowPaging;
                if (value != allowPaging)
                {
                    this.ViewState["AllowPaging"] = value;
                    if (base.Initialized)
                    {
                        base.RequiresDataBinding = true;
                    }
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int PageCount
        {
            get
            {
                if (this._pageCount >= 0)
                {
                    return this._pageCount;
                }

                int? pageCount = this.ViewState["PageCount"] as int?;
                return pageCount.GetValueOrDefault(0);
            }
        }
        private int _pageCount;

        [Browsable(true)]
        [Category("Paging")]
        [DefaultValue(0)]
        public virtual int PageIndex
        {
            get { return this._pageIndex; }
            set
            {
                this._pageIndex = value;
                if (base.Initialized)
                {
                    base.RequiresDataBinding = true;
                }
            }
        }
        private int _pageIndex;

        [Browsable(true)]
        [Category("Paging")]
        public virtual int PageSize
        {
            get
            {
                int? pageSize = this.ViewState["PageSize"] as int?;
                return pageSize.GetValueOrDefault(10);
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                if (this.PageSize != value)
                {
                    this.ViewState["PageSize"] = value;
                    if (base.Initialized)
                    {
                        base.RequiresDataBinding = true;
                    }
                }
            }
        }

        [Browsable(true)]
        [Bindable(true)]
        public string EmptyDataText
        {
            get
            {
                string s = this.ViewState[WebConstants.ViewStateVariables.EmptyDataText] as string;
                if (s == null)
                {
                    return string.Empty;
                }
                else
                {
                    return s;
                }
            }
            set
            {
                this.ViewState[WebConstants.ViewStateVariables.EmptyDataText] = value;
            }
        }

        [Browsable(true)]
        [Themeable(true)]
        public GalleryViewMode ViewMode
        {
            get
            {
                GalleryViewMode? viewMode = this.ViewState[WebConstants.ViewStateVariables.ViewMode] as GalleryViewMode?;
                return viewMode.GetValueOrDefault(GalleryViewMode.Square);
            }
            set
            {
                this.ViewState[WebConstants.ViewStateVariables.ViewMode] = value;
            }
        }

        new private TableItemStyle AlternatingItemStyle;
        new private ITemplate AlternatingItemTemplate;
        new private int EditItemIndex;
        new private TableItemStyle EditItemStyle;
        new private ITemplate EditItemTemplate;
        new private bool ExtractTemplateRows;
        new private ITemplate SelectedItemTemplate;

        #endregion

        public MetaGallery()
        {
            this.RepeatColumns = 3;
            this.RepeatDirection = RepeatDirection.Horizontal;
        }

        #region Templates

        protected override void CreateControlHierarchy(bool useDataSource)
        {
            this.HeaderTemplate = new CompiledTemplateBuilder(this.BuildHeader);
            this.ItemTemplate = new CompiledTemplateBuilder(this.BuildItem);
            this.FooterTemplate = new CompiledTemplateBuilder(this.BuildPager);

            base.CreateControlHierarchy(true);
        }

        private void BuildHeader(Control container)
        {
            if (container == null) { throw new ArgumentNullException("container"); }

            if (!this._hasRows)
            {
                Label emptyDataLabel = new Label();
                emptyDataLabel.ID = "_emptyDataLabel";
                emptyDataLabel.Text = this.EmptyDataText;

                container.Controls.Add(emptyDataLabel);
            }
        }

        private void BuildItem(Control container)
        {
            Control metaGalleryElement = this.Page.LoadControl("~/Controls/MetaGalleryElement.ascx");

            container.Controls.Add(metaGalleryElement);

            ((IMetaGalleryElement)metaGalleryElement).ViewMode = this.ViewMode;
            metaGalleryElement.ID = "_metaGalleryElement";
            metaGalleryElement.DataBinding += new EventHandler(metaGalleryElement_DataBinding);
        }
        protected void metaGalleryElement_DataBinding(object sender, EventArgs e)
        {
            Control metaGalleryElement = sender as Control;
            ((IMetaGalleryElement)metaGalleryElement).DataItem = DataBinder.GetDataItem(metaGalleryElement.NamingContainer);
        }

        private void BuildPager(Control container)
        {
            if (this._hasRows && (this.PageCount > 1))
            {
                if ((this.PageIndex < this.PageCount) && (this.Items.Count < this.PageSize))
                {
                    this._pageCount = this.PageIndex + 1;
                }

                Panel pagerPanel = new Panel();
                pagerPanel.ID = "_pagerPanel";
                pagerPanel.CssClass = "pager";

                Label pagerCaption = new Label();
                pagerCaption.Text = "Page&nbsp;";
                pagerPanel.Controls.Add(pagerCaption);

                if (this.PageCount < 12)
                {
                    for (int i = 0; i < this.PageCount; i++)
                    {
                        pagerPanel.Controls.Add(this.GenerateNavigationButton(i));
                    }
                }
                else
                {
                    int start = this.PageIndex - 5;
                    int end = this.PageIndex + 5;
                    if (start < 0)
                    {
                        end -= start;
                        start = 0;
                    }
                    if (end >= this.PageCount)
                    {
                        start -= (end - this.PageCount + 1);
                        end = this.PageCount - 1;
                    }

                    if (this.PageIndex > 0)
                    {
                        pagerPanel.Controls.Add(this.GenerateNavigationButton(WebConstants.PagingMovementCommands.First, "&#0171;"));
                    }
                    if (start > 0)
                    {
                        pagerPanel.Controls.Add(this.GenerateNavigationButton(WebConstants.PagingMovementCommands.Previous, "&lt;"));
                    }
                    for (int i = start; i <= end; i++)
                    {
                        pagerPanel.Controls.Add(this.GenerateNavigationButton(i));
                    }
                    if (end < (this.PageCount - 1))
                    {
                        pagerPanel.Controls.Add(this.GenerateNavigationButton(WebConstants.PagingMovementCommands.Next, "&gt;"));
                    }
                    if (end < this.PageCount)
                    {
                        pagerPanel.Controls.Add(this.GenerateNavigationButton(WebConstants.PagingMovementCommands.Last, "&#0187;"));
                    }
                }

                container.Controls.Add(pagerPanel);
            }
        }

        protected void pageLink_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case WebConstants.PagingMovementCommands.First:
                    this.PageIndex = 0;
                    break;
                case WebConstants.PagingMovementCommands.Previous:
                    this.PageIndex -= 5;
                    break;
                case WebConstants.PagingMovementCommands.Next:
                    this.PageIndex += 5;
                    break;
                case WebConstants.PagingMovementCommands.Last:
                    this.PageIndex = this.PageCount - 1;
                    break;
                default:
                    this.PageIndex = Int32.Parse(e.CommandName);
                    break;
            }

            this.CreateDataSourceSelectArguments();
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageIndex = ((DropDownList)sender).SelectedIndex;
            this.CreateDataSourceSelectArguments();
        }

        private LinkButton GenerateNavigationButton(string commandArgument, string text)
        {
            LinkButton lb = new LinkButton();
            lb.CommandName = commandArgument;
            lb.Command += new CommandEventHandler(pageLink_Command);
            lb.Text = text;

            return lb;
        }

        private LinkButton GenerateNavigationButton(int pageNumber)
        {
            LinkButton lb = this.GenerateNavigationButton(pageNumber.ToString(), (pageNumber + 1).ToString());
            lb.CssClass = string.Format("pager-link-{0}", (pageNumber == this.PageIndex));
            return lb;
        }

        #endregion

        #region Data

        protected override IEnumerable GetData()
        {
            IEnumerable data = base.GetData();

            if (this.AllowPaging)
            {
                int rowCount = base.SelectArguments.TotalRowCount;
                if (rowCount == -1)
                {
                    throw new InvalidOperationException("Can't page unless the data source supports paging.");
                }

                this._pageCount = (rowCount / this.PageSize);
                if (rowCount > (this._pageCount * this.PageSize))
                {
                    this._pageCount++;
                }
            }

            this._hasRows = ((data != null) && (data.GetEnumerator().MoveNext()));

            return data;
        }

        protected override DataSourceSelectArguments CreateDataSourceSelectArguments()
        {
            if (this.AllowPaging)
            {
                if (this._arguments == null)
                {
                    this._arguments = new DataSourceSelectArguments();
                    this.GetSessionData().TryGetValue(this.GetSessionKey(), out this._pageIndex);
                }

                this._arguments.StartRowIndex = this.PageSize * this.PageIndex;
                this._arguments.MaximumRows = this.PageSize;
                this._arguments.AddSupportedCapabilities(DataSourceCapabilities.RetrieveTotalRowCount);
                this._arguments.RetrieveTotalRowCount = true;

                return this._arguments;
            }
            else
            {
                return DataSourceSelectArguments.Empty;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            Dictionary<string, int> MetaGalleryDataSession = this.GetSessionData();
            if (MetaGalleryDataSession.ContainsKey(this.GetSessionKey()))
            {
                MetaGalleryDataSession.Remove(this.GetSessionKey());
            }
            MetaGalleryDataSession.Add(this.GetSessionKey(), this._pageIndex);
            this.SaveSessionData(MetaGalleryDataSession);

            base.OnPreRender(e);
        }

        #endregion

        #region Interfaces

        public interface IMetaGalleryElement
        {
            object DataItem
            {
                get;
                set;
            }

            GalleryViewMode ViewMode
            {
                get;
                set;
            }
        }

        #endregion

        #region Session

        private string GetSessionKey()
        {
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            MD5.ComputeHash(new UTF8Encoding().GetBytes(string.Format("{0}/{1}", this.Page.Request.Url.PathAndQuery, this.ClientID)));


            return BitConverter.ToString(MD5.Hash);
        }

        private Dictionary<string, int> GetSessionData()
        {
            Dictionary<string, int> MetaGalleryDataSession =
                HttpContext.Current.Session[WebConstants.SessionVariables.MetaGalleryData] as Dictionary<string, int>;
            if (MetaGalleryDataSession == null)
            {
                MetaGalleryDataSession = new Dictionary<string, int>();
            }
            return MetaGalleryDataSession;
        }

        private void SaveSessionData(Dictionary<string, int> MetaGalleryDataSession)
        {
            HttpContext.Current.Session[WebConstants.SessionVariables.MetaGalleryData] = MetaGalleryDataSession;
        }

        #endregion

        protected override bool OnBubbleEvent(object source, EventArgs e)
        {
            if (e is GalleryElementModifiedEventArgs)
            {
                // A MetaGallery element has handled an action and wants us to databind.
                this.RequiresDataBinding = true;
                //this.Parent.DataBind();
                this.NamingContainer.DataBind();
                return true;
            }

            return base.OnBubbleEvent(source, e);
        }
    }
}