using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WLQuickApps.SocialNetwork.WebSite
{
    public class InvitationHelper : IDisposable
    {
        private Dictionary<string, string> _inviteList;

        public InvitationHelper()
        {
            this._inviteList = HttpContext.Current.Session[WebConstants.SessionVariables.InviteList] as Dictionary<string, string>;
            if (this._inviteList == null)
            {
                this._inviteList = new Dictionary<string, string>();
            }
        }

        public void AddInvite(string key, string value)
        {
            this._inviteList[key] = value;
            this.PersistInviteList();
        }

        public void ClearInviteList()
        {
            this._inviteList.Clear();
            this.PersistInviteList();
        }

        public void RemoveInvite(string key)
        {
            this._inviteList.Remove(key);
            this.PersistInviteList();
        }

        public Dictionary<string, string> GetInviteList()
        {
            return this._inviteList;
        }

        private void PersistInviteList()
        {
            HttpContext.Current.Session[WebConstants.SessionVariables.InviteList] = this._inviteList;
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._inviteList != null)
                {
                    this.PersistInviteList();
                    this._inviteList = null;
                }
            }
        }

        #endregion
    }
}
