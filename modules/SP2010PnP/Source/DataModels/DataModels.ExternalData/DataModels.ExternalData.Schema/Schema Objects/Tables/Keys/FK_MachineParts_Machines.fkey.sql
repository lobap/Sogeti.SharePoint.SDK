ALTER TABLE [dbo].[MachineParts]
    ADD CONSTRAINT [FK_MachineParts_Machines] FOREIGN KEY ([MachineId]) REFERENCES [dbo].[Machines] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

