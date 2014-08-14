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
    public class SupplierDTO
    {
        public int Id{ get; set; }
        public string SupplierName { get; set; }
        public string DUNS { get; set; }
        public double Rating { get; set; }
    }
}


