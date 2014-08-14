***********************************************************************************
Pre-requisite:

1. You are a member of the SharePoint Farm Administrators group

2. You are a member of the Windows Administrators group

3. You have a local SharePoint site (such as http://<Hostname>:80/). Settings.xml has all parameters that RIInstall.bat will use for deployment ex:WebApp Port,site Name.

4. The "SharePoint 2010 Administration" service is running (Start->Administrative Tools->Services->SharePoint 2010 Administration).

5. The "SharePoint 2010 Timer" service is running (Start->Administrative Tools->Services->SharePoint 2010 Timer).

6. SharePoint Designer 2010 is installed. Required if you wish to open and examine workflows.

***********************************************************************************
Install the DataModels.ListOf RI using RIInstall.bat:

1. Right click on Setup\RIInstall.bat, select "Run as administrator".

NOTE:This RI will not work in SharePoint 2010 Foundation Version.

*********************************************************************************** 
How to Verify:
 1. Browse to new site collection (http://<Hostname>:80/sites/ManufacturingLstOfWF)
 2. Browse to lists, verify all lists has sample data
 3. verify all pages are showing
 4. Add a document to site construction/maintenance, set type as estimate, status approved,select a project and save.
 5. Wait for the workflow to run.
 6. Verify items are created in project calender and project sites.

***********************************************************************************
Manual Steps Automated by RIInstall.bat:

 1. Install SandBox RI by following steps: Source\ExecutionModels\SandBoxed\Readme.txt
 2. Install WorkFlow RI by following steps: Source\ExecutionModels\WorkFlow\Readme.txt
 3. Open Source\DataModels\DataModels.ListOf\ListOf.sln
 4. Update site Url property to "http://<<HostName>>:80/sites/ManufacturingLstOfWF" for DataModels.ListOf project.
 5.	Right click on the Solution and select Build Solution.
 6.	Once build succeeded, right click the DataModels.ListOf project and select Deploy.
 7. Activate the required features
 8. Associate Workflow with estimate content type
 9. Install sample data into list

***********************************************************************************
Manual Steps Automated by RIUninstall.bat:

 1. Delete ManufacturingLstOfWF site collection
 2. Remove packages from farm.

