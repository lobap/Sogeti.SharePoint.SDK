call 00_Parameters.bat

REM ****************************************************************************
REM Create ProductCatalog Site Collection 
REM ****************************************************************************

Echo Create %productcatalogurl%
        "%SPAdminTool%" -o Createsite -url %productcatalogurl% -owneremail %owneremail% -ownerlogin %ownerlogin% -description "Contoso Product Catalog" -sitetemplate STS#1

Echo Activate features on ProductCatalog 
    echo Activating feature ContosoProductCatalogSite ...
    "%SPAdminTool%" -o activatefeature -id %ContosoProductCatalogSite_FeatureId% -url %productcatalogurl%

	echo Activating feature ContosoProductCatalogWeb ...
    "%SPAdminTool%" -o activatefeature -id %ContosoProductCatalogWeb_FeatureId% -url %productcatalogurl%

	echo Activating feature ContosoTheme ...
    "%SPAdminTool%" -o activatefeature -id %ContosoTheme_FeatureId% -url %productcatalogurl%
    
   	echo Activating feature ContosoContextualHelp ...
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %productcatalogurl%

	echo Activating feature ContosoPSSGlobalNav ...
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNav_FeatureId% -url %productcatalogurl%

REM ****************************************************************************
Echo Create Promotions Site Collection 
REM ****************************************************************************

IF "%ContentDeployment%"=="Authoring" GOTO Authoring
IF "%ContentDeployment%"=="Targeting" GOTO Targeting

REM the default is Authroing
:Authoring

    echo "%SPAdminTool%" -o Createsite -url %promotionsurl% -owneremail %owneremail% -ownerlogin %ownerlogin% -description "Contoso Promotions" -sitetemplate STS#1
         "%SPAdminTool%" -o Createsite -url %promotionsurl% -owneremail %owneremail% -ownerlogin %ownerlogin% -description "Contoso Promotions" -sitetemplate STS#1

    Echo Activate features on Promotions =============================================

 	echo Activating feature PublishingSite ...
    "%SPAdminTool%" -o activatefeature -id F6924D36-2FA8-4f0b-B16D-06B7250180FA -url %promotionsurl%

 	echo Activating feature PublishingWeb ...
    "%SPAdminTool%" -o activatefeature -id 94c94ca6-b32f-4da9-a9e3-1f3d343d7ecb -url %promotionsurl%

 	echo Activating feature ContosoTheme ...
    "%SPAdminTool%" -o activatefeature -id %ContosoTheme_FeatureId% -url %promotionsurl%

	echo Activating feature ContosoContextualHelp ...
    "%SPAdminTool%" -o activatefeature -id %ContosoContextualHelp_FeatureId% -url %promotionsurl%

	echo Activating feature ContosoPSSGlobalNav ...
    "%SPAdminTool%" -o activatefeature -id %ContosoPSSGlobalNav_FeatureId% -url %promotionsurl%
    
    echo Activating feature ContosoPromotionsSite ...
    "%SPAdminTool%" -o activatefeature -id %ContosoPromotionsSite_FeatureId% -url %promotionsurl%
    
    echo Activating feature ContosoPromotionWeb ...
    "%SPAdminTool%" -o activatefeature -id %ContosoPromotionWeb_FeatureId% -url %promotionsurl%

	goto END

:Targeting

    REM for the target farm, do not specify -sitetemplate when create the site collection, and do not activate any features
	echo "%SPAdminTool%" -o Createsite -url %promotionsurl% -owneremail %owneremail% -ownerlogin %ownerlogin% -description "Contoso Promotions" 
	     "%SPAdminTool%" -o Createsite -url %promotionsurl% -owneremail %owneremail% -ownerlogin %ownerlogin% -description "Contoso Promotions" 

	goto END

:END