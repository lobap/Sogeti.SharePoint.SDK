//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Collections.Generic;
using System.Linq;
using Supplier.SampleService;

namespace DataModels.ExternalData.PartsManagement.ContactsSystem
{
    public partial class BdcSupplierService
    {
        public static BdcSupplier ReadItem(string supplierID)
        {
            using (Service svc = new Service())
            {
                Supplier.SampleService.Supplier supplier = svc.GetSupplierByID(int.Parse(supplierID));
                return ConvertSupplier(supplier);
            }
        }

        public static IEnumerable<BdcContact> BdcSupplierToBdcContact(string supplierId)
        {
            using (Service svc = new Service())
            {
                List<Contact> contacts = svc.GetContactsBySupplierID(int.Parse(supplierId));
                List<BdcContact> supplierContacts = new List<BdcContact>();

                foreach (Contact contact in contacts)
                {
                    supplierContacts.Add(ConvertContact(contact));
                }

                return supplierContacts;
            }
        }

        private static BdcContact ConvertContact(Contact contact)
        {
            BdcContact bdcContact = new BdcContact();

            bdcContact.Identifier1 = contact.ID.ToString();
            bdcContact.SupplierID = contact.SupplierId.ToString();
            bdcContact.DisplayName = contact.FirstName + " " + contact.LastName;
            bdcContact.PrimaryPhone = contact.WorkPhone;
            bdcContact.SecondaryPhone = contact.MobilePhone;
            bdcContact.OtherPhone = contact.HomePhone;
            bdcContact.Email = contact.Email;
            bdcContact.Website = contact.Website;

            if (contact.ContactAddresses != null && contact.ContactAddresses.Count() > 0)
            {
                bdcContact.Address1 = contact.ContactAddresses[0].Address1;
                bdcContact.Address2 = contact.ContactAddresses[0].Address2;
                bdcContact.City = contact.ContactAddresses[0].City;
                bdcContact.PostalCode = contact.ContactAddresses[0].PostalCode;
                bdcContact.State = contact.ContactAddresses[0].State;
                bdcContact.Country = contact.ContactAddresses[0].Country;
            }
            return bdcContact;
        }
        
        public static BdcSupplier ConvertSupplier (Supplier.SampleService.Supplier supplier)
        {
            BdcSupplier bdcSupplier = new BdcSupplier();
            bdcSupplier.DUNS = supplier.DUNS;
            bdcSupplier.Name = supplier.Name;
            bdcSupplier.Rating = supplier.Rating;
            bdcSupplier.SupplierID = supplier.SupplierId.ToString();

            return bdcSupplier;
        }

        public static IEnumerable<BdcSupplier> ReadList()
        {
            using (Service svc = new Service())
            {
                List<BdcSupplier> bdcSuppliers = new List<BdcSupplier>();
                List<Supplier.SampleService.Supplier> suppliers = svc.GetAllSuppliers();

                foreach (var supplier in suppliers)
                {
                    bdcSuppliers.Add(ConvertSupplier(supplier)); 
                }
                return bdcSuppliers.AsEnumerable();
            }
        }

        public static IEnumerable<BdcSupplier> ReadList2(string supplierName)
        {
            using (Service svc = new Service())
            {
                List<BdcSupplier> bdcSuppliers = new List<BdcSupplier>();
                List<Supplier.SampleService.Supplier> suppliers = svc.GetSuppliersByName(supplierName);

                foreach (var supplier in suppliers)
                {
                    bdcSuppliers.Add(ConvertSupplier(supplier));
                }
                return bdcSuppliers.AsEnumerable();
            }
        }
    }
}
