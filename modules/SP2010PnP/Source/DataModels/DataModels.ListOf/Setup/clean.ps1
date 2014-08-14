


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


		
WriteHeading "Remove $Hybrid2ActPackage Package to FarmSolutions"

						$checkSolution=Get-SPSolution -Identity $Hybrid2ActPackage
						if ($checkSolution -ne $null)
						{
							if($checkSolution.deployed -eq $True)
							{
								WriteHeading "Uninstall $Hybrid2ActPackage"
								Uninstall-SPSolution -Identity $Hybrid2ActPackage -Confirm:$false -Local:$true
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							}
							
							WriteHeading "Remove $Hybrid2ActPackage from form"	
							REmove-SPSolution -Identity $Hybrid2ActPackage -force:$true -Confirm:$false
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
						}
						$error.clear()
                       


				
								
$error.clear()
$settings.Sites.Site | ForEach-Object {
		$RISite=$_
		
				#check for any exists with that name:
					$newsite ="http://"+ $HostName+":"+$WebApp.Port+"/sites/" +  $RISite.Name
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
		
}	    		
	

	



