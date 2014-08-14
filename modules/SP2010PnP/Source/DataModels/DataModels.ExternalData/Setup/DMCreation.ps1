
Set-ExecutionPolicy Unrestricted 
# Set up the environment
. .\initialize.ps1

# Set SharePoint Environment
 . "C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\CONFIG\POWERSHELL\Registration\\sharepoint.ps1"
 


$HostName=gc env:computername
cd $currentDirectory


[System.Xml.XmlElement]$settings = Get-Settings

if ($settings -eq $null)
	{
		return $false
	}

	

$PMSite=$settings.Site.Name
$normalWeb=$settings.NormalWeb
$siteURL="http://"+$HostName+":" + $normalWeb.Port
#Check for farmAdmin
$AdminClaims = new-spclaimsPrincipal -identity $currentuser -identitytype windowssamaccountname
function Grant($model)
{
	#$model
	grant-spbusinessdatacatalogmetadataobject -identity $model -Principal $AdminClaims -Right SetPermissions
	grant-spbusinessdatacatalogmetadataobject -identity $model -Principal $AdminClaims -Right Execute
	grant-spbusinessdatacatalogmetadataobject -identity $model -Principal $AdminClaims -Right Edit
	grant-spbusinessdatacatalogmetadataobject -identity $model -Principal $AdminClaims -Right Selectableinclients 
}

Function addToBdcGroup
{



		$Categories= Get-SPBusinessDataCatalogMetadataObject -Namespace "DataModels.ExternalData.PartsManagement" -BdcObjectType entity -servicecontext $siteURL -Name Categories
		$Departments= Get-SPBusinessDataCatalogMetadataObject -Namespace "DataModels.ExternalData.PartsManagement" -BdcObjectType entity -servicecontext $siteURL -Name Departments
		$InventoryLocations= Get-SPBusinessDataCatalogMetadataObject -Namespace "DataModels.ExternalData.PartsManagement" -BdcObjectType entity -servicecontext $siteURL -Name InventoryLocations
		$MachineDepartments= Get-SPBusinessDataCatalogMetadataObject -Namespace "DataModels.ExternalData.PartsManagement" -BdcObjectType entity -servicecontext $siteURL -Name MachineDepartments
		$MachineParts= Get-SPBusinessDataCatalogMetadataObject -Namespace "DataModels.ExternalData.PartsManagement" -BdcObjectType entity -servicecontext $siteURL -Name MachineParts
		$Machines= Get-SPBusinessDataCatalogMetadataObject -Namespace "DataModels.ExternalData.PartsManagement" -BdcObjectType entity -servicecontext $siteURL -Name Machines
		$Manufacturers= Get-SPBusinessDataCatalogMetadataObject -Namespace "DataModels.ExternalData.PartsManagement" -BdcObjectType entity -servicecontext $siteURL -Name Manufacturers
		$Parts= Get-SPBusinessDataCatalogMetadataObject -Namespace "DataModels.ExternalData.PartsManagement" -BdcObjectType entity -servicecontext $siteURL -Name Parts
		$PartSuppliers= Get-SPBusinessDataCatalogMetadataObject -Namespace "DataModels.ExternalData.PartsManagement" -BdcObjectType entity -servicecontext $siteURL -Name PartSuppliers
		$Suppliers= Get-SPBusinessDataCatalogMetadataObject -Namespace "DataModels.ExternalData.PartsManagement" -BdcObjectType entity -servicecontext $siteURL -Name Suppliers

		#$Categories

		Grant($Categories)
		Grant($Departments)
		Grant($InventoryLocations)
		Grant($MachineDepartments)
		Grant($MachineParts)
		Grant($Machines)
		Grant($Manufacturers)
		Grant($Parts)
		Grant($PartSuppliers)
		Grant($Suppliers)


}


#Check  Normal web is running

	
	WriteHeading "Connecting to $siteURL"
	$web=Get-SPWeb -site $siteURL
	if ($web -eq $null)
	{
		Write-Error "Unable to Connect $siteURL. Please check settings.xml for NormalWeb Port information."
		break;
	}




WriteHeading "Create new site Collection: $PMSite" 

		#check for any exists with that name:
			$newsiteURL ="http://"+ $HostName+":" + $normalWeb.Port+"/sites/" + $PMSite
			 
			$site = get-spsite -identity $newsiteURL
	
			if ($site -ne $null)
				{
					$error.clear()
					WriteHeading "Removing existing $newsiteURL.."
					remove-spsite -identity $newsiteURL -confirm:$false
					if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}
				}

		$error.clear()
			WriteHeading "Creating New Site with  $newsiteURL.."
			New-SPSite -URL $newsiteURL -OwnerAlias $currentuser -Name $PMSite -Template "STS#1"
	     if ($error) 
			{
				writeError $error[0].Exception 
				break;
			}		

#Uninstall and remove solution
	$error.clear()
	$solution =get-spsolution 
	$solutionid= $PartsManagementPackage

#solution name
WriteHeading "Retrive Existing Solution with Name : $PartsManagementPackage"
$deployedSolutions = Get-SPSolution | Where-Object {$_.Name -eq $PartsManagementPackage}
$solutionid= $deployedSolutions.SolutionId



			 if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}

 if($solutionid -ne  $null )

{
	   WriteHeading "Uninstall $PartsManagementPackage"
	   if($deployedSolutions.Deployed) 
		   {
		     
		     Uninstall-SPSolution -Identity $solutionid -Confirm:$false -AllWebapplication:$true -Local:$true
			 
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

WriteHeading "Adding $PartsManagementPackage to Farm"
		$error.clear()
		ADD-SPSolution -LiteralPath $currentDirectory\..\DataModels.ExternalData.PartsManagement\bin\Debug\$PartsManagementPackage
			STSADM -o execadmsvcjobs
		if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}
			
writeheading "Installing $PartsManagementPackage"
		$error.clear()
		
		Install-SPSolution -Identity $PartsManagementPackage -GACDeployment:$true -WebApplication $siteURL -force:$true -Local:$true
		STSADM -o execadmsvcjobs
		if ($error) 
					{
						writeError $error[0].Exception 
						break;
					}
		
						
WriteHeading "Successfully Installed"


WriteHeading "Activate Features:"
		$error.clear()


		$error.clear()
	
			#WriteHeading "Activating Feature : Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management - BDC Model "
				#Enablefeature($BdcModel)
			WriteHeading "Activating Feature : Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management - External List Instances "
				Enablefeature($ExternalList)
	     	WriteHeading "Activating Feature : Patterns and Practies SharePoint Guidance V3 - Data Models - External Data - Parts Management - WebParts"
				Enablefeature($webparts)
        	WriteHeading "Activating Feature : Patterns and Practices SharePoint Guidance V3 - Data Models - External Data - Parts Management -  Pages"
				Enablefeature($pages)

			WriteHeading "Give permissions to BDC Service applications"
			addToBdcGroup
