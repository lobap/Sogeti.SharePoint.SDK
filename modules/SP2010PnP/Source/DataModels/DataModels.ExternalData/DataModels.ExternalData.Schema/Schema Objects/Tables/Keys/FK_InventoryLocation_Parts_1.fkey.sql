ALTER TABLE [dbo].[InventoryLocations]
    ADD CONSTRAINT [FK_InventoryLocation_Parts] FOREIGN KEY ([PartSKU]) REFERENCES [dbo].[Parts] ([SKU]) ON DELETE NO ACTION ON UPDATE NO ACTION;

