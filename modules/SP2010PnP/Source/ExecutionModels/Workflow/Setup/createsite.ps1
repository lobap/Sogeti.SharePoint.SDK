
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

. "$ExeModelDir\Shared Code\Setup\ps\Commoncreatesite.ps1"




$error.clear()
# call test data script to populate
$settings.Sites.Site | ForEach-Object {
		$RISite=$_
		

			if ($RISite.WFRI -eq "True")
			{

			$newsite="http://"+$HostName+":" + $WebApp.Port+"/sites/"+ $RISite.Name
					#add Hybrid2 Activities package to farm solutions
							WriteHeading "Step : Add Package($Hybrid2ActPackage) to solutions."
								$checkSolution=Get-SPSolution -Identity $Hybrid2ActPackage
								if ($checkSolution -ne $null)
								{
									if($checkSolution.deployed -eq $True)
									{
										disable-SPfeature -Identity $SiteProvActivityFID -Force:$true -Confirm:$false

										WriteHeading "Uninstall $Hybrid2ActPackage"
										Uninstall-SPSolution -Identity $$Hybrid2ActPackage -Confirm:$false -Local:$true
									}
									
									WriteHeading "Remove $$Hybrid2ActPackage "	
									REmove-SPSolution -Identity $Hybrid2ActPackage -force:$true -Confirm:$false
								}
								$error.clear()
								WriteHeading "Adding Solution $Hyrbid2solPath\ExecutionModels.Workflow.FullTrust.Activities\bin\Debug\$Hybrid2ActPackage"
								ADD-SPSolution -LiteralPath "$Hyrbid2solPath\ExecutionModels.Workflow.FullTrust.Activities\bin\Debug\$Hybrid2ActPackage"
								if ($error) 
									{
										writeError $error[0].Exception 
										break;
									}
							#install Hybrid2 Activities Package
							WriteHeading "Install Solution : $Hybrid2ActPackage"
						
							Install-SPsolution -Identity $Hybrid2ActPackage -Force:$true -GACDeployment:$true -Local:$true
							if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							#Activate features 
							
							$webAppURL="http://"+$HostName+":" + $centralAdminPort
							WriteHeading "Activate Navigation Feature"
							Enable-SPfeature -Identity $SiteProvActivityFID -Force:$true -URL $newsite
								if ($error) 
								{
									writeError $error[0].Exception 
									break;
								}
							
							#add solution to solution gallary
								WriteHeading "Step4: Add Package($WFSandboxPackage) to solution Gallary."

								#check of if any existing with that name

								$exitpackage= get-SPUserSolution -Identity $WFSandboxPackage -site $newsite

								$packagefullpath= $Hyrbid2solPath +"\ExecutionModels.Workflow.Sandbox.Activities\bin\Debug\"+$WFSandboxPackage
								
								if ($exitpackage -ne $null)
									{	
										WriteHeading "Removing Existing Package."
										Remove-SPUserSolution -Identity $WFSandboxPackage -site $newsite -Confirm:$false
									}

								$error.clear()
								WriteHeading "Adding Package from $WFSandboxPackage to $newsite"
								Add-SPUserSolution -LiteralPath $packagefullpath  -Site $newsite  >>$logfilename
								if ($error) 
									{
										writeError $error[0].Exception 
										break;
									}

							#install Sandbox RI Package
							$error.clear()
								 
								WriteHeading "Step5: Activate :$WFSandboxPackage"

							 	Install-SPUserSolution -Identity $WFSandboxPackage -Site $newsite

								if ($error) 
									{
										writeError $error[0].Exception 
										break;
									}

								#add Create Project site wsp to solution gallary
								$CrProjSitePkg="Create_Project_Site.wsp"
								WriteHeading "Step4: Add Package($CrProjSitePkg) to solution Gallary."

								#check of if any existing with that name

								$exitpackage= get-SPUserSolution -Identity $CrProjSitePkg -site $newsite

								$packagefullpath= $Hyrbid2solPath +"\Create Project Site\bin\Debug\"+$CrProjSitePkg
								
								if ($exitpackage -ne $null)
									{	
										WriteHeading "Removing Existing Package."
										Remove-SPUserSolution -Identity $CrProjSitePkg -site $newsite -Confirm:$false
									}

								$error.clear()
								WriteHeading "Adding Package from $CrProjSitePkg to $newsite"
								Add-SPUserSolution -LiteralPath $packagefullpath  -Site $newsite  >>$logfilename
								if ($error) 
									{
										writeError $error[0].Exception 
										break;
									}

							#install Sandbox RI Package
							$error.clear()
								 
								WriteHeading "Step: Activate :$CrProjSitePkg"

							 	Install-SPUserSolution -Identity $CrProjSitePkg -Site $newsite

								if ($error) 
									{
										writeError $error[0].Exception 
										break;
									}

								
						#Activate Create project feature
						WriteHeading "Step: Activate :Workflow template CreateProjectSite"
						Enable-SPFeature -Identity "ad9e8207-82c4-45a7-b8c4-88bf781deead" -Url $newsite
						if ($error) 
									{
										writeError $error[0].Exception 
										break;
									}
						WriteHeading "Step Work flow Association with Estimate Content Type."
						
            						& "$PublishDataExe" /WFAssociation $newsite "0x010100B022E34803AB43AEA87C937368261B49" "Create Project Site" "WFRI" "Tasks" "Workflow History"
							if ($error) 
									{
										writeError $error[0].Exception 
										break;
									}
									$error.clear()


			}
		. "$ExeModelDir\Shared Code\Setup\ps\testdata.ps1"
		}
cd $currentDirectory



