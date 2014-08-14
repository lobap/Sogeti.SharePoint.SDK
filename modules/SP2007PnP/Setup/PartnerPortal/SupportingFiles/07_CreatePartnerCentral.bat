call 00_Parameters.bat

REM ****** Create PartnerCentral Site Collection ======================================================================================================

Echo Create Partner Central Site collection using blank template and blank root site under ContosoWeb as %adminurl% 
  "%SPAdminTool%" -o Createsite -url %adminurl% -owneremail %owneremail% -ownerlogin %ownerlogin% -sitetemplate STS#1 -description "Contoso Partner Central"
 
echo %time%  Status : **** Activating MOSS Enterprise feature
  "%SPAdminTool%" -o activatefeature -id 8581A8A7-CF16-4770-AC54-260265DDB0B2 -url %adminurl%
  
echo %time%  Status : **** Activating PublishingSite feature
  "%SPAdminTool%" -o activatefeature -id F6924D36-2FA8-4f0b-B16D-06B7250180FA -url %adminurl%

echo %time%  Status : **** Activating Contoso Partner Central (Site) Feature...
  "%SPAdminTool%" -o activatefeature -id %PartnerRollupWebPart_FeatureId% -url %adminurl%
  
echo %time%  Status : **** Activating PartnerRollupPage Feature...
  "%SPAdminTool%" -o activatefeature -id %PartnerRollupPage_FeatureId% -url %adminurl%

echo %time%  Status : **** Activating Sub Site Creation Workflow Feature...
  "%SPAdminTool%" -o activatefeature -id %MicrosoftSubSiteCreationSite_FeatureId% -url %adminurl%

echo %time%  Status : **** Activating Contoso Partner Site Directory Feature...
  "%SPAdminTool%" -o activatefeature -id %PartnerSiteDirectorySite_FeatureId% -url %adminurl%
  
echo Activating feature ContosoContextualHelp ...
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %adminurl%
  
  

REM ****** Create SPGsubSite & Partner Directory ==================================================================================

ECHO %time% Status : **** Creating Subsite %SpgSubsiteUrl%
"%SPAdminTool%" -o createweb -url %SpgSubsiteUrl% -sitetemplate STS#1 -title SPGSUBSITE -description "Sub Site Creation" 

ECHO %time% Status : **** Creating Subsite %partnerdirectory% using Site Directory template
"%SPAdminTool%" -o createweb -url %partnerdirectory% -sitetemplate SPSSITES#0 -title PartnerDirectory -description "Partner Directory" 

echo Activating feature SubSiteCreation ...
"%SPAdminTool%" -o activatefeature -id %MicrosoftSubSiteCreationWeb_FeatureId% -url %SpgSubsiteUrl%

echo Activating feature Partner Site Directory ...
"%SPAdminTool%" -o activatefeature -id %PartnerSiteDirectoryWeb_FeatureId% -url %partnerdirectory%

echo Activating feature ContosoContextualHelp ...
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %SpgSubsiteUrl%
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %partnerdirectory%


REM ****** Populatae Test Data ==================================================================================

cd "%subfolder%"
call BuildContosoSetupExe.bat

Echo Populate Test Data:
call %subfolder%\ContosoSetup.exe /testdata %SpgSubsiteParent% %webappurl% http://%LoadBanlancedHost%:%ExtranetPort%


