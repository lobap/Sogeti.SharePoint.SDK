
Set-ExecutionPolicy Unrestricted 
#set error preference
	$erroractionpreference = "SilentlyContinue"
	#$erroractionpreference = "Continue"

function Pause ($Message="Press any key to continue...")
{
Write-Host -NoNewLine $Message
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
Write-Host ""
}


$currentDirectory = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath


$ExeModelDir=$currentDirectory+"\..\..\"
cd $ExeModelDir
$ExeModelDir = [Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath


. "$ExeModelDir\Shared Code\Setup\ps\outputfunctions.ps1"
. "$ExeModelDir\Shared Code\Setup\ps\commoninitialize.ps1"





 
