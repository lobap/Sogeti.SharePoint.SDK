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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Behaviors;
using Microsoft.Practices.SharePoint.Common.Configuration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public class SandboxFarmProxyPropertyBagHierarchyTests
    {
        [TestMethod]
        [HostType("Moles")]
        public void ctor_ValidStackBuilt()
        {
            //Arrange
 
            //Act
            var stack = new SandboxFarmPropertyBagHierarchy();

            //Assert
            Assert.IsTrue(stack.PropertyBags.Count() == 1);
            Assert.IsInstanceOfType(stack.PropertyBags.First(), typeof(SandboxFarmPropertyBag));
        }
    }
}
