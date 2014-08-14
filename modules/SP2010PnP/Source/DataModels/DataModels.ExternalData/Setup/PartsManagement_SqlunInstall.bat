
REM @rem ----------------------------------------------------------------------
REM @rem    Drop Partsmanagement database
REM @rem ----------------------------------------------------------------------

SQLCMD -E -S %ComputerName%\SharePoint -i DropPartsManagement.sql -oDropPartsManagement.log

