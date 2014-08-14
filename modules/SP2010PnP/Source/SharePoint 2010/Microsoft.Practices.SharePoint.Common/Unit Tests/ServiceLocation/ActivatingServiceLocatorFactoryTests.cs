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
using Microsoft.Practices.ServiceLocation.Moles;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.SharePoint.Common.Tests.ServiceLocation
{
    [TestClass]
    public class ActivatingServiceLocatorFactoryTests
    {
        [TestMethod]
        public void FactoryCreatesServiceLocator()
        {
            //Arrange
            var target = new ActivatingServiceLocatorFactory();

            //Act
            var serviceLocator = target.Create() as ActivatingServiceLocator;

            //Assert
            Assert.IsNotNull(serviceLocator);
        }

        [TestMethod]
        public void CanLoadTypeMappings()
        {
            //Arrange
            var target = new ActivatingServiceLocatorFactory();
            var typeMappings = new List<TypeMapping>();
            typeMappings.Add(TypeMapping.Create<ISomething, Something>());
            var serviceLocator = new ActivatingServiceLocator();

            //Act
            target.LoadTypeMappings(serviceLocator, typeMappings);

            //Assert
            Assert.IsTrue(serviceLocator.IsTypeRegistered<ISomething>());
        }

        [TestMethod]
        public void TypeMappingsNullDoesNotThrow()
        {
            //Arrange
            var target = new ActivatingServiceLocatorFactory();

            //Act
            target.LoadTypeMappings(new ActivatingServiceLocator(), null);

            //Assert is by not having the exception thrown.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowOnInvalidLocatorType()
        {
            //Arrange
            var target = new ActivatingServiceLocatorFactory();

            //Act
            target.LoadTypeMappings(
                new SServiceLocatorImplBase(), 
                new List<TypeMapping>());

            // Assert
            // throw
        }
    }
}
