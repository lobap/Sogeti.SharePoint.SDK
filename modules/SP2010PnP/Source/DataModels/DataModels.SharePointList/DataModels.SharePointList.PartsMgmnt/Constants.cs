//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace DataModels.SharePointList.PartsMgmnt
{
    public static class Constants
    {
        public static readonly string Sku = "SKU";
        public static readonly string PartColumnName = "Parts";
        public static readonly string PartsMgmntLibrary = "PartsMgmntPages";
        public static readonly int DeafultPageSize = 10;

        public static class EmptyData
        {
            public static readonly string PartResults = "-- No Parts Matching that SKU --";
            public static readonly string MachineCategoryResults = "-- No Machines in that Category --";
            public static readonly string MachineDepartmentResults = "-- No Machines for that Department --";
            public static readonly string MachineManufacturerResults = "-- No Machines for that Manufacturer --";
            public static readonly string MachinePartResults = "-- No Parts for that Machine --";
            public static readonly string MachineResults = "-- No Machines Matching that Model Number  --";
            public static readonly string ManufacturerResults = "-- No Manufacturers Matching that Name --";
            public static readonly string CategoryResults = "-- No Categories Matching that Name --";
            public static readonly string DepartmentResults = "-- No Departments Matching that Name --";
            public static readonly string SupplierResults = "-- No Suppliers Matcing that Name --";
            public static readonly string PartSupplierResults = "-- No Parts for the Selected Supplier --";
            public static readonly string PartLocationResults = "-- No Parts for that Location --";
        }
    }
}
