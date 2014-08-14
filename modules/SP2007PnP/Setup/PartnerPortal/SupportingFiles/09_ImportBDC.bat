call 00_Parameters.bat

call BuildContosoSetupExe.bat

cd "%subfolder%"
ECHO %time%  Status : **** Importing Application Defination File into %ContosoSSP%
call .\ContosoSetup /ssp %ContosoSSP% "%webappurl%" "%SourceFolder%Source\PartnerPortal\ProductCatalogDefinition.xml"

REM **********************************************************************************************
REM ********** Configure BDC access for ContosoWinPartner1 and ContosoWinPartner2      ***********
REM **********************************************************************************************
call "%subfolder%ContosoSetup.exe" /bdcwinaccess "%ContosoSSP%" "%AppPoolIdLocation%"


