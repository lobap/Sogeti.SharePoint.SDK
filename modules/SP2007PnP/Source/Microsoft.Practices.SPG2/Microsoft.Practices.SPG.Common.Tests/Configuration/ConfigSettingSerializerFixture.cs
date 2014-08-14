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
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.SPG.Common.Tests.Configuration
{
    [TestClass]
    public class ConfigSettingSerializerFixture
    {
         [TestMethod]
        public void CanConvertInt()
        {
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(int), 99);

            var conversionResult = target.Deserialize(typeof(int), stringValue);

            Assert.AreEqual(99, conversionResult);
        }

        [TestMethod]
        public void CanConvertString()
        {
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(string), "Blurp");

            var conversionResult = target.Deserialize(typeof(string), stringValue);

            Assert.AreEqual("Blurp", conversionResult);
        }

        [TestMethod]
        public void CanConvertDateTime()
        {
            var target = new ConfigSettingSerializer();
            var expected = new DateTime(2000, 1, 1, 1, 1, 1, 1);
            string stringValue = target.Serialize(typeof(DateTime), expected);

            var conversionResult = target.Deserialize(typeof(DateTime), stringValue);

            Assert.AreEqual(expected, conversionResult);
        }

        [TestMethod]
        public void CanConvertComplexObject()
        {
            var target = new ConfigSettingSerializer();
            var expected = new MockSerializableObject() {Address = "adress", Age = 1, Name="Name"};
            string stringValue = target.Serialize(typeof(MockSerializableObject), expected);

            var conversionResult = target.Deserialize(typeof(MockSerializableObject), stringValue) as MockSerializableObject;

            Assert.AreEqual(expected.Age, conversionResult.Age);            
        }

        [TestMethod]
        public void CanConvertType()
        {
            var target = new ConfigSettingSerializer();
            Type expected = typeof (ConfigSettingSerializer);
            string stringValue = target.Serialize(typeof(Type), expected);

            var conversionResult = target.Deserialize(typeof(Type), stringValue);

            Assert.AreEqual(expected, conversionResult);                        
        }

    }

    [Serializable]
    public class MockSerializableObject
    {
        public string Name;
        public string Address;
        public int Age;
    }
}
