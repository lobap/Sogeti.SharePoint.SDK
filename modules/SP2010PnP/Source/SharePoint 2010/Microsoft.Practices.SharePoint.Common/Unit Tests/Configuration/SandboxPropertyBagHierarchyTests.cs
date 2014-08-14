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
    public class SandboxPropertyBagHierarchyTests
    {
        [TestMethod]
        [HostType("Moles")]
        public void ctor_ValidStackBuilt()
        {
            //Arrange
            BSPSite site = new BSPSite();
            var web = site.SetRootWeb();
            web.ServerRelativeUrl = "foo/bar";


            BSPList list = web.Lists.SetOne();
            list.Title = ConfigurationList.ConfigListName;
            web.ID = TestsConstants.TestGuid;
            site.ID = new Guid("{7C039254-10B7-49F0-AA8D-F592206C7130}");
            var moleWeb = new Microsoft.SharePoint.Moles.MSPWeb(web);
            moleWeb.GetListString = (listUrl) =>
            {
                if (listUrl == "foo/bar/Lists/" + ConfigurationList.ConfigListName)
                    return list;
                return null;
            };
            //Act
            var stack = new SandboxPropertyBagHierarchy(web);

            //Assert
            Assert.IsTrue(stack.PropertyBags.Count() == 2);
            Assert.IsInstanceOfType(stack.PropertyBags.First(), typeof(SPWebPropertyBag));
            Assert.IsInstanceOfType(stack.PropertyBags.Skip(1).First(), typeof(SPSitePropertyBag));
        }
    }
}
