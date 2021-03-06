USE [master]
GO
/****** Object:  Database [VendorManagement]    Script Date: 03/04/2010 16:07:08 ******/
if exists (select * from dbo.sysobjects where id =
object_id(N'[VendorManagement]') and OBJECTPROPERTY(id, N'IsDatabase') = 1)
DROP DATABASE [VendorManagement]
GO
CREATE DATABASE [VendorManagement] 
GO
ALTER DATABASE [VendorManagement] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VendorManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VendorManagement] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [VendorManagement] SET ANSI_NULLS OFF
GO
ALTER DATABASE [VendorManagement] SET ANSI_PADDING OFF
GO
ALTER DATABASE [VendorManagement] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [VendorManagement] SET ARITHABORT OFF
GO
ALTER DATABASE [VendorManagement] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [VendorManagement] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [VendorManagement] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [VendorManagement] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [VendorManagement] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [VendorManagement] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [VendorManagement] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [VendorManagement] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [VendorManagement] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [VendorManagement] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [VendorManagement] SET  DISABLE_BROKER
GO
ALTER DATABASE [VendorManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [VendorManagement] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [VendorManagement] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [VendorManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [VendorManagement] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [VendorManagement] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [VendorManagement] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [VendorManagement] SET  READ_WRITE
GO
ALTER DATABASE [VendorManagement] SET RECOVERY SIMPLE
GO
ALTER DATABASE [VendorManagement] SET  MULTI_USER
GO
ALTER DATABASE [VendorManagement] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [VendorManagement] SET DB_CHAINING OFF
GO
USE [VendorManagement]
GO
declare @hostname varchar(50)
select @hostname= HOST_NAME()
declare @acctName varchar(50)
select @acctName= @hostName +'\Impersonationacct'
/****** Object:  User [spgv3-03\testuser]    Script Date: 03/04/2010 16:07:09 ******/
declare @sql varchar(1000)
set @sql='CREATE LOGIN ['+ @acctName+'] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]'

EXEC (@sql)
Set @sql ='CREATE USER ['+ @acctName+'] FOR LOGIN ['+@acctName+'] WITH DEFAULT_SCHEMA=[dbo]'
EXEC (@sql)
GO
/****** Object:  User [spgv3-03\Administrator]    Script Date: 03/04/2010 16:07:09 ******/
declare @hostname varchar(50)
select @hostname= HOST_NAME()
declare @acctName varchar(50)
select @acctName= @hostName +'\Administrator'
declare @sql varchar(1000)
Set @sql ='CREATE USER ['+ @acctName+'] FOR LOGIN ['+@acctName+'] WITH DEFAULT_SCHEMA=[dbo]'
EXEC (@sql)
GO
/****** Object:  Table [dbo].[Vendors]    Script Date: 03/04/2010 16:07:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Vendors](
       [Id] [int] IDENTITY(1,1) NOT NULL,
       [Name] [varchar](50) NULL,
       [Address] [varchar](50) NULL,
       [City] [varchar](50) NULL,
       [State] [varchar](50) NULL,
       [ZipCode] [varchar](50) NULL,
       [Country] [varchar](50) NULL,
       [Telephone] [varchar](50) NULL,
       [Industry] [varchar](50) NULL,
       [AccountsPayable] [bigint] NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
       [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Vendors] ON
INSERT [dbo].[Vendors] ([Id], [Name], [Address], [City], [State], [ZipCode], [Country], [Telephone], [Industry], [AccountsPayable]) VALUES (1, N'Contoso', N'Two Microsoft Way', N'Bellevue', N'WA', N'98009', N'United States', N'(555)555-0100', N'Hotels', 650000)
INSERT [dbo].[Vendors] ([Id], [Name], [Address], [City], [State], [ZipCode], [Country], [Telephone], [Industry], [AccountsPayable]) VALUES (2, N'A. Datum Corporation', N'Quintana Ave.', N'Capital Federal', N'Buenos Aires', N'1014', N'Argentina', N'(555)555-0101', N'Finance', 550000)
INSERT [dbo].[Vendors] ([Id], [Name], [Address], [City], [State], [ZipCode], [Country], [Telephone], [Industry], [AccountsPayable]) VALUES (3, N'Fabrikam, Inc.', N'One Microsoft Way', N'Redmond', N'WA', N'98052', N'USA', N'555-555-0150', N'Information Technology', NULL)
SET IDENTITY_INSERT [dbo].[Vendors] OFF
/****** Object:  Table [dbo].[TransactionTypes]    Script Date: 03/04/2010 16:07:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransactionTypes](
       [Id] [int] IDENTITY(1,1) NOT NULL,
       [TransactionType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ActivityTypes] PRIMARY KEY CLUSTERED 
(
       [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[TransactionTypes] ON
INSERT [dbo].[TransactionTypes] ([Id], [TransactionType]) VALUES (1, N'Quote')
INSERT [dbo].[TransactionTypes] ([Id], [TransactionType]) VALUES (2, N'Contract')
INSERT [dbo].[TransactionTypes] ([Id], [TransactionType]) VALUES (5, N'Estimate')
SET IDENTITY_INSERT [dbo].[TransactionTypes] OFF
/****** Object:  Table [dbo].[VendorTransactions]    Script Date: 03/04/2010 16:07:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VendorTransactions](
       [ID] [int] IDENTITY(1,1) NOT NULL,
       [VendorID] [int] NOT NULL,
       [TransactionTypeId] [int] NOT NULL,
       [Notes] [varchar](max) NULL,
       [TransactionDate] [datetime] NULL CONSTRAINT [DF_ClientActivity_ActivityDate]  DEFAULT (getdate()),
       [Amount] [money] NULL,
 CONSTRAINT [PK_ClientActivity] PRIMARY KEY CLUSTERED 
(
       [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_ActivityID] ON [dbo].[VendorTransactions] 
(
       [TransactionTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ClientID] ON [dbo].[VendorTransactions] 
(
       [VendorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[VendorTransactions] ON
INSERT [dbo].[VendorTransactions] ([ID], [VendorID], [TransactionTypeId], [Notes], [TransactionDate], [Amount]) VALUES (11, 1, 1, N'Vendor Quote', CAST(0x00009D2F00BD76FC AS DateTime), 1500.0000)
INSERT [dbo].[VendorTransactions] ([ID], [VendorID], [TransactionTypeId], [Notes], [TransactionDate], [Amount]) VALUES (12, 1, 2, N'Estimate', CAST(0x00009D2F00BD88F7 AS DateTime), 42233.0000)
INSERT [dbo].[VendorTransactions] ([ID], [VendorID], [TransactionTypeId], [Notes], [TransactionDate], [Amount]) VALUES (14, 1, 5, N'Contract', CAST(0x00009D2E00000000 AS DateTime), 3000.0000)
INSERT [dbo].[VendorTransactions] ([ID], [VendorID], [TransactionTypeId], [Notes], [TransactionDate], [Amount]) VALUES (15, 2, 1, N'Vendor Quote', CAST(0x00009D2F00BD76FC AS DateTime), 1500.0000)
INSERT [dbo].[VendorTransactions] ([ID], [VendorID], [TransactionTypeId], [Notes], [TransactionDate], [Amount]) VALUES (16, 2, 2, N'Estimate', CAST(0x00009D2F00BD88F7 AS DateTime), 123432.0000)
INSERT [dbo].[VendorTransactions] ([ID], [VendorID], [TransactionTypeId], [Notes], [TransactionDate], [Amount]) VALUES (17, 2, 5, N'Contract', CAST(0x00009D2E00000000 AS DateTime), 3000.0000)
INSERT [dbo].[VendorTransactions] ([ID], [VendorID], [TransactionTypeId], [Notes], [TransactionDate], [Amount]) VALUES (18, 1, 5, N'Contract #2', CAST(0x00009D2E00000000 AS DateTime), 3000.0000)
INSERT [dbo].[VendorTransactions] ([ID], [VendorID], [TransactionTypeId], [Notes], [TransactionDate], [Amount]) VALUES (19, 3, 2, N'Test', CAST(0x00009D2F00BE80CD AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[VendorTransactions] OFF
/****** Object:  View [dbo].[VendorTransactionView]    Script Date: 03/04/2010 16:07:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VendorTransactionView]
AS
SELECT     TOP (100) PERCENT dbo.VendorTransactions.ID, dbo.VendorTransactions.VendorID, dbo.VendorTransactions.TransactionTypeId, dbo.VendorTransactions.Notes, 
                      dbo.VendorTransactions.TransactionDate, dbo.VendorTransactions.Amount, dbo.Vendors.Name, dbo.TransactionTypes.TransactionType
FROM         dbo.TransactionTypes INNER JOIN
                      dbo.VendorTransactions ON dbo.TransactionTypes.Id = dbo.VendorTransactions.TransactionTypeId INNER JOIN
                      dbo.Vendors ON dbo.VendorTransactions.VendorID = dbo.Vendors.Id
ORDER BY dbo.VendorTransactions.TransactionDate, dbo.Vendors.Name, dbo.TransactionTypes.TransactionType
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[33] 4[28] 2[10] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TransactionTypes"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 84
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "VendorTransactions"
            Begin Extent = 
               Top = 6
               Left = 236
               Bottom = 114
               Right = 406
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Vendors"
            Begin Extent = 
               Top = 6
               Left = 444
               Bottom = 114
               Right = 595
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2610
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VendorTransactionView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VendorTransactionView'
GO
/****** Object:  ForeignKey [FK_ClientActivity_ActivityTypes]    Script Date: 03/04/2010 16:07:15 ******/
ALTER TABLE [dbo].[VendorTransactions]  WITH CHECK ADD  CONSTRAINT [FK_ClientActivity_ActivityTypes] FOREIGN KEY([TransactionTypeId])
REFERENCES [dbo].[TransactionTypes] ([Id])
GO
ALTER TABLE [dbo].[VendorTransactions] CHECK CONSTRAINT [FK_ClientActivity_ActivityTypes]
GO
/****** Object:  ForeignKey [FK_ClientActivity_Clients]    Script Date: 03/04/2010 16:07:15 ******/
ALTER TABLE [dbo].[VendorTransactions]  WITH CHECK ADD  CONSTRAINT [FK_ClientActivity_Clients] FOREIGN KEY([VendorID])
REFERENCES [dbo].[Vendors] ([Id])
GO
ALTER TABLE [dbo].[VendorTransactions] CHECK CONSTRAINT [FK_ClientActivity_Clients]
GO

declare @hostname varchar(50)
select @hostname= HOST_NAME()
declare @accName varchar(50)
 select @accName=@hostname + '\sandboxsvcacct'
EXEC sp_addrolemember 'db_owner', @accName
GO
USE [VendorManagement]
GO
declare @hostname varchar(50)
select @hostname= HOST_NAME()
declare @accName varchar(50)
 select @accName=@hostname + '\impersonationacct'
EXEC sp_addrolemember 'db_owner', @accName
GO


