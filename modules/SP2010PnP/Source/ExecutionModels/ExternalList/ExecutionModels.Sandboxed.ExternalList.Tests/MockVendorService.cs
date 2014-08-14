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
using ExecutionModels.Sandboxed.ExternalList.VendorList;

namespace ExecutionModels.Sandboxed.ExternalList.Tests
{
    class MockVendorService: IVendorService
    {
        public DataTable GetAllVendorsRetVal { get; set; }
        public DataTable GetTransactionByVendorRetVal { get; set; }
        public DataTable GetAllVendorsWithTransactionCountRetVal { get; set; }

        public DataTable GetAllVendors()
        {
            return GetAllVendorsRetVal;
        }

        public DataTable GetTransactionByVendor(int VendorId)
        {
            return GetTransactionByVendorRetVal;
        }

        public DataTable GetAllVendorsWithTransactionCount()
        {
            return GetAllVendorsWithTransactionCountRetVal;
        }

        public int GetTransactionCountByVendor(int VendorId)
        {
            throw new NotImplementedException();
        }
    }
}


