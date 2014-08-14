	@echo off
	cls
	setlocal
	pushd .
	Set SourceFolder=%~dp0%
	Set SourceFolder=%SourceFolder:~0,-20%
	set subfolder=%~dp0%SupportingFiles\

	cd %subfolder%

	Echo Would like to uninstall Contoso Setup?
	echo CTRL + C To quit. Or 
	pause
	cd %subfolder%

	cd %subfolder%
	echo call 910_CleanFBA.bat
		 call 910_CleanFBA.bat

	cd %subfolder%
	echo call 907_CleanPartnerCentral.bat
		 call 907_CleanPartnerCentral.bat

	cd %subfolder%
	echo call 906_CleanPartnerSites.bat
		 call 906_CleanPartnerSites.bat

	cd %subfolder%
	call 905_CleanPublishingPortal.bat
		 call 905_CleanPublishingPortal.bat

	cd %subfolder%
	echo call 904c_DeActivateAppFeatures.bat
		 call 904c_DeActivateAppFeatures.bat

	cd %subfolder%
	echo call 904b2_RetractSolutions.bat
		 call 904b2_RetractSolutions.bat
		 
	cd %subfolder%
	echo call 904b1_DeleteSolutions.bat
		 call 904b1_DeleteSolutions.bat

	cd %subfolder%
	echo call 904a_CleanContosoWebandSsp.bat
		 call 904a_CleanContosoWebandSsp.bat

	cd %subfolder%
	echo call 902_CleanContosoServices.bat
		 call 902_CleanContosoServices.bat

	cd %subfolder%
	echo call 901b_CleanWindowsUsers.bat
		 call 901b_CleanWindowsUsers.bat

	cd %subfolder%
	echo call 901a_CleanTestCertificate.bat
		 call 901a_CleanTestCertificate.bat

	pause