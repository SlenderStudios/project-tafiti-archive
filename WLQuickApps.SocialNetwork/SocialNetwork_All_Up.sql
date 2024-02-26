IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_WebEvent_FullAccess' AND type = 'R')
CREATE ROLE [aspnet_WebEvent_FullAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Roles_ReportingAccess' AND type = 'R')
CREATE ROLE [aspnet_Roles_ReportingAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Roles_FullAccess' AND type = 'R')
CREATE ROLE [aspnet_Roles_FullAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Roles_BasicAccess' AND type = 'R')
CREATE ROLE [aspnet_Roles_BasicAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Profile_ReportingAccess' AND type = 'R')
CREATE ROLE [aspnet_Profile_ReportingAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Profile_FullAccess' AND type = 'R')
CREATE ROLE [aspnet_Profile_FullAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Profile_BasicAccess' AND type = 'R')
CREATE ROLE [aspnet_Profile_BasicAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Personalization_ReportingAccess' AND type = 'R')
CREATE ROLE [aspnet_Personalization_ReportingAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Personalization_FullAccess' AND type = 'R')
CREATE ROLE [aspnet_Personalization_FullAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Personalization_BasicAccess' AND type = 'R')
CREATE ROLE [aspnet_Personalization_BasicAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Membership_ReportingAccess' AND type = 'R')
CREATE ROLE [aspnet_Membership_ReportingAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Membership_FullAccess' AND type = 'R')
CREATE ROLE [aspnet_Membership_FullAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Membership_BasicAccess' AND type = 'R')
CREATE ROLE [aspnet_Membership_BasicAccess]
GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_WebEvent_FullAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_WebEvent_FullAccess] AUTHORIZATION [aspnet_WebEvent_FullAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Roles_ReportingAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Roles_ReportingAccess] AUTHORIZATION [aspnet_Roles_ReportingAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Roles_FullAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Roles_FullAccess] AUTHORIZATION [aspnet_Roles_FullAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Roles_BasicAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Roles_BasicAccess] AUTHORIZATION [aspnet_Roles_BasicAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Profile_ReportingAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Profile_ReportingAccess] AUTHORIZATION [aspnet_Profile_ReportingAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Profile_FullAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Profile_FullAccess] AUTHORIZATION [aspnet_Profile_FullAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Profile_BasicAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Profile_BasicAccess] AUTHORIZATION [aspnet_Profile_BasicAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Personalization_ReportingAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Personalization_ReportingAccess] AUTHORIZATION [aspnet_Personalization_ReportingAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Personalization_FullAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Personalization_FullAccess] AUTHORIZATION [aspnet_Personalization_FullAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Personalization_BasicAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Personalization_BasicAccess] AUTHORIZATION [aspnet_Personalization_BasicAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Membership_ReportingAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Membership_ReportingAccess] AUTHORIZATION [aspnet_Membership_ReportingAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Membership_FullAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Membership_FullAccess] AUTHORIZATION [aspnet_Membership_FullAccess]'

GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'aspnet_Membership_BasicAccess')
EXEC sys.sp_executesql N'CREATE SCHEMA [aspnet_Membership_BasicAccess] AUTHORIZATION [aspnet_Membership_BasicAccess]'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_SchemaVersions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_SchemaVersions](
	[Feature] [nvarchar](128) NOT NULL,
	[CompatibleSchemaVersion] [nvarchar](128) NOT NULL,
	[IsCurrentVersion] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Feature] ASC,
	[CompatibleSchemaVersion] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Location]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Location](
	[LocationID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Location_LocationID]  DEFAULT (newid()),
	[Name] [text] NOT NULL,
	[Address1] [varchar](64) NULL,
	[Address2] [varchar](64) NULL,
	[City] [varchar](64) NULL,
	[Region] [varchar](64) NULL,
	[Country] [varchar](64) NULL,
	[PostalCode] [varchar](16) NOT NULL,
	[Latitude] [float] NOT NULL CONSTRAINT [DF_Location_Latitude]  DEFAULT ((0)),
	[Longitude] [float] NOT NULL CONSTRAINT [DF_Location_Longitude]  DEFAULT ((0)),
	[Type] [varchar](64) NOT NULL,
	[SearchData] [varchar](256) NOT NULL CONSTRAINT [DF_Location_SearchData]  DEFAULT ('?'),
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[LocationID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Friend]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Friend](
	[RequesterUserID] [uniqueidentifier] NOT NULL,
	[RequestedUserID] [uniqueidentifier] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Friend] PRIMARY KEY CLUSTERED 
(
	[RequesterUserID] ASC,
	[RequestedUserID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tag](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](32) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Album]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Album](
	[BaseItemID] [int] NOT NULL,
 CONSTRAINT [PK_Album] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_WebEvent_Events]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_WebEvent_Events](
	[EventId] [char](32) NOT NULL,
	[EventTimeUtc] [datetime] NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [nvarchar](256) NOT NULL,
	[EventSequence] [decimal](19, 0) NOT NULL,
	[EventOccurrence] [decimal](19, 0) NOT NULL,
	[EventCode] [int] NOT NULL,
	[EventDetailCode] [int] NOT NULL,
	[Message] [nvarchar](1024) NULL,
	[ApplicationPath] [nvarchar](256) NULL,
	[ApplicationVirtualPath] [nvarchar](256) NULL,
	[MachineName] [nvarchar](256) NOT NULL,
	[RequestUrl] [nvarchar](1024) NULL,
	[ExceptionType] [nvarchar](256) NULL,
	[Details] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Setup_RestorePermissions]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Setup_RestorePermissions]
    @name   sysname
AS
BEGIN
    DECLARE @object sysname
    DECLARE @protectType char(10)
    DECLARE @action varchar(60)
    DECLARE @grantee sysname
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT Object, ProtectType, [Action], Grantee FROM #aspnet_Permissions where Object = @name

    OPEN c1

    FETCH c1 INTO @object, @protectType, @action, @grantee
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = @protectType + '' '' + @action + '' on '' + @object + '' TO ['' + @grantee + '']''
        EXEC (@cmd)
        FETCH c1 INTO @object, @protectType, @action, @grantee
    END

    CLOSE c1
    DEALLOCATE c1
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Setup_RemoveAllRoleMembers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Setup_RemoveAllRoleMembers]
    @name   sysname
AS
BEGIN
    CREATE TABLE #aspnet_RoleMembers
    (
        Group_name      sysname,
        Group_id        smallint,
        Users_in_group  sysname,
        User_id         smallint
    )

    INSERT INTO #aspnet_RoleMembers
    EXEC sp_helpuser @name

    DECLARE @user_id smallint
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT User_id FROM #aspnet_RoleMembers

    OPEN c1

    FETCH c1 INTO @user_id
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = ''EXEC sp_droprolemember '' + '''''''' + @name + '''''', '''''' + USER_NAME(@user_id) + ''''''''
        EXEC (@cmd)
        FETCH c1 INTO @user_id
    END

    CLOSE c1
    DEALLOCATE c1
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Applications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_Applications](
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ApplicationName] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Applications]') AND name = N'aspnet_Applications_Index')
CREATE CLUSTERED INDEX [aspnet_Applications_Index] ON [dbo].[aspnet_Applications] 
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserGroup](
	[BaseItemID] [int] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[IsInviteAccepted] [bit] NOT NULL,
	[IsApprovedByOwner] [bit] NOT NULL,
 CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserBaseItemRating]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserBaseItemRating](
	[UserID] [uniqueidentifier] NOT NULL,
	[BaseItemID] [int] NOT NULL,
	[Rating] [int] NOT NULL,
 CONSTRAINT [PK_UserBaseItemRating] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[BaseItemID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Comment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Comment](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Text] [text] NOT NULL,
	[PostDateTime] [datetime] NOT NULL,
	[BaseItemID] [int] NOT NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_Membership](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL DEFAULT ((0)),
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[MobilePIN] [nvarchar](16) NULL,
	[Email] [nvarchar](256) NULL,
	[LoweredEmail] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership]') AND name = N'aspnet_Membership_index')
CREATE CLUSTERED INDEX [aspnet_Membership_index] ON [dbo].[aspnet_Membership] 
(
	[ApplicationId] ASC,
	[LoweredEmail] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_Profile](
	[UserId] [uniqueidentifier] NOT NULL,
	[PropertyNames] [ntext] NOT NULL,
	[PropertyValuesString] [ntext] NOT NULL,
	[PropertyValuesBinary] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Profile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Profile](
	[UserID] [uniqueidentifier] NOT NULL,
	[FirstName] [varchar](255) NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[Gender] [int] NULL,
	[DOB] [datetime] NULL,
	[WindowsLiveUUID] [varchar](255) NULL,
	[RSSFeedURL] [varchar](255) NULL,
	[BaseItemID] [int] NOT NULL,
	[MessengerPresenceID] [varchar](255) NOT NULL,
	[DomainToken] [varchar](500) NOT NULL CONSTRAINT [DF_Profile_DomainToken]  DEFAULT (''),
	[OwnerHandle] [varchar](255) NOT NULL CONSTRAINT [DF_Profile_OwnerHandle]  DEFAULT (''),
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]') AND name = N'aspnet_UsersInRoles_index')
CREATE NONCLUSTERED INDEX [aspnet_UsersInRoles_index] ON [dbo].[aspnet_UsersInRoles] 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_PersonalizationPerUser](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[PathId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser]') AND name = N'aspnet_PersonalizationPerUser_index1')
CREATE UNIQUE CLUSTERED INDEX [aspnet_PersonalizationPerUser_index1] ON [dbo].[aspnet_PersonalizationPerUser] 
(
	[PathId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser]') AND name = N'aspnet_PersonalizationPerUser_ncindex2')
CREATE UNIQUE NONCLUSTERED INDEX [aspnet_PersonalizationPerUser_ncindex2] ON [dbo].[aspnet_PersonalizationPerUser] 
(
	[UserId] ASC,
	[PathId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAllUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_PersonalizationAllUsers](
	[PathId] [uniqueidentifier] NOT NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BaseItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BaseItem](
	[BaseItemID] [int] IDENTITY(1,1) NOT NULL,
	[ItemType] [varchar](50) NOT NULL,
	[TagList] [text] NOT NULL,
	[LocationID] [uniqueidentifier] NOT NULL,
	[TotalViews] [int] NOT NULL,
	[TotalRatingScore] [float] NOT NULL,
	[TotalRatingCount] [int] NOT NULL,
	[AverageRating]  AS (coalesce([TotalRatingScore]/nullif([TotalRatingCount],(0)),(0))),
	[Title] [nvarchar](255) NOT NULL,
	[Description] [ntext] NOT NULL,
	[OwnerUserID] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[SubType] [varchar](50) NOT NULL,
	[PrivacyLevel] [int] NOT NULL,
	[ThumbnailBits] [varbinary](max) NULL,
	[IsApproved] [bit] NOT NULL CONSTRAINT [DF_BaseItem_IsApproved]  DEFAULT ((1)),
	[ImageURL] [varchar](255) NOT NULL,
 CONSTRAINT [PK_BaseItem] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BaseItemTag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BaseItemTag](
	[BaseItemID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
 CONSTRAINT [PK_BaseItemTag] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC,
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Media]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Media](
	[BaseItemID] [int] NOT NULL,
	[AlbumBaseItemID] [int] NOT NULL,
 CONSTRAINT [PK_Media_1] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ThumbnailCache]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ThumbnailCache](
	[BaseItemID] [int] NOT NULL,
	[Bits] [image] NOT NULL,
	[MaximumWidth] [int] NOT NULL,
	[MaximumHeight] [int] NOT NULL,
	[LastAccessedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_MediaThumbnail] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC,
	[MaximumWidth] ASC,
	[MaximumHeight] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Collection]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Collection](
	[BaseItemID] [int] NOT NULL,
 CONSTRAINT [PK_Collection] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BaseItemAssociation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BaseItemAssociation](
	[BaseItemID] [int] NOT NULL,
	[AssociatedBaseItemID] [int] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Group]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Group](
	[BaseItemID] [int] NOT NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CollectionItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CollectionItem](
	[BaseItemID] [int] NOT NULL,
	[CollectionBaseItemID] [int] NOT NULL,
 CONSTRAINT [PK_CollectionItem] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Forum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Forum](
	[BaseItemID] [int] NOT NULL,
 CONSTRAINT [PK_Forum] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PendingEmailInvites]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PendingEmailInvites](
	[BaseItemID] [int] NOT NULL,
	[Email] [varchar](64) NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_Users](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[UserName] [nvarchar](256) NOT NULL,
	[LoweredUserName] [nvarchar](256) NOT NULL,
	[MobileAlias] [nvarchar](16) NULL DEFAULT (NULL),
	[IsAnonymous] [bit] NOT NULL DEFAULT ((0)),
	[LastActivityDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users]') AND name = N'aspnet_Users_Index')
CREATE UNIQUE CLUSTERED INDEX [aspnet_Users_Index] ON [dbo].[aspnet_Users] 
(
	[ApplicationId] ASC,
	[LoweredUserName] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users]') AND name = N'aspnet_Users_Index2')
CREATE NONCLUSTERED INDEX [aspnet_Users_Index2] ON [dbo].[aspnet_Users] 
(
	[ApplicationId] ASC,
	[LastActivityDate] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Paths]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_Paths](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[Path] [nvarchar](256) NOT NULL,
	[LoweredPath] [nvarchar](256) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Paths]') AND name = N'aspnet_Paths_index')
CREATE UNIQUE CLUSTERED INDEX [aspnet_Paths_index] ON [dbo].[aspnet_Paths] 
(
	[ApplicationId] ASC,
	[LoweredPath] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[aspnet_Roles](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[RoleName] [nvarchar](256) NOT NULL,
	[LoweredRoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles]') AND name = N'aspnet_Roles_index1')
CREATE UNIQUE CLUSTERED INDEX [aspnet_Roles_index1] ON [dbo].[aspnet_Roles] 
(
	[ApplicationId] ASC,
	[LoweredRoleName] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Event]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Event](
	[BaseItemID] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[BaseItemID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_DeleteInactiveProfiles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Profile_DeleteInactiveProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
    BEGIN
        SELECT  0
        RETURN
    END

    DELETE
    FROM    dbo.aspnet_Profile
    WHERE   UserId IN
            (   SELECT  UserId
                FROM    dbo.aspnet_Users u
                WHERE   ApplicationId = @ApplicationId
                        AND (LastActivityDate <= @InactiveSinceDate)
                        AND (
                                (@ProfileAuthOptions = 2)
                             OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                             OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                            )
            )

    SELECT  @@ROWCOUNT
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
    BEGIN
        SELECT 0
        RETURN
    END

    SELECT  COUNT(*)
    FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p
    WHERE   ApplicationId = @ApplicationId
        AND u.UserId = p.UserId
        AND (LastActivityDate <= @InactiveSinceDate)
        AND (
                (@ProfileAuthOptions = 2)
                OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
            )
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_IsUserInRole]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_IsUserInRole]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(2)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    DECLARE @RoleId uniqueidentifier
    SELECT  @RoleId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(2)

    SELECT  @RoleId = RoleId
    FROM    dbo.aspnet_Roles
    WHERE   LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
        RETURN(3)

    IF (EXISTS( SELECT * FROM dbo.aspnet_UsersInRoles WHERE  UserId = @UserId AND RoleId = @RoleId))
        RETURN(1)
    ELSE
        RETURN(0)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_GetRolesForUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetRolesForUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(1)

    SELECT r.RoleName
    FROM   dbo.aspnet_Roles r, dbo.aspnet_UsersInRoles ur
    WHERE  r.RoleId = ur.RoleId AND r.ApplicationId = @ApplicationId AND ur.UserId = @UserId
    ORDER BY r.RoleName
    RETURN (0)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_AddUsersToRoles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_AddUsersToRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000),
	@CurrentTimeUtc   datetime
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)
	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames	table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles	table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers	table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num		int
	DECLARE @Pos		int
	DECLARE @NextPos	int
	DECLARE @Name		nvarchar(256)

	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N'','', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		SELECT TOP 1 Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END

	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1

	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N'','', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		DELETE FROM @tbNames
		WHERE LOWER(Name) IN (SELECT LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE au.UserId = u.UserId)

		INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
		  SELECT @AppId, NEWID(), Name, LOWER(Name), 0, @CurrentTimeUtc
		  FROM   @tbNames

		INSERT INTO @tbUsers
		  SELECT  UserId
		  FROM	dbo.aspnet_Users au, @tbNames t
		  WHERE   LOWER(t.Name) = au.LoweredUserName AND au.ApplicationId = @AppId
	END

	IF (EXISTS (SELECT * FROM dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr WHERE tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId))
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr, aspnet_Users u, aspnet_Roles r
		WHERE		u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	INSERT INTO dbo.aspnet_UsersInRoles (UserId, RoleId)
	SELECT UserId, RoleId
	FROM @tbUsers, @tbRoles

	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000)
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)


	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames  table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles  table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers  table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num	  int
	DECLARE @Pos	  int
	DECLARE @NextPos  int
	DECLARE @Name	  nvarchar(256)
	DECLARE @CountAll int
	DECLARE @CountU	  int
	DECLARE @CountR	  int


	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N'','', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId
	SELECT @CountR = @@ROWCOUNT

	IF (@CountR <> @Num)
	BEGIN
		SELECT TOP 1 N'''', Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END


	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1


	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N'','', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	SELECT @CountU = @@ROWCOUNT
	IF (@CountU <> @Num)
	BEGIN
		SELECT TOP 1 Name, N''''
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT au.LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE u.UserId = au.UserId)

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(1)
	END

	SELECT  @CountAll = COUNT(*)
	FROM	dbo.aspnet_UsersInRoles ur, @tbUsers u, @tbRoles r
	WHERE   ur.UserId = u.UserId AND ur.RoleId = r.RoleId

	IF (@CountAll <> @CountU * @CountR)
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 @tbUsers tu, @tbRoles tr, dbo.aspnet_Users u, dbo.aspnet_Roles r
		WHERE		 u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND
					 tu.UserId NOT IN (SELECT ur.UserId FROM dbo.aspnet_UsersInRoles ur WHERE ur.RoleId = tr.RoleId) AND
					 tr.RoleId NOT IN (SELECT ur.RoleId FROM dbo.aspnet_UsersInRoles ur WHERE ur.UserId = tu.UserId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	DELETE FROM dbo.aspnet_UsersInRoles
	WHERE UserId IN (SELECT UserId FROM @tbUsers)
	  AND RoleId IN (SELECT RoleId FROM @tbRoles)
	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        ' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_GetUsersInRoles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetUsersInRoles]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId
    ORDER BY u.UserName
    RETURN(0)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_FindUsersInRole]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_FindUsersInRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256),
    @UserNameToMatch  nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId AND LoweredUserName LIKE LOWER(@UserNameToMatch)
    ORDER BY u.UserName
    RETURN(0)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_AnyDataInTables]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_AnyDataInTables]
    @TablesToCheck int
AS
BEGIN
    -- Check Membership table if (@TablesToCheck & 1) is set
    IF ((@TablesToCheck & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N''vw_aspnet_MembershipUsers'') AND (type = ''V''))))
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Membership))
        BEGIN
            SELECT N''aspnet_Membership''
            RETURN
        END
    END

    -- Check aspnet_Roles table if (@TablesToCheck & 2) is set
    IF ((@TablesToCheck & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N''vw_aspnet_Roles'') AND (type = ''V''))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 RoleId FROM dbo.aspnet_Roles))
        BEGIN
            SELECT N''aspnet_Roles''
            RETURN
        END
    END

    -- Check aspnet_Profile table if (@TablesToCheck & 4) is set
    IF ((@TablesToCheck & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N''vw_aspnet_Profiles'') AND (type = ''V''))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Profile))
        BEGIN
            SELECT N''aspnet_Profile''
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 8) is set
    IF ((@TablesToCheck & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N''vw_aspnet_WebPartState_User'') AND (type = ''V''))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_PersonalizationPerUser))
        BEGIN
            SELECT N''aspnet_PersonalizationPerUser''
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 16) is set
    IF ((@TablesToCheck & 16) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N''aspnet_WebEvent_LogEvent'') AND (type = ''P''))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 * FROM dbo.aspnet_WebEvent_Events))
        BEGIN
            SELECT N''aspnet_WebEvent_Events''
            RETURN
        END
    END

    -- Check aspnet_Users table if (@TablesToCheck & 1,2,4 & 8) are all set
    IF ((@TablesToCheck & 1) <> 0 AND
        (@TablesToCheck & 2) <> 0 AND
        (@TablesToCheck & 4) <> 0 AND
        (@TablesToCheck & 8) <> 0 AND
        (@TablesToCheck & 32) <> 0 AND
        (@TablesToCheck & 128) <> 0 AND
        (@TablesToCheck & 256) <> 0 AND
        (@TablesToCheck & 512) <> 0 AND
        (@TablesToCheck & 1024) <> 0)
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Users))
        BEGIN
            SELECT N''aspnet_Users''
            RETURN
        END
        IF (EXISTS(SELECT TOP 1 ApplicationId FROM dbo.aspnet_Applications))
        BEGIN
            SELECT N''aspnet_Applications''
            RETURN
        END
    END
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_Users]'))
EXEC dbo.sp_executesql @statement = N'
  CREATE VIEW [dbo].[vw_aspnet_Users]
  AS SELECT [dbo].[aspnet_Users].[ApplicationId], [dbo].[aspnet_Users].[UserId], [dbo].[aspnet_Users].[UserName], [dbo].[aspnet_Users].[LoweredUserName], [dbo].[aspnet_Users].[MobileAlias], [dbo].[aspnet_Users].[IsAnonymous], [dbo].[aspnet_Users].[LastActivityDate]
  FROM [dbo].[aspnet_Users]
  ' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByName]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier

    IF (@UpdateLastActivity = 1)
    BEGIN
        SELECT TOP 1 m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, @CurrentTimeUtc, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1

        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        WHERE    @UserId = UserId
    END
    ELSE
    BEGIN
        SELECT TOP 1 m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1
    END

    RETURN 0
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByUserId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByUserId]
    @UserId               uniqueidentifier,
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    IF ( @UpdateLastActivity = 1 )
    BEGIN
        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        FROM     dbo.aspnet_Users
        WHERE    @UserId = UserId

        IF ( @@ROWCOUNT = 0 ) -- User ID not found
            RETURN -1
    END

    SELECT  m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate, m.LastLoginDate, u.LastActivityDate,
            m.LastPasswordChangedDate, u.UserName, m.IsLockedOut,
            m.LastLockoutDate
    FROM    dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   @UserId = u.UserId AND u.UserId = m.UserId

    IF ( @@ROWCOUNT = 0 ) -- User ID not found
       RETURN -1

    RETURN 0
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByEmail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByEmail]
    @ApplicationName  nvarchar(256),
    @Email            nvarchar(256)
AS
BEGIN
    IF( @Email IS NULL )
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                m.LoweredEmail IS NULL
    ELSE
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                LOWER(@Email) = m.LoweredEmail

    IF (@@rowcount = 0)
        RETURN(1)
    RETURN(0)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetPasswordWithFormat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetPasswordWithFormat]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @UpdateLastLoginActivityDate    bit,
    @CurrentTimeUtc                 datetime
AS
BEGIN
    DECLARE @IsLockedOut                        bit
    DECLARE @UserId                             uniqueidentifier
    DECLARE @Password                           nvarchar(128)
    DECLARE @PasswordSalt                       nvarchar(128)
    DECLARE @PasswordFormat                     int
    DECLARE @FailedPasswordAttemptCount         int
    DECLARE @FailedPasswordAnswerAttemptCount   int
    DECLARE @IsApproved                         bit
    DECLARE @LastActivityDate                   datetime
    DECLARE @LastLoginDate                      datetime

    SELECT  @UserId          = NULL

    SELECT  @UserId = u.UserId, @IsLockedOut = m.IsLockedOut, @Password=Password, @PasswordFormat=PasswordFormat,
            @PasswordSalt=PasswordSalt, @FailedPasswordAttemptCount=FailedPasswordAttemptCount,
		    @FailedPasswordAnswerAttemptCount=FailedPasswordAnswerAttemptCount, @IsApproved=IsApproved,
            @LastActivityDate = LastActivityDate, @LastLoginDate = LastLoginDate
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF (@UserId IS NULL)
        RETURN 1

    IF (@IsLockedOut = 1)
        RETURN 99

    SELECT   @Password, @PasswordFormat, @PasswordSalt, @FailedPasswordAttemptCount,
             @FailedPasswordAnswerAttemptCount, @IsApproved, @LastLoginDate, @LastActivityDate

    IF (@UpdateLastLoginActivityDate = 1 AND @IsApproved = 1)
    BEGIN
        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @CurrentTimeUtc
        WHERE   UserId = @UserId

        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @CurrentTimeUtc
        WHERE   @UserId = UserId
    END


    RETURN 0
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScrubPII]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ScrubPII]

AS

BEGIN
	SET NOCOUNT ON;

	-- create the containing table
	CREATE TABLE #FakeNames (
		FirstName varchar(100),
		Lastname varchar(100)
	)

	-- populate the table with the legal approved fake names
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jyothi'', ''Pai'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dominik'', ''Paiha'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Aaron M.'', ''Painter'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Wilson'', ''Pais'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jae B.'', ''Pak'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rok'', ''Palčič'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Punya'', ''Palit'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tude'', ''Palma'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mingda'', ''Pan'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Stig'', ''Panduro'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Prakash'', ''Paramasivam'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Robert'', ''Parham'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dan'', ''Park'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Darren'', ''Parker'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Alfons'', ''Parovszky'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tiit'', ''Parts'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dorena'', ''Paschke'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rajesh M.'', ''Patel'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Reshma'', ''Patel'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sanjay'', ''Patel'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Pannarat'', ''Pattanapitakkul'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michael'', ''Patten'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Leonids'', ''Paturskis'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ana'', ''Pavicic'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Prakash'', ''Pazamalai'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Simon'', ''Pearson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Flemming'', ''Pedersen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Yael'', ''Peled'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''David'', ''Pelton'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Yun-Feng'', ''Peng'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Laurent'', ''Penisson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lori'', ''Penor'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lionel'', ''Penuchot'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''John'', ''Peoples'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michel'', ''Pereira'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Francisco'', ''Pérez-Olaeta'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tom'', ''Perham'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Brian'', ''Perry'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Stanislav'', ''Pesotsky'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Wirote'', ''Petchdenlarp'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Cristian'', ''Petculescu'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Palle'', ''Petersen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Pierangela'', ''Pezzini'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michael'', ''Pfeiffer'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Terrence'', ''Philip'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Carol'', ''Philips'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jeff'', ''Phillips'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mike'', ''Phipps'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tiffany'', ''Phuvasate'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Chaiwat'', ''Piamkulawanich'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michal'', ''Piaseczny'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Guido'', ''Pica'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Pilar'', ''Pinilla'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Armando'', ''Pinto'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Worapon'', ''Pitayaphongpat'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tanja'', ''Plate'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Craig'', ''Playstead'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Christiane'', ''Ploenes'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Idan'', ''Plonsky'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ekaterina'', ''Podkolzina'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Toni'', ''Poe'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Grigory'', ''Pogulsky'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Carole'', ''Poland'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michał'', ''Poliszkiewicz'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Josh'', ''Pollock'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Laura Steele'', ''Polly'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Suraj'', ''Poozhiyil'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Darya'', ''Popkova'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Daphna'', ''Porath'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Juha-Pekka'', ''Posti'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Matej'', ''Potokar'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Cristina'', ''Potra'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Houman'', ''Pournasseh'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Balázs'', ''Pozsony'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Chris'', ''Preston'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Diane'', ''Prescott'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jeff'', ''Price'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Julian'', ''Price'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Vernette'', ''Price'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''David'', ''Probst'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Urszula'', ''Ptak-Małysiak'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jim'', ''Ptaszynski'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Muniraju'', ''Pulipalyam'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sean'', ''Purcell'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Monika'', ''Radzikowska-Strzeżek'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jesper'', ''Ræbild'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Amritansh'', ''Raghav'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michael'', ''Raheem'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Stuart'', ''Railson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kim'', ''Ralls'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Prithvi'', ''Raj'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Luciana'', ''Ramos'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Yossi'', ''Ran'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Markus'', ''Rankenburg'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Arvind B.'', ''Rao'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Simon'', ''Rapier'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rui'', ''Raposo'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Morten'', ''Rasmussen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Andrew'', ''Rath'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lene'', ''Rathkjen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mike'', ''Ray'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rodrigo'', ''Ready'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''April'', ''Reagan'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jamie'', ''Reding'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Barak'', ''Regev'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Miles'', ''Reid'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tsvi '', ''Reiter'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Per'', ''Reitzel'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Enoch T.'', ''Remick'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Olivier'', ''Renaud'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Bjorn'', ''Rettig'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jeff'', ''Ressler'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Delphine'', ''Ribaute'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Don'', ''Richardson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Roy'', ''Richardson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Shawn'', ''Richardson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Wendy'', ''Richardson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jack S.'', ''Richins'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Martin'', ''Rienstra'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ralf'', ''Riethmüller'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Anders'', ''Riis'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Bjarne'', ''Riis'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Steve'', ''Riley'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Titti'', ''Ringstrom'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Stefan'', ''Rißling'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Gretchen'', ''Rivas'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Juan-Carlos'', ''Rivas'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Stuart'', ''Rivchun'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Arif'', ''Rizaldy'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Alex'', ''Robinson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Scott'', ''Rockfeld'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''John'', ''Rodman'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Don'', ''Roessler'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Alex'', ''Roland'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Marek'', ''Rolecki'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kelly'', ''Rollin'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Daniel'', ''Roman'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tali'', ''Roth'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michael'', ''Rottbøll'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sebastian'', ''Röttel'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rajesh'', ''Rotti'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Robert'', ''Rounthwaite'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dag'', ''Rovik'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Pablo'', ''Rovira Diez'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Todd'', ''Rowe'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sabine'', ''Royant'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Idan'', ''Rubin'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Erik'', ''Rucker'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Raimonds'', ''Rudmanis'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michael'', ''Ruggiero'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Pedro'', ''Ruivo'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rafał'', ''Rumian'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Amy'', ''Rusko'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Davide'', ''Ruspini'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Giuseppe'', ''Russo'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Andy'', ''Ruth'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Erik'', ''Ryan'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Artur'', ''Rybka'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Joanna'', ''Rybka'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Christian'', ''Rytt'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lane'', ''Sacksteder'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Peter'', ''Saddow'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Malvin'', ''Seale'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Suriya'', ''Sajjateerakool'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tamer'', ''Salah'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Karolina'', ''Sałas-Szlejter'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sharon'', ''Salavaria'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dina'', ''Salimzianova'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Teemu'', ''Sallinen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ivo '', ''Salmre'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Raymond '', ''Sam'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mandar'', ''Samant'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Prasanna'', ''Samarawickrama'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Patricia'', ''San Juan'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ken'', ''Sánchez'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Yolanda'', ''Sánchez'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mikael'', ''Sandberg'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Giraldo Hierro'', ''Sandoval'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Patrick'', ''Sands'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Barbara'', ''Sankovic'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dubravka'', ''Salopek'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Beata'', ''Sapieżyńska'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''José '', ''Saraiva'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''K.'', ''Saravana'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Esko'', ''Sario'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Raman'', ''Sarin'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tihomir'', ''Sasic'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rie'', ''Sasaki'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michiyo'', ''Sato'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Naoki'', ''Sato'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rimvydas'', ''Saulis'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kazuki'', ''Sawanoi'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ciam'', ''Sawyer'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dennis'', ''Saylor'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jim'', ''Scardelis'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Peter'', ''Scepanek'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''George'', ''Schaller'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Gary'', ''Schare'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Boris'', ''Scholl'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Thorsten'', ''Scholl'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Janet'', ''Schorr'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Andreas'', ''Schou'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Steve'', ''Schmidt'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Martin'', ''Schmöllerl'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Karsten'', ''Schopp'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sabina'', ''Schütz'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Juergen'', ''Schwertl'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Elisabetta'', ''Scotti'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Scott'', ''Seely'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Nimrod'', ''Segev'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Joachim'', ''Seidler'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Vitore'', ''Selca'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Chris'', ''Sells'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mariya'', ''Sergienko'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Albert Cabello'', ''Serrano'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Roxanne'', ''Seubert'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Miguel'', ''Severino'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rostislav'', ''Shabalin'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Naji'', ''Shaban'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Osama'', ''Shabaneh'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Niraj'', ''Shah'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Parul'', ''Shah'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sanjay'', ''Shah'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Paul'', ''Shakespear'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Uri'', ''Shalit'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ashvini'', ''Sharma'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ruslan'', ''Shashkov'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rhae-Christie'', ''Shaw'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Alan'', ''Shen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Paul'', ''Shen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kazuto'', ''Shibata'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Daniel'', ''Shimshoni'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Brett'', ''Shirley'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Patrick'', ''Shortt'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jill'', ''Shrader'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Shish'', ''Shridhar'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Eran'', ''Shtiegman'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Hallur '', ''Sigurdarson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Alexandre'', ''Silva'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Edmund'', ''Silva'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Anav'', ''Silverman'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Iliana'', ''Simbaeva'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Britta'', ''Simon'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''David'', ''Simpson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Katherine'', ''Simpson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Pavel'', ''Simsa'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Andrew'', ''Sinclair'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Gurinder'', ''Singh'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Paul'', ''Singh'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ashima'', ''Singhal'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Soumya'', ''Singhi'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sigal'', ''Sivan-Yidov'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Glenn'', ''Sjöblom'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Bonnie L.'', ''Skelly'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Morgan'', ''Skinner'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Egil Ivar'', ''Skjold'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Anders B.'', ''Skjønaa'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dale'', ''Sleppy'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Peter'', ''Sloth'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Randy'', ''Slycord'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ben'', ''Smith'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Brian'', ''Smith'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Denise'', ''Smith'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jeff'', ''Smith'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''John'', ''Smith'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ronaldo'', ''Smith Jr.'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Samantha'', ''Smith'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tony'', ''Smith'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lorrin G.'', ''Smith-Bates'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Guntars'', ''Smits'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Timothy (Tim)'', ''Sneath'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Derek'', ''Snyder'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''David'', ''So'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Philip'', ''Søborg'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Neven'', ''Sokec'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ajay'', ''Solanki'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Antonio Gratacos'', ''Solsona'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lolan'', ''Song'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Yukako'', ''Souza'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Anibal'', ''Sousa'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Fernando'', ''Sousa'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Leonilde Glória'', ''Sousa'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Luis'', ''Sousa'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ryan'', ''Spanton'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Melanie'', ''Speckmann'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Phil'', ''Spencer'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Koos'', ''Splinter'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Martin'', ''Spona'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Christof'', ''Sprenger'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mahesh'', ''Sreenivas'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sethu'', ''Srinivasan'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Merav'', ''Sror'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Roman'', ''Stachnio'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Betsy'', ''Stadick'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jeff'', ''Stammler'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Elke'', ''Stangl'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Andrzej'', ''Stąpor'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Darius'', ''Stasevicius'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mark'', ''Steele'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Heidi'', ''Steen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Stefano'', ''Stefani'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kim'', ''Steffensen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Victor'', ''Stehmann'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Alan'', ''Steiner'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dominik'', ''Steiner'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jaka'', ''Stele'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Max'', ''Stevens'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Charlotte'', ''Stevnsborg'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''April'', ''Stewart'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Florian'', ''Stiller'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jared'', ''Stivers'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Anne-Mette'', ''Stoltze'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Amy'', ''Strande'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Bostjan'', ''Strazar'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Susanna'', ''Stubberod'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Gerrit Jan'', ''Straatsma'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Stig'', ''Struve-Christensen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Muru'', ''Subramani'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ananth'', ''Sundararaj'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michael'', ''Sullivan'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Nate'', ''Sun'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Vijay'', ''Sundaram'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Krishna'', ''Sunkammurali'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ari'', ''Suominen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Brad'', ''Sutton'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kern'', ''Sutton'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Paul'', ''Suurs'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Peter'', ''Svensk'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Eric'', ''Swift'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Manoj'', ''Syamala'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lonnie'', ''Sylvest'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Gerhard'', ''Sziber'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Oliver'', ''Szimmetat'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Radosław'', ''Szymczak'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Greta'', ''Szypuła'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Roberto'', ''Tamburello'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Marco'', ''Tanara'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kate'', ''Taneyhill'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rakesh'', ''Tangirala'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Daniel P.'', ''Taylor'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Maurice'', ''Taylor'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sameer A.'', ''Tejani'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jeff'', ''Teper'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Brian'', ''Teutsch'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Abhijit'', ''Thakur'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Bernard'', ''Tham'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ram'', ''Thirunavukkarasu'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Peder'', ''Thode'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sandhya'', ''Thodla'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tien Dung'', ''Thoi'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Iben'', ''Thorell'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Alicia'', ''Thornber'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Elvar'', ''Thorkelsson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Justin'', ''Thorp'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Steven'', ''Thorpe'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mike'', ''Tiano'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Diane'', ''Tibbott'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Danielle'', ''Tiedt'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ian'', ''Tien'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Hung-Fu (Tony)'', ''Ting'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''John'', ''Tippett'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tsing-Mun'', ''To'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Teodor'', ''Todorov'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Karen'', ''Toh'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lisa'', ''Toftemark'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Bjørn'', ''Tollevsen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dragan'', ''Tomiċ'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Diego Arteaga'', ''Torreira'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''András'', ''Tóth'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Louise'', ''Toubro'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tim'', ''Toyoshima'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Shirleen H.'', ''Travers'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lertchai'', ''Treetawatchaiwong'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Henning'', ''Troelsen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Carol'', ''Troup'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kim'', ''Truelsen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Marcel'', ''Truempy'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Marcelo'', ''Truffat'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Adam'', ''Trukawka'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Steffen'', ''Tschimmel'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lynn'', ''Tsoflias'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Lance'', ''Tucker'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michael'', ''Tucker'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Alexander'', ''Tumanov'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sittichai'', ''Tuntisangaroon'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Richard'', ''Tupy'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Olinda'', ''Turner'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Richard'', ''Turner'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sairaj'', ''Uddin'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Pieter'', ''Uittenbogaard'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Hans-Walter'', ''Untch'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sunil'', ''Uppal'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Erik'', ''Ušaj'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Valery'', ''Ushakov'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Robert'', ''Uttecht'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Shachaf'', ''Vainer-Eytan'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rene'', ''Valdes'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rachel '', ''Valdez'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Brian'', ''Valentine'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Esther'', ''Valle'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Eva'', ''Valverde'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''James'', ''van Eaton'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Roger'', ''Van Houten'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Luc'', ''van Vugt'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michael'', ''Vanderhyde'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Garrett'', ''Vargas'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tanja'', ''Varvikko'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Catherine'', ''Vegher'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Siri'', ''Velauthapillai'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Elena'', ''Velez Amezaga'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tjeerd'', ''Veninga'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Raja D.'', ''Venugopal'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kevin'', ''Verboort'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ágnes'', ''Verebélyi'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Gregory'', ''Verny'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Giorgio'', ''Veronesi'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Birgitte'', ''Vestphael'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Nina'', ''Vietzen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Fred'', ''Viidul'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Peter'', ''Villadsen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Aldis'', ''Vilums'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Allan'', ''Vinther-Wahl'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jelle'', ''Visser'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Davy'', ''Vliegen'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Viktor'', ''Volodin'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''William'', ''Vong'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Florian'', ''Voss'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Connie'', ''Vrettos'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Yury'', ''Vronsky'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Sandra'', ''Vujcic'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Roland'', ''Wacker'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rohinton'', ''Wadia'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Darren'', ''Waite'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Anne'', ''Wallace'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Felhofer'', ''Walter'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rob'', ''Walters'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jeff'', ''Wang'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jian Shuo'', ''Wang'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Qiang'', ''Wang'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tony'', ''Wang'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Thiti'', ''Wang-Aryattawanich'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jason'', ''Warren'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jason M.'', ''Watters'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Peter'', ''Waxman'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kelly J.'', ''Weadock'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Gregory'', ''Weber'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Martin'', ''Weber'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Anne'', ''Weiler'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Cornelia'', ''Weiler'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Walter'', ''Weinfurter'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Thorsten'', ''Weinrich'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Daniel'', ''Weisman'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Charlotte'', ''Weiss'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Brian'', ''Welcker'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Peter'', ''Wenauer'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Maira'', ''Wenzel'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Bart'', ''Wessels'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Paul'', ''West'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Pia'', ''Westermann'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Wendy'', ''Wheeler'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jim'', ''Wickham'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Marianne'', ''Wier'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Colin'', ''Wilcox'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jeff'', ''Williams'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dan'', ''Wilson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jim'', ''Wilson'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Steve'', ''Winfield'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Roland'', ''Winkler'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Greg'', ''Winston'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Aneta'', ''Wiśniewska'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mark'', ''Wistrom'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rainer'', ''Witte'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''James'', ''Wittrell'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Mariusz'', ''Wolodzko'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Y. L.'', ''Wong'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Zac'', ''Woodall'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''John'', ''Woods'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Agnieszka'', ''Wojciechowska'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michiel'', ''Wories'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''David'', ''Wright'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Steven M.'', ''Wright'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Qiong'', ''Wu'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Pieter'', ''Wycoff'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ming-Yang'', ''Xie'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ye'', ''Xu'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Ioannis'', ''Xylaras'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Shmuel'', ''Yair'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''David'', ''Yalovsky'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Makoto'', ''Yamagishi'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Maki'', ''Yamane'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Chen'', ''Yang'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Shengda'', ''Yang'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Arthur'', ''Yasinski'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Eiji'', ''Yasuda'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Yaron'', ''Yedida'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tai'', ''Yee'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''John'', ''Yokim'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Norimichi'', ''Yonekura'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Garrett'', ''Young'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Rob'', ''Young'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Robin'', ''Young'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Tom'', ''Youtsey'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Wei'', ''Yu'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Gary W.'', ''Yukish'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Greg'', ''Yvkoff'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Eugene'', ''Zabokritski'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Marko'', ''Zajc'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Kerstin'', ''Zakardissnehf'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Amr'', ''Zaki'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Robert'', ''Zare'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michael'', ''Zeman'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Jeffrey'', ''Zeng'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Frank'', ''Zhang'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Larry'', ''Zhang'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Barbara'', ''Zighetti'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Dieter'', ''Zilch'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Karin'', ''Zimprich'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Erwin'', ''Zischka'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Annelie'', ''Zuber'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Markus'', ''Zulechner'')
	INSERT INTO #FakeNames(Firstname, Lastname) VALUES(''Michael '', ''Zwilling'')

	-- declare the cursor
	DECLARE MY_CURSOR Cursor

	FOR 
	Select UserID, FirstName, LastName
	From dbo.Profile

	-- open the cursor
	Open My_Cursor
		-- declare the variables
		DECLARE @VAR_UserID UniqueIdentifier, @VAR_FirstName varchar(100), @VAR_LastName varchar(100)

		-- fetch the first row
		Fetch NEXT FROM MY_Cursor INTO @VAR_UserID, @VAR_FirstName, @VAR_LastName
		
		-- while we aren''t at the end
		While (@@FETCH_STATUS <> -1)
			BEGIN
				IF (@@FETCH_STATUS <> -2)

				-- Update the profile
				UPDATE	dbo.Profile
				SET		FirstName = (select top 1 firstname from #FakeNames order by newid()),
						LastName = (select top 1 LastName from #FakeNames order by newid())
				WHERE	UserID = @VAR_UserID

				-- fetch the next row
				Fetch NEXT FROM MY_Cursor INTO @VAR_UserID, @VAR_FirstName, @VAR_LastName
			END

	CLOSE MY_CURSOR

	DEALLOCATE MY_CURSOR

	drop table #FakeNames
	
	-- Update the usernames
	UPDATE	dbo.aspnet_Users
	SET		UserName =
	(
		select	FirstName + LastName
		from	dbo.Profile
		Where	dbo.Profile.UserID = dbo.aspnet_Users.UserID
	)

	-- reset the lowered names
	UPDATE	dbo.aspnet_Users
	SET		LoweredUserName = lower(username)

	-- replace the email addresses
	UPDATE [dbo].[aspnet_Membership]
	SET 
	[Email] = ''alogan@microsoft.com''
	,[LoweredEmail] = ''alogan@microsoft.com''

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users_CreateUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Users_CreateUser]
    @ApplicationId    uniqueidentifier,
    @UserName         nvarchar(256),
    @IsUserAnonymous  bit,
    @LastActivityDate DATETIME,
    @UserId           uniqueidentifier OUTPUT
AS
BEGIN
    IF( @UserId IS NULL )
        SELECT @UserId = NEWID()
    ELSE
    BEGIN
        IF( EXISTS( SELECT UserId FROM dbo.aspnet_Users
                    WHERE @UserId = UserId ) )
            RETURN -1
    END

    INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
    VALUES (@ApplicationId, @UserId, @UserName, LOWER(@UserName), @IsUserAnonymous, @LastActivityDate)

    RETURN 0
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users_DeleteUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Users_DeleteUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @TablesToDeleteFrom int,
    @NumTablesDeletedFrom int OUTPUT
AS
BEGIN
    DECLARE @UserId               uniqueidentifier
    SELECT  @UserId               = NULL
    SELECT  @NumTablesDeletedFrom = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    DECLARE @ErrorCode   int
    DECLARE @RowCount    int

    SET @ErrorCode = 0
    SET @RowCount  = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   u.LoweredUserName       = LOWER(@UserName)
        AND u.ApplicationId         = a.ApplicationId
        AND LOWER(@ApplicationName) = a.LoweredApplicationName

    IF (@UserId IS NULL)
    BEGIN
        GOTO Cleanup
    END

    -- Delete from Membership table if (@TablesToDeleteFrom & 1) is set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N''vw_aspnet_MembershipUsers'') AND (type = ''V''))))
    BEGIN
        DELETE FROM dbo.aspnet_Membership WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
               @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_UsersInRoles table if (@TablesToDeleteFrom & 2) is set
    IF ((@TablesToDeleteFrom & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N''vw_aspnet_UsersInRoles'') AND (type = ''V''))) )
    BEGIN
        DELETE FROM dbo.aspnet_UsersInRoles WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Profile table if (@TablesToDeleteFrom & 4) is set
    IF ((@TablesToDeleteFrom & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N''vw_aspnet_Profiles'') AND (type = ''V''))) )
    BEGIN
        DELETE FROM dbo.aspnet_Profile WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_PersonalizationPerUser table if (@TablesToDeleteFrom & 8) is set
    IF ((@TablesToDeleteFrom & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N''vw_aspnet_WebPartState_User'') AND (type = ''V''))) )
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Users table if (@TablesToDeleteFrom & 1,2,4 & 8) are all set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (@TablesToDeleteFrom & 2) <> 0 AND
        (@TablesToDeleteFrom & 4) <> 0 AND
        (@TablesToDeleteFrom & 8) <> 0 AND
        (EXISTS (SELECT UserId FROM dbo.aspnet_Users WHERE @UserId = UserId)))
    BEGIN
        DELETE FROM dbo.aspnet_Users WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:
    SET @NumTablesDeletedFrom = 0

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
	    ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_MembershipUsers]'))
EXEC dbo.sp_executesql @statement = N'
  CREATE VIEW [dbo].[vw_aspnet_MembershipUsers]
  AS SELECT [dbo].[aspnet_Membership].[UserId],
            [dbo].[aspnet_Membership].[PasswordFormat],
            [dbo].[aspnet_Membership].[MobilePIN],
            [dbo].[aspnet_Membership].[Email],
            [dbo].[aspnet_Membership].[LoweredEmail],
            [dbo].[aspnet_Membership].[PasswordQuestion],
            [dbo].[aspnet_Membership].[PasswordAnswer],
            [dbo].[aspnet_Membership].[IsApproved],
            [dbo].[aspnet_Membership].[IsLockedOut],
            [dbo].[aspnet_Membership].[CreateDate],
            [dbo].[aspnet_Membership].[LastLoginDate],
            [dbo].[aspnet_Membership].[LastPasswordChangedDate],
            [dbo].[aspnet_Membership].[LastLockoutDate],
            [dbo].[aspnet_Membership].[FailedPasswordAttemptCount],
            [dbo].[aspnet_Membership].[FailedPasswordAttemptWindowStart],
            [dbo].[aspnet_Membership].[FailedPasswordAnswerAttemptCount],
            [dbo].[aspnet_Membership].[FailedPasswordAnswerAttemptWindowStart],
            [dbo].[aspnet_Membership].[Comment],
            [dbo].[aspnet_Users].[ApplicationId],
            [dbo].[aspnet_Users].[UserName],
            [dbo].[aspnet_Users].[MobileAlias],
            [dbo].[aspnet_Users].[IsAnonymous],
            [dbo].[aspnet_Users].[LastActivityDate]
  FROM [dbo].[aspnet_Membership] INNER JOIN [dbo].[aspnet_Users]
      ON [dbo].[aspnet_Membership].[UserId] = [dbo].[aspnet_Users].[UserId]
  ' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetNumberOfUsersOnline]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetNumberOfUsersOnline]
    @ApplicationName            nvarchar(256),
    @MinutesSinceLastInActive   int,
    @CurrentTimeUtc             datetime
AS
BEGIN
    DECLARE @DateActive datetime
    SELECT  @DateActive = DATEADD(minute,  -(@MinutesSinceLastInActive), @CurrentTimeUtc)

    DECLARE @NumOnline int
    SELECT  @NumOnline = COUNT(*)
    FROM    dbo.aspnet_Users u(NOLOCK),
            dbo.aspnet_Applications a(NOLOCK),
            dbo.aspnet_Membership m(NOLOCK)
    WHERE   u.ApplicationId = a.ApplicationId                  AND
            LastActivityDate > @DateActive                     AND
            a.LoweredApplicationName = LOWER(@ApplicationName) AND
            u.UserId = m.UserId
    RETURN(@NumOnline)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_GetProperties]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Profile_GetProperties]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT @UserId = UserId
    FROM   dbo.aspnet_Users
    WHERE  ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN
    SELECT TOP 1 PropertyNames, PropertyValuesString, PropertyValuesBinary
    FROM         dbo.aspnet_Profile
    WHERE        UserId = @UserId

    IF (@@ROWCOUNT > 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    END
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UpdateUserInfo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUserInfo]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @IsPasswordCorrect              bit,
    @UpdateLastLoginActivityDate    bit,
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @LastLoginDate                  datetime,
    @LastActivityDate               datetime
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @IsApproved                             bit
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @IsApproved = m.IsApproved,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        GOTO Cleanup
    END

    IF( @IsPasswordCorrect = 0 )
    BEGIN
        IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAttemptWindowStart ) )
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = 1
        END
        ELSE
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1
        END

        BEGIN
            IF( @FailedPasswordAttemptCount >= @MaxInvalidPasswordAttempts )
            BEGIN
                SET @IsLockedOut = 1
                SET @LastLockoutDate = @CurrentTimeUtc
            END
        END
    END
    ELSE
    BEGIN
        IF( @FailedPasswordAttemptCount > 0 OR @FailedPasswordAnswerAttemptCount > 0 )
        BEGIN
            SET @FailedPasswordAttemptCount = 0
            SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )
            SET @FailedPasswordAnswerAttemptCount = 0
            SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )
            SET @LastLockoutDate = CONVERT( datetime, ''17540101'', 112 )
        END
    END

    IF( @UpdateLastLoginActivityDate = 1 )
    BEGIN
        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @LastActivityDate
        WHERE   @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END

        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @LastLoginDate
        WHERE   UserId = @UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END


    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
        FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
        FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
        FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
        FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
    WHERE @UserId = UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetPassword]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetPassword]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @PasswordAnswer                 nvarchar(128) = NULL
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @PasswordFormat                         int
    DECLARE @Password                               nvarchar(128)
    DECLARE @passAns                                nvarchar(128)
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @Password = m.Password,
            @passAns = m.PasswordAnswer,
            @PasswordFormat = m.PasswordFormat,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    IF ( NOT( @PasswordAnswer IS NULL ) )
    BEGIN
        IF( ( @passAns IS NULL ) OR ( LOWER( @passAns ) <> LOWER( @PasswordAnswer ) ) )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
        ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )
            END
        END

        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    IF( @ErrorCode = 0 )
        SELECT @Password, @PasswordFormat

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_SetPassword]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_SetPassword]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @NewPassword      nvarchar(128),
    @PasswordSalt     nvarchar(128),
    @CurrentTimeUtc   datetime,
    @PasswordFormat   int = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    UPDATE dbo.aspnet_Membership
    SET Password = @NewPassword, PasswordFormat = @PasswordFormat, PasswordSalt = @PasswordSalt,
        LastPasswordChangedDate = @CurrentTimeUtc
    WHERE @UserId = UserId
    RETURN(0)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_ResetPassword]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_ResetPassword]
    @ApplicationName             nvarchar(256),
    @UserName                    nvarchar(256),
    @NewPassword                 nvarchar(128),
    @MaxInvalidPasswordAttempts  int,
    @PasswordAttemptWindow       int,
    @PasswordSalt                nvarchar(128),
    @CurrentTimeUtc              datetime,
    @PasswordFormat              int = 0,
    @PasswordAnswer              nvarchar(128) = NULL
AS
BEGIN
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @UserId                                 uniqueidentifier
    SET     @UserId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    SELECT @IsLockedOut = IsLockedOut,
           @LastLockoutDate = LastLockoutDate,
           @FailedPasswordAttemptCount = FailedPasswordAttemptCount,
           @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart,
           @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount,
           @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM dbo.aspnet_Membership WITH ( UPDLOCK )
    WHERE @UserId = UserId

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Membership
    SET    Password = @NewPassword,
           LastPasswordChangedDate = @CurrentTimeUtc,
           PasswordFormat = @PasswordFormat,
           PasswordSalt = @PasswordSalt
    WHERE  @UserId = UserId AND
           ( ( @PasswordAnswer IS NULL ) OR ( LOWER( PasswordAnswer ) = LOWER( @PasswordAnswer ) ) )

    IF ( @@ROWCOUNT = 0 )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
    ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )
            END
        END

    IF( NOT ( @PasswordAnswer IS NULL ) )
    BEGIN
        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UnlockUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_UnlockUser]
    @ApplicationName                         nvarchar(256),
    @UserName                                nvarchar(256)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
        RETURN 1

    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = 0,
        FailedPasswordAttemptCount = 0,
        FailedPasswordAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 ),
        FailedPasswordAnswerAttemptCount = 0,
        FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 ),
        LastLockoutDate = CONVERT( datetime, ''17540101'', 112 )
    WHERE @UserId = UserId

    RETURN 0
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UpdateUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUser]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @Email                nvarchar(256),
    @Comment              ntext,
    @IsApproved           bit,
    @LastLoginDate        datetime,
    @LastActivityDate     datetime,
    @UniqueEmail          int,
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId, @ApplicationId = a.ApplicationId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership WITH (UPDLOCK, HOLDLOCK)
                    WHERE ApplicationId = @ApplicationId  AND @UserId <> UserId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            RETURN(7)
        END
    END

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    UPDATE dbo.aspnet_Users WITH (ROWLOCK)
    SET
         LastActivityDate = @LastActivityDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    UPDATE dbo.aspnet_Membership WITH (ROWLOCK)
    SET
         Email            = @Email,
         LoweredEmail     = LOWER(@Email),
         Comment          = @Comment,
         IsApproved       = @IsApproved,
         LastLoginDate    = @LastLoginDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN -1
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]
    @ApplicationName       nvarchar(256),
    @UserName              nvarchar(256),
    @NewPasswordQuestion   nvarchar(256),
    @NewPasswordAnswer     nvarchar(128)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Membership m, dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId
    IF (@UserId IS NULL)
    BEGIN
        RETURN(1)
    END

    UPDATE dbo.aspnet_Membership
    SET    PasswordQuestion = @NewPasswordQuestion, PasswordAnswer = @NewPasswordAnswer
    WHERE  UserId=@UserId
    RETURN(0)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_CheckSchemaVersion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_CheckSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    IF (EXISTS( SELECT  *
                FROM    dbo.aspnet_SchemaVersions
                WHERE   Feature = LOWER( @Feature ) AND
                        CompatibleSchemaVersion = @CompatibleSchemaVersion ))
        RETURN 0

    RETURN 1
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_RegisterSchemaVersion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_RegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128),
    @IsCurrentVersion          bit,
    @RemoveIncompatibleSchema  bit
AS
BEGIN
    IF( @RemoveIncompatibleSchema = 1 )
    BEGIN
        DELETE FROM dbo.aspnet_SchemaVersions WHERE Feature = LOWER( @Feature )
    END
    ELSE
    BEGIN
        IF( @IsCurrentVersion = 1 )
        BEGIN
            UPDATE dbo.aspnet_SchemaVersions
            SET IsCurrentVersion = 0
            WHERE Feature = LOWER( @Feature )
        END
    END

    INSERT  dbo.aspnet_SchemaVersions( Feature, CompatibleSchemaVersion, IsCurrentVersion )
    VALUES( LOWER( @Feature ), @CompatibleSchemaVersion, @IsCurrentVersion )
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UnRegisterSchemaVersion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_UnRegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    DELETE FROM dbo.aspnet_SchemaVersions
        WHERE   Feature = LOWER(@Feature) AND @CompatibleSchemaVersion = CompatibleSchemaVersion
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_Profiles]'))
EXEC dbo.sp_executesql @statement = N'
  CREATE VIEW [dbo].[vw_aspnet_Profiles]
  AS SELECT [dbo].[aspnet_Profile].[UserId], [dbo].[aspnet_Profile].[LastUpdatedDate],
      [DataSize]=  DATALENGTH([dbo].[aspnet_Profile].[PropertyNames])
                 + DATALENGTH([dbo].[aspnet_Profile].[PropertyValuesString])
                 + DATALENGTH([dbo].[aspnet_Profile].[PropertyValuesBinary])
  FROM [dbo].[aspnet_Profile]
  ' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_DeleteRole]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Roles_DeleteRole]
    @ApplicationName            nvarchar(256),
    @RoleName                   nvarchar(256),
    @DeleteOnlyIfRoleIsEmpty    bit
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    DECLARE @RoleId   uniqueidentifier
    SELECT  @RoleId = NULL
    SELECT  @RoleId = RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
    BEGIN
        SELECT @ErrorCode = 1
        GOTO Cleanup
    END
    IF (@DeleteOnlyIfRoleIsEmpty <> 0)
    BEGIN
        IF (EXISTS (SELECT RoleId FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId))
        BEGIN
            SELECT @ErrorCode = 2
            GOTO Cleanup
        END
    END


    DELETE FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DELETE FROM dbo.aspnet_Roles WHERE @RoleId = RoleId  AND ApplicationId = @ApplicationId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_RoleExists]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Roles_RoleExists]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(0)
    IF (EXISTS (SELECT RoleName FROM dbo.aspnet_Roles WHERE LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId ))
        RETURN(1)
    ELSE
        RETURN(0)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_GetAllRoles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Roles_GetAllRoles] (
    @ApplicationName           nvarchar(256))
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN
    SELECT RoleName
    FROM   dbo.aspnet_Roles WHERE ApplicationId = @ApplicationId
    ORDER BY RoleName
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_Roles]'))
EXEC dbo.sp_executesql @statement = N'
  CREATE VIEW [dbo].[vw_aspnet_Roles]
  AS SELECT [dbo].[aspnet_Roles].[ApplicationId], [dbo].[aspnet_Roles].[RoleId], [dbo].[aspnet_Roles].[RoleName], [dbo].[aspnet_Roles].[LoweredRoleName], [dbo].[aspnet_Roles].[Description]
  FROM [dbo].[aspnet_Roles]
  ' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_UsersInRoles]'))
EXEC dbo.sp_executesql @statement = N'
  CREATE VIEW [dbo].[vw_aspnet_UsersInRoles]
  AS SELECT [dbo].[aspnet_UsersInRoles].[UserId], [dbo].[aspnet_UsersInRoles].[RoleId]
  FROM [dbo].[aspnet_UsersInRoles]
  ' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Paths_CreatePath]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Paths_CreatePath]
    @ApplicationId UNIQUEIDENTIFIER,
    @Path           NVARCHAR(256),
    @PathId         UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    BEGIN TRANSACTION
    IF (NOT EXISTS(SELECT * FROM dbo.aspnet_Paths WHERE LoweredPath = LOWER(@Path) AND ApplicationId = @ApplicationId))
    BEGIN
        INSERT dbo.aspnet_Paths (ApplicationId, Path, LoweredPath) VALUES (@ApplicationId, @Path, LOWER(@Path))
    END
    COMMIT TRANSACTION
    SELECT @PathId = PathId FROM dbo.aspnet_Paths WHERE LOWER(@Path) = LoweredPath AND ApplicationId = @ApplicationId
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_WebPartState_Paths]'))
EXEC dbo.sp_executesql @statement = N'
  CREATE VIEW [dbo].[vw_aspnet_WebPartState_Paths]
  AS SELECT [dbo].[aspnet_Paths].[ApplicationId], [dbo].[aspnet_Paths].[PathId], [dbo].[aspnet_Paths].[Path], [dbo].[aspnet_Paths].[LoweredPath]
  FROM [dbo].[aspnet_Paths]
  ' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_WebPartState_Shared]'))
EXEC dbo.sp_executesql @statement = N'
  CREATE VIEW [dbo].[vw_aspnet_WebPartState_Shared]
  AS SELECT [dbo].[aspnet_PersonalizationAllUsers].[PathId], [DataSize]=DATALENGTH([dbo].[aspnet_PersonalizationAllUsers].[PageSettings]), [dbo].[aspnet_PersonalizationAllUsers].[LastUpdatedDate]
  FROM [dbo].[aspnet_PersonalizationAllUsers]
  ' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_WebPartState_User]'))
EXEC dbo.sp_executesql @statement = N'
  CREATE VIEW [dbo].[vw_aspnet_WebPartState_User]
  AS SELECT [dbo].[aspnet_PersonalizationPerUser].[PathId], [dbo].[aspnet_PersonalizationPerUser].[UserId], [DataSize]=DATALENGTH([dbo].[aspnet_PersonalizationPerUser].[PageSettings]), [dbo].[aspnet_PersonalizationPerUser].[LastUpdatedDate]
  FROM [dbo].[aspnet_PersonalizationPerUser]
  ' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_WebEvent_LogEvent]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_WebEvent_LogEvent]
        @EventId         char(32),
        @EventTimeUtc    datetime,
        @EventTime       datetime,
        @EventType       nvarchar(256),
        @EventSequence   decimal(19,0),
        @EventOccurrence decimal(19,0),
        @EventCode       int,
        @EventDetailCode int,
        @Message         nvarchar(1024),
        @ApplicationPath nvarchar(256),
        @ApplicationVirtualPath nvarchar(256),
        @MachineName    nvarchar(256),
        @RequestUrl      nvarchar(1024),
        @ExceptionType   nvarchar(256),
        @Details         ntext
AS
BEGIN
    INSERT
        dbo.aspnet_WebEvent_Events
        (
            EventId,
            EventTimeUtc,
            EventTime,
            EventType,
            EventSequence,
            EventOccurrence,
            EventCode,
            EventDetailCode,
            Message,
            ApplicationPath,
            ApplicationVirtualPath,
            MachineName,
            RequestUrl,
            ExceptionType,
            Details
        )
    VALUES
    (
        @EventId,
        @EventTimeUtc,
        @EventTime,
        @EventType,
        @EventSequence,
        @EventOccurrence,
        @EventCode,
        @EventDetailCode,
        @Message,
        @ApplicationPath,
        @ApplicationVirtualPath,
        @MachineName,
        @RequestUrl,
        @ExceptionType,
        @Details
    )
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[installation_Create_Seed_Data]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[installation_Create_Seed_Data]
	
AS
BEGIN

	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES(''common'',''1'',''True'')
	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES(''health monitoring'',''1'',''True'')
	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES(''membership'',''1'',''True'')
	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES(''personalization'',''1'',''True'')
	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES(''profile'',''1'',''True'')
	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES(''role manager'',''1'',''True'')
	INSERT INTO [dbo].[aspnet_Applications]
			   ([ApplicationName],[LoweredApplicationName],[ApplicationId],[Description])
		 VALUES (''SocialNetwork'',''socialnetwork'',''e62f2881-e423-45ae-9670-44b1b24cab5f'',NULL)
	INSERT INTO [dbo].[aspnet_Roles]
			   ([ApplicationId],[RoleId],[RoleName],[LoweredRoleName],[Description])
		 VALUES (''e62f2881-e423-45ae-9670-44b1b24cab5f'',''85e349ae-da8b-4f20-aca4-88ffcf707a96'',''Administrators'',''administrators'',NULL)
	INSERT INTO [dbo].[Location]
			   ([LocationID],[Name],[Address1],[Address2],[City],[Region],[Country],[PostalCode],[Latitude],[Longitude],[Type],[SearchData])
		 VALUES (''00000000-0000-0000-0000-000000000000'',''Unspecified'','''','''','''','''','''','''',0,0,'''',''!!!!!!'')

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Media_GetPicture]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[aspnet_Media_GetPicture]
	-- Add the parameters for the stored procedure here
	@ItemId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- get the photo details with the DAT
	-- join it to the user''s profile
	SELECT		base.Title, base.ImageURL, prof.DomainToken, prof.OwnerHandle
	FROM		BaseItem base 
	INNER JOIN	Profile prof ON base.OwnerUserID = prof.UserID 
	WHERE		base.SubType = ''Picture'' and base.BaseItemID = @ItemId

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Album_RemoveOrphanURL]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Album_RemoveOrphanURL]
	@AlbumID int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Temp1 TABLE ( itemid int ) 
	insert into @Temp1 select baseitemid from media where baseitemid in(select baseitemid b from baseitem where subtype = ''picture'' and len(imageurl)>4) and albumbaseitemid = @AlbumID
	delete from media where baseitemid in( select itemid from @Temp1)
	delete from baseitem where baseitemid in( select itemid from @Temp1)
	delete from Album where baseitemid in( select itemid from @Temp1)
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Applications_CreateApplication]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Applications_CreateApplication]
    @ApplicationName      nvarchar(256),
    @ApplicationId        uniqueidentifier OUTPUT
AS
BEGIN
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName

    IF(@ApplicationId IS NULL)
    BEGIN
        DECLARE @TranStarted   bit
        SET @TranStarted = 0

        IF( @@TRANCOUNT = 0 )
        BEGIN
	        BEGIN TRANSACTION
	        SET @TranStarted = 1
        END
        ELSE
    	    SET @TranStarted = 0

        SELECT  @ApplicationId = ApplicationId
        FROM dbo.aspnet_Applications WITH (UPDLOCK, HOLDLOCK)
        WHERE LOWER(@ApplicationName) = LoweredApplicationName

        IF(@ApplicationId IS NULL)
        BEGIN
            SELECT  @ApplicationId = NEWID()
            INSERT  dbo.aspnet_Applications (ApplicationId, ApplicationName, LoweredApplicationName)
            VALUES  (@ApplicationId, @ApplicationName, LOWER(@ApplicationName))
        END


        IF( @TranStarted = 1 )
        BEGIN
            IF(@@ERROR = 0)
            BEGIN
	        SET @TranStarted = 0
	        COMMIT TRANSACTION
            END
            ELSE
            BEGIN
                SET @TranStarted = 0
                ROLLBACK TRANSACTION
            END
        END
    END
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_aspnet_Applications]'))
EXEC dbo.sp_executesql @statement = N'
  CREATE VIEW [dbo].[vw_aspnet_Applications]
  AS SELECT [dbo].[aspnet_Applications].[ApplicationName], [dbo].[aspnet_Applications].[LoweredApplicationName], [dbo].[aspnet_Applications].[ApplicationId], [dbo].[aspnet_Applications].[Description]
  FROM [dbo].[aspnet_Applications]
  ' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_FindUsersByName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByName]
    @ApplicationName       nvarchar(256),
    @UserNameToMatch       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT u.UserId
        FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND u.LoweredUserName LIKE LOWER(@UserNameToMatch)
        ORDER BY u.UserName


    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_FindUsersByEmail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByEmail]
    @ApplicationName       nvarchar(256),
    @EmailToMatch          nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    IF( @EmailToMatch IS NULL )
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.Email IS NULL
            ORDER BY m.LoweredEmail
    ELSE
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.LoweredEmail LIKE LOWER(@EmailToMatch)
            ORDER BY m.LoweredEmail

    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY m.LoweredEmail

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetAllUsers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_GetAllUsers]
    @ApplicationName       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0


    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
    SELECT u.UserId
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u
    WHERE  u.ApplicationId = @ApplicationId AND u.UserId = m.UserId
    ORDER BY u.UserName

    SELECT @TotalRecords = @@ROWCOUNT

    SELECT u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName
    RETURN @TotalRecords
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_GetProfiles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Profile_GetProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @PageIndex              int,
    @PageSize               int,
    @UserNameToMatch        nvarchar(256) = NULL,
    @InactiveSinceDate      datetime      = NULL
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT  u.UserId
        FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p
        WHERE   ApplicationId = @ApplicationId
            AND u.UserId = p.UserId
            AND (@InactiveSinceDate IS NULL OR LastActivityDate <= @InactiveSinceDate)
            AND (     (@ProfileAuthOptions = 2)
                   OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                   OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                 )
            AND (@UserNameToMatch IS NULL OR LoweredUserName LIKE LOWER(@UserNameToMatch))
        ORDER BY UserName

    SELECT  u.UserName, u.IsAnonymous, u.LastActivityDate, p.LastUpdatedDate,
            DATALENGTH(p.PropertyNames) + DATALENGTH(p.PropertyValuesString) + DATALENGTH(p.PropertyValuesBinary)
    FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p, #PageIndexForUsers i
    WHERE   u.UserId = p.UserId AND p.UserId = i.UserId AND i.IndexId >= @PageLowerBound AND i.IndexId <= @PageUpperBound

    SELECT COUNT(*)
    FROM   #PageIndexForUsers

    DROP TABLE #PageIndexForUsers
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Personalization_GetApplicationId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Personalization_GetApplicationId] (
    @ApplicationName NVARCHAR(256),
    @ApplicationId UNIQUEIDENTIFIER OUT)
AS
BEGIN
    SELECT @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser_GetPageSettings]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    SELECT p.PageSettings FROM dbo.aspnet_PersonalizationPerUser p WHERE p.PathId = @PathId AND p.UserId = @UserId
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser_ResetPageSettings]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE PathId = @PathId AND UserId = @UserId
    RETURN 0
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser_SetPageSettings]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CurrentTimeUtc, @UserId OUTPUT
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    IF (EXISTS(SELECT PathId FROM dbo.aspnet_PersonalizationPerUser WHERE UserId = @UserId AND PathId = @PathId))
        UPDATE dbo.aspnet_PersonalizationPerUser SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE UserId = @UserId AND PathId = @PathId
    ELSE
        INSERT INTO dbo.aspnet_PersonalizationPerUser(UserId, PathId, PageSettings, LastUpdatedDate) VALUES (@UserId, @PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_SetProperties]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Profile_SetProperties]
    @ApplicationName        nvarchar(256),
    @PropertyNames          ntext,
    @PropertyValuesString   ntext,
    @PropertyValuesBinary   image,
    @UserName               nvarchar(256),
    @IsUserAnonymous        bit,
    @CurrentTimeUtc         datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
       BEGIN TRANSACTION
       SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DECLARE @UserId uniqueidentifier
    DECLARE @LastActivityDate datetime
    SELECT  @UserId = NULL
    SELECT  @LastActivityDate = @CurrentTimeUtc

    SELECT @UserId = UserId
    FROM   dbo.aspnet_Users
    WHERE  ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
        EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, @IsUserAnonymous, @LastActivityDate, @UserId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Users
    SET    LastActivityDate=@CurrentTimeUtc
    WHERE  UserId = @UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS( SELECT *
               FROM   dbo.aspnet_Profile
               WHERE  UserId = @UserId))
        UPDATE dbo.aspnet_Profile
        SET    PropertyNames=@PropertyNames, PropertyValuesString = @PropertyValuesString,
               PropertyValuesBinary = @PropertyValuesBinary, LastUpdatedDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    ELSE
        INSERT INTO dbo.aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate)
             VALUES (@UserId, @PropertyNames, @PropertyValuesString, @PropertyValuesBinary, @CurrentTimeUtc)

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_CreateUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Membership_CreateUser]
    @ApplicationName                        nvarchar(256),
    @UserName                               nvarchar(256),
    @Password                               nvarchar(128),
    @PasswordSalt                           nvarchar(128),
    @Email                                  nvarchar(256),
    @PasswordQuestion                       nvarchar(256),
    @PasswordAnswer                         nvarchar(128),
    @IsApproved                             bit,
    @CurrentTimeUtc                         datetime,
    @CreateDate                             datetime = NULL,
    @UniqueEmail                            int      = 0,
    @PasswordFormat                         int      = 0,
    @UserId                                 uniqueidentifier OUTPUT
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @NewUserId uniqueidentifier
    SELECT @NewUserId = NULL

    DECLARE @IsLockedOut bit
    SET @IsLockedOut = 0

    DECLARE @LastLockoutDate  datetime
    SET @LastLockoutDate = CONVERT( datetime, ''17540101'', 112 )

    DECLARE @FailedPasswordAttemptCount int
    SET @FailedPasswordAttemptCount = 0

    DECLARE @FailedPasswordAttemptWindowStart  datetime
    SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )

    DECLARE @FailedPasswordAnswerAttemptCount int
    SET @FailedPasswordAnswerAttemptCount = 0

    DECLARE @FailedPasswordAnswerAttemptWindowStart  datetime
    SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, ''17540101'', 112 )

    DECLARE @NewUserCreated bit
    DECLARE @ReturnValue   int
    SET @ReturnValue = 0

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    SET @CreateDate = @CurrentTimeUtc

    SELECT  @NewUserId = UserId FROM dbo.aspnet_Users WHERE LOWER(@UserName) = LoweredUserName AND @ApplicationId = ApplicationId
    IF ( @NewUserId IS NULL )
    BEGIN
        SET @NewUserId = @UserId
        EXEC @ReturnValue = dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CreateDate, @NewUserId OUTPUT
        SET @NewUserCreated = 1
    END
    ELSE
    BEGIN
        SET @NewUserCreated = 0
        IF( @NewUserId <> @UserId AND @UserId IS NOT NULL )
        BEGIN
            SET @ErrorCode = 6
            GOTO Cleanup
        END
    END

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @ReturnValue = -1 )
    BEGIN
        SET @ErrorCode = 10
        GOTO Cleanup
    END

    IF ( EXISTS ( SELECT UserId
                  FROM   dbo.aspnet_Membership
                  WHERE  @NewUserId = UserId ) )
    BEGIN
        SET @ErrorCode = 6
        GOTO Cleanup
    END

    SET @UserId = @NewUserId

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership m WITH ( UPDLOCK, HOLDLOCK )
                    WHERE ApplicationId = @ApplicationId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            SET @ErrorCode = 7
            GOTO Cleanup
        END
    END

    IF (@NewUserCreated = 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate = @CreateDate
        WHERE  @UserId = UserId
        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    INSERT INTO dbo.aspnet_Membership
                ( ApplicationId,
                  UserId,
                  Password,
                  PasswordSalt,
                  Email,
                  LoweredEmail,
                  PasswordQuestion,
                  PasswordAnswer,
                  PasswordFormat,
                  IsApproved,
                  IsLockedOut,
                  CreateDate,
                  LastLoginDate,
                  LastPasswordChangedDate,
                  LastLockoutDate,
                  FailedPasswordAttemptCount,
                  FailedPasswordAttemptWindowStart,
                  FailedPasswordAnswerAttemptCount,
                  FailedPasswordAnswerAttemptWindowStart )
         VALUES ( @ApplicationId,
                  @UserId,
                  @Password,
                  @PasswordSalt,
                  @Email,
                  LOWER(@Email),
                  @PasswordQuestion,
                  @PasswordAnswer,
                  @PasswordFormat,
                  @IsApproved,
                  @IsLockedOut,
                  @CreateDate,
                  @CreateDate,
                  @CreateDate,
                  @LastLockoutDate,
                  @FailedPasswordAttemptCount,
                  @FailedPasswordAttemptWindowStart,
                  @FailedPasswordAnswerAttemptCount,
                  @FailedPasswordAnswerAttemptWindowStart )

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAdministration_ResetUserState]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetUserState] (
    @Count                  int                 OUT,
    @ApplicationName        NVARCHAR(256),
    @InactiveSinceDate      DATETIME            = NULL,
    @UserName               NVARCHAR(256)       = NULL,
    @Path                   NVARCHAR(256)       = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser
        WHERE Id IN (SELECT PerUser.Id
                     FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
                     WHERE Paths.ApplicationId = @ApplicationId
                           AND PerUser.UserId = Users.UserId
                           AND PerUser.PathId = Paths.PathId
                           AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
                           AND (@UserName IS NULL OR Users.LoweredUserName = LOWER(@UserName))
                           AND (@Path IS NULL OR Paths.LoweredPath = LOWER(@Path)))

        SELECT @Count = @@ROWCOUNT
    END
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAdministration_GetCountOfState]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_GetCountOfState] (
    @Count int OUT,
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN

    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
        IF (@AllUsersScope = 1)
            SELECT @Count = COUNT(*)
            FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND AllUsers.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
        ELSE
            SELECT @Count = COUNT(*)
            FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND PerUser.UserId = Users.UserId
                  AND PerUser.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
                  AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
                  AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAllUsers_SetPageSettings]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    IF (EXISTS(SELECT PathId FROM dbo.aspnet_PersonalizationAllUsers WHERE PathId = @PathId))
        UPDATE dbo.aspnet_PersonalizationAllUsers SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE PathId = @PathId
    ELSE
        INSERT INTO dbo.aspnet_PersonalizationAllUsers(PathId, PageSettings, LastUpdatedDate) VALUES (@PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_CreateRole]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_Roles_CreateRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS(SELECT RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId))
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    INSERT INTO dbo.aspnet_Roles
                (ApplicationId, RoleName, LoweredRoleName)
         VALUES (@ApplicationId, @RoleName, LOWER(@RoleName))

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_DeleteProfiles]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[aspnet_Profile_DeleteProfiles]
    @ApplicationName        nvarchar(256),
    @UserNames              nvarchar(4000)
AS
BEGIN
    DECLARE @UserName     nvarchar(256)
    DECLARE @CurrentPos   int
    DECLARE @NextPos      int
    DECLARE @NumDeleted   int
    DECLARE @DeletedUser  int
    DECLARE @TranStarted  bit
    DECLARE @ErrorCode    int

    SET @ErrorCode = 0
    SET @CurrentPos = 1
    SET @NumDeleted = 0
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    WHILE (@CurrentPos <= LEN(@UserNames))
    BEGIN
        SELECT @NextPos = CHARINDEX(N'','', @UserNames,  @CurrentPos)
        IF (@NextPos = 0 OR @NextPos IS NULL)
            SELECT @NextPos = LEN(@UserNames) + 1

        SELECT @UserName = SUBSTRING(@UserNames, @CurrentPos, @NextPos - @CurrentPos)
        SELECT @CurrentPos = @NextPos+1

        IF (LEN(@UserName) > 0)
        BEGIN
            SELECT @DeletedUser = 0
            EXEC dbo.aspnet_Users_DeleteUser @ApplicationName, @UserName, 4, @DeletedUser OUTPUT
            IF( @@ERROR <> 0 )
            BEGIN
                SET @ErrorCode = -1
                GOTO Cleanup
            END
            IF (@DeletedUser <> 0)
                SELECT @NumDeleted = @NumDeleted + 1
        END
    END
    SELECT @NumDeleted
    IF (@TranStarted = 1)
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END
    SET @TranStarted = 0

    RETURN 0

Cleanup:
    IF (@TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END
    RETURN @ErrorCode
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAllUsers_GetPageSettings]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT p.PageSettings FROM dbo.aspnet_PersonalizationAllUsers p WHERE p.PathId = @PathId
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAdministration_DeleteAllState]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_DeleteAllState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Count int OUT)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        IF (@AllUsersScope = 1)
            DELETE FROM aspnet_PersonalizationAllUsers
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM dbo.aspnet_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)
        ELSE
            DELETE FROM aspnet_PersonalizationPerUser
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM dbo.aspnet_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)

        SELECT @Count = @@ROWCOUNT
    END
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    DELETE FROM dbo.aspnet_PersonalizationAllUsers WHERE PathId = @PathId
    RETURN 0
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAdministration_ResetSharedState]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetSharedState] (
    @Count int OUT,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationAllUsers
        WHERE PathId IN
            (SELECT AllUsers.PathId
             FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
             WHERE Paths.ApplicationId = @ApplicationId
                   AND AllUsers.PathId = Paths.PathId
                   AND Paths.LoweredPath = LOWER(@Path))

        SELECT @Count = @@ROWCOUNT
    END
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAdministration_FindState]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_FindState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @PageIndex              INT,
    @PageSize               INT,
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound INT
    DECLARE @PageUpperBound INT
    DECLARE @TotalRecords   INT
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table to store the selected results
    CREATE TABLE #PageIndex (
        IndexId int IDENTITY (0, 1) NOT NULL,
        ItemId UNIQUEIDENTIFIER
    )

    IF (@AllUsersScope = 1)
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT Paths.PathId
        FROM dbo.aspnet_Paths Paths,
             ((SELECT Paths.PathId
               FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND AllUsers.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT DISTINCT Paths.PathId
               FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND PerUser.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path,
               SharedDataPerPath.LastUpdatedDate,
               SharedDataPerPath.SharedDataLength,
               UserDataPerPath.UserDataLength,
               UserDataPerPath.UserCount
        FROM dbo.aspnet_Paths Paths,
             ((SELECT PageIndex.ItemId AS PathId,
                      AllUsers.LastUpdatedDate AS LastUpdatedDate,
                      DATALENGTH(AllUsers.PageSettings) AS SharedDataLength
               FROM dbo.aspnet_PersonalizationAllUsers AllUsers, #PageIndex PageIndex
               WHERE AllUsers.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT PageIndex.ItemId AS PathId,
                      SUM(DATALENGTH(PerUser.PageSettings)) AS UserDataLength,
                      COUNT(*) AS UserCount
               FROM aspnet_PersonalizationPerUser PerUser, #PageIndex PageIndex
               WHERE PerUser.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
               GROUP BY PageIndex.ItemId
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC
    END
    ELSE
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT PerUser.Id
        FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
        WHERE Paths.ApplicationId = @ApplicationId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
              AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
        ORDER BY Paths.Path ASC, Users.UserName ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path, PerUser.LastUpdatedDate, DATALENGTH(PerUser.PageSettings), Users.UserName, Users.LastActivityDate
        FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths, #PageIndex PageIndex
        WHERE PerUser.Id = PageIndex.ItemId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
        ORDER BY Paths.Path ASC, Users.UserName ASC
    END

    RETURN @TotalRecords
END' 
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserGroup_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserGroup]'))
ALTER TABLE [dbo].[UserGroup]  WITH CHECK ADD  CONSTRAINT [FK_UserGroup_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserGroup] CHECK CONSTRAINT [FK_UserGroup_aspnet_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserGroup_Group]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserGroup]'))
ALTER TABLE [dbo].[UserGroup]  WITH CHECK ADD  CONSTRAINT [FK_UserGroup_Group] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[Group] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserGroup] CHECK CONSTRAINT [FK_UserGroup_Group]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserBaseItemRating_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserBaseItemRating]'))
ALTER TABLE [dbo].[UserBaseItemRating]  WITH CHECK ADD  CONSTRAINT [FK_UserBaseItemRating_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserBaseItemRating] CHECK CONSTRAINT [FK_UserBaseItemRating_aspnet_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserBaseItemRating_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserBaseItemRating]'))
ALTER TABLE [dbo].[UserBaseItemRating]  WITH CHECK ADD  CONSTRAINT [FK_UserBaseItemRating_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserBaseItemRating] CHECK CONSTRAINT [FK_UserBaseItemRating_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Comment_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[Comment]'))
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Comment_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Comment]'))
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Me__Appli__145C0A3F]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Membership]'))
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Me__Appli__3D5E1FD2]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Membership]'))
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Me__Appli__3D5E1FD2] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Membership] CHECK CONSTRAINT [FK__aspnet_Me__Appli__3D5E1FD2]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Me__UserI__15502E78]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Membership]'))
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Me__UserI__3E52440B]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Membership]'))
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Me__UserI__3E52440B] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_Membership] CHECK CONSTRAINT [FK__aspnet_Me__UserI__3E52440B]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Pr__UserI__29572725]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Profile]'))
ALTER TABLE [dbo].[aspnet_Profile]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Pr__UserI__52593CB8]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Profile]'))
ALTER TABLE [dbo].[aspnet_Profile]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Pr__UserI__52593CB8] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_Profile] CHECK CONSTRAINT [FK__aspnet_Pr__UserI__52593CB8]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Profile_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Profile]'))
ALTER TABLE [dbo].[Profile]  WITH CHECK ADD  CONSTRAINT [FK_Profile_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Profile] CHECK CONSTRAINT [FK_Profile_aspnet_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Profile_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[Profile]'))
ALTER TABLE [dbo].[Profile]  WITH CHECK ADD  CONSTRAINT [FK_Profile_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Profile] CHECK CONSTRAINT [FK_Profile_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__RoleI__37A5467C]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]'))
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__RoleI__60A75C0F]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]'))
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Us__RoleI__60A75C0F] FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles] CHECK CONSTRAINT [FK__aspnet_Us__RoleI__60A75C0F]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__UserI__36B12243]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]'))
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__UserI__5FB337D6]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles]'))
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Us__UserI__5FB337D6] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles] CHECK CONSTRAINT [FK__aspnet_Us__UserI__5FB337D6]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Pe__PathI__4F7CD00D]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser]'))
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Pe__PathI__787EE5A0]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser]'))
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Pe__PathI__787EE5A0] FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] CHECK CONSTRAINT [FK__aspnet_Pe__PathI__787EE5A0]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Pe__UserI__5070F446]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser]'))
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Pe__UserI__797309D9]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser]'))
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Pe__UserI__797309D9] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] CHECK CONSTRAINT [FK__aspnet_Pe__UserI__797309D9]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Pe__PathI__4BAC3F29]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAllUsers]'))
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Pe__PathI__74AE54BC]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAllUsers]'))
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Pe__PathI__74AE54BC] FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers] CHECK CONSTRAINT [FK__aspnet_Pe__PathI__74AE54BC]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BaseItem_Location]') AND parent_object_id = OBJECT_ID(N'[dbo].[BaseItem]'))
ALTER TABLE [dbo].[BaseItem]  WITH CHECK ADD  CONSTRAINT [FK_BaseItem_Location] FOREIGN KEY([LocationID])
REFERENCES [dbo].[Location] ([LocationID])
GO
ALTER TABLE [dbo].[BaseItem] CHECK CONSTRAINT [FK_BaseItem_Location]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BaseItemTag_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[BaseItemTag]'))
ALTER TABLE [dbo].[BaseItemTag]  WITH CHECK ADD  CONSTRAINT [FK_BaseItemTag_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BaseItemTag] CHECK CONSTRAINT [FK_BaseItemTag_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BaseItemTag_Tag]') AND parent_object_id = OBJECT_ID(N'[dbo].[BaseItemTag]'))
ALTER TABLE [dbo].[BaseItemTag]  WITH CHECK ADD  CONSTRAINT [FK_BaseItemTag_Tag] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tag] ([TagID])
GO
ALTER TABLE [dbo].[BaseItemTag] CHECK CONSTRAINT [FK_BaseItemTag_Tag]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Media_Album]') AND parent_object_id = OBJECT_ID(N'[dbo].[Media]'))
ALTER TABLE [dbo].[Media]  WITH CHECK ADD  CONSTRAINT [FK_Media_Album] FOREIGN KEY([AlbumBaseItemID])
REFERENCES [dbo].[Album] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Media] CHECK CONSTRAINT [FK_Media_Album]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Media_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[Media]'))
ALTER TABLE [dbo].[Media]  WITH CHECK ADD  CONSTRAINT [FK_Media_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Media] CHECK CONSTRAINT [FK_Media_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ThumbnailCache_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ThumbnailCache]'))
ALTER TABLE [dbo].[ThumbnailCache]  WITH CHECK ADD  CONSTRAINT [FK_ThumbnailCache_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThumbnailCache] CHECK CONSTRAINT [FK_ThumbnailCache_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Collection_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[Collection]'))
ALTER TABLE [dbo].[Collection]  WITH CHECK ADD  CONSTRAINT [FK_Collection_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Collection] CHECK CONSTRAINT [FK_Collection_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BaseItem_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[BaseItemAssociation]'))
ALTER TABLE [dbo].[BaseItemAssociation]  WITH CHECK ADD  CONSTRAINT [FK_BaseItem_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BaseItemAssociation] CHECK CONSTRAINT [FK_BaseItem_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Group_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[Group]'))
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Group_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionItem_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollectionItem]'))
ALTER TABLE [dbo].[CollectionItem]  WITH CHECK ADD  CONSTRAINT [FK_CollectionItem_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CollectionItem] CHECK CONSTRAINT [FK_CollectionItem_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Forum_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[Forum]'))
ALTER TABLE [dbo].[Forum]  WITH CHECK ADD  CONSTRAINT [FK_Forum_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Forum] CHECK CONSTRAINT [FK_Forum_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PendingEmailInvites_BaseItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[PendingEmailInvites]'))
ALTER TABLE [dbo].[PendingEmailInvites]  WITH CHECK ADD  CONSTRAINT [FK_PendingEmailInvites_BaseItem] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[BaseItem] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PendingEmailInvites] CHECK CONSTRAINT [FK_PendingEmailInvites_BaseItem]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__Appli__0425A276]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Users]'))
ALTER TABLE [dbo].[aspnet_Users]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Us__Appli__2D27B809]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Users]'))
ALTER TABLE [dbo].[aspnet_Users]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Us__Appli__2D27B809] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Users] CHECK CONSTRAINT [FK__aspnet_Us__Appli__2D27B809]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Pa__Appli__45F365D3]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Paths]'))
ALTER TABLE [dbo].[aspnet_Paths]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Pa__Appli__6EF57B66]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Paths]'))
ALTER TABLE [dbo].[aspnet_Paths]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Pa__Appli__6EF57B66] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Paths] CHECK CONSTRAINT [FK__aspnet_Pa__Appli__6EF57B66]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Ro__Appli__32E0915F]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Roles]'))
ALTER TABLE [dbo].[aspnet_Roles]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__aspnet_Ro__Appli__5BE2A6F2]') AND parent_object_id = OBJECT_ID(N'[dbo].[aspnet_Roles]'))
ALTER TABLE [dbo].[aspnet_Roles]  WITH CHECK ADD  CONSTRAINT [FK__aspnet_Ro__Appli__5BE2A6F2] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Roles] CHECK CONSTRAINT [FK__aspnet_Ro__Appli__5BE2A6F2]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Event_Group]') AND parent_object_id = OBJECT_ID(N'[dbo].[Event]'))
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Group] FOREIGN KEY([BaseItemID])
REFERENCES [dbo].[Group] ([BaseItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Group]
go
--------------------------------------------------------
-- MANUALLY ADDED
--------------------------------------------------------
create PROCEDURE [dbo].[aspnet_Album_RemoveOrphanURLEx]
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRAN
      delete from media where baseitemid in(select baseitemid b from baseitem where subtype = 'picture' and len(imageurl)>4 and OwnerUserID in (select UserID from Profile where  len(domaintoken)<4 ));
	  delete FROM BaseItem  where SubType = 'Picture' and len(imageurl)>4 and OwnerUserID in (select UserID from Profile where  len(domaintoken)<4 );
	COMMIT TRAN
END
go

EXEC [dbo].[installation_Create_Seed_Data]
go

