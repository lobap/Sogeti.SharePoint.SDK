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
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.SharePoint.Behaviors;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.Practices.SharePoint.Common.Tests.Behaviors;
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public class FullTrustPropertyBagHierarchyTests
    {
        [TestMethod]
        [HostType("Moles")]
        public void ctor_ValidStackBuilt()
        {
            //Arrange
            var site = new BSPSite();
            BSPWeb web = site.SetRootWeb();
            web.Site = site;
            var webApp = new BSPWebApplication();
            site.WebApplication = webApp;
            var f = new BSPConfiguredFarm();

            webApp.Farm = SPFarm.Local;

            //Act
            var stack = new FullTrustPropertyBagHierarchy(web);

            //Assert
            Assert.IsTrue(stack.PropertyBags.Count() == 4);
            Assert.IsInstanceOfType(stack.PropertyBags.First(), typeof(SPWebPropertyBag));
            Assert.IsInstanceOfType(stack.PropertyBags.Skip(1).First(), typeof(SPSitePropertyBag));
            Assert.IsInstanceOfType(stack.PropertyBags.Skip(2).First(), typeof(SPWebAppPropertyBag));
            Assert.IsInstanceOfType(stack.PropertyBags.Skip(3).First(), typeof(SPFarmPropertyBag));
        }
    }
}
