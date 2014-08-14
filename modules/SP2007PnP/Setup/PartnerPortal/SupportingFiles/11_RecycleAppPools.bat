call 00_Parameters.bat

REM ************************************************************
REM ********** Recycle AppPool *********************************
REM ************************************************************
IF exist "%systemroot%\system32\inetsrv\appcmd.exe"  GOTO appcmd
	 iisapp /a "SharePoint Central Administration v3" /r
	 iisapp /a "%ContosoWEBAppPool%" /r
	 iisapp /a "%ContosoSSPAppPool%" /r
	 iisapp /a "%AppPoolName%" /r
GOTO end
:appcmd
 "%systemroot%\system32\inetsrv\appcmd.exe" recycle apppool /apppool.name:"SharePoint Central Administration v3"
 "%systemroot%\system32\inetsrv\appcmd.exe" recycle apppool /apppool.name:"%ContosoWEBAppPool%"
 "%systemroot%\system32\inetsrv\appcmd.exe" recycle apppool /apppool.name:"%ContosoSSPAppPool%"
 "%systemroot%\system32\inetsrv\appcmd.exe" recycle apppool /apppool.name:"%AppPoolName%"
:end