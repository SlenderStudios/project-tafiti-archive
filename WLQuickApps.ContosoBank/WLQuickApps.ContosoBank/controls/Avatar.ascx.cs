using System;
using System.Web.UI;

using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class Avatar : UserControl, IAvatarData
    {
        private Entity.Avatar item;

        #region IAvatarData Members

        public Entity.Avatar Item
        {
            get { return item; }
            set { item = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            AvatarImage.ImageUrl = item.Location;
        }
    }
}