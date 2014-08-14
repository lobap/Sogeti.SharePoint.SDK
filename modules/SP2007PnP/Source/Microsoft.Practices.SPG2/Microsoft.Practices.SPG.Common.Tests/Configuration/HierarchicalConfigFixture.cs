//===============================================================================
// Microsoft patterns & practices
// SharePoint Guidance version 2.0
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace Microsoft.Practices.SPG.Common.Tests.Configuration
{
    [TestClass]
    public class HierarchicalConfigFixture
    {
        private MockLogger logger;
        [TestInitialize]
        public void Init()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<ILogger, MockLogger>(InstantiationType.AsSingleton));

            logger = SharePointServiceLocator.Current.GetInstance<ILogger>() as MockLogger;
        }

        [TestCleanup]
        public void Cleanup()
        {
            SharePointServiceLocator.Reset();
            
        }


        [TestMethod]
        public void CanGetValueBasedOnKey()
        {
            MockPropertyBag defaultPropertyBag = new MockPropertyBag();
            defaultPropertyBag["key"] = "3";
            var target = new HierarchicalConfig(defaultPropertyBag, new MockConfigSettingSerializer());
            int configValue = target.GetByKey<int>("key");

            Assert.AreEqual(3, configValue);
        }

        [TestMethod]
        public void CanSetValueBasedOnKey()
        {
            MockPropertyBag defaultPropertyBag = new MockPropertyBag();
            var target = new HierarchicalConfig(defaultPropertyBag, new MockConfigSettingSerializer());
            target.SetInPropertyBag("Key", 3, defaultPropertyBag);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationException), "Configsetting with key 'key' could not be retrieved. The configured value could not be converted from 'abc' to an instance of 'System.DateTime'. The technical exception was: System.AbandonedMutexException: Something was wrong")]
        public void DeSerializeErrorGivesClearErrroMessage()
        {
            MockPropertyBag defaultPropertyBag = new MockPropertyBag();
            defaultPropertyBag.values.Add("key", "abc");
            var target = new HierarchicalConfig(defaultPropertyBag, new MockConfigSettingSerializer(){ThrowError = true});
            target.GetByKey<DateTime>("key");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationException), "Configsetting with key 'key' could not be set to 'MyValue' with type 'System.String'. The technical exception was: System.AbandonedMutexException: Something was wrong")]
        public void SerializeErrorGivesClearErrroMessage()
        {
            MockPropertyBag defaultPropertyBag = new MockPropertyBag();
            var target = new HierarchicalConfig(defaultPropertyBag, new MockConfigSettingSerializer() { ThrowError = true });
            target.SetInPropertyBag("key", "MyValue", defaultPropertyBag);
        }

        [TestMethod]
        [ExpectedException(typeof(NoSharePointContextException))]
        public void IfContextIsNullDefaultPropertyBagThrows()
        {
            var target = new HierarchicalConfig();
            target.GetByKey<DateTime>("key");
        }

        [TestMethod]
        public void KeyNotFoundThrows()
        {
            var target = new HierarchicalConfig(new MockPropertyBag());

            try
            {
                target.GetByKey<string>("key");
                Assert.Fail();
            }
            catch(ConfigurationException configurationException)
            {
                Assert.AreEqual("There was no value configured for key 'key' in a propertyBag with level 'CurrentSPFarm' or above.",
                    configurationException.Message);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            
        }

        [TestMethod]
        public void GetValueWillUseHierarchy()
        {
            MockPropertyBag parent = BuildPropertyBagHierarcy();

            parent["value"] = "value";

            var target = new HierarchicalConfig(parent.Children.First());

            Assert.AreEqual("value", target.GetByKey<string>("value"));
        }

        [TestMethod]
        public void KeyExistsWillUseHierarchy()
        {
            MockPropertyBag parent = BuildPropertyBagHierarcy();

            var target = new HierarchicalConfig(parent.Children.First());

            Assert.IsFalse(target.ContainsKey("value"));

            parent["value"] = "value";

            Assert.IsTrue(target.ContainsKey("value"));
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationException), "The key 'Site_BadValue' cannot be used. Key's may not be prefixed with the text 'Site_' because this is used by the SPSitePropertyBag to differentiate between properties of the SPWeb and the SPSite.")]
        public void UsingSiteAsPrefixFails()
        {
            var target = new HierarchicalConfig(new MockPropertyBag());

            target.SetInPropertyBag("Site_BadValue", "value", new MockPropertyBag());
        }

        [TestMethod]
        public void SetConfigValueLogs()
        {
            SPFarm fakeFarm = Isolate.Fake.Instance<SPFarm>();
            Isolate.WhenCalled(() => fakeFarm.Properties).WillReturn(new Hashtable());
            var target = new HierarchicalConfig(new MockPropertyBag());

            target.SetInPropertyBag("myKey", "myValue", fakeFarm);

            Assert.AreEqual("Set value in hierarchical config.\n\tKey: 'myKey'\n\tLevel: 'CurrentSPFarm'\n\tValue: 'myValue'", logger.TraceMessage);
        }


        private MockPropertyBag BuildPropertyBagHierarcy()
        {
            MockPropertyBag parent = new MockPropertyBag();
            MockPropertyBag child1 = new MockPropertyBag() {Parent = parent};
            MockPropertyBag child2 = new MockPropertyBag() {Parent = parent};
            parent.Children = new IPropertyBag[] {child1, child2};
            return parent;
        }
    }

    public class MockLogger : BaseLogger
    {
        public string TraceMessage;

        protected override void WriteToOperationsLog(string message, int eventId, EventLogEntryType severity, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category)
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
            if (ThrowError)
                throw new AbandonedMutexException("Something goes wrong");

            if (value == "3")
                return 3;

            return null;
        }

        public string Serialize(Type type, object value)
        {
            if (ThrowError)
                throw new AbandonedMutexException("Something goes wrong");

            return value.ToString();
        }
    }

}
