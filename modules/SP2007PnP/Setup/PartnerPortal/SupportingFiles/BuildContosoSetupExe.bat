IF EXIST "%subfolder%\ContosoSetup.exe" GOTO AfterBuildExe
call 00_Parameters.bat

echo set path to execute VC commands
call setEvn.bat

set path=%path%; %SystemRoot%\Microsoft.NET\Framework\v3.5; %SystemRoot%\Microsoft.NET\Framework\v2.0.50727;

ECHO %time%  Status : **** Building ContosoSetupExe Solution using MSBUILD

MSBUILD "%Sourcefolder%Setup\PartnerPortal\SetupSource\ContosoSetup.sln" /t:rebuild /p:Configuration=Debug

:AfterBuildExe
