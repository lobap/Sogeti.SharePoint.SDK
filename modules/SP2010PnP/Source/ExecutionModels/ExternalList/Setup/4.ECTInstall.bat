
ECHO  ECT Installation is in progress....
ECHO ---------------------------------------------------------------------
Set CurrentFolder=%~dp0%
REM  set vsts command prompt
call  "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86
REM Path=%path%;%windir%\Microsoft.NET\Framework64\v4.0.21006

	CD "%CurrentFolder%"
	
REM call ps1 file

	powershell.exe "& .\ECTCreation.ps1"



