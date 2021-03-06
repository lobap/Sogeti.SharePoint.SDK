option explicit
' on error resume next

dim objSystem, objAccount
dim args, strArg, x,y
dim strContosoAppPoolAccount,strComputerName,User1,User2,User3,User4,strPartner2Username, strGroupName, strPassword,strUsername,strContosoSharePointAccount, strTrustedServiceGroup, strServiceUserAccount
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
strComputerName = "MyComputer"
strGroupName = "SPGUsers"
User1 ="SandboxSvcAcct"
user4 ="Administrator"
strPassword = "P2ssw0rd$"

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

strComputerName=args(0)
'strPassword =args(1)

'wscript.echo strComputerName
'wscript.echo strpassword

set objSystem = GetObject("WinNT://" & strComputerName)




if(args.count > 0) then


	if (lcase(args(1)) = "clean") then

		wscript.echo "clean"
		   'Delete partner1 group users
			
				Set NewAccount(0) = New Account
				NewAccount(0).UserName = User1

                wscript.echo deleteUser(NewAccount(0))
               
               wscript.echo deleteGroup("ContosoUsers")
	

			wscript.echo "--------------------------------------------------"		
			'wscript.echo deleteGroup(strGroupName & "1")
			wscript.echo "--------------------------------------------------"
		
			
		
		
		wscript.Quit
	else
		wscript.echo "create"
	end if
end if


wscript.echo 
wscript.echo "--------------------------------------------------"

	
		Set NewAccount(0) = New Account
		NewAccount(0).UserName = User1
		NewAccount(0).FullName = User1 
		NewAccount(0).Password = strPassword

		wscript.echo "--------------------------------------------------"
		wscript.echo createNtUser(NewAccount(0))
		wscript.echo addToGroup(NewAccount(0).UserName,"users")
        wscript.echo addToGroup(NewAccount(0).UserName,"WSS_WPG")
        wscript.echo addToGroup(NewAccount(0).UserName,"WSS_Admin_WPG")
        wscript.echo addToGroup(NewAccount(0).UserName,"WSS_Restricted_WPG_V4")
        wscript.echo addToGroup(NewAccount(0).UserName,"IIS_IUSRS")
        wscript.echo addToGroup(NewAccount(0).UserName,"Performance Monitor Users")
        wscript.echo "--------------------------------------------------"
		
        
        wscript.echo createGroup("ContosoUsers")
' -------------------------------------------------------------------------------------



function createGroup(strGroupName)
on error resume next

	'Creating a group in a Windows NT domain
	Dim ObjGroup
	Set objGroup = objSystem.Create("group",strGroupName)
	ObjGroup.SetInfo
	
	if err <> 0 then
		createGroup = "Did not create group: " & strGroupName
        wscript.echo err.Description
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
        wscript.echo err.Description 
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
         wscript.echo err.Description 
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
            wscript.echo err.Description 
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
