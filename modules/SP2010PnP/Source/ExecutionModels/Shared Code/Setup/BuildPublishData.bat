echo off

Set tempFolder=%~dp0%
REM  set vsts command prompt
call  "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86


	CD "%tempFolder%"

Rem Compile  Solution

ECHO Build PublishData Solution in process... > Build.log
ECHO Build PublishData Solution in process... 
 call MSBUild.exe ".\PublishDataSources\PublishData.sln" /t:build  >> RIInstall.log

	IF %Errorlevel% == 1 goto buildfailed


goto end

:buildfailed
  ECHO compile Failed. Please Check %CurrentFolder%\RIinstall.log for more details
  goto end

:end



