set gacutil=%programfiles%\Microsoft SDKs\Windows\v6.0A\Bin\gacutil.exe

echo *** RegisterInGac.bat %1 %2
echo * Trying to find GacUtil.exe:

if not exist "%gacutil%" echo * Could not find GACUtil at: %gacutil%  
if not exist "%gacutil%" set gacutil=%ProgramW6432%\Microsoft SDKs\Windows\v6.0A\Bin\gacutil.exe
if not exist "%gacutil%" set gacutil=C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\gacutil.exe
if not exist "%gacutil%" echo * Could not find GACUtil at: %gacutil%
if not exist "%gacutil%" goto error

echo * Using gacutil.exe from %gacutil%

call "%gacutil%" %1 %2
goto done

:error
echo * Warning: Could not (un)register %2 in the gac. 

:done
