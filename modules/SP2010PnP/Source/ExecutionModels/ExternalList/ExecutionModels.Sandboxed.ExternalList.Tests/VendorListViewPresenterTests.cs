//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Data;
using ExecutionModels.Sandboxed.ExternalList.VendorList;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExecutionModels.Sandboxed.ExternalList.Tests
{
    [TestClass]
    public class VendorListViewPresenterTests
    {

        [TestMethod]
        public void SetVendorDataWithTransactionCount_SetsData()
        {
            //Arrange
            MockVendorService mockVendorService = new MockVendorService();
            IVendorListView mockview = new MockVendorListView();
            VendorListViewPresenter target = new TestableVendorListViewPresenter(mockview, mockVendorService);
            mockVendorService.GetAllVendorsWithTransactionCountRetVal = new DataTable();

            //Act
            target.SetVendorDataWithTransactionCount();

            //Assert
            Assert.AreSame(mockview.VendorDataWithTransactionCount, mockVendorService.GetAllVendorsWithTransactionCountRetVal);
            Assert.IsInstanceOfType(mockview.VendorDataWithTransactionCount, typeof(DataTable));
            Assert.IsNotNull(mockview.VendorDataWithTransactionCount);

        }

    }


    /// <summary>
    /// Sub class to expose testable constructor
    /// </summary>
    class TestableVendorListViewPresenter : VendorListViewPresenter
    {
        public TestableVendorListViewPresenter(IVendorListView view, IVendorService VendorService): base(view, VendorService)
        {
            
        }
    }

    public class TestUtil
    {

    }
}



