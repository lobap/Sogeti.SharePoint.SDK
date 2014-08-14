echo off
cls
ECHO  Client.RI Installation is in Progress.........
===========================================================
set ClientCurrFold=%~dp0%

call  "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86

	CD "%ClientCurrFold%"
	
Rem Compile & Package SharePoint List Solution

ECHO Build Solution in process... > Install.log
ECHO Build Solution in process... 
 call MSBUild.exe "..\Client.sln" /t:build >> Install.log
 	IF %Errorlevel% == 1 goto buildfailed

ECHO Package Solution in Process....
ECHO Package Solution in Process....>> Install.log
 call MSBUild.exe "..\Client.sln" /t:build /p:IsPackaging=true >> Install.log

	IF %Errorlevel% == 1 goto packagefailed

ECHO *************** Install ExecutionModels Proxy RI ***************
 cd ".\..\..\ExecutionModels\Proxy\Setup\"
ECHO ********************************************************************
 call "InstallPart.bat"

 CD "%ClientCurrFold%"
 ECHO *************** Install SharePoint List RI *****************
 cd ".\..\..\DataModels\DataModels.SharePointList\Setup\"
 ECHO *************************************************************
 call "InstallPart.bat"

CD "%ClientCurrFold%"

powershell.exe "& .\createsite.ps1"
CD "%ClientCurrFold%"
ECHO Copy ClientAccessPolicy to 81\Vendor folder
 Copy "ClientAccessPolicy.xml"  "c:\inetpub\wwwroot" /y
ECHO Change Authencation for Vendor Virtual Directory..

%systemroot%\system32\inetsrv\appcmd.exe set config "contoso/vendor" /section:AnonymousAuthentication /enabled:true /commit:apphost
%systemroot%\system32\inetsrv\appcmd.exe set config "contoso/vendor" /section:windowsAuthentication /enabled:true /commit:apphost

%systemroot%\system32\inetsrv\appcmd.exe set config "contoso" /section:windowsAuthentication /enabled:true /commit:apphost

ECHO Update localhost with %computername%
cd ".\..\..\ExecutionModels\
goto end

:buildfailed
  ECHO compile Failed. Please Check %CurrentFolder%\RIinstall.log for more details
  goto end
:packagefailed
  ECHO Creating Package Failed. Please Check %CurrentFolder%\RIinstall.log for more details
:end
	Pause


