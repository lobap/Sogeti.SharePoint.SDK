
# Set up the environment
. .\initialize.ps1

# Set SharePoint Environment
 . "C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\CONFIG\POWERSHELL\Registration\\sharepoint.ps1"
 
cd $currentDirectory
$logfilename=RIInstall.log

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


WriteHeading "Create(Remove if already exists) new site Collection: " + $RIsiteName
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
				}
		#WriteHeading "Creating new site Collection:$newsite"

		$error.clear()
		#	New-SPSite -URL $newsite -OwnerAlias $currentuser.Name -Name $RIsite.Title -Template "STS#1"
	     if ($error) 
			{
				writeError $error[0].Exception 
				break;
			}		
			
writeheading "Restore backup.."
attrib -r +s "$DataModelDir\..\..\\Installers\datamodel_SPB\partssite.spb"
stsadm.exe -o restore -url $newsite -filename "$DataModelDir\..\..\\Installers\datamodel_SPB\partssite.spb" -overwrite
if ($error) 
			{
				writeError $error[0].Exception 
				break;
			}
WriteHeading "Adding Current User to site collection Admin"


$userdomain=[Environment]::UserDomainName
$username=[Environment]::UserName
set-spsiteadministration $newsite -secondaryowneralias $userdomain\$username

WriteHeading "Adding $listdatapkg Package to FarmSolutions"

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
                        $packagePathName="$DataModelDir" + "\DataModels.SharePointList\DataModels.SharePointList.Model\bin\Debug\$listdatapkg"
						
						WriteHeading "Adding Solution " + $packagePathName
						ADD-SPSolution -LiteralPath $packagePathName
						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
						

WriteHeading "Adding $partsmngpkg Pacakge to FarmSolutions"
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
						$error.clear()
                        $packagePathName="$DataModelDir" + "\DataModels.SharePointList\DataModels.SharePointList.PartsMgmnt\bin\Debug\$partsmngpkg"
						
						WriteHeading "Adding Solution " + $packagePathName
						ADD-SPSolution -LiteralPath $packagePathName
						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
						WriteHeading "Install Solution : $partsmngpkg"
						
						Install-SPsolution -Identity $partsmngpkg -GACDeployment:$true -Local:$true -WebApplication $siteURL -force:$true
						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}

					  WriteHeading "Activate Feature :Patterns and Practies SharePoint Guidance V3 - Data Models - SharePoint List Data - Parts Management - Web Parts"
					  Enable-SPfeature -Identity $PartsMngWebPartsFID -Url $newsite
					  if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
							
					  WriteHeading "Activate Feature :Patterns and Practies SharePoint Guidance V3 - Data Models - SharePoint List Data - Parts Management - Pages"
					  Enable-SPfeature -Identity $PartsMngPagesFID -Url $newsite
					  if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
					  
						WriteHeading "Install Solution : $listdatapkg"
						Install-SPsolution -Identity $listdatapkg -GACDeployment:$true -Local:$true -force:$true
						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}

					#Add SharePoint List Sandbox Package to Site
					$exitpackage= get-SPUserSolution -Identity $splistSandpkg -site $newsite

					 $packagePathName="$DataModelDir" + "\DataModels.SharePointList\DataModels.SharePointList.Sandbox\bin\Debug\$splistSandpkg"
						if ($exitpackage -ne $null)
						{	
							WriteHeading "Removing Existing Package."
							Remove-SPUserSolution -Identity $splistSandpkg -site $newsite -Confirm:$false
							if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
						}

						$error.clear()
						WriteHeading "Adding Package from $packagePathName to $newsite"
						Add-SPUserSolution -LiteralPath $packagePathName  -Site $newsite  
						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}

					#install Hybrid RI Package
					$error.clear()
						
						WriteHeading "Step : Activate :$splistSandpkg"

					 	Install-SPUserSolution -Identity $splistSandpkg -Site $newsite

						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
					WriteHeading "Patterns and Practies SharePoint Guidance V3 - Data Models - SharePoint List Data - Parts Management - Pages - Sandbox"
					Enable-SPFeature -Identity "9d97f987-e315-4d00-b494-cdbf4e8f1d80" -Url $newsite
					if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
WriteHeading "Browse $newsite"