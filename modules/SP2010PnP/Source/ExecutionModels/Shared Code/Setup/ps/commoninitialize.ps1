#Set the colors for the console window
$Host.Ui.RawUi.BackGroundColor = "Black"
$Host.Ui.RawUi.ForeGroundColor = "White"
$HostName=gc env:computername
$CurrentUser = [System.Security.Principal.WindowsIdentity]::GetCurrent()
$SandRIsolName= "ExecutionModels.Sandboxed.sln"
$lofilename = "RISetup.log"
$SandRIPackage="ExecutionModels.Sandboxed.wsp"
$HybridRIPackage="ExecutionModels.Sandboxed.Proxy.wsp"
$FullTrustPackage="VendorSystemProxyRegistration.wsp"
$Hybrid2ActPackage="ExecutionModels.Workflow.FullTrust.SubsiteCreation.wsp"
$HybridWFActPackage="ExecutionModels.Workflow.SandboxActivity.wsp"
$CreateProjectSitepkg="Create Project Site.wsp"
$WFSandboxPackage="ExecutionModels.Workflow.SandboxActivity.wsp"
$FarmSolPackage="ExecutionModels.FullTrust.WSP"
$FarmSolJobsPackage="ExecutionModels.FullTrust.Jobs.wsp"
$ApprovedEstJobFID="2cf8a30f-448a-4b8b-9bce-fb818f8099eb"
$ApprovedEstLstFID="d4b37f6f-9564-4b71-9005-733714a1eb6a"
$AdminFormsNavFID="781e2927-9f46-4cdd-b8e6-bdae4a44586d"
$SiteProvActivityFID="5468c754-8af2-4734-831c-5c3778c377f3"
$CopyLibraryModuleFID="f33934c3-83ed-42d5-9aeb-0458de74e46c"
$HybridRIsolPath=$ExeModelDir+"\Proxy\"
$SandRIsolPath=$ExeModelDir+"\Sandboxed\"
$FTRIsolPath=$ExeModelDir+"\FullTrust\"
$Hyrbid2solPath=$ExeModelDir+"\workflow\"

WriteHeading "start the Microsoft SharePoint Foundation User Code Service:"
	Start-Service –Name SPUserCodeV4


function Get-Settings() {
	WriteHeading " Loading Settings "
	[xml]$settings = Get-Content .\settings.xml

	return $settings.settings
}
