call 00_Parameters.bat

echo set path to execute VC commands
call setEvn.bat

set path=%path%; %SystemRoot%\Microsoft.NET\Framework\v3.5; %SystemRoot%\Microsoft.NET\Framework\v2.0.50727;

REM *************************************************************************************
REM ************** Check & Create SDK Lib folder if not exists***************************
REM *************************************************************************************
ECHO mkdir "%programfiles%\Microsoft SDKs\Windows\v6.0A\lib"
IF exist "%programfiles%\Microsoft SDKs\Windows\v6.0A\lib"  GOTO :LibExists
cd "%programfiles%\Microsoft SDKs\Windows\v6.0A\"
mkdir "%programfiles%\Microsoft SDKs\Windows\v6.0A\lib"
cd %subfolder%
:LibExists



echo ****** Register Microsoft.Practices.ServiceLocation.dll in GAC
call "%Sourcefolder%Source\Scripts\RegisterInGac.bat" /i "%Sourcefolder%lib\Microsoft.Practices.ServiceLocation.dll"

REM *************************************************************************************
REM ************** Build ContosoSetup.sln ***********************************************
REM *************************************************************************************

ECHO %time%  Status : **** Building ContosoSetupExe Solution using MSBUILD
MSBUILD "%Sourcefolder%Setup\PartnerPortal\SetupSource\ContosoSetup.sln" /t:rebuild /p:Configuration=Debug

REM *************************************************************************************
REM ************** Build Microsoft.Practices.SPG2.sln ***********************************
REM *************************************************************************************

ECHO %time%  Status : **** Building Microsoft.Practices.SPG2.sln with MSBUILD
MSBUILD "%Sourcefolder%Source\Microsoft.Practices.SPG2\Microsoft.Practices.SPG2.sln" /t:rebuild /p:Configuration=Debug

REM *************************************************************************************
REM ************** Build Contoso.PartnerPortal.sln ************************************************
REM *************************************************************************************

ECHO %time%  Status : **** Building RI Solution using MSBUILD
MSBUILD "%Sourcefolder%Source\PartnerPortal\Contoso.PartnerPortal.sln" /t:rebuild /p:Configuration=Debug
 
 
