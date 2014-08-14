//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Data;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace ExecutionModels.Sandboxed.ExternalList.VendorList
{
    public class VendorService : IVendorService
    {
        public VendorService()
        {

        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public DataTable GetAllVendors()
        {
            var web = SPContext.Current.Web;
            string test = Constants.ectVendorListName;
            var dt = web.Lists[Constants.ectVendorListName].Items.GetDataTable();

            return dt;
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public DataTable GetAllVendorsWithTransactionCount()
        {
            var vendors = GetAllVendors();
            vendors.Columns.Add("TransactionCount");
            var columnIndex = vendors.Columns.Count - 1;
            foreach (DataRow row in vendors.Rows)
            {
                int vendorId = int.Parse(row.ItemArray[0].ToString());
                row[columnIndex] = GetTransactionCountByVendor(vendorId);
            }

            return vendors;
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public int GetTransactionCountByVendor(int vendorId)
        {
            var query = new SPQuery
                        {
                            ViewFields = "<FieldRef Name='ID' />",
                            Query =
                                string.Format(
                                                 "<Where><Eq><FieldRef Name='VendorID' /><Value Type='Counter'>{0}</Value></Eq></Where>",
                                                 vendorId.ToString())
                        };
            return SPContext.Current.Web.Lists[Constants.ectVendorTransactionListName].GetItems(query).Count;
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public DataTable GetTransactionByVendor(int vendorId)
        {
            var query = new SPQuery
                        {
                            ViewFields = "<FieldRef Name='Name' />" +
                                         "<FieldRef Name='TransactionType' />" +

                                         //TODO: This appears to be a bug. Commenting out now, Reaching out for help.
                                // "<FieldRef Name='TransactionDate' />" +
                                         "<FieldRef Name='Amount' />" +
                                         "<FieldRef Name='Notes' />",
                            Query = string.Format(
                                "<Where><Eq><FieldRef Name='VendorID' /><Value Type='Counter'>{0}</Value></Eq></Where>", vendorId.ToString())
                        };

            return SPContext.Current.Web.Lists[Constants.ectVendorTransactionListName].GetItems(query).GetDataTable();

        }
    }
}


