echo off
cls
ECHO  Sandboxed RI Installation is in progress....
ECHO  ======================================================
Set CurrentFolder=%~dp0%
REM  set vsts command prompt
call  "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86
REM Path=%path%;%windir%\Microsoft.NET\Framework64\v4.0.21006

	CD "%CurrentFolder%"
	REM GAC ServiceLocator
 GACUTIL -if "..\..\..\..\Lib\Microsoft.Practices.ServiceLocation.dll"

Rem Compile & Package Sample Solution

ECHO Build Solution in process... > RIInstall.log
ECHO Build Solution in process... 
 call MSBUild.exe "..\ExecutionModels.Sandboxed.sln" /t:build  >> RIInstall.log

	IF %Errorlevel% == 1 goto buildfailed
ECHO Package Solution in Process....
ECHO Package Solution in Process....>> RIInstall.log
 call MSBUild.exe "..\ExecutionModels.Sandboxed.sln" /t:build /p:IsPackaging=true >>RIInstall.log

	IF %Errorlevel% == 1 goto packagefailed
	
REM ECHO Build PublishData solution
call "..\..\Shared Code\Setup\BuildPublishData.bat"


CD "%CurrentFolder%"


REM call ps1 file

	powershell.exe "& .\createsite.ps1"
goto end

:buildfailed
  ECHO compile Failed. Please Check %CurrentFolder%\RIinstall.log for more details
  goto end
:packagefailed
  ECHO Creating Package Failed. Please Check %CurrentFolder%\RIinstall.log for more details
:end
	Pause


