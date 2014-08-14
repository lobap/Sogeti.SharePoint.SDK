echo off
cls
ECHO Uninstall FullTrust(Timerjob) RI.....
	Set CurrentFolder=%~dp0%
	CD "%CurrentFolder%"
	powershell.exe "& .\deletesite.ps1"
pause