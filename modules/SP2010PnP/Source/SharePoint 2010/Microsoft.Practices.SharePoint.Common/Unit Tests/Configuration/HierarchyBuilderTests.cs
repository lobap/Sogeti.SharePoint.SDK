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
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.Practices.SharePoint.Common.Tests.Behaviors;
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public class HierarchyBuilderTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            SharePointEnvironment.Reset();
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetHierarchy_GetsHierarchyForSandbox()
        {
            //Arrange
            var context = new MockAppContextProvider();
            context.IsProxyInstalledRetValue = false;
            context.SetSandbox();
            SharePointEnvironment.ApplicationContextProvider = context;
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
            IPropertyBagHierarchy target = HierarchyBuilder.GetHierarchy(web);
            
            //Assert
            Assert.IsInstanceOfType(target, typeof(SandboxPropertyBagHierarchy));
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetHierarchy_GetsHierarchyForSandbox_WithProxyInstalled()
        {
            //Arrange
            var context = new MockAppContextProvider();
            context.AddProxyType(ProxyArgs.ReadConfigArgs.OperationTypeName);
            context.AddProxyType(ProxyArgs.ProxyInstalledArgs.OperationTypeName);
            context.SetSandbox();
            SharePointEnvironment.ApplicationContextProvider = context;
            var site = new BSPSite();
            BSPWeb web = site.SetRootWeb();

            web.ServerRelativeUrl = "foo/bar";

            BSPList list = web.Lists.SetOne();
            list.Title = ConfigurationList.ConfigListName;
            web.ID = TestsConstants.TestGuid;
            site.ID = new Guid("{7C039254-10B7-49F0-AA8D-F592206C7130}");
            context.SetWeb(web);
            var moleWeb = new Microsoft.SharePoint.Moles.MSPWeb(web);
            moleWeb.GetListString = (listUrl) =>
            {
                if (listUrl == "foo/bar/Lists/" + ConfigurationList.ConfigListName)
                    return list;
                return null;
            };
            //Act
            IPropertyBagHierarchy target = HierarchyBuilder.GetHierarchy(web);

            //Assert
            Assert.IsInstanceOfType(target, typeof(SandboxWithProxyPropertyBagHierarchy));
        }



        [TestMethod]
        [HostType("Moles")]
        public void GetHierarchy_GetsHierarchyForSandboxFarm()
        {
            //Arrange
            var context = new MockAppContextProvider();
            context.AddProxyType(ProxyArgs.ReadConfigArgs.OperationTypeName);
            context.AddProxyType(ProxyArgs.ProxyInstalledArgs.OperationTypeName);
            context.SetSandbox();
            SharePointEnvironment.ApplicationContextProvider = context;
  
            //Act
            IPropertyBagHierarchy target = HierarchyBuilder.GetHierarchy(null);

            //Assert
            Assert.IsInstanceOfType(target, typeof(SandboxFarmPropertyBagHierarchy));
        }


        [TestMethod]
        [HostType("Moles")]
        public void GetHierarchy_GetsHierarchyForFullTrust()
        {
            //Arrange
            var site = new BSPSite();
            var web = site.SetRootWeb();
            var webApp = new BSPWebApplication();
            site.WebApplication = webApp;
            var f = new BSPConfiguredFarm();
            webApp.Farm = SPFarm.Local;

            //Act
            IPropertyBagHierarchy target = HierarchyBuilder.GetHierarchy(web);

            //Assert
            Assert.IsInstanceOfType(target, typeof(FullTrustPropertyBagHierarchy));
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetHierarchy_GetsHierarchyForFarm()
        {
            //Arrange
            var f = new BSPConfiguredFarm();

            //Act
            IPropertyBagHierarchy target = HierarchyBuilder.GetHierarchy(null);

            //Assert
            Assert.IsInstanceOfType(target, typeof(FarmPropertyBagHierarchy));
        }
    }
}
