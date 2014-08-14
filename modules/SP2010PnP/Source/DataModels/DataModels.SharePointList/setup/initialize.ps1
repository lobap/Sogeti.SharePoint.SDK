
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

$partsmngpkg="datamodels.sharepointlistdata.partsmanagement.wsp"
$listdatapkg="datamodels.sharepointlistdata.model.wsp"
$splistSandpkg="DataModels.SharePointList.Sandbox.wsp"

$RIsiteName="SharePointList"
$PartsMngWebPartsFID="a715a1b0-8cc5-459a-9ba0-d69c5c0e1248"
$PartsMngPagesFID="49acdd6c-10e6-4030-bb23-e28c30bc0aab"
$PartsMngNavFID="d0afa4a0-2a78-4464-bf10-b714b83285f0"
$ListInstFID="698b61ea-7f62-4b9e-b11e-13dba3c952a3"
$ContentTypesFID="f8f5d273-f38b-4f69-b885-b52d435f1e5d"
$InitializeModelFID="3ec349a9-1c63-4cba-afb3-98631c08db15"

. "$ExeModelDir\Shared Code\Setup\ps\outputfunctions.ps1"
. "$ExeModelDir\Shared Code\Setup\ps\commoninitialize.ps1"





 
