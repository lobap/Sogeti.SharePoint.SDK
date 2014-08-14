***********************************************************************************
Pre-requisite:

1. You are a member of the SharePoint Farm Administrators group

2. You are a member of the Windows Administrators group

3. You have a local SharePoint site (such as http://<Hostname>:80/). Settings.xml has all parameters that RIInstall.bat will use for deployment ex:WebApp Port,site Name.

4. The "SharePoint 2010 Administration" service is running (Start->Administrative Tools->Services->SharePoint 2010 Administration).

5. The "Microsoft SharePoint Foundation Sandboxed Code Service" service is running (SharePoint Central Administration->(Under System Settings) Manage services on server).

***********************************************************************************
Install the Sandbox RI using RIInstall.bat:

1. Right click on Setup\RIInstall.bat, select "Run as administrator".

***********************************************************************************  
How To Verify?
 1.Add few Projects to the Projects list under Manufacturing Site.
 2.Add few documents/files to the Estimates Library under Construction SubSite.
 3.Add few documents/files to the Estimates Library under Maintenance SubSite.
 4.After adding documents/files you could verify the Aggregate view diplaying ID, SOWStatus and EstimateValue on the HomePage.(i.e. http://<Hostname>/sites/Manufacturing/default.aspx.)

***********************************************************************************  
Manual Steps Automated by RIInstall.bat:
1.	Start (if not started) the Microsoft SharePoint Foundation Sandboxed Code Service in Central Administration 
    a.	To start the Microsoft SharePoint Foundation Sandboxed Code Service by using Central Administration
    b.	Verify that you have the following administrative credentials:
    c.	You must be a member of the Farm Administrators group.
    d.	On the Central Administration Web site, on the Quick Launch, click System Settings.
    e.	On the System Settings page, in the Servers section, click Manage services on server.
    f.	On the Services on Server page, in the Microsoft SharePoint Foundation Sandboxed Code Service row, in the Action column click Start to start the service.

2.	Make sure http://<<HostName>>:80 is working 
3.	Create new site collection under port 80 named "Manufacturing" (http://<<HostName>>:80/sites/Manufacturing)
4.	Create two subsites named "Maintenance" and "Construction" under the above site collection (i.e. (http://<<HostName>>:80/sites/Manufacturing))
6.	Run lib\GAC_ServiceLocation.bat with Administrative Privileges.
7.	Open Visual Studio 2010 as Administrator.
8.	Open Source\ExecutionModels\Sandboxed\ExecutionModels.Sandboxed.sln
9.	Update site URL property to "http://<<HostName>>:80/sites/Manufacturing" for the ExecutionModels.Sandboxed Project.
10.	Right click on Solution and select Build Solution.
11.	Once build Success, right click on ExecutionModels.Sandboxed project and select Deploy
12.	Once deploy Success, browse to "http://<<HostName>>:80/sites/Manufacturing". Go to Site actions > Site Settings > Under Site Collection Administration > Site collection features > Activate  “Patterns and Practices SharePoint Guidance V3 - Execution Models - Estimate Content Types”  Feature
13.	Add “P&P SPG V3 - Execution Models Sandbox Aggregate View” WebPart to default Home Page , this WebPart is available under the category “P&P SPG V3”.
14.	Browse to the construction subsite  (http://<<HostName>>:80/sites/Manufacturing/Construction) 
15.	Activate “(Patterns and Practices SharePoint Guidance V3 - Execution Models - Estimates Library” feature from Manage site features(site actions > site  settings > Manage site features)
16.	Repeat the above 2 steps for the Maintenance subsite (http://<<HostName>>:80/sites/Manufacturing/Maintenance). 


***********************************************************************************  
Manual Steps Automated by RIUninstall.bat:
 It will delete site collection http://<Hostname>/sites/Manufacturing

***********************************************************************************  
Known Issues: 

  1) webpart may show error if you are using SharePoint2010 beta 2 version.