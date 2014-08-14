//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.Model
{
    public static class Constants
    {
        public static class ListUrls
        {
            public const string Categories = "Lists/Categories";
            public const string Departments = "Lists/Departments";
            public const string InventoryLocations = "Lists/InventoryLocations";
            public const string MachineDepartments = "Lists/MachineDepartments";
            public const string MachineParts = "Lists/MachineParts";
            public const string Machines ="Lists/Machines";
            public const string Manufacturers = "Lists/Manufacturers";
            public const string PartSuppliers = "Lists/PartSuppliers";
            public const string Parts = "Lists/Parts";
            public const string Suppliers = "Lists/Suppliers";
        }

        public static class Fields
        {
            public static class InternalName
            {
                public const string BinNumber = "BinNumber";
                public const string CategoryName = "CategoryName";
                public const string DepartmentNumber = "DepartmentNumber";
                public const string DUNS = "DUNS";
                public const string ManufacturerAddress = "ManufacturerAddress";
                public const string ModelNumber = "ModelNumber";
                public const string Quantity = "Quantity";
                public const string Rating = "Rating";
                public const string SKU = "SKU";
                public const string Department = "DepartmentLookup";
                public const string Category = "CategoryLookup";
                public const string Machine = "MachineLookup";
                public const string Manufacturer = "ManufacturerLookup";
                public const string Part = "PartLookup";
                public const string Description = "PartsDescription";
                public const string Supplier = "SupplierLookup";
            }

            public static class Guids
            {
                public static readonly Guid BinNumber = new Guid("{9856789e-6f40-4479-8e49-34a85ed863cf}");
                public static readonly Guid CategoryName = new Guid("{25d6d370-732a-4325-b5d9-64451f245e8c}");
                public static readonly Guid DepartmentNumber = new Guid("{c572ec42-3ba0-416d-a91c-60352e51f103}");
                public static readonly Guid DUNS = new Guid("{435bc5ba-2019-45e1-90d7-7b03b0a38e79}");
                public static readonly Guid ManufacturerAddress = new Guid("{f2290cdf-1893-466f-b38c-b5ca0888370a}");
                public static readonly Guid ModelNumber = new Guid("{0ed6bdc7-d508-4dc8-8ab5-c2c4ffe148fc}");
                public static readonly Guid Quantity = new Guid("{499d922b-e331-4330-8fc1-eff0106a54f6}");
                public static readonly Guid Rating = new Guid("{a0e35d31-3b02-44a4-9d1a-41491608e428}");
                public static readonly Guid SKU = new Guid("{55334a5e-9810-4a11-87a6-3e420c014b0a}");
                public static readonly Guid Department = new Guid("{295168ac-29a6-4bb1-91c6-ced3bcf5f086}");
                public static readonly Guid Category = new Guid("{2da96196-e156-46bd-92ef-1a262569a18b}");
                public static readonly Guid Machine = new Guid("{322e5c46-da10-4948-b3ab-dc657bc51a4a}");
                public static readonly Guid Manufacturer = new Guid("{9d2b7421-2ac0-42db-8dd7-847ded5e9049}");
                public static readonly Guid Part = new Guid("{4962bb01-d4a4-409d-895c-fd412baa8293}");
                public static readonly Guid Supplier = new Guid("{3eb6e763-48dd-48a4-ab98-3d211b090d0f}");
                public static readonly Guid Description = new Guid("{42DF0D4D-1B95-47C4-BB86-5F71F1FCFBED}");
            }
        }

        public class ContentTypes
        {
            public readonly SPContentTypeId Category = new SPContentTypeId("0x01001BE67949DD484746B132BFA579DE4D1A");
            public readonly SPContentTypeId Department = new SPContentTypeId("0x01004F9943253ED2684E90E2BC4FCD34A54B");
            public readonly SPContentTypeId InventoryLocation = new SPContentTypeId("0x01003ADD73260584374BA9AF545AB6ECF52B");
            public readonly SPContentTypeId MachineDepartment = new SPContentTypeId("0x0100F8A89D8611A0E8439DC1395EE949C21A");
            public readonly SPContentTypeId MachinePart = new SPContentTypeId("0x0100220B06426A421E41A0CA50F1FA1F421F");
            public readonly SPContentTypeId Machine = new SPContentTypeId("0x01002131F2F6C84FE042A765CDC2C96765F4");
            public readonly SPContentTypeId Manufacturer = new SPContentTypeId("0x0100AE7CD2D2A3D4BC469EB4E95692B750F3");
            public readonly SPContentTypeId PartSupplier = new SPContentTypeId("0x0100679072FB68AF53409FD9F7E845B4D053");
            public readonly SPContentTypeId Part = new SPContentTypeId("0x01001966A9D6EDFEB845A8DD2DDA365BF5DC");
            public readonly SPContentTypeId Supplier = new SPContentTypeId("0x0100C8F084CAA9580A4CB3E1B290909459F6");
        }
    }
}
