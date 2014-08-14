rem "%VS90COMNTOOLS%vsvars32.bat"

iisreset -stop

"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -i Contoso.AccountingManagement\bin\debug\Contoso.AccountingManagement.dll
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -i Contoso.HRManagement\bin\debug\Contoso.HRManagement.dll
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -i Contoso.TrainingManagement.Common\bin\debug\Contoso.TrainingManagement.Common.dll
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -i Contoso.TrainingManagement.Repository\bin\debug\Contoso.TrainingManagement.Repository.dll
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -i Contoso.TrainingManagement.ServiceLocator\bin\debug\Contoso.TrainingManagement.ServiceLocator.dll
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -i Contoso.TrainingManagement\bin\debug\Contoso.TrainingManagement.dll

iisreset -start