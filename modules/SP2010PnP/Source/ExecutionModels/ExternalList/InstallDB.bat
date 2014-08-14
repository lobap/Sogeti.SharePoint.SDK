ECHO OFF
CLs
ECHO Script is going to Install Vendor Management DataBase under %computerName%\SharePoint Instance.
pause
CD %~dp0%
SQLCMD -S %computerName%\SharePoint -E -i VendorManagement.sql

PAUSE