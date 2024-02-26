using System.Collections.Generic;
using System.Linq;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class VideoLogic
    {
        public static List<Video> GetAllVideos()
        {
            ContosoBankDataContext ctx = new ContosoBankDataContext();
            var temp = from fs in ctx.Videos
                       orderby fs.UploadDate descending
                       select fs;

            return temp.ToList();
        }

        public static Video GetLatestVideo()
        {
            ContosoBankDataContext ctx = new ContosoBankDataContext();
            var temp = from fs in ctx.Videos
                       orderby fs.UploadDate descending
                       select fs;

            return temp.FirstOrDefault();
        }
    }
}