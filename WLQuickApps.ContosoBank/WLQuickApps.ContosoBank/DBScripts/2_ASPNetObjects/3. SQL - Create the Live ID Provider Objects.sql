/******************************************************************************
				CREATE THE TABLES
******************************************************************************/

/****** Object:  Table [dbo].[aspnet_LiveIDAssociation]    Script Date: 05/01/2008 10:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_LiveIDAssociation](
	[LiveID] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ASPNET] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_aspnet_LiveIDAssociation] PRIMARY KEY CLUSTERED 
(
	[LiveID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

/******************************************************************************
				CREATE THE PROCEDURES 
******************************************************************************/

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_AddLiveIDAssociation]    Script Date: 05/01/2008 10:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_AddLiveIDAssociation] (@LiveID nvarchar(256),@UserName nvarchar(256)) AS INSERT INTO dbo.aspnet_LiveIDAssociation (LiveID, ASPNET) SELECT top 1 @LiveID, UserId from dbo.aspnet_Users where UserName = @UserName

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetLiveIDAssociation]    Script Date: 05/01/2008 10:31:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetLiveIDAssociation] (@LiveID nvarchar(256), @UserName nvarchar(256) OUT) AS BEGIN SELECT @UserName = UserName from dbo.aspnet_LiveIDAssociation a, dbo.aspnet_Users b WHERE a.LiveID = @LiveID and a.ASPNET = b.UserId END

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_RemoveLiveIDAssociation]    Script Date: 05/01/2008 10:32:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_RemoveLiveIDAssociation] (@LiveID nvarchar(256)) AS DELETE dbo.aspnet_LiveIDAssociation where LiveId = @LiveID