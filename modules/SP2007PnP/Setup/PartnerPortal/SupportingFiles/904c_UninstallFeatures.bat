call 00_Parameters.bat

echo set path to execute VC commands
call setEvn.bat

set path=%path%; %SystemRoot%\Microsoft.NET\Framework\v3.5; %SystemRoot%\Microsoft.NET\Framework\v2.0.50727;

	"%SPAdminTool%" -o uninstallfeature -force -name "AJAXSupport"
	"%SPAdminTool%" -o uninstallfeature -force -name "ContosoPSSGlobalNav"
	"%SPAdminTool%" -o uninstallfeature -force -name "ContosoPSSGlobalNavWebApp"
	"%SPAdminTool%" -o uninstallfeature -force -name "ContosoTheme"
	"%SPAdminTool%" -o uninstallfeature -force -name "Contoso.PartnerPortal.OrderExceptionSiteElements"
	"%SPAdminTool%" -o uninstallfeature -force -name "ContosoPSSOrderException"
	"%SPAdminTool%" -o uninstallfeature -force -name "ContosoProductCatalogSite"
	"%SPAdminTool%" -o uninstallfeature -force -name "ContosoProductCatalogWeb"
	"%SPAdminTool%" -o uninstallfeature -force -name "ContosoPromotionsSite"
	"%SPAdminTool%" -o uninstallfeature -force -name "ContosoPromotionWeb"
	"%SPAdminTool%" -o uninstallfeature -force -name "IncidentSubSiteRollup"
	"%SPAdminTool%" -o uninstallfeature -force -name "IncidentSubSiteResources"
	"%SPAdminTool%" -o uninstallfeature -force -name "LOBServicesClient"
	"%SPAdminTool%" -o uninstallfeature -force -name "OrderExceptionInfo"
	"%SPAdminTool%" -o uninstallfeature -force -name "PartnerRollupWebPart"
	"%SPAdminTool%" -o uninstallfeature -force -name "PartnerRollupPage"
	"%SPAdminTool%" -o uninstallfeature -force -name "PartnerSiteLandingPage"
	"%SPAdminTool%" -o uninstallfeature -force -name "PartnerPromotionsWebPart"
	"%SPAdminTool%" -o uninstallfeature -force -name "SubSiteCreation"