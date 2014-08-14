


# Set up the environment
. .\initialize.ps1

# Set SharePoint Environment
 . "C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\CONFIG\POWERSHELL\Registration\\sharepoint.ps1"
 

cd $currentDirectory

[System.Xml.XmlElement]$settings = Get-Settings

if ($settings -eq $null) {
	return $false
}
$error.clear()

$WebApp=$settings.WebApp

$settings.Sites.Site | ForEach-Object {
		$RISite=$_
		$newsite ="http://"+ $HostName+":"+$WebApp.Port+"/sites/"+ $RIsite.Name
		if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}

					#Remove solution to solution gallary


				#check of if any existing with that name

				$exitpackage= get-SPUserSolution -Identity $SandRIPackage -site $newsite


				if ($exitpackage -ne $null)
				{	
					WriteHeading "Remove Existing Package."
					$error.clear()
					Uninstall-SPUserSolution -Identity $SandRIPackage -Site $newsite -Confirm:$false

					if ($error) 
						{
							writeError $error[0].Exception 
							break;
						}
					Remove-SPUserSolution -Identity $SandRIPackage -site $newsite -Confirm:$false
					if ($error) 
						{
							writeError $error[0].Exception 
							break;
						}

				}


				
			
			#check for any exists with that name:
				
				$site = get-spsite -identity $newsite
				
				if ($site -ne $null)
				{
					WriteHeading "Remove existing $newsite.."
					$error.clear()
					remove-spsite -identity $newsite -confirm:$false
					if ($error) 
						{
							writeError $error[0].Exception 
							break;
						}

				}
		
		}



	



