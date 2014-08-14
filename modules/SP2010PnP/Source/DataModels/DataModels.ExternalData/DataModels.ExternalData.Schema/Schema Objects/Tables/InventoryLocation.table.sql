/*CREATE TABLE [dbo].[InventoryLocation] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [PartSKU]   VARCHAR (255) NULL,
    [BinNumber] INT           NULL,
    [Quantity]  INT           NULL
);*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AlternateBackShade', @value = N'95', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AlternateBackThemeColorIndex', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AlternateBackTint', @value = N'100', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'BackShade', @value = N'100', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'BackTint', @value = N'100', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'DatasheetForeThemeColorIndex', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'DatasheetGridlinesThemeColorIndex', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'DateCreated', @value = N'3/11/2010 2:46:04 PM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'DisplayViewsOnSharePointSite', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'FilterOnLoad', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'HideNewField', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'LastUpdated', @value = N'3/11/2010 2:47:00 PM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'MS_OrderByOn', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'MS_Orientation', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Name', @value = N'InventoryLocation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'OrderByOnLoad', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'PublishToWeb', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ReadOnlyWhenDisconnected', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'RecordCount', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ThemeFontIndex', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'TotalsRow', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Updatable', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AggregateType', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AppendOnly', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'17', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'CurrencyLCID', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'GUID', @value = N'仨枂흢䡅ƍ进家ꜞ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Name', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ResultType', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'InventoryLocation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'TextAlign', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Type', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'ID';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AggregateType', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AppendOnly', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'CurrencyLCID', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'GUID', @value = N'ࡠퟻ�䄪ꎹ䎣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Name', @value = N'PartSKU', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ResultType', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Size', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'PartSKU', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'InventoryLocation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'TextAlign', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Type', @value = N'10', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'UnicodeCompression', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'PartSKU';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AggregateType', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AppendOnly', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'CurrencyLCID', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'GUID', @value = N'ﯧ숁䂧䫓⾀㼰수怡', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Name', @value = N'BinNumber', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ResultType', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'BinNumber', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'InventoryLocation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'TextAlign', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Type', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'BinNumber';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AggregateType', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'AppendOnly', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'CurrencyLCID', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'GUID', @value = N'ܦ렄豿䋁沐ᵚ゠ॻ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Quantity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'ResultType', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Quantity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'InventoryLocation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'TextAlign', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/


GO
/*EXECUTE sp_addextendedproperty @name = N'Type', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'InventoryLocation', @level2type = N'COLUMN', @level2name = N'Quantity';*/

