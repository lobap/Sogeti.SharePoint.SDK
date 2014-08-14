REM @echo off
REM cls
REM setlocal
REM pushd .
REM @rem ----------------------------------------------------------------------
REM @rem    Creating Windows Users
REM @rem ----------------------------------------------------------------------

cscript //h:cscript //s

call 00_parameters.bat

cscript createLocalAccounts.vbs create %computername% %Password%