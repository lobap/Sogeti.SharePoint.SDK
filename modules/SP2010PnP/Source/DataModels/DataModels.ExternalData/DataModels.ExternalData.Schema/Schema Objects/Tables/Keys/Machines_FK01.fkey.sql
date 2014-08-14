ALTER TABLE [dbo].[Machines]
    ADD CONSTRAINT [Machines_FK01] FOREIGN KEY ([ManufacturerId]) REFERENCES [dbo].[Manufacturers] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;



