CREATE TABLE [dbo].[ContactAddresses] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [ContactID]  INT           NOT NULL,
    [Address1]   VARCHAR (100) NULL,
    [Address2]   VARCHAR (50)  NULL,
    [City]       VARCHAR (100) NULL,
    [State]      VARCHAR (50)  NULL,
    [PostalCode] VARCHAR (12)  NULL,
    [Country]    VARCHAR (50)  NULL
);



