


# Set up the environment
. .\initialize.ps1

# Set SharePoint Environment
 . "C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\CONFIG\POWERSHELL\Registration\\sharepoint.ps1"
 

cd $currentDirectory
$centralAdminPort=[Microsoft.SharePoint.Administration.SPAdministrationWebApplication]::Local.Sites.VirtualServer.Port

[System.Xml.XmlElement]$settings = Get-Settings

if ($settings -eq $null) {
	return $false
}
$error.clear()
$WebApp=$settings.WebApp

$checkSolution=Get-SPSolution -Identity $FarmSolJobsPackage
						if ($checkSolution -ne $null)
						{
							if($checkSolution.deployed -eq $True)
							{
								$webAppURL="http://"+$HostName+":" + $WebApp.Port
							   WriteHeading "Deactivate Job Feature"
								Disable-SPfeature -Identity $ApprovedEstJobFID -Force:$true -URL $webAppURL -Confirm:$false
								
								WriteHeading "Uninstall $FarmSolJobsPackage"
								Uninstall-SPSolution -Identity $FarmSolJobsPackage -Confirm:$false -Local:$true
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							}
							
							WriteHeading "Remove $FarmSolJobsPackage from farm"	
							REmove-SPSolution -Identity $FarmSolJobsPackage -force:$true -Confirm:$false
							if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
						}
						$error.clear()
						$checkSolution=Get-SPSolution -Identity $FarmSolPackage
						if ($checkSolution -ne $null)
						{
							
							 WriteHeading "Deactivate :AdminFormsNavFID : $AdminFormsNavFID"
							 $CentAdmURL="http://"+$HostName+":" + $centralAdminPort
								Disable-SPfeature -Identity $AdminFormsNavFID -Force:$true -URL $CentAdmURL -Confirm:$false
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							
							if($checkSolution.deployed -eq $True)
							{
								WriteHeading "Uninstall $FarmSolPackage"
								Uninstall-SPSolution -Identity $FarmSolPackage -Confirm:$false -Local:$true
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							}
							
							WriteHeading "Remove $FarmSolPackage from farm"	
							REmove-SPSolution -Identity $FarmSolPackage -force:$true -Confirm:$false
							if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
						}
$settings.Sites.Site | ForEach-Object {
		$RISite=$_
		$newsite ="http://"+ $HostName+":"+$WebApp.Port+ "/sites/"+$RIsite.Name
	    if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}

				
				$error.clear()

				
				
			
			#check for any exists with that name:
				$error.clear()
				$site = get-spsite -identity $newsite
				
				if ($site -ne $null)
				{
					WriteHeading "Remove existing $newsite.."
					remove-spsite -identity $newsite -confirm:$false
					if ($error) 
						{
							writeError $error[0].Exception 
							break;
						}
				}
		
		}



	



