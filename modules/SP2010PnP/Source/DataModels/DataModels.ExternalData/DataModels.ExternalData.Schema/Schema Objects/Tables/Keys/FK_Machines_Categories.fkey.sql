ALTER TABLE [dbo].[Machines]
    ADD CONSTRAINT [FK_Machines_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;









