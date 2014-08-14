//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace DataModels.SharePointList.PartsMgmnt.DTOs
{
    public class PartInventoryDTO
    {
        public int PartId{ get; set; }
        public string PartName { get; set; }
        public string Sku { get; set; }
        public int InventoryLocationId { get; set; }
        public string LocationBin { get; set; }
        public double InventoryQuantity { get; set; }
  
    }
}


