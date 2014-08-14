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

5. The "SharePoint 2010 Timer" service is running (Start->Administrative Tools->Services->SharePoint 2010 Timer).

6. Microsoft Silverlight 4 Tools for Visual Studio 2010 from http://go.microsoft.com/fwlink/?LinkID=177428. 
 
7. ADO.NET Data Services Update for .NET Framework 3.5 SP1 for Windows 7 and Windows Server 2008 R2 (KB982307) from http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=3e102d74-37bf-4c1e-9da6-5175644fe22d

NOTE: If you get any JavaScript error for AJAX CSOM/AJAX REST while testing Client RI then add http://ajax.microsoft.com into trust sites list and try again.
***********************************************************************************

Install the Client RI using RIInstall.bat:

1. Right click on Setup\RIInstall.bat, select "Run as administrator".

2. Create windows user group "ContosoUsers", and add the current logged in user to this group. Log-out from windows and log-in in order for this to take effect.

3. Configure IIS Manager for Contoso site
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
How to Verify:

 1.	Log out of windows and log in again so that the updated membership of ContosoUsers group takes affect. Otherwise, IIS does not know the logged-in user is a member of ContosoUsers. This step is only required one time after you create ContosoUsers and add the current logged in user to it. However, if you directly add an ALLOW rule for logged-in user in IIS Authorization Rule, you don’t need to re-login.
 2. Please make sure that you open IE 32 bit to test. It's a known issue that Silverlight does not work on 64bit IE. To start a 32bit IE: Start-> type "Internet" in the search box. Then select "Internet Explorer"
 3. Type in the address bar: http://<hostname>/sites/sharepointlist/client
 4. Verify that you can see Client Navigation links in the left column.
 5. Verify all pages/links under Client Navigation
 
***********************************************************************************
Manual Steps Automated by RIInstall.bat:

1.	Make sure http://<<HostName>>:80 is working 
2.	Install Proxy RI by following steps: Source\ExecutionModels\Proxy\Readme.txt
3.	Install SharePoint List RI by following steps: Source\DataModels\DataModels.SharePointList\Readme.txt
4.  Create Blank subsite "Client" under http://<<HostName>>:80/Sites/SharePointList
4.	Open Visual Studio 2010 as Administrator.
5.	Open Source\Client\Client.sln
6.	Update site Url property to "http://<<HostName>>:80/sites/SharePointList/Client" for Client.SharePoint project.
7.	Right click on the Solution and select Build Solution.
8.	Once build succeeded, right click the Client.SharePoint project and select Deploy.
9.	After Deploy Success, browse to "http://<<HostName>>:80/sites/SharePointList/Client" and verify the following features were activated:
  a.	Patterns and Practices SharePoint Guidance V3 - Client -JavaScript Files
  b.	Patterns and Practices SharePoint Guidance V3 - Client -Silverlight Apps
  c.	Patterns and Practices SharePoint Guidance V3 - Client –Pages
  d.	Patterns and Practices SharePoint Guidance V3 - Client –Libraries

***********************************************************************************
Manual Steps Automated by RIUninstall.bat:

 1. Delete Created Web for Client
 2. Remove solution packages from Farm.

Known Issues :
1. User will continue to see SilverLight Install prompt if using IE 64 bit.