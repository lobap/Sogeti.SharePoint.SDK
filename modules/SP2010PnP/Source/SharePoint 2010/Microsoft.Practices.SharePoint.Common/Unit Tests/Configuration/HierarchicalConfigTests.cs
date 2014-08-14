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
using System.Moles;
using System.Threading;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Moles;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Moles.Framework;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Behaviors;
using System.Runtime.Serialization;
using Microsoft.Practices.SharePoint.Common.Tests.Behaviors;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public partial class HierarchicalConfigTests
    {
        private MockLogger logger;
        [TestInitialize]
        public void Init()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(
                new ActivatingServiceLocator()
                .RegisterTypeMapping<ILogger, MockLogger>(InstantiationType.AsSingleton));

            logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>()
                as MockLogger;
        }

        [TestCleanup]
        public void Cleanup()
        {
            SharePointServiceLocator.Reset();
            SharePointEnvironment.Reset();
        }


        [TestMethod]
        public void GetByKey_GetValueByKey()
        {
            //Arrange
            // This is an example of a behaved type
            // (a stub that is extended to implement general behavior 
            // and can be re-used)
            var defaultPropertyBag = new BIPropertyBag();
            string key = "key";
            int expected = 3;
            string namespaceKey = ConfigManager.PnPKeyNamespace + "." + key;
            defaultPropertyBag.Values.SetOne(namespaceKey, expected.ToString());
            var stack = new BIConfigStack();
            stack.Bags.Add(defaultPropertyBag);

            var serializer = new SIConfigSettingSerializer()
            {
                DeserializeTypeString = (type, data) =>
                    {
                        object ret = null;
                        if (type == typeof(int))
                        {
                            ret = int.Parse(data);
                        }
                        return ret;
                    },
            };

            //Act
            var target = new HierarchicalConfig(stack, serializer);
            int configValue = target.GetByKey<int>(key);

            //Assert
            Assert.AreEqual(expected, configValue);
        }


        [TestMethod]
        public void GetByKey_GetValue_WillUseHierarchy()
        {
            //Arrange
            string key = "{6313EE1A-5A12-46A3-A537-4905678FBD9E}";
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + key;
            string value = "{821FC688-2C8C-4F8B-A700-EDB81400B63B}";

            var parent = new BIPropertyBag();
            var child = new BIPropertyBag();
            parent.Values.SetOne(namespacedKey, value);
            child.Values.Count = 0;
            var stack = new BIConfigStack();
            stack.Bags.Add(child);
            stack.Bags.Add(parent);

            //Act
            var target = new HierarchicalConfig(stack);
            string actual = target.GetByKey<string>(key);

            // Assert
            Assert.AreEqual(value, actual);
        }

        [TestMethod]

        public void GetByKey_GetValueWithNullKey_ThrowsArgumentNullException()
        {
            var target = new HierarchicalConfig();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                int configValue = target.GetByKey<int>(null);
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert 
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        public void GetByKey_KeyNotFound_ThrowsConfigurationException()
        {
            // Arrange
            var defaultPropertyBag = new BIPropertyBag();
            defaultPropertyBag.Values.Count = 0;
            var stack = new BIConfigStack();
            stack.Bags.Add(defaultPropertyBag);
            bool expectedExceptionThrown = false;

            var target = new HierarchicalConfig(stack);

            //Act
            try
            {
                target.GetByKey<string>("key");
            }
            catch (ConfigurationException)
            {
                expectedExceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(expectedExceptionThrown, "Failed to throw exception on key not found");
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetByKey_GetByValueWithLevelWithNullKey_ThrowsArgumentNullException()
        {
            //Arrange
            var locator = SharePointServiceLocator.GetCurrent() as ActivatingServiceLocator;
            var context = new MSPContext();
            MSPContext.CurrentGet = () => context;
            var target = new HierarchicalConfig();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                string expectedContains = target.GetByKey<string>(null, ConfigLevel.CurrentSPFarm);
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert
            Assert.IsTrue(expectedExceptionThrown);
        }


 
        [TestMethod]
        public void ContainsKey_ContainsWithHierarchy()
        {
            //Arrange
            var parent = new BIPropertyBag();
            var child = new BIPropertyBag();
            string key = "value";
            string namespaceKey = ConfigManager.PnPKeyNamespace + "." + key;
            parent.Values.Count = 0;
            child.Values.Count = 0;
            var stack = new BIConfigStack();
            stack.Bags.Add(child);
            stack.Bags.Add(parent);

            //Act, Assert
            var target = new HierarchicalConfig(stack);
            IPropertyBag parentBag = parent;

            Assert.IsFalse(target.ContainsKey("value"));
            parentBag[namespaceKey] = null;
            Assert.IsTrue(target.ContainsKey(key));
        }



        [TestMethod]
        public void ContainsKey_IfContextIsNull_ThrowsNoSharePointException()
        {
            //Arrange
            var target = new HierarchicalConfig();
            bool expectedExceptionThrown = false;

            // Act
            try
            {
                target.GetByKey<DateTime>("key");
            }
            catch (NoSharePointContextException)
            {
                expectedExceptionThrown = true;
            }

            // Assert 
            Assert.IsTrue(expectedExceptionThrown);
        }


        [TestMethod]
        public void GetByKey_DeSerializeWrongType_ThrowsConfigurationException()
        {
            //Arrange
            var propertyBag = new BIPropertyBag();
            propertyBag.Values.Count = 0;
            string key = "key";
            string nameSpacedKey = ConfigManager.PnPKeyNamespace + "." + key;
            string expectedError = "SerializationException";
            propertyBag.Values[nameSpacedKey] = "abc";
            var stack = new BIConfigStack();
            stack.Bags.Add(propertyBag);
            string errString = null;

            //Act
            var target = new HierarchicalConfig(stack, new MockConfigSettingSerializer() { ThrowError = true });

            try
            {
                target.GetByKey<DateTime>("key");
            }
            catch (ConfigurationException ex)
            {
                errString = ex.ToString();
            }

            // Assert
            Assert.IsNotNull(errString);
            Assert.IsTrue(errString.Contains(expectedError));
        }

        [TestMethod]
        public void GetSettingFrom_GetASetting()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            BIPropertyBag bag = GetPropertyBag(ConfigLevel.CurrentSPSite);
            hierarchy.AddPropertyBag(bag);
            string key = TestsConstants.TestGuidName;
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + key;
            string expected = "{80C23A3E-566B-4B11-A881-5868F2BCB198}";
            bag.Values[namespacedKey] = expected;
            bag.Level = ConfigLevel.CurrentSPWeb;
            var config = new HierarchicalConfig(hierarchy);

            //Act
            string target = config.GetByKey<string>(key, ConfigLevel.CurrentSPWeb);

            //Assert 
            Assert.AreEqual(expected, target);
        }

        [TestMethod]
        public void GetSettingFrom_GetASetting_NoKeyReturnsNull()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            BIPropertyBag bag = GetPropertyBag(ConfigLevel.CurrentSPWeb);
            hierarchy.AddPropertyBag(bag);
            string key = TestsConstants.TestGuidName;
            var config = new HierarchicalConfig(hierarchy);
            ConfigurationException configEx = null;

            //Act
            try
            {
                string target = config.GetByKey<string>(key, ConfigLevel.CurrentSPWeb);
            }
            catch (ConfigurationException ex)
            {
                configEx = ex;
            }

            //Assert 
            Assert.IsNotNull(configEx);
            Assert.IsTrue(configEx.Message.Contains(key));
        }

        [TestMethod]
        public void GetSettingFrom_GetASettingWithHierarchy()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            string key = TestsConstants.TestGuidName;
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + key;
             string expected = "{80C23A3E-566B-4B11-A881-5868F2BCB198}";
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPWeb));
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPSite));
            BIPropertyBag bag = GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            bag.Values[namespacedKey] = expected;
            hierarchy.AddPropertyBag(bag);
            var config = new HierarchicalConfig(hierarchy);

            //Act
            string target = config.GetByKey<string>(key, ConfigLevel.CurrentSPWeb);

            //Assert 
            Assert.AreEqual(expected, target);
        }

        [TestMethod]
        public void GetSettingFrom_GetASettingWithHierarchy_FromMidHierarchyFindsKey()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            string key = TestsConstants.TestGuidName;
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + key;
            string expected = "{80C23A3E-566B-4B11-A881-5868F2BCB198}";
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPWeb));
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPSite));
            BIPropertyBag bag = GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            bag.Values[namespacedKey] = expected;
            hierarchy.AddPropertyBag(bag);
            var config = new HierarchicalConfig(hierarchy);

            //Act
            string target = config.GetByKey<string>(key, ConfigLevel.CurrentSPSite);

            //Assert 
            Assert.AreEqual(expected, target);
        }


        [TestMethod]
        public void GetSettingFrom_GetASettingWithHierarchyDoesntFindKey_FromLevelAboveKey()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            string key = TestsConstants.TestGuidName;
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + key;
            string value = "{80C23A3E-566B-4B11-A881-5868F2BCB198}";
            BIPropertyBag bag = GetPropertyBag(ConfigLevel.CurrentSPWeb);
            bag.Values[namespacedKey] = value;
            hierarchy.AddPropertyBag(bag);
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPSite));
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPWebApplication));
            var target = new HierarchicalConfig(hierarchy);
            ConfigurationException configEx = null;

            //Act
            try
            {
                string result = target.GetByKey<string>(key, ConfigLevel.CurrentSPSite);
            }
            catch (ConfigurationException ex)
            {
                configEx = ex;
            }

            //Assert 
            Assert.IsNotNull(configEx);
            Assert.IsTrue(configEx.Message.Contains(key));
        }

        [TestMethod]
        public void GetSettingFrom_GetASetting_WithNullKey()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            bool expectedExceptionThrown = false;
            var target = new HierarchicalConfig(hierarchy);

            //Act
            try
            {
                target.GetByKey<string>(null, ConfigLevel.CurrentSPSite);
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            Assert.IsTrue(expectedExceptionThrown);
        }


        [TestMethod]
        public void ContainsFrom_ContainsASetting()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            bool expected = true;
            BIPropertyBag bag = GetPropertyBag(ConfigLevel.CurrentSPWeb);
            hierarchy.AddPropertyBag(bag);
            string key = TestsConstants.TestGuidName;
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + key;
            string value = "{80C23A3E-566B-4B11-A881-5868F2BCB198}";
            bag.Values[namespacedKey] = value;
            var config = new HierarchicalConfig(hierarchy);

            //Act
            bool target = config.ContainsKey(key, ConfigLevel.CurrentSPWeb);

            //Assert 
            Assert.AreEqual(expected, target);
        }

        [TestMethod]
        public void ContainsFrom_ContainsASetting_NoKeyReturnsFalse()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            bool expected = false;
            BIPropertyBag bag = GetPropertyBag(ConfigLevel.CurrentSPWeb);
            hierarchy.AddPropertyBag(bag);
            string key = TestsConstants.TestGuidName;

            var config = new HierarchicalConfig(hierarchy);

            //Act
            bool target = config.ContainsKey(key, ConfigLevel.CurrentSPWeb);

            //Assert 
            Assert.AreEqual(expected, target);
        }

        [TestMethod]
        public void ContainsFrom_ContainsASettingWithHierarchy()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            bool expected = true;
            string key = TestsConstants.TestGuidName;
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + key;
 
            string value = "{80C23A3E-566B-4B11-A881-5868F2BCB198}";
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPWeb));
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPSite));
            BIPropertyBag bag = GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            bag.Values[namespacedKey] = value;
            hierarchy.AddPropertyBag(bag);
            var config = new HierarchicalConfig(hierarchy);

            //Act
            bool target = config.ContainsKey(key, ConfigLevel.CurrentSPWeb);

            //Assert 
            Assert.AreEqual(expected, target);
        }

        [TestMethod]
        public void ContainsFrom_ContainsASettingWithHierarchy_FromMidHierarchyReturnsTrue()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            bool expected = true;
            string key = TestsConstants.TestGuidName;
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + key;
             string value = "{80C23A3E-566B-4B11-A881-5868F2BCB198}";
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPWeb));
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPSite));
            BIPropertyBag bag = GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            bag.Values[namespacedKey] = value;
            hierarchy.AddPropertyBag(bag);
            var config = new HierarchicalConfig(hierarchy);

            //Act
            bool target = config.ContainsKey(key, ConfigLevel.CurrentSPSite);

            //Assert 
            Assert.AreEqual(expected, target);
        }


        [TestMethod]
        public void ContainsFrom_ContainsASettingWithHierarchyReturnsFalse_FromLevelAboveKey()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            string key = TestsConstants.TestGuidName;
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + key;
             bool expected = false;
            string value = "{80C23A3E-566B-4B11-A881-5868F2BCB198}";
            BIPropertyBag bag = GetPropertyBag(ConfigLevel.CurrentSPWeb);
            bag.Values[namespacedKey] = value;
            hierarchy.AddPropertyBag(bag);
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPSite));
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPWebApplication));
            var config = new HierarchicalConfig(hierarchy);

            //Act
            bool target = config.ContainsKey(key, ConfigLevel.CurrentSPSite);

            //Assert 
            Assert.AreEqual(expected, target);
        }

        [TestMethod]
        public void ContainsFrom_ContainsASettingWithHierarchy_NoKeyReturnsFalse()
        {
            //Arrange
            var hierarchy = new TestablePropertyBagHierarchy();
            string key = TestsConstants.TestGuidName;
            bool expected = false;
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPWeb));
            hierarchy.AddPropertyBag(GetPropertyBag(ConfigLevel.CurrentSPSite));
            var config = new HierarchicalConfig(hierarchy);

            //Act
            bool target = config.ContainsKey(key, ConfigLevel.CurrentSPWeb);

            //Assert 
            Assert.AreEqual(expected, target);
        }

        [TestMethod]
        public void ContainsFrom_ContainsASetting_WithNullKeyThrowsArgumentNullException()
        {
            //Arrange
            bool expectedExceptionThrown = false;
            var hierarchy = new TestablePropertyBagHierarchy();
            var config = new HierarchicalConfig(hierarchy);

            //Act
            try
            {
                config.ContainsKey(null, ConfigLevel.CurrentSPSite);
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            Assert.IsTrue(expectedExceptionThrown);
        }



        private BIPropertyBag GetPropertyBag(ConfigLevel level)
        {
            var propBag = new BIPropertyBag();
            propBag.Values.Count = 0;
            propBag.Level = level;
            return propBag;
        }

    }


    public class MockLogger : BaseLogger
    {
        public string TraceMessage;

        protected override void WriteToOperationsLog(string message, int eventId, EventSeverity severity, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category)
        {
            TraceMessage = message;
        }

        protected override void WriteToOperationsLog(string message, int eventId, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToOperationsLog(string message, int eventId, SandboxEventSeverity severity, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, SandboxTraceSeverity severity, string category)
        {
            TraceMessage = message;
        }
    }

    public class MockConfigSettingSerializer : IConfigSettingSerializer
    {
        public bool ThrowError = false;

        public string ConfigValueAsString;

        public object Deserialize(Type type, string value)
        {
            object ret = null;
            if (ThrowError)
                throw new SerializationException("Something goes wrong");

            if (value == "3")
                ret = 3;

            return ret;
        }

        public string Serialize(Type type, object value)
        {
            if (ThrowError)
                throw new SerializationException("Something goes wrong");

            return value.ToString();
        }
    }

    public class MockAppContextProvider : IApplicationContextProvider
    {
        private string appfriendlynameretval = "foobar";
        SPWeb web = null;
        List<string> proxiesInstalled = new List<string>();
        public bool IsProxyInstalledRetValue { get; set; }

        public MockAppContextProvider()
        {
            IsProxyInstalledRetValue = true;
        }

        public void AddProxyType(string proxyType)
        {
            proxiesInstalled.Add(proxyType);
        }

        public void SetWeb(SPWeb web)
        {
            this.web = web;
        }

        public void SetSandbox()
        {
            this.appfriendlynameretval = "Sandbox";
        }

        public string GetCurrentAppDomainFriendlyName()
        {
            return this.appfriendlynameretval;
        }

        public Microsoft.SharePoint.SPWeb GetSPContextCurrentWeb()
        {
            return this.web;
        }

        public SPFarm GetSPFarmLocal()
        {
            return SPFarm.Local;
        }

        public object ExecuteRegisteredProxyOperation(string assemblyName, string typeName, Microsoft.SharePoint.UserCode.SPProxyOperationArgs args)
        {
            if(proxiesInstalled.Contains(typeName))
                return true;
            else
                return false;
        }


        public bool IsProxyCheckerInstalled()
        {
            return true;
        }

        public bool IsProxyInstalled(string assemblyName, string typeForProxy)
        {
            return IsProxyInstalledRetValue;
        }
    }
}
