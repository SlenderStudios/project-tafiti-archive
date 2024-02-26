using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Browser;

using WLQuickApps.FieldManager.Silverlight.SiteService;
using System.Windows.Threading;

namespace WLQuickApps.FieldManager.Silverlight
{
    public partial class Page : UserControl
    {
        private ScriptObject _viewLeagueMethod;
        private ScriptObject _viewFieldMethod;
        private ScriptObject _viewDirectionsMethod;
        private ScriptObject _clearDirectionsMethod;
        private ScriptObject _viewTrafficMethod;
        private ScriptObject _viewContactsMethod;

        private Field _currentField;
        private League _currentLeague;

        private Canvas _currentTab;
        private Canvas _fieldTab;
        private StackPanel _currentFieldItem;

        DispatcherTimer _timer;

        private bool _isLoggedIn;

        public Page()
        {
            InitializeComponent();

            this._viewLeagueMethod = (ScriptObject)HtmlPage.Window.GetProperty("viewLeague");
            this._viewFieldMethod = (ScriptObject)HtmlPage.Window.GetProperty("viewField");
            this._viewDirectionsMethod = (ScriptObject)HtmlPage.Window.GetProperty("viewDirections");
            this._clearDirectionsMethod = (ScriptObject)HtmlPage.Window.GetProperty("clearDirections");
            this._viewTrafficMethod = (ScriptObject)HtmlPage.Window.GetProperty("viewTraffic");
            this._viewContactsMethod = (ScriptObject)HtmlPage.Window.GetProperty("viewContacts");

            SiteServiceClient client = this.CreateServiceClient();
            client.GetDisplayNameAsync();

            this._timer = new DispatcherTimer();
            this._timer.Interval = new TimeSpan(0, 0, 1);
            this._timer.Tick += new EventHandler(_timer_Tick);
            this._timer.Start();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlPage.RegisterScriptableObject("FieldManagerSilverlight", this);
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this._dayLabel.Text = now.ToShortDateString();
            this._timeLabel.Text = now.ToShortTimeString();
        }

        #region Populate New Data Methods
        private void OnLeagues(LeagueItem[] leagues)
        {
            List<League> lists = League.CreateFromLeagueList(leagues);
            if (this._isLoggedIn)
            {
                lists.Insert(0, League.CreateMyFieldsLeague());
            }

            this._leaguesListBox.ItemsSource = lists;

            if (leagues.Length > 0)
            {
                SiteServiceClient client = this.CreateServiceClient();
                client.GetFieldsForLeagueAsync(leagues[0].LeagueID);
                client.GetLeagueAsync(leagues[0].LeagueID);
                this._viewLeagueMethod.InvokeSelf(leagues[0].LeagueID);
                this._currentLeague = League.CreateFromLeague(leagues[0]);
                this._selectedLeagueLabel.Text = this._currentLeague.Title;
            }
            else if (this._isLoggedIn)
            {
                this.CreateServiceClient().GetMyFieldsAsync();
                this._selectedLeagueLabel.Text = "<My Fields>";
                this._leagueNameLabel.Text = "My";
            }
        }

        private void OnLeague(LeagueItem league)
        {
            this._currentLeague = League.CreateFromLeague(league);
            this._selectedLeagueLabel.Text = this._currentLeague.Title;
            this._leagueNameLabel.Text = this._currentLeague.Title;
        }

        private void OnFields(FieldItem[] fields)
        {
            this._fieldsList.ItemsSource = Field.CreateFromFieldList(fields);

            if (fields.Length > 0)
            {
                this.OnField(fields[0], false);
                this._emptyFieldsLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.LoadingFadeIn.Stop();
                this.LoadingFadeOut.Begin();
                this._emptyFieldsLabel.Visibility = Visibility.Visible;
            }
        }

        private void OnField(FieldItem fieldItem, bool pan)
        {
            this._currentField = Field.CreateFromField(fieldItem);

            this._mainFieldPanel.DataContext = this._currentField;
            this._weatherNameLabel.Text = this._currentField.Title;
            this._weatherList.ItemsSource = this._currentField.Forecast;

            this._viewFieldMethod.InvokeSelf(this._currentField.FieldID, this._currentField.Address, this._currentField.Latitude, this._currentField.Longitude, pan);
            this.LoadingFadeIn.Stop();
            this.LoadingFadeOut.Begin();
        }
        #endregion

        #region Scriptable Members
        [ScriptableMember]
        public void ViewField(int fieldID)
        {
            SiteServiceClient siteService = this.CreateServiceClient();
            siteService.GetFieldAsync(fieldID);
            this.Loading.Begin();
            this.LoadingFadeOut.Stop();
            this.LoadingFadeIn.Begin();
        }

        [ScriptableMember]
        public int GetCurrentLeagueID()
        {
            if (this._currentLeague == null) { return 0; }

            return Convert.ToInt32(this._currentLeague.LeagueID);
        }
        #endregion

