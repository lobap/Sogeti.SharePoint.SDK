call 00_Parameters.bat

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
set certmgr=%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\certmgr.exe
set RootCATest_name=ContosoRootCATest
set RootCATest_pvk=%subfolder%ContosoRootCATest.pvk
set RootCATest_cer=%subfolder%ContosoRootCATest.cer
set ContosoServicesUniqueKey=ContosoServicesUniqueKey%ComputerName%

echo RootCATest_cer=%RootCATest_cer%
echo RootCATest_pvk=%RootCATest_pvk%
echo ContosoServicesUniqueKey=%ContosoServicesUniqueKey%

cd %subfolder%
if not exist "%RootCATest_pvk%" goto NextStep1
	del "%RootCATest_pvk%"
:NextStep1
if not exist "%RootCATest_cer%" goto NextStep2
	del "%RootCATest_cer%"
:NextStep2	

echo Cleaning Existing Certificates if any:

call "%PROGRAMFILES%\Microsoft SDKs\Windows\v6.0A\bin\certmgr.exe" -del -r LocalMachine -s Root -CTL -c -n "%RootCATest_name%"

call "%PROGRAMFILES%\Microsoft SDKs\Windows\v6.0A\bin\certmgr.exe" -del -r LocalMachine -s my -CTL -c -n "%computername%"