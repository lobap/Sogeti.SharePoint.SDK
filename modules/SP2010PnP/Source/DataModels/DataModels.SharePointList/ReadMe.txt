***********************************************************************************
Pre-requisite:

1. You are a member of the SharePoint Farm Administrators group

2. You are a member of the Windows Administrators group

3. You have a local SharePoint site (such as http://<Hostname>:80/). Settings.xml has all parameters that RIInstall.bat will use for deployment ex:WebApp Port,site Name.

4. The "SharePoint 2010 Administration" service is running (Start->Administrative Tools->Services->SharePoint 2010 Administration).

5. The "SharePoint 2010 Timer" service is running (Start->Administrative Tools->Services->SharePoint 2010 Timer).

6. The "Microsoft SharePoint Foundation Sandboxed Code Service" service is running (SharePoint Central Administration->(Under System Settings) Manage services on server).

***********************************************************************************
Install the DataModels.SharePointList RI using RIInstall.bat:

1. Right click on Setup\RIInstall.bat, select "Run as administrator".

NOTE:This RI will not work in SharePoint 2010 Foundation Version.

*********************************************************************************** 
How to Verify:

 1. Browse to new site collection (http://<Hostname>:80/sites/sharepointlist)
 2. Browse to lists, verify all lists has sample data
 3. verify all pages are showing
 4. Try to add/find new parts/machines...

*********************************************************************************** 
Manual Steps Automated by RIInstall.bat:

1.	Make sure http://<<HostName>>:80 is working 
2.	Restore small backup into new site collection under port 80 named "SharepointList" i.e. http://<<HostName>>:80/sites/SharepointList
    a.	stsadm.exe -o restore -url http://<<HostName>>:80/sites/SharepointList -filename <<droplocation>>\Installers\datamodel_spb\partssite.spb –overwrite
    b.	Add current user as site collection administrator for http://<<HostName>>:80/sites/SharepointList using SharePoint Central Administration
3.	Open Visual Studio 2010 as Administrator.
4.	Open Source\DataModels\DataModels.SharePointList\DataModels.SharePointList.sln
5.	Update site Url property to "http://<<HostName>>:80/sites/SharepointList" for DataModels.SharePointList.PartsMgmnt Project.
6.	Update site Url Property to "http://<<HostName>>:80/sites/SharepointList" for DataModels.SharePointList.Model Project.
7.	Update site Url Property to "http://<<HostName>>:80/sites/SharepointList" for DataModels.SharePointList.Sandbox
8.	Right click on Solution and select Build Solution.
9.	Once build succeeded, right click on DataModels.SharePointList.PartsMgmnt Project and select Deploy.
10.	Right click on DataModels.SharePointList.Model Project and select Deploy.
11.	After Deploy Success, browse to http://<<HostName>>:80/sites/SharepointList and verify the following features were activated:
	Patterns and Practices SharePoint Guidance V3 - Data Models - SharePoint List Data - Parts Management - Web Parts
	Patterns and Practices SharePoint Guidance V3 - Data Models - SharePoint List Data - Parts Management - Pages

*********************************************************************************** 
Manual Steps Automated by RIUninstall.bat:

 1. Delete Created Site Collection
 2. Remove Packages from Farm.