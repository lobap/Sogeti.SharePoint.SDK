
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

#check for site exists
writeheading $RiSiteName
$newsite ="http://"+ $HostName+":"+$WebApp.Port+"/sites/" + $RIsiteName
WriteHeading "Create(Remove if already exists) new site Collection: $newsite "
		#check for any exists with that name:
			
			
			$site = get-spsite -identity $newsite
			
			if ($site -ne $null)
				{
					WriteHeading "Removing existing $newsite.."
					remove-spsite -identity $newsite -confirm:$false
				}
		WriteHeading "Creating new site Collection:$newsite"

		$error.clear()
#			New-SPSite -URL $newsite -OwnerAlias $currentuser.Name -Name $RIsite.Title -Template "STS#1"
			$slsubsite="client"
			$subsiteURL=$newsite
			New-SPWeb -Url $subsiteURL -Template STS#1 -Name $slsubsite -AddToQuickLaunch -AddToTopNav -UseParentTopNav -Verbose

	     if ($error) 
			{
				writeError $error[0].Exception 
				break;
			}

		
writeheading $ClientRIPackage
#deploy solution to new stie
	$newsite="http://"+$HostName+":" + $WebApp.Port+"/sites/"+ $RISiteName
					
							WriteHeading "Step : Add Package($ClientRIPackage) to solutions."
								$checkSolution=Get-SPSolution |where {$_.Name -eq $ClientRIPackage}
								if ($checkSolution -ne $null)
								{
									if($checkSolution.deployed -eq $True)
									{
										WriteHeading "Uninstall $ClientRIPackage"
										Uninstall-SPSolution -Identity $ClientRIPackage -Confirm:$false -Local:$true
									}
									
									WriteHeading "Remove $ClientRIPackage "	
									REmove-SPSolution -Identity $ClientRIPackage -force:$true -Confirm:$false
								}
								$error.clear()
								WriteHeading "Adding Solution $currentDirectory\..\Client.SharePoint\bin\Debug\$ClientRIPackage"
								ADD-SPSolution -LiteralPath "$currentDirectory\..\Client.SharePoint\bin\Debug\$ClientRIPackage"
								if ($error) 
									{
										writeError $error[0].Exception 
										break;
									}
							#install Client RI Activities Package
							WriteHeading "Install Solution : $ClientRIPackage"
						
							Install-SPsolution -Identity $ClientRIPackage -Force:$true -GACDeployment:$true -Local:$true
							if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							#Activate features 
							
							
							
							
							WriteHeading "Activate Feature :Patterns and Practices SharePoint Guidance V3 - Client -Libraries"
							Enable-SPfeature -Identity $LibrariesFID -Force:$true -URL $newsite
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							
							
							WriteHeading "Activate Feature :Patterns and Practices SharePoint Guidance V3 - Client -Pages"
							Enable-SPfeature -Identity $PagesFID -Force:$true -URL $newsite
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							
							WriteHeading "Activate Feature :Patterns and Practices SharePoint Guidance V3 - Client -Silverlight Apps"
							Enable-SPfeature -Identity $SilverAppsFID -Force:$true -URL $newsite
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							WriteHeading "Activate Feature :Patterns and Practices SharePoint Guidance V3 - Client -Javascript Files"
							Enable-SPfeature -Identity $JavaFilesFID -Force:$true -URL $newsite
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
	

cd $currentDirectory
& "$PublishDataExe" /UpdateHost "$currentDirectory\..\Client.ExtService.Silverlight\ServiceReferences.ClientConfig" "$hostname"
