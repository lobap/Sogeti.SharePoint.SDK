CREATE TABLE [dbo].[InventoryLocations] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [PartSKU]   VARCHAR (255) NULL,
    [BinNumber] INT           NULL,
    [Quantity]  INT           NULL
);

