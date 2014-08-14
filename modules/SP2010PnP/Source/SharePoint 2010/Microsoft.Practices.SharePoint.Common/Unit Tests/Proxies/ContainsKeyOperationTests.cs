//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy;
using Microsoft.Practices.SharePoint.Common.Tests;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint.Moles;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.Practices.SharePoint.Common.Tests.Behaviors;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SharePoint.Common.Tests.Configuration;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Logging;

namespace Microsoft.Practices.SharePoint.Common.ConfigProxy.Tests
{
    [TestClass]
    public class ContainsKeyOperationTests
    {
        [TestInitialize]
        public void Initialize()
        {
            var logger = new MockLogger();
            var locator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(locator);
            locator.RegisterTypeMapping<ILogger, MockLogger>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            SharePointServiceLocator.Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Execute_ThrowsArgumentNullException_WhenArgsAreNull()
        {
            var proxyOp = new ContainsKeyOperation();

            //Act 
            proxyOp.Execute(null);

            //Assert caught by exception...
        }

        [TestMethod]
        public void Execute_ReturnsConfigurationException_WhenSiteIdIsEmpty()
        {
            var proxyOp = new ContainsKeyOperation();
            var args = new ContainsKeyDataArgs();
            args.Key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName; 
            args.Level = (int)ConfigLevel.CurrentSPWebApplication;

            //Act 
            object result = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ConfigurationException));
        }

        [TestMethod]
        public void Execute_ReturnsArgumentNullException_WhenKeyIsNull()
        {
            var proxyOp = new ContainsKeyOperation();
            var args = new ContainsKeyDataArgs();
            args.Key = null;
            args.SiteId = TestsConstants.TestGuid;
            args.Level = (int)ConfigLevel.CurrentSPWebApplication;

            //Act 
            object result = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void Execute_ReturnsConfigurationException_WhenWrongArgTypeProvided()
        {
            var proxyOp = new ContainsKeyOperation();
            var args = new ReadConfigArgs();

            //Act 
            object result = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ConfigurationException));
        }


        [TestMethod]
        public void Execute_ReturnsAConfigurationException_WhenKeyDoesntHaveNamespace()
        {
            //arrange
            var args = new ContainsKeyDataArgs();
            args.Key = TestsConstants.TestGuidName;
            args.Level = (int) ConfigLevel.CurrentSPWebApplication;
            args.SiteId = TestsConstants.TestGuid;

            var proxyOp = new ContainsKeyOperation();

            //Act 
            var ex = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(ex, typeof(ConfigurationException));
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_ContainsKeyReturnsTrue_WithValidKeyAtWebAppLevel()
        {
            //arrange
            SPWebAppPropertyBag.ClearCache();
            var args = new ContainsKeyDataArgs();
            string key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            args.Key = key;
            args.Level = (int)ConfigLevel.CurrentSPWebApplication;
            args.SiteId = TestsConstants.TestGuid;
            var proxyOp = new ContainsKeyOperation();
            string expectedData = "{92700BB6-B144-434F-A97B-5F696068A425}";

            var webApp = new BSPWebApplication();
            WebAppSettingStore wss = new WebAppSettingStore();
            var webPO = new MSPPersistedObject((SPPersistedObject)webApp.Instance);
            wss.Settings[key] = expectedData;

            MSPSite.ConstructorGuid = (instance, guid) =>
            {
                var site = new MSPSite(instance)
                {
                    WebApplicationGet = () =>
                    {
                        webPO.GetChildString<WebAppSettingStore>((s) => wss);
                        return webApp;
                    },
                    Dispose = () => { }

                };
            };

            //Act 
            var target = proxyOp.Execute(args);

            //Assert .
            Assert.IsInstanceOfType(target, typeof(bool));
            Assert.IsTrue((bool)target);
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_ContainsKeyReturnsFalse_WithValidKeyNotSetAtWebAppLevel()
        {
            //arrange
            var args = new ContainsKeyDataArgs();
            string key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            args.Key = key;
            args.Level = (int)ConfigLevel.CurrentSPWebApplication;
            args.SiteId = TestsConstants.TestGuid;
            var proxyOp = new ContainsKeyOperation();

            var webApp = new BSPWebApplication(); 
            WebAppSettingStore wss = new WebAppSettingStore();
            var webPO = new MSPPersistedObject((SPPersistedObject)webApp.Instance);

            MSPSite.ConstructorGuid = (instance, guid) =>
            {
                var site = new MSPSite(instance)
                {
                    WebApplicationGet = () =>
                    {
                        webPO.GetChildString<WebAppSettingStore>((s) => wss);
                        return webApp;
                    },
                    Dispose = () => { }
                };
            };

            //Act 
            var target = proxyOp.Execute(args);

            //Assert 
            Assert.IsInstanceOfType(target, typeof(bool));
            Assert.IsFalse((bool)target);
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_ContainsKeyReturnsTrue_WithValidKeyAtFarmLevel()
        {
            //arrange
            SPFarmPropertyBag.ClearCache();
            var args = new ContainsKeyDataArgs();
            string key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            var proxyOp = new ContainsKeyOperation();
            var f = new BSPConfiguredFarm();
            string expectedData = "{92700BB6-B144-434F-A97B-5F696068A425}";           

            args.Key = key;
            args.Level = (int)ConfigLevel.CurrentSPFarm;
            args.SiteId = TestsConstants.TestGuid;

            f.SettingStore.Settings[key] = expectedData;

            //Act 
            var target = proxyOp.Execute(args);

            //Assert 
            Assert.IsInstanceOfType(target, typeof(bool));
            Assert.IsTrue((bool)target);
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_ContainsKeyReturnsFalse_WithValidKeyNotSetAtFarmLevel()
        {
            //arrange
            var args = new ContainsKeyDataArgs();
            string key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            var proxyOp = new ContainsKeyOperation();
            var f = new BSPConfiguredFarm();


            args.Key = key;
            args.Level = (int)ConfigLevel.CurrentSPFarm;
            args.SiteId = TestsConstants.TestGuid;

            //Act 
            var target = proxyOp.Execute(args);

            //Assert 
            Assert.IsInstanceOfType(target, typeof(bool));
            Assert.IsFalse((bool)target);
        }

        [TestMethod]
        public void Execute_ReturnsConfigurationException_WithInvalidLevel()
        {
            //arrange
            var args = new ContainsKeyDataArgs();
            string key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            var proxyOp = new ContainsKeyOperation();

            args.Key = key;
            args.Level = (int)ConfigLevel.CurrentSPWeb;
            args.SiteId = TestsConstants.TestGuid;

            //Act 
            var target = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(target, typeof(ConfigurationException));
         }
    }
}