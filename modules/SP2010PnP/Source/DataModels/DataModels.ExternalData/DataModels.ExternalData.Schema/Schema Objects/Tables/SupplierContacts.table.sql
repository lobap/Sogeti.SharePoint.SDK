CREATE TABLE [dbo].[SupplierContacts] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [SupplierID]  INT           NOT NULL,
    [FirstName]   VARCHAR (50)  NOT NULL,
    [LastName]    VARCHAR (50)  NOT NULL,
    [HomePhone]   VARCHAR (15)  NOT NULL,
    [WorkPhone]   VARCHAR (15)  NULL,
    [MobilePhone] VARCHAR (15)  NULL,
    [Website]     VARCHAR (150) NULL,
    [Email]       VARCHAR (50)  NULL
);








