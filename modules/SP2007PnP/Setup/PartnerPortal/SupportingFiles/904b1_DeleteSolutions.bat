call 00_Parameters.bat

echo set path to execute VC commands
call setEvn.bat

set path=%path%; %SystemRoot%\Microsoft.NET\Framework\v3.5; %SystemRoot%\Microsoft.NET\Framework\v2.0.50727;

echo Deleting solution %ContosoCommonPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%ContosoCommonPackageName%"

echo Deleting solution %ServicesSupportPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%ServicesSupportPackageName%"
    
echo Deleting solution %SubSiteCreationPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%SubSiteCreationPackageName%"

echo Deleting solution %AJAXSupportPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%AJAXSupportPackageName%"
    
echo Deleting solution %PortalPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%PortalPackageName%"

echo Deleting solution %ProductCatalogPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%ProductCatalogPackageName%"

echo Deleting solution %PromotionsPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%PromotionsPackageName%"

echo Deleting solution %CollaborationPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%CollaborationPackageName%"

echo Deleting solution %IncidentPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%IncidentPackageName%"
    
echo Deleting solution %OrderExceptionPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%OrderExceptionPackageName%"

echo Deleting solution %PartnerCentralPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%PartnerCentralPackageName%"
    
echo Deleting solution %PartnerDirectoryPackageName% from SharePoint ...
    "%SPAdminTool%" -o deletesolution -name "%PartnerDirectoryPackageName%"
