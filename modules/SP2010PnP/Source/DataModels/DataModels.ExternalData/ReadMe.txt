***********************************************************************************
Pre-requisite:

1. You are a member of the SharePoint Farm Administrators group

2. You are a member of the Windows Administrators group

3. You have a local SharePoint site (such as http://<Hostname>:80/). Settings.xml has all parameters that RIInstall.bat will use for deployment ex:WebApp Port,site Name.

4. The "SharePoint 2010 Administration" service is running (Start->Administrative Tools->Services->SharePoint 2010 Administration).

5. The "SharePoint 2010 Timer" service is running (Start->Administrative Tools->Services->SharePoint 2010 Timer).

***********************************************************************************
Install the DataModels.ExternalData RI using RIInstall.bat:

1. Right click on Setup\RIInstall.bat, select "Run as administrator".

NOTE:This RI will not work in SharePoint 2010 Foundation Version.

***********************************************************************************
How to Verify:
 1. Browse to new site collection (http://<Hostname>:80/sites/PartsManagement)
 2. Check the database PartsManagement is created in SQL Server 
 3. Browse to lists, verify all lists have sample data similar to PartsManagement database tables

 Sample Data for Suppliers:
             "A. Datum Corporation", 
            "City Power & Light ",
            "Coho Vineyard",
            "Fabrikam, Inc.", 
            "Graphic Design Institute ",
            "Litware, Inc. ",
            "Northwind Traders ",
            "Trey Research"

***********************************************************************************
Manual Steps Automated by RIInstall.bat:

1.	Make sure http://<<HostName>>:80 is working 
2.	Create new site collection under port 80 named “PartsManagement” i.e. http://<<HostName>>:80/sites/PartsManagement
3.	Open Source\DataModels\DataModels.ExternalData\Setup and run PartsManagement_SqlInstall.bat file with Administrative Permissions to create Partsmanagement Database in SQL Server.
4.	Open Visual Studio 2010 as Administrator.
5.	Open Source\DataModels\DDataModels.ExternalData\DataModels.ExternalData.sln
6.	Update site Url property to "http://<<HostName>>:80/sites/PartsManagement" for DataModels.ExternalData.PartsManagement Project.
7.	Right click on Solution and select Build solution.
8.	Once build succeeded, right click DataModels.ExternalData.PartsManagement project and select Deploy.
9.	After Deploy Success, browse to "http://<<HostName>>:80/sites/PartsManagement" and verify the following features were activated:
	Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management - BDC Model 
	Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management - External List Instances
	Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management -  Pages
	Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management – WebParts
	Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management - Connector
11.	Browse to Central Administration ->Application Management->Manage service applications->Business Data Connectivity Service
12.	Give following  set of object  permissions to all the service applications with namespace “DataModels.ExternalData.PartsManagement” for the current user.
	Edit
	Execute
	Selectable in Clients
	Set Permissions

13.	After deployment the following pages are created and links to them displayed in quick launch area.
	Manage Machines
	Manage Suppliers
	Machines By Part
	Parts by Machine


***********************************************************************************
Manual Steps Automated by RIUninstall.bat:

 1. Delete PartsManagement site collection
 2. Remove packages from Farm.
 3. Uninstalls all the activated features activated by the package
 4. Drop PartsManagement database from .\Sharepoint instance

KnownIssue:
 -if we add sample filter column twice then it will give no results.