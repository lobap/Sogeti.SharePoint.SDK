CREATE PROCEDURE [dbo].[GetAllSuppliers]
	@supplierID int = 0 
AS
	SELECT [ID],
			[DUNS],
			[Name] ,
			[Rating] 

	FROM Suppliers 
RETURN 0