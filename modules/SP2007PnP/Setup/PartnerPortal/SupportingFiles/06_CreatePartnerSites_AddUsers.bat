call 00_Parameters.bat

REM Add Users  =======================================

ECHO %time%  Status : **** Adding ContosoWinPartner1 group to partner1 site %partner1url% as design role 
ECHO %time%  Status : **** Adding ContosoWinPartner2 group to partner2 site %partner2url% as design role  
ECHO %time%  Status : **** Adding ContosoWinAdmin as Full Control into %partner1url%
ECHO %time%  Status : **** Adding ContosoWinAdmin as Full Control into %partner2url%
    "%SPAdminTool%" -o adduser  -url %partner1url% -userlogin contosowinpartner1 -useremail partner1@SPG.com -role Design -username ContosoWinPartner1
    "%SPAdminTool%" -o adduser  -url %partner2url% -userlogin contosowinpartner2 -useremail partner2@SPG.com -role Design -username ContosoWinPartner2
    "%SPAdminTool%" -o adduser  -url %partner1url% -userlogin ContosoWinAdmin -useremail ContosoWinAdmin@SPG.com -role "Full Control" -username ContosoWinAdmin
    "%SPAdminTool%" -o adduser  -url %partner2url% -userlogin ContosoWinAdmin -useremail ContosoWinAdmin@SPG.com -role "Full Control" -username ContosoWinAdmin
