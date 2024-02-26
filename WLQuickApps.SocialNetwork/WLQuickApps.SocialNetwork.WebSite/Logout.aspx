<%@ Page Language="C#" MasterPageFile="" Theme="" %>

<script runat="server">
    
    protected void Page_Load(object sender, EventArgs e)
    {
        WindowsLiveLogin.LogoutUser();
        this.Response.Redirect(FormsAuthentication.DefaultUrl);
    }
    
</script>