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
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.UserCode;
using Microsoft.Practices.SharePoint.Common.Tests.Mocks;
using Microsoft.Practices.SharePoint.Common.Configuration;

namespace Microsoft.Practices.SharePoint.Common.Tests
{
    [TestClass]
    public class SharePointEnvironmentTests
    {
        [TestCleanup]
        public void Teardown()
        {
            SharePointEnvironment.Reset();
        }

        [TestMethod]
        public void InSandBox_ReturnsTrue_WithSandBoxInAppDomainName()
        {
            SharePointEnvironment.Reset();
            MockApplicationContextProvider.Reset();
            MockApplicationContextProvider appContextProvider = new MockApplicationContextProvider();
            SharePointEnvironment.ApplicationContextProvider = appContextProvider;
            MockApplicationContextProvider.AppDomainFriendlyName = "MySandboxAppDomain";

            Assert.IsTrue(SharePointEnvironment.InSandbox);
        }

        [TestMethod]
        public void InSandBox_ReturnsFalse_WithoutSandBoxInAppDomainName()
        {
            SharePointEnvironment.Reset();
            MockApplicationContextProvider.Reset();
            MockApplicationContextProvider appContextProvider = new MockApplicationContextProvider();
            SharePointEnvironment.ApplicationContextProvider = appContextProvider;
            MockApplicationContextProvider.AppDomainFriendlyName = "MyAppDomain";

            Assert.IsFalse(SharePointEnvironment.InSandbox);
        }

        [TestMethod]
        [HostType("Moles")]
        public void CanAccessFarm_ReturnsTrue_WhenSPFarmLocalNotNull()
        {
            SharePointEnvironment.Reset();
            MockApplicationContextProvider.Reset();
            MockApplicationContextProvider appContextProvider = new MockApplicationContextProvider();
            SharePointEnvironment.ApplicationContextProvider = appContextProvider;
            MockApplicationContextProvider.SPFarmLocal = new MSPFarm();

            Assert.IsTrue(SharePointEnvironment.CanAccessFarm);
        }

        [TestMethod]
        public void CanAccessFarm_ReturnsFalse_WhenSPFarmLocalNull()
        {
            SharePointEnvironment.Reset();
            MockApplicationContextProvider.Reset();
            MockApplicationContextProvider appContextProvider = new MockApplicationContextProvider();
            SharePointEnvironment.ApplicationContextProvider = appContextProvider;
            MockApplicationContextProvider.SPFarmLocal= null;

            Assert.IsFalse(SharePointEnvironment.CanAccessFarm);
        }

        [TestMethod]
        public void CanAccessSharePoint_ReturnsTrue_WithSandBoxInAppDomainName()
        {
            SharePointEnvironment.Reset();
            MockApplicationContextProvider.Reset();
            MockApplicationContextProvider appContextProvider = new MockApplicationContextProvider();
            SharePointEnvironment.ApplicationContextProvider = appContextProvider;
            MockApplicationContextProvider.AppDomainFriendlyName = "MySandboxAppDomain";

            Assert.IsTrue(SharePointEnvironment.CanAccessSharePoint);
        }

        [TestMethod]
        [HostType("Moles")]
        public void CanAccessSharePoint_ReturnsTrue_WhenNotInSandboxButSPFarmLocalNotNull()
        {
            SharePointEnvironment.Reset();
            MockApplicationContextProvider.Reset();
            MockApplicationContextProvider appContextProvider = new MockApplicationContextProvider();
            SharePointEnvironment.ApplicationContextProvider = appContextProvider;
            MockApplicationContextProvider.AppDomainFriendlyName = "MyAppDomain";
            MockApplicationContextProvider.SPFarmLocal = new MSPFarm();

            Assert.IsTrue(SharePointEnvironment.CanAccessSharePoint);
        }

        [TestMethod]
        public void CanAccessSharePoint_ReturnsFalse_WhenNotInSandboxAndSPFarmLocalNull()
        {
            SharePointEnvironment.Reset();
            MockApplicationContextProvider.Reset();
            MockApplicationContextProvider appContextProvider = new MockApplicationContextProvider();
            SharePointEnvironment.ApplicationContextProvider = appContextProvider;
            MockApplicationContextProvider.AppDomainFriendlyName = "MyAppDomain";
            MockApplicationContextProvider.SPFarmLocal = null;

            Assert.IsFalse(SharePointEnvironment.CanAccessSharePoint);
        }

        [TestMethod]
        public void ProxyInstalled_ReturnsTrue_WhenExecuteProxyOperationReturnsTrue()
        { 
            SharePointEnvironment.Reset();
            MockApplicationContextProvider.Reset();
            MockApplicationContextProvider appContextProvider = new MockApplicationContextProvider();
            SharePointEnvironment.ApplicationContextProvider = appContextProvider; 
            MockApplicationContextProvider.ExecuteRegisteredProxyOperationRetVal = true;

            Assert.IsTrue(SharePointEnvironment.ProxyInstalled("namespaceForProxy", "typeForProxy"));
        }

        [TestMethod]
        public void ProxyInstalled_ReturnsFalse_WhenApplicationContextReturnsFalse()
        {
            SharePointEnvironment.Reset();
            MockApplicationContextProvider.Reset();
            MockApplicationContextProvider appContextProvider = new MockApplicationContextProvider();
            SharePointEnvironment.ApplicationContextProvider = appContextProvider;
            MockApplicationContextProvider.IsProxyCheckerInstalledRetVal = false;

            Assert.IsFalse(SharePointEnvironment.ProxyInstalled("namespaceForProxy", "typeForProxy"));
        }

        [TestMethod]
        public void ProxyInstalled_CachesRetVal_IfPreviouslyDeterminedFalse()
        {
            SharePointEnvironment.Reset();
            MockApplicationContextProvider.Reset();
            MockApplicationContextProvider appContextProvider = new MockApplicationContextProvider();
            SharePointEnvironment.ApplicationContextProvider = appContextProvider;
            MockApplicationContextProvider.IsProxyCheckerInstalledRetVal = false;

            Assert.IsFalse(SharePointEnvironment.ProxyInstalled("namespaceForProxy", "typeForProxy"));

            MockApplicationContextProvider.IsProxyCheckerInstalledRetVal = true;
            Assert.IsFalse(SharePointEnvironment.ProxyInstalled("namespaceForProxy", "typeForProxy"));
        }

    }

}
