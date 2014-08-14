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
    public class Service : IService
    {
        private List<Contact> AllContacts = new List<Contact>();
        private List<Supplier> AllSuppliers = new List<Supplier>();
        private List<ContactAddress> AllAddresses = new List<ContactAddress>();

        public Service()
        {
            LoadAllData();
        }

        private void LoadAllData()
        {
            for (int i = 0; i < 100; i++)
            {
                Supplier supplier = EntityHelper.PopulateSupplier(i);
                for (int j = 0; j < 5; j++)
                {
                    Contact contact = EntityHelper.PopulateContact(j, i);
                    contact.ContactAddresses = new List<ContactAddress>();
                    for (int k = 0; k < 2; k++)
                    {
                        ContactAddress address = EntityHelper.PopulateAddress(k, j, i);
                        contact.ContactAddresses.Add(address);
                        AllAddresses.Add(address);
                    }
                    AllContacts.Add(contact);
                }
                AllSuppliers.Add(supplier);
            }
        }

        public List<Contact> GetContactsBySupplierID(int supplierId)
        {
            return AllContacts.Where(p => p.SupplierId == supplierId).ToList();
        }

        public Contact GetContactByID(int contactId)
        {
            return AllContacts.FirstOrDefault(p => p.ID == contactId);
        }
        public Supplier GetSupplierByID(int supplierId)
        {
            return AllSuppliers.FirstOrDefault(p => p.SupplierId == supplierId);
        }

        public List<Supplier> GetAllSuppliers()
        {
            return AllSuppliers;
        }

        public List<Supplier> GetSuppliersByName(string supplierName)
        {
            return AllSuppliers.Where(p => p.Name.ToLower().StartsWith(supplierName.ToLower())).ToList();
        }

        public List<Contact> GetAllContacts()
        {
            return AllContacts;
        }

        public List<ContactAddress> GetAddressesByContact(int contactId)
        {
            return AllAddresses.Where(p => p.ContactID == contactId).ToList();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                AllSuppliers = null;
                AllContacts = null;
                AllAddresses = null;
            }
        }

    }
}
