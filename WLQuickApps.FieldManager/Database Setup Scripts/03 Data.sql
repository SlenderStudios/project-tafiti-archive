delete from LeagueFields
go
Delete from Leagues
go
delete from fields
go

SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO

DECLARE @pv binary(16)

-- Drop constraints from [dbo].[LeagueFields]
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LeagueFields]') AND name = N'FK_LeagueFields_Fields')
ALTER TABLE [dbo].[LeagueFields] DROP CONSTRAINT [FK_LeagueFields_Fields]

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LeagueFields]') AND name = N'FK_LeagueFields_League')
ALTER TABLE [dbo].[LeagueFields] DROP CONSTRAINT [FK_LeagueFields_League]

go

-- Add 30 rows to [dbo].[Leagues]
SET IDENTITY_INSERT [dbo].[Leagues] ON
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (6, N'League Type', N'League Title', N'League Description')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (7, N'', N'New League', N'New League')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (8, N'', N'Hawaii League', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (9, N'', N'Alaska League', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (10, N'', N'Grapefruit league', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (11, N'', N'Wide League', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (12, N'', N'Empty League', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (13, N'', N'Mayflower', N'liga')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (14, N'Um?', N'And Another League League', N'dflksdjf;askdhflsdk')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (15, N'', N'', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (16, N'', N'55', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (17, N'', N'56', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (18, N'', N'57', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (19, N'', N'58', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (20, N'', N'59', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (21, N'', N'60', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (22, N'', N'61', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (23, N'', N'99', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (24, N'', N'100', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (25, N'', N'101', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (26, N'', N'102', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (27, N'', N'103', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (28, N'', N'104', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (29, N'', N'105', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (30, N'', N'106', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (31, N'', N'107', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (32, N'', N'109', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (33, N'U-13A', N'Buffalo Leaguer', N'Test leaguer')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (35, N'', N'My league 1', N'')
INSERT INTO [dbo].[Leagues] ([LeagueID], [Type], [Title], [Description]) VALUES (36, N'', N'My league 2', N'')
go
SET IDENTITY_INSERT [dbo].[Leagues] OFF
go

-- Add 40 rows to [dbo].[Fields]
SET IDENTITY_INSERT [dbo].[Fields] ON
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (29, N'2', N'', N'98033 (postal code), Washington, United States', 47.6, -122.2, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (30, N'Safeco Field', N'Where the Mariners play', N'Safeco Field (stadium), Seattle, Washington, United States', 47.590237796845116, -122.33206214837836, 1, 5, N'North', N'425-555-1234', N'Closed due to snow')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (31, N'4', N'', N'16625 Redmond Way, Redmond, WA 98052-4444', 47.7, -122.1, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (32, N'SharpLogic', N'', N'7901 168th Ave NE, Redmond, WA 98052-4468', 47.673862, -122.116263, 0, 1, N'', N'', N'Closed for repair')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (33, N'Redmond', N'', N'1 Microsoft Way, Redmond, WA 98052-8300', 47.64312, -122.130609, 1, 3, N'', N'', N'Open')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (34, N'Waikiki', N'Waikiki', N'Waikiki Beach (beach), Hawaii, United States', 21.27949875540433, -157.83016718957876, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (35, N'Honolulu', N'', N'University of Hawaii-Kapiolani Cmty (college), Honolulu, Hawaii, United States', 21.269704264134532, -157.80302401781711, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (36, N'Juneau', N'', N'Juneau, Alaska, United States', 58.3032802269834, -134.4104593936022, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (37, N'Dole Plantation', N'', N'Dole Plantation, Honolulu, Hawaii, United States', 21.525578518368842, -158.03843331775079, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (38, N'Morning Star School', N'', N'Morning Star School (school), Tampa, Florida, United States', 28.040117326078626, -82.457130905898779, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (42, N'Houston', N'', N'Houston, Texas, United States', 29.760486591201218, -95.36974696578045, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (43, N'Lake Carmel', N'', N'Lake Carmel, New York, United States', 41.462069130104616, -73.668234727223123, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (44, N'Farmingdale', N'', N'Farmingdale, New York, United States', 40.731860436127718, -73.445720824442446, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (45, N'LAX', N'', N'LAX [Los Angeles International Airport] (airport), Los Angeles, California, United States', 33.943994533306956, -118.40817311171939, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (46, N'Area 51', N'', N'Area 51 [Nevada Test Site], Nevada, United States', 36.933650295291578, -116.09741083247735, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (47, N'DC', N'', N'White House (building), Washington, D.C., District of Columbia, United States', 38.897575568480363, -77.036692780576658, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (48, N'Montana', N'', N'Helena, Montana, United States', 46.589765347599986, -112.02119491694877, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (49, N'New Orleans', N'', N'New Orleans, Louisiana, United States', 29.95371947301005, -90.07775054780042, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (50, N'Phoenix', N'', N'Phoenix, Arizona, United States', 33.447938384456435, -112.08253684588465, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (51, N'Detroit', N'', N'Detroit, Michigan, United States', 42.331688815230635, -83.047792004943133, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (52, N'Lincoln', N'', N'Lincoln, Nebraska, United States', 40.81364853634404, -96.7076990619159, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (53, N'Topeka', N'', N'Topeka, Kansas, United States', 39.05678044784419, -95.677494799213719, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (54, N'Vail', N'', N'S Frontage Rd W, Vail, CO 81657', 39.6170232994199, -106.43601945903511, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (55, N'Beaver Creek', N'', N'Beaver Creek, Eagle, Colorado, United States', 39.60493044949439, -106.51682059691636, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (56, N'Breckenridge', N'', N'Breckenridge, Summit, Colorado, United States', 39.482226101055112, -106.04721094053214, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (57, N'Keystone', N'Closed due to snow', N'Keystone, Summit, Colorado, United States', 39.608198695012241, -105.95520146222289, 0, 2, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (58, N'Heavenly', N'', N'Ski Run Blvd, South Lake Tahoe, CA 96150', 38.93378581636518, -119.94676884491314, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (59, N'Chicago', N'', N'Chicago, Illinois, United States', 41.884154330243895, -87.632454032476645, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (60, N'Pullman', N'sfasdfasdfasdfasdfasfd', N'Pullum Ave, Sargent, NE 68874', 41.640528312738709, -99.378009329256585, 1, 5, N'3', N'', N'Open')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (62, N'UW', N'', N'3800 Montlake Blvd NE, Seattle, WA 98195', 47.651074, -122.303896, 1, 1, N'8', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (63, N'AA', N'', N'7201 E Green Lake Dr N, Seattle, WA 98115-5301', 47.680264, -122.325831, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (64, N'DFA', N'', N'E Green Lake Dr N, Seattle, WA 98103', 47.682684407261235, -122.33040924259922, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (65, N'DAFSDFASDF', N'', N'7201 E Green Lake Dr N, Seattle, WA 98115-5301', 47.680264, -122.325831, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (66, N'NMMM', N'', N'E Green Lake Dr N, Seattle, WA 98103', 47.682684407261235, -122.33040924259922, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (67, N'QQQ', N'', N'48343 [Morris] (county), Texas, United States', 33.113545983070956, -94.732606429434156, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (68, N'QQQ', N'', N'48343 [Morris] (county), Texas, United States', 33.113545983070956, -94.732606429434156, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (69, N'yyy', N'', N'48313 [Madison] (county), Texas, United States', 30.965506130615303, -95.928367647428288, 1, 1, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (70, N'Craig''s Turf', N'', N'14528 73rd Ave NE, Kenmore, WA 98028', 47.73345320336194, -122.24482697229895, 1, 4, N'', N'', N'')
INSERT INTO [dbo].[Fields] ([FieldID], [Title], [Description], [Address], [Latitude], [Longitude], [IsOpen], [NumberOfFields], [ParkingLot], [PhoneNumber], [Status]) VALUES (71, N'Hello', N'', N'98052 (postal code), Washington, United States', 47.687062558519216, -122.11840017076202, 1, 1, N'', N'', N'')
SET IDENTITY_INSERT [dbo].[Fields] OFF
go
-- Add 56 rows to [dbo].[LeagueFields]
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (6, 29)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (6, 30)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (6, 31)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (6, 32)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (6, 33)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (8, 34)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (8, 35)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (8, 37)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (9, 36)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (10, 38)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (10, 42)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (10, 49)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (10, 69)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 33)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 36)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 37)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 38)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 42)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 43)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 44)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 45)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 46)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 47)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 48)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 49)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 50)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (11, 51)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 33)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 38)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 42)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 43)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 44)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 45)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 46)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 47)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 48)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 49)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 50)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 51)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 52)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 53)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 55)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 56)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 57)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 58)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 59)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 60)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (13, 62)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (14, 51)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (33, 29)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (33, 31)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (33, 33)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (33, 62)
INSERT INTO [dbo].[LeagueFields] ([LeagueID], [FieldID]) VALUES (33, 70)

-- Add constraints to [dbo].[LeagueFields]
ALTER TABLE [dbo].[LeagueFields] ADD CONSTRAINT [FK_LeagueFields_Fields] FOREIGN KEY ([FieldID]) REFERENCES [dbo].[Fields] ([FieldID]) ON DELETE CASCADE
ALTER TABLE [dbo].[LeagueFields] ADD CONSTRAINT [FK_LeagueFields_League] FOREIGN KEY ([LeagueID]) REFERENCES [dbo].[Leagues] ([LeagueID]) ON DELETE CASCADE

GO

-- Reseed identity on [dbo].[Fields]
DBCC CHECKIDENT('[dbo].[Fields]', RESEED, 71)
GO

-- Reseed identity on [dbo].[Leagues]
DBCC CHECKIDENT('[dbo].[Leagues]', RESEED, 37)
GO