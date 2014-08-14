call 00_Parameters.bat

REM ====================================================================================
REM ***** Delete ContosoWeb App *****
echo "%SPAdminTool%" -o unextendvs -url %webappurl% -deletecontent -deleteiissites
	 "%SPAdminTool%" -o unextendvs -url %webappurl% -deletecontent -deleteiissites

REM ====================================================================================
REM ***** Delete ContosSSP
echo "%SPAdminTool%" -o deletessp -title %ContosoSSP% -force -deletedatabases
	 "%SPAdminTool%" -o deletessp -title %ContosoSSP% -force -deletedatabases

REM ====================================================================================
REM ***** Delete ContosSSPWEB App
echo "%SPAdminTool%" -o unextendvs -url %SSPurl% -deletecontent -deleteiissites
	 "%SPAdminTool%" -o unextendvs -url %SSPurl% -deletecontent -deleteiissites

ECHO Status : **** deleting ContosodB from  %SQLServerName%
SQLCMD -E -S "%SQLServerName%" -i .\deleteContosoDBs.sql