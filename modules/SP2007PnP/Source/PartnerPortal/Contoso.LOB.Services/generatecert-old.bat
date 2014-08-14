
"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\makecert.exe" -sr LocalMachine -ss Root -a sha1 -n CN=localhost -sky exchange -pe
"%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin\certmgr.exe" -add -r LocalMachine -s Root -c -n localhost -r CurrentUser -s TrustedPeople