
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping foreign keys from [dbo].[aspnet_Profile]'
GO
ALTER TABLE [dbo].[aspnet_Profile] DROP
CONSTRAINT [FK__aspnet_Pr__UserI__52593CB8]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[UserBaseItemRating]'
GO
ALTER TABLE [dbo].[UserBaseItemRating] DROP
CONSTRAINT [FK_UserBaseItemRating_aspnet_Users],
CONSTRAINT [FK_UserBaseItemRating_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[aspnet_UsersInRoles]'
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles] DROP
CONSTRAINT [FK__aspnet_Us__RoleI__60A75C0F],
CONSTRAINT [FK__aspnet_Us__UserI__5FB337D6]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[UserGroup]'
GO
ALTER TABLE [dbo].[UserGroup] DROP
CONSTRAINT [FK_UserGroup_aspnet_Users]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[aspnet_Membership]'
GO
ALTER TABLE [dbo].[aspnet_Membership] DROP
CONSTRAINT [FK__aspnet_Me__Appli__3D5E1FD2],
CONSTRAINT [FK__aspnet_Me__UserI__3E52440B]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Media]'
GO
ALTER TABLE [dbo].[Media] DROP
CONSTRAINT [FK_Media_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[CollectionItem]'
GO
ALTER TABLE [dbo].[CollectionItem] DROP
CONSTRAINT [FK_CollectionItem_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[aspnet_Paths]'
GO
ALTER TABLE [dbo].[aspnet_Paths] DROP
CONSTRAINT [FK__aspnet_Pa__Appli__6EF57B66]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Comment]'
GO
ALTER TABLE [dbo].[Comment] DROP
CONSTRAINT [FK_Comment_User],
CONSTRAINT [FK_Comment_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Collection]'
GO
ALTER TABLE [dbo].[Collection] DROP
CONSTRAINT [FK_Collection_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[aspnet_PersonalizationAllUsers]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers] DROP
CONSTRAINT [FK__aspnet_Pe__PathI__74AE54BC]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[BaseItemTag]'
GO
ALTER TABLE [dbo].[BaseItemTag] DROP
CONSTRAINT [FK_BaseItemTag_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Profile]'
GO
ALTER TABLE [dbo].[Profile] DROP
CONSTRAINT [FK_Profile_aspnet_Users],
CONSTRAINT [FK_Profile_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[ThumbnailCache]'
GO
ALTER TABLE [dbo].[ThumbnailCache] DROP
CONSTRAINT [FK_ThumbnailCache_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[aspnet_PersonalizationPerUser]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] DROP
CONSTRAINT [FK__aspnet_Pe__PathI__787EE5A0],
CONSTRAINT [FK__aspnet_Pe__UserI__797309D9]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Group]'
GO
ALTER TABLE [dbo].[Group] DROP
CONSTRAINT [FK_Group_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[BaseItem]'
GO
ALTER TABLE [dbo].[BaseItem] DROP
CONSTRAINT [FK_BaseItem_Location]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[PendingEmailInvites]'
GO
ALTER TABLE [dbo].[PendingEmailInvites] DROP
CONSTRAINT [FK_PendingEmailInvites_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Forum]'
GO
ALTER TABLE [dbo].[Forum] DROP
CONSTRAINT [FK_Forum_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[aspnet_Users]'
GO
ALTER TABLE [dbo].[aspnet_Users] DROP
CONSTRAINT [FK__aspnet_Us__Appli__2D27B809]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[aspnet_Roles]'
GO
ALTER TABLE [dbo].[aspnet_Roles] DROP
CONSTRAINT [FK__aspnet_Ro__Appli__5BE2A6F2]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[BaseItemAssociation]'
GO
ALTER TABLE [dbo].[BaseItemAssociation] DROP
CONSTRAINT [FK_BaseItem_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Applications]'
GO
ALTER TABLE [dbo].[aspnet_Applications] DROP CONSTRAINT [PK__aspnet_Applicati__276EDEB3]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Applications]'
GO
ALTER TABLE [dbo].[aspnet_Applications] DROP CONSTRAINT [UQ__aspnet_Applicati__286302EC]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Applications]'
GO
ALTER TABLE [dbo].[aspnet_Applications] DROP CONSTRAINT [UQ__aspnet_Applicati__29572725]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Applications]'
GO
ALTER TABLE [dbo].[aspnet_Applications] DROP CONSTRAINT [DF__aspnet_Ap__Appli__2A4B4B5E]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Membership]'
GO
ALTER TABLE [dbo].[aspnet_Membership] DROP CONSTRAINT [PK__aspnet_Membershi__3C69FB99]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Membership]'
GO
ALTER TABLE [dbo].[aspnet_Membership] DROP CONSTRAINT [DF__aspnet_Me__Passw__3F466844]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Paths]'
GO
ALTER TABLE [dbo].[aspnet_Paths] DROP CONSTRAINT [PK__aspnet_Paths__6E01572D]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Paths]'
GO
ALTER TABLE [dbo].[aspnet_Paths] DROP CONSTRAINT [DF__aspnet_Pa__PathI__6FE99F9F]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_PersonalizationAllUsers]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers] DROP CONSTRAINT [PK__aspnet_Personali__73BA3083]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_PersonalizationPerUser]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] DROP CONSTRAINT [PK__aspnet_Personali__76969D2E]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_PersonalizationPerUser]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] DROP CONSTRAINT [DF__aspnet_Perso__Id__778AC167]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Profile]'
GO
ALTER TABLE [dbo].[aspnet_Profile] DROP CONSTRAINT [PK__aspnet_Profile__5165187F]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Roles]'
GO
ALTER TABLE [dbo].[aspnet_Roles] DROP CONSTRAINT [PK__aspnet_Roles__5AEE82B9]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Roles]'
GO
ALTER TABLE [dbo].[aspnet_Roles] DROP CONSTRAINT [DF__aspnet_Ro__RoleI__5CD6CB2B]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_SchemaVersions]'
GO
ALTER TABLE [dbo].[aspnet_SchemaVersions] DROP CONSTRAINT [PK__aspnet_SchemaVer__31EC6D26]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Users]'
GO
ALTER TABLE [dbo].[aspnet_Users] DROP CONSTRAINT [PK__aspnet_Users__2C3393D0]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Users]'
GO
ALTER TABLE [dbo].[aspnet_Users] DROP CONSTRAINT [DF__aspnet_Us__UserI__2E1BDC42]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Users]'
GO
ALTER TABLE [dbo].[aspnet_Users] DROP CONSTRAINT [DF__aspnet_Us__Mobil__2F10007B]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_Users]'
GO
ALTER TABLE [dbo].[aspnet_Users] DROP CONSTRAINT [DF__aspnet_Us__IsAno__300424B4]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_UsersInRoles]'
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles] DROP CONSTRAINT [PK__aspnet_UsersInRo__5EBF139D]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[aspnet_WebEvent_Events]'
GO
ALTER TABLE [dbo].[aspnet_WebEvent_Events] DROP CONSTRAINT [PK__aspnet_WebEvent___08B54D69]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[BaseItem]'
GO
ALTER TABLE [dbo].[BaseItem] DROP CONSTRAINT [PK_BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[BaseItem]'
GO
ALTER TABLE [dbo].[BaseItem] DROP CONSTRAINT [DF_BaseItem_IsApproved]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Profile]'
GO
ALTER TABLE [dbo].[Profile] DROP CONSTRAINT [PK_Profile]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[aspnet_Paths]'
GO
ALTER TABLE [dbo].[aspnet_Paths] ALTER COLUMN [PathId] [uniqueidentifier] NOT NULL

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [aspnet_Paths_index] on [dbo].[aspnet_Paths]'
GO
CREATE UNIQUE CLUSTERED INDEX [aspnet_Paths_index] ON [dbo].[aspnet_Paths] ([ApplicationId], [LoweredPath])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK__aspnet_Paths__3E52440B] on [dbo].[aspnet_Paths]'
GO
ALTER TABLE [dbo].[aspnet_Paths] ADD CONSTRAINT [PK__aspnet_Paths__3E52440B] PRIMARY KEY NONCLUSTERED  ([PathId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[aspnet_PersonalizationPerUser]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] ALTER COLUMN [Id] [uniqueidentifier] NOT NULL

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [aspnet_PersonalizationPerUser_index1] on [dbo].[aspnet_PersonalizationPerUser]'
GO
CREATE UNIQUE CLUSTERED INDEX [aspnet_PersonalizationPerUser_index1] ON [dbo].[aspnet_PersonalizationPerUser] ([PathId], [UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK__aspnet_Personali__22AA2996] on [dbo].[aspnet_PersonalizationPerUser]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] ADD CONSTRAINT [PK__aspnet_Personali__22AA2996] PRIMARY KEY NONCLUSTERED  ([Id])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [aspnet_PersonalizationPerUser_ncindex2] on [dbo].[aspnet_PersonalizationPerUser]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [aspnet_PersonalizationPerUser_ncindex2] ON [dbo].[aspnet_PersonalizationPerUser] ([UserId], [PathId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[aspnet_Users]'
GO
ALTER TABLE [dbo].[aspnet_Users] ALTER COLUMN [UserId] [uniqueidentifier] NOT NULL
ALTER TABLE [dbo].[aspnet_Users] ALTER COLUMN [MobileAlias] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[aspnet_Users] ALTER COLUMN [IsAnonymous] [bit] NOT NULL

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [aspnet_Users_Index] on [dbo].[aspnet_Users]'
GO
CREATE UNIQUE CLUSTERED INDEX [aspnet_Users_Index] ON [dbo].[aspnet_Users] ([ApplicationId], [LoweredUserName])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK__aspnet_Users__398D8EEE] on [dbo].[aspnet_Users]'
GO
ALTER TABLE [dbo].[aspnet_Users] ADD CONSTRAINT [PK__aspnet_Users__398D8EEE] PRIMARY KEY NONCLUSTERED  ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [aspnet_Users_Index2] on [dbo].[aspnet_Users]'
GO
CREATE NONCLUSTERED INDEX [aspnet_Users_Index2] ON [dbo].[aspnet_Users] ([ApplicationId], [LastActivityDate])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Profile]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Profile]
(
[UserID] [uniqueidentifier] NOT NULL,
[FirstName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Gender] [int] NULL,
[DOB] [datetime] NULL,
[WindowsLiveCID] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RSSFeedURL] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BaseItemID] [int] NOT NULL,
[MessengerPresenceID] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DomainToken] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OwnerHandle] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Profile]([UserID], [FirstName], [LastName], [Gender], [DOB], [WindowsLiveCID], [RSSFeedURL], [BaseItemID], [MessengerPresenceID]) SELECT [UserID], [FirstName], [LastName], [Gender], [DOB], [WindowsLiveCID], [RSSFeedURL], [BaseItemID], [MessengerPresenceID] FROM [dbo].[Profile]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Profile]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Profile]', N'Profile'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Profile] on [dbo].[Profile]'
GO
ALTER TABLE [dbo].[Profile] ADD CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED  ([UserID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[BaseItem]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_BaseItem]
(
[BaseItemID] [int] NOT NULL IDENTITY(1, 1),
[ItemType] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TagList] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationID] [uniqueidentifier] NOT NULL,
[TotalViews] [int] NOT NULL,
[TotalRatingScore] [float] NOT NULL,
[TotalRatingCount] [int] NOT NULL,
[AverageRating] AS (coalesce([TotalRatingScore]/nullif([TotalRatingCount],(0)),(0))),
[Title] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OwnerUserID] [uniqueidentifier] NOT NULL,
[CreateDate] [datetime] NOT NULL,
[SubType] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PrivacyLevel] [int] NOT NULL,
[ThumbnailBits] [varbinary] (max) NULL,
[IsApproved] [bit] NOT NULL CONSTRAINT [DF_BaseItem_IsApproved] DEFAULT ((1)),
[ImageURL] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_BaseItem] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_BaseItem]([BaseItemID], [ItemType], [TagList], [LocationID], [TotalViews], [TotalRatingScore], [TotalRatingCount], [Title], [Description], [OwnerUserID], [CreateDate], [SubType], [PrivacyLevel], [ThumbnailBits], [IsApproved]) SELECT [BaseItemID], [ItemType], [TagList], [LocationID], [TotalViews], [TotalRatingScore], [TotalRatingCount], [Title], [Description], [OwnerUserID], [CreateDate], [SubType], [PrivacyLevel], [ThumbnailBits], [IsApproved] FROM [dbo].[BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_BaseItem] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[BaseItem]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_BaseItem]', N'BaseItem'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_BaseItem] on [dbo].[BaseItem]'
GO
ALTER TABLE [dbo].[BaseItem] ADD CONSTRAINT [PK_BaseItem] PRIMARY KEY CLUSTERED  ([BaseItemID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[aspnet_Applications]'
GO
ALTER TABLE [dbo].[aspnet_Applications] ALTER COLUMN [ApplicationId] [uniqueidentifier] NOT NULL

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [aspnet_Applications_Index] on [dbo].[aspnet_Applications]'
GO
CREATE CLUSTERED INDEX [aspnet_Applications_Index] ON [dbo].[aspnet_Applications] ([LoweredApplicationName])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK__aspnet_Applicati__0F975522] on [dbo].[aspnet_Applications]'
GO
ALTER TABLE [dbo].[aspnet_Applications] ADD CONSTRAINT [PK__aspnet_Applicati__0F975522] PRIMARY KEY NONCLUSTERED  ([ApplicationId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[aspnet_Roles]'
GO
ALTER TABLE [dbo].[aspnet_Roles] ALTER COLUMN [RoleId] [uniqueidentifier] NOT NULL

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [aspnet_Roles_index1] on [dbo].[aspnet_Roles]'
GO
CREATE UNIQUE CLUSTERED INDEX [aspnet_Roles_index1] ON [dbo].[aspnet_Roles] ([ApplicationId], [LoweredRoleName])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK__aspnet_Roles__412EB0B6] on [dbo].[aspnet_Roles]'
GO
ALTER TABLE [dbo].[aspnet_Roles] ADD CONSTRAINT [PK__aspnet_Roles__412EB0B6] PRIMARY KEY NONCLUSTERED  ([RoleId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[aspnet_Membership]'
GO
ALTER TABLE [dbo].[aspnet_Membership] ALTER COLUMN [PasswordFormat] [int] NOT NULL

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [aspnet_Membership_index] on [dbo].[aspnet_Membership]'
GO
CREATE CLUSTERED INDEX [aspnet_Membership_index] ON [dbo].[aspnet_Membership] ([ApplicationId], [LoweredEmail])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK__aspnet_Membershi__182C9B23] on [dbo].[aspnet_Membership]'
GO
ALTER TABLE [dbo].[aspnet_Membership] ADD CONSTRAINT [PK__aspnet_Membershi__182C9B23] PRIMARY KEY NONCLUSTERED  ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[vw_aspnet_Users]'
GO
EXEC sp_refreshview N'[dbo].[vw_aspnet_Users]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[vw_aspnet_MembershipUsers]'
GO
EXEC sp_refreshview N'[dbo].[vw_aspnet_MembershipUsers]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[vw_aspnet_Profiles]'
GO
EXEC sp_refreshview N'[dbo].[vw_aspnet_Profiles]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[vw_aspnet_Roles]'
GO
EXEC sp_refreshview N'[dbo].[vw_aspnet_Roles]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[vw_aspnet_UsersInRoles]'
GO
EXEC sp_refreshview N'[dbo].[vw_aspnet_UsersInRoles]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[vw_aspnet_WebPartState_Paths]'
GO
EXEC sp_refreshview N'[dbo].[vw_aspnet_WebPartState_Paths]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[vw_aspnet_WebPartState_Shared]'
GO
EXEC sp_refreshview N'[dbo].[vw_aspnet_WebPartState_Shared]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[vw_aspnet_WebPartState_User]'
GO
EXEC sp_refreshview N'[dbo].[vw_aspnet_WebPartState_User]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[installation_Create_Seed_Data]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[installation_Create_Seed_Data]
	
AS
BEGIN

	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES('common','1','True')
	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES('health monitoring','1','True')
	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES('membership','1','True')
	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES('personalization','1','True')
	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES('profile','1','True')
	INSERT INTO [dbo].[aspnet_SchemaVersions]
			   ([Feature],[CompatibleSchemaVersion],[IsCurrentVersion])
		 VALUES('role manager','1','True')
	INSERT INTO [dbo].[aspnet_Applications]
			   ([ApplicationName],[LoweredApplicationName],[ApplicationId],[Description])
		 VALUES ('SocialNetwork','socialnetwork','e62f2881-e423-45ae-9670-44b1b24cab5f',NULL)
	INSERT INTO [dbo].[aspnet_Roles]
			   ([ApplicationId],[RoleId],[RoleName],[LoweredRoleName],[Description])
		 VALUES ('e62f2881-e423-45ae-9670-44b1b24cab5f','85e349ae-da8b-4f20-aca4-88ffcf707a96','Administrators','administrators',NULL)
	INSERT INTO [dbo].[Location]
			   ([LocationID],[Name],[Address1],[Address2],[City],[Region],[Country],[PostalCode],[Latitude],[Longitude],[Type],[SearchData])
		 VALUES ('00000000-0000-0000-0000-000000000000','Unspecified','','','','','','',0,0,'','!!!!!!')

END

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[aspnet_Media_GetPicture]'
GO
-- =============================================
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
	-- join it to the user's profile
	SELECT		base.Title, base.ImageURL, prof.DomainToken, prof.OwnerHandle
	FROM		BaseItem base 
	INNER JOIN	Profile prof ON base.OwnerUserID = prof.UserID 
	WHERE		base.SubType = 'Picture' and base.BaseItemID = @ItemId

END

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[aspnet_Album_RemoveOrphanURL]'
GO
CREATE PROCEDURE [dbo].[aspnet_Album_RemoveOrphanURL]
	@AlbumID int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Temp1 TABLE ( itemid int ) 
	insert into @Temp1 select baseitemid from media where baseitemid in(select baseitemid b from baseitem where subtype = 'picture' and len(imageurl)>4) and albumbaseitemid = @AlbumID
	delete from media where baseitemid in( select itemid from @Temp1)
	delete from baseitem where baseitemid in( select itemid from @Temp1)
	delete from Album where baseitemid in( select itemid from @Temp1)
END

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[vw_aspnet_Applications]'
GO
EXEC sp_refreshview N'[dbo].[vw_aspnet_Applications]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[aspnet_Album_RemoveOrphanURLEx]'
GO
SET QUOTED_IDENTIFIER OFF
GO
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

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[aspnet_Setup_RestorePermissions]'
GO

ALTER PROCEDURE [dbo].[aspnet_Setup_RestorePermissions]
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
        SET @cmd = @protectType + ' ' + @action + ' on ' + @object + ' TO [' + @grantee + ']'
        EXEC (@cmd)
        FETCH c1 INTO @object, @protectType, @action, @grantee
    END

    CLOSE c1
    DEALLOCATE c1
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET QUOTED_IDENTIFIER ON
GO
PRINT N'Creating primary key [PK__aspnet_Personali__25869641] on [dbo].[aspnet_PersonalizationAllUsers]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers] ADD CONSTRAINT [PK__aspnet_Personali__25869641] PRIMARY KEY CLUSTERED  ([PathId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK__aspnet_Profile__1B0907CE] on [dbo].[aspnet_Profile]'
GO
ALTER TABLE [dbo].[aspnet_Profile] ADD CONSTRAINT [PK__aspnet_Profile__1B0907CE] PRIMARY KEY CLUSTERED  ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK__aspnet_SchemaVer__7C8480AE] on [dbo].[aspnet_SchemaVersions]'
GO
ALTER TABLE [dbo].[aspnet_SchemaVersions] ADD CONSTRAINT [PK__aspnet_SchemaVer__7C8480AE] PRIMARY KEY CLUSTERED  ([Feature], [CompatibleSchemaVersion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK__aspnet_UsersInRo__20C1E124] on [dbo].[aspnet_UsersInRoles]'
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles] ADD CONSTRAINT [PK__aspnet_UsersInRo__20C1E124] PRIMARY KEY CLUSTERED  ([UserId], [RoleId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK__aspnet_WebEvent___09DE7BCC] on [dbo].[aspnet_WebEvent_Events]'
GO
ALTER TABLE [dbo].[aspnet_WebEvent_Events] ADD CONSTRAINT [PK__aspnet_WebEvent___09DE7BCC] PRIMARY KEY CLUSTERED  ([EventId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [aspnet_UsersInRoles_index] on [dbo].[aspnet_UsersInRoles]'
GO
CREATE NONCLUSTERED INDEX [aspnet_UsersInRoles_index] ON [dbo].[aspnet_UsersInRoles] ([RoleId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[aspnet_Applications]'
GO
ALTER TABLE [dbo].[aspnet_Applications] ADD CONSTRAINT [UQ__aspnet_Applicati__0DAF0CB0] UNIQUE NONCLUSTERED  ([LoweredApplicationName])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[aspnet_Applications] ADD CONSTRAINT [UQ__aspnet_Applicati__0EA330E9] UNIQUE NONCLUSTERED  ([ApplicationName])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[aspnet_Applications] ADD CONSTRAINT [DF__aspnet_Ap__Appli__108B795B] DEFAULT (newid()) FOR [ApplicationId]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[aspnet_Membership]'
GO
ALTER TABLE [dbo].[aspnet_Membership] ADD CONSTRAINT [DF__aspnet_Me__Passw__1920BF5C] DEFAULT ((0)) FOR [PasswordFormat]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[aspnet_Paths]'
GO
ALTER TABLE [dbo].[aspnet_Paths] ADD CONSTRAINT [DF__aspnet_Pa__PathI__3F466844] DEFAULT (newid()) FOR [PathId]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[aspnet_PersonalizationPerUser]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] ADD CONSTRAINT [DF__aspnet_Perso__Id__239E4DCF] DEFAULT (newid()) FOR [Id]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[aspnet_Roles]'
GO
ALTER TABLE [dbo].[aspnet_Roles] ADD CONSTRAINT [DF__aspnet_Ro__RoleI__4222D4EF] DEFAULT (newid()) FOR [RoleId]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[aspnet_Users]'
GO
ALTER TABLE [dbo].[aspnet_Users] ADD CONSTRAINT [DF__aspnet_Us__UserI__3A81B327] DEFAULT (newid()) FOR [UserId]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[aspnet_Users] ADD CONSTRAINT [DF__aspnet_Us__Mobil__3B75D760] DEFAULT (NULL) FOR [MobileAlias]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[aspnet_Users] ADD CONSTRAINT [DF__aspnet_Us__IsAno__3C69FB99] DEFAULT ((0)) FOR [IsAnonymous]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[aspnet_Membership]'
GO
ALTER TABLE [dbo].[aspnet_Membership] ADD
CONSTRAINT [FK__aspnet_Me__Appli__09A971A2] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId]),
CONSTRAINT [FK__aspnet_Me__Appli__3D5E1FD2] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId]),
CONSTRAINT [FK__aspnet_Me__UserI__0B91BA14] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]),
CONSTRAINT [FK__aspnet_Me__UserI__3E52440B] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[aspnet_Paths]'
GO
ALTER TABLE [dbo].[aspnet_Paths] ADD
CONSTRAINT [FK__aspnet_Pa__Appli__282DF8C2] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId]),
CONSTRAINT [FK__aspnet_Pa__Appli__6EF57B66] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[aspnet_Roles]'
GO
ALTER TABLE [dbo].[aspnet_Roles] ADD
CONSTRAINT [FK__aspnet_Ro__Appli__2A164134] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId]),
CONSTRAINT [FK__aspnet_Ro__Appli__5BE2A6F2] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[aspnet_Users]'
GO
ALTER TABLE [dbo].[aspnet_Users] ADD
CONSTRAINT [FK__aspnet_Us__Appli__2645B050] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId]),
CONSTRAINT [FK__aspnet_Us__Appli__2D27B809] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[aspnet_PersonalizationPerUser]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] ADD
CONSTRAINT [FK__aspnet_Pe__PathI__151B244E] FOREIGN KEY ([PathId]) REFERENCES [dbo].[aspnet_Paths] ([PathId]),
CONSTRAINT [FK__aspnet_Pe__PathI__787EE5A0] FOREIGN KEY ([PathId]) REFERENCES [dbo].[aspnet_Paths] ([PathId]),
CONSTRAINT [FK__aspnet_Pe__UserI__17036CC0] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]),
CONSTRAINT [FK__aspnet_Pe__UserI__797309D9] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[aspnet_PersonalizationAllUsers]'
GO
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers] ADD
CONSTRAINT [FK__aspnet_Pe__PathI__18EBB532] FOREIGN KEY ([PathId]) REFERENCES [dbo].[aspnet_Paths] ([PathId]),
CONSTRAINT [FK__aspnet_Pe__PathI__74AE54BC] FOREIGN KEY ([PathId]) REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[aspnet_Profile]'
GO
ALTER TABLE [dbo].[aspnet_Profile] ADD
CONSTRAINT [FK__aspnet_Pr__UserI__0D7A0286] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]),
CONSTRAINT [FK__aspnet_Pr__UserI__52593CB8] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[aspnet_UsersInRoles]'
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles] ADD
CONSTRAINT [FK__aspnet_Us__RoleI__114A936A] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[aspnet_Roles] ([RoleId]),
CONSTRAINT [FK__aspnet_Us__RoleI__60A75C0F] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[aspnet_Roles] ([RoleId]),
CONSTRAINT [FK__aspnet_Us__UserI__1332DBDC] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]),
CONSTRAINT [FK__aspnet_Us__UserI__5FB337D6] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Comment]'
GO
ALTER TABLE [dbo].[Comment] ADD
CONSTRAINT [FK_Comment_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE CASCADE,
CONSTRAINT [FK_Comment_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Profile]'
GO
ALTER TABLE [dbo].[Profile] ADD
CONSTRAINT [FK_Profile_aspnet_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE CASCADE,
CONSTRAINT [FK_Profile_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[UserBaseItemRating]'
GO
ALTER TABLE [dbo].[UserBaseItemRating] ADD
CONSTRAINT [FK_UserBaseItemRating_aspnet_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE CASCADE,
CONSTRAINT [FK_UserBaseItemRating_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[UserGroup]'
GO
ALTER TABLE [dbo].[UserGroup] ADD
CONSTRAINT [FK_UserGroup_aspnet_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[BaseItemAssociation]'
GO
ALTER TABLE [dbo].[BaseItemAssociation] ADD
CONSTRAINT [FK_BaseItem_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[BaseItemTag]'
GO
ALTER TABLE [dbo].[BaseItemTag] ADD
CONSTRAINT [FK_BaseItemTag_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Collection]'
GO
ALTER TABLE [dbo].[Collection] ADD
CONSTRAINT [FK_Collection_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[CollectionItem]'
GO
ALTER TABLE [dbo].[CollectionItem] ADD
CONSTRAINT [FK_CollectionItem_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Forum]'
GO
ALTER TABLE [dbo].[Forum] ADD
CONSTRAINT [FK_Forum_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Group]'
GO
ALTER TABLE [dbo].[Group] ADD
CONSTRAINT [FK_Group_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Media]'
GO
ALTER TABLE [dbo].[Media] ADD
CONSTRAINT [FK_Media_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[PendingEmailInvites]'
GO
ALTER TABLE [dbo].[PendingEmailInvites] ADD
CONSTRAINT [FK_PendingEmailInvites_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ThumbnailCache]'
GO
ALTER TABLE [dbo].[ThumbnailCache] ADD
CONSTRAINT [FK_ThumbnailCache_BaseItem] FOREIGN KEY ([BaseItemID]) REFERENCES [dbo].[BaseItem] ([BaseItemID]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[BaseItem]'
GO
ALTER TABLE [dbo].[BaseItem] ADD
CONSTRAINT [FK_BaseItem_Location] FOREIGN KEY ([LocationID]) REFERENCES [dbo].[Location] ([LocationID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO
EXEC sp_rename N'[dbo].[Profile].[WindowsLiveCID]', N'WindowsLiveUUID', 'COLUMN'
GO