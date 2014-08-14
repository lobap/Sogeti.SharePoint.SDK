-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPartsBySKU]
	-- Add the parameters for the stored procedure here
	@partSku varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [SKU],
			[Name],
			[Description]
	
	FROM Parts WHERE Parts.SKU = @partSku
	
END