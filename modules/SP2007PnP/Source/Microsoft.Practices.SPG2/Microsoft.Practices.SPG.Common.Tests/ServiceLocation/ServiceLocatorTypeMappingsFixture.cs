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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace Microsoft.Practices.SPG.Common.Tests.ServiceLocation
{
    [TestClass]
    public class ServiceLocatorTypeMappingsFixture
    {
        [TestMethod]
        public void CanRegisterTypeMapping()
        {
            var mockPropertyBag = new MockPropertyBag();
            MockConfigManager hierarchicalConfig = new MockConfigManager();    
            var target = new ServiceLocatorConfig(hierarchicalConfig);

            target.RegisterTypeMapping<ISomething, Something>();

            var typeMappings = hierarchicalConfig.Value as List<TypeMapping>;

            TypeMapping mapping = typeMappings.First();
            Assert.AreEqual("Microsoft.Practices.SPG.Common.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                , mapping.FromAssembly);
            Assert.AreEqual("Microsoft.Practices.SPG.Common.Tests.ServiceLocation.ISomething, Microsoft.Practices.SPG.Common.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                , mapping.FromType);
            Assert.AreEqual("Microsoft.Practices.SPG.Common.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                , mapping.ToAssembly);
            Assert.AreEqual("Microsoft.Practices.SPG.Common.Tests.ServiceLocation.Something, Microsoft.Practices.SPG.Common.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                , mapping.ToType);
        }

        [TestMethod]
        public void CanGetTypeMappings()
        {
            var mockPropertyBag = new MockPropertyBag();
            MockConfigManager hierarchicalConfig = new MockConfigManager();

            List<TypeMapping> typeMappings = new List<TypeMapping>();
            typeMappings.Add(new TypeMapping(){FromAssembly = "1"});
            typeMappings.Add(new TypeMapping() { FromAssembly = "2" });
            typeMappings.Add(new TypeMapping() { FromAssembly = "3" });

            hierarchicalConfig.Value = typeMappings;

            var target = new ServiceLocatorConfig(hierarchicalConfig);
            IEnumerable<TypeMapping> registeredTypeMappings = target.GetTypeMappings();

            Assert.AreEqual(3, registeredTypeMappings.Count());
        }

        [TestMethod]
        public void RegisteringTypeMappingTwiceOverwritesLastOne()
        {
            var mockPropertyBag = new MockPropertyBag();
            MockConfigManager hierarchicalConfig = new MockConfigManager();
            var target = new ServiceLocatorConfig(hierarchicalConfig);

            target.RegisterTypeMapping<ISomething, Something>();
            target.RegisterTypeMapping<ISomething, SomethingElse>();

            var typeMappings = hierarchicalConfig.Value as List<TypeMapping>;

            TypeMapping mapping = typeMappings.First();

            Assert.IsTrue(mapping.ToType.Contains("SomethingElse"));

        }

        [TestMethod]
        public void CanRegisterTypeMappingWithKey()
        {
            var mockPropertyBag = new MockPropertyBag();
            MockConfigManager hierarchicalConfig = new MockConfigManager();
            var target = new ServiceLocatorConfig(hierarchicalConfig);

            target.RegisterTypeMapping<ISomething, Something>("key1");
            target.RegisterTypeMapping<ISomething, Something>("key2");

            var typeMappings = hierarchicalConfig.Value as List<TypeMapping>;

            TypeMapping mapping1 = typeMappings.First();
            TypeMapping mapping2 = typeMappings.Skip(1).First();
            Assert.AreEqual("key1", mapping1.Key);
            Assert.AreEqual("key2", mapping2.Key);
        }

        [TestMethod]
        public void CanRemoveTypeMapping()
        {
            var mockPropertyBag = new MockPropertyBag();
            MockConfigManager hierarchicalConfig = new MockConfigManager();

            List<TypeMapping> typeMappings = new List<TypeMapping>();
            typeMappings.Add(new TypeMapping() { FromAssembly = "1" });
            typeMappings.Add(new TypeMapping() { FromAssembly = "2" });
            typeMappings.Add(new TypeMapping() { FromAssembly = "3" });

            hierarchicalConfig.Value = new List<TypeMapping>(typeMappings);

            var target = new ServiceLocatorConfig(hierarchicalConfig);

            target.RemoveTypeMapping(typeMappings[0]);
            IEnumerable<TypeMapping> registeredTypeMappings = target.GetTypeMappings();
            Assert.AreEqual(2, registeredTypeMappings.Count());
            Assert.AreSame(typeMappings[1], registeredTypeMappings.First());
            Assert.AreSame(typeMappings[2], registeredTypeMappings.ElementAt(1));
        }
    }

    interface ISomething
    {
        
    }

    class Something : ISomething
    {
        
    }

    class SomethingElse : ISomething
    {

    }

    class MockConfigManager : IConfigManager
    {
        public object Value;

        public void RemoveKeyFromPropertyBag(string key, SPFarm propertyBag)
        {
            throw new NotImplementedException();
        }

        public void RemoveKeyFromPropertyBag(string key, SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public void RemoveKeyFromPropertyBag(string key, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public void RemoveKeyFromPropertyBag(string key, SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, SPFarm propertyBag)
        {
            return true;
        }

        public bool ContainsKeyInPropertyBag(string key, SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, SPFarm propertyBag)
        {
            this.Value = value;
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPFarm propertyBag)
        {
            return (TValue)Value;
        }
    }

}
