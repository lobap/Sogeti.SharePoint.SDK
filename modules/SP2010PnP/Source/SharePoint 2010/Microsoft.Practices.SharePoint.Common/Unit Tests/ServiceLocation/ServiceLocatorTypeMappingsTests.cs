//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.SharePoint.Common.Tests.ServiceLocation
{
    [TestClass]
    public class ServiceLocatorTypeMappingsTests
    {
        [TestMethod]
        public void Equals_ReturnsTrueWhenSameInstance()
        {
            //Arrange
            var mapping = new TypeMapping()
                              {
                                  FromAssembly = "fromAssem",
                                  FromType = "fromType",
                                  ToAssembly = "ToAssem",
                                  ToType = "ToType",
                                  Key = "Key"
                              };

            //Act
            bool target = mapping.Equals(mapping);

            //Assert
            Assert.IsTrue(target);
        }

        [TestMethod]
        public void Equals_ReturnsTrue_WhenSameValueDifferentInstances()
        {
            //Arrange
            var mapping1 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = "Key"
            };

            var mapping2 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = "Key"
            };

            //Act
            bool target = mapping1.Equals(mapping2);

            //Assert
            Assert.IsTrue(target);
        }

        [TestMethod]
        public void Equals_ReturnsTrue_WhenSameAndBothKeysNull()
        {
            //Arrange
            var mapping1 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = null
            };

            var mapping2 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = null
            };

            //Act
            bool target = mapping1.Equals(mapping2);

            //Assert
            Assert.IsTrue(target);
        }

        [TestMethod]
        public void Equals_ReturnsFalse_WhenNotSameValue()
        {
            //Arrange
            var mapping1 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = "Key1"
            };

            var mapping2 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = "Key"
            };

            //Act
            bool target = mapping1.Equals(mapping2);

            //Assert
            Assert.IsFalse(target);
        }

        [TestMethod]
        public void Equals_ReturnsFalse_WhenInstanceKeyNull()
        {
            //Arrange
            var mapping1 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = null};

            var mapping2 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = "Key"
            };

            //Act
            bool target = mapping1.Equals(mapping2);

            //Assert
            Assert.IsFalse(target);
        }

        [TestMethod]
        public void Equals_ReturnsFalse_WhenCompareKeyNull()
        {
            //Arrange
            var mapping1 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = "Key"
            };

            var mapping2 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = null
            };

            //Act
            bool target = mapping1.Equals(mapping2);

            //Assert
            Assert.IsFalse(target);
        }

        [TestMethod]
        public void GetHashCode_ReturnsDifferentValueForDifferentMappings()
        {
            //Arrange
            var mapping1 = new TypeMapping()
                               {
                                   FromAssembly = "fromAssem",
                                   FromType = "fromType",
                                   ToAssembly = "ToAssem",
                                   ToType = "ToType",
                                   Key = null
                               };

            var mapping2 = new TypeMapping()
                               {
                                   FromAssembly = "fromAssem",
                                   FromType = "fromType",
                                   ToAssembly = "ToAssem",
                                   ToType = "ToType",
                                   Key = "Key"
                               };

            //Act
            int target1 = mapping1.GetHashCode();
            int target2 = mapping2.GetHashCode();

            //Assert
            Assert.AreNotEqual(target1, target2);
        }


        [TestMethod]
        public void GetHashCode_ReturnsSameValueForSameMappings()
        {
            //Arrange
            var mapping1 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = null
            };

            var mapping2 = new TypeMapping()
            {
                FromAssembly = "fromAssem",
                FromType = "fromType",
                ToAssembly = "ToAssem",
                ToType = "ToType",
                Key = null
            };

            //Act
            int target1 = mapping1.GetHashCode();
            int target2 = mapping2.GetHashCode();

            //Assert
            Assert.AreEqual(target1, target2);
        }
    }
}
