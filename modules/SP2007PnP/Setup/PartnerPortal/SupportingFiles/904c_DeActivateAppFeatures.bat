call 00_Parameters.bat

echo set path to execute VC commands
call setEvn.bat

set path=%path%; %SystemRoot%\Microsoft.NET\Framework\v3.5; %SystemRoot%\Microsoft.NET\Framework\v2.0.50727;

REM ********************************************
REM ******  DeActivate Features in Webapp ********
REM ********************************************

Echo DeActiving Features in %webappurl% =============================================

REM Deactivate feature "ContosoPSSGlobalNav"
echo "%SPAdminTool%" -o deactivatefeature -id %ContosoPSSGlobalNavWebApp_FeatureId% -url %webappurl%
     "%SPAdminTool%" -o deactivatefeature -id %ContosoPSSGlobalNavWebApp_FeatureId% -url %webappurl%

REM Deactivate feature "AJAXSupport" 
echo "%SPAdminTool%" -o deactivatefeature -id %AJAXSupport_FeatureId% -url %webappurl%
     "%SPAdminTool%" -o deactivatefeature -id %AJAXSupport_FeatureId% -url %webappurl%

REM Deactivate feature "Contoso LOB Services Client"
echo "%SPAdminTool%" -o deactivatefeature -id %LOBServicesClient_FeatureId% -url %webappurl%
     "%SPAdminTool%" -o deactivatefeature -id %LOBServicesClient_FeatureId% -url %webappurl%
     

