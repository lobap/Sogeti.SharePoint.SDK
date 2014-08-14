
@rem ----------------------------------------------------------------------
@rem    Creating Contoso ApplicationPool and Services Web site under IIS
@rem ----------------------------------------------------------------------

call 00_Parameters.bat

CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS delete w3svc/AppPools/%AppPoolName%
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS delete w3svc/AppPools/%SPAppPoolName%

echo delete 8585 web site
IF exist "%systemroot%\system32\inetsrv\appcmd.exe"  GOTO appcmd

FOR /F "tokens=1*" %%G IN ('CSCRIPT getmetabaseid.js "%ServicesWebName%"') DO %systemroot%\system32\IISweb /delete w3svc/%%G
FOR /F "tokens=1*" %%G IN ('CSCRIPT getmetabaseid.js "%SPServicesWebName%"') DO %systemroot%\system32\IISweb /delete w3svc/%%G
 GOTO end
:appcmd
 %systemroot%\system32\inetsrv\appcmd delete site "%ServicesWebName%"
 %systemroot%\system32\inetsrv\appcmd delete site "%SPServicesWebName%"
:end

ECHO rmdir "%SYSTEMDRIVE%\inetpub\wwwroot\contososervices" /q /s
IF exist "%SYSTEMDRIVE%\inetpub\wwwroot\contososervices\"  GOTO :CONTOSOSERVICESExists
GOTO CONTOSOSERVICESExistsEnd
:CONTOSOSERVICESExists
rmdir "%SYSTEMDRIVE%\inetpub\wwwroot\contososervices" /q /s
:CONTOSOSERVICESExistsEnd