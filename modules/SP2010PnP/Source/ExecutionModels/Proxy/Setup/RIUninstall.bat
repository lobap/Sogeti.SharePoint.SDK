ECHO off
cls

ECHO Uninstall Proxy RI.....
	Set CurrentFolder=%~dp0%
	CD "%CurrentFolder%"
	powershell.exe "& .\deletesite.ps1"

	ECHO Delete Contoso WebSite from IIS
	%systemroot%\system32\inetsrv\appcmd delete site "Contoso"
	CD "%CurrentFolder%"
	cscript //h:cscript //s
	cd %CurrentFolder%
	
	cscript createLocalAccounts.vbs %ComputerName% clean 

pause