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
    public static class EntityHelper
    {
        public static string[] SupplierNames = 
        {
            "A. Datum Corporation", 
            "City Power & Light ",
            "Coho Vineyard",
            "Fabrikam, Inc.", 
            "Graphic Design Institute ",
            "Litware, Inc. ",
            "Northwind Traders ",
            "Trey Research"
        };

        public static string[] ContactFirstNames =
        {
            "Kim",
            "Erin M.",
            "Alfons",
            "Hazem",
            "Pernille",
            "Dorena",
            "Luka",
            "Josh",
            "Shu"
 
        };

        public static string[] ContactLastNames =
        {
            "Abercrombie",
            "Hagens",
            "Parovszky",
            "Abolrous",
            "Halberg", 
            "Paschke", 
            "Abrus", 
            "Barnhill", 
            "Ito" 
        };

        public static Contact PopulateContact(int instance, int supplierId)
        {
            Contact contact = new Contact();

            int contactNumber = instance % ContactFirstNames.Length;
            int SupplierNumber = instance % SupplierNames.Length;

            contact.ID = instance;
            contact.FirstName = ContactFirstNames[contactNumber];
            contact.LastName = ContactLastNames[contactNumber];
            contact.HomePhone = "555-555-01" + (instance % 100).ToString();
            contact.WorkPhone = "555-555-01" + (instance % 100).ToString();
            contact.MobilePhone = "555-555-01" + (instance % 100).ToString();
            contact.Email = contact.FirstName + "." + contact.LastName + "@" + SupplierNames[SupplierNumber] + ".com";
            contact.Website = "www." + SupplierNames[SupplierNumber] + ".com";
            contact.SupplierId = supplierId;

            return contact;
        }

        public static Supplier PopulateSupplier(int instance)
        {
            Supplier supplier = new Supplier();
            int SupplierNumber = instance % SupplierNames.Length;

            supplier.SupplierId = instance;
            supplier.Name = SupplierNames[SupplierNumber];
            supplier.DUNS = "DUNS" + instance.ToString("###");
            supplier.Rating = instance;

            return supplier;
        }

        public static ContactAddress PopulateAddress(int instance, int contactId, int supplierID)
        {
            ContactAddress address = new ContactAddress();
            address.Address1 = instance + contactId + supplierID + " My Street";
            address.Address2 = "Unit #" + instance + contactId + supplierID;
            address.City = "City" + instance + contactId + supplierID;
            address.State = "State" + instance + contactId + supplierID;
            address.PostalCode = "0" + instance + contactId + supplierID;
            address.Country = "United States";
            address.ContactID = contactId;
            address.ID = instance;

            return address;
        }
    }
}
