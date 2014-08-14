
REM @rem ----------------------------------------------------------------------
REM @rem    Creating Windows Users
REM @rem ----------------------------------------------------------------------

cscript //h:cscript //s

CSCRIPT C:\Inetpub\AdminScripts\ADSUTIL.VBS CREATE w3svc/AppPools/SPGSSSA IIsApplicationPool
CSCRIPT C:\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/AppPools/SPGSSSA/WamUserName %computername%\SecureSvcAppPool
CSCRIPT C:\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/AppPools/SPGSSSA/WamUserPass "P2ssw0rd$"
CSCRIPT c:\Inetpub\AdminScripts\ADSUTIL.VBS SET w3svc/AppPools/SPGSSSA/AppPoolIdentityType 3