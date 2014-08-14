echo off
cls

Set CurrentFolder=%~dp0%
REM @rem ----------------------------------------------------------------------
REM @rem    Creating Partsmanagement database
REM @rem ----------------------------------------------------------------------
CD "%CurrentFolder%"
SQLCMD  -E -S %ComputerName%\SharePoint -v restorepath="%CurrentFolder%" -i RestoreDB.sql -o Partsmanagementsql.log

REM @rem ----------------------------------------------------------------------
REM @rem   Add User
REM @rem ----------------------------------------------------------------------


SQLCMD  -E -S %ComputerName%\SharePoint -i AddDboUser.sql -o Partsmanagementsql.log

