call 00_Parameters.bat

cd %subfolder%

echo "%SPAdminTool%" -o unextendvs -url http://%LoadBanlancedHost%:%ExtranetPort% -deleteiissites
     "%SPAdminTool%" -o unextendvs -url http://%LoadBanlancedHost%:%ExtranetPort% -deleteiissites

echo "%SPAdminTool%" -o unextendvs -url http://%LoadBanlancedHost%:%SSPExtwepAppPort% -deleteiissites
     "%SPAdminTool%" -o unextendvs -url http://%LoadBanlancedHost%:%SSPExtwepAppPort% -deleteiissites

ECHO Status : **** deleting ContosoFBAdb from  %SQLServerName%
cd %subfolder%
SQLCMD -E -S "%SQLServerName%" -i .\deleteContosoFBAdb.sql


