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
using Microsoft.Practices.SharePoint.Common.Tests.Behaviors;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Administration.Moles;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public class ConfigMangerTests
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
        public void GetFromPropertyBagGenericSucceeds()
        {
            //Arrange
            string key = TestsConstants.TestGuidName;
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + key;
            string value = "{821FC688-2C8C-4F8B-A700-EDB81400B63B}";

            var bag = new BIPropertyBag();
            bag.Values.SetOne(namespacedKey, value);

            //Act
            var target = new ConfigManager();
            string actual = target.GetFromPropertyBag<string>(key, bag);

            // Assert
            Assert.AreEqual(value, actual);
        }

        [TestMethod]
        public void GetFromPropertyBagGenericWithNullKeyThrowsArgumentNullException()
        {
            //Arrange
            var target = new ConfigManager();
            var bag = new BIPropertyBag();
            bag.Values.SetOne("foo", "bar");
            bag.Values.Count = 0;
            bool expectedExceptionThrown = false;


            //Act
            try
            {
                string actual = target.GetFromPropertyBag<string>(null, bag);
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        public void GetFromPropertyBagGenericWithNullBagThrowsArgumentNullException()
        {
            //Arrange
            var target = new ConfigManager();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                string actual = target.GetFromPropertyBag<string>("foo", (IPropertyBag)null);
            }
            catch
            {
                expectedExceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(expectedExceptionThrown);
        }



        [TestMethod]
        public void GetFromPropertyBagWithNullKeyThrowsArgumentNullException()
        {
            //Arrange
            var target = new ConfigManager();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                string actual = target.GetFromPropertyBag(typeof(string), null, ConfigLevel.CurrentSPSite) as string;
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFromPropertyBagWithNullTypeThrowsArgumentNullException()
        {
            //Arrange
            var target = new ConfigManager();
            bool expectedExceptionThrown = false;

            //Act
            string actual = target.GetFromPropertyBag(null, "foobar", ConfigLevel.CurrentSPSite) as string;

            // Assert
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        public void ContainsKeyInPropertyBagSucceeds()
        {
            //Arrange
            var bag = new BIPropertyBag();
            string key = TestsConstants.TestGuidName;
            string namespaceKey = ConfigManager.PnPKeyNamespace + "." + key;
            bag.Values.SetOne(namespaceKey, "foobar");
            var target = new ConfigManager();

            //Act
            bool expected = target.ContainsKeyInPropertyBag(TestsConstants.TestGuidName, bag);

            //Assert
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void ContainsKeyInPropertyBagWithNullKeyThrowsArgumentNullException()
        {
            //Arrange
            var bag = new BIPropertyBag();
            var target = new ConfigManager();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                bool expected = target.ContainsKeyInPropertyBag(null, bag);
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert 
            Assert.IsTrue(expectedExceptionThrown);

        }


        [TestMethod]
        public void SetValueBasedOnKeySucceeds()
        {
            //Arrange
            string key = "{6313EE1A-5A12-46A3-A537-4905678FBD9E}";
            var propBag = new BIPropertyBag();
            propBag.Values.Count = 0;
            var hierarchy = new BIConfigStack();
            hierarchy.Bags.Add(propBag);
            var target = new ConfigManager(hierarchy, new MockConfigSettingSerializer());

            //Act
            target.SetInPropertyBag(key, 3, propBag);

            //Assert
            Assert.IsTrue(target.ContainsKeyInPropertyBag(key, propBag));
            int value = target.GetFromPropertyBag<int>(key, propBag);
            Assert.IsTrue(value == 3);
        }

        [TestMethod]
        [HostType("Moles")]
        public void SaveValueWithRetrySucceeds()
        {
            //Arrange
            string key = "{6313EE1A-5A12-46A3-A537-4905678FBD9E}";
            var propBag = new BIPropertyBag();
            int retry = 0;

            propBag.ItemSetStringString = (k, v) =>
            {
                if (retry < 2)
                {
                    retry++;
                    var ex = new MSPUpdatedConcurrencyException();
                    throw ex.Instance;
                }
                else
                    propBag.Values[k] = v;
            };

            propBag.Values.Count = 0;
            var hierarchy = new BIConfigStack();
            hierarchy.Bags.Add(propBag);
            var target = new ConfigManager(hierarchy, new MockConfigSettingSerializer());

            //Act
            target.SetInPropertyBag(key, 3, propBag);

            //Assert
            Assert.IsTrue(target.ContainsKeyInPropertyBag(key, propBag));
            int value = target.GetFromPropertyBag<int>(key, propBag);
            Assert.IsTrue(value == 3);
        }

        [TestMethod]
        [HostType("Moles")]
        public void SaveValueExceedsRetryFails()
        {
            //Arrange
            string key = "{6313EE1A-5A12-46A3-A537-4905678FBD9E}";
            var propBag = new BIPropertyBag();
            int retry = 0;
            bool expectedExceptionThrown = false;

            propBag.ItemSetStringString = (k, v) =>
            {
                if (retry < 3)
                {
                    retry++;
                    var ex = new MSPUpdatedConcurrencyException();
                    throw ex.Instance;
                }
                else
                    propBag.Values[k] = v;
            };

            propBag.Values.Count = 0;
            var hierarchy = new BIConfigStack();
            hierarchy.Bags.Add(propBag);
            var target = new ConfigManager(hierarchy, new MockConfigSettingSerializer());

            //Act
            try
            {
                target.SetInPropertyBag(key, 3, propBag);
            }
            catch (ConfigurationException)
            {
                expectedExceptionThrown = true;
            }


            //Assert
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        public void SerializationFailureThrowsConfigurationException()
        {
            //Arrange
            var propertyBag = new BIPropertyBag();
            propertyBag.Values.Count = 0;
            string errString = null;
            string expectedError = "SerializationException";
            var hierarchy = new BIConfigStack();
            hierarchy.Bags.Add(propertyBag);

            // Act
            var target = new ConfigManager(hierarchy, new MockConfigSettingSerializer() { ThrowError = true });
            try
            {
                target.SetInPropertyBag("key", "MyValue", propertyBag);
            }
            catch (ConfigurationException ex)
            {
                errString = ex.Message;
            }

            // Assert
            Assert.IsNotNull(errString);
            Assert.IsTrue(errString.Contains(expectedError));
        }

        [TestMethod]
        public void UsingSiteAsSuffixThrowsException()
        {
            //Arrange
            string key = "{821FC688-2C8C-4F8B-A700-EDB81400B63B}" + SPSitePropertyBag.KeySuffix;
            var hierarchy = new BIConfigStack();
            hierarchy.Bags.Add(new BIPropertyBag());

            var target = new ConfigManager(hierarchy);
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target.SetInPropertyBag(key, "value", new BIPropertyBag());
            }
            catch (ConfigurationException)
            {
                expectedExceptionThrown = true;
            }

            //Assert 
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        [HostType("Moles")]
        public void SetConfigValueLogs()
        {
            //Arrange
            var propertyBag = new BIPropertyBag();
            propertyBag.Values.Count = 0;
            var farm = new BSPFarm();
            string key = "myKey";
            string value = "myValue";
            var hierarchy = new BIConfigStack();
            hierarchy.Bags.Add(propertyBag);

            //Act
            var target = new ConfigManager(hierarchy);
            target.SetInPropertyBag(key, value, target.GetPropertyBag(ConfigLevel.CurrentSPFarm));

            //Assert
            StringAssert.Contains(logger.TraceMessage, key);
            StringAssert.Contains(logger.TraceMessage, value);
        }

        [TestMethod]
        public void RemoveKeyFromPropertyBagSucceeds()
        {
            //Arrange
            BIPropertyBag bag = new BIPropertyBag();
            string namespacedKey = ConfigManager.PnPKeyNamespace + "." + TestsConstants.TestGuidName;
            bag.Values.SetOne(namespacedKey, "{821FC688-2C8C-4F8B-A700-EDB81400B63B}");
            var hierarchy = new BIConfigStack();
            hierarchy.Bags.Add(new BIPropertyBag());

            var target = new ConfigManager(hierarchy);

            //Act
            Assert.IsTrue(bag.Values.ContainsKey(namespacedKey));
            target.RemoveKeyFromPropertyBag(TestsConstants.TestGuidName, bag);

            //Assert
            Assert.IsFalse(bag.Values.ContainsKey(namespacedKey));
        }
    }
}