        #region Async Request Handlers
        private SiteServiceClient CreateServiceClient()
        {
            SiteServiceClient siteService = new SiteServiceClient(
                new BasicHttpBinding(),
                new EndpointAddress(Utilities.AppUrlRoot + "/SiteService.svc/soap"));

            siteService.GetAllLeaguesCompleted += new EventHandler<GetAllLeaguesCompletedEventArgs>(siteService_GetAllLeaguesCompleted);
            siteService.GetDisplayNameCompleted += new EventHandler<GetDisplayNameCompletedEventArgs>(siteService_GetDisplayNameCompleted);
            siteService.GetFieldCompleted += new EventHandler<GetFieldCompletedEventArgs>(siteService_GetFieldCompleted);
            siteService.GetFieldsForLeagueCompleted += new EventHandler<GetFieldsForLeagueCompletedEventArgs>(siteService_GetFieldsCompleted);
            siteService.GetLeagueCompleted += new EventHandler<GetLeagueCompletedEventArgs>(siteService_GetLeagueCompleted);
            siteService.GetLeaguesCompleted += new EventHandler<GetLeaguesCompletedEventArgs>(siteService_GetLeaguesCompleted);
            siteService.GetMyFieldsCompleted += new EventHandler<GetMyFieldsCompletedEventArgs>(siteService_GetMyFieldsCompleted);

            return siteService;
        }

        void siteService_GetMyFieldsCompleted(object sender, GetMyFieldsCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.PopError(e.Error);
                return;
            }

            this.OnFields(e.Result);
        }

        void siteService_GetLeagueCompleted(object sender, GetLeagueCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.PopError(e.Error);
                return;
            }

            this.OnLeague(e.Result);
        }

        void siteService_GetDisplayNameCompleted(object sender, GetDisplayNameCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.PopError(e.Error);
                return;
            }

            if (string.IsNullOrEmpty(e.Result))
            {
                this._userNameLabel.Text = "Anonymous";
            }
            else
            {
                this._isLoggedIn = true;
                this._userNameLabel.Text = e.Result;
            }

            this.CreateServiceClient().GetLeaguesAsync();
        }

        void siteService_GetLeaguesCompleted(object sender, GetLeaguesCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.PopError(e.Error);
                return;
            }

            this.OnLeagues(e.Result);
        }

        void siteService_GetFieldsCompleted(object sender, GetFieldsForLeagueCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.PopError(e.Error);
                return;
            }

            this.OnFields(e.Result);
        }

        void siteService_GetFieldCompleted(object sender, GetFieldCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.PopError(e.Error);
                return;
            }

            this.OnField(e.Result, true);
        }

        void siteService_GetAllLeaguesCompleted(object sender, GetAllLeaguesCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.PopError(e.Error);
                return;
            }

            this.OnLeagues(e.Result);
        }
        #endregion

        #region DropDown List Control
        private void _leaguesDropButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._leaguesListBox.Visibility == Visibility.Collapsed)
            {
                this._leaguesListBox.Visibility = Visibility.Visible;
            }
            else
            {
                this._leaguesListBox.Visibility = Visibility.Collapsed;
            }
        }

        private void _leaguesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._leaguesListBox.Visibility = Visibility.Collapsed;

            int leagueID = Convert.ToInt32(((League)((ListBox)sender).SelectedItem).LeagueID);
            if (leagueID < 0)
            {
                this.CreateServiceClient().GetMyFieldsAsync();
                this._selectedLeagueLabel.Text = "<My Fields>";
                this._leagueNameLabel.Text = "My";
            }
            else
            {
                SiteServiceClient client = this.CreateServiceClient();
                client.GetFieldsForLeagueAsync(leagueID);
                client.GetLeagueAsync(leagueID);
                this._viewLeagueMethod.InvokeSelf(leagueID);
            }

            this._clearDirectionsMethod.InvokeSelf();
            this._viewContactsMethod.InvokeSelf(false);
            this._highlightFieldTab();
            this.Loading.Begin();
            this.LoadingFadeOut.Stop();
            this.LoadingFadeIn.Begin();
        }
        #endregion

        private void _fieldListItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SiteServiceClient client = this.CreateServiceClient();
            client.GetFieldAsync(Convert.ToInt32((sender as StackPanel).Tag));

            if (this._currentFieldItem != null)
            {
                ((Border)this._currentFieldItem.Children[0]).BorderBrush = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                ((Border)((StackPanel)sender).Children[0]).BorderThickness = new Thickness(1.0);
            }
            ((Border)((StackPanel)sender).Children[0]).BorderBrush = this.fieldsBorderBrush;
            ((Border)((StackPanel)sender).Children[0]).BorderThickness = new Thickness(1.5);
            this._currentFieldItem = (StackPanel)sender;

            this._clearDirectionsMethod.InvokeSelf();
            this._viewTrafficMethod.InvokeSelf(false);
            this._viewContactsMethod.InvokeSelf(false);
            this._highlightFieldTab();
            this.Loading.Begin();
            this.LoadingFadeOut.Stop();
            this.LoadingFadeIn.Begin();
        }

        #region Map Commands
        private void ShowField()
        {
            if (this._currentField == null) { return; }

            this._viewFieldMethod.InvokeSelf(this._currentField.FieldID, this._currentField.Address, this._currentField.Latitude, this._currentField.Longitude);
            this._clearDirectionsMethod.InvokeSelf();
            this._viewTrafficMethod.InvokeSelf(false);
            this._viewContactsMethod.InvokeSelf(false);
        }

        private void ShowTraffic()
        {
            if (this._currentField == null) { return; }

            this._clearDirectionsMethod.InvokeSelf();
            this._viewTrafficMethod.InvokeSelf(true);
            this._viewContactsMethod.InvokeSelf(false);
        }

        private void ShowDirections()
        {
            if (this._currentField == null) { return; }

            this._viewDirectionsMethod.InvokeSelf(this._currentField.Address);
            this._viewTrafficMethod.InvokeSelf(false);
            this._viewContactsMethod.InvokeSelf(false);
        }

        private void ShowContacts()
        {
            if (this._currentField == null) { return; }

            this._clearDirectionsMethod.InvokeSelf();
            this._viewTrafficMethod.InvokeSelf(false);
            this._viewContactsMethod.InvokeSelf(true);
        }
        #endregion

        #region Tab Events
        private void _viewFieldClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void _viewDirectionsClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void _viewTrafficClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void _viewContactsClick(object sender, MouseButtonEventArgs e)
        {

        }
        #endregion

        #region Animation Event Handlers

        private void _tab_Loaded(object sender, EventArgs e)
        {
            object obj = VisualTreeHelper.GetParent(sender as Canvas);
            if (obj == this.tabField)
            {
                this._fieldTab = (Canvas)sender;
                this._currentTab = (Canvas)sender;
                Storyboard.SetTarget(this.tabSelect, ((Canvas)sender).Children[1]);
                this.tabSelect.Begin();
            }
        }

        private void _tab_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this._currentTab != sender)
            {
                this.tabMouseOver.Stop();
                Storyboard.SetTarget(this.tabMouseOver, ((Canvas)sender).Children[1]);
                this.tabMouseOver.Begin();
            }
        }

        private void _tab_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this._currentTab != sender)
            {
                this.tabMouseOut.Stop();
                Storyboard.SetTarget(this.tabMouseOut, ((Canvas)sender).Children[1]);
                this.tabMouseOut.Begin();
            }
        }

        private void _tab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this._currentTab != sender)
            {
                this.tabMouseOver.Stop();
                this.tabMouseOut.Stop();
                if (this._currentTab != null)
                {
                    this.tabDeselect.Stop();
                    Storyboard.SetTarget(this.tabDeselect, this._currentTab.Children[1]);
                    this.tabDeselect.Begin();
                }
                this.tabSelect.Stop();
                Storyboard.SetTarget(this.tabSelect, ((Canvas)sender).Children[1]);
                this.tabSelect.Begin();
                this._currentTab = (Canvas)sender;
            }
        }

        private void _field_MouseEnter(object sender, MouseEventArgs e)
        {
            this.fieldMouseOver.Stop();
            Storyboard.SetTarget(this.fieldMouseOver, (DependencyObject)sender);
            this.fieldMouseOver.Begin();
        }

        private void _field_MouseLeave(object sender, MouseEventArgs e)
        {
            this.fieldMouseOut.Stop();
            Storyboard.SetTarget(this.fieldMouseOut, (DependencyObject)sender);
            this.fieldMouseOut.Begin();
        }

        private void panelMyFields_Loaded(object sender, RoutedEventArgs e)
        {
            this.panelShine.Begin();
        }

        private void loadingThrobber_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loading.Begin();
        }

        private void LoadingFadeOut_Completed(object sender, EventArgs e)
        {
            this.Loading.Stop();
        }
        #endregion

        #region Utility Methods
        private void PopError(Exception e)
        {
            HtmlPage.Window.Invoke("alert", "DEBUG:\n" + e.Message);
        }

        private void _highlightFieldTab()
        {
            if (this._fieldTab != this._currentTab)
            {
                this.tabMouseOver.Stop();
                this.tabMouseOut.Stop();
                if (this._currentTab != null)
                {
                    this.tabDeselect.Stop();
                    Storyboard.SetTarget(this.tabDeselect, this._currentTab.Children[1]);
                    this.tabDeselect.Begin();
                }
                this.tabSelect.Stop();
                Storyboard.SetTarget(this.tabSelect, this._fieldTab.Children[1]);
                this.tabSelect.Begin();
                this._currentTab = this._fieldTab;
            }
        }
        #endregion

        private void tabFriends_Click(object sender, RoutedEventArgs e)
        {
            this.ShowContacts();
        }

        private void tabTraffic_Click(object sender, RoutedEventArgs e)
        {
            this.ShowTraffic();
        }

        private void tabDirections_Click(object sender, RoutedEventArgs e)
        {
            this.ShowDirections();
        }

        private void tabField_Click(object sender, RoutedEventArgs e)
        {
            this.ShowField();
        }

    }
}
