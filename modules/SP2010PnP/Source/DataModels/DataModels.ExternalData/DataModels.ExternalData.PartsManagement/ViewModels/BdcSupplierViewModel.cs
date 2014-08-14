//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModels.ExternalData.PartsManagement.ContactsSystem
{
    /// <summary>
    /// This class contains the properties for Entity1. The properties keep the data for Entity1.
    /// If you want to rename the class, don't forget to rename the entity in the model xml as well.
    /// </summary>
    public partial class BdcSupplierViewModel
    {
        public BdcSupplierViewModel()
        {
        }

        public string SupplierID { get; set; }
        public string DUNS { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
    }
}
