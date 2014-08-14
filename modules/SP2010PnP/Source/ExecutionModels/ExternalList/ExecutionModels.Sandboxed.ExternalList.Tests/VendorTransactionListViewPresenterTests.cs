//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using ExecutionModels.Sandboxed.ExternalList.VendorTransactionList;
using ExecutionModels.Sandboxed.ExternalList.VendorList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace ExecutionModels.Sandboxed.ExternalList.Tests
{
    [TestClass]
    public class VendorTransactionListViewPresenterTests
    {
        [TestMethod]
        public void SetVendorDetails_SetsData()
        {
            //Arrange
            MockVendorService mockVendorService = new MockVendorService();
            IVendorTransactionListView mockview = new MockVendorTransactionListView();
            VendorTransactionListViewPresenter target = new TestableVendorTransactionListViewPresenter(mockview, mockVendorService);
            string VendorID = "1";
            mockVendorService.GetTransactionByVendorRetVal = new DataTable();
            target.VendorId = VendorID;
            //Act
            target.SetVendorDetails();

            //Assert
            Assert.AreSame(mockview.VendorTransaction, mockVendorService.GetTransactionByVendorRetVal);

        }
    }

    /// <summary>
    /// Sub class to expose testable constructor
    /// </summary>
    class TestableVendorTransactionListViewPresenter : VendorTransactionListViewPresenter
    {
        public TestableVendorTransactionListViewPresenter(IVendorTransactionListView view, IVendorService VendorService)
            : base(view, VendorService)
        {

        }
    }
}


