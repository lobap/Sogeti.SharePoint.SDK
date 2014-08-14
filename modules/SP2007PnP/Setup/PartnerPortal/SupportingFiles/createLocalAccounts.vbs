option explicit
' on error resume next

dim objSystem, objAccount
dim args, strArg, x,y
dim strContosoAppPoolAccount,strComputerName,strPartner1Username,strPartner2Username, strGroupName, strPassword,strUsername,strContosoSharePointAccount, strTrustedServiceGroup, strServiceUserAccount
dim strContosoSpTrustedAccounts
ReDim NewAccount(0)
Class Account
	Public UserName
	Public FullName
	Public Description
	Public Password
End Class



''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'	READ THIS PART AND CARRY OUT THE TASKS NOTED HERE BEFORE RUNNING THE SCRIPT	
'
'	change the following variables
'
' 	strComputername - where the accounts are created - deleted
' 	strGroupName - name of group to contain accounts created or name of group to be deleted 
' 	strPassword - the password to assign to each user created
' 
'	THEN ADD OR MODIFY ACCOUNTS IN setAccountDetails function to your requirements
'
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'
strComputerName = "MOSSVM"
strGroupName = "ContosoWinPartner"
strPartner1Username ="ContosoPartner1User"
strPartner2Username ="ContosoPartner2User"
strPassword = "P2ssw0rd$"
strContosoSharePointAccount = "ContosoWinAdmin"
strTrustedServiceGroup = "ContosoTrustedAccounts"
strServiceUserAccount = "ContosoServiceUser"
strContosoAppPoolAccount="ContosoAppPoolUser"
strContosoSpTrustedAccounts="ContosoSPTrustedAccounts"
'
'
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'
'
'
'
'
'
'
'
'
'
'
'
'
'

set args = wscript.Arguments

strComputerName=args(1)
strPassword = args(2)

set objSystem = GetObject("WinNT://" & strComputerName)




if(args.count > 0) then


	if (lcase(args(0)) = "clean") then

		wscript.echo "clean"
		   'Delete partner1 group users
			for x = 6 to 9
				Set NewAccount(0) = New Account
				NewAccount(0).UserName = strPartner1Username & x
				wscript.echo "--------------------------------------------------"
				wscript.echo deleteUser(NewAccount(0))
			next

			wscript.echo "--------------------------------------------------"		
			wscript.echo deleteGroup(strGroupName & "1")
			wscript.echo "--------------------------------------------------"
		
		'Delete partner2 group users
			for x = 6 to 9
				Set NewAccount(0) = New Account
				NewAccount(0).UserName = strPartner2Username & x
				wscript.echo "--------------------------------------------------"
				wscript.echo deleteUser(NewAccount(0))
			next

			wscript.echo "--------------------------------------------------"		
			wscript.echo deleteGroup(strGroupName & "2")
			wscript.echo "--------------------------------------------------"
			
			wscript.echo "--------------------------------------------------"		
			wscript.echo deleteGroup(strTrustedServiceGroup)
			wscript.echo "--------------------------------------------------"
			wscript.echo "--------------------------------------------------"		
			wscript.echo deleteGroup(strContosoSpTrustedAccounts)
			wscript.echo "--------------------------------------------------"

			
		Set NewAccount(0) = New Account
		NewAccount(0).UserName = strContosoSharePointAccount
		wscript.echo "--------------------------------------------------"
		wscript.echo deleteUser(NewAccount(0))

		Set NewAccount(0) = New Account
		NewAccount(0).UserName = strServiceUserAccount

		wscript.echo "--------------------------------------------------"
		wscript.echo deleteUser(NewAccount(0))
		'delete ContosoAppPoolAccount
		Set NewAccount(0) = New Account
		NewAccount(0).UserName = strContosoAppPoolAccount

		wscript.echo "--------------------------------------------------"
		wscript.echo deleteUser(NewAccount(0))
		
		wscript.Quit
	else
		wscript.echo "create"
	end if
end if


wscript.echo 
wscript.echo "--------------------------------------------------"

	wscript.echo createGroup(strGroupName & "1")
	for x = 6 to 9
		Set NewAccount(0) = New Account
		NewAccount(0).UserName = strPartner1Username & x
		NewAccount(0).FullName = strPartner1Username & x
		NewAccount(0).Description = " user of " & strGroupName & "1 group."
		NewAccount(0).Password = strPassword

		wscript.echo "--------------------------------------------------"
		wscript.echo createNtUser(NewAccount(0))
		wscript.echo addToGroup(NewAccount(0).UserName,strGroupName & "1")
		wscript.echo addToGroup(NewAccount(0).UserName,"users")
	next

  wscript.echo createGroup(strGroupName & "2")
  
	for x = 6 to 9
		Set NewAccount(0) = New Account
		NewAccount(0).UserName = strPartner2Username & x
		NewAccount(0).FullName = strPartner2Username & x
		NewAccount(0).Description = " user of " & strGroupName & "2 group."
		NewAccount(0).Password = strPassword

		wscript.echo "--------------------------------------------------"
		wscript.echo createNtUser(NewAccount(0))
		wscript.echo addToGroup(NewAccount(0).UserName,strGroupName & "2")
		wscript.echo addToGroup(NewAccount(0).UserName,"users")

	next
	
	Set NewAccount(0) = New Account
		NewAccount(0).UserName = strContosoSharePointAccount
		NewAccount(0).FullName = strContosoSharePointAccount
		NewAccount(0).Description = " Contoso Windows Admin Account"
		NewAccount(0).Password = strPassword

		wscript.echo "--------------------------------------------------"
		wscript.echo createNtUser(NewAccount(0))
		wscript.echo addToGroup(NewAccount(0).UserName,"users")
		wscript.echo addToGroup(NewAccount(0).UserName,"IIS_WPG")
		wscript.echo addToGroup(NewAccount(0).UserName,"WSS_WPG")
		wscript.echo addToGroup(NewAccount(0).UserName,"WSS_ADMIN_WPG")
		wscript.echo addToGroup(NewAccount(0).UserName,"WSS_RESTRICTED_WPG")
		wscript.echo addToGroup(NewAccount(0).UserName,strGroupName & "1")
		wscript.echo addToGroup(NewAccount(0).UserName,strGroupName & "2")

wscript.echo "--------------------------------------------------"

        ' Create group for trusted accounts . 
		wscript.echo createGroup(strTrustedServiceGroup)
		wscript.echo addToGroup(NewAccount(0).UserName,strTrustedServiceGroup)
		wscript.echo createGroup(strContosoSpTrustedAccounts)
		
		Set NewAccount(0) = New Account
		NewAccount(0).UserName = strServiceUserAccount
		NewAccount(0).FullName = strServiceUserAccount
		NewAccount(0).Description = " Contoso acount for Web Services"
		NewAccount(0).Password = strPassword

		wscript.echo "--------------------------------------------------"
		wscript.echo createNtUser(NewAccount(0))
		wscript.echo addToGroup(NewAccount(0).UserName,"users")
		wscript.echo addToGroup(NewAccount(0).UserName,"IIS_WPG")
		wscript.echo addToGroup(NewAccount(0).UserName,"WSS_WPG")
		wscript.echo addToGroup(NewAccount(0).UserName,strContosoSpTrustedAccounts)
		wscript.echo addToGroup(NewAccount(0).UserName,strTrustedServiceGroup)
			
		
wscript.echo "--------------------------------------------------"

        ' Create ContosoAppPoolAccount for ContosoWebApp & ContosoSSP AppPools . 
		Set NewAccount(0) = New Account
		NewAccount(0).UserName = strContosoAppPoolAccount
		NewAccount(0).FullName = strContosoAppPoolAccount
		NewAccount(0).Description = " Contoso acount for Contoso WebApp & SSP AppPool Identiry"
		NewAccount(0).Password = strPassword

		wscript.echo "--------------------------------------------------"
		wscript.echo createNtUser(NewAccount(0))
		wscript.echo addToGroup(NewAccount(0).UserName,"users")
		wscript.echo addToGroup(NewAccount(0).UserName,"IIS_WPG")
		wscript.echo addToGroup(NewAccount(0).UserName,"WSS_WPG")
		wscript.echo addToGroup(NewAccount(0).UserName,strGroupName & "1")
		wscript.echo addToGroup(NewAccount(0).UserName,strGroupName & "2")
		wscript.echo addToGroup(NewAccount(0).UserName,strTrustedServiceGroup)
			
		
wscript.echo "--------------------------------------------------"


' -------------------------------------------------------------------------------------



function createGroup(strGroupName)
on error resume next

	'Creating a group in a Windows NT domain
	Dim ObjGroup
	Set objGroup = objSystem.Create("group",strGroupName)
	ObjGroup.SetInfo
	
	if err <> 0 then
		createGroup = "Did not create group: " & strGroupName
	else
		createGroup = "Created group : " & objGroup.Name
	end if
	
on error goto 0
end function


function createNtUser(NewAccount)
on error resume next

	Dim objUser
	set objUser = objSystem.Create("user", NewAccount.UserName)
	objUser.FullName = NewAccount.FullName
	objUser.Description = NewAccount.Description
	objUser.SetPassword NewAccount.Password
	objUser.SetInfo
	
	if err <> 0 then
		createNtUser = NewAccount.UserName & " was not created."
	else
		createNtUser = objUser.Name & " created."
	end if
	
	
on error goto 0
end function


function deleteUser(NewAccount)
on error resume next

	objSystem.Delete "user", NewAccount.UserName
	
	if err <> 0 then
		deleteUser = NewAccount.UserName & " was not deleted."
	else
		deleteUser = NewAccount.UserName & " deleted."
	end if
	
on error goto 0
end function


function deleteGroup(strGroupName)
on error resume next
	objSystem.Delete "group", strGroupName
	
	if err <> 0 then
		deleteGroup = "Did not deleted group : " & strGroupName
	else
		deleteGroup = "Deleted group : " & strGroupName
	end if
	
on error goto 0
end function


function addToGroup(strUserName, strGroupName)
on error resume next
	
	Dim oGroup
	Set oGroup = objSystem.GetObject("Group", strGroupName)
	oGroup.Add ("WinNT://" & strComputerName & "/" & strUserName)
	Set oGroup=Nothing
	
	if err <> 0 then
		addToGroup = "Did not Add " & strUserName & " to group " & strGroupName
	else
		addToGroup = "Added " & strUserName & " to group " & strGroupName
	end if
	
on error goto 0
end function
