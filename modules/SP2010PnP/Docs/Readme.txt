Developing Applications for SharePoint 2010
Microsoft patterns and practices

==================== Updated on Mar 30, 2011 =========================================================
Documentation Fixes
-	Updated the topic "Using a Feature Receiver to Register a Type Mapping":
	Changed to register type mapping in FeatureInstalled event instead of FeatureActivated event
	since FeatureActivated does not run at a high enough permission level to update type mapping.
-	Updated the topic "How to: Import and Package a Declarative Workflow in Visual Studio":
	The overview of the topic has be modified by saying "The solution package (.wsp file) created by this How-to document 
	can be installed only on Microsoft SharePoint Server site collections, not SharePoint Foundation site collections"
-	Updated the topic "Reference Implementation: Workflow Activities":
	The following notes is removed since it is not true: 
	"The workflow contained in this project could also be implemented for SharePoint Foundation. 
	However workflows must be authored on the same server version to which they are deployed. 
	SharePoint Foundation 2010 workflows cannot be deployed to SharePoint Server 2010, and vice versa"

Migration of Microsoft.Practices.SharePoint (With Tests).sln to Moles version 0.94
-	Microsoft.Practices.SharePoint.Common.Tests.csproj was migrated to Moels version 0.94. 
-	The readme.txt (\SharePointGuidance2010\Source\SharePoint 2010\readme.txt) was updated 
	since steps for building Microsoft.Practices.SharePoint (With Tests).sln was simplified

==================== Updated on Aug 17, 2010 =========================================================
Documentation Fixes
-	Service location documentation incorrectly described the process for registering a singleton service.  
-	Documentation missing a note that category and area registeration for logging must be done from a high privileged context (can't be done from a content
	web).
-	Fixed documentation error on using a feature receiver to register logging areas and categories.

Bug Fixes
-	Solution did not build with the latest release of Moles (0.93).  Updated test project to use this release.
-	Exception thrown when removing the type mapping for a logger that had replaced the default implementation.  This was caused by 
	logging occurring low in the configuration stack.  Removed logging at this level to break the dependency between configuration and service location.


==================== Update on July 29, 2010  =========================================================
Documentation Fixes
-	Corrected step 6 of Enabling ECMA Client Object Model IntelliSense for a Visual Web Part
-	Fixed several repeated titles in the table of contents
-	Fixed an error in Using Feature Event Receivers to Configure Diagnostic Areas and Categories – the wrong method name was used for registering event 
    sources (EnsureAreasRegisteredAsEventSource)

Bug Fixes
-	ConfigurationManager - DeleteStore logic for WebAppSettingStore and FarmAppSettingStore was inverted, and therefore the store was not deleted.  The logic was fixed.
-	ConfigurationManager - A security exception was thrown when using HierarchicalConfig to check for a value that didn’t exist in any of the stores 
    and no values had been saved yet to either the farm or web application setting store.  In that case the logic attempted to create a setting store which threw a 
	security exception.  The Farm and Web Application objects cannot be written to from a content web.  The logic was changed to defer the creation of the setting 
	store to when a setting is written, in which case the code must be in a context where it can create the setting store as well.  
-	Logging - EnsureAreasRegisteredAsEventSource registers the event sources on a WFE that are configured.  However it did not register the default diagnostic area.  
    The logic was updated to always register the default area as well as an event source.
-	Workflow RI – Using a “#” in the project title caused a failure on site creation for a project.  The sample data was altered to remove the #.  
    For a production implementation, we would need to add more robust checking logic.
-	Workflow RI – If a project was not selected for the estimate, than the workflow was cancelled since the project name is used in site creation.  
    Several of the FieldLinks for the Content Type were made required in order to avoid this problem, including the project lookup field.


==================== Original Release on June 30, 2010 ============================================================
****************************************** 
Known issues
****************************************** 

* If the right-hand side pane of the SharePointGuidance2010.chm or SharePointGuidanceLibrary2010.chm file does not display correctly, take the following steps:

    (1) In Windows Explorer, open the folder that contains the CHM file, right-click the CHM file, and then click Properties.

	(2) In the Properties window, click Unblock, and then click OK.

	(3) Double-click the CHM file. If you see an Open File-Security Warning dialog box, clear the Always ask before opening this file check box.

* If the drop folder has an ampersand (&) in the folder name, for example C:\P&P-SPG, the install scripts for the reference implementations will fail. 


******************************************
What's in this download
******************************************

This download includes documentation in various formats together with several Visual Studio 2010 solutions containing source code.

* Documentation **************************
  
  *DropLocation\Docs\SharePointGuidance.chm
    This is the guidance documentation in CHM format.
  
  *DropLocation\Docs\SharePointGuidanceLibrary2010.chm
	This is the API documentation for the SharePoint Guidance Library in CHM format.

* Solutions ****************************** 

  Note: Many of the solutions have a parallel solution that includes unit tests. 
  You must download the Moles framework if you want to run the unit tests.
  Each reference implementation includes a readme.txt file in the root folder that explains
  how to install and configure the reference implementation components.

  *DropLocation\Source\SharePoint 2010\Microsoft.Practices.SharePoint.sln
	The solution includes the projects that contain the SharePoint Service Locator, the Application Settings Manager, 
	and the SharePoint Logger.
  
  *DropLocation\Source\ExecutionModels\Sandboxed\ExecutionModels.Sandboxed.sln
	The solution contains the source code and resources for the Sandboxed Solution Reference Implementation.
	
  *DropLocation\Source\ExecutionModels\FullTrust\ExecutionModels.FarmSolution.sln
	The solution contains the source code and resources for the Farm Solution Reference Implementation.
		   
  *DropLocation\Source\ExecutionModels\Proxy\ExecutionModels.Sandboxed.Proxy.sln
	The solution contains the source code and resources for the Full Trust Proxy Reference Implementation.

  *DropLocation\Source\ExecutionModels\ExternalList\ExecutionModels.Sandbox.ExternalList.sln
	The solution contains the source code and resources for the External List Reference Implementation.
		   
  *DropLocation\Source\ExecutionModels\WorkFlow\ExecutionModels.Workflow.sln
	The solution contains the source code and resources for the Workflow Reference Implementation.

  *DropLocation\Source\DataModels\DataModels.ExternalData\DataModels.ExternalData.sln
    The solution contains the source code and resources for the External Data Models Reference Implementation.
    
  *DropLocation\Source\DataModels\DataModels.SharePointList\DataModels.SharePointList.sln
    The solution contains the source code and resources for the SharePoint List Data Models Reference Implementation.
    
  *DropLocation\Source\DataModels\DataModels.ListOf\ListOf.sln
    The solution contains the source code and resources for the "List Of" Reference Implementation.
    This reference implementation is not documented, but helps to demonstrate design patterns for large or complex lists.
  
  *DropLocation\Source\Client\Client.sln
    The solution contains the source code and resources for the "Client" Reference Implementation.
   

* Keys folder ******************************
   
   *DropLocation\Keys\Microsoft.Practices.Sharepoint.snk
     This is the public key that is used in the SharePoint Guidance Library (Microsoft.Practices.SharePoint.sln)

* Lib folder ******************************* 
   
   *Microsoft.Practices.ServiceLocation.dll
    This assembly is used by Microsoft.Practices.SharePoint.sln and ExecutionModels.Sandboxed.sln


