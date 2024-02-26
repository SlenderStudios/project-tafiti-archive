using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

public partial class Admin_ApproveItems : System.Web.UI.Page
{
    private SearchResults _searchResults;

    protected void Page_Init(object sender, EventArgs e)
    {
        this._unapprovedItemsTotalLabel.Text = string.Format("Total: {0} (100 max displayed)", BaseItemManager.GetUnapprovedBaseItemsCount());

        this._searchResults = BaseItemManager.GetUnapprovedBaseItems();

        this._pictureTabPanel.HeaderText = string.Format("Pictures ({0})", this._searchResults.Pictures.Count);
        this._videoTabPanel.HeaderText = string.Format("Video ({0})", this._searchResults.Videos.Count);
        this._audioTabPanel.HeaderText = string.Format("Audio ({0})", this._searchResults.Audios.Count);
        this._groupsTabPanel.HeaderText = string.Format("Groups ({0})", this._searchResults.Groups.Count);
        this._eventsTabPanel.HeaderText = string.Format("Events ({0})", this._searchResults.Events.Count);
        this._usersTabPanel.HeaderText = string.Format("Users ({0})", this._searchResults.Users.Count);
        this._collectionsTabPanel.HeaderText = string.Format("Collections ({0})", this._searchResults.Collections.Count);

        this._usersGallery.DataSource = this._searchResults.Users;
        this._groupsGallery.DataSource = this._searchResults.Groups;
        this._eventsCalendar.EventList = new List<Event>(this._searchResults.Events);
        this._picturesGallery.DataSource = this._searchResults.Pictures;
        this._audioGallery.DataSource = this._searchResults.Audios;
        this._videoGallery.DataSource = this._searchResults.Videos;
        this._collectionsGallery.DataSource = this._searchResults.Collections;

        this._pictureTabPanel.Enabled = (this._searchResults.Pictures.Count > 0);
        this._videoTabPanel.Enabled = (this._searchResults.Videos.Count > 0);
        this._audioTabPanel.Enabled = (this._searchResults.Audios.Count > 0);
        this._usersTabPanel.Enabled = (this._searchResults.Users.Count > 0);
        this._groupsTabPanel.Enabled = (this._searchResults.Groups.Count > 0);
        this._eventsTabPanel.Enabled = (this._searchResults.Events.Count > 0);
        this._collectionsTabPanel.Enabled = (this._searchResults.Collections.Count > 0);

        foreach (string groupType in SettingsWrapper.SpecialGroups)
        {
            ReadOnlyCollection<Group> groups = this._searchResults.GetSpecialGroups(groupType);
            if (groups.Count == 0) { continue; }

            MetaGallery metaGallery = new MetaGallery();
            metaGallery.DataSource = groups;
            TabPanel tabPanel = new TabPanel();
            // TODO: We're appending the 's' on the groups & events because we know Team[s], Practice[s], and Game[s] all work. However,
            // there are lots of things that aren't pluralized by simply appending an 's'. Check back here when things stop looking right.
            tabPanel.HeaderText = string.Format("{0}s ({1})", groupType, groups.Count);
            tabPanel.Controls.Add(metaGallery);
            LinkButton linkButton = new LinkButton();
            linkButton.ID = string.Format("_approve{0}s", groupType);
            linkButton.Text = string.Format("Approve All {0}s On This Tab", groupType);
            linkButton.Command += new CommandEventHandler(this._approveSpecialGroups_Command);
            linkButton.CommandName = "ItemType";
            linkButton.CommandArgument = groupType;
            tabPanel.Controls.Add(linkButton);
            ConfirmButtonExtender confirmExtender = new ConfirmButtonExtender();
            confirmExtender.ConfirmText = string.Format("Are you sure you want to approve all of these {0}s?", groupType);
            confirmExtender.TargetControlID = linkButton.ID;
            tabPanel.Controls.Add(confirmExtender);
            this._searchResultsTabs.Tabs.Add(tabPanel);
        }
        
        foreach (string eventType in SettingsWrapper.SpecialEvents)
        {
            ReadOnlyCollection<Event> events = this._searchResults.GetSpecialEvents(eventType);
            if (events.Count == 0) { continue; }

            MetaGallery metaGallery = new MetaGallery();
            metaGallery.DataSource = events;
            TabPanel tabPanel = new TabPanel();
            // TODO: We're appending the 's' on the groups & events because we know Team[s], Practice[s], and Game[s] all work. However,
            // there are lots of things that aren't pluralized by simply appending an 's'. Check back here when things stop looking right.
            tabPanel.HeaderText = string.Format("{0}s ({1})", eventType, events.Count);
            tabPanel.Controls.Add(metaGallery);
            LinkButton linkButton = new LinkButton();
            linkButton.ID = string.Format("_approve{0}s", eventType);
            linkButton.Text = string.Format("Approve All {0}s On This Tab", eventType);
            linkButton.Command += new CommandEventHandler(this._approveSpecialEvents_Command);
            linkButton.CommandName = "ItemType";
            linkButton.CommandArgument = eventType;
            tabPanel.Controls.Add(linkButton);
            ConfirmButtonExtender confirmExtender = new ConfirmButtonExtender();
            confirmExtender.ConfirmText = string.Format("Are you sure you want to approve all of these {0}s?", eventType);
            confirmExtender.TargetControlID = linkButton.ID;
            tabPanel.Controls.Add(confirmExtender);
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

    protected void _approveAudio_Command(object sender, CommandEventArgs e)
    {  
        foreach (BaseItem baseItem in this._searchResults.Audios)
        {
            this.ApproveBaseItem(baseItem);
        }
        this.RedirectToApproveItems();
    }

    protected void _approveCollections_Command(object sender, CommandEventArgs e)
    {
        foreach (BaseItem baseItem in this._searchResults.Collections)
        {
            this.ApproveBaseItem(baseItem);
        }
        this.RedirectToApproveItems();
    }

    protected void _approveEvents_Command(object sender, CommandEventArgs e)
    {
        foreach (BaseItem baseItem in this._searchResults.Events)
        {
            this.ApproveBaseItem(baseItem);
        }
        this.RedirectToApproveItems();
    }

    protected void _approveGroups_Command(object sender, CommandEventArgs e)
    {
        foreach (BaseItem baseItem in this._searchResults.Groups)
        {
            this.ApproveBaseItem(baseItem);
        }
        this.RedirectToApproveItems();
    }

    protected void _approvePictures_Command(object sender, CommandEventArgs e)
    {
        foreach (BaseItem baseItem in this._searchResults.Pictures)
        {
            this.ApproveBaseItem(baseItem);
        }
        this.RedirectToApproveItems();
    }

    protected void _approveSpecialGroups_Command(object sender, CommandEventArgs e)
    {
        foreach (BaseItem baseItem in this._searchResults.GetSpecialGroups(e.CommandArgument.ToString()))
        {
            this.ApproveBaseItem(baseItem);
        }
        this.RedirectToApproveItems();
    }

    protected void _approveSpecialEvents_Command(object sender, CommandEventArgs e)
    {
        foreach (BaseItem baseItem in this._searchResults.GetSpecialEvents(e.CommandArgument.ToString()))
        {
            this.ApproveBaseItem(baseItem);
        }
        this.RedirectToApproveItems();
    }

    protected void _approveUsers_Command(object sender, CommandEventArgs e)
    {
        foreach (BaseItem baseItem in this._searchResults.Users)
        {
            this.ApproveBaseItem(baseItem);
        }
        this.RedirectToApproveItems();
    }

    protected void _approveVideo_Command(object sender, CommandEventArgs e)
    {
        foreach (BaseItem baseItem in this._searchResults.Videos)
        {
            this.ApproveBaseItem(baseItem);
        }
        this.RedirectToApproveItems();
    }

    protected void ApproveBaseItem(BaseItem baseItem)
    {
        try 
        { 
            baseItem.Approve();
        }
        catch{}   
    }

    protected void RedirectToApproveItems()
    {
        Response.Redirect("~/Admin/ApproveItems.aspx");
    }
}
