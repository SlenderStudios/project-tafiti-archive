using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    public class TemporaryUser : IDisposable
    {
        public TemporaryUser(bool logIn)
        {
            this.GetTempUser(logIn);
        }

        public User TempUser
        {
            get
            {
                if (this._tempUser == null)
                {
                    string baseline = Guid.NewGuid().ToString("N");
                    this._tempUser = UserManager.CreateUser(baseline, baseline + "@WLQuickApps.com", baseline, baseline, Gender.Unspecified, DateTime.Now.AddYears(-14), baseline, baseline, Location.Empty, string.Empty, null, "22BC3EEF0F511CA", string.Empty,string.Empty);
                }

                return _tempUser;
            }
        }
        private User _tempUser;

        public User GetTempUser(bool logUserIn)
        {
            if (logUserIn)
            {
                Utilities.SwitchToCustomUser(this.TempUser.UserName);
            }
            return this.TempUser;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this._tempUser != null)
            {
                this._tempUser.Delete();
                this._tempUser = null;
            }
        }

        #endregion
    }
}
