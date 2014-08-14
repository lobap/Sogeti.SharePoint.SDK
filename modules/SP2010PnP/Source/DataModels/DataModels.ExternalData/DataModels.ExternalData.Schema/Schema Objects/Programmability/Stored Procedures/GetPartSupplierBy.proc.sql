CREATE PROCEDURE [dbo].[GetPartSupplier]
	@partSku varchar(255), 
	@SupplierId int
AS
	SELECT SupplierId, PartSKU, Suppliers.Name, Parts.SKU FROM PartSuppliers 
	INNER JOIN Suppliers ON PartSuppliers.SupplierId = Suppliers.ID 
	INNER JOIN Parts ON PartSuppliers.PartSKU = Parts.SKU
	WHERE PartSuppliers.SupplierId = @SupplierId AND PartSuppliers.PartSKU = @partSku
	
	ORDER BY Suppliers.Name
RETURN 0