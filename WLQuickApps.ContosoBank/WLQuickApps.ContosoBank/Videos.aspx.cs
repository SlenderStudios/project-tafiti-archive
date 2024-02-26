/*****************************************************************************
 * Videos.apx
 * Notes: Page to show and play latest community videos
 * **************************************************************************/

using System;

using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank
{
    public partial class Videos : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Intitialise the player with the latest video
                CurrentSilverlightStreamingMediaPlayer.MediaSource = VideoLogic.GetLatestVideo().VideoURL;
            }
        }
    }
}