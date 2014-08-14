CREATE PROCEDURE [dbo].[GetSupplierContact]
	@contactID int = 0 
AS
	SELECT [ID],
			[SupplierID],
			[FirstName] ,
			[LastName]  ,
			[HomePhone]     ,
			[WorkPhone]     ,
			[MobilePhone]     ,
			[Email],
			[Website]

	FROM SupplierContacts WHERE ID=@contactID
RETURN 0