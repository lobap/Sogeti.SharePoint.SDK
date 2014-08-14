set stscmd=C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\12\BIN\stsadm.exe

"%stscmd%" -o retractsolution -name Contoso.LOB.Services.Client.wsp -local
"%stscmd%" -o retractsolution -name Contoso.PSS.Administration.wsp -local -allcontenturls
"%stscmd%" -o retractsolution -name Contoso.PSS.GlobalNavigation.wsp -local
"%stscmd%" -o retractsolution -name Contoso.PSS.Collaboration.wsp -local -allcontenturls
"%stscmd%" -o retractsolution -name Contoso.PSS.Incident.wsp -local -allcontenturls
"%stscmd%" -o retractsolution -name Contoso.PSS.OrderException.wsp -local -allcontenturls
"%stscmd%" -o retractsolution -name Contoso.PSS.Portal.wsp -local -allcontenturls

"%stscmd%" -o deletesolution -name Contoso.LOB.Services.Client.wsp
"%stscmd%" -o deletesolution -name Contoso.PSS.Administration.wsp
"%stscmd%" -o deletesolution -name Contoso.PSS.GlobalNavigation.wsp 
"%stscmd%" -o deletesolution -name Contoso.PSS.Collaboration.wsp 
"%stscmd%" -o deletesolution -name Contoso.PSS.Incident.wsp
"%stscmd%" -o deletesolution -name Contoso.PSS.OrderException.wsp
"%stscmd%" -o deletesolution -name Contoso.PSS.Portal.wsp

pushd Contoso.LOB.Services.Client\bin\debug
"%stscmd%" -o addsolution -filename Contoso.LOB.Services.Client.wsp
"%stscmd%" -o deploysolution -name Contoso.LOB.Services.Client.wsp -local -allowgacdeployment
popd

pushd Contoso.PSS.Administration\bin\debug
"%stscmd%" -o addsolution -filename Contoso.PSS.Administration.wsp
"%stscmd%" -o deploysolution -name Contoso.PSS.Administration.wsp -local -allowgacdeployment -url http://localhost
popd

pushd COntoso.PSS.GlobalNavigation\bin\debug
"%stscmd%" -o addsolution -filename Contoso.PSS.GlobalNavigation.wsp
"%stscmd%" -o deploysolution -name Contoso.PSS.GlobalNavigation.wsp -local -allowgacdeployment
popd

pushd COntoso.PSS.Collaboration\bin\debug
"%stscmd%" -o addsolution -filename Contoso.PSS.Collaboration.wsp
"%stscmd%" -o deploysolution -name Contoso.PSS.Collaboration.wsp -local -allowgacdeployment -url http://localhost
popd

pushd COntoso.PSS.Incident\bin\debug
"%stscmd%" -o addsolution -filename COntoso.PSS.Incident.wsp
"%stscmd%" -o deploysolution -name COntoso.PSS.Incident.wsp -local -allowgacdeployment -url http://localhost
popd

pushd COntoso.PSS.OrderException\bin\debug
"%stscmd%" -o addsolution -filename COntoso.PSS.OrderException.wsp
"%stscmd%" -o deploysolution -name COntoso.PSS.OrderException.wsp -local -allowgacdeployment -url http://localhost
popd

pushd COntoso.PSS.Portal\bin\debug
"%stscmd%" -o addsolution -filename COntoso.PSS.Portal.wsp
"%stscmd%" -o deploysolution -name COntoso.PSS.Portal.wsp -local -allowgacdeployment -url http://localhost
popd