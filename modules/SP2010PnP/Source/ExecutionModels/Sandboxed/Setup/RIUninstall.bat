ECHO Uninstall RI.....
	Set CurrentFolder=%~dp0%
	CD "%CurrentFolder%"
	powershell.exe "& .\deletesite.ps1"
pause