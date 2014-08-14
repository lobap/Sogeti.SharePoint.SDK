
Set-ExecutionPolicy Unrestricted 
#set error preference
	$erroractionpreference = "SilentlyContinue"
	

$currentDirectory = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath


$ExeModelDir=$currentDirectory+"\..\..\..\ExecutionModels"
$DataModelDir=$currentDirectory+"\..\..\"
cd $ExeModelDir
$ExeModelDir = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath

cd $DataModelDir
$DataModelDir = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath

cd $currentDirectory

. "$ExeModelDir\Shared Code\Setup\ps\outputfunctions.ps1"
. "$ExeModelDir\Shared Code\Setup\ps\commoninitialize.ps1"


	# Initializing feature ids
	$BdcModel="1d7ad3d0-1fe5-4b10-8cdf-858291174817"
	$ExternalList="bbd879cc-f701-4391-9fe6-22721ed9421f"
	$pages="257f30a9-4acd-42c3-85d7-0aba07d8ef98"
	$webparts="975358d1-36ed-42fa-a0aa-926136c18319"
	$Navigation ="6062f22b-1a01-42f0-960c-6648b5c9ab56"


	$PartsManagementPackage="DataModels.ExternalData.PartsManagement.wsp"

	#Get Current User
$currentuser=[Security.Principal.WindowsIdentity]::GetCurrent().Name

Function Enablefeature([string]$feature)
{
	$checkFeature= Get-SPfeature | Where-Object {$_.Id -eq $feature}
	
	if($error)
		{
			write-Error $error[0].Exception 
			#break;
		}
	if($checkFeature)
		{
		Enable-spfeature -identity $feature -confirm:$false -URL $newsiteURL
		 if ($error) 
				{
						writeError $error[0].Exception 
						break;
					}
		}
}


	

Function uninstallfeature([string]$feature)
{
	$checkFeature= Get-SPfeature | Where-Object {$_.Id -eq $feature}
	if($error)
	{
		write-Error $error[0].Exception 
		break;
	}
	if($checkFeature -ne $null)
	{
	Uninstall-spfeature -identity $feature -force -confirm:$false

	}
}
