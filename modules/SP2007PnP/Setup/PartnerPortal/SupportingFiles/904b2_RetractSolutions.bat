call 00_Parameters.bat

echo set path to execute VC commands
call setEvn.bat

set path=%path%; %SystemRoot%\Microsoft.NET\Framework\v3.5; %SystemRoot%\Microsoft.NET\Framework\v2.0.50727;

REM *************************************************************************************
REM ************** Retract Solutions from Global                 ************************
REM *************************************************************************************


REM ContosoCommon =============================================
echo Retracting solution %ContosoCommonPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%ContosoCommonPackageName%" -%DeployLocalOrImmediate%

REM PartnerPortal =============================================
echo Retracting solution %PortalPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%PortalPackageName%" -%DeployLocalOrImmediate%

REM ServicesSupport =============================================
echo Retracting solution %ServicesSupportPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%ServicesSupportPackageName%" -%DeployLocalOrImmediate%

REM SubSiteCreation Package =============================================
echo Retracting solution %SubSiteCreationPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%SubSiteCreationPackageName%" -%DeployLocalOrImmediate%

REM AJAXSupport =============================================
echo Retracting solution %AJAXSupportPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%AJAXSupportPackageName%" -%DeployLocalOrImmediate%
    
REM Collaboration =============================================  
echo Retracting solution %CollaborationPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%CollaborationPackageName%" -%DeployLocalOrImmediate%

REM PartnerDirectory =============================================
echo Retracting solution %PartnerDirectoryPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%PartnerDirectoryPackageName%" -%DeployLocalOrImmediate%


REM *************************************************************************************
REM ************** Retract Solutions from webappurl              ************************
REM *************************************************************************************


REM Incident Package =============================================
echo Retracting solution %IncidentPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%IncidentPackageName%" -url %webappurl% -%DeployLocalOrImmediate%

REM OrderException =============================================
echo Retracting solution %OrderExceptionPackageName% ...
   "%SPAdminTool%" -o retractsolution -name "%OrderExceptionPackageName%" -url %webappurl% -%DeployLocalOrImmediate%

REM PartnerCentral =============================================
echo Retracting solution %PartnerCentralPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%PartnerCentralPackageName%" -url %webappurl% -%DeployLocalOrImmediate%

REM ProductCatalog =============================================
echo Retracting solution %ProductCatalogPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%ProductCatalogPackageName%" -url %webappurl% -%DeployLocalOrImmediate%

REM PromotionsPackageName =============================================
echo Retracting solution %PromotionsPackageName% ...
    "%SPAdminTool%" -o retractsolution -name "%PromotionsPackageName%" -url %webappurl% -%DeployLocalOrImmediate%


REM *************************************************************************************
REM ************** make sure all timer jobs are finished in the local server
REM *************************************************************************************

echo make sure all timer jobs are finished in the local server
echo "%SPAdminTool%" -o EXECADMSVCJOBs 
     "%SPAdminTool%" -o EXECADMSVCJOBs 
