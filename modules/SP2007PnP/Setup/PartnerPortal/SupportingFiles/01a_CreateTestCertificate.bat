call 00_Parameters.bat

REM call 901a_CleanTestCertificate.bat

REM ======================================================================================================== 
REM if you don't have Microsoft SDK installed, 
REM you can copy makecert.exe and certmgr.exe to the SupportingFiles folder from a developer PC
REM ======================================================================================================== 

if exist "%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\makecert.exe"  goto UseSDK
:UseLocal
    set makecert=makecert.exe
    set certmgr=certmgr.exe
    goto end
:UseSDK
    set makecert=%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\makecert.exe
    set certmgr=%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\certmgr.exe
:end

set makecert=%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\makecert.exe
set  certmgr=%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\certmgr.exe
set RootCATest_name=ContosoRootCATest
set RootCATest_pvk=%subfolder%ContosoRootCATest.pvk
set RootCATest_cer=%subfolder%ContosoRootCATest.cer
set ContosoServicesUniqueKey=ContosoServicesUniqueKey%ComputerName%

echo RootCATest_cer=%RootCATest_cer%
echo RootCATest_pvk=%RootCATest_pvk%
echo ContosoServicesUniqueKey=%ContosoServicesUniqueKey%

cd %subfolder%

REM ======================================================================================================== 
REM ======= Create and Install Temporary Certificates in WCF for Transport Security During Development =====
REM ======= For a equavalent manual procedure, Please see the following Patterns & practices How to:   =====
REM ======= http://msdn.microsoft.com/en-us/library/cc949067.aspx                                      =====
REM ======================================================================================================== 

REM ==== only run this once in the AppServer
REM ==== you need to enter the same password for all password prompt
echo create %CAPrivateFileName" and %CAPublicFileName%
echo call "%makecert%" -n "CN=%RootCATest_name%" -r -sv "%RootCATest_pvk%" "%RootCATest_cer%"
     call "%makecert%" -n "CN=%RootCATest_name%" -r -sv "%RootCATest_pvk%" "%RootCATest_cer%"

echo add %RootCATest_name% in Certificates (Local Computer)/Trusted Root Certification Authorities    
REM ==== run this in App Server and All WFEs
echo call "%certmgr%" -add "%RootCATest_cer%" -c -s -r LocalMachine Root
     call "%certmgr%" -add "%RootCATest_cer%" -c -s -r LocalMachine Root

REM ==== only run this once in the AppServer
REM ==== need to enter the same password
echo create certificate in Certificates (Local Computer)/Personal, with Issued To = %computername%, Issued By=%RootCATest_name%
echo call "%makecert%" -sk %ContosoServicesUniqueKey% -iv "%RootCATest_pvk%" -n "CN=%computername%" -ic "%RootCATest_cer%" -sr localmachine -ss my -sky exchange -pe
     call "%makecert%" -sk %ContosoServicesUniqueKey% -iv "%RootCATest_pvk%" -n "CN=%computername%" -ic "%RootCATest_cer%" -sr localmachine -ss my -sky exchange -pe
