﻿ALTER TABLE [dbo].[PartSuppliers]
    ADD CONSTRAINT [aaaaaPartSupplier_PK] PRIMARY KEY NONCLUSTERED ([PartSKU] ASC, [SupplierId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];
