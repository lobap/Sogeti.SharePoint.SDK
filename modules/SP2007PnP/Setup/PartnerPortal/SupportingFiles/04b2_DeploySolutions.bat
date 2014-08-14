call 00_Parameters.bat

echo set path to execute VC commands
call setEvn.bat

set path=%path%; %SystemRoot%\Microsoft.NET\Framework\v3.5; %SystemRoot%\Microsoft.NET\Framework\v2.0.50727;

REM *************************************************************************************
REM ************** Deploy Solutions Global                       ************************
REM *************************************************************************************

echo Deploying solution %ContosoCommonPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%ContosoCommonPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force
     "%SPAdminTool%" -o deploysolution -name "%ContosoCommonPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force

echo Deploying solution %PortalPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%PortalPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force
     "%SPAdminTool%" -o deploysolution -name "%PortalPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force

echo Deploying solution %ServicesSupportPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%ServicesSupportPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force
     "%SPAdminTool%" -o deploysolution -name "%ServicesSupportPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force

echo Deploying solution %SubSiteCreationPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%SubSiteCreationPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force 
     "%SPAdminTool%" -o deploysolution -name "%SubSiteCreationPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force

echo Deploying solution %AJAXSupportPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%AJAXSupportPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force
     "%SPAdminTool%" -o deploysolution -name "%AJAXSupportPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force

echo Deploying solution %CollaborationPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%CollaborationPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force 
     "%SPAdminTool%" -o deploysolution -name "%CollaborationPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force

echo Deploying solution %PartnerDirectoryPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%PartnerDirectoryPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force
     "%SPAdminTool%" -o deploysolution -name "%PartnerDirectoryPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force
     

REM *************************************************************************************
REM ************** Deploy Solutions To webappurl                 ************************
REM *************************************************************************************

echo Deploying solution %OrderExceptionPackageName% ... 
echo "%SPAdminTool%" -o deploysolution -name "%OrderExceptionPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force -url %webappurl% 
     "%SPAdminTool%" -o deploysolution -name "%OrderExceptionPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force -url %webappurl% 
        
echo Deploying solution %IncidentPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%IncidentPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force -url %webappurl%
     "%SPAdminTool%" -o deploysolution -name "%IncidentPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force -url %webappurl%

echo Deploying solution %PartnerCentralPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%PartnerCentralPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force -url %webappurl% 
     "%SPAdminTool%" -o deploysolution -name "%PartnerCentralPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force -url %webappurl% 

echo Deploying solution %ProductCatalogPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%ProductCatalogPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force -url %webappurl% 
     "%SPAdminTool%" -o deploysolution -name "%ProductCatalogPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force -url %webappurl% 

echo Deploying solution %PromotionsPackageName% ...
echo "%SPAdminTool%" -o deploysolution -name "%PromotionsPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force -url %webappurl% 
     "%SPAdminTool%" -o deploysolution -name "%PromotionsPackageName%" -%DeployLocalOrImmediate% -allowGacDeployment -force -url %webappurl% 
     

REM *************************************************************************************
REM ************** make sure all timer jobs are finished in the local server
REM *************************************************************************************

echo make sure all timer jobs are finished in the local server
echo "%SPAdminTool%" -o EXECADMSVCJOBs 
     "%SPAdminTool%" -o EXECADMSVCJOBs 
