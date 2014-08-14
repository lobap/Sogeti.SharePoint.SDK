Set-ExecutionPolicy Unrestricted 
# Set up the environment
. .\initialize.ps1

#set error preference
	#$erroractionpreference = "SilentlyContinue"


	
$HostName=gc env:computername
$currentDirectory = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath
$PartsManagementPackage="DataModels.ExternalData.PartsManagement.wsp"

 


 cd $currentDirectory
[System.Xml.XmlElement]$settings = Get-Settings

if ($settings -eq $null)
{
	return $false
}
$PMSite=$settings.Site.Name

	
# Set SharePoint Environment
 . "C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\CONFIG\POWERSHELL\Registration\\sharepoint.ps1"
$normalWeb=$settings.NormalWeb

 



#Check  Normal web is running

	$siteURL="http://"+$HostName+":" + $normalWeb.Port
	WriteHeading "Connecting to $siteURL"
	$web=Get-SPWeb -site $siteURL
	if ($web -eq $null)
	{
		Write-Error "Unable to Connect $siteURL. Please check settings.xml for NormalWeb Port information."
		break;
	}
	

			writeheading "Uninstall Features"
			WriteHeading "Uninstalling Feature : Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management - BDC Model "
			uninstallfeature($BdcModel)
			WriteHeading "Unistalling Feature :Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management - External List Instances "
			uninstallfeature($ExternalList)
			WriteHeading "Unistalling Feature : Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management -  Pages"
			uninstallfeature($pages)
			WriteHeading "Unistalling Feature : Patterns and Practies SharePoint Guidance V3 - Data Models - External Data - Parts Management - WebParts"
			uninstallfeature($webparts)
			WriteHeading "Unistalling Feature : Patterns and Practies SharePoint Guidance V3 - Data Models - External Data - Parts Management -  Navigation"
			uninstallfeature($Navigation)
			

#Delete PartsManagement subsite Under normalweb



$newsiteURL ="http://"+ $HostName+"/sites/" + $PMSite
			 if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}
			$site = get-spsite | Where-Object {$_.Url -eq $newsiteURL}
		
			if ($site -ne $null)
				{
			
					WriteHeading "Removing existing $newsiteURL.."
					remove-spsite -identity $newsiteURL -confirm:$false
					 STSADM -o execadmsvcjobs
				}



#Uninstall & Remove Solution

	$error.clear()
	$deployedSolutions = Get-SPSolution | Where-Object {$_.Name -eq $PartsManagementPackage}
$solutionid= $deployedSolutions.SolutionId
			 if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}

 if($solutionid -ne  $null )

{
	   Write-Host "Uninstall $PartsManagementPackage"
	   if($deployedSolutions.Deployed) 
		   {
		       
		     Uninstall-SPSolution -Identity $solutionid -Confirm:$false -AllWebapplication:$true -Local:$true
			 STSADM -o execadmsvcjobs
			 if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}
					
			 writeheading "uninstalled"
		    }

	    Write-Host  "Removing $PartsManagementPackage"
	    Remove-SPSolution -Identity $solutionID -Force:$true -Confirm:$false
			STSADM -o execadmsvcjobs
		if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}
	    
	
}

   

  


