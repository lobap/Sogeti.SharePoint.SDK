//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Moles;
using System.Xml.Serialization.Moles;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Utilities.Moles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SharePoint.Common.Logging;
using System.Runtime.Serialization;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public class ConfigSettingSerializerTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            SharePointEnvironment.Reset();
        }


        [TestMethod]
        public void Serialize_CanSerializeEnum()
        {
            //Arrange
            var expected = ConfigLevel.CurrentSPWeb;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(ConfigLevel), expected);
            var conversionResult = target.Deserialize(typeof(ConfigLevel), stringValue);

            // Assert
            Assert.AreEqual(expected, conversionResult);
        }

        [TestMethod]
        public void Serialize_DeserializeWrongEnum_ThrowsException()
        {
            //Arrange
            var expected = ConfigLevel.CurrentSPWeb;
            bool expectedExceptionThrown = false;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(ConfigLevel), expected);

            try
            {
                var conversionResult = target.Deserialize(typeof(SandboxTraceSeverity), stringValue);
            }
            catch (ArgumentException)
            {
                expectedExceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        public void Serialize_CanSerializeDeserializeBool()
        {
            // Arrange
            bool expected = true;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(bool), expected);
            var conversionResult = target.Deserialize(typeof(bool), stringValue);

            // Assert
            Assert.AreEqual(expected, conversionResult);
        }

        [TestMethod]
        public void Serialize_CanSerializeDeserializeByte()
        {
            // Arrange
            byte expected = 34;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(byte), expected);
            var conversionResult = target.Deserialize(typeof(byte), stringValue);

            // Assert
            Assert.AreEqual(expected, conversionResult);
        }

        [TestMethod]
        public void Serialize_CanSerializeDeserializeSByte()
        {
            // Arrange
            sbyte expected = -34;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(sbyte), expected);
            var conversionResult = target.Deserialize(typeof(sbyte), stringValue);

            // Assert
            Assert.AreEqual(expected, conversionResult);
        }

        [TestMethod]
        public void Serialize_CanSerializeDeserializeDouble()
        {
            // Arrange
            double expected = 0.0123458191232112;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(double), expected);
            double conversionResult = (double) target.Deserialize(typeof(double), stringValue);

            // Assert
            Assert.IsTrue(expected==conversionResult);
        }

        [TestMethod]
        public void Serialize_CanSerializeDeserializeChar()
        {
            // Arrange
            char expected = 'g';

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(char), expected);
            var conversionResult = target.Deserialize(typeof(char), stringValue);

            // Assert
            Assert.AreEqual(expected, conversionResult);
        }

        [TestMethod]
        public void Serialize_CanSerializeDeserializeSingle()
        {
            // Arrange
            Single expected = 12.4321f;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(Single), expected);
            Single conversionResult = (Single) target.Deserialize(typeof(Single), stringValue);

            // Assert
            Assert.IsTrue(expected==conversionResult);
        }


        [TestMethod]
        public void Serialize_CanSerializeDeserializeInt()
        {
            // Arrange
            int expected = -99;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(int), expected);
            var conversionResult = target.Deserialize(typeof(int), stringValue);

            // Assert
            Assert.AreEqual(expected, conversionResult);
        }


        [TestMethod]
        public void Serialize_CanSerializeDeserializeUint()
        {
            // Arrange
            uint expected = 12345;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(uint), expected);
            var conversionResult = target.Deserialize(typeof(uint), stringValue);

            // Assert
            Assert.AreEqual(expected, conversionResult);
        }

        [TestMethod]
        public void Serialize_CanSerializeDeserializeFloat()
        {
            // Arrange
            float expected = 12345;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(float), expected);
            float conversionResult = (float) target.Deserialize(typeof(float), stringValue);

            // Assert
            Assert.IsTrue(expected == conversionResult);
        }

        [TestMethod]
        public void Serialize_CanSerializeDeserializeString()
        {
            // Arrange
            string expected = "Blurp";

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(string), expected); 
            var conversionResult = target.Deserialize(typeof(string), stringValue);

            // Assert
            Assert.AreEqual(expected, conversionResult);
        }

        [TestMethod]
        public void Serialize_CanSerializeDeserializeNull()
        {
            // Arrange
            string expected = null;

            // Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(string), expected);
            var conversionResult = target.Deserialize(typeof(string), stringValue);

            // Assert
            Assert.AreEqual(expected, conversionResult);
        }


        [TestMethod]
        public void Serialize_CanSerializeDeserializeDateTime()
        {
            //Arrange
            var expected = new DateTime(2000, 1, 1, 1, 1, 1, 1);

            //Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(DateTime), expected);
            var conversionResult = target.Deserialize(typeof(DateTime), stringValue);

            //Assert
            Assert.AreEqual(expected, conversionResult);
        }

        [TestMethod]
        public void Serialize_CanSerializeDeserializeComplexObject()
        {
            //Arrange
            var expected = new MockSerializableObject() { Address = "address", Age = 1, Name = "Name" };
 
            //Act
            var target = new ConfigSettingSerializer();
            string stringValue = target.Serialize(typeof(MockSerializableObject), expected);
            var conversionResult = target.Deserialize(typeof(MockSerializableObject), stringValue) as MockSerializableObject;

            //Assert
            Assert.AreEqual(expected.Age, conversionResult.Age);
            Assert.AreEqual(expected.Address, conversionResult.Address);
            Assert.AreEqual(expected.Name, conversionResult.Name);
        }

        [TestMethod]
        [HostType("Moles")]
        public void Serialize_XmlSerializerFailure_ThrowsConfigurationException()
        {
            //Arrange
            MXmlSerializerFactory.AllInstances.CreateSerializerType = (instance, type) => { throw new TypeLoadException("error"); };
            var target = new ConfigSettingSerializer();
            var testConfig = new ServiceLocationConfigData();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target.Serialize(typeof(ServiceLocationConfigData), testConfig);
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
        public void Deerialize_XmlSerializerFailure_ThrowsConfigurationException()
        {
            //Arrange
            MXmlSerializerFactory.AllInstances.CreateSerializerType = (instance, type) => { throw new TypeLoadException("error"); };
            var target = new ConfigSettingSerializer();
            var testConfig = new ServiceLocationConfigData();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target.Deserialize(typeof(ServiceLocationConfigData), "foobar");
            }
            catch (ConfigurationException)
            {
                expectedExceptionThrown = true;
            }

            //Assert caught by expected exception
            Assert.IsTrue(expectedExceptionThrown);
        }

        [Serializable]
        public class MockSerializableObject
        {
            public string Name;
            public string Address;
            public int Age;
        }

        [TestMethod]
        public void CanConvertType()
        {
            //Arrange
            var target = new ConfigSettingSerializer();
            Type expected = typeof(ConfigSettingSerializer);
            string stringValue = target.Serialize(typeof(Type), expected);

            //Act
            var conversionResult = target.Deserialize(typeof(Type), stringValue);

            //Assert
            Assert.AreEqual(expected, conversionResult);
        }

    }
}
