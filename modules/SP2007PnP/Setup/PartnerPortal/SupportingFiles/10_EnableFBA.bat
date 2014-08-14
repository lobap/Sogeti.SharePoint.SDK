call 00_Parameters.bat

REM ************************************************
REM ********** Build Setup.exe *********************
REM ************************************************

cd "%subfolder%"
call BuildContosoSetupExe.bat


REM ************************************************
REM ********** Import ContosoFBAdb *****************
REM ************************************************

cd "%subfolder%"
ECHO %time%  Status : **** Importing ContosodB into %SQLServerName%

cd %subfolder%
SQLCMD -E -S "%SQLServerName%" -i .\ContosoFBAdb.sql

REM ************************************************
REM ********** Extend Contoso WebApp ***************
REM ************************************************

ECHO %time%  Status : **** Extending Web Application : %webappurl% (it may take several minutes)

echo Extend Web Application for %webappurl% to Intranet Zone: http://%LoadBanlancedHost%:%ExtranetPort%
	"%SPAdminTool%" -o extendvsinwebfarm -url http://%LoadBanlancedHost%:%ExtranetPort% -vsname %ContosoWeb%

echo Change Authentication for Extranet(http://%LoadBanlancedHost%:%ExtranetPort%) to Partners
 "%SPAdminTool%" -o authentication -url http://%LoadBanlancedHost%:%ExtranetPort% -type Forms -membershipprovider Partners -rolemanager PartnerGroups


REM *********************************************************
REM ********** Update Web.Config for WebApp Extranet port ***
REM *********************************************************

REM Get Web.Config Folders for ExtranetPort
FOR /F "tokens=1*" %%G IN ('""%subfolder%getmetabaseid.js" "SharePoint - %ExtranetPort%""') DO SET ExtranetMetaId=%%G
FOR /F "tokens=4*" %%G IN ('CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS GET w3svc/%ExtranetMetaId%/root/path') DO SET ExtranetPath=%%G

ECHO update web.config for Extranet to add partners and partnergroups
call "%subfolder%ContosoSetup.exe" /webconfig %SQLServerName% "%ExtranetPath%"

REM ECHO update web.config for wcf on Extranet config
REM call "%subfolder%ContosoSetup.exe" /wcfconfig "%ExtranetPath%"

REM ************************************************
REM ********** Update Web.Config for ContosoSSP   **
REM ************************************************

REM Get Web.Config Folders for %ContosoSSPWeb%
FOR /F "tokens=1*" %%G IN ('""%subfolder%getmetabaseid.js" "%ContosoSSPWeb%""') DO SET SSPMetaId=%%G
FOR /F "tokens=4*" %%G IN ('CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS GET w3svc/%SSPMetaId%/root/path') DO SET SSPWebPath=%%G

ECHO update web.config for SSP
call "%subfolder%ContosoSetup.exe" /webconfig %SQLServerName% "%SSPWebPath%"

REM ************************************************
REM ********** Extend ContosoSSP Web App ***********
REM ************************************************

ECHO Extending ContosoSSPWeb to http://%LoadBanlancedHost%:%SSPExtwepAppPort% as Intranet Zone
	"%SPAdminTool%" -o extendvsinwebfarm -url http://%LoadBanlancedHost%:%SSPExtwepAppPort% -vsname %ContosoSSPWeb%
echo Change Authentication for SSP Intranet (http://%LoadBanlancedHost%:%SSPExtwepAppPort%) to Partners
"%SPAdminTool%" -o authentication -url http://%LoadBanlancedHost%:%SSPExtwepAppPort% -type Forms -membershipprovider Partners -rolemanager PartnerGroups

REM ************************************************
REM *** Config BDC Access for FBA partners *********
REM ************************************************

call "%subfolder%ContosoSetup.exe" /bdcfbaaccess "%ContosoSSP%"

REM ************************************************
REM ********** Update Web.Config for CentralAdmin  * 
REM ****************************************************

REM Get Web.Config Folders for Central Admin
FOR /F "tokens=1*" %%G IN ('""%subfolder%getmetabaseid.js" "SharePoint Central Administration v3""') DO SET CenAdminMetaId=%%G
FOR /F "tokens=4*" %%G IN ('CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS GET w3svc/%CenAdminMetaId%/root/path') DO SET CenAdminWebPath=%%G
 
ECHO update web.config for CentralAdmin
call "%subfolder%ContosoSetup.exe" /centralwebconfig %SQLServerName% "%CenAdminWebPath%"


REM ************************************************************
REM ********** Add FBA User Permissions to site collections  ***
REM ************************************************************

Echo Add ContosoFbaPartner1 to Partner1 site %partner1url%
"%SPAdminTool%" -o adduser  -url %partner1url% -userlogin PartnerGroups:ContosoFbaPartner1 -useremail partner1@SPG.com -role Design -username ContosoFbaPartner1

Echo Add ContosoFbaPartner2 to Partner2 site %partner2url%
"%SPAdminTool%" -o adduser  -url %partner2url% -userlogin PartnerGroups:ContosoFbaPartner2 -useremail partner1@SPG.com -role Design -username ContosoFbaPartner2

Echo Add ContosoFbaPartners to Default sites %webappurl%
"%SPAdminTool%" -o adduser  -url %webappurl% -userlogin PartnerGroups:ContosoFbaPartner1 -useremail partner1@SPG.com -role Read -username ContosoFbaPartner1
"%SPAdminTool%" -o adduser  -url %webappurl% -userlogin PartnerGroups:ContosoFbaPartner2 -useremail partner1@SPG.com -role Read -username ContosoFbaPartner2

Echo Add ContosoFbaPartners to Portal sites %promotionsurl%
"%SPAdminTool%" -o adduser  -url %promotionsurl% -userlogin PartnerGroups:ContosoFbaPartner1 -useremail partner1@SPG.com -role Read -username ContosoFbaPartner1
"%SPAdminTool%" -o adduser  -url %promotionsurl% -userlogin PartnerGroups:ContosoFbaPartner2 -useremail partner1@SPG.com -role Read -username ContosoFbaPartner2

Echo Add ContosoFbaPartners to Portal sites %productcatalogurl%
"%SPAdminTool%" -o adduser  -url %productcatalogurl% -userlogin PartnerGroups:ContosoFbaPartner1 -useremail partner1@SPG.com -role Read -username ContosoFbaPartner1
"%SPAdminTool%" -o adduser  -url %productcatalogurl% -userlogin PartnerGroups:ContosoFbaPartner2 -useremail partner1@SPG.com -role Read -username ContosoFbaPartner2


REM ************************************************************
REM ********** Add %ContosoAppPoolId% to Contosofba DB as owner ***
REM ************************************************************


cd %subfolder%

ECHO PRINT 'Adding ContosoService as dbowner' > ServiceLoginPermissions.sql
ECHO GO >> ServiceLoginPermissions.sql
ECHO USE [master] >> ServiceLoginPermissions.sql
ECHO GO >> ServiceLoginPermissions.sql
ECHO CREATE LOGIN [%ContosoAppPoolId%] FROM WINDOWS WITH DEFAULT_DATABASE=[ContosoFBAdb] >> ServiceLoginPermissions.sql
ECHO GO >> ServiceLoginPermissions.sql
ECHO USE [ContosoFBAdb] >> ServiceLoginPermissions.sql
ECHO GO >> ServiceLoginPermissions.sql
ECHO CREATE USER [%ContosoAppPoolId%] FOR LOGIN [%ContosoAppPoolId%] >> ServiceLoginPermissions.sql
ECHO GO >> ServiceLoginPermissions.sql
ECHO USE [ContosoFBAdb] >> ServiceLoginPermissions.sql
ECHO GO >> ServiceLoginPermissions.sql
ECHO EXEC sp_addrolemember N'db_owner', N'%ContosoAppPoolId%' >> ServiceLoginPermissions.sql
ECHO GO >> ServiceLoginPermissions.sql

cd %subfolder%
SQLCMD -E -S "%SQLServerName%" -i .\ServiceLoginPermissions.sql
