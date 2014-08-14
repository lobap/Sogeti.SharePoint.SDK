call 00_Parameters.bat

REM Deploy solutions ===========================================


REM Create Site Collections =======================================

ECHO %time%  Status : **** Creating Partner1 Portal(Site Collection) : %parter1url%
ECHO %time%  Status : **** Creating Partner2 Portal(Site Collection) : %parter2url%
  "%SPAdminTool%" -o Createsite -url %partner1url% -owneremail %owneremail% -ownerlogin %ownerlogin% -sitetemplate STS#0 -description "Partner1"
  "%SPAdminTool%" -o Createsite -url %partner2url% -owneremail %owneremail% -ownerlogin %ownerlogin% -sitetemplate STS#0 -description "Partner2"
  
ECHO %time%  Status : **** Creating Partner1 Incidents Site : %parter1url%/incidents
ECHO %time%  Status : **** Creating Partner2 Incidents Site : %parter2url%/incidents
  "%SPAdminTool%" -o createweb -url %partner1url%/incidents -sitetemplate sts#1 -title Incidents
  "%SPAdminTool%" -o createweb -url %partner2url%/incidents -sitetemplate sts#1 -title Incidents

ECHO %time%  Status : **** Creating Partner1 Order Exceptions Site : %parter1url%/orderexceptions
ECHO %time%  Status : **** Creating Partner2 Order Exceptions Site : %parter2url%/orderexceptions
  "%SPAdminTool%" -o createweb -url %partner1url%/orderexceptions -sitetemplate sts#1 -title "Order Exceptions"
  "%SPAdminTool%" -o createweb -url %partner2url%/orderexceptions -sitetemplate sts#1 -title "Order Exceptions"

REM Activate Features ========================================

echo Activating feature PublishingSite ...
    "%SPAdminTool%" -o activatefeature -id F6924D36-2FA8-4f0b-B16D-06B7250180FA -url %partner1url%
    "%SPAdminTool%" -o activatefeature -id F6924D36-2FA8-4f0b-B16D-06B7250180FA -url %partner2url%

echo Activating feature PublishingWeb ...
    "%SPAdminTool%" -o activatefeature -id 94C94CA6-B32F-4da9-A9E3-1F3D343D7ECB -url %partner1url%
    "%SPAdminTool%" -o activatefeature -id 94C94CA6-B32F-4da9-A9E3-1F3D343D7ECB -url %partner2url%

echo Activating feature ContosoPartnerPortalPromotionsSite
    "%SPAdminTool%" -o activatefeature -id %ContosoPromotionsSite_FeatureId% -url %partner1url%
    "%SPAdminTool%" -o activatefeature -id %ContosoPromotionsSite_FeatureId% -url %partner2url%    
    
echo Activating feature PartnerCollaborationSite ...
    "%SPAdminTool%" -o activatefeature -id %PartnerCollaborationSite_FeatureId% -url %partner2url%
    "%SPAdminTool%" -o activatefeature -id %PartnerCollaborationSite_FeatureId% -url %partner1url%

echo Activating feature PartnerCollaborationWeb 
    "%SPAdminTool%" -o activatefeature -id %PartnerCollaborationWeb_FeatureId% -url %partner1url%
    "%SPAdminTool%" -o activatefeature -id %PartnerCollaborationWeb_FeatureId% -url %partner2url%

echo Activating feature IncidentSubSiteResources 
    "%SPAdminTool%" -o activatefeature -id %IncidentSubSiteResources_FeatureId% -url %partner1url%
    "%SPAdminTool%" -o activatefeature -id %IncidentSubSiteResources_FeatureId% -url %partner2url%

echo Activating feature IncidentDashboard
    "%SPAdminTool%" -o activatefeature -id %IncidentDashboard_FeatureId% -url %partner1url%/incidents
    "%SPAdminTool%" -o activatefeature -id %IncidentDashboard_FeatureId% -url %partner2url%/incidents

echo Activating feature ContosoPSSOrderExceptionSite
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSOrderExceptionSite_FeatureId% -url %partner1url%
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSOrderExceptionSite_FeatureId% -url %partner2url%
    
echo Activating feature ContosoTheme ...
    "%SPAdminTool%" -o activatefeature -id %ContosoTheme_FeatureId% -url %partner2url%
    "%SPAdminTool%" -o activatefeature -id %ContosoTheme_FeatureId% -url %partner1url%
    "%SPAdminTool%" -o activatefeature -id %ContosoTheme_FeatureId% -url %partner1url%/incidents
    "%SPAdminTool%" -o activatefeature -id %ContosoTheme_FeatureId% -url %partner1url%/orderexceptions
    "%SPAdminTool%" -o activatefeature -id %ContosoTheme_FeatureId% -url %partner2url%/incidents
    "%SPAdminTool%" -o activatefeature -id %ContosoTheme_FeatureId% -url %partner2url%/orderexceptions

echo Activating feature ContosoContextualHelp ...
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %partner2url%
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %partner1url%
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %partner1url%/incidents
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %partner1url%/orderexceptions
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %partner2url%/incidents
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %partner2url%/orderexceptions
    
echo Activating feature ContosoPSSGlobalNav ...
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNav_FeatureId% -url %partner2url%
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNav_FeatureId% -url %partner1url%
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNav_FeatureId% -url %partner1url%/incidents
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNav_FeatureId% -url %partner1url%/orderexceptions
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNav_FeatureId% -url %partner2url%/incidents
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNav_FeatureId% -url %partner2url%/orderexceptions


REM Add Global Template ========================================

ECHO Adding subsiteCreation template to Global
    "%SPAdminTool%" -o addtemplate -filename "%SourceFolder%Source\PartnerPortal\incidentsubsite.stp" -title "SPGSubsiteTemplate"

