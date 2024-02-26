-- This script:
------ Creates Database
------ Configures the DB
------ Creates the Roles
------ Creates the Schemas

USE [master]
GO
/****** Object:  Database [FieldManager]    Script Date: 04/11/2008 16:13:35 ******/
CREATE DATABASE [FieldManager] ON  PRIMARY 
( NAME = N'FieldManager', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\Data\FieldManager.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FieldManager_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\Data\FieldManager_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'FieldManager', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FieldManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FieldManager] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [FieldManager] SET ANSI_NULLS OFF
GO
ALTER DATABASE [FieldManager] SET ANSI_PADDING OFF
GO
ALTER DATABASE [FieldManager] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [FieldManager] SET ARITHABORT OFF
GO
ALTER DATABASE [FieldManager] SET AUTO_CLOSE ON
GO
ALTER DATABASE [FieldManager] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [FieldManager] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [FieldManager] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [FieldManager] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [FieldManager] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [FieldManager] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [FieldManager] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [FieldManager] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [FieldManager] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [FieldManager] SET  DISABLE_BROKER
GO
ALTER DATABASE [FieldManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [FieldManager] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [FieldManager] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [FieldManager] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [FieldManager] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [FieldManager] SET  READ_WRITE
GO
ALTER DATABASE [FieldManager] SET RECOVERY SIMPLE
GO
ALTER DATABASE [FieldManager] SET  MULTI_USER
GO
ALTER DATABASE [FieldManager] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [FieldManager] SET DB_CHAINING OFF
GO

USE [FieldManager]
GO
/****** Object:  Role [aspnet_Membership_BasicAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Membership_BasicAccess]
GO
/****** Object:  Role [aspnet_Membership_ReportingAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Membership_ReportingAccess]
GO
/****** Object:  Role [aspnet_Membership_FullAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Membership_FullAccess]
GO
/****** Object:  Role [aspnet_Personalization_BasicAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Personalization_BasicAccess]
GO
/****** Object:  Role [aspnet_Personalization_ReportingAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Personalization_ReportingAccess]
GO
/****** Object:  Role [aspnet_Personalization_FullAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Personalization_FullAccess]
GO
/****** Object:  Role [aspnet_Profile_BasicAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Profile_BasicAccess]
GO
/****** Object:  Role [aspnet_Profile_ReportingAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Profile_ReportingAccess]
GO
/****** Object:  Role [aspnet_Profile_FullAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Profile_FullAccess]
GO
/****** Object:  Role [aspnet_Roles_BasicAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Roles_BasicAccess]
GO
/****** Object:  Role [aspnet_Roles_ReportingAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Roles_ReportingAccess]
GO
/****** Object:  Role [aspnet_Roles_FullAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_Roles_FullAccess]
GO
/****** Object:  Role [aspnet_WebEvent_FullAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE ROLE [aspnet_WebEvent_FullAccess]
GO
/****** Object:  Schema [aspnet_WebEvent_FullAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE SCHEMA [aspnet_WebEvent_FullAccess] AUTHORIZATION [aspnet_WebEvent_FullAccess]
GO
/****** Object:  Schema [aspnet_Roles_ReportingAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE SCHEMA [aspnet_Roles_ReportingAccess] AUTHORIZATION [aspnet_Roles_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Roles_FullAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE SCHEMA [aspnet_Roles_FullAccess] AUTHORIZATION [aspnet_Roles_FullAccess]
GO
/****** Object:  Schema [aspnet_Roles_BasicAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE SCHEMA [aspnet_Roles_BasicAccess] AUTHORIZATION [aspnet_Roles_BasicAccess]
GO
/****** Object:  Schema [aspnet_Profile_ReportingAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE SCHEMA [aspnet_Profile_ReportingAccess] AUTHORIZATION [aspnet_Profile_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Profile_FullAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE SCHEMA [aspnet_Profile_FullAccess] AUTHORIZATION [aspnet_Profile_FullAccess]
GO
/****** Object:  Schema [aspnet_Profile_BasicAccess]    Script Date: 04/11/2008 16:13:35 ******/
CREATE SCHEMA [aspnet_Profile_BasicAccess] AUTHORIZATION [aspnet_Profile_BasicAccess]
GO
/****** Object:  Schema [aspnet_Personalization_ReportingAccess]    Script Date: 04/11/2008 16:13:36 ******/
CREATE SCHEMA [aspnet_Personalization_ReportingAccess] AUTHORIZATION [aspnet_Personalization_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Personalization_FullAccess]    Script Date: 04/11/2008 16:13:36 ******/
CREATE SCHEMA [aspnet_Personalization_FullAccess] AUTHORIZATION [aspnet_Personalization_FullAccess]
GO
/****** Object:  Schema [aspnet_Personalization_BasicAccess]    Script Date: 04/11/2008 16:13:36 ******/
CREATE SCHEMA [aspnet_Personalization_BasicAccess] AUTHORIZATION [aspnet_Personalization_BasicAccess]
GO
/****** Object:  Schema [aspnet_Membership_ReportingAccess]    Script Date: 04/11/2008 16:13:36 ******/
CREATE SCHEMA [aspnet_Membership_ReportingAccess] AUTHORIZATION [aspnet_Membership_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Membership_FullAccess]    Script Date: 04/11/2008 16:13:36 ******/
CREATE SCHEMA [aspnet_Membership_FullAccess] AUTHORIZATION [aspnet_Membership_FullAccess]
GO
/****** Object:  Schema [aspnet_Membership_BasicAccess]    Script Date: 04/11/2008 16:13:36 ******/
CREATE SCHEMA [aspnet_Membership_BasicAccess] AUTHORIZATION [aspnet_Membership_BasicAccess]
GO
