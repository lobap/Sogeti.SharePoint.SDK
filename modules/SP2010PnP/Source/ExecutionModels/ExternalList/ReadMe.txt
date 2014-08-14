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

6. SharePoint Designer 2010 is installed.

***********************************************************************************
Install the ExternalList RI using RIInstall.bat:

1. Right click on Setup\RIInstall.bat, select "Run as administrator". 

2. Goto Central Admin ->Application Management ->Manage service applications ->Select the above Created Secure Store Application(SPGAPP) 
	
	a) Double click on SPGAPP->Select Target Application ->Click Set Credential on ribbon.
	d) Enter User name "<machine name>\Impersonationacct" and Password "P2ssw0rd$" and click OK.
	c) Select Target Application --> Edit from Ribbon
	d) click Next with default options, remove existing Members and add current user,<machine name>\sandboxsvcacct as members.
	e) Remove and add local Administrator  into Administrators group then click ok
	
3. Application Management ->Manage service applications-->Business Data Connectivity Service
	a) Select "Vendors" then click on Set Object Permissions from Ribbon
	b) Add following users and give permissions
		-- <machine name>\SandboxSvcAcct :Execute, Selectable In Clients
		-- <machine name>\Administrator :Edit, Execute Selectable In Clients, Set Permissions
		-- <machine name>\ImpersonationAcct : Execute
		-- Current Windows Login : Execute, Selectable In Clients
	 c) Repeate step 'b' for other two Objects('Vendor Transaction Types','Vendor Transactions')

4. Add <Drop Location>\Source\ExecutionModels\ExternalList\ExecutionModels.Sandboxed.ExternalList\bin\Debug\ExecutionModels.Sandbox.ExternalList.wsp to http://<<servername>>/sites/ECT solution galary &  Activate 
  a) Actiave site feature "Patterns and Practices SharePoint Guidance V3 - Execution Models - Vendor Transactions List Instances"
  
5. Reset IIS.
6. Add Vendors List Webpart to home Page


***********************************************************************************
How to Verify(http://<<servername>>/sites/ECT):
  1)Login with a site user configured above and validate that you can view the External Lists from the site (View all Site Content)

  2)Verify that the Client List Web Part Displays Properly (sandboxed).

NOTE : If you see any errors while accessing data please remove permissions on target application and re-add them.  
if still get errors then please delete Secure Store Service Application (neme:SPGSSA) from Central Administration, and follow the steps in "Configure Secure Store Service" below.
  
***********************************************************************************
Manual Steps Automated by RIInstall.bat:

Service Accounts:
1.	Create three (3) new local (for Single Server) or domain (for Server Farm) user accounts. These accounts will be required to complete the configuration that follows.  No special Network permissions are required. Each user should configured with “User cannot change password” checked and “Password never expires” checked.  Create Accounts with the following usernames
	SandboxSvcAcct 
	ImpersonationAcct
	SecureSvcAppPool
 
2.	From Central Administration > Security > Configure Managed Accounts
3.	Select “Register Managed Account”. 
***********************************************************************************
Warning: Don’t register an account to the Managed Account if it is already registered. Otherwise you may risk destroying the SharePoint environment.
4.	Enter the Account Information for the account to run the Sandbox Service (ex: “[YOUR_DOMAIN]\ SandboxSvcAcct”) and Select OK.
***********************************************************************************
5.	From Central Administraton > Security > Configure Service Accounts
6.	Select “Windows Service – Microsoft SharePoint Foundation Sandboxed Code Service” from the First Drop Down
7.	Select the newly created Managed Account in the second Drop Down and hit OK.
 
Configure Secure Store Service 
1.	Navigate to Central Administration > Application Management > Manage Service Applications
2.	Select New > Secure Store Service
3.	From the “Create New Secure Store Service Application” Screen, Enter:
    a.	Service Application Name – [ex:SPGSSSA]
    b.	Database Server & Database name [Should be defaulted]
    c.	Windows Authentication
    d.	Failover Server [blank in most cases]
    e.	Select “Create a New Application Pool”
        i.	Enter a new Name(ex: SPGSSSA)
        ii.	Select “Configurable”
***********************************************************************************
Warning: Don’t register an account to the Managed Account if it is already registered. Otherwise you may risk destroying the SharePoint environment.
        iii.	Select “Register New Managed Account” and enter the username/password of the account to run the application pool of the web service (ex:”[YOUR_DOMAIN]\SecureSvcAppPool“)
***********************************************************************************
        iv.	Hit OK
4.	Select The Newly Created Service from the list.
5.	Select “Generate New Key” from the Key management section of the ribbon  for the Secure Store Service.
    a.	Enter and confirm a valid passphrase
6.	With the Service Configuration Complete, we must now create a new Secure Store Target Application.
    a.	Select “New” from the Manage Target Applications section of the ribbon
    b.	Enter the required information for the Target Application
        i.	Target Application ID [prefer no spaces, ex:SPGSSSTA]
        ii.	Display name [friendly display name]
        iii.	Contact Email
        iv.	Application Type [Group or Individual, prefer Group]
        v.	Default page [or none if Group was selected]
    c.	Enter the required Credential Fields to gather [defaults OK in most circumstances]
    d.	Add the Administrators and Members (Users or Groups) to map the credentials for. **NOTE – THE MANAGED ACCOUNT CONFIGURED TO RUN THE SANDBOX SERVICE in step #1 above (ex:[YOUR_DOMAIN]\SandboxSvcAcct) or an Active Directory Group Containing the Account MUST BE INCLUDED AS A MEMBER HERE. 
    e.	Add current windows user into members list.
    f.	Add the user [YOUR_DOMAIN]\Administrator  to the Administrators Group
    g.	Click on OK
    h.	Now set the credential mapping  for the Secure Store Target Application By Selecting “Set Credentials” 
    i.	Here you will enter the credentials that will be used by the Target Application for impersonation (ex: [YOUR_DOMAIN]\ImpersonationAcct). Any users or Groups configured as Members above, will have the identity of the account entered here.  
    j.	THIS ACCOUNT MUST HAVE READ/WRITE ACCESS INTO THE DATA SOURCE (i.e. SQL Server) CONTAINING THE EXTERNAL DATA. In our scenario, this user will need to be granted permission in the VendorManagement Database. For demo purposes, dbo access is OK.
    k.	Enter the valid local User or Domain account (ex: [YOUR_DOMAIN]\ImpersonationAcct).  and hit ‘OK’

Creat Site Collection:
Create a site Collection http://<<servername>>/sites/ECT.

Configure Data Source :

1.	Run Installdb.bat with Administrative Permissions.
    a.	It will do following things.
        i.	Create a New SQL Database called “VendorManagement”.
        ii.	Execute the sql  script “VendorManagement.sql” script against your new Sql Server database. 
        iii.	Add the local or domain user Account configured in the Secure Store Service for impersonation. ([YOUR_DOMAIN]\ImpersonationAcct) as dbo for the VendorManagement Database.

2.	Open your site in SharePoint Designer
3.	Select “External Content Types”
4.	Under “New” on the Ribbon, select “External Content Type”
5.	Select “New external content type” and edit the name and display name. Enter the name “Vendors” for this ECT.  
6.	Edit NameSpace and enter “ExecutionModels.ExternalList“
7.	Select “click here to discover external data sources and define operations”
8.	Hit “Add Connection”, then Select the “SQL Server” for the type and enter the correct SQL Connection information (select Connect with Impersonated Windows Identity”)
9.	Enter the Secure Store Application ID created above(ex: SPGSSSTA)
10.	Re-enter Credentials with access to the Data Source when prompted.( ex: [YOUR_DOMAIN]\ImpersonationAcct)
11.	With the Datasource open, find the Vendors table, right-click and select “Create All Operations”
    a.	Vendors - SQL Table – Enter ECT Name : “Vendors”
12.	Select the “ID” field and make sure “Map to Identifier is Checked”
13.	Select the “Name Field and select “Show in Picker”
14.	Hit “Next”, then “Finish” to Save the ECT.
15.	Click “Save”  button
16.	Click on “External Content Type”
17.	Create a new External Content Type (Step4) and  Create below Content Types :
    a.	Enter ECT Name: “Vendor Transaction Types ”, select SQL Table – TransactionTypes
Repeat Step 11 to step 15.
    b.	Enter ECT Name “Vendor Transactions”,select  SQL View – VendorTransactionView  
Repeat Step 11 to step 14.

BDC Permissions:
1.	From Central Administration > Application Management > Manage Service Applications, Select “Business Data Connectivity Service”
2.	For EACH of the content types (ECT’s )configured  above, mouse over and select “Set Permissions”
3.	Enter the Credentials of a valid site user (i.e. local account [for testing])
    a.	Select Execute, Selectable in Clients for this user
4.	Enter the Credentials of an Administrator
    a.	Select Execute, Edit for this user, Selectable in Clients and Set Permissions for this user
5.	*IMPORTANT – Enter the Credentials of the user configured as a Managed Account to run the Sandbox Service in Step #1. ([YOUR_DOMAIN]\SandboxSvcAcct)
    a.	Select Execute, Selectable in Clients for this user
6.	*IMPORTANT – Enter the Credentials of the user configured as the Impersonation Account in Step #1. ([YOUR_DOMAIN]\ImpersonationAcct)
    a.	Select Execute for this user
    b.	Hit OK.
7.	Go To Central Administration > Application Management > select Configure Service application associations > click  the default  link  under "Application Proxy group" column > click "set as default" link beside your service application (ex:.-SPGSSSA)

Build and Deploy External List:
1.	Open  Source\ExecutionModels\ExternalList\ExecutionModels.Sandbox.ExternalList.sln in Visual Studio with Administrative Privileges.
2.	Update siteUrl property with your site url for "ExecutionModels.Sandboxed.ExternalList" Project 
3.	Right click on project  "ExecutionModels.Sandbox.ExternalList" select Build 
4.	Once build success, right click on Project "ExecutionModels.Sandboxed.ExternalList" select Deploy
5.	Add the “Vendors List”  WebPart to a page and verify that the Client List Web Part Displays Properly (sandboxed).

NOTE : Make sure you use SandboxsvcAcct while opening the page. If you get any webpart error then close and open page again.

•	For more details click on the list item.


***********************************************************************************	
Manual Steps Automated by RIUninstall.bat:
  1. Deletes database vendormanagement
  2. Deletes Service Application and its related database.
  3. Deletes ECT site collection