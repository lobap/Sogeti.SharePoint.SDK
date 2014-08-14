 @echo off
 cls
 Set CurrentFolder=%~dp0%
 REM  set vsts command prompt
call  "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86
set SourceFolder=%~dp0%\..\..\..\
cd %SourceFolder%
 SET SourceFolder=%cd%\
cd %currentfolder%

REM @rem ----------------------------------------------------------------------
REM @rem    RI ECT INSTALLATION
REM @rem ----------------------------------------------------------------------


ECHO Build Solution .........
CALL msbuild.EXE ".\..\ExecutionModels.Sandbox.ExternalList.sln" /t:build >> Install.log
	IF %Errorlevel% == 1 goto buildfailed
ECHO Package Solution .........
CALL msbuild.EXE ".\..\ExecutionModels.Sandbox.ExternalList.sln" /p:IsPackaging=true >> Install.log
 	IF %Errorlevel% == 1 goto packagefailed

call 1.CreateWindowsUsers.bat
call 2.CreateAppPool.bat
call 3.SqlInstall.bat
call 4.ECTInstall.bat


	goto EndInstall
:buildfailed
	ECHO Build Failed. Please Check %CurrentFolder%\SPGLIBInstall.log for more details.
	goto EndInstall
:packagefailed
	ECHO Package Failed. Please Check %CurrentFolder%\SPGLIBInstall.log for more details.
:EndInstall
 ECHO Please follow Manual steps from Readme.txt to complete ECT installation.
pause
