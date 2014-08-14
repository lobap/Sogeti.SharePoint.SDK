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
using Supplier.SampleService;

namespace DataModels.ExternalData.PartsManagement.ContactsSystem
{
    /// <summary>
    /// All the methods for retrieving, updating and deleting data are implemented in this class file.
    /// The samples below show the finder and specific finder method for Entity1.
    /// </summary>
    public class BdcContactService
    {
        /// <summary>
        /// This is a sample specific finder method for SupplierContact.
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity1</returns>
        public static BdcContact ReadItem(string contactID)
        {
            using (Service svc = new Service())
            {
                Contact contact = svc.GetContactByID(int.Parse(contactID));

                return ConvertContact(contact);
            }
        }

        /// <summary>
        /// This is a sample finder method for SupplierContact. 
        /// </summary>
        /// <returns>IEnumerable of Entities</returns>
        public static IEnumerable<BdcContact> ReadList()
        {
            using (Service svc = new Service())
            {
                List<Contact> contacts = svc.GetAllContacts();

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
            bdcContact.SupplierID = contact.SupplierId.ToString() ;
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

    }
}
