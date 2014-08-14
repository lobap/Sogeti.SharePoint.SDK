REM =======================================================================================================
REM =====START SET path if user try to run individual Files ===============================================
	IF "%SourceFolder%"=="" GOTO SetSourceFolder
		GOTO AfterSourceFolder
	:SetSourceFolder	
		Set SourceFolder=%~dp0%..\..\..\
		Set subfolder=%~dp0%
	:AfterSourceFolder

REM =======================================================================================================
REM ==== set SQLserverName for SQL Server Instance to install ContosoFBAdb (for FBA roles and members) ====

	set SQLserverName=%computername%

	if exist "%ProgramFiles%\Microsoft Office Servers\12.0\Data"  goto OFFICESERVERS
	if exist "%ProgramFiles(X86)%\Microsoft Office Servers\12.0\Data"  goto OFFICESERVERS
	goto END_SQLserverName		
	:OFFICESERVERS
		set SQLserverName=%computername%\OFFICESERVERS
	:END_SQLserverName

	REM set	SQLserverName=%computername%\OFFICESERVERS
	REM set SQLserverName=%computername%\SQLExpress
	REM set SQLserverName=%computername%
	
REM =======================================================================================================
REM === Whether to use Load Balanced Host ================================================================

	set LoadBanlancedHost=%computername%
	REM set LoadBanlancedHost=ContosoNLB

REM =======================================================================================================
REM === Use Domain User or Local User for AppPool =========================================================

	set AppPoolIdLocation=%computername%
	REM set AppPoolIdLocation=%USERDOMAIN%
	
REM =======================================================================================================
REM ==== local or immediate solution deployment	
	set DeployLocalOrImmediate=local
REM =======================================================================================================
REM ==== Default value for multiple WFEs farm ==============================================================
REM === Update YourSQLServerName with Actual Sql server instance.
	REM set DeployLocalOrImmediate=immediate
	REM set SQLserverName=YourSQLServerName
	REM set LoadBanlancedHost=ContosoNLB
	REM set AppPoolIdLocation=%USERDOMAIN%

REM =======================================================================================================
REM ===== SharePoint WebApp Owner Information =============================================================

	set OwnerLogin=%USERDOMAIN%\%USERNAME%
	set owneremail=%USERNAME%@%USERDOMAIN%

REM =======================================================================================================
REM ======Common Password for ContosoWinAdmin =============================================================

   	set Password=P2ssw0rd$

REM =======================================================================================================
REM ===== Contoso Services  ===============================================================================

    REM set ServicesSource=Services
    set ServicesSource=Source
    set ServicesWebName=ContosoServices8585
    set AppPoolName=ContosoServiceAppPool
    set AppPoolLogin=%AppPoolIdLocation%\ContosoWinAdmin
    set AppPoolServiceLogin=%AppPoolIdLocation%\ContosoServiceUser
    set AppPoolLoginPassword=%Password%
    
    set SPServicesWebName=ContosoSPServices8787
    set SPAppPoolName=ContosoSPServiceAppPool
    set SPAppPoolLogin=%AppPoolIdLocation%\ContosoAppPoolUser
    set SPAppPoolServiceLogin=%AppPoolIdLocation%\ContosoAppPoolUser
    set SPAppPoolLoginPassword=%Password%

REM =======================================================================================================
REM ===== Contoso Web App =================================================================================

    set ContosoWinAdmin=%AppPoolIdLocation%\ContosoWinAdmin
    set ContosoAppPoolId=%AppPoolIdLocation%\ContosoAppPoolUser
    set ContosoAppPoolPwd=%Password%
    set ContosoAppPoolEmail=ContosoAppPool@SPG.COM

   
    set ContosoWebAppPool=ContosoWebAppPool
    set ContosoWebAppPoolId=%AppPoolIdLocation%\ContosoAppPoolUser
    set ContosoWebAppPoolPssword=%Password%
    set ContosoWebAppPoolEmail=ContosoWinAdmin@SPG.COM

	set ContosoCommonPackageName=ContosoCommon.wsp
	set PortalPackageName=ContosoPartnerPortal.wsp
	set ProductCatalogPackageName=ContosoPartnerPortalProductCatalog.wsp
	set PromotionsPackageName=ContosoPartnerPortalPromotions.wsp
	set CollaborationPackageName=ContosoPartnerPortalCollaboration.wsp
	set OrderExceptionPackageName=ContosoPartnerPortalCollaborationOrderException.wsp
	set SubSiteCreationPackageName=MicrosoftPracticesSPGSubSiteCreation.wsp
	set IncidentPackageName=ContosoPartnerPortalCollaborationIncident.wsp
	set AJAXSupportPackageName=MicrosoftPracticesSPGAJAXSupport.wsp
	set PartnerCentralPackageName=ContosoPartnerPortalPartnerCentral.wsp
	set PartnerDirectoryPackageName=ContosoPartnerPortalPartnerDirectory.wsp
	set ServicesSupportPackageName=ContosoLOBServicesClient.wsp

	set ContosoWebPort=9001
	set ExtranetPort=9002
	set webappurl=http://%LoadBanlancedHost%:%ContosoWebPort%
	set promotionsurl=%webappurl%/sites/promotions
	set productcatalogurl=%webappurl%/sites/productcatalog
	set partner1url=%webappurl%/sites/partner1
	set partner2url=%webappurl%/sites/partner2
	set adminweb=/sites/PartnerCentral
	set adminurl=%webappurl%%adminweb%
	set SpgSubsiteParent=%adminurl%
	set SpgSubsiteUrl=%SpgSubsiteParent%/SpgSubsite
	set partnerdirectory=%SpgSubsiteParent%/partnerdirectory
	set ContosoWeb=ContosoWeb

    set ContosoPartnerPortalEventSource_FeatureId=306e3ea3-ecaf-4542-9282-58a4a8134b3e

	set AJAXSupport_FeatureId=e3446eb2-a4e1-48ed-927e-c0f167d57a7c

	set ContosoContextualHelp_FeatureId=92a99385-aefc-4718-98c2-1ac541c55a6e

	set ContosoPSSGlobalNavWebApp_FeatureId=2f9a50ad-6165-4a3d-8b37-cb2666a62dbd
	set ContosoPSSGlobalNav_FeatureId=be8c9c0c-3201-45e7-a433-806d9dfc35eb

	set ContosoTheme_FeatureId=7a576729-6fb8-4597-bbad-59c2aa7d7d0e

	set ContosoPSSOrderExceptionSite_FeatureId=c5decae2-d3e2-4048-a25a-42c5fe6eac29
	set ContosoPSSOrderExceptionWeb_FeatureId=23389722-f5d6-42da-b39b-ff75cf991ab3

	set ContosoProductCatalogSite_FeatureId=946de646-f75e-4800-9694-d4426c586612
	set ContosoProductCatalogWeb_FeatureId=68a85277-0049-49ed-8d52-fd4dbbd52f1b

	set ContosoPromotionsSite_FeatureId=6fd12cb8-f910-4cf4-b7d0-6b74769483ae
	set ContosoPromotionWeb_FeatureId=2c988455-7e87-4a52-a81d-2756121ff663

	set IncidentDashboard_FeatureId=86dfa338-3643-49c3-a935-26b043547326

	set MicrosoftSubSiteCreationSite_FeatureId=9c6f9bca-2cf5-4ec7-b493-39d081730ca7
	set MicrosoftSubSiteCreationWeb_FeatureId=d962e88b-1ce2-44f3-b30d-8d8f23dfc2e2
	
	set IncidentSubSiteResources_FeatureId=1c28f48f-6544-45b3-8db0-e87977c09e46

	set LOBServicesClient_FeatureId=8b0f085e-72a0-4d9f-ac74-0038dc0f6dd5

	set PartnerRollupWebPart_FeatureId=212f448e-32bb-4bee-b7c6-f845cf7c33ac
	set PartnerRollupPage_FeatureId=57e15457-3dff-4be4-b22c-08f1325463ff

	set PartnerCollaborationSite_FeatureId=f15ec5c3-d97f-4838-b874-0224dbbee676
	set PartnerCollaborationWeb_FeatureId=0d4249b5-6c67-466a-8c5a-a49cb7aff73e
	
	set PartnerSiteDirectorySite_FeatureId=24B19A96-B876-47d2-B43A-6FA7D5A70D07
	set PartnerSiteDirectoryWeb_FeatureId=e541660c-c063-4c97-8706-dc486914dc27
	
	set PartnerRedirect_FeatureId=d56f7159-1f51-4b34-8a1f-a69f173244f1


REM =======================================================================================================
REM ===== Contoso SSP =====================================================================================
    set ContosoSSPAppPool=ContosoSSPAppPool
    set ContosoSSPAppPoolId=%AppPoolIdLocation%\ContosoAppPoolUser
    set ContosoSSPAppPoolPssword=%Password%
	
	set ContosoSSPDB=ContosoSSPDB
	set ContosoSSPAppDB=ContosoSSPAppDB
	set ContosoSSPWeb=ContosoSSPWeb
	set ContosoSSP=ContosoSSP

	set SSPwepAppPort=9004
	set SSPExtwepAppPort=9005

	set mysiteurl=/mysites

	set SSPurl=http://%LoadBanlancedHost%:%SSPwepAppPort%
	set indexserver=%computername%

	if exist "%ProgramFiles(X86)%"  goto On64bitOS
	:On32bitOS
		set indexlocation=%ProgramFiles%\Microsoft Office Servers\12.0\Data\Office server\Applications
		goto end
	:On64bitOS
		set indexlocation=%ProgramFiles(X86)%\Microsoft Office Servers\12.0\Data\Office server\Applications
	:end

REM =======================================================================================================
REM =========  Authroing or Targeting     =================================================================
	Set ContentDeployment=Authoring
REM	Set ContentDeployment=Targeting

REM =======================================================================================================
REM =========  STSADM and devenv location =================================================================

	set devenv=devenv
	set SPAdminTool=%CommonProgramFiles%\Microsoft Shared\web server extensions\12\BIN\stsadm.exe
	
	if exist "%SPAdminTool%" goto STSEnd
		SPAdminTool=C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm.exe
		
	if exist "%SPAdminTool%" goto STSEnd
		SPAdminTool=C:\Program Files (x86)\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm.exe	
	:STSEnd
	
