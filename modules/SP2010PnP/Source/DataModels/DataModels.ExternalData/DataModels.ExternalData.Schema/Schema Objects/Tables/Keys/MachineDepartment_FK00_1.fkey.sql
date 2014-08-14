ALTER TABLE [dbo].[MachineDepartments]
    ADD CONSTRAINT [MachineDepartment_FK00] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Departments] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

