ECHO OFF
cls
ECHO Adding Microsoft.Practices.ServiceLocation to GAC
Set CurrentFolder=%~dp0%
call  "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86
cd %currentFolder%

GACUTIL -if Microsoft.Practices.ServiceLocation.dll

Pause