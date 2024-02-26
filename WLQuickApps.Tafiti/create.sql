USE [Tafiti]
GO
/****** Object:  Table [dbo].[ShelfStack]    Script Date: 01/11/2008 16:06:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShelfStack](
	[ShelfStackID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Shelf_ShelfID]  DEFAULT (newid()),
	[Label] [nvarchar](256) NOT NULL,
	[CreatedTimestamp] [datetime] NOT NULL CONSTRAINT [DF_Shelf_LastModifiedTimestamp]  DEFAULT (getdate()),
	[LastModifiedTimestamp] [datetime] NOT NULL CONSTRAINT [DF_Shelf_LastModifiedTimestamp_1]  DEFAULT (getdate()),
 CONSTRAINT [PK_Shelf] PRIMARY KEY CLUSTERED 
(
	[ShelfStackID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 01/11/2008 16:07:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [char](32) NOT NULL,
	[EmailCount] [int] NOT NULL CONSTRAINT [DF_Users_EmailCount]  DEFAULT ((0)),
	[EmailCountTimestamp] [datetime] NOT NULL CONSTRAINT [DF_Users_EmailCountTimestamp]  DEFAULT (getdate()),
	[LastLogin] [datetime] NOT NULL CONSTRAINT [DF_Users_LastLogin]  DEFAULT (getdate()),
	[EmailHash] [nvarchar](64) NOT NULL CONSTRAINT [DF_Users_EmailHash]  DEFAULT (''),
	[DisplayName] [nvarchar](64) NOT NULL,
	[MessengerPresenceID] [nvarchar](64) NOT NULL CONSTRAINT [DF_Users_MessengerPresenceID]  DEFAULT (''),
	[AlwaysSendMessages] [bit] NOT NULL CONSTRAINT [DF_Users_AlwaysSendMessages]  DEFAULT ((0)),
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ShelfStackOwners]    Script Date: 01/11/2008 16:06:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ShelfStackOwners](
	[UserID] [char](32) NOT NULL,
	[ShelfStackID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ShelfOwners] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[ShelfStackID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PendingInvites]    Script Date: 01/11/2008 16:06:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PendingInvites](
	[ShelfStackID] [uniqueidentifier] NOT NULL,
	[EmailHash] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Conversations]    Script Date: 01/11/2008 16:06:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Conversations](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[ShelfStackID] [uniqueidentifier] NOT NULL,
	[UserID] [char](32) NOT NULL,
	[Timestamp] [datetime] NOT NULL CONSTRAINT [DF_Conversations_Timestamp]  DEFAULT (getdate()),
	[Text] [ntext] NOT NULL,
 CONSTRAINT [PK_Conversations] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ShelfStackItem]    Script Date: 01/11/2008 16:06:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ShelfStackItem](
	[ShelfStackItemID] [int] IDENTITY(1,1) NOT NULL,
	[ShelfStackID] [uniqueidentifier] NOT NULL,
	[AddedBy] [char](32) NOT NULL,
	[Title] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Url] [nvarchar](2048) NOT NULL,
	[ImageUrl] [nvarchar](2048) NOT NULL CONSTRAINT [DF_ShelfItem_ImageUrl]  DEFAULT (''),
	[Height] [int] NOT NULL CONSTRAINT [DF_ShelfItem_Height]  DEFAULT ((0)),
	[Width] [int] NOT NULL CONSTRAINT [DF_ShelfItem_Width]  DEFAULT ((0)),
	[Source] [nvarchar](256) NOT NULL CONSTRAINT [DF_ShelfItem_Source]  DEFAULT (''),
	[Domain] [nvarchar](32) NOT NULL,
	[AddedTimestamp] [datetime] NOT NULL CONSTRAINT [DF_ShelfItem_AddedTimestamp]  DEFAULT (getdate()),
 CONSTRAINT [PK_ShelfItem] PRIMARY KEY CLUSTERED 
(
	[ShelfStackItemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK_Conversations_Shelf]    Script Date: 01/11/2008 16:06:36 ******/
ALTER TABLE [dbo].[Conversations]  WITH CHECK ADD  CONSTRAINT [FK_Conversations_Shelf] FOREIGN KEY([ShelfStackID])
REFERENCES [dbo].[ShelfStack] ([ShelfStackID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Conversations] CHECK CONSTRAINT [FK_Conversations_Shelf]
GO
/****** Object:  ForeignKey [FK_Conversations_Users]    Script Date: 01/11/2008 16:06:36 ******/
ALTER TABLE [dbo].[Conversations]  WITH CHECK ADD  CONSTRAINT [FK_Conversations_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Conversations] CHECK CONSTRAINT [FK_Conversations_Users]
GO
/****** Object:  ForeignKey [FK_PendingInvites_Shelf]    Script Date: 01/11/2008 16:06:38 ******/
ALTER TABLE [dbo].[PendingInvites]  WITH CHECK ADD  CONSTRAINT [FK_PendingInvites_Shelf] FOREIGN KEY([ShelfStackID])
REFERENCES [dbo].[ShelfStack] ([ShelfStackID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PendingInvites] CHECK CONSTRAINT [FK_PendingInvites_Shelf]
GO
/****** Object:  ForeignKey [FK_ShelfItem_Shelf]    Script Date: 01/11/2008 16:06:52 ******/
ALTER TABLE [dbo].[ShelfStackItem]  WITH CHECK ADD  CONSTRAINT [FK_ShelfItem_Shelf] FOREIGN KEY([ShelfStackID])
REFERENCES [dbo].[ShelfStack] ([ShelfStackID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShelfStackItem] CHECK CONSTRAINT [FK_ShelfItem_Shelf]
GO
/****** Object:  ForeignKey [FK_ShelfItem_Users]    Script Date: 01/11/2008 16:06:52 ******/
ALTER TABLE [dbo].[ShelfStackItem]  WITH CHECK ADD  CONSTRAINT [FK_ShelfItem_Users] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShelfStackItem] CHECK CONSTRAINT [FK_ShelfItem_Users]
GO
/****** Object:  ForeignKey [FK_ShelfOwners_Shelf]    Script Date: 01/11/2008 16:06:55 ******/
ALTER TABLE [dbo].[ShelfStackOwners]  WITH CHECK ADD  CONSTRAINT [FK_ShelfOwners_Shelf] FOREIGN KEY([ShelfStackID])
REFERENCES [dbo].[ShelfStack] ([ShelfStackID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShelfStackOwners] CHECK CONSTRAINT [FK_ShelfOwners_Shelf]
GO
/****** Object:  ForeignKey [FK_ShelfOwners_Users]    Script Date: 01/11/2008 16:06:55 ******/
ALTER TABLE [dbo].[ShelfStackOwners]  WITH CHECK ADD  CONSTRAINT [FK_ShelfOwners_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShelfStackOwners] CHECK CONSTRAINT [FK_ShelfOwners_Users]
GO
