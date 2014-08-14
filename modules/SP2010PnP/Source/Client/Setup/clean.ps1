


# Set up the environment
. .\initialize.ps1

# Set SharePoint Environment
 . "C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\CONFIG\POWERSHELL\Registration\\sharepoint.ps1"
 

cd $currentDirectory

[System.Xml.XmlElement]$settings = Get-Settings

if ($settings -eq $null) {
	return $false
}

$WebApp=$settings.WebApp
$RISite=$settings.Site
$RIsiteName  =$RISite.Name

#Check  Normal web and Central Admin are running
	$siteURL="http://"+$HostName+":" + $WebApp.Port
	WriteHeading "Connecting to $siteURL"
	$web=Get-SPWeb -site $siteURL
	if ($web -eq $null)
	{
		WriteError "Unable to Connect $siteURL. Please check settings.xml for Web Application Port information."
		break;
	}


		
WriteHeading "Remove $ClientRIPackage Package to FarmSolutions"
$newsite="http://"+$HostName+":" + $WebApp.Port+"/sites/"+ $RISiteName
						$checkSolution=Get-SPSolution -Identity $ClientRIPackage
						if ($checkSolution -ne $null)
						{
							if($checkSolution.deployed -eq $True)
							{
								
										WriteHeading "Deactivate Feature :Patterns and Practices SharePoint Guidance V3 - Client -Libraries"
										Disable-SPfeature -Identity $LibrariesFID -Force:$true -URL $newsite -Confirm:$false
											
							
										WriteHeading "DeActivate Feature :Patterns and Practices SharePoint Guidance V3 - Client -Pages"
										Disable-SPfeature -Identity $PagesFID -Force:$true -URL $newsite -Confirm:$false
											
							
										WriteHeading "DeActivate Feature :Patterns and Practices SharePoint Guidance V3 - Client -Silverlight Apps"
										Disable-SPfeature -Identity $SilverAppsFID -Force:$true -URL $newsite -Confirm:$false
											
										WriteHeading "DeActivate Feature :Patterns and Practices SharePoint Guidance V3 - Client -Javascript Files"
										Disable-SPfeature -Identity $JavaFilesFID -Force:$true -URL $newsite -Confirm:$false
											
	
								WriteHeading "Uninstall $ClientRIPackage"
								Uninstall-SPSolution -Identity $ClientRIPackage -Confirm:$false -Local:$true
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							}
							
							WriteHeading "Remove $ClientRIPackage from form"	
							REmove-SPSolution -Identity $ClientRIPackage -force:$true -Confirm:$false
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
						}
						$error.clear()
                       


				
								
$error.clear()

					$site = get-spweb -identity $newsite
					
					if ($site -ne $null)
						{
							WriteHeading "Removing existing $newsite.."
							remove-spweb -identity $newsite -confirm:$false
							 if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}

						}
		

	

	



