/****** Object:  Table [dbo].[aspnet_LiveIDAssociation]    Script Date: 05/01/2008 10:28:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_LiveIDAssociation]') AND type in (N'U'))
DROP TABLE [dbo].[aspnet_LiveIDAssociation]

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_AddLiveIDAssociation]    Script Date: 05/01/2008 10:31:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_AddLiveIDAssociation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_AddLiveIDAssociation]

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetLiveIDAssociation]    Script Date: 05/01/2008 10:31:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetLiveIDAssociation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetLiveIDAssociation]

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_RemoveLiveIDAssociation]    Script Date: 05/01/2008 10:32:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_RemoveLiveIDAssociation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_RemoveLiveIDAssociation]