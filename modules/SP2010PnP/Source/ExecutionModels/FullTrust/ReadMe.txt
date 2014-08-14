***********************************************************************************
Pre-requisite:

1. You are a member of the SharePoint Farm Administrators group

2. You are a member of the Windows Administrators group

3. You have a local SharePoint site (such as http://<Hostname>:80/). Settings.xml has all parameters that RIInstall.bat will use for deployment ex:WebApp Port,site Name.

4. The "SharePoint 2010 Administration" service is running (Start->Administrative Tools->Services->SharePoint 2010 Administration).

5. The "SharePoint 2010 Timer" service is running (Start->Administrative Tools->Services->SharePoint 2010 Timer).

***********************************************************************************
Install the Farm Solution RI using RIInstall.bat:

1. Right click on Setup\RIInstall.bat, select "Run as administrator".

NOTE:This RI will not work in SharePoint 2010 Foundation Version.

***********************************************************************************
How to Verify:
  1. Browse to Central Administration Site
  2. Click on "Monitoring"
  3. Click on "Configure Approved Estimates Aggregation Timer Job" Link Under "Timer Jobs"--> click on "Apply Changes"
  4. Click on "Run Now" button.
  5. Verify Job Status from Jobs Status screen(Monitoring-->Check job status). Once Job is success then we can see Approved Estimates from all site collections in Approved Estimate List under http://<<Hostname>>:80/sites/HeadQuarters
  
  6. You may also change the status of an estimate to "Approved", Wait for the next time job to finish or run "Run Now" again, and refresh the "Approved Estimates" page. YOu shoud see a new item added to the list.

*********************************************************************************** 
Manual Steps Automated by RIInstall.bat:
1.	Run lib\GAC_ServiceLocation.bat with Administrative Privileges.
2.	Follow Sandbox RI Readme and Get  ExecutionModels.Sandboxed.wsp
3.	Create a new site collection named “HeadQuarters” (i.e : http://<<HostName>>:80/sites/HeadQuarters  Title: DivisionalHeadQuarters)
    a.	Upload “ExecutionModels.Sandboxed.wsp” to above site solution gallery and activate.
4.	Create a new site collection named “MaintenanceBB” (i.e : http://<<HostName>>:80/sites/MaintenanceBB  Title: Facilities Maintenance-Blue Bell)
    a.	Upload “ExecutionModels.Sandboxed.wsp” to above site solution gallery and activate
    b.	Create  “Maintenance” subsite under above site and  activate “(Patterns and Practices SharePoint Guidance V3 - Execution Models - Estimates Library” feature from Manage site features(site actions > site  settings > Manage site features)
    c.	Add sample data to Projects List for above site collection.
    d.	Add few estimates to Estimates List in the Maintenance subsite
5.	Create a new site collection named “ConstructionBB” (i.e : http://<<HostName>>:80/sites/ConstructionBB  Title: Construction - Blue Bell )
    a.	Upload “ExecutionModels.Sandboxed.wsp” to above site solution gallery and activate
    b.	Create  “Construction” subsite under above site and activate “(Patterns and Practices SharePoint Guidance V3 - Execution Models - Estimates Library” feature from Manage site features(site actions > site  settings > Manage site features)
    c.	Add sample data to Projects List for above site collection.
    d.	Add few estimates to Estimates List in the Construction subsite
6.	Create a new site collection named “MaintenanceNB” (i.e: http://<<HostName>>:80/sites/MaintenanceNB  Title: Facilities Maintenance- New Brunswick )
    a.	Upload “ExecutionModels.Sandboxed.wsp” to above site solution gallery and activate
    b.	Create  “Maintenance” subsite under above site and activate “(Patterns and Practices SharePoint Guidance V3 - Execution Models - Estimates Library” feature from Manage site features(site actions > site  settings > Manage site features)
    c.	Add sample data to Projects List for above site collection.
    d.	Add few estimates to Estimates List in the Maintenance subsite
7.	Create a new site collection named “ConstructionNB” (i.e : http://<<HostName>>:80/sites/ConstructionNB  Title : Construction - New Brunswick)
    a.	Upload “ExecutionModels.Sandboxed.wsp” to above site solution gallery and activate
    b.	Create  “Construction” Subsite under above Site and  Activate “(Patterns and Practices SharePoint Guidance V3 - Execution Models - Estimates Library” feature from Manage site features(site actions > site  settings > Manage site features)
    c.	Add sample data to projects list for above site collection.
    d.	Add few estimates to Estimates List in the Construction subsite
8.	Open Visual Studio 2010 as Administrator
9.	Open Source\ExecutionModels\FullTrust\ExecutionModels.FarmSolution.sln 
10.	Update site URL property to "http://<<HostName>>:80/sites/HeadQuarters" for ExecutionModels.FarmSolution.Jobs Project.
11.	Update site URL property to "http://<<HostName>>:<<CentralAdminPort>>" for ExecutionModels.FarmSolution Project
12.	Right click on Solution and select Build Solution
13.	Once build Success, right click on ExecutionModels.FarmSolution.Jobs and select Deploy
14.	Right click on ExecutionModels.FarmSolution and select Deploy
15.	Once deploy Success, browse to Central Administration Site
    a.	Click on Monitoring
    b.	Click on  “Approved Estimates Aggregation Timer Job”
    c.	Click on “Apply Changes” button

***********************************************************************************
Manual Steps Automated by RIUninstall.bat:
 1. It will delete site collections created by RIInstall


 

