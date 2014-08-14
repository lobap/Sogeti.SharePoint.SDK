Developing SharePoint Applications–August 2009
Welcome to the August 2009 release of the patterns & practices SharePoint Guidance. This file contains late-breaking information that can be useful in using the guidance.

============
Known Issues
============
To see the SharePoint Guidance Library known issues, see: http://spg.codeplex.com/Wiki/View.aspx?title=Known%20Issues%20%2f%20Fixes.

============
System Requirements
============
 * Supported Operating Systems: Windows Server 2003; Windows Server 2008
 * Microsoft Visual Studio 2008 with Service Pack 1.
 * Microsoft .NET Framework 3.5
 * Microsoft Silverlight 2 SDK
 * Microsoft Office SharePoint Server 2007 with Service Pack 1 or Service Pack 2
 * Windows SharePoint Services 3.0 Tools: Visual Studio 2008 Extensions, version 1.3
 * Windows SharePoint Services 3.0: Software Development Kit (SDK)



==================================================     
Compiling the SharePoint Guidance Library Source
==================================================
To compile the SharePoint Guidance Library source code, perform these steps:

	1. Open the solution (Source\Microsoft.Practices.SPG2\Microsoft.Practices.SPG2.sln) in Visual Studio.
	2. Build the solution. 
		- The Microsoft.Practices.SPG.Common assembly will be added to the Global Application Cache.
		- The Microsoft.Practices.SPG.SubSiteCreation assembly will be placed in the Source\Microsoft.Practices.SPG2\Microsoft.Practices.SPG.SubSiteCreation\bin\Debug folder. This assembly contains a subsite creation workflow and associated workflow activities.
		- The Microsoft.Practices.SPG.SubSiteCreation.Features assembly will be placed in the Source\Microsoft.Practices.SPG2\Microsoft.Practices.SPG.SubSiteCreation\bin\Debug folder. This assembly packages workflow and activities from the Microsoft.Practices.SPG.SubSiteCreation assembly into a Web Part Solution Package (WSP).


========================================================
Solutions Included in the SharePoint Guidance
========================================================
The SharePoint Guidance includes several sample solutions.

 * Source\PartnerPortal\Contoso.PartnerPortal.sln
 * Source\PartnerPortal\Contoso.PartnerPortal (with Tests).sln
 * Source\Microsoft.Practices.SPG2\Microsoft.Practices.SPG2.sln
 * Source\Microsoft.Practices.SPG2\Microsoft.Practices.SPG2 (With Tests).sln
 * Source\TM\Contoso.TrainingManagement.RI\Contoso.TrainingManagement.RI.sln
 * Source\TM\Contoso.TrainingManagement.RI\Contoso.TrainingManagement.RI (Unit Tests).sln
 * Source\TM\Contoso.TrainingManagement.RI.UpdateTheme\Contoso.TrainingManagement.RI.UpdateTheme.sln
 * Source\TM\Contoso.TrainingManagement.RI.Upgrade\Contoso.TrainingManagement.RI.Upgrade.sln


====================================
Documentation
====================================
This SharePoint Guidance includes the following documentation:

	* Developing SharePoint Applications – Aug 2009.chm: This is the guidance documentation.
	* Developing SharePoint Applications Library Reference - Aug 2009.chm: This is the class library reference documentation.