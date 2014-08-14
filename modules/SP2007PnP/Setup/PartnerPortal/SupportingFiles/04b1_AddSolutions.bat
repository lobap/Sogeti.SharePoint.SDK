call 00_Parameters.bat

echo set path to execute VC commands
call setEvn.bat

set path=%path%; %SystemRoot%\Microsoft.NET\Framework\v3.5; %SystemRoot%\Microsoft.NET\Framework\v2.0.50727;

REM *************************************************************************************
REM ************** Add Solutions ********************************************************
REM *************************************************************************************


ECHO %time%  Status : **** Adding %ContosoCommonPackageName% Package
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.Common\bin\Debug\%ContosoCommonPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.Common\bin\Debug\%ContosoCommonPackageName%"

ECHO %time%  Status : **** Adding %PortalPackageName% Package
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal\bin\Debug\%PortalPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal\bin\Debug\%PortalPackageName%"

ECHO %time%  Status : **** Adding %ProductCatalogPackageName% Package
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.ProductCatalog\bin\Debug\%ProductCatalogPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.ProductCatalog\bin\Debug\%ProductCatalogPackageName%"

ECHO %time%  Status : **** Adding %PromotionsPackageName% Package
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.Promotions\bin\Debug\%PromotionsPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.Promotions\bin\Debug\%PromotionsPackageName%"

ECHO %time%  Status : **** Adding %CollaborationPackageName% Package
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.Collaboration\bin\Debug\%CollaborationPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.Collaboration\bin\Debug\%CollaborationPackageName%"
     
ECHO %time%  Status : **** Adding %IncidentPackageName% Package 
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.Collaboration.Incident\bin\Debug\%IncidentPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.Collaboration.Incident\bin\Debug\%IncidentPackageName%"
    
ECHO %time%  Status : **** Adding %SubSiteCreationPackageName% Package
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\Microsoft.Practices.SPG2\Microsoft.Practices.SPG.SubSiteCreation.Features\bin\Debug\%SubSiteCreationPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\Microsoft.Practices.SPG2\Microsoft.Practices.SPG.SubSiteCreation.Features\bin\Debug\%SubSiteCreationPackageName%"

ECHO %time%  Status : **** Adding %OrderExceptionPackageName% Package
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.Collaboration.OrderException\bin\Debug\%OrderExceptionPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.Collaboration.OrderException\bin\Debug\%OrderExceptionPackageName%"
     
ECHO %time%  Status : **** Adding %AJAXSupportPackageName% Package
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\Microsoft.Practices.SPG2\Microsoft.Practices.SPG.AJAXSupport\bin\Debug\%AJAXSupportPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\Microsoft.Practices.SPG2\Microsoft.Practices.SPG.AJAXSupport\bin\Debug\%AJAXSupportPackageName%"

ECHO %time%  Status : **** Adding %PartnerCentralPackageName% Package
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.PartnerCentral\bin\Debug\%PartnerCentralPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.PartnerCentral\bin\Debug\%PartnerCentralPackageName%"
     
ECHO %time%  Status : **** Adding %PartnerDirectoryPackageName% Package
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.PartnerDirectory\bin\Debug\%PartnerDirectoryPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.PartnerDirectory\bin\Debug\%PartnerDirectoryPackageName%"
     
ECHO %time%  Status : **** Adding solution %ServicesSupportPackageName% to SharePoint ...
echo "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.LOB.Services.Client\bin\Debug\%ServicesSupportPackageName%"
     "%SPAdminTool%" -o addsolution -filename "%Sourcefolder%Source\PartnerPortal\Contoso.LOB.Services.Client\bin\Debug\%ServicesSupportPackageName%"