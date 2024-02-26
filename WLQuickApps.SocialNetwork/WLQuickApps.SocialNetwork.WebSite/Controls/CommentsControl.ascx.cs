using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WLQuickApps.SocialNetwork.Business;
using System.ComponentModel;
using WLQuickApps.SocialNetwork.WebSite;

public partial class CommentControl : System.Web.UI.UserControl
{
    [ToolboxItem(true)]
    public bool HalfPanel
    {
        get { return this._halfPanel; }
        set { this._halfPanel = value; }
    }
    private bool _halfPanel = false;

    [ToolboxItem(true)]
    public bool OldestCommentsFirst
    {
        get { return this._oldestCommentsFirst; }
        set { this._oldestCommentsFirst = value; }
    }
    private bool _oldestCommentsFirst;

    public int BaseItemID
    {
        get { return this._baseItemID; }
        set 
        {
            this._baseItemID = value;
            this._contentTextBox.Visible = this.BaseItemItem.CanContribute;
            this._addCommentButton.Visible = this.BaseItemItem.CanContribute;
        }
    }
    protected int _baseItemID;

    public BaseItem BaseItemItem
    {
        get { return BaseItemManager.GetBaseItem(this.BaseItemID); }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.RegisterRequiresControlState(this);
    }

    protected void Page_Load(object sender, EventArgs e) 
    {
        if (!this.IsPostBack)
        {
            this._contentTextBox.Attributes.Add("onkeypress", string.Format("return LimitText(this, {0})", this._contentTextBox.MaxLength));
            this._contentTextBox.Attributes.Add("onblur", string.Format("LimitText(this, {0})", this._contentTextBox.MaxLength));
        }

        if (this._halfPanel)
        {
            this._contentTextBox = this._smallContentTextBox;
            this._addCommentButton = this._smallAddCommentButton;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (this.IsPostBack)
        {
            this.RefreshComments();
        }
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

    protected void _deleteComment_Command(object sender, CommandEventArgs e)
    {
        CommentManager.DeleteComment(Int32.Parse(e.CommandArgument.ToString()));
        this.RefreshComments();
    }

    protected void _addCommentButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(this._contentTextBox.Text))
        {
            return;
        }

        CommentManager.CreateComment(this.BaseItemID, this._contentTextBox.Text);

        CommunicationManager.SendMessage(this.BaseItemItem.Owner.Email, 
            string.Format("{0} has left a comment on {1}: {2}", UserManager.LoggedInUser.Title, this.BaseItemItem.Title, this._contentTextBox.Text),
            this.Request.Url.AbsoluteUri);

        this._contentTextBox.Text = string.Empty;

        this.RefreshComments();
    }

    protected void _commentsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[WebConstants.DataBindingParameters.BaseItemID] = this._baseItemID;

        if (!e.ExecutingSelectCount)
        {
            e.InputParameters[WebConstants.DataBindingParameters.OldestFirst] = this._oldestCommentsFirst;
        }
        else if (e.InputParameters.Contains(WebConstants.DataBindingParameters.OldestFirst))
        {
            e.InputParameters.Remove(WebConstants.DataBindingParameters.OldestFirst);
        }
    }

    private void RefreshComments()
    {
        this.DataList1.DataBind();
    }
}
