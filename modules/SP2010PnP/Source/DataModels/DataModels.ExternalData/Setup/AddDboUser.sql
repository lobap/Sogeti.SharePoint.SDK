declare @hostname varchar(50)
select @hostname= HOST_NAME()
declare @acctName varchar(50)
select @acctName= SUSER_NAME()

declare @sql varchar(1000)
USE [master]

Set @sql ='CREATE LOGIN ['+ @acctName+'] FROM WINDOWS WITH DEFAULT_DATABASE=[master]'
print @sql
EXEC (@sql)

USE [PartsManagement]

Set @sql ='CREATE USER ['+ @acctName+'] FOR LOGIN ['+ @acctName+']'
print @sql
EXEC (@sql)


USE [PartsManagement]

Set @sql ='EXEC sp_addrolemember N''db_owner'', N'''+ @acctName+''''

print @sql
EXEC (@sql)
GO
