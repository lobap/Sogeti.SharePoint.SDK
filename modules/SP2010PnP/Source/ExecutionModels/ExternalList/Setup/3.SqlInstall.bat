
REM @rem ----------------------------------------------------------------------
REM @rem    Creating vendor management database
REM @rem ----------------------------------------------------------------------

SQLCMD -E -S %ComputerName%\SharePoint -i .\..\VendorManagement.sql -o Vendormanagement.log

