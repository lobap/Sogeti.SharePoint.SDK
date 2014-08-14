
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
$RIsite=$settings.Site


$user1=$settings.user1
$user2=$settings.user2
$user3=$settings.user3
$user4=$settings.user4
$ServiceApplication= $settings.ServiceApplication
$ServiceProxy =$settings.ServiceProxy
$targetApp =$settings.TargetApp
$Email =$settings.Email
$AppPool =$settings.AppPool
$machineName = gc env:computername

WriteHeading $RIsite.Name

WriteHeading "ComputerName:$machineName"
#configure service account
$error.clear()
	WriteHeading "Update Microsoft SharePoint Foundation Sandboxed Code Service Account..."
	$svc = Get-SPServiceInstance | where {$_.TypeName -eq "Microsoft SharePoint Foundation Sandboxed Code Service"}
	$pi = $svc.Service.ProcessIdentity
	$pi.CurrentIdentityType = "NetworkService"
	$pi.Update()
	$pi.Deploy()

$error.clear()

WriteHeading "Removing Service Application"

# Remove Service Application proxy.

	$proxy=get-spserviceapplicationproxy|Where{$_.DisplayName -eq $ServiceProxy.Name}
	if($proxy -ne $null)
	{
		Remove-SPServiceApplicationProxy -Identity $proxy.ID -confirm:$false
		WriteHeading "Removed Proxy."
	}
	
	
#Remove Service Application.

 $sp=get-spserviceapplication -Name $ServiceApplication.Name
	remove-spserviceapplication -identity $sp.id -Confirm:$false -RemoveData:$true

	$poolName=$AppPool.Name
	Remove-SPServiceApplicationPool –Identity: $poolName -Confirm:$false
	CSCRIPT c:\Inetpub\AdminScripts\ADSUTIL.VBS delete w3svc/AppPools/$poolName

WriteHeading "Removing Site collection"

#Removing Site Collection

WriteHeading "(Remove if already exists) new site Collection: "+$RIsite.Name
		#check for any exists with that name:
			$newsite ="http://"+ $HostName+":"+$WebApp.Port+"/sites/"+$RIsite.Name
WriteHeading $newsite
			
WriteHeading "Setting Site.."
			$site = get-spsite $newsite

WriteHeading "Name:"+ $site.id
			
			if ($site -ne $null)
				{
					
					remove-spsite -identity $site.id -confirm:$false
				}



$userName=$hostname + "\"+ $user1.Name
$check = get-SPManagedAccount|where{$_.UserName -eq $userName}
	if($check -ne $null)
	{
		
		Remove-SPManagedAccount -Identity $userName -Confirm:$false
	}

$userName=$hostname + "\"+ $user2.Name
$check = get-SPManagedAccount|where{$_.UserName -eq $userName}
	if($check -ne $null)
	{
		Remove-SPManagedAccount -Identity $userName -Confirm:$false
	}	

$userName=$hostname + "\"+ $user3.Name
$check = get-SPManagedAccount|where{$_.UserName -eq $userName}
	if($check -ne $null)
	{
		Remove-SPManagedAccount -Identity $userName -Confirm:$false
	}
#End delete site collection	