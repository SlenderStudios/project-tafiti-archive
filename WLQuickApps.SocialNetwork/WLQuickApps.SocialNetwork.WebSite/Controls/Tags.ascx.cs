using System;
using System.Data;
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
using System.Reflection;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;

public partial class Tags : System.Web.UI.UserControl
{
    public int BaseItemID
    {
        get { return this._baseItemID; }
        set { this._baseItemID = value; }
    }
    protected int _baseItemID;

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.RegisterRequiresControlState(this);
        this.DataBinding += new EventHandler(Tags_DataBinding);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //this._tagList.DataBind();
    }

    protected void Tags_DataBinding(object sender, EventArgs e)
    {
        this._tagList.DataBind();
        this._addTagHolder.Visible = BaseItemManager.GetBaseItem(this._baseItemID).CanEdit;
    }

    protected override void LoadControlState(object savedState)
    {
        Dictionary<string, object> stateData = (Dictionary<string, object>)savedState;

        base.LoadControlState(stateData[WebConstants.ControlStateVariables.BaseState]);
        this._baseItemID = (int)stateData[WebConstants.ControlStateVariables.BaseItemID];
    }

    protected override object SaveControlState()
    {
        Dictionary<string, object> stateData = new Dictionary<string, object>(2);

        stateData[WebConstants.ControlStateVariables.BaseState] = base.SaveControlState();
        stateData[WebConstants.ControlStateVariables.BaseItemID] = this._baseItemID;

        return stateData;
    }

    protected void _newTag_PreRender(object sender, EventArgs e)
    {
        this._newTag.Attributes["onkeypress"] = String.Format("return {0}_newTag_keyPress(this, event);", this.ClientID);
        this._newTag.Attributes["onkeyup"] = String.Format("return {0}_newTag_keyUp(this, event);", this.ClientID);
    }

    protected void _addTag_PreRender(object sender, EventArgs e)
    {
        this._addTag.Style[HtmlTextWriterStyle.Display] = "none";
    }

    protected void _addTag_Click(object sender, EventArgs e)
    {
        string[] tags = this.Request.Form[this._newTag.UniqueID].Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        BaseItem baseItem = BaseItemManager.GetBaseItem(this._baseItemID);
        foreach (string tag in tags)
        {
            baseItem.AddTag(tag);
        }

        this._newTag.Text = string.Empty;
        this.Page.DataBind();
    }

    protected void _removeTag_Command(object sender, CommandEventArgs e)
    {
        string tag = e.CommandArgument.ToString();

        BaseItemManager.GetBaseItem(this._baseItemID).RemoveTag(tag);
        this.Page.DataBind();
    }

    protected void _tagList_ItemDataBound(object sender, EventArgs e)
    {
        this._emptyDataLabel.Visible = false;
    }

    protected void _tagDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[WebConstants.DataBindingParameters.BaseItemID] = this._baseItemID;
    }
}
