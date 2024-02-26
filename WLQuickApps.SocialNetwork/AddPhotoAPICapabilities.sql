/* 
****************************************************
		WHAT DOES THIS FILE DO
- Add the Columns to the underlying database tables
	- DAT/OWNERHANDLE to dbo.PROFILE
- create the SPROCs
- Set the columns which are null to be empty string
- Update the schema to disable NULLS
****************************************************
*/

/* 
****************************************************
	PREWIPE THE COLUMNS WHICH ARE NULL
****************************************************
*/
UPDATE	dbo.BaseItem
SET	ImageURL = ''
WHERE	ImageURL is NULL

UPDATE	dbo.Profile
SET	OwnerHandle = ''
WHERE	OwnerHandle is NULL

UPDATE	dbo.Profile
SET	DomainToken = ''
WHERE	DomainToken is NULL


/* 
****************************************************
	SET dbo.Profile(DAT/OWNERHANDLE) to ALLOW NULLS
****************************************************
*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Profile
	DROP CONSTRAINT FK_Profile_BaseItem
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Profile
	DROP CONSTRAINT FK_Profile_aspnet_Users
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Profile
	(
	UserID uniqueidentifier NOT NULL,
	FirstName varchar(255) NOT NULL,
	LastName varchar(255) NOT NULL,
	Gender int NULL,
	DOB datetime NULL,
	WindowsLiveUUID varchar(255) NULL,
	RSSFeedURL varchar(255) NULL,
	BaseItemID int NOT NULL,
	MessengerPresenceID varchar(255) NOT NULL,
	DomainToken varchar(500) NOT NULL,
	OwnerHandle varchar(255) NOT NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.Profile)
	 EXEC('INSERT INTO dbo.Tmp_Profile (UserID, FirstName, LastName, Gender, DOB, WindowsLiveUUID, RSSFeedURL, BaseItemID, MessengerPresenceID, DomainToken, OwnerHandle)
		SELECT UserID, FirstName, LastName, Gender, DOB, WindowsLiveUUID, RSSFeedURL, BaseItemID, MessengerPresenceID, DomainToken, OwnerHandle FROM dbo.Profile WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.Profile
GO
EXECUTE sp_rename N'dbo.Tmp_Profile', N'Profile', 'OBJECT' 
GO
ALTER TABLE dbo.Profile ADD CONSTRAINT
	PK_Profile PRIMARY KEY CLUSTERED 
	(
	UserID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Profile ADD CONSTRAINT
	FK_Profile_aspnet_Users FOREIGN KEY
	(
	UserID
	) REFERENCES dbo.aspnet_Users
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Profile ADD CONSTRAINT
	FK_Profile_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT


/* 
****************************************************
	SET BASEITEM.ImageURL to NOT NULL
****************************************************
*/
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BaseItem
	DROP CONSTRAINT FK_BaseItem_Location
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BaseItem
	DROP CONSTRAINT DF_BaseItem_IsApproved
GO
CREATE TABLE dbo.Tmp_BaseItem
	(
	BaseItemID int NOT NULL IDENTITY (1, 1),
	ItemType varchar(50) NOT NULL,
	TagList text NOT NULL,
	LocationID uniqueidentifier NOT NULL,
	TotalViews int NOT NULL,
	TotalRatingScore float(53) NOT NULL,
	TotalRatingCount int NOT NULL,
	AverageRating  AS (coalesce([TotalRatingScore]/nullif([TotalRatingCount],(0)),(0))),
	Title nvarchar(255) NOT NULL,
	Description ntext NOT NULL,
	OwnerUserID uniqueidentifier NOT NULL,
	CreateDate datetime NOT NULL,
	SubType varchar(50) NOT NULL,
	PrivacyLevel int NOT NULL,
	ThumbnailBits varbinary(MAX) NULL,
	IsApproved bit NOT NULL,
	ImageURL varchar(255) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_BaseItem ADD CONSTRAINT
	DF_BaseItem_IsApproved DEFAULT ((1)) FOR IsApproved
GO
SET IDENTITY_INSERT dbo.Tmp_BaseItem ON
GO
IF EXISTS(SELECT * FROM dbo.BaseItem)
	 EXEC('INSERT INTO dbo.Tmp_BaseItem (BaseItemID, ItemType, TagList, LocationID, TotalViews, TotalRatingScore, TotalRatingCount, Title, Description, OwnerUserID, CreateDate, SubType, PrivacyLevel, ThumbnailBits, IsApproved, ImageURL)
		SELECT BaseItemID, ItemType, TagList, LocationID, TotalViews, TotalRatingScore, TotalRatingCount, Title, Description, OwnerUserID, CreateDate, SubType, PrivacyLevel, ThumbnailBits, IsApproved, ImageURL FROM dbo.BaseItem WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_BaseItem OFF
GO
ALTER TABLE dbo.BaseItemAssociation
	DROP CONSTRAINT FK_BaseItem_BaseItem
GO
ALTER TABLE dbo.BaseItemTag
	DROP CONSTRAINT FK_BaseItemTag_BaseItem
GO
ALTER TABLE dbo.Collection
	DROP CONSTRAINT FK_Collection_BaseItem
GO
ALTER TABLE dbo.CollectionItem
	DROP CONSTRAINT FK_CollectionItem_BaseItem
GO
ALTER TABLE dbo.Comment
	DROP CONSTRAINT FK_Comment_BaseItem
GO
ALTER TABLE dbo.Forum
	DROP CONSTRAINT FK_Forum_BaseItem
GO
ALTER TABLE dbo.[Group]
	DROP CONSTRAINT FK_Group_BaseItem
GO
ALTER TABLE dbo.Media
	DROP CONSTRAINT FK_Media_BaseItem
GO
ALTER TABLE dbo.PendingEmailInvites
	DROP CONSTRAINT FK_PendingEmailInvites_BaseItem
GO
ALTER TABLE dbo.Profile
	DROP CONSTRAINT FK_Profile_BaseItem
GO
ALTER TABLE dbo.ThumbnailCache
	DROP CONSTRAINT FK_ThumbnailCache_BaseItem
GO
ALTER TABLE dbo.UserBaseItemRating
	DROP CONSTRAINT FK_UserBaseItemRating_BaseItem
GO
DROP TABLE dbo.BaseItem
GO
EXECUTE sp_rename N'dbo.Tmp_BaseItem', N'BaseItem', 'OBJECT' 
GO
ALTER TABLE dbo.BaseItem ADD CONSTRAINT
	PK_BaseItem PRIMARY KEY CLUSTERED 
	(
	BaseItemID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.BaseItem ADD CONSTRAINT
	FK_BaseItem_Location FOREIGN KEY
	(
	LocationID
	) REFERENCES dbo.Location
	(
	LocationID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.UserBaseItemRating ADD CONSTRAINT
	FK_UserBaseItemRating_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ThumbnailCache ADD CONSTRAINT
	FK_ThumbnailCache_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Profile ADD CONSTRAINT
	FK_Profile_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.PendingEmailInvites ADD CONSTRAINT
	FK_PendingEmailInvites_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Media ADD CONSTRAINT
	FK_Media_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.[Group] ADD CONSTRAINT
	FK_Group_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Forum ADD CONSTRAINT
	FK_Forum_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Comment ADD CONSTRAINT
	FK_Comment_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CollectionItem ADD CONSTRAINT
	FK_CollectionItem_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Collection ADD CONSTRAINT
	FK_Collection_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BaseItemTag ADD CONSTRAINT
	FK_BaseItemTag_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BaseItemAssociation ADD CONSTRAINT
	FK_BaseItem_BaseItem FOREIGN KEY
	(
	BaseItemID
	) REFERENCES dbo.BaseItem
	(
	BaseItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
