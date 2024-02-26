IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Branch]') AND type in (N'U'))
DROP TABLE [dbo].[Branch]
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 05/12/2008 11:29:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branch](
	[BranchID] [int] IDENTITY(1,1) NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[Address] [nvarchar](max) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED 
(
	[BranchID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserProfile]') AND type in (N'U'))
DROP TABLE [dbo].[UserProfile]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 05/12/2008 11:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[DisplayName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Avatar] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Postcode] [nvarchar](6) COLLATE Latin1_General_CI_AS NOT NULL,
	[Country] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
	[LiveID] [nvarchar](256) COLLATE Latin1_General_CI_AS NULL,
	[Rating] [int] NOT NULL,
	[UserProfileID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
(
	[UserProfileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Videos]') AND type in (N'U'))
DROP TABLE [dbo].[Videos]
GO
/****** Object:  Table [dbo].[Videos]    Script Date: 05/12/2008 11:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Videos](
	[VideoId] [int] IDENTITY(1,1) NOT NULL,
	[VideoTitle] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[VideoURL] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[NumViews] [int] NOT NULL,
	[Rating] [int] NOT NULL,
	[FrameImage] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[UploadDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Videos] PRIMARY KEY CLUSTERED 
(
	[VideoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ForumSubject]') AND type in (N'U'))
DROP TABLE [dbo].[ForumSubject]
GO
/****** Object:  Table [dbo].[ForumSubject]    Script Date: 05/12/2008 11:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForumSubject](
	[ForumSubjectID] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](1000) COLLATE Latin1_General_CI_AS NOT NULL,
	[SubjectType] [int] NOT NULL,
	[NumViews] [int] NOT NULL,
	[UserProfileID] [uniqueidentifier] NOT NULL,
	[PostDate] [datetime] NOT NULL,
	[IsStickyPost] [bit] NOT NULL,
 CONSTRAINT [PK_ForumSubject] PRIMARY KEY CLUSTERED 
(
	[ForumSubjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ForumSubject]  WITH CHECK ADD  CONSTRAINT [FK_ForumSubject_UserProfile] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[UserProfile] ([UserProfileID])
GO
ALTER TABLE [dbo].[ForumSubject] CHECK CONSTRAINT [FK_ForumSubject_UserProfile]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ForumReply]') AND type in (N'U'))
DROP TABLE [dbo].[ForumReply]
GO
/****** Object:  Table [dbo].[ForumReply]    Script Date: 05/12/2008 11:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForumReply](
	[ForumReplyID] [int] IDENTITY(1,1) NOT NULL,
	[ForumSubject] [int] NOT NULL,
	[ReplyText] [nvarchar](1000) COLLATE Latin1_General_CI_AS NOT NULL,
	[Tags] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[UserProfileID] [uniqueidentifier] NOT NULL,
	[ReplyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ForumReply] PRIMARY KEY CLUSTERED 
(
	[ForumReplyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ForumReply]  WITH CHECK ADD  CONSTRAINT [FK_ForumReply_ForumSubject] FOREIGN KEY([ForumSubject])
REFERENCES [dbo].[ForumSubject] ([ForumSubjectID])
GO
ALTER TABLE [dbo].[ForumReply] CHECK CONSTRAINT [FK_ForumReply_ForumSubject]
GO
ALTER TABLE [dbo].[ForumReply]  WITH CHECK ADD  CONSTRAINT [FK_ForumReply_UserProfile] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[UserProfile] ([UserProfileID])
GO
ALTER TABLE [dbo].[ForumReply] CHECK CONSTRAINT [FK_ForumReply_UserProfile]
GO
