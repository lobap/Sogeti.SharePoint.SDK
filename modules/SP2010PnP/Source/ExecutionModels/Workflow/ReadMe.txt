
***********************************************************************************
Pre-requisite:

1. You are a member of the SharePoint Farm Administrators group

2. You are a member of the Windows Administrators group

3. You have a local SharePoint site (such as http://<Hostname>:80/). Settings.xml has all parameters that RIInstall.bat will use for deployment ex:WebApp Port,site Name.

4. The "SharePoint 2010 Administration" service is running (Start->Administrative Tools->Services->SharePoint 2010 Administration).

5. SharePoint Designer 2010 is installed. Required if you wish to open and examine workflows.

***********************************************************************************
Install the Work Flow RI using RIInstall.bat:

1. Right click on RIInstall.bat, select "Run as administrator".

NOTE:This RI will not work in SharePoint 2010 Foundation Version.

*********************************************************************************** 
What does RIInstall.bat do?
  1. Compile Sandboxed.Proxy solution
  2. Package Sandboxed.Proxy solution
  3. Compile Workflow RI solution
  4. Package Workflow RI solution
  5. Install Sandboxed RI for given sitename in settings.xml
  6. Add ExecutionModels.Workflow.FullTrust.Activities Package to farm solutions
  7. Install ExecutionModels.Workflow.FullTrust.Activities Package.
  8. Add ExecutionModels.Workflow.Sandboxed.Activities Package to solution gallery of http://<Hostname>/sites/ManufacturingWF
  9. Install ExecutionModels.Workflow.Sandboxed.Activities Package.
  8. Add CreateProjectSite  package to solution gallery of http://<Hostname>/sites/ManufacturingWF
  9. Activate CreateProjectSite package
  10. Work flow Association with Estimate Content Type
  11. Populate sample data from settings.xml (Projects and estimates)

*********************************************************************************** 
How to Verify?
  1. Browse to http://<Hostname>/sites/ManufacturingWF
  2. Go to the construction or maintenance web sites.
  3. Open the estimates list.
  4. Add an excel spreadsheet.
  5. For the properties, set the type to Estimate, set the status to approved, and select a project.
  6. The workflow will run.  When it completes, look at the workflow status, and you will see the url.  The url will be the site name + the project name for the project. For example,http://<Hostname>/sites/ManufacturingWF/construction/<projectname>
  7. Navigate to the new site.  
  8. Open up all content
  9. You should see the templates library, and it should contain two docs.

*********************************************************************************** 
What does RIUninstall.bat do?
 1. It will delete site collection http://<Hostname>/sites/ManufacturingWF


Known Issues: 
 1.If Project name contains any special Characters like # then workflow will show error.