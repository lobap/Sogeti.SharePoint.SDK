
Set-ExecutionPolicy Unrestricted 
#set error preference
	$erroractionpreference = "SilentlyContinue"
	#$erroractionpreference = "Continue"



$currentDirectory = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath


$ExeModelDir=$currentDirectory+"\..\..\..\ExecutionModels"
$DataModelDir=$currentDirectory+"\..\..\"
cd $ExeModelDir
$ExeModelDir = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath

cd $DataModelDir
$DataModelDir = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath



. "$ExeModelDir\Shared Code\Setup\ps\outputfunctions.ps1"
. "$ExeModelDir\Shared Code\Setup\ps\commoninitialize.ps1"

$ListOfPkg="DataModels.ListOf.CreateLink.wsp"




 
