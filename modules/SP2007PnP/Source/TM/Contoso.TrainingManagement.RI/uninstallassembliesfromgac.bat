rem "%VS90COMNTOOLS%vsvars32.bat"

iisreset -stop

"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -u Contoso.AccountingManagement
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -u Contoso.HRManagement
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -u Contoso.TrainingManagement.Common
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -u Contoso.TrainingManagement.Repository
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -u Contoso.TrainingManagement.ServiceLocator
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -u Contoso.TrainingManagement
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" -u Contoso.TrainingManagement.ContentTypeUpgrade

iisreset -start