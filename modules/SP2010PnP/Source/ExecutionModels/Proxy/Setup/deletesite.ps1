
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
	WriteHeading "Update Microsoft SharePoint Foundation Sandboxed Code Service Account..."
	$svc = Get-SPServiceInstance | where {$_.TypeName -eq "Microsoft SharePoint Foundation Sandboxed Code Service"}
	$pi = $svc.Service.ProcessIdentity
	$pi.CurrentIdentityType = "NetworkService"
	$pi.Update()
	$pi.Deploy()

$error.clear()
$user1="$HostName\SandboxSvcAcct"
	$check = get-SPManagedAccount|where{$_.UserName -eq $user1}
	if($check -ne $null)
	{
		
		Remove-SPManagedAccount -Identity $user1 -Confirm:$false
	}

$error.clear()
$WebApp=$settings.WebApp


						$checkSolution=Get-SPSolution -Identity $FullTrustPackage
						if ($checkSolution -ne $null)
						{
							if($checkSolution.deployed -eq $True)
							{
								WriteHeading "Uninstall $FullTrustPackage"
								Uninstall-SPSolution -Identity $FullTrustPackage -Confirm:$false -Local:$true
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							}
							
							WriteHeading "Remove $FullTrustPackage from form"	
							REmove-SPSolution -Identity $FullTrustPackage -force:$true -Confirm:$false
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
						}

$settings.Sites.Site | ForEach-Object {
		$RISite=$_
		$newsite ="http://"+ $HostName+":"+$WebApp.Port+ "/sites/"+ $RIsite.Name
		if ($error) 
				{
			      writeError $error[0].Exception 
			      break;
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

	



