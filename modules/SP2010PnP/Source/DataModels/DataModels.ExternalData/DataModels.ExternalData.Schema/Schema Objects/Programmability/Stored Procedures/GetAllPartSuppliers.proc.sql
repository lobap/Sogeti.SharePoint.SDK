CREATE PROCEDURE [dbo].[GetPartSuppliers]

AS
	SELECT SupplierId, PartSKU, Suppliers.Name, Parts.SKU FROM PartSuppliers 
	INNER JOIN Suppliers ON PartSuppliers.SupplierId = Suppliers.ID 
	INNER JOIN Parts ON PartSuppliers.PartSKU = Parts.SKU

	ORDER BY Suppliers.Name
RETURN 0