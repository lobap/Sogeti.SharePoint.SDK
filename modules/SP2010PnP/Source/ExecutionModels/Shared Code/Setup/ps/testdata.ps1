$newsite="http://"+$HostName+":" + $WebApp.Port+"/sites/"+ $RISite.Name

 # WriteHeading "Adding Test Data to Proejcts List.."
		$RISite.Projects.Project | ForEach-Object {
			$Project =$_
			   $prName= $Project.Name
				#  WriteHeading "$PublishDataExe /Projects $newsite $prName" 
				WriteHeading "Adding Test Data to Projects.."
				if ( $prName -ne "" )
					{	
						# WriteHeading "$PublishDataExe /Projects $newsite $prName" 
						& "$PublishDataExe" /Projects $newsite `"$prName`"
					}

				if ($error) 
					{
						
						writeError $error[0].Exception 
						break;
					}

		}

		WriteHeading "Adding Test Data to Estimates.."
		$RISite.Webs.Web | ForEach-Object {
			$Web =$_
			   $webName= ""+$Web.Name
			if ( $webName -ne "" )
			{
			    WriteHeading "$webName"
			     $Web.Estimates.Estimate| ForEach-Object{
			             $Estimates=$_
			             $estimTitle=$Estimates.Title
					#	 WriteHeading $estimTitle
						 $estimPId=$Estimates.ProjectId
					#	 WriteHeading $estimPId
						 $estimVendorID=$Estimates.VendorId
					#	 WriteHeading $estimVendorID
						 $estimVendorName=$Estimates.VendorName
					#	 WriteHeading $estimVendorName
						 $estimfileName=$Estimates.Name
					#	 WriteHeading $estimfileName
						 $estimEstimateValue=$Estimates.EstimateValue
					#	 WriteHeading $estimEstimateValue
						 $estimcontentType=$Estimates.ContentType
					#    WriteHeading $estimcontentType
						 $estimSOWStatus=$Estimates.SOWStatus
					#	 WriteHeading $estimSOWStatus
						
						 & "$PublishDataExe" /Estimates $newsite $webName $estimfileName $estimTitle $estimcontentType $estimVendorID $estimVendorName $estimEstimateValue $estimSOWStatus $estimPId
			       
			     }
			 }
				

		}

