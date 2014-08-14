
Set-ExecutionPolicy Unrestricted 
#set error preference
	$erroractionpreference = "SilentlyContinue"
	#$erroractionpreference = "Continue"



$currentDirectory = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath


$ExeModelDir=$currentDirectory+"\..\..\ExecutionModels"


cd $ExeModelDir
$ExeModelDir = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath
$PublishDataExe="$ExeModelDir\Shared Code\Setup\PublishDataSources\PublishData\bin\x64\Debug\PublishData.exe"



. "$ExeModelDir\Shared Code\Setup\ps\outputfunctions.ps1"
. "$ExeModelDir\Shared Code\Setup\ps\commoninitialize.ps1"

$ClientRIPackage="Client.SharePoint.wsp"
$JavaFilesFID="a0601d72-5c8a-4983-98f1-f3e511a6e190"
$LibrariesFID="6ac545fe-f18f-41f9-8d33-3072042c7caf"
$PagesFID="37545e2c-1883-4395-90c7-183e1d0a3364"
$SilverAppsFID="13562255-9e2e-4e3c-9f5d-5f7b2eb1ec80"




 
