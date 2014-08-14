
@rem ----------------------------------------------------------------------
@rem    Creating Windows Users
@rem ----------------------------------------------------------------------

cscript //h:cscript //s

call 00_parameters.bat

cscript createLocalAccounts.vbs clean %computername% %Password%