set stscmd=C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\12\BIN\stsadm.exe

REM ****************************Partner Portal Root*****************************************************************************************************************************
"%stscmd%" -o deletesite -url http://localhost:9001
"%stscmd%" -o createsite -url http://localhost:9001 -owneremail v-chacho@microsoft.com -ownerlogin redmond\v-chacho -title "Contoso Partner Portal" -sitetemplate sts#1

REM **********************Partner 1 Site COllection ********************************************************************************************************************************
"%stscmd%" -o deletesite -url http://localhost:9001/sites/partner1
"%stscmd%" -o createsite -url http://localhost:9001/sites/partner1 -owneremail v-chacho@microsoft.com -ownerlogin redmond\v-chacho -title "Partner 1" -sitetemplate sts#1
"%stscmd%" -o activatefeature -id F6924D36-2FA8-4f0b-B16D-06B7250180FA -url http://localhost:9001/sites/partner1
"%stscmd%" -o activatefeature -id 94C94CA6-B32F-4da9-A9E3-1F3D343D7ECB -url http://localhost:9001/sites/partner1
	
"%stscmd%" -o createweb -url http://localhost:9001/sites/partner1/incidents -sitetemplate sts#1 -title Incidents
"%stscmd%" -o createweb -url http://localhost:9001/sites/partner1/orderexceptions -sitetemplate sts#1 -title "Order Exceptions"

REM **************************Promotions *************************************************************************************************************************************************
"%stscmd%" -o deletesite -url http://localhost:9001/sites/promotions
"%stscmd%" -o createsite -url http://localhost:9001/sites/promotions -owneremail v-chacho@microsoft.com -ownerlogin redmond\v-chacho -title "Contoso Partner Promotions" -sitetemplate sts#1
"%stscmd%" -o activatefeature -id F6924D36-2FA8-4f0b-B16D-06B7250180FA -url http://localhost:9001/sites/promotions
"%stscmd%" -o activatefeature -id 94C94CA6-B32F-4da9-A9E3-1F3D343D7ECB -url http://localhost:9001/sites/promotions

REM **************************Product Catalog**********************************************************************************************************************************************
"%stscmd%" -o deletesite -url http://localhost:9001/sites/productcatalog
"%stscmd%" -o createsite -url http://localhost:9001/sites/productcatalog -owneremail v-chacho@microsoft.com -ownerlogin redmond\v-chacho -title "Contoso Product Catalog" -sitetemplate sts#1
"%stscmd%" -o activatefeature -id 8581A8A7-CF16-4770-AC54-260265DDB0B2 -url http://localhost:9001/sites/productcatalog
"%stscmd%" -o activatefeature -id 0806D127-06E6-447a-980E-2E90B03101B8 -url http://localhost:9001/sites/productcatalog


REM **************************Partner Central*************************************************************************************************************************************
"%stscmd%" -o deletesite -url http://localhost:9001/sites/partnercentral
"%stscmd%" -o createsite -url http://localhost:9001/sites/partnercentral -owneremail v-chacho@microsoft.com -ownerlogin redmond\v-chacho -title "Contoso Partner Central" -sitetemplate sts#1
"%stscmd%" -o activatefeature -id F6924D36-2FA8-4f0b-B16D-06B7250180FA -url http://localhost:9001/sites/partnercentral
"%stscmd%" -o activatefeature -id 94C94CA6-B32F-4da9-A9E3-1F3D343D7ECB -url http://localhost:9001/sites/partnercentral
"%stscmd%" -o activatefeature -id 8581A8A7-CF16-4770-AC54-260265DDB0B2 -url http://localhost:9001/sites/partnercentral
"%stscmd%" -o activatefeature -id 0806D127-06E6-447a-980E-2E90B03101B8 -url http://localhost:9001/sites/partnercentral

"%stscmd%" -o createweb -url http://localhost:9001/sites/partnercentral/spgsubsite -sitetemplate sts#1 -title "Sub Site Creation"
"%stscmd%" -o createweb -url http://localhost:9001/sites/partnercentral/partnerdirectory -sitetemplate SPSSITES#0 -title "Partner Site Directory"