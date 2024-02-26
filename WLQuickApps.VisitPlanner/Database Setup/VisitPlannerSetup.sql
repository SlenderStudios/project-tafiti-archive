-- What is in this script
---- CREATES the objects
---- IMPORTS the data (deletes all content)

--------------------------------------------------------------------
-- SCHEMA AND STRUCTURE
--
-- Was generated from the VisitPlannerDev database backup from
-- Infusion 200802221215
--------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UserExists]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		Vincent Ngo
-- Create date: Oct 16, 2007
-- Description:	Checks if Windows Live UserID exists in database
-- =============================================
CREATE PROCEDURE [dbo].[sp_UserExists]

	@LiveID VARCHAR(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if EXISTS(SELECT * FROM Users WHERE WindowsLiveID = @LiveID)
		return 1
	else
		return 0
END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ClearTables]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ClearTables]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM Users
	DELETE FROM UserToCollection
	DELETE FROM Collections
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SaveCollection]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SaveCollection]
	@collectionXML AS NTEXT, @collectionID AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Collections SET Collection = @collectionXML WHERE CollectionID = @collectionID

END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Concierge]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Concierge](
	[ConciergeID] [int] IDENTITY(1,1) NOT NULL,
	[ConciergeCollection] [ntext] NULL,
 CONSTRAINT [PK_Concierge] PRIMARY KEY CLUSTERED 
(
	[ConciergeID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Destinations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Destinations](
	[DestinationID] [int] IDENTITY(1,1) NOT NULL,
	[DestinationName] [varchar](50) NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[DestinationID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Collections]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Collections](
	[CollectionID] [int] IDENTITY(1,1) NOT NULL,
	[Collection] [ntext] NULL,
 CONSTRAINT [PK_Collections] PRIMARY KEY CLUSTERED 
(
	[CollectionID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[WindowsLiveID] [varchar](50) NOT NULL,
	[UserType] [varchar](50) NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
 CONSTRAINT [PK_Users_1] PRIMARY KEY CLUSTERED 
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DestinationToConcierge]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DestinationToConcierge](
	[DestinationID] [int] NOT NULL,
	[ConciergeID] [int] NOT NULL,
 CONSTRAINT [PK_LocationToConcierge] PRIMARY KEY CLUSTERED 
(
	[DestinationID] ASC,
	[ConciergeID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserToCollection]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserToCollection](
	[CollectionID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[DestinationID] [int] NOT NULL,
 CONSTRAINT [PK_UserToCollection] PRIMARY KEY CLUSTERED 
(
	[CollectionID] ASC,
	[UserID] ASC,
	[DestinationID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetConciergeByDestination]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetConciergeByDestination]
	@destinationID AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	SELECT     Concierge.ConciergeID, Concierge.ConciergeCollection
FROM         Concierge INNER JOIN
                      DestinationToConcierge ON Concierge.ConciergeID = DestinationToConcierge.ConciergeID
WHERE     (DestinationToConcierge.DestinationID = @destinationID)
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetConcierge]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetConcierge]
	@conciergeID AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	SELECT * FROM Concierge WHERE ConciergeID = @conciergeID
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SaveConcierge]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SaveConcierge]
	@collectionXML AS NTEXT, @destinationID AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp int;
	SET @temp = (
	SELECT ConciergeID 
	FROM DestinationToConcierge
	WHERE DestinationID = @destinationID
	)

    UPDATE Concierge SET ConciergeCollection = @collectionXML WHERE ConciergeID = @temp

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetCollections]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		Vincent Ngo
-- Create date: Oct 18, 2007
-- Description:	Returns a collection
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCollections]
	@userID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM UsertoCollection INNER JOIN Collections ON UsertoCollection.CollectionID = Collections.CollectionID
	WHERE UsertoCollection.UserID = @userID
END




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetCollection]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		Vincent Ngo
-- Create date: Oct 18, 2007
-- Description:	Returns a collection
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCollection]
	@collectionID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Collections
	WHERE CollectionID = @collectionID
END




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertCollection]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertCollection]
	@collectionXML AS NTEXT, @UserID AS VARCHAR(50), @DestinationID AS VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Collections VALUES(@collectionXML)
	--DECLARE @collID = @@identity
	--DECLARE @userID INT
	--SELECT @userID = UserID FROM Users WHERE WindowsLiveID = @LiveID
	INSERT INTO UserToCollection (UserID,CollectionID,DestinationID) VALUES(@userID,@@identity,@destinationID)
	--SELECT * FROM Collections WHERE Collections = @CollectionID

END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetDestinationsByUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		Vincent Ngo
-- Create date: Oct 18, 2007
-- Description:	Returns a collection
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetDestinationsByUser]
	@userID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT     Destinations.DestinationID, Destinations.DestinationName
FROM         UserToCollection INNER JOIN
                      Destinations ON UserToCollection.DestinationID = Destinations.DestinationID
WHERE     (UserToCollection.UserID = @userID)
GROUP BY Destinations.DestinationID, Destinations.DestinationName
END




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetDestinations]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:        Infusion Development
-- Create date: December 9, 2007
-- Description:   Gets a list of the destinations
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetDestinations]

AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT DestinationID, DestinationName FROM Destinations
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetUserByLiveID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





-- =============================================
-- Author:		Vincent Ngo
-- Create date: Oct 18, 2007
-- Description:	Returns a single user 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUserByLiveID]
	@LiveID VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT     Users.UserID, Users.WindowsLiveID, Users.UserType, Users.FirstName, Users.LastName, UserToCollection.CollectionID, UserToCollection.DestinationID
FROM         Users LEFT OUTER JOIN
                      UserToCollection ON Users.UserID = UserToCollection.UserID
WHERE     (Users.WindowsLiveID = @LiveID)
END






' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		Vincent Ngo
-- Create date: Oct 18, 2007
-- Description:	Returns a single user 
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUser]
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT     Users.UserID, Users.WindowsLiveID, Users.UserType, Users.FirstName, Users.LastName, UserToCollection.CollectionID, UserToCollection.DestinationID
FROM         Users LEFT OUTER JOIN
                      UserToCollection ON Users.UserID = UserToCollection.UserID
WHERE     (Users.UserID = @UserID)
END






' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SavePersonal]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SavePersonal]
	@firstName AS VARCHAR(50), @lastName AS VARCHAR(50), @UserID AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Users SET FirstName = @firstName, LastName = @lastName WHERE UserID = @UserID
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


-- =============================================
-- Author:        Vincent Ngo
-- Create date: Oct 15, 2007
-- Description:   Stores the WindowsLiveID UserID as well as a user type
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertUser]
(
      @LiveID AS VARCHAR(50), @UserType AS VARCHAR(50)
)
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    -- Insert statements for procedure here
      
      INSERT INTO Users VALUES (@LiveID, @UserType, '''', '''')
      EXEC sp_GetUserByLiveID @LiveID
END
' 
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LocationToConcierge_Concierge]') AND parent_object_id = OBJECT_ID(N'[dbo].[DestinationToConcierge]'))
ALTER TABLE [dbo].[DestinationToConcierge]  WITH CHECK ADD  CONSTRAINT [FK_LocationToConcierge_Concierge] FOREIGN KEY([ConciergeID])
REFERENCES [dbo].[Concierge] ([ConciergeID])
GO
ALTER TABLE [dbo].[DestinationToConcierge] CHECK CONSTRAINT [FK_LocationToConcierge_Concierge]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LocationToConcierge_Locations]') AND parent_object_id = OBJECT_ID(N'[dbo].[DestinationToConcierge]'))
ALTER TABLE [dbo].[DestinationToConcierge]  WITH CHECK ADD  CONSTRAINT [FK_LocationToConcierge_Locations] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destinations] ([DestinationID])
GO
ALTER TABLE [dbo].[DestinationToConcierge] CHECK CONSTRAINT [FK_LocationToConcierge_Locations]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserToCollection_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserToCollection]'))
ALTER TABLE [dbo].[UserToCollection]  WITH CHECK ADD  CONSTRAINT [FK_UserToCollection_Collections] FOREIGN KEY([CollectionID])
REFERENCES [dbo].[Collections] ([CollectionID])
GO
ALTER TABLE [dbo].[UserToCollection] CHECK CONSTRAINT [FK_UserToCollection_Collections]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserToCollection_Destinations]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserToCollection]'))
ALTER TABLE [dbo].[UserToCollection]  WITH CHECK ADD  CONSTRAINT [FK_UserToCollection_Destinations] FOREIGN KEY([DestinationID])
REFERENCES [dbo].[Destinations] ([DestinationID])
GO
ALTER TABLE [dbo].[UserToCollection] CHECK CONSTRAINT [FK_UserToCollection_Destinations]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserToCollection_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserToCollection]'))
ALTER TABLE [dbo].[UserToCollection]  WITH CHECK ADD  CONSTRAINT [FK_UserToCollection_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UserToCollection] CHECK CONSTRAINT [FK_UserToCollection_Users]
GO



----------------------------------------------------------------------------------------
--											DATA
--
-- This script was generated whilst playing with SQL Data Compare
-- for more information see http://www.red-gate.com/products/SQL_Data_Compare/index.htm
--
--
-- It deletes all the data and then reimports the baseline data.
----------------------------------------------------------------------------------------
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO

-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)

BEGIN TRANSACTION

-- Drop constraints from [dbo].[DestinationToConcierge]
ALTER TABLE [dbo].[DestinationToConcierge] DROP CONSTRAINT [FK_LocationToConcierge_Concierge]
ALTER TABLE [dbo].[DestinationToConcierge] DROP CONSTRAINT [FK_LocationToConcierge_Locations]

-- Drop constraint FK_UserToCollection_Destinations from [dbo].[UserToCollection]
ALTER TABLE [dbo].[UserToCollection] DROP CONSTRAINT [FK_UserToCollection_Destinations]

-- Add 3 rows to [dbo].[Concierge]
SET IDENTITY_INSERT [dbo].[Concierge] ON
EXEC(N'INSERT INTO [dbo].[Concierge] ([ConciergeID], [ConciergeCollection]) VALUES (1, N''<?xml version="1.0"?><rss version="2.0" xmlns:geo="http://www.w3.org/2003/01/geo/wgs84_pos#"><channel><item><category>Accomodation</category><title>Contoso Hotel</title><videoURL /><imageURL>DemoImages/90A2853B6CFBCC2AFC987BB18B832.jpg</imageURL><pushpinURL>images/homeLocation.png</pushpinURL><shortDescription>Contemporary, sophisticated hotel situated in the heart of the famous Strip</shortDescription><longDescription>Contemporary, sophisticated hotel situated in the heart of the famous Strip</longDescription><addressLine1>3131 Las Vegas Boulevard S</addressLine1><addressLine2>Las Vegas, NV 89109</addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords>Accomodation Hotel Room Bed Double Queen</keywords><geo:Point xmlns:geo="geo"><geo:lat>36.1173060856917</geo:lat><geo:long>-115.172931067938</geo:long></geo:Point></item><item><category>Movie</category><title>Mystere by Cirque Du Soleil</title><videoURL /><imageURL>images/location_flower.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>Music, comedy, acrobatics and dance show performed by members of Cirque du Soleil in a theater built specifically for this performance.</shortDescription><longDescription>Music, comedy, acrobatics and dance show performed by members of Cirque du Soleil in a theater built specifically for this performance.</longDescription><addressLine1>3300 Las Vegas Blvd S</addressLine1><addressLine2>Las Vegas, NV</addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords>Music comedy acrobatics</keywords><geo:Point xmlns:geo="geo"><geo:lat>36.1241</geo:lat><geo:long>-115.171401</geo:long></geo:Point></item><item><category>Movie</category><title>Blue Man Group</title><videoURL /><imageURL>images/location_flower.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>An imaginative stage show that combines percussion-driven music, flashy special effects and instruments made from plastic piping into a visual extravaganza.</shortDescription><longDescription>An imaginative stage show that combines percussion-driven music, flashy special effects and instruments made from plastic piping into a visual extravaganza.</longDescription><addressLine1>3355 Las Vegas Blvd S</addressLine1><addressLine2>Las Vegas, NV </addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords>Theater Stage Music</keywords><geo:Point xmlns:geo="geo"><geo:lat>36.121401</geo:lat><geo:long>-115.1704</geo:long></geo:Point></item><item><category>Movie</category><title>IMAX Theatre &amp; Ridefilm</title><videoURL /><imageURL>DemoImages/entrance.jpg</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>This first Imax theatre in Las Vegas has a seven-story screen and 30,000 watts of sound.</shortDescription><longDescription>This first Imax theatre in Las Vegas has a seven-story screen and 30,000 watts of sound.</longDescription><addressLine1>3900 Las Vegas Blvd. S</addressLine1><addressLine2>Las Vegas, NV </addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords>Movie IMAX Theater</keywords><geo:Point xmlns:geo="geo"><geo:lat>36.096101</geo:lat><geo:long>-115.174501</geo:long></geo:Point></item><item><category>Misc</category><title>Shark Reef at Mandalay Bay</title><videoURL /><imageURL>DemoImages/another-shark.jpg</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>Located at the Mandalay Bay Hotel, this aquarium is home to several varieties of tropical fish and sharks.</shortDescription><longDescription>Located at the Mandalay Bay Hotel, this aquarium is home to several varieties of tropical fish and sharks.</longDescription><addressLine1>3950 Las Vegas Blvd. S</addressLine1><addressLine2>Las Vegas, NV </addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords>Fish Aquarium Entertainment</keywords><geo:Point xmlns:geo="geo"><geo:lat>36.0923</geo:lat><geo:long>-115.174201</'')')
EXEC(N'DECLARE @pv binary(16)
'+N'SELECT @pv=TEXTPTR([ConciergeCollection]) FROM [dbo].[Concierge] WHERE [ConciergeID]=1
UPDATETEXT [dbo].[Concierge].[ConciergeCollection] @pv NULL NULL N''geo:long></geo:Point></item><item><category>Food</category><title>GameWorks</title><videoURL /><imageURL>images/location_flower.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>Eat, drink and be merry at this high-energy entertainment complex that offers dining, drinking, partying and cutting-edge game technology and attractions.</shortDescription><longDescription>Eat, drink and be merry at this high-energy entertainment complex that offers dining, drinking, partying and cutting-edge game technology and attractions.</longDescription><addressLine1>3785 Las Vegas Blvd. S</addressLine1><addressLine2>Las Vegas, NV </addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords>Eat drink food</keywords><geo:Point xmlns:geo="geo"><geo:lat>36.102801</geo:lat><geo:long>-115.1728</geo:long></geo:Point></item><item><category>Misc</category><title>Forum Shops at Caesar''''s Palace</title><videoURL /><imageURL>images/location_flower.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>This busy shopping mall was built to resemble a street from Ancient Rome.</shortDescription><longDescription>This busy shopping mall was built to resemble a street from Ancient Rome.</longDescription><addressLine1>3570 Las Vegas Blvd S</addressLine1><addressLine2>Las Vegas, NV </addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords /><geo:Point xmlns:geo="geo"><geo:lat>36.11635</geo:lat><geo:long>-115.172799</geo:long></geo:Point></item><item><category>Art</category><title>Fashion Show Mall</title><videoURL /><imageURL>images/location_flower.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>Great mall in the heart of Las Vegas.</shortDescription><longDescription>Great mall in the heart of Las Vegas.</longDescription><addressLine1>3200 Las Vegas Blvd. S </addressLine1><addressLine2>Las Vegas, NV </addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords>Mall Shopping</keywords><geo:Point xmlns:geo="geo"><geo:lat>36.1255353430929</geo:lat><geo:long>-115.169334411621</geo:long></geo:Point></item><item><category>Food</category><title>Morton''''s steakhouse</title><videoURL /><imageURL>images/location_flower.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>Morton''''s steakhouse</shortDescription><longDescription>Morton''''s steakhouse</longDescription><addressLine1>400 E. Flamingo</addressLine1><addressLine2>Las Vegas, NV </addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords>Food Steak Dining</keywords><geo:Point xmlns:geo="geo"><geo:lat>36.1144025937406</geo:lat><geo:long>-115.153997</geo:long></geo:Point></item><item><category>Food</category><title>Emeril''''s New Orleans Fish House</title><videoURL /><imageURL>images/location_flower.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>Emeril''''s New Orleans Fish House</shortDescription><longDescription>Emeril''''s New Orleans Fish House</longDescription><addressLine1>3799 Las Vegas Blvd. S</addressLine1><addressLine2>Las Vegas, NV </addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords>Food Dining Fish</keywords><geo:Point xmlns:geo="geo"><geo:lat>36.1019603582521</geo:lat><geo:long>-115.168218612671</geo:long></geo:Point></item><item><category>Food</category><title>Bartolotta Ristorante di Mare</title><videoURL /><imageURL>images/location_flower.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>Bartolotta Ristorante di Mare</shortDescription><longDescription>Bartolotta Ristorante di Mare</longDescription><addressLine1>3131 Las Vegas Blvd. S</addressLine1><addressLine2>Las Vegas, NV </addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords /><geo:Point xmlns:geo="geo"><geo:lat>36.1272685185289</geo:lat><geo:long>-115.166673660278</geo:long></geo:Point></item><item><category>Misc</category><title>''
UPDATETEXT [dbo].[Concierge].[ConciergeCollection] @pv NULL NULL N''PURE Nightclub</title><videoURL /><imageURL>images/location_flower.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>PURE Nightclub</shortDescription><longDescription>PURE Nightclub</longDescription><addressLine1>3570 S. Las Vegas Blvd</addressLine1><addressLine2>Las Vegas, NV </addressLine2><date /><recurrence>Daily</recurrence><duration>60</duration><keywords>Dancing Night Club</keywords><geo:Point xmlns:geo="geo"><geo:lat>36.1163142064647</geo:lat><geo:long>-115.17276763916</geo:long></geo:Point></item><item><category>Misc</category><title>Las Vegas Motor Speedway</title><videoURL>DemoImages/speedway.wmv</videoURL><imageURL>images/Btn_PushPin.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>Home to several NASCAR events, this race car track also offers a driving school.</shortDescription><longDescription>Come on weekends to me professional F1 racer Parl Tracy!</longDescription><addressLine1 /><addressLine2 /><date /><recurrence>Daily</recurrence><duration>60</duration><keywords /><geo:Point xmlns:geo="geo"><geo:lat>36.1752449430044</geo:lat><geo:long>-115.136713385582</geo:long></geo:Point></item><item><category>Misc</category><title>Las Vegas Country Club</title><videoURL /><imageURL>images/Btn_PushPin.png</imageURL><pushpinURL>images/Btn_PushPin.png</pushpinURL><shortDescription>Good times</shortDescription><longDescription /><addressLine1 /><addressLine2 /><date /><recurrence>Daily</recurrence><duration>60</duration><keywords /><geo:Point xmlns:geo="geo"><geo:lat>36.1344781176067</geo:lat><geo:long>-115.145988464355</geo:long></geo:Point></item></channel></rss>''
')
EXEC(N'INSERT INTO [dbo].[Concierge] ([ConciergeID], [ConciergeCollection]) VALUES (2, N''<?xml version="1.0"?><rss version="2.0" xmlns:geo="http://www.w3.org/2003/01/geo/wgs84_pos#"><channel><item><category>Accomodation</category><title>Contoso Hotel</title><imageURL>images/location_flower.png</imageURL><pushpinURL>images/homeLocation.png</pushpinURL><shortDescription>Contemporary, sophisticated hotel situated in the heart of the city</shortDescription><longDescription>Contemporary, sophisticated hotel situated in the heart of the city</longDescription><addressLine1>1901 5th Ave</addressLine1><addressLine2>Seattle, WA 98101</addressLine2><date></date><keywords>Accomodation Hotel Room Bed Double Queen</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.612943</geo:lat><geo:long>-122.33794</geo:long></geo:Point></item><item><category>Music</category><title>Experience Music Project</title><imageURL>DemoImages/emp.jpg</imageURL><shortDescription>Experience Music Project (EMP) is dedicated to the exploration of creativity and innovation in popular music.</shortDescription><longDescription>Experience Music Project (EMP) is dedicated to the exploration of creativity and innovation in popular music. By blending interpretative, interactive exhibition with cutting-edge technology, EMP captures and reflects the essence of rock ‘n’ roll, its roots in jazz, soul, gospel, country and the blues, as well as rock’s influence on hip-hop, punk and other recent genres. Visitors can view rare artifacts and memorabilia and experience the creative process by listening to musicians tell their own stories.</longDescription><addressLine1>325 5th Ave N</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Music Museum</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.621068</geo:lat><geo:long>-122.347587</geo:long></geo:Point></item><item><category>Misc</category><title>Space Needle</title><imageURL>DemoImages/spaceneedle.jpg</imageURL><shortDescription>The symbol of Seattle, and one of the most recognizable structures in the world.</shortDescription><longDescription>Built in 1962, the Space Needle served as the symbol of that year''''s World''''s Fair.  It has since become the symbol of Seattle, and one of the most recognizable structures in the world.  The privately owned Space Needle is managed by the Space Needle Corporation.</longDescription><addressLine1>400 Broad St</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Attraction Monument</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.6198</geo:lat><geo:long>-122.348809</geo:long></geo:Point></item><item><category>Food</category><title>The Pink Door</title><imageURL>DemoImages/pinkdoor.jpg</imageURL><shortDescription>The Pink Door tempts with homespun Italian-American food,conversation that flows liberally as the Baralo, provocative -always free- live entertainment, and a warm, lively respite from the ordinary world.</shortDescription><longDescription>The Pink Door tempts with homespun Italian-American food,conversation that flows liberally as the Baralo, provocative -always free- live entertainment, and a warm, lively respite from the ordinary world.</longDescription><addressLine1>1919 Post Alley</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Restaurant Eat Food Drink Entertainment</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.610127</geo:lat><geo:long>-122.342046</geo:long></geo:Point></item><item><category>Music</category><title>Dimitriou''''s Jazz Alley</title><imageURL>images/location_flower.png</imageURL><shortDescription>Seattle jazz nightclub and restaurant.</shortDescription><longDescription>Seattle jazz nightclub and restaurant established in 1979.</longDescription><addressLine1>2033 6th Ave</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Restaurant Food Music Entertainment</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.614638</geo:lat><geo:long>-122.338732</geo:long></geo:Point></item><item><category>Misc</'')')
EXEC(N'DECLARE @pv binary(16)
'+N'SELECT @pv=TEXTPTR([ConciergeCollection]) FROM [dbo].[Concierge] WHERE [ConciergeID]=2
UPDATETEXT [dbo].[Concierge].[ConciergeCollection] @pv NULL NULL N''category><title>Gas Works Park</title><imageURL>images/location_flower.png</imageURL><shortDescription>Gas Works Park has a play area with a large play barn, and big hill popular for flying kites.</shortDescription><longDescription>Gas Works Park has a play area with a large play barn, and big hill popular for flying kites. Special park features include a sundial, and a beautiful view of Seattle. Burke-Gilman Trail runs past Gas Works parking lot and follows the Burlington-Northern Railroad 12.5 miles north to Kirkland Log Boom Park.</longDescription><addressLine1>2101 N Northlake Way</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Park Kite Fly Picnic</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.646862</geo:lat><geo:long>-122.333883</geo:long></geo:Point></item><item><category>Misc</category><title>Westlake Center</title><imageURL>images/location_flower.png</imageURL><shortDescription>Visit Westlake Center located in Seattle, Washington for the best in shopping, entertainment and dining!</shortDescription><longDescription>Visit Westlake Center located in Seattle, Washington for the best in shopping, entertainment and dining!</longDescription><addressLine1>400 Pine St</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Shopping Dinning Monorail Cafe</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.61118</geo:lat><geo:long>-122.337659</geo:long></geo:Point></item><item><category>Misc</category><title>Banya</title><imageURL>images/location_flower.png</imageURL><shortDescription>Banya5 is an urban spa and health facility which offers a unique blend of old world wellness rituals in a friendly contemporary environment.</shortDescription><longDescription>Banya 5 is an urban spa and health facility which offers a unique blend of old world wellness rituals in a friendly contemporary environment. A series of relaxing wet heat and dry heat experiences coupled with exhilarating pool plunges produce a vivid circulatory experience with numerous mental and physical health benefits. Banya 5 is an environment which promotes wellness, vitality and rejuvenation of the soul.</longDescription><addressLine1>217 9th Ave N</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Spa Relaxation Swim Massage</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.619901</geo:lat><geo:long>-122.339772</geo:long></geo:Point></item><item><category>Food</category><title>Agua Verde Cafe & Paddle Club</title><imageURL>images/location_flower.png</imageURL><shortDescription>The Agua Verde paddle club and cafe offers kayak rentals and great Mexican food on the waterfront of Seattle, Washington.</shortDescription><longDescription>We offer creative, healthy Mexican food in a casual, waterfront setting. Sea kayaks are available for hourly rental downstairs. Paddle past eclectic floating homes with vistas of the Seattle skyline in the background.</longDescription><addressLine1>1303 NE Boat St</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Park Kayak Rental Food Cafe</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.651811</geo:lat><geo:long>-122.314539</geo:long></geo:Point></item><item><category>Misc</category><title>REI</title><imageURL>images/location_flower.png</imageURL><shortDescription>More than a store, we rank among the Emerald City''''s top sightseeing attractions.</shortDescription><longDescription>Serving Seattleites since 1944. More than a store, we rank among the Emerald City''''s top sightseeing attractions. Here you''''ll find the motherlode of gear—plus a mountain bike test trail, gear-testing stations, a 65-foot climbing wall and more hands-on fun. Come let our friendly, knowledgeable staff help equip you for your next adventure!</longDescription><addressLine1>222 Yale Ave N</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Shopping Outdoor Adventure</keywords><recurrence>Dai''
UPDATETEXT [dbo].[Concierge].[ConciergeCollection] @pv NULL NULL N''ly</recurrence><geo:Point><geo:lat>47.619918</geo:lat><geo:long>-122.330413</geo:long></geo:Point></item><item><category>Misc</category><title>Nordstrom</title><imageURL>images/location_flower.png</imageURL><shortDescription></shortDescription><longDescription>Opened as a shoe store in 1901, this department store chain is now one of the nation''''s leading fashion retailers, offering fine apparel and accessories for everyone in the family. The stores are sleek and modern, mostly decorated in marble and wood. Shoes remain the specialty, with a huge selection that includes unusual sizes. Designer labels like Ralph Lauren, Donna Karan and Calvin Klein fill the clothing racks.</longDescription><addressLine1>1601 2nd Ave</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Shopping Shoes</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.610302</geo:lat><geo:long>-122.339771</geo:long></geo:Point></item><item><category>Food</category><title>Pike Place Fish Market</title><imageURL>images/location_flower.png</imageURL><shortDescription>Located in Pike Place Market in Seattle, they have a crew of fish mongers who throw fish to entertain customers.</shortDescription><longDescription>We are the Seattle fresh fish company that everyone talks about. Besides offering only the best quality, freshest seafood, our dedication to having fun and creating excitement while we work makes us "world famous."</longDescription><addressLine1>86 Pike Place</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Food Eat Drink Fish</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.608833</geo:lat><geo:long>-122.34007</geo:long></geo:Point></item><item><category>Food</category><title>Purple Cafe and Wine Bar</title><imageURL>images/location_flower.png</imageURL><shortDescription>A multifaceted food and wine concept that merges casual sophistication with an upbeat metropolitan style.</shortDescription><longDescription>A multifaceted food and wine concept that merges casual sophistication with an upbeat metropolitan style. Purple features a global wine selection coupled with a menu that blends classic American styles, seasonal northwest ingredients and mediterranean themes.</longDescription><addressLine1>1225 4th Ave</addressLine1><addressLine2>Seattle, WA</addressLine2><date></date><keywords>Restaurant Drink Eat Food</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>47.607677</geo:lat><geo:long>-122.334392</geo:long></geo:Point></item></channel></rss>''
')
EXEC(N'INSERT INTO [dbo].[Concierge] ([ConciergeID], [ConciergeCollection]) VALUES (3, N''<?xml version="1.0"?><rss version="2.0" xmlns:geo="http://www.w3.org/2003/01/geo/wgs84_pos#"><channel><item><category>Accomodation</category><title>Contoso Hotel</title><imageURL>images/location_flower.png</imageURL><pushpinURL>images/homeLocation.png</pushpinURL><shortDescription>Contemporary, sophisticated hotel situated in the heart of the city</shortDescription><longDescription>Contemporary, sophisticated hotel situated in the heart of the city</longDescription><addressLine1>1650 Broadway</addressLine1><addressLine2>New York City, NY 10019</addressLine2><date></date><keywords>Accomodation Hotel Room Bed Double Queen</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.762071</geo:lat><geo:long>-73.983628</geo:long></geo:Point></item><item><category>Misc</category><title>Takashimaya</title><imageURL>images/location_flower.png</imageURL><shortDescription>A unique commingling of Eastern and Western sensibilities inspires the rare and wonderful designs and artisan-made pieces culled from around the globe.</shortDescription><longDescription>To visit Takashimaya New York is to enter a world apart. A unique commingling of Eastern and Western sensibilities inspires the rare and wonderful designs and artisan-made pieces culled from around the globe. Displaying the unusual sensitivity of the Japanese towards refined craftsmanship and beautiful packaging, and the modern appreciation for functional design, shopping online transcends the usual boundaries. At Takashimaya, the world is at your fingertips.</longDescription><addressLine1>693 5th Ave</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Shopping Japan</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.76131</geo:lat><geo:long>-73.975173</geo:long></geo:Point></item><item><category>Misc</category><title>Barney''''s of New York</title><imageURL>images/location_flower.png</imageURL><shortDescription>Barneys New York for the latest fashions from top designers.</shortDescription><longDescription>Barneys New York for the latest fashions from top designers such as Lanvin, Diane von Furstenberg, Fendi, Christian Louboutin, Chloe and more!</longDescription><addressLine1>575 5th Ave</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Shopping Fashion Designers</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.75603</geo:lat><geo:long>-73.979029</geo:long></geo:Point></item><item><category>Art</category><title>Museum Of Modern Art</title><imageURL>images/location_flower.png</imageURL><shortDescription>Founded in 1929 as an educational institution, The Museum of Modern Art is dedicated to being the foremost museum of modern art in the world.</shortDescription><longDescription>Founded in 1929 as an educational institution, The Museum of Modern Art is dedicated to being the foremost museum of modern art in the world. Through the leadership of its trustees and staff, The Museum of Modern Art manifests this commitment by establishing, preserving, and documenting a permanent collection of the highest order that reflects the vitality, complexity, and unfolding patterns of modern and contemporary art; by presenting exhibitions and educational programs of unparalleled significance; by sustaining a library, archives, and conservation laboratory that are recognized as international centers of research; and by supporting scholarship and publications of preeminent intellectual merit. </longDescription><addressLine1>11 W 53rd St</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Museum Art Culture</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.760552</geo:lat><geo:long>-73.97616</geo:long></geo:Point></item><item><category>Art</category><title>Museum of Natural History</title><imageURL>DemoImages/anmh.jpg</imageURL><shortDescription>The American Museum of Natural History</shortDescription><longDescription>The American Museum of Natural History is one of the wor'')')
EXEC(N'DECLARE @pv binary(16)
'+N'SELECT @pv=TEXTPTR([ConciergeCollection]) FROM [dbo].[Concierge] WHERE [ConciergeID]=3
UPDATETEXT [dbo].[Concierge].[ConciergeCollection] @pv NULL NULL N''ld''''s preeminent institutions for scientific research and education, with collections of more than 32 million specimens and artifacts.</longDescription><addressLine1>175 Central Park W</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Museum Art Culture</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.779707</geo:lat><geo:long>-73.973401</geo:long></geo:Point></item><item><category>Misc</category><title>Great Jones Spa</title><imageURL>DemoImages/jonesspa.jpg</imageURL><shortDescription>The Great Jones Spa incorporates the Eastern ideas of spiritual harmony with the best in therapies from around the world to create a unique experience for you.</shortDescription><longDescription>The Great Jones Spa incorporates the Eastern ideas of spiritual harmony with the best in therapies from around the world to create a unique experience for you.</longDescription><addressLine1>29 Great Jones St</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords></keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.727157</geo:lat><geo:long>-73.993387</geo:long></geo:Point></item><item><category>Food</category><title>Trattoria Dell''''Arte</title><imageURL>images/location_flower.png</imageURL><shortDescription>Across from Carnegie Hall, Trattoria Dell''''Arte features the largest antipasto bar in New York, a sprawling selection of seafood and vegetable specialties perfect for parties of all sizes.</shortDescription><longDescription>Across from Carnegie Hall, Trattoria Dell''''Arte features the largest antipasto bar in New York, a sprawling selection of seafood and vegetable specialties perfect for parties of all sizes. Designed after a Tuscan artist''''s studio, the rooms include half-finished paintings, oversized sculptures of fragmented body parts, a gallery of Italian noses, a wine cellar dining room, and a candle-filled private room.</longDescription><addressLine1>900 7th Ave</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Food Drink Entertainment</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.765531</geo:lat><geo:long>-73.980038</geo:long></geo:Point></item><item><category>Music</category><title>Birdland Jazz Club</title><imageURL>DemoImages/birdland.jpg</imageURL><shortDescription>Jazz landmark club established in New York in 1949, named after Charlie "Yardbird" Parker.</shortDescription><longDescription>Jazz landmark club established in New York in 1949, named after Charlie "Yardbird" Parker.</longDescription><addressLine1>315 W 44th St</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Music Jazz Entertainment</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.758979</geo:lat><geo:long>-73.989885</geo:long></geo:Point></item><item><category>Food</category><title>Levain''''s Bakery</title><imageURL>images/location_flower.png</imageURL><shortDescription>Little Levain Bakery sells really big chocolate chip cookies... </shortDescription><longDescription>Little Levain Bakery sells really big chocolate chip cookies... </longDescription><addressLine1>167 W 74th St</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Food Cookie Desert</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.779825</geo:lat><geo:long>-73.980449</geo:long></geo:Point></item><item><category>Misc</category><title>Empire State Building</title><imageURL>DemoImages/statueliberty.jpg</imageURL><shortDescription>New York''''s famous Empire State Building, a New York City Landmark and a National Historic Landmark, soars more than a quarter of a mile into the atmosphere above the heart of Manhattan.</shortDescription><longDescription>The Empire State Building is cemented in both New York and U.S. History. Built during the Depression, the building was the center of a competition between Walter Chrysler (Chrysler Corp.) and John Jakob Raskob (creator of General Motors) to see who could b''
UPDATETEXT [dbo].[Concierge].[ConciergeCollection] @pv NULL NULL N''uild the tallest building.</longDescription><addressLine1>350 Fifth Avenue</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Monument Attraction Shopping Office</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.74844</geo:lat><geo:long>-73.984559</geo:long></geo:Point></item><item><category>Food</category><title>Gotham Grill</title><imageURL>images/location_flower.png</imageURL><shortDescription>New York City dining at its finest.</shortDescription><longDescription>What began as a raucous eighties-style grand cafe has mellowed over two decades into a trusted American classic with Alfred Portale relentlessly pursuing perfection in the kitchen.</longDescription><addressLine1>12 E 12th St</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Food Entertainment Drink Restaurant</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.734377</geo:lat><geo:long>-73.993882</geo:long></geo:Point></item><item><category>Art</category><title>Angel of the Waters Fountain</title><imageURL>DemoImages/angelofwater.jpg</imageURL><shortDescription>Emma Stebbins water fountain sculpture.</shortDescription><longDescription>Located at mid-Park on the north side of 72nd Street, Angel of the Waters Fountain at Bethesda Terrace was placed in the Park in 1873. Today the fountain is one of the favorite places in Central Park for wedding pictures and romantic walks.</longDescription><addressLine1>Central Park</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Sculpture Fountain Park</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.7824550624046</geo:lat><geo:long>-73.9655050504696</geo:long></geo:Point></item><item><category>Misc</category><title>Statue of Liberty</title><imageURL>images/location_flower.png</imageURL><shortDescription>The Statue of Liberty Enlightening the World was a gift of friendship from the people of France to the people of the United States.</shortDescription><longDescription>Located on a 12 acre island, the Statue of Liberty Enlightening the World was a gift of friendship from the people of France to the people of the United States and is a universal symbol of freedom and democracy. The Statue of Liberty was dedicated on October 28, 1886, designated as a National Monument in 1924 and restored for her centennial on July 4, 1986.</longDescription><addressLine1>Liberty Island</addressLine1><addressLine2>New York City, NY</addressLine2><date></date><keywords>Monument Attraction</keywords><recurrence>Daily</recurrence><geo:Point><geo:lat>40.6892366406123</geo:lat><geo:long>-74.044543999545</geo:long></geo:Point></item></channel></rss>''
')
SET IDENTITY_INSERT [dbo].[Concierge] OFF

-- Add 3 rows to [dbo].[Destinations]
SET IDENTITY_INSERT [dbo].[Destinations] ON
INSERT INTO [dbo].[Destinations] ([DestinationID], [DestinationName]) VALUES (1, 'Las Vegas')
INSERT INTO [dbo].[Destinations] ([DestinationID], [DestinationName]) VALUES (3, 'Seattle')
INSERT INTO [dbo].[Destinations] ([DestinationID], [DestinationName]) VALUES (4, 'New York')
SET IDENTITY_INSERT [dbo].[Destinations] OFF

-- Add 3 rows to [dbo].[DestinationToConcierge]
INSERT INTO [dbo].[DestinationToConcierge] ([DestinationID], [ConciergeID]) VALUES (1, 1)
INSERT INTO [dbo].[DestinationToConcierge] ([DestinationID], [ConciergeID]) VALUES (3, 2)
INSERT INTO [dbo].[DestinationToConcierge] ([DestinationID], [ConciergeID]) VALUES (4, 3)

-- Add constraints to [dbo].[DestinationToConcierge]
ALTER TABLE [dbo].[DestinationToConcierge] ADD CONSTRAINT [FK_LocationToConcierge_Concierge] FOREIGN KEY ([ConciergeID]) REFERENCES [dbo].[Concierge] ([ConciergeID])
ALTER TABLE [dbo].[DestinationToConcierge] ADD CONSTRAINT [FK_LocationToConcierge_Locations] FOREIGN KEY ([DestinationID]) REFERENCES [dbo].[Destinations] ([DestinationID])

-- Add constraint FK_UserToCollection_Destinations to [dbo].[UserToCollection]
ALTER TABLE [dbo].[UserToCollection] WITH NOCHECK ADD CONSTRAINT [FK_UserToCollection_Destinations] FOREIGN KEY ([DestinationID]) REFERENCES [dbo].[Destinations] ([DestinationID])

COMMIT TRANSACTION
GO

-- Reseed identity on [dbo].[Destinations]
DBCC CHECKIDENT('[dbo].[Destinations]', RESEED, 5)
GO

-- Reseed identity on [dbo].[Concierge]
DBCC CHECKIDENT('[dbo].[Concierge]', RESEED, 3)
GO
