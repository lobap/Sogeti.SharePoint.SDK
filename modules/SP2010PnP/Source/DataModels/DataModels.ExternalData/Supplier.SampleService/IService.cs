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

namespace Supplier.SampleService
{
    interface IService : IDisposable
    {
        Contact GetContactByID(int contactId);

        Supplier GetSupplierByID(int supplierId);

        List<Supplier> GetAllSuppliers();

        List<Supplier> GetSuppliersByName(string supplierName);

        List<Contact> GetContactsBySupplierID(int supplierId);

        List<Contact> GetAllContacts();
    }
}
