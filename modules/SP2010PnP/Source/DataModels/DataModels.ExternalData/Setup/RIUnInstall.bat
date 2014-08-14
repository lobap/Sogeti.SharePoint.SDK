

echo off
cls
ECHO   DataModels:ExternalData Uninstallation is in progress....
ECHO ---------------------------------------------------------------------
Set CurrentFolder=%~dp0%
REM  set vsts command prompt
call  "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86
REM Path=%path%;%windir%\Microsoft.NET\Framework64\v4.0.21006

	CD "%CurrentFolder%"

ECHO Uninstall in process....

C:\Windows\System32\WindowsPowerShell\v1.0\PowerShell.exe -command "Set-ExecutionPolicy Unrestricted "
C:\Windows\System32\WindowsPowerShell\v1.0\PowerShell.exe -command "& 'C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\CONFIG\POWERSHELL\Registration\\sharepoint.ps1'"
cd %CurrentFolder%
C:\Windows\System32\WindowsPowerShell\v1.0\PowerShell.exe -command ".\Clean.ps1" 

ECHO Uninstall PartsManagement Database..

call PartsManagement_SqlunInstall.bat

pause
