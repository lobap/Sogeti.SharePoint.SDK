//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace DataModels.ExternalData.PartsManagement.ViewModels
{
    public class BdcMachine
    {
        public int ID { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public string Name { get; set; }
        public string ModelNumber { get; set; }

    }
}
