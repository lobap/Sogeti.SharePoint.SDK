***********************************************************************************
Warning: 

1)	Don’t register an account to the Managed Account if it is already registered. Otherwise you may risk destroying the SharePoint environment.
2)	Don’t delete a account from windows uses if it is registered as a Managed Account. You have to change the all the service accounts to different managed account first, then remove the account from the managed accounts (Central Administration->Security->Configure managed accounts->Remove), and finally you may delete the user from windows. Otherwise, you may destroying the SharePoint environment.
***********************************************************************************
Pre-requisite:

1. You are a member of the SharePoint Farm Administrators group

2. You are a member of the Windows Administrators group

3. You have a local SharePoint site (such as http://<Hostname>:80/). Settings.xml has all parameters that RIInstall.bat will use for deployment ex:WebApp Port,site Name.

4. The "SharePoint 2010 Administration" service is running (Start->Administrative Tools->Services->SharePoint 2010 Administration).

***********************************************************************************
Install the Sandboxed.Proxy RI using RIInstall.bat:

1. Right click on Setup\RIInstall.bat, select "Run as administrator".

Manual Steps to configure Contoso vender WCF service after running RIInstall.bat:

1.	If you are running Windows 7, you need to enable URL Authorization for IIS:
	o	Control Panel->Programs->Turn Windows features on or off
	o	Internet Information Services->World Wide Web Services->Security
	o	Check URL Authorization (this may require a re-boot)
2. Configure IIS Manager for Contoso site as the following
	-	For Contoso
		o	IIS Authentication
			*	Enable Anonymous Authentication (should already be set)
			*	Enable Windows Authentication
		o	IIS Authorization
			*	Allow All Users
		o	.NET Authorization
			*	Allow All Users
	-	For Contoso\Vendor
		o	IIS Authentication
			*	Enable Anonymous Authentication
			*	Enable Windows Authentication
		o	IIS Authorization
			*	Deny Anonymous Users
			*	Allow SandboxSvcAcct
			*	Allow ContosoUsers
		o	.NET Authorization
			*	Allow All Users

*********************************************************************************** 
How to Verify?
  1. Browse to http://<Hostname>/sites/SpringfieldProxy
  2. "Execution Models Proxy Aggregate View" should show some rows 
  3. Click on vendor name of any row
  4. Should get vendor details in popup

*********************************************************************************** 
Manual Steps Automated by RIInstall.bat:

Manual Steps to configure Contoso vendor WCF service
1.	If you are running Windows 7, you need to enable URL Authorization for IIS:
•	Control Panel->Programs->Turn Windows features on or off
•	Internet Information Services->World Wide Web Services->Security
•	Check URL Authorization (this may require a re-boot)

2.	Create windows user called "SandboxSvcAcct" and set password to P2ssw0rd$ (P-two-s-s-w-zero-r-d-Dollar). You are free to use a different password of your choice.
Add SandboxSvcAcct to all the following windows groups.
•	WSS_WPG (should already be a member)
•	WSS_ADMIN_WPG
•	WSS_RESTRICTED_WPG_V4
•	IIS_IUSRS
•	Performance Monitor Users

3.	Start SharePoint Central Admin to configure Microsoft SharePoint Foundation Sandboxed Code Service to run under SandboxSvcAcct
•	Start SP Central Admin
•	Under Security, click Configure service accounts
•	Click drop down under "Select an account for this component", the default is "Network Service" 
•	If SandboxSvcAcct is already in the dropdown list, select [MachineName/DomainName]\SandboxSvcAcct, and Click OK

4.	Only do the folllowing if the SandboxSvcAcct is not in the above dropdown list
************************************************************************************
WARNING: Don’t do the following to register SandboxSvcAcct to the Managed Account if it is already registered. Otherwise you may risk destroying the SharePoint environment.
************************************************************************************
•	Click Register new manage account
•	In the Central Administration->Register Managed Account page, type SandboxSvcAcct in the Username box, and P2ssw0rd$ in the Password box. Click OK
•	Click Credential Management drop down where is says Select one…
•	Select Windows Service – Microsoft SharePoint Foundation Sandboxed Code Service in the dropdown
•	Click drop down under Select an account for this component 
•	Select [MachineName/DomainName]\SandboxSvcAcct
•	Click OK

This guide assumes that you will be deploying RI to SpringfieldProxy Site Collection under the default Web Application, http://<<HostName>>:80.  You can change it to any other working Web Application by replacing port 80 in the steps below.
1.	Start (if not started) the Microsoft SharePoint Foundation Sandboxed Code Service in Central Administration 
    a.	To start the Microsoft SharePoint Foundation Sandboxed Code Service by using Central Administration
    b.	Verify that you have the following administrative credentials:
        i.	You must be a member of the Farm Administrators group.
        ii.	On the Central Administration Web site, on the Quick Launch, click System Settings.
        iii.	On the System Settings page, in the Servers section, click Manage services on server.
        iv.	On the Services on Server page, in the Microsoft SharePoint Foundation Sandboxed Code Service row, in the Action column click Start to start the service.
3.	Follow Sandboxed\Readme.txt with site name as SpringfieldProxy
4.	Make sure http://<<HostName>>:80/sites/SpringfieldProxy is working 
5.	GAC ServiceLocation Assembly by Running  lib\GAC_ServiceLocation.bat with Administrative Privileges
6.	Create new Site with port 81 in IIS, with name "Contoso"
7.	Add an application under Contoso named "Vendor" and map it to Source\ExecutionModels\Proxy\Vendor
8.	Open Visual Studio 2010 as Administrator
9.	Open Sources\ExecutionModels\Proxy\ExecutionModels.Sandboxed.Proxy.sln
10.	Update siteURL property to "http://<<HostName>>:80/sites/SpringfieldProxy" for the ExecutionModels.Sandboxed.Proxy project
11.	Right click on Solution and select Build Solution
NOTE: If step6 site is hosted in different server(not in localhost) then please update following source , Build 
    Project :VendorSystemProxy
	file : accountspayableproxyops.cs
	 Update "address" variable with proper service URL.

12.	Verify service by browsing to "http://<<HostName>>:81/Vendor/Service.svc"
NOTE: If service is not working then try to give the <machine name>\IIS_IUSRS group read & execute permissions on the Contoso app through IIS Manager.

13.	Once build Success, right click on ExecutionModels.Sandboxed.Proxy Project and select Deploy
14.	Configure IIS Manager for Contoso site
	Contoso:
	IIS Authentication
			*  Enable Anonymous Authentication (should already be set)
			*  Enable Windows Authentication
	IIS Authorization
			*  Allow All Users
	.NET Authorization
			*  Allow All Users

	Contoso\Vendor:
	IIS Authentication
			*  Enable Anonymous Authentication
			*  Enable Windows Authentication
	IIS Authorization
			*  Deny Anonymous Users
			*  Allow SandboxSvcAcct
			*  Allow ContosoUsers
	.NET Authorization
			*  Allow All Users

15.	Once deploy Success, browse http://<<HostName>>:80/sites/SpringfieldProxy 
16.	Add "Execution Models Proxy Aggregate View" WebPart to home page of site(http://<<HostName>>:80/sites/SpringfieldProxy)

*********************************************************
Manual Steps Automated by RIUninstall.bat:
 1. It will delete site collection http://<Hostname>/sites/SpringfieldProxy
 2. It will delete Contoso WebSite from IIS.

	
Note:Before run Unit tests, add current windows user into contosousers group,logoff and login.