echo off
cls
ECHO Uninstall Client RI...
	Set CurrentFolder=%~dp0%
	CD "%CurrentFolder%"

		powershell.exe "& .\clean.ps1"
	
pause