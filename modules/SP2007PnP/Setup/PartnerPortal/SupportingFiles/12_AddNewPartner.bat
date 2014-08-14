    REM @echo off
    REM cls
    REM setlocal
    REM pushd .

    cscript //h:cscript //s

    REM ============================================================================
    REM ==== the first parameter is the partner number

	if "%1" == "" (
		echo Invalid argument.
		goto End
	)

    REM ============================================================================
    REM Get global Parameters

    cd "%subfolder%"
	call 00_parameters.bat

    REM ============================================================================
    REM Local Parameters to use in this batch file

	set N="%1"
	set PartnerN=Partner%N%
    set PartnerNUrl=%webappurl%/sites/partner%N%
	set ContosoWinPartnerN=ContosoWinPartner%N%
	set ContosoFbaPartnerN=ContosoFbaPartner%N%

    REM ============================================================================
    REM ==== Create Partner Site Collections 

    "%SPAdminTool%" -o Createsite -url %PartnerNUrl% -owneremail %owneremail% -ownerlogin %ownerlogin% -sitetemplate STS#0 -description "%PartnerN%"
    "%SPAdminTool%" -o Createweb -url %PartnerNUrl%/incidents -sitetemplate STS#1 -title "Incidents"
    "%SPAdminTool%" -o Createweb -url %PartnerNUrl%/orderexceptions -sitetemplate STS#1 -title "Order Exceptions"

    REM ============================================================================
    REM ==== Activate Partner Site Features 

	echo Activating feature PublishingSite ...
    "%SPAdminTool%" -o activatefeature -id F6924D36-2FA8-4f0b-B16D-06B7250180FA -url %PartnerNUrl%

	echo Activating feature PublishingWeb ...
    "%SPAdminTool%" -o activatefeature -id 94C94CA6-B32F-4da9-A9E3-1F3D343D7ECB -url %PartnerNUrl%

	echo Activating feature IncidentSubSiteResources 
    "%SPAdminTool%" -o activatefeature -id %IncidentSubSiteResources_FeatureId% -url %PartnerNUrl%

	echo Activating feature IncidentDashboard
    "%SPAdminTool%" -o activatefeature -id %IncidentDashboard_FeatureId% -url %PartnerNUrl%/incidents

	echo Activating feature PartnerPromotionsWebPart ...
    "%SPAdminTool%" -o activatefeature -id %PartnerPromotionsWebPart_FeatureId% -url %PartnerNUrl%
    
    echo Activating feature ContosoPSSOrderExceptionSite
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSOrderExceptionSite_FeatureId% -url %PartnerNUrl%

    REM ============================================================================
    REM ==== Create ContosoWinPartnerN group and users in local Windows or AD

    cd "%subfolder%"
    
REM TODO: modify createLocalAccounts.vbs to accept thrid parameter partnerId
	cscript createLocalAccounts.vbs create %computername% %Password% %partnerId%
	
    REM ============================================================================
    REM ==== Create ContosoFBAPartnerN role and members in AspNetMemberDB

	cd %subfolder%
REM TODO: Create AddContosoFBAPartnerN.sql to populate AspNetMemberDB
    SQLCMD -E -S "%SQLServerName%" -i .\AddContosoFBAPartnerN.sql
                       
    REM ============================================================================
    REM ==== Make sure ContosoSetup.Exe is built

    cd "%subfolder%"
    call BuildContosoSetupExe.bat

    REM ============================================================================
    REM ==== Add Partner Info into Partner Central

    cd "%subfolder%"

REM TODO: Modify contosoSetup.exe to take parameter partnerId
    call %subfolder%\ContosoSetup.exe /testdata %SpgSubsiteParent% %webappurl% http://%LoadBanlancedHost%:%ExtranetPort% %partnerId%

    REM ============================================================================
    REM ==== add PartnerN permissions to BDC

    cd "%subfolder%"

REM TODO: Modify contosoSetup.exe to take parameter partnerId
    call "%subfolder%ContosoSetup.exe" /bdcfbaaccess "%ContosoSSP%" %partnerId%

    REM ============================================================================
    REM ==== add PartnerN users permissions

    "%SPAdminTool%" -o adduser  -url %PartnerNUrl% -userlogin %ContosoWinPartnerN% -useremail partner1@SPG.com -role Design -username %ContosoWinPartnerN%
    "%SPAdminTool%" -o adduser  -url %webappurl%   -userlogin %ContosoWinPartnerN% -useremail partner1@SPG.com -role Design -username %ContosoWinPartnerN%
    "%SPAdminTool%" -o adduser  -url %promotionsurl%   -userlogin %ContosoWinPartnerN% -useremail partner1@SPG.com -role Design -username %ContosoWinPartnerN%
    "%SPAdminTool%" -o adduser  -url %productcatalogurl%   -userlogin %ContosoWinPartnerN% -useremail partner1@SPG.com -role Design -username %ContosoWinPartnerN%
    "%SPAdminTool%" -o adduser  -url %adminurl%    -userlogin %ContosoWinPartnerN% -useremail partner2@SPG.com -role Read   -username %ContosoWinPartnerN%

    "%SPAdminTool%" -o adduser  -url %PartnerNUrl% -userlogin PartnerGroups:%ContosoFbaPartnerN% -useremail partner1@SPG.com -role Design -username %ContosoFbaPartnerN%
    "%SPAdminTool%" -o adduser  -url %webappurl%   -userlogin PartnerGroups:%ContosoFbaPartnerN% -useremail partner1@SPG.com -role Design -username %ContosoFbaPartnerN%
    "%SPAdminTool%" -o adduser  -url %promotionsurl%   -userlogin PartnerGroups:%ContosoFbaPartnerN% -useremail partner1@SPG.com -role Design -username %ContosoFbaPartnerN%
    "%SPAdminTool%" -o adduser  -url %productcatalogurl%   -userlogin %ContosoWinPartnerN% -useremail partner1@SPG.com -role Design -username %ContosoWinPartnerN%
    "%SPAdminTool%" -o adduser  -url %adminurl%    -userlogin PartnerGroups:%ContosoFbaPartnerN% -useremail partner1@SPG.com -role Read   -username %ContosoFbaPartnerN%

    REM ============================================================================
:End