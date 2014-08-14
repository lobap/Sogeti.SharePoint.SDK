


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

#Check  Normal web and Central Admin are running
	$siteURL="http://"+$HostName+":" + $WebApp.Port
	WriteHeading "Connecting to $siteURL"
	$web=Get-SPWeb -site $siteURL
	if ($web -eq $null)
	{
		WriteError "Unable to Connect $siteURL. Please check settings.xml for Web Application Port information."
		break;
	}


$error.clear()

WriteHeading "Remove(if already exists) new site Collection: " + $RIsiteName
		#check for any exists with that name:
			$newsite ="http://"+ $HostName+":"+$WebApp.Port+"/sites/" + $RIsiteName
			 if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}
			$site = get-spsite -identity $newsite
			
			if ($site -ne $null)
				{
					WriteHeading "Removing existing $newsite.."
					remove-spsite -identity $newsite -confirm:$false
					 if ($error) 
						{
							writeError $error[0].Exception 
							break;
						}

				}
		
	    		
			
WriteHeading "Remove $listdatapkg Package to FarmSolutions"

						$checkSolution=Get-SPSolution -Identity $listdatapkg
						if ($checkSolution -ne $null)
						{
							if($checkSolution.deployed -eq $True)
							{
								WriteHeading "Uninstall $listdatapkg"
								Uninstall-SPSolution -Identity $listdatapkg -Confirm:$false -Local:$true
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							}
							
							WriteHeading "Remove $listdatapkg from form"	
							REmove-SPSolution -Identity $listdatapkg -force:$true -Confirm:$false
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
						}
						$error.clear()
                       

WriteHeading "Remove $partsmngpkg Pacakge to FarmSolutions"
		$checkSolution=Get-SPSolution -Identity $partsmngpkg
						if ($checkSolution -ne $null)
						{
							if($checkSolution.deployed -eq $True)
							{
								WriteHeading "Uninstall $partsmngpkg"
								Uninstall-SPSolution -Identity $partsmngpkg -Confirm:$false -Local:$true -AllWebApplications:$true
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							}
							
							WriteHeading "Remove $partsmngpkg from form"	
							REmove-SPSolution -Identity $partsmngpkg -force:$true -Confirm:$false
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
						}
								


	



