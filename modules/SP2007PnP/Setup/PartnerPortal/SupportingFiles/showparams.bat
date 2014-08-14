@echo off
echo install will use these parameters
echo.
echo -----------------------------------------------------------------
echo.
echo SharedService Name			          %ContosoSSP%
echo Contoso application name:    	  %webappurl%
echo Windows users password:              %Password%
echo.
echo Path to stsadm:                      %SPAdminTool%
echo.
echo SQLServer Name:                      %SQLserverName%
echo ContosoWeb Url:                      %webappurl%
echo owner login:                         %ownerLogin%
echo owner email:                         %owneremail%
echo ContosoServicesAppPool name:         %AppPoolServiceLogin%
echo ******* NOTE: If Current Server is Active Directory Server then AppPool login should be %USERDOMAIN%\contososerviceuser 
echo ******* Please Edit 00_Parameters.bat if required.
echo ContosoPublishing url                %promotionsurl%
echo Partner1  url			  %webappurl%/sites/partner1
echo Partner2  url			  %webappurl%/sites/partner2

echo SourceFolder			  %SourceFolder%
echo -----------------------------------------------------------------
echo you can edit all above parameters in SupportingFiles\00_Parameters.bat using notepad
echo CTRL + C To quit. Or 
echo.
pause
