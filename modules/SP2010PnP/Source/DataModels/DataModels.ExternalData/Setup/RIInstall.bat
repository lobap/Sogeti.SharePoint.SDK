echo off
cls
ECHO  DataModels:ExternalData Installation is in progress....
ECHO ---------------------------------------------------------------------
Set CurrentFolder=%~dp0%
REM  set vsts command prompt
call  "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86
CD "%CurrentFolder%"

	
ECHO Build Solution .........
CALL msbuild.EXE ".\..\DataModels.ExternalData.sln" /t:build > Install.log
	IF %Errorlevel% == 1 goto buildfailed
ECHO Package Solution .........
CALL msbuild.EXE ".\..\DataModels.ExternalData.sln" /p:IsPackaging=true >> Install.log
 	IF %Errorlevel% == 1 goto packagefailed

ECHO Creating PartsManagement Database in %computername%\sharepoint instance:
	call PartsManagement_SqlInstall.bat




	PowerShell.exe -command "Set-ExecutionPolicy Unrestricted "
	PowerShell.exe -command ".\DMCreation.ps1" 

	goto EndInstall
:buildfailed
	ECHO Build Failed. Please Check %CurrentFolder%\SPGLIBInstall.log for more details.
	goto EndInstall
:packagefailed
	ECHO Package Failed. Please Check %CurrentFolder%\SPGLIBInstall.log for more details.
:EndInstall
PAUSE





