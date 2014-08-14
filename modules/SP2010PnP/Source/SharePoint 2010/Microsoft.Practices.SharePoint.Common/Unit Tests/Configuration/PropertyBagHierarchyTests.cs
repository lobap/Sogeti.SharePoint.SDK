//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public class PropertyBagHierarchyTests
    {
        [TestMethod]
        public void GetPropertyBagForLevel_WithOnePropertyBag_ReturnsBag()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPWeb));

            //Act
            IPropertyBag bag = hierarchy.GetPropertyBagForLevel(ConfigLevel.CurrentSPWeb); 

            //Assert 
            Assert.IsNotNull(bag);
            Assert.IsTrue(bag.Level == ConfigLevel.CurrentSPWeb);
        }

        [TestMethod]
        public void GetPropertyBagForLevel_WithMultiplePropertyBags_ReturnsBag()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPWeb));
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPSite));
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPWebApplication));
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPFarm));

            //Act
            IPropertyBag farmBag = hierarchy.GetPropertyBagForLevel(ConfigLevel.CurrentSPFarm);
            IPropertyBag webBag = hierarchy.GetPropertyBagForLevel(ConfigLevel.CurrentSPWeb);
            IPropertyBag siteBag = hierarchy.GetPropertyBagForLevel(ConfigLevel.CurrentSPSite);
            IPropertyBag webAppBag = hierarchy.GetPropertyBagForLevel(ConfigLevel.CurrentSPWebApplication);
            //Assert 
            Assert.IsNotNull(farmBag);
            Assert.IsTrue(farmBag.Level == ConfigLevel.CurrentSPFarm);
            Assert.IsNotNull(webBag);
            Assert.IsTrue(webBag.Level == ConfigLevel.CurrentSPWeb);
            Assert.IsNotNull(siteBag);
            Assert.IsTrue(siteBag.Level == ConfigLevel.CurrentSPSite);
            Assert.IsNotNull(webBag);
            Assert.IsTrue(webBag.Level == ConfigLevel.CurrentSPWeb);
        }

        [TestMethod]
        public void GetPropertyBagForMissingLevel_ReturnsNull()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            hierarchy.AddPropertyBag(new BIPropertyBag() { Level = ConfigLevel.CurrentSPFarm });

            //Act
            IPropertyBag bag = hierarchy.GetPropertyBagForLevel(ConfigLevel.CurrentSPWeb);

            //Assert 
            Assert.IsNull(bag);
        }



        private BIPropertyBag GetPropertyBag(ConfigLevel level)
        {
            var propBag = new BIPropertyBag();
            propBag.Values.Count = 0;
            propBag.Level = level;
            return propBag;
        }
    }
}
