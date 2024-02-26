using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using WLQuickApps.SocialNetwork.Data;

namespace WLQuickApps.SocialNetwork.Business
{
    public enum UserGroupStatus
    {
        Invited,
        Joined,
        WaitingForApproval
    }

    public enum MediaType
    {
        Audio,
        Picture,
        Video,
        File,
    }

    public enum PrivacyLevel
    {
        Public,
        Private,
        Invisible,
    }

    public enum Gender
    {
        Male,
        Female,
        Unspecified,
    }

    public enum ImageMissingReason
    {
        None,
        NotFound,
        PermissionsInvalid,
        PermissionsRevoked,
        Other
    }

    internal enum BaseItemType
    {
        Group,
        Media,
        User,
        Album,
    }

    sealed public class Constants
    {
        private Constants() { }

        sealed public class AppSettingKeys
        {
            private AppSettingKeys() { }

            public const string MapPointUserName = "MapPointUserName";
            public const string MapPointPassword = "MapPointPassword";
            public const string MapPointDataSource = "MapPointDataSource";
            public const string SilverlightStreamingIDPrefix = "SilverlightStreamingIDPrefix";
            public const string SilverlightStreamingUserName = "SilverlightStreamingUserName";
            public const string SilverlightStreamingPassword = "SilverlightStreamingPassword";
            public const string AutomaticallyApproveNewUsers = "AutomaticallyApproveNewUsers";
            public const string AutomaticallyApproveNewMedia = "AutomaticallyApproveNewMedia";
            public const string AutomaticallyApproveNewGroups = "AutomaticallyApproveNewGroups";
            public const string AutomaticallyApproveNewCollections = "AutomaticallyApproveNewCollections";
            public const string LiveAlertsMessageUrl = "LiveAlertsMessageUrl";
            public const string LiveAlertsSubscriptionUrl = "LiveAlertsSubscriptionUrl";
            public const string LiveAlertsPassword = "LiveAlertsPassword";
            public const string LiveAlertsPin = "LiveAlertsPin";
            public const string MediaDropPath = "MediaDropPath";
            public const string ProcessorQueuePath = "ProcessorQueuePath";
            public const string MapPointFindServiceUrl = "MapPointFindServiceUrl";
            public const string EnableLiveAlerts = "EnableLiveAlerts";
            public const string PrivacyStatementOverrideUrl = "PrivacyStatementOverrideUrl";
            public const string HostingLabel = "HostingLabel";
            public const string ExpressionMediaEncoderPath = "ExpressionMediaEncoderPath";
            public const string MediaThumbnailTimeout = "MediaThumbnailTimeout";
            public const string SiteFromEmailAddress = "SiteFromEmailAddress";

        }

        sealed public class BaseItemTypes
        {
            private BaseItemTypes() { }

            public const string Album = "Album";
            public const string Collection = "Collection";
            public const string CollectionItem = "CollectionItem";
            public const string Forum = "Forum";
            public const string Group = "Group";
            public const string Media = "Media";
            public const string User = "User";
        }

        sealed public class GroupTypes
        {
            private GroupTypes() { }

            public const string GenericEvent = "_GenericEvent_";
        }

        sealed public class MediaTypes
        {
            private MediaTypes() { }

            public const string Picture = "Picture";
            public const string Video = "Video";
            public const string Audio = "Audio";
            public const string File = "File";
        }

    }
}
