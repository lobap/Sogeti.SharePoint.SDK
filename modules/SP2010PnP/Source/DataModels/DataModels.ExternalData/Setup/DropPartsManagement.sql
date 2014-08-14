
USE [master]
GO



/****** Object:  Database [PartsManagement]    Script Date: 04/12/2010 06:35:27 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'PartsManagement')
BEGIN

	ALTER DATABASE  [PartsManagement]
	SET OFFLINE WITH ROLLBACK IMMEDIATE
	ALTER DATABASE  [PartsManagement]
	SET ONLINE

	DROP DATABASE [PartsManagement]
END
