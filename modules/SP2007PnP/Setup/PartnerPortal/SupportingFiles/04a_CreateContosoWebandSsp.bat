call 00_Parameters.bat

REM --------------------------------------------
REM ------  Create Contoso Web App  ------------
REM --------------------------------------------
ECHO %time%  Status : **** Creating Contoso Web Portal : %webappurl% (it may take several minutes)
echo "%SPAdminTool%" -o extendvs -url %webappurl% -description %ContosoWeb% -ownerlogin %ownerLogin% -owneremail %owneremail% -databasename ContosoWEBAppDB -sitetemplate STS#1 -apidname %ContosoWebAppPool% -apidtype configurableid -apidlogin %ContosoAppPoolId% -apidpwd %ContosoAppPoolPwd%
     "%SPAdminTool%" -o extendvs -url %webappurl% -description %ContosoWeb% -ownerlogin %ownerLogin% -owneremail %owneremail% -databasename ContosoWEBAppDB -sitetemplate STS#1 -apidname %ContosoWebAppPool% -apidtype configurableid -apidlogin %ContosoAppPoolId% -apidpwd %ContosoAppPoolPwd%

ECHO %time%  Status : **** Adding ContosoWinPartner1 groups as Design role into %webappurl%
echo "%SPAdminTool%" -o adduser  -url %webappurl% -userlogin contosowinpartner1 -useremail partner1@SPG.com -role Design -username ContosoWinPartner1
     "%SPAdminTool%" -o adduser  -url %webappurl% -userlogin contosowinpartner1 -useremail partner1@SPG.com -role Design -username ContosoWinPartner1

ECHO %time%  Status : **** Adding ContosoWinPartner2 groups as Design role into %webappurl%
echo "%SPAdminTool%" -o adduser  -url %webappurl% -userlogin contosowinpartner2 -useremail partner2@SPG.com -role Design -username ContosoWinPartner2
     "%SPAdminTool%" -o adduser  -url %webappurl% -userlogin contosowinpartner2 -useremail partner2@SPG.com -role Design -username ContosoWinPartner2

ECHO %time%  Status : **** Adding %ContosoWinAdmin% as Full Control into %webappurl%
echo "%SPAdminTool%" -o adduser  -url %webappurl% -userlogin %ContosoWinAdmin% -useremail %ContosoWebAppPoolEmail% -role "Full Control" -username %ContosoWinAdmin%
     "%SPAdminTool%" -o adduser  -url %webappurl% -userlogin %ContosoWinAdmin% -useremail %ContosoWebAppPoolEmail% -role "Full Control" -username %ContosoWinAdmin%

ECHO %time%  Status : **** Adding %ContosoAppPoolId% as Read into %webappurl%
echo "%SPAdminTool%" -o adduser  -url %webappurl% -userlogin %ContosoAppPoolId% -useremail %ContosoWebAppPoolEmail% -role "Read" -username %ContosoAppPoolId%
     "%SPAdminTool%" -o adduser  -url %webappurl% -userlogin %ContosoAppPoolId% -useremail %ContosoWebAppPoolEmail% -role "Read" -username %ContosoAppPoolId%


REM ********************************************
REM ******  Create SSP WEB          ************
REM ********************************************

ECHO %time%  Status : **** Creating SSP Web Portal : %SSPurl%
echo "%SPAdminTool%" -o extendvs -url %SSPurl% -description %ContosoSSPWeb% -ownerlogin %ownerLogin% -owneremail %owneremail%  -exclusivelyusentlm -databasename %ContosoSSPAppDB% -donotcreatesite -apcreatenew -apidname %ContosoSSPAppPool% -apidtype configurableid -apidlogin %ContosoAppPoolId% -apidpwd %ContosoAppPoolPwd%
     "%SPAdminTool%" -o extendvs -url %SSPurl% -description %ContosoSSPWeb% -ownerlogin %ownerLogin% -owneremail %owneremail%  -exclusivelyusentlm -databasename %ContosoSSPAppDB% -donotcreatesite -apcreatenew -apidname %ContosoSSPAppPool% -apidtype configurableid -apidlogin %ContosoAppPoolId% -apidpwd %ContosoAppPoolPwd%

Echo Add %ownerLogin% as full control to ssp Web
echo "%SPAdminTool%" -o addpermissionpolicy  -url %SSPurl% -userlogin %ownerLogin% -permissionlevel "Full Control" -username %ownerLogin%
     "%SPAdminTool%" -o addpermissionpolicy  -url %SSPurl% -userlogin %ownerLogin% -permissionlevel "Full Control" -username %ownerLogin%

Echo Add %ContosoWinAdmin% as full control to ssp Web
echo "%SPAdminTool%" -o addpermissionpolicy  -url %SSPurl% -userlogin %ContosoWinAdmin% -permissionlevel "Full Control" -username %ContosoWinAdmin%
     "%SPAdminTool%" -o addpermissionpolicy  -url %SSPurl% -userlogin %ContosoWinAdmin% -permissionlevel "Full Control" -username %ContosoWinAdmin%


REM ******                          ************
REM ******  Create  SSP in SSP WEB  ************
REM ********************************************

ECHO %time%  Status : **** Creating SSP : %ContosoSSP%
REM  "%SPAdminTool%" -o createssp -title %ContosoSSP% -url %SSPurl% -mysiteurl %SSPurl%%mysiteurl% -ssplogin %ownerLogin% -indexserver %indexserver% -indexlocation "%indexlocation%" -ssppassword TypeOwnerPasswordHere -sspdatabasename "%ContosoSSPDB%"
echo "%SPAdminTool%" -o createssp -title %ContosoSSP% -url %SSPurl% -mysiteurl %SSPurl%%mysiteurl% -ssplogin %ContosoSSPAppPoolId% -indexserver %indexserver% -indexlocation "%indexlocation%" -ssppassword %ContosoSSPAppPoolPssword% -sspdatabasename "%ContosoSSPDB%"
     "%SPAdminTool%" -o createssp -title %ContosoSSP% -url %SSPurl% -mysiteurl %SSPurl%%mysiteurl% -ssplogin %ContosoSSPAppPoolId% -indexserver %indexserver% -indexlocation "%indexlocation%" -ssppassword %ContosoSSPAppPoolPssword% -sspdatabasename "%ContosoSSPDB%"

REM ********************************************
REM ******  Associate SSP           ************
REM ********************************************

echo Associate web apps with Conotso ssp
echo "%SPAdminTool%" -o associatewebapp -title %ContosoSSP% -url %webappurl%,%SSPurl%
     "%SPAdminTool%" -o associatewebapp -title %ContosoSSP% -url %webappurl%,%SSPurl%


