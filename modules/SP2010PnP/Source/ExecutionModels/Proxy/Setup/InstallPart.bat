

ECHO  Proxy RI Installation is in progress....
ECHO *************************************************
ECHO NOTE : Setup will prompt you to enter Password for Service Account(SandboxSvcAcct), Please enter P2ssw0rd$ (P-two-s-s-w-zero-r-d-Dollar)
PAUSE
ECHO *************************************************
Set CurrentFolder=%~dp0%
set SourceFolder=%~dp0%\..\..\..\
cd %SourceFolder%
 SET SourceFolder=%cd%\

ECHO *************************************************
ECHO Creating Windows user...........................
ECHO *************************************************
	cscript //h:cscript //s
	cd %CurrentFolder%
	
	cscript createLocalAccounts.vbs %ComputerName% setup

ECHO *************************************************
REM create web site for port 81
		%systemroot%\system32\inetsrv\appcmd add site /name:"Contoso" /bindings:http://*:81 /physicalPath:"%SYSTEMDRIVE%\inetpub\wwwroot"

REM Create virtural Dir for Vendor
		%systemroot%\system32\inetsrv\appcmd add app /site.name:"Contoso" /path:"/Vendor" /physicalPath:"%SourceFolder%ExecutionModels\Proxy\Vendor"


	CD "%CurrentFolder%"

Rem Compile & Package Sample Solution



REM  set vsts command prompt
call  "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86

GACUTIL -u Microsoft.Practices.SharePoint.Common

	CD "%CurrentFolder%"
	REM GAC ServiceLocator
 GACUTIL -if "..\..\..\..\Lib\Microsoft.Practices.ServiceLocation.dll"

Rem Compile & Package Sample Solution

ECHO Build Proxy Solution in process... > RIInstall.log
ECHO Build Proxy Solution in process... 
 call MSBUild.exe "..\ExecutionModels.Sandboxed.Proxy.sln" /t:build  >> RIInstall.log

	IF %Errorlevel% == 1 goto buildfailed
ECHO Package Proxy Solution in Process....
ECHO Package Proxy Solution in Process....>> RIInstall.log
 call MSBUild.exe "..\ExecutionModels.Sandboxed.Proxy.sln" /t:build /p:IsPackaging=true >>RIInstall.log

	IF %Errorlevel% == 1 goto packagefailed

	
		mkdir "%SourceFolder%ExecutionModels\Proxy\Vendor\Bin"
		
		copy "%SourceFolder%ExecutionModels\Proxy\Service\Vendor.Services\bin\Debug\*.*"  "%SourceFolder%ExecutionModels\Proxy\Vendor\Bin\" /y


	ECHO Build Sandboxed Solution in process... > RIInstall.log
ECHO Build Solution in process... 
 call MSBUild.exe "..\..\Sandboxed\ExecutionModels.Sandboxed.sln" /t:build  >> RIInstall.log

	IF %Errorlevel% == 1 goto buildfailed
ECHO Package Sandboxed Solution in Process....
ECHO Package Sandboxed Solution in Process....>> RIInstall.log
 call MSBUild.exe "..\..\Sandboxed\ExecutionModels.Sandboxed.sln" /t:build /p:IsPackaging=true >>RIInstall.log

	IF %Errorlevel% == 1 goto packagefailed
	
REM ECHO Build PublishData solution
call "..\..\Shared Code\Setup\BuildPublishData.bat"
IF %Errorlevel% == 1 goto buildfailed

CD "%CurrentFolder%"


REM call ps1 file

	powershell.exe "& .\createsite.ps1"
	CD "%CurrentFolder%"
ECHO Change Authencation for Vendor Virtual Directory..

%systemroot%\system32\inetsrv\appcmd.exe set config "contoso/vendor" /section:AnonymousAuthentication /enabled:true /commit:apphost
%systemroot%\system32\inetsrv\appcmd.exe set config "contoso/vendor" /section:windowsAuthentication /enabled:true /commit:apphost

%systemroot%\system32\inetsrv\appcmd.exe set config "contoso" /section:windowsAuthentication /enabled:true /commit:apphost
goto end

:buildfailed
  ECHO compile Failed. Please Check %CurrentFolder%\RIinstall.log for more details
  goto end
:packagefailed
  ECHO Creating Package Failed. Please Check %CurrentFolder%\RIinstall.log for more details
:end
	


