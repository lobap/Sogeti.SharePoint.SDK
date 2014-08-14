call 00_Parameters.bat

ECHO %time%  Status : **** Add ContosoWinAdmin as Full Control into %adminurl%
ECHO %time%  Status : **** Add ContosoWinPartner1 groups as Readers into %adminurl%
ECHO %time%  Status : **** Add ContosoWinPartner2 groups as Readers into %adminurl%
  "%SPAdminTool%" -o adduser  -url %adminurl% -userlogin ContosoWinAdmin -useremail ContosoWinAdmin@SPG.com -role "Full Control" -username ContosoWinAdmin
 