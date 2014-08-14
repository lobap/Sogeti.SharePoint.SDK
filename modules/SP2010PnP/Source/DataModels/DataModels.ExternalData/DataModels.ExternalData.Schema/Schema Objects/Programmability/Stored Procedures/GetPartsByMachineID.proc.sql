-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE GetPartsByMachineID
	-- Add the parameters for the stored procedure here
	@machineId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Parts.[SKU],
		   Parts.[Name],
		   Parts.[Description]
	
	 FROM Machines 
	INNER JOIN MachineParts ON Machines.ID = MachineParts.MachineId 
	INNER JOIN Parts ON MachineParts.PartSKU = Parts.SKU 
	WHERE Machines.ID = @machineId;
END