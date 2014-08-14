call 00_Parameters.bat

ECHO Status : **** Modify Web.Config Files **************

cd "%subfolder%"
call BuildContosoSetupExe.bat
 

cd "%subfolder%"
ECHO update web.config for Extranet

REM Get Web.Config Folders for ExtranetPort
FOR /F "tokens=1*" %%G IN ('""%subfolder%\getmetabaseid.js" "%ContosoWeb%""') DO SET ContosoWebMetaId=%%G
FOR /F "tokens=4*" %%G IN ('CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS GET w3svc/%ContosoWebMetaId%/root/path') DO SET ContosoWebPath=%%G

echo call "%subfolder%ContosoSetup.exe" /centralwebconfig %SQLServerName% "%ContosoWebPath%"
     call "%subfolder%ContosoSetup.exe" /centralwebconfig %SQLServerName% "%ContosoWebPath%"

echo replace ServiceHostComputerName with the computer name
echo call "%subfolder%ContosoSetup.exe" /lobwebconfig %ComputerName% "%sourceFolder%source\PartnerPortal\Contoso.LOB.Web"
     call "%subfolder%ContosoSetup.exe" /lobwebconfig %ComputerName% "%sourceFolder%source\PartnerPortal\Contoso.LOB.Web"

