call 00_Parameters.bat
@rem ----------------------------------------------------------------------
@rem    Creating Contoso ApplicationPool and Services Web site under IIS
@rem ----------------------------------------------------------------------

REM ******************************************************************************************************************************
ECHO %time%  Status : **** Creating Application Pool:%AppPoolName%
REM ******************************************************************************************************************************

CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS CREATE w3svc/AppPools/%AppPoolName% IIsApplicationPool
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/AppPools/%AppPoolName%/WamUserName %AppPoolServiceLogin%
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/AppPools/%AppPoolName%/WamUserPass %AppPoolLoginPassword%
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/AppPools/%AppPoolName%/AppPoolIdentityType 3

REM ******************************************************************************************************************************
ECHO %time%  Status : **** Creating Application Pool:%SPAppPoolName%
REM ******************************************************************************************************************************

CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS CREATE w3svc/AppPools/%SPAppPoolName% IIsApplicationPool
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/AppPools/%SPAppPoolName%/WamUserName %SPAppPoolServiceLogin%
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/AppPools/%SPAppPoolName%/WamUserPass %SPAppPoolLoginPassword%
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/AppPools/%SPAppPoolName%/AppPoolIdentityType 3


ECHO %time%  Status : **** Creating New Web Site for Contoso Services : http://%ServicesWebName%:8585 ************************** 

ECHO mkdir "%SYSTEMDRIVE%\inetpub\wwwroot\contososervices"
IF exist "%SYSTEMDRIVE%\inetpub\wwwroot\contososervices\"  GOTO :CONTOSOSERVICESExists
cd "%SYSTEMDRIVE%\inetpub\wwwroot\"
mkdir "%SYSTEMDRIVE%\inetpub\wwwroot\contososervices"
:CONTOSOSERVICESExists

cd %subfolder%
IF exist "%systemroot%\system32\inetsrv\appcmd.exe"  GOTO appcmd

:IISweb
	%systemroot%\system32\IISweb /Create %SYSTEMDRIVE%\inetpub\wwwroot\contososervices "%ServicesWebName%" /b 8585
	%systemroot%\system32\IISweb /Create %SYSTEMDRIVE%\inetpub\wwwroot\contososervices "%SPServicesWebName%" /b 8787
	goto afterweb
:appcmd
	%systemroot%\system32\inetsrv\appcmd add site /name:"%ServicesWebName%" /bindings:http://*:8585 /physicalPath:"%SYSTEMDRIVE%\inetpub\wwwroot\contososervices"
	%systemroot%\system32\inetsrv\appcmd add site /name:"%SPServicesWebName%" /bindings:http://*:8787 /physicalPath:"%SYSTEMDRIVE%\inetpub\wwwroot\contososervices"

:afterweb

REM ******************************************************************************************************************************
echo %time%  Status : ****  GET metabase id for "%ServicesWebName% **************************************************************
REM ******************************************************************************************************************************

FOR /F "tokens=1*" %%G IN ('CSCRIPT getmetabaseid.js "%ServicesWebName%"') DO SET ServicesMetaId=%%G

REM ******************************************************************************************************************************
echo %time%  Status : ****  GET metabase id for "%SPServicesWebName% **************************************************************
REM ******************************************************************************************************************************

FOR /F "tokens=1*" %%G IN ('CSCRIPT getmetabaseid.js "%SPServicesWebName%"') DO SET SPServicesMetaId=%%G

REM ******************************************************************************************************************************
echo %time%  Status : ****  Set WWW Service AppPool and Access for "%ServicesWebName% ********************************************
REM ******************************************************************************************************************************

CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%ServicesMetaId%/root/Apppoolid %AppPoolName%
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%ServicesMetaId%/root/AccessExecute True

REM ******************************************************************************************************************************
echo %time%  Status : ****  Set WWW Service AppPool and Access for "%SPServicesWebName% ********************************************
REM ******************************************************************************************************************************

CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%SPServicesMetaId%/root/Apppoolid %SPAppPoolName%
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%SPServicesMetaId%/root/AccessExecute True

REM ******************************************************************************************************************************
echo %time%  Status : ****  Enable Windows authentication on "%ServicesWebName%:8585  ********************************************
REM ******************************************************************************************************************************

CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%ServicesMetaId%/root/authNTLM  true
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%ServicesMetaId%/root/NTAuthenticationProviders "Negotiate,ntlm"

REM ******************************************************************************************************************************
echo %time%  Status : ****  Enable Windows authentication on "%SPServicesWebName%:8787  ********************************************
REM ******************************************************************************************************************************

CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%SPServicesMetaId%/root/authNTLM  true
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%SPServicesMetaId%/root/NTAuthenticationProviders "Negotiate,ntlm"

REM ******************************************************************************************************************************
echo %time%  Status : ****  Create WWW Services and Virtual Directories under "%SPServicesWebName% ***********************************
REM ******************************************************************************************************************************
IF exist "%systemroot%\system32\inetsrv\appcmd.exe"  GOTO appcmd11
:IISweb11
	call %WINDIR%\Microsoft.NET\Framework\v2.0.50727\aspnet_regiis.exe -s W3SVC/%SPServicesMetaId%/root

	ECHO %time%  Status : **** Creating Virtual Directory for Partner Portal Services : http://%SPServicesWebName%:8787/Contoso.PartnerPortal.Services
	%SystemRoot%/System32/IISVdir.vbs /Create "%SPServicesWebName%" "Contoso.PartnerPortal.Services" "%SourceFolder%%ServicesSource%\PartnerPortal\Contoso.PartnerPortal.Services"
	
	GOTO aftersettings1

:appcmd11
	
	
	%systemroot%\system32\inetsrv\appcmd add app /site.name:"%SPServicesWebName%" /path:"/Contoso.PartnerPortal.Services" /physicalPath:"%SourceFolder%%ServicesSource%\PartnerPortal\Contoso.PartnerPortal.Services"
       	
:aftersettings1


REM ******************************************************************************************************************************
echo %time%  Status : ****  Create WWW Services and Virtual Directories under "%ServicesWebName% ***********************************
REM ******************************************************************************************************************************

IF exist "%systemroot%\system32\inetsrv\appcmd.exe"  GOTO appcmd1

:IISweb1
	call %WINDIR%\Microsoft.NET\Framework\v2.0.50727\aspnet_regiis.exe -s W3SVC/%ServicesMetaId%/root


	ECHO %time%  Status : **** Creating Virtual Directory for LOB Services : http://%ServicesWebName%:8585/Contoso.LOB.Services
	%SystemRoot%/System32/IISVdir.vbs /Create "%ServicesWebName%" "Contoso.LOB.Services" "%SourceFolder%%ServicesSource%\PartnerPortal\Contoso.LOB.Services"
	
	ECHO %time%  Status : **** Creating Virtual Directory for LOB Web : http://%ServicesWebName%:8585/Contoso.LOB.Web
	%SystemRoot%/System32/IISVdir.vbs /Create "%ServicesWebName%" "Contoso.LOB.Web" "%SourceFolder%%ServicesSource%\PartnerPortal\Contoso.LOB.Web"
	
	GOTO aftersettings

:appcmd1
	
	%systemroot%\system32\inetsrv\appcmd add app /site.name:"%ServicesWebName%" /path:"/Contoso.LOB.Services" /physicalPath:"%SourceFolder%%ServicesSource%\PartnerPortal\Contoso.LOB.Services"
	%systemroot%\system32\inetsrv\appcmd add app /site.name:"%ServicesWebName%" /path:"/Contoso.LOB.Web" /physicalPath:"%SourceFolder%%ServicesSource%\PartnerPortal\Contoso.LOB.Web"

       	
:aftersettings

REM ******************************************************************************************************************************
echo %time%  Status : ****  Set WWW Services AppPool and Access     **************************************************************
REM ******************************************************************************************************************************

CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%ServicesMetaId%/root/Contoso.LOB.Services/Apppoolid %AppPoolName%
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%ServicesMetaId%/root/Contoso.LOB.Services/AccessExecute True

CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%ServicesMetaId%/root/Contoso.LOB.Web/Apppoolid %AppPoolName%
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%ServicesMetaId%/root/Contoso.LOB.Web/AccessExecute True

CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%SPServicesMetaId%/root/Contoso.PartnerPortal.Services/Apppoolid %SPAppPoolName%
CSCRIPT %SYSTEMDRIVE%\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/%SPServicesMetaId%/root/Contoso.PartnerPortal.Services/AccessExecute True

REM ******************************************************************************************************************************
echo set path to execute VC commands
REM ******************************************************************************************************************************

REM call setEvn.bat

REM ******************************************************************************************************************************
ECHO %time%  Status : **** Enabling Service Model 
REM ******************************************************************************************************************************

  "%systemroot%\Microsoft.NET\Framework\v3.0\Windows Communication Foundation\ServiceModelReg.exe" -I 


REM iisreset -noforce
