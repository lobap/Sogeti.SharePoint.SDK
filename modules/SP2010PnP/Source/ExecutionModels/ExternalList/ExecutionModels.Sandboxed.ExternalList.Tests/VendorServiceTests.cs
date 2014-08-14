//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using ExecutionModels.Sandboxed.ExternalList.VendorList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace ExecutionModels.Sandboxed.ExternalList.Tests
{
    
    
    /// <summary>
    ///This is a test class for VendorServiceTest and is intended
    ///to contain all VendorServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VendorServiceTests
    {

        /// <summary>
        ///A test for VendorService Constructor
        ///</summary>
        [TestMethod()]
        public void VendorServiceConstructorTest()
        {
            VendorService target = new VendorService();
            Assert.IsNotNull(target);
        }
    }
}
