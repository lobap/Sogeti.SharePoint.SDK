ALTER TABLE [dbo].[PartSuppliers]
    ADD CONSTRAINT [PartSupplier_FK01] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Suppliers] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;



