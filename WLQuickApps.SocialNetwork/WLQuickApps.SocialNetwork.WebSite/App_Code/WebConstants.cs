using System;
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
    sealed public class WebConstants
    {
        private WebConstants() { }

        sealed public class AppSettingKeys
        {
            private AppSettingKeys() { }

            public const string FeedbackEmail = "FeedbackEmail";
            public const string P3PCompactPolicy = "P3PCompactPolicy";
            public const string SiteTitle = "SiteTitle";
            public const string SpecialGroups = "SpecialGroups";
            public const string SpecialEvents = "SpecialEvents";
            public const string SpecialForums = "SpecialForums";
            public const string WindowsLiveAppID = "WindowsLiveAppID";
            public const string WindowsLiveSecret = "WindowsLiveSecret";
            public const string LiveAlertsChangeUrl = "LiveAlertsChangeUrl";
            public const string EnableLiveAlerts = "EnableLiveAlerts";
            public const string MicrosoftAnalyticsID = "MicrosoftAnalyticsID";
        }

        sealed public class CacheKeys
        {
            private CacheKeys() { }

            public const string Picture = "Picture";
        }

        sealed public class ControlStateVariables
        {
            private ControlStateVariables() { }

            public const string BaseState = "base";
            public const string BaseItemID = "baseItemID";
            public const string DisplayMode = "displayMode";
            public const string EventID = "eventID";
            public const string EventStatus = "eventStatus";
            public const string GroupID = "groupID";
            public const string LocationID = "locationID";
            public const string MediaID = "mediaID";
            public const string PictureTempName = "pictureTempName";
            public const string ShowLocationCaption = "showLocationCaption";
            public const string TagType = "tagType";
            public const string UserID = "userID";
            public const string UserName = "userName";
            public const string WindowsLiveUUID = "windowsLiveUUID";
        }

        sealed public class HttpHeaderKeys
        {
            private HttpHeaderKeys() { }

            public const string P3P = "P3P";
        }

        sealed public class ViewStateVariables
        {
            private ViewStateVariables() { }

            public const string RequestCount = "RequestCount";
            public const string RequestsLabelText = "RequestsLabelText";
            public const string SingularRequestsLabelText = "SingularRequestsLabelText";
            public const string EmptyDataText = "EmptyDataText";
            public const string ViewMode = "ViewMode";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";
            public const string HeaderText = "HeaderText";
            public const string SentInvites = "SentInvites";
        }

        sealed public class QueryVariables
        {
            private QueryVariables() { }

            public const string BaseItemID = "baseItemID";
            public const string UserName = "userName";
            public const string LocationID = "locationID";
            public const string StartAddress = "startAddress";
            public const string EndAddress = "endAddress";
            public const string MaxThumbnailHeight = "maxHeight";
            public const string MaxThumbnailWidth = "maxWidth";
            public const string Mode = "mode";
            public const string Resort = "resort";
            public const string ReturnURL = "ReturnUrl";
            public const string SearchName = "searchName";
        }

        sealed public class WindowsLiveVariables
        {
            private WindowsLiveVariables() { }

            public const string Action = "action";
            public const string AppContext = "appctx";
            public const string AppID = "appid";
            public const string ClearCookieAction = "clearcookie";
            public const string EncryptionKeyPrefix = "ENCRYPTION";
            public const string LogoutAction = "logout";
            public const string Signature = "sig";
            public const string SignatureKeyPrefix = "SIGNATURE";
            public const string Token = "stoken";
            public const string UID = "uid";
        }

        sealed public class SessionVariables
        {
            private SessionVariables() { }

            public const string ContextLocation = "ContextLocation";
            public const string InviteList = "InviteList";
            public const string MetaGalleryData = "MetaGalleryData";
            public const string WindowsLiveUUID = "WindowsLiveUUID";
            public const string WindowsOwnerHandle = "WindowsOwnerHandle";
        }

        sealed public class DataBindingParameters
        {
            public const string AlbumBaseItemID = "albumBaseItemID";
            public const string BaseItem = "baseItem";
            public const string BaseItemID = "baseItemID";
            public const string Collection = "collection";
            public const string Group = "group";
            public const string OldestFirst = "oldestFirst";
            public const string SearchRangeStart = "searchRangeStart";
            public const string SearchRangeEnd = "searchRangeEnd";
            public const string UserID = "userID";
            public const string UserName = "userName";
        }

        sealed public class PagingMovementCommands
        {
            public const string First = "First";
            public const string Previous = "Previous";
            public const string Next = "Next";
            public const string Last = "Last";
        }
    }
}
