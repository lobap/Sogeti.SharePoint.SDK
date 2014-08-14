CREATE PROCEDURE [dbo].[GetSupplierByID]
	@supplierID int = 0 
AS
	SELECT [ID],
			[DUNS],
			[Name] ,
			[Rating] 

	FROM Suppliers WHERE ID=@supplierID
RETURN 0