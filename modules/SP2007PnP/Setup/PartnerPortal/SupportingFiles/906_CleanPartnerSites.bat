call 00_Parameters.bat

ECHO deleting Partner1:%parter1url%
"%SPAdminTool%" -o deletesite -url %partner1url% 

ECHO deleting Partner2:%parter2url%
"%SPAdminTool%" -o deletesite -url %partner2url% 

echo Delete incidentsubsite Template
"%SPAdminTool%" -o deletetemplate -title "SPGSubsiteTemplate"