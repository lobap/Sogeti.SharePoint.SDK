


$PublishDataExe = $ExeModelDir+"\Shared Code\Setup\PublishDataSources\PublishData\bin\x64\Debug\PublishData.exe"
#Create new site collection for RI
$error.clear()
$settings.Sites.Site | ForEach-Object {
		$RIsite=$_
		WriteHeading "Create(Remove if already exists) new site Collection: " + $RIsite.Name
		#check for any exists with that name:
			$newsite ="http://"+ $HostName+":"+$WebApp.Port+"/sites/" + $RIsite.Name
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
		WriteHeading "Creating new site Collection:$newsite"

		$error.clear()
			New-SPSite -URL $newsite -OwnerAlias $currentuser.Name -Name $RIsite.Title -Template "STS#1"
	     if ($error) 
			{
				writeError $error[0].Exception 
				break;
			}		
			
		#add solution to solution gallary
			WriteHeading "Step4: Add Package($SandRIPackage) to solution Gallary."

			#check of if any existing with that name

			$exitpackage= get-SPUserSolution -Identity $SandRIPackage -site $newsite

			$packagefullpath= $ExeModelDir +"\Sandboxed\ExecutionModels.Sandboxed\bin\Debug\"+$SandRIPackage
			
			if ($exitpackage -ne $null)
				{	
					WriteHeading "Removing Existing Package."
					Remove-SPUserSolution -Identity $SandRIPackage -site $newsite -Confirm:$false
				}

			$error.clear()
			WriteHeading "Adding Package from $packagefullpath to $newsite"
			Add-SPUserSolution -LiteralPath $packagefullpath  -Site $newsite  >>$logfilename
			if ($error) 
				{
					writeError $error[0].Exception 
					break;
				}

		#install Sandbox RI Package
		$error.clear()
			 
			WriteHeading "Step5: Activate :$SandRIPackage"

		 	Install-SPUserSolution -Identity $SandRIPackage -Site $newsite

			if ($error) 
				{
					writeError $error[0].Exception 
					break;
				}

			
			
			WriteHeading "Step6: Creating Sub Sites... "
			
			$error.clear()
			$RIsite.Webs.Web | ForEach-Object {
				$subsite = $_
				if($subsite)
				{
					$subsiteURL = $newsite + "/" + $subsite.Name
					New-SPWeb -Url $subsiteURL -Template "STS#1" -Name $subsite.Name -Description $subsite.Description -AddToQuickLaunch:$false -AddToTopNav:$ture -UseParentTopNav -Verbose
					Enable-SPFeature -Identity "bf2ed24d-3831-4576-af7f-e96b42c4da30" -Url $subsiteURL
					
					if ($error) 
						{
							writeError $error[0].Exception 
							break;
						}
				}
			}

}


		






