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
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.SharePoint.UserCode.Moles;
using Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy;
using Microsoft.Practices.SharePoint.Common.Tests;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Moles;
using Microsoft.SharePoint.Utilities.Moles;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.Practices.SharePoint.Common.Tests.Behaviors;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SharePoint.Common.Tests.Configuration;
using Microsoft.Practices.SharePoint.Common.Logging;

namespace Microsoft.Practices.SharePoint.Common.ConfigProxy.Tests
{
    [TestClass]
    public class ReadConfigurationOperationTests
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
        public void Execute_ReadConfigThrowsArgumentNullException_WhenArgsAreNull()
        {
            bool target = false;
            var proxyOp = new ReadConfigurationOperation();

            //Act 
            try
            {
                proxyOp.Execute(null);
            }
            catch (ArgumentNullException)
            {
                target = true;
            }

            //Assert 
            Assert.IsTrue(target);
        }


        [TestMethod]
        public void Execute_ReturnsConfigurationException_WhenSiteIdIsEmpty()
        {
            var proxyOp = new ReadConfigurationOperation();
            var args = new ReadConfigArgs();
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
            var proxyOp = new ReadConfigurationOperation();
            var args = new ReadConfigArgs();
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
            var proxyOp = new ReadConfigurationOperation();
            var args = new ContainsKeyDataArgs();

            //Act 
            object result = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ConfigurationException));
        }


        [TestMethod]
        public void Execute_ReturnsAConfigurationException_WhenKeyDoesntHaveNamespace()
        {
            //arrange
            var args = new ReadConfigArgs();
            args.Key = TestsConstants.TestGuidName;
            args.Level = (int)ConfigLevel.CurrentSPWebApplication;
            args.SiteId = TestsConstants.TestGuid;

            var proxyOp = new ReadConfigurationOperation();

            //Act 
            var ex = proxyOp.Execute(args);

            //Assert
            Assert.IsInstanceOfType(ex, typeof(ConfigurationException));
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_ReturnsKeyData_WithValidKeyAtWebAppLevel()
        {
            //arrange
            SPWebAppPropertyBag.ClearCache();
            var args = new ReadConfigArgs();
            string key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            args.Key = key;
            args.Level = (int)ConfigLevel.CurrentSPWebApplication;
            args.SiteId = TestsConstants.TestGuid;
            var proxyOp = new ReadConfigurationOperation();
            string expectedData = "{92700BB6-B144-434F-A97B-5F696068A425}";

            MSPPersistedObject webPO;
            WebAppSettingStore wss;

            MSPSite.ConstructorGuid = (instance, guid) =>
            {
                var site = new MSPSite(instance)
                {
                    WebApplicationGet = () =>
                    {
                        var webApp = new BSPConfiguredWebApp();
                        wss = new WebAppSettingStore();
                        wss.Settings[key] = expectedData;
                        webPO = new MSPPersistedObject((SPPersistedObject)webApp.Instance);
                        webPO.GetChildString<WebAppSettingStore>((s) => wss);
                        return webApp;
                    },
                    Dispose = () => { }

                };
            };

            //Act 
            object target = proxyOp.Execute(args);

            //Assert .
            Assert.IsInstanceOfType(target, typeof(string));
            Assert.AreEqual(expectedData, (string)target);
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_ReturnsNull_WithValidKeyNotSetAtWebAppLevel()
        {
            //arrange
            SPWebAppPropertyBag.ClearCache();
            var args = new ReadConfigArgs();
            string key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            args.Key = key;
            args.Level = (int)ConfigLevel.CurrentSPWebApplication;
            args.SiteId = TestsConstants.TestGuid;
            var proxyOp = new ReadConfigurationOperation();
            MSPPersistedObject webPO;
            WebAppSettingStore wss;

            MSPSite.ConstructorGuid = (instance, guid) =>
            {
                var site = new MSPSite(instance)
                {
                    WebApplicationGet = () =>
                    {
                        var webApp = new BSPConfiguredWebApp();
                        wss = new WebAppSettingStore();
                        webPO = new MSPPersistedObject((SPPersistedObject)webApp.Instance);
                        webPO.GetChildString<WebAppSettingStore>((s) => wss);
                        return webApp;
                    },
                    Dispose = () => { }

                };
            };

            //Act 
            object target = proxyOp.Execute(args);

            //Assert 
            Assert.IsNull(target);

        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_ReturnsKey_WithValidKeyAtFarmLevel()
        {
            //arrange
            SPFarmPropertyBag.ClearCache();
            var args = new ReadConfigArgs();
            string key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            var proxyOp = new ReadConfigurationOperation();
            var f = new BSPConfiguredFarm();
            string expectedData = "{92700BB6-B144-434F-A97B-5F696068A425}";

            args.Key = key;
            args.Level = (int)ConfigLevel.CurrentSPFarm;
            args.SiteId = TestsConstants.TestGuid;

            f.SettingStore.Settings[key] = expectedData;
 

            //Act 
            var target = proxyOp.Execute(args);

            //Assert 
            Assert.IsInstanceOfType(target, typeof(string));
            Assert.AreEqual(expectedData, target);
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_ReturnsNull_WithValidKeyNotSetAtFarmLevel()
        {
            //arrange
            SPFarmPropertyBag.ClearCache();
            var args = new ReadConfigArgs();
            string key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            var proxyOp = new ReadConfigurationOperation();
            var f = new BSPConfiguredFarm();


            args.Key = key;
            args.Level = (int)ConfigLevel.CurrentSPFarm;
            args.SiteId = TestsConstants.TestGuid;

            //Act 
            var target = proxyOp.Execute(args);

            //Assert 
            Assert.IsNull(target);
        }

        [TestMethod]
        public void Execute_ReadKeyReturnsConfigurationException_WithInvalidLevel()
        {
            //arrange
            var args = new ReadConfigArgs();
            string key = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            var proxyOp = new ReadConfigurationOperation();

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

