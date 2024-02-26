using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;
using AjaxControlToolkit;

public partial class Search : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        this._tagTextBox.Text = this.Request.QueryString["tag"];
        this._locationControl.Name = this.Request.QueryString["name"];
        this._locationControl.Address1 = this.Request.QueryString["address1"];
        this._locationControl.Address2 = this.Request.QueryString["address2"];
        this._locationControl.City = this.Request.QueryString["city"];
        this._locationControl.Region = this.Request.QueryString["region"];
        this._locationControl.Country = this.Request.QueryString["country"];
        this._locationControl.PostalCode = this.Request.QueryString["postalCode"];

        SearchResults searchResults = SearchManager.SearchNetwork(
            this._locationControl.Name,
            this._locationControl.Address1,
            this._locationControl.Address2,
            this._locationControl.City,
            this._locationControl.Region,
            this._locationControl.Country,
            this._locationControl.PostalCode,
            this._tagTextBox.Text);

        this._pictureTabPanel.HeaderText = string.Format("Pictures ({0})", searchResults.Pictures.Count);
        this._videoTabPanel.HeaderText = string.Format("Videos ({0})", searchResults.Videos.Count);
        this._audioTabPanel.HeaderText = string.Format("Audios ({0})", searchResults.Audios.Count);
        this._groupsTabPanel.HeaderText = string.Format("Groups ({0})", searchResults.Groups.Count);
        this._eventsTabPanel.HeaderText = string.Format("Events ({0})", searchResults.Events.Count);
        this._usersTabPanel.HeaderText = string.Format("Users ({0})", searchResults.Users.Count);
        this._collectionsTabPanel.HeaderText = string.Format("Maps ({0})", searchResults.Collections.Count);

        this._usersGallery.DataSource = searchResults.Users;
        this._groupsGallery.DataSource = searchResults.Groups;
        this._eventsCalendar.EventList = new List<Event>(searchResults.Events);
        this._picturesGallery.DataSource = searchResults.Pictures;
        this._audioGallery.DataSource = searchResults.Audios;
        this._videoGallery.DataSource = searchResults.Videos;
        this._collectionsGallery.DataSource = searchResults.Collections;

        this._pictureTabPanel.Enabled = (searchResults.Pictures.Count > 0);
        this._videoTabPanel.Enabled = (searchResults.Videos.Count > 0);
        this._audioTabPanel.Enabled = (searchResults.Audios.Count > 0);
        this._usersTabPanel.Enabled = (searchResults.Users.Count > 0);
        this._groupsTabPanel.Enabled = (searchResults.Groups.Count > 0);
        this._eventsTabPanel.Enabled = (searchResults.Events.Count > 0);
        this._collectionsTabPanel.Enabled = (searchResults.Collections.Count > 0);

        foreach (string groupType in SettingsWrapper.SpecialGroups)
        {
            ReadOnlyCollection<Group> groups = searchResults.GetSpecialGroups(groupType);
            if (groups.Count == 0) { continue; }

            MetaGallery metaGallery = new MetaGallery();
            metaGallery.DataSource = groups;
            TabPanel tabPanel = new TabPanel();
            // TODO: We're appending the 's' on the groups & events because we know Team[s], Practice[s], and Game[s] all work. However,
            // there are lots of things that aren't pluralized by simply appending an 's'. Check back here when things stop looking right.
            tabPanel.HeaderText = string.Format("{0}s ({1})", groupType, groups.Count);
            tabPanel.Controls.Add(metaGallery);
            this._searchResultsTabs.Tabs.Add(tabPanel);
        }

        foreach (string eventType in SettingsWrapper.SpecialEvents)
        {
            ReadOnlyCollection<Event> events = searchResults.GetSpecialEvents(eventType);
            if (events.Count == 0) { continue; }

            MetaGallery metaGallery = new MetaGallery();
            metaGallery.DataSource = events;
            TabPanel tabPanel = new TabPanel();
            // TODO: We're appending the 's' on the groups & events because we know Team[s], Practice[s], and Game[s] all work. However,
            // there are lots of things that aren't pluralized by simply appending an 's'. Check back here when things stop looking right.
            tabPanel.HeaderText = string.Format("{0}s ({1})", eventType, events.Count);
            tabPanel.Controls.Add(metaGallery);
            this._searchResultsTabs.Tabs.Add(tabPanel);
        }

        this._noResultsPanel.Visible = true;
        this._searchResultsPanel.Visible = false;
        foreach (TabPanel tabPanel in this._searchResultsTabs.Tabs)
        {
            if (tabPanel.Enabled)
            {
                this._searchResultsTabs.ActiveTab = tabPanel;

                this._noResultsPanel.Visible = false;
                this._searchResultsPanel.Visible = true;

                break;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DataBind();
    }

    protected void _deleteMedia_Command(object sender, CommandEventArgs e)
    {
        Media media = MediaManager.GetMedia(Int32.Parse(e.CommandArgument.ToString()));
        media.Delete();
        Response.Redirect(WebUtilities.GetViewItemUrl(media.ParentAlbum));
    }

    protected void _searchButton_Click(object sender, EventArgs e)
    {
        this.Response.Redirect(string.Format("~/Search.aspx?name={0}&address1={1}&address2={2}&city={3}&region={4}&country={5}&postalCode={6}&tag={7}",
            this._locationControl.Name,
            this._locationControl.Address1,
            this._locationControl.Address2,
            this._locationControl.City,
            this._locationControl.Region,
            this._locationControl.Country,
            this._locationControl.PostalCode,
            this._tagTextBox.Text));
    }
}
