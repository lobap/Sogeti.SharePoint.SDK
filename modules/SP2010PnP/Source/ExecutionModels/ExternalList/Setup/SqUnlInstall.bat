
REM @rem ----------------------------------------------------------------------
REM @rem    Deleting vendor management database
REM @rem ----------------------------------------------------------------------

SQLCMD -E -S %ComputerName%\SharePoint -i .\..\VendorManagement_Delete.sql -o Vendormanagement_Delete.log

