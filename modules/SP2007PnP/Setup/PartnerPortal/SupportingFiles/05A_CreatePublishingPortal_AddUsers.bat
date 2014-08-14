call 00_Parameters.bat

Echo Configure Publishing Portal Permissions =============================================
    ECHO %time%  Status : **** Adding ContosoWinPartner1 groups as Design role into %promotionsurl%
    ECHO %time%  Status : **** Adding ContosoWinPartner2 groups as Design role into %promotionsurl%
    ECHO %time%  Status : **** Adding ContosoWinAdmin as Full Control into %promotionsurl%
    "%SPAdminTool%" -o adduser  -url %promotionsurl% -userlogin contosowinpartner1 -useremail partner1@SPG.com -role Read -username ContosoWinPartner1
    "%SPAdminTool%" -o adduser  -url %promotionsurl% -userlogin contosowinpartner2 -useremail partner2@SPG.com -role Read -username ContosoWinPartner2
    "%SPAdminTool%" -o adduser  -url %promotionsurl% -userlogin ContosoWinAdmin -useremail ContosoWinAdmin@SPG.com -role "Full Control" -username ContosoWinAdmin
    
    ECHO %time%  Status : **** Adding ContosoWinPartner1 groups as Design role into %productcatalogurl%
    ECHO %time%  Status : **** Adding ContosoWinPartner2 groups as Design role into %productcatalogurl%
    ECHO %time%  Status : **** Adding ContosoWinAdmin as Full Control into %productcatalogurl%
    "%SPAdminTool%" -o adduser  -url %productcatalogurl% -userlogin contosowinpartner1 -useremail partner1@SPG.com -role Read -username ContosoWinPartner1
    "%SPAdminTool%" -o adduser  -url %productcatalogurl% -userlogin contosowinpartner2 -useremail partner2@SPG.com -role Read -username ContosoWinPartner2
    "%SPAdminTool%" -o adduser  -url %productcatalogurl% -userlogin ContosoWinAdmin -useremail ContosoWinAdmin@SPG.com -role "Full Control" -username ContosoWinAdmin
