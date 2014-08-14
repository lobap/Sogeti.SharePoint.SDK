 @echo off
 cls
 Set CurrentFolder=%~dp0%
set SourceFolder=%~dp0%\..\..\..\
cd %SourceFolder%
 SET SourceFolder=%cd%\
cd %currentfolder%

REM @rem ----------------------------------------------------------------------
REM @rem    RI ECT UN INSTALLATION
REM @rem ----------------------------------------------------------------------

call SqUnlInstall.bat
call ECTUnInstall.bat
cscript //h:cscript //s

cscript createLocalAccounts.vbs "%ComputerName%" clean
pause
