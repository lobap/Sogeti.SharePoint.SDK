
# Set up the environment
. .\initialize.ps1

# Set SharePoint Environment
 . "C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\CONFIG\POWERSHELL\Registration\\sharepoint.ps1"
 
cd $currentDirectory
$logfilename=RIInstall.log
$centralAdminPort=[Microsoft.SharePoint.Administration.SPAdministrationWebApplication]::Local.Sites.VirtualServer.Port

[System.Xml.XmlElement]$settings = Get-Settings

if ($settings -eq $null) {
	return $false
}
$error.clear()
$WebApp=$settings.WebApp

#Check  Normal web and Central Admin are running
	$siteURL="http://"+$HostName+":" + $WebApp.Port
	if ($error) 
		{
	      writeError $error[0].Exception 
		  break;
		}
	WriteHeading "Connecting to $siteURL"
	$web=Get-SPWeb -site $siteURL
	if ($web -eq $null)
	{
		WriteError "Unable to Connect $siteURL. Please check settings.xml for Web Application Port information."
		break;
	}

. "$ExeModelDir\Shared Code\Setup\ps\Commoncreatesite.ps1"

# related full trust



$error.clear()
# call test data script to populate
$settings.Sites.Site | ForEach-Object {
		$RISite=$_
		
		WriteHeading $RISite.Title
		if ($RISite.FarmSol -eq "True")
		{
			
			

			#add FullTrustProxy Registration package to farm solutions
					WriteHeading "Step : Add Package($FarmSolJobsPackage) to Farm"
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
                        
						WriteHeading "Adding ExecutionModels.FarmSolution($FarmSolPackage) Package to farm"
						
						WriteHeading "Step : Adding Package($FarmSolPackage) "
						
						$error.clear()
                        $packagePathName= "$FTRIsolPath\ExecutionModels.FullTrust\bin\Debug\$FarmSolPackage"
						WriteHeading "Adding Solution " + $packagePathName
						ADD-SPSolution -LiteralPath $packagePathName
						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
						WriteHeading "Install Solution : $FarmSolPackage"
						
						Install-SPsolution -Identity $FarmSolPackage -GACDeployment:$true -Local:$true -force:$true 
						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
						#Activate features 
						
						$webAppURL="http://"+$HostName+":" + $centralAdminPort
						WriteHeading "Activate Navigation Feature"
						Enable-SPfeature -Identity $AdminFormsNavFID -Force:$true -URL $webAppURL
							if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}

						$packagePathName= "$FTRIsolPath\ExecutionModels.FullTrust.Jobs\bin\Debug\$FarmSolJobsPackage"
						WriteHeading "Adding Solution : $packagePathName"
						ADD-SPSolution -LiteralPath $packagePathName
						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
						WriteHeading "Install Solution : $FarmSolJobsPackage"
						
						Install-SPsolution -Identity $FarmSolJobsPackage -GACDeployment:$true -Local:$true -force:$true 
						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
						#Activate features 
						#ApprovedEstJobFID
						$webAppURL="http://"+$HostName+":" + $WebApp.Port
						WriteHeading "Activate Job Feature"
						Enable-SPfeature -Identity $ApprovedEstJobFID -Force:$true -URL $webAppURL
							if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
						$newsite="http://"+$HostName+":" + $WebApp.Port+"/sites/"+ $RISite.Name
						WriteHeading "Activate Approved Estimate List Feature"
						Enable-SPfeature -Identity $ApprovedEstLstFID -Force:$true -URL $newsite 
						if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}


						

						
		   }

		  . "$ExeModelDir\Shared Code\Setup\ps\testdata.ps1"

		if ($error) 
			{
				writeError $error[0].Exception 
				break;
			}
		}
		

cd $currentDirectory

