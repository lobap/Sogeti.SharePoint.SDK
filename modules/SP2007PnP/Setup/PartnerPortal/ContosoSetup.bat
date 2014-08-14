	@echo off
	cls
	setlocal
	pushd .

	Set SourceFolder=%~dp0%
	Set SourceFolder=%SourceFolder:~0,-20%
	set subfolder=%~dp0%SupportingFiles\

	cd %subfolder%
	call 00_Parameters.bat

	cd %subfolder%
	call showparams.bat

	Echo Log File: %subfolder%ContosoInstall.log for More detail

	cd %subfolder%
	echo.                                                             > ContosoInstall.log
	echo *********************************************************** >> ContosoInstall.log
	echo *********************************************************** >> ContosoInstall.log
	echo %time%                                                      >> ContosoInstall.log
	echo ContosoSetup.bat Started                                    >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 01a_CreateTestCertificate.bat
	echo call 01a_CreateTestCertificate.bat                          >> ContosoInstall.log
	echo.
	echo.                                                            >> ContosoInstall.log
	echo Please enter the password for all the password prompts      >> ContosoInstall.log
	echo Please enter the password for all the password prompts      
	echo The password is for the private key of your Root Certificate Authority >> ContosoInstall.log
	echo The password is for the private key of your Root Certificate Authority 
	echo You may choose any password.                                >> ContosoInstall.log
	echo You may choose any password.                                
	echo The password for all the prompts needs to be the same       >> ContosoInstall.log
	echo The password for all the prompts needs to be the same       
	echo.                                                            >> ContosoInstall.log
	echo.
	
	     call 01a_CreateTestCertificate.bat                          >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

    echo.                                                            >> ContosoInstall.log
    echo.                                                            
	echo The rest of the setup may take 20-40 minutes.               >> ContosoInstall.log
	echo The rest of the setup may take 20-40 minutes.               
	echo There are total 11 steps                                    >> ContosoInstall.log
	echo There are total 11 steps                                    
    echo You may come back later to check for the status             >> ContosoInstall.log
    echo You may come back later to check for the status             
    echo.                                                            >> ContosoInstall.log
    echo.                                                            
	echo *********************************************************** >> ContosoInstall.log
	echo *********************************************************** 
    
	echo call 01b_CreateWindowsUsers.bat 
	echo call 01b_CreateWindowsUsers.bat                             >> ContosoInstall.log
	     call 01b_CreateWindowsUsers.bat                             >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.
	echo
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log
	
	echo call EVENTCREATE /ID 1 /L APPLICATION /T INFORMATION /SO "Contoso Partner Portal" /D "Event Source Created for Contoso Partner Portal"
	echo call EVENTCREATE /ID 1 /L APPLICATION /T INFORMATION /SO "Contoso Partner Portal" /D "Event Source Created for Contoso Partner Portal" >> ContosoInstall.log
	     call EVENTCREATE /ID 1 /L APPLICATION /T INFORMATION /SO "Contoso Partner Portal" /D "Event Source Created for Contoso Partner Portal" >> ContosoInstall.log
	
	
	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 02_CreateContosoServices.bat 
	echo call 02_CreateContosoServices.bat                           >> ContosoInstall.log
	     call 02_CreateContosoServices.bat                           >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 03a_BuildSolutions.bat
	echo call 03a_BuildSolutions.bat                                 >> ContosoInstall.log
	     call 03a_BuildSolutions.bat                                 >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 03b_PackageSolutions.bat
	echo call 03b_PackageSolutions.bat                               >> ContosoInstall.log
	     call 03b_PackageSolutions.bat                               >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 04a_CreateContosoWebandSsp.bat
	echo call 04a_CreateContosoWebandSsp.bat                         >> ContosoInstall.log
	     call 04a_CreateContosoWebandSsp.bat                         >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 04b1_AddSolutions.bat 
	echo call 04b1_AddSolutions.bat                                  >> ContosoInstall.log
	     call 04b1_AddSolutions.bat                                  >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	REM ==== For farm install (with mulitple WFEs), make sure 
	REM ==== " set DeployLocalOrImmediate=immediate "
	REM ==== in 00_Parameters.bat to use timer job for asynchronous solution deployment.
	REM ==== Do not use " set DeployLocalOrImmediate=local "

	echo call 04b2_DeploySolutions.bat 
	echo call 04b2_DeploySolutions.bat                               >> ContosoInstall.log
	     call 04b2_DeploySolutions.bat                               >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 04c_ActivateAppFeatures.bat
	echo call 04c_ActivateAppFeatures.bat                            >> ContosoInstall.log
	     call 04c_ActivateAppFeatures.bat                            >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log


	echo call 05A_CreatePublishingPortal.bat
	echo call 05A_CreatePublishingPortal.bat                         >> ContosoInstall.log
	     call 05A_CreatePublishingPortal.bat                         >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 05A_CreatePublishingPortal_AddUsers.bat 
	echo call 05A_CreatePublishingPortal_AddUsers.bat                >> ContosoInstall.log
	     call 05A_CreatePublishingPortal_AddUsers.bat                >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 06_CreatePartnerSites.bat 
	echo call 06_CreatePartnerSites.bat                              >> ContosoInstall.log
	     call 06_CreatePartnerSites.bat                              >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 06_CreatePartnerSites_AddUsers.bat 
	echo call 06_CreatePartnerSites_AddUsers.bat                     >> ContosoInstall.log
	     call 06_CreatePartnerSites_AddUsers.bat                     >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 07_CreatePartnerCentral.bat 
	echo call 07_CreatePartnerCentral.bat                            >> ContosoInstall.log
	     call 07_CreatePartnerCentral.bat                            >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 07_CreatePartnerCentral_AddUsers.bat 
	echo call 07_CreatePartnerCentral_AddUsers.bat                   >> ContosoInstall.log
	     call 07_CreatePartnerCentral_AddUsers.bat                   >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 08_ModifyWebconfig.bat 
	echo call 08_ModifyWebconfig.bat                                 >> ContosoInstall.log
	     call 08_ModifyWebconfig.bat                                 >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 09_ImportBDC.bat 
	echo call 09_ImportBDC.bat                                       >> ContosoInstall.log
	     call 09_ImportBDC.bat                                       >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 10_EnableFBA.bat 
	echo call 10_EnableFBA.bat                                       >> ContosoInstall.log
	     call 10_EnableFBA.bat                                       >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 11_RecycleAppPools.bat 
	echo call 11_RecycleAppPools.bat                                 >> ContosoInstall.log
	     call 11_RecycleAppPools.bat                                 >> ContosoInstall.log

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo Automated Installation steps are finished.                  >> ContosoInstall.log    
	echo please continue with the folowing steps                     >> ContosoInstall.log       
	echo in Setup\Supportingfiles\contosoSSLSetup document           >> ContosoInstall.log
	echo to configure SSL for IIS website ContosoServices8585.       >> ContosoInstall.log                 
    echo.                                                            >> ContosoInstall.log

	echo Automated Installation steps are finished.                
	echo Please continue with the folowing steps                           
	echo in Setup\Supportingfiles\contosoSSLSetup document
	echo to configure SSL for IIS website ContosoServices8585.                        
    echo.
    
    pause

	cd %subfolder%
  	if exist "%ProgramFiles%\Adobe"  goto OpenPdfFile
	if exist "%ProgramFiles(X86)%\Adobe"  goto OpenPdfFile
	goto OpenTextFile
	:OpenPdfFile:
	echo start contosoSSLSetup.pdf                                  >> ContosoInstall.log
	start contosoSSLSetup.pdf    
    goto FileOpened
	:OpenTextFile:
	echo start contosoSSLSetup.txt                                  >> ContosoInstall.log
	start contosoSSLSetup.txt    
	:FileOpened

	pause

	cd %subfolder%
	echo.
	echo.                                                            >> ContosoInstall.log
	echo ***********************************************************
	echo *********************************************************** >> ContosoInstall.log
	echo %time%  
	echo %time%                                                      >> ContosoInstall.log

	echo call 11_RecycleAppPools.bat 
	echo call 11_RecycleAppPools.bat                                 >> ContosoInstall.log
	     call 11_RecycleAppPools.bat                                 >> ContosoInstall.log

	echo ***********************************************************************
	echo.
	echo Check Log File: %subfolder%ContosoInstall.log for More detail
	echo.

	Goto End
	:AccessError 
	Echo Unable to process setup, It may be that UAC is Turned on OR you donot have Administrative privilages.
	Echo Please use RUN AS Administrator if UAC is on.
	:End
	pause