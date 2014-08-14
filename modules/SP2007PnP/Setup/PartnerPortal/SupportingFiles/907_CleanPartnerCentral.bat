call 00_Parameters.bat

ECHO %time% Status : **** Clean SPGsubSite, partnerdirectory, partnercentral
	"%SPAdminTool%" -o deleteweb -url %SpgSubsiteUrl%
	"%SPAdminTool%" -o deleteweb -url %partnerdirectory%
	"%SPAdminTool%" -o deletesite -url %adminurl% 

