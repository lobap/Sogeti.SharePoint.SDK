call 00_Parameters.bat

echo set path to execute VC commands
call setEvn.bat

set path=%path%; %SystemRoot%\Microsoft.NET\Framework\v3.5; %SystemRoot%\Microsoft.NET\Framework\v2.0.50727;


REM *************************************************************************
REM ************** Package WSPs *********************************************
REM *************************************************************************
echo %time%  Status : **** Packaging Contoso.PartnerPortal projects using Extensions
cd %Sourcefolder%Source\PartnerPortal\"
echo "%devenv%" "Contoso.PartnerPortal.sln" /Deploy debug /Package 
"%devenv%" "Contoso.PartnerPortal.sln" /Deploy debug /Package 