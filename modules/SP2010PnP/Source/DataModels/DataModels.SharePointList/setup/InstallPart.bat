ECHO  ======================================================
ECHO  SharePoint List Installation is in progress....
ECHO  ======================================================
Set CurrentFolder=%~dp0%
REM  set vsts command prompt
call  "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86
REM Path=%path%;%windir%\Microsoft.NET\Framework64\v4.0.21006

	CD "%CurrentFolder%"
	
Rem Compile & Package SharePoint List Solution

ECHO Build Solution in process... > Install.log
ECHO Build Solution in process... 
 call MSBUild.exe "..\DataModels.SharePointList.sln" /t:build  >> Install.log

	IF %Errorlevel% == 1 goto buildfailed
ECHO Package Solution in Process....
ECHO Package Solution in Process....>> Install.log
 call MSBUild.exe "..\DataModels.SharePointList.sln" /t:build /p:IsPackaging=true >>Install.log

	IF %Errorlevel% == 1 goto packagefailed
	

CD "%CurrentFolder%"

	powershell.exe "& .\createsite.ps1"

goto end

:buildfailed
  ECHO compile Failed. Please Check %CurrentFolder%\RIinstall.log for more details
  goto end
:packagefailed
  ECHO Creating Package Failed. Please Check %CurrentFolder%\RIinstall.log for more details
:end
	


