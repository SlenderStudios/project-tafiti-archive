using System;
using System.Collections.Generic;
using System.Text;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    public enum MediaType
    {
        Video,
        Audio
    }

    [Serializable]
    public class SilverlightStreamingSubmission
    {
        public string ID;
        public string Path;
        public MediaType Type;
        public string SilverlightStreamingUserName;
        public string SilverlightStreamingPassword;

        public SilverlightStreamingSubmission(string id, string path, MediaType type, string silverlightStreamingUserName, string silverlightStreamingPassword)
        {
            this.ID = id;
            this.Path = path;
            this.Type = type;
            this.SilverlightStreamingUserName = silverlightStreamingUserName;
            this.SilverlightStreamingPassword = silverlightStreamingPassword;
        }

    }

}
