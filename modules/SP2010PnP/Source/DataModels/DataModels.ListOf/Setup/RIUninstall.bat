echo off
cls
ECHO Uninstall ListOf Data Model...
	Set CurrentFolder=%~dp0%
	CD "%CurrentFolder%"

		powershell.exe "& .\clean.ps1"
	
pause