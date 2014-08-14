-- =============================================
-- Script Template
-- =============================================

USE [master]
GO
/****** Object:  Database [DataModelSampleData]    Script Date: 03/12/2010 07:56:16 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'DataModelSampleData')
BEGIN
CREATE DATABASE [DataModelSampleData] ON  PRIMARY 
( NAME = N'DataModelSampleData', FILENAME = N'C:\Program Files\Microsoft Office Servers\14.0\Data\MSSQL10.SHAREPOINT\MSSQL\DATA\DataModelSampleData.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 5024KB )
 LOG ON 
( NAME = N'DataModelSampleData_log', FILENAME = N'C:\Program Files\Microsoft Office Servers\14.0\Data\MSSQL10.SHAREPOINT\MSSQL\DATA\DataModelSampleData_log.ldf' , SIZE = 5024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END
GO
ALTER DATABASE [DataModelSampleData] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DataModelSampleData].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DataModelSampleData] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [DataModelSampleData] SET ANSI_NULLS OFF
GO
ALTER DATABASE [DataModelSampleData] SET ANSI_PADDING OFF
GO
ALTER DATABASE [DataModelSampleData] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [DataModelSampleData] SET ARITHABORT OFF
GO
ALTER DATABASE [DataModelSampleData] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [DataModelSampleData] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [DataModelSampleData] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [DataModelSampleData] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [DataModelSampleData] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [DataModelSampleData] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [DataModelSampleData] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [DataModelSampleData] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [DataModelSampleData] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [DataModelSampleData] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [DataModelSampleData] SET  DISABLE_BROKER
GO
ALTER DATABASE [DataModelSampleData] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [DataModelSampleData] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [DataModelSampleData] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [DataModelSampleData] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [DataModelSampleData] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [DataModelSampleData] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [DataModelSampleData] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [DataModelSampleData] SET  READ_WRITE
GO
ALTER DATABASE [DataModelSampleData] SET RECOVERY SIMPLE
GO
ALTER DATABASE [DataModelSampleData] SET  MULTI_USER
GO
ALTER DATABASE [DataModelSampleData] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [DataModelSampleData] SET DB_CHAINING OFF
GO
USE [DataModelSampleData]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 03/12/2010 07:56:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Supplier]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Supplier](
       [Name] [varchar](255) NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Supplier] ([Name]) VALUES (N'Coho Vineyard ')
INSERT [dbo].[Supplier] ([Name]) VALUES (N'Graphic Design Institute ')
INSERT [dbo].[Supplier] ([Name]) VALUES (N'Southridge Video ')
INSERT [dbo].[Supplier] ([Name]) VALUES (N'Wingtip Toys ')
INSERT [dbo].[Supplier] ([Name]) VALUES (N'The Phone Company ')
INSERT [dbo].[Supplier] ([Name]) VALUES (N'Adventure Works ')

/****** Object:  Table [dbo].[Parts]    Script Date: 03/12/2010 07:56:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Parts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Parts](
	[Name] [varchar](255) NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Parts] ([Name]) VALUES (N'Saw Blade - Band Saw')
INSERT [dbo].[Parts] ([Name]) VALUES (N'Gasket')
INSERT [dbo].[Parts] ([Name]) VALUES (N'Oil')
INSERT [dbo].[Parts] ([Name]) VALUES (N'Hydraulic Oil')
INSERT [dbo].[Parts] ([Name]) VALUES (N'Grease')
INSERT [dbo].[Parts] ([Name]) VALUES (N'Bearings')
INSERT [dbo].[Parts] ([Name]) VALUES (N'Lathe Bit')
/****** Object:  Table [dbo].[Manufacturer]    Script Date: 03/12/2010 07:56:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Manufacturer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Manufacturer](
	[Name] [varchar](255) NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Manufacturer] ([Name]) VALUES (N'A. Datum Corporation ')
INSERT [dbo].[Manufacturer] ([Name]) VALUES (N'Adventure Works ')
INSERT [dbo].[Manufacturer] ([Name]) VALUES (N'Alpine Ski House ')
INSERT [dbo].[Manufacturer] ([Name]) VALUES (N'Litware, Inc. ')
INSERT [dbo].[Manufacturer] ([Name]) VALUES (N'Fabrikam, Inc')
INSERT [dbo].[Manufacturer] ([Name]) VALUES (N'Trey Research ')
INSERT [dbo].[Manufacturer] ([Name]) VALUES (N'Wide World Importers ')
INSERT [dbo].[Manufacturer] ([Name]) VALUES (N'City Power & Light ')
/****** Object:  Table [dbo].[Machines]    Script Date: 03/12/2010 07:56:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Machines]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Machines](
	[Name] [varchar](255) NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Machines] ([Name]) VALUES (N'Band Saw')
INSERT [dbo].[Machines] ([Name]) VALUES (N'Lathe')
INSERT [dbo].[Machines] ([Name]) VALUES (N'10 ton Press')
INSERT [dbo].[Machines] ([Name]) VALUES (N'Arc Welder')
INSERT [dbo].[Machines] ([Name]) VALUES (N'Circular Saw')
/****** Object:  Table [dbo].[Departments]    Script Date: 03/12/2010 07:56:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Departments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Departments](
	[Name] [varchar](255) NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Departments] ([Name]) VALUES (N'Assembly')
INSERT [dbo].[Departments] ([Name]) VALUES (N'Welding')
INSERT [dbo].[Departments] ([Name]) VALUES (N'Shipping')
INSERT [dbo].[Departments] ([Name]) VALUES (N'Receiving')
INSERT [dbo].[Departments] ([Name]) VALUES (N'Inspection')
INSERT [dbo].[Departments] ([Name]) VALUES (N'Inventory Control')
INSERT [dbo].[Departments] ([Name]) VALUES (N'Fabrication')
INSERT [dbo].[Departments] ([Name]) VALUES (N'Paint')
