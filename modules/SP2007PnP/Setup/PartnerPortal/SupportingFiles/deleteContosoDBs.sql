
USE [master]
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoSSPAppDB')
EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'ContosoSSPAppDB'
GO
USE [master]
GO
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoSSPAppDB')
ALTER DATABASE [ContosoSSPAppDB] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoSSPAppDB')
ALTER DATABASE [ContosoSSPAppDB] SET  SINGLE_USER 
GO
USE [master]
GO
/****** Object:  Database [ContosoFBAdb]    Script Date: 02/21/2009 02:49:14 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoSSPAppDB')
DROP DATABASE [ContosoSSPAppDB]
GO

USE [master]
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoWebAppDB')
EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'ContosoWEBAppDB'
GO
USE [master]
GO
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoWebAppDB')
ALTER DATABASE [ContosoWEBAppDB] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoWebAppDB')
ALTER DATABASE [ContosoWEBAppDB] SET  SINGLE_USER 
GO
USE [master]
GO
/****** Object:  Database [ContosoFBAdb]    Script Date: 02/21/2009 02:49:14 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoWebAppDB')
DROP DATABASE [ContosoWEBAppDB]
GO
USE [master]
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoSSPdb')
EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'ContosoSSPdb'
GO
USE [master]
GO
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoSSPdb')
ALTER DATABASE [ContosoSSPdb] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoSSPdb')
ALTER DATABASE [ContosoSSPdb] SET  SINGLE_USER 
GO
USE [master]
GO
/****** Object:  Database [ContosoFBAdb]    Script Date: 02/21/2009 02:49:14 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ContosoSSPdb')
DROP DATABASE [ContosoSSPdb]
GO