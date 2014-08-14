
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
		. "$ExeModelDir\Shared Code\Setup\ps\testdata.ps1"

		# check if Sandbox Webpart need to add webpart need to add
			if ($RISite.SandWebPart -eq "True")
			{
			    #add webpart to default page
				$newsite="http://"+$HostName+":" + $WebApp.Port+"/sites/"+ $RISite.Name
				if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}
				WriteHeading  "Adding WebPart to default page."
				$error.clear()
				$WebScope=Start-SPAssignment
					  $site=Get-SPSite($newsite)
					  $web=$WebScope|Get-SPWeb($newsite)
					  if ($error) 
							{
								writeError $error[0].Exception 
								break;
							}
					  $file=$web.GetFile("default.aspx")
				  #get the webpart manager

					  $manager=$file.GetLimitedWebPartManager([System.Web.UI.WebControls.WebParts.PersonalizationScope]::Shared)

					   $webpart = new-object Microsoft.SharePoint.WebPartPages.SPUserCodeWebPart

					   $webpart.AssemblyFullName="ExecutionModels.Sandboxed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9ae6d3a07c1fb8de"
					   $webpart.SolutionId="eef6447a-4f05-47fe-9e8a-7c2886671008"
					   $webpart.TypeFullName="ExecutionModels.Sandboxed.AggregateView.AggregateView"
					   $webpart.Title="P&P SPG V3 - Execution Models Sandbox Aggregate View"
					   $manager.AddWebPart($webpart,"left",0)
					   if ($error) 
						{
							writeError $error[0].Exception 
							break;
						}

				Stop-SPAssignment $WebScope
				

			}
		}
cd $currentDirectory



