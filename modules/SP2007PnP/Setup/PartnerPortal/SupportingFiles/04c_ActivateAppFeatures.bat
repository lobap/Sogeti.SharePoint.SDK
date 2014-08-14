call 00_Parameters.bat

echo set path to execute VC commands
call setEvn.bat

set path=%path%; %SystemRoot%\Microsoft.NET\Framework\v3.5; %SystemRoot%\Microsoft.NET\Framework\v2.0.50727;

REM ********************************************
REM ******  Activate Features in Webapp ********
REM ********************************************

Echo Activing Features in %webappurl% =============================================

REM activate feature "AJAXSupport" 
echo "%SPAdminTool%" -o activatefeature -id %AJAXSupport_FeatureId% -url %webappurl%
     "%SPAdminTool%" -o activatefeature -id %AJAXSupport_FeatureId% -url %webappurl%

REM activate feature "Contoso LOB Services Client"
echo "%SPAdminTool%" -o activatefeature -id %LOBServicesClient_FeatureId% -url %webappurl%
     "%SPAdminTool%" -o activatefeature -id %LOBServicesClient_FeatureId% -url %webappurl%
     
REM activate feature "Contoso PSS Global Navigation"
echo "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNavWebApp_FeatureId% -url %webappurl%
     "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNavWebApp_FeatureId% -url %webappurl%


echo Activating feature ContosoPSSGlobalNav
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNav_FeatureId% -url %webappurl%

echo Activating feature ContosoTheme
    "%SPAdminTool%" -o activatefeature -id %ContosoTheme_FeatureId% -url %webappurl%
    
echo Activating feature PartnerRedirectPage ...
    "%SPAdminTool%" -o activatefeature -id %PartnerRedirect_FeatureId% -url %webappurl%
