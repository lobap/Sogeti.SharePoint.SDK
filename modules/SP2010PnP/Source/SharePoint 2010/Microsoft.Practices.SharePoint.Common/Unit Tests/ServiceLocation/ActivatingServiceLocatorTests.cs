//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.SharePoint.Common.Tests.ServiceLocation
{
    /// <summary>
    /// Summary description for ServiceLocatorFixture
    /// </summary>
    [TestClass]
    public class ActivatingServiceLocatorTests
    {
        [TestMethod]
        public void CanRegisterAndResolveTypeMapping()
        {
            //Arrange
            var target = new ActivatingServiceLocator();

            //Act
            target.RegisterTypeMapping<IMyObject, MyObject>();
            var result = target.GetInstance<IMyObject>();

            //Assert
            Assert.IsInstanceOfType(result, typeof(MyObject));
        }

        [TestMethod]
        public void LastRegistrationWins()
        {
            //Arrange
            var target = new ActivatingServiceLocator();

            //Act
            target.RegisterTypeMapping<IMyObject, MyObject>();
            target.RegisterTypeMapping<IMyObject, MyObject2>();

            //Assert
            Assert.IsInstanceOfType(target.GetInstance<IMyObject>(), typeof(MyObject2));
        }

        [TestMethod]
        [ExpectedException(typeof(ActivationException))]
        public void GetWithoutKeyThrows()
        {
            //Arrange
            var target = new ActivatingServiceLocator();

            //Act
            target.RegisterTypeMapping<IMyObject, MyObject>("key1");
            var result = target.GetInstance<IMyObject>();

            //Assert
            Assert.IsInstanceOfType(result, typeof(MyObject));
        }

        [TestMethod]
        public void CanRegisterMultipleTypes()
        {
            //Arrange
            var target = new ActivatingServiceLocator();

            //Act
            target.RegisterTypeMapping<IMyObject, MyObject>("key1");
            target.RegisterTypeMapping<IMyObject, MyObject2>("key2");
            var result = target.GetInstance<IMyObject>("key1");
            
            //Assert
            Assert.IsInstanceOfType(result, typeof(MyObject));
        }

        [TestMethod]
        public void CanResolveAll()
        {
            //Arrange
            var target = new ActivatingServiceLocator();

            //Act
            target.RegisterTypeMapping<IMyObject, MyObject>("key1");
            target.RegisterTypeMapping<IMyObject, MyObject2>("key2");
            var result = target.GetAllInstances<IMyObject>();

            //Assert
            Assert.IsInstanceOfType(result.First(), typeof(MyObject));
            Assert.IsInstanceOfType(result.Skip(1).First(), typeof(MyObject2));
        }

        [TestMethod]
        public void CanResolveByKey()
        {
            //Arrange
            var target = new ActivatingServiceLocator();

            //Act
            target.RegisterTypeMapping<IMyObject, MyObject>("key1");
            target.RegisterTypeMapping<IMyObject, MyObject2>("key2");

            //Assert
            var result = target.GetInstance<IMyObject>("key2");
            Assert.IsInstanceOfType(result, typeof(MyObject2));
        }


        [TestMethod]
        public void CanRegisterSingleton()
        {
            //Arrange
            var locator = new ActivatingServiceLocator();

            //Act
            locator.RegisterTypeMapping(TypeMapping.Create<IMyObject, MyObject>(InstantiationType.AsSingleton));

            //Assert
            Assert.AreSame(locator.GetInstance<IMyObject>(), locator.GetInstance<IMyObject>());
        }

        [TestMethod]
        public void ResolveAllRetrievesSingletons()
        {
            //Arrange
            var locator = new ActivatingServiceLocator();

            //Act
            locator.RegisterTypeMapping(TypeMapping.Create<IMyObject, MyObject>("key1", InstantiationType.AsSingleton));
            locator.RegisterTypeMapping(TypeMapping.Create<IMyObject, MyObject2>("key2", InstantiationType.AsSingleton));
            var all1 = locator.GetAllInstances<IMyObject>();
            var all2 = locator.GetAllInstances<IMyObject>();

            //Assert
            Assert.AreSame(locator.GetInstance<IMyObject>("key1"), locator.GetInstance<IMyObject>("key1"));
            Assert.AreEqual(all1.First(), all2.First());
            Assert.AreEqual(all1.Skip(1).First(), all2.Skip(1).First());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterWithNullImplThrows()
        {
            //Arrange
            ActivatingServiceLocator locator = new ActivatingServiceLocator();

            //Act
            locator.RegisterTypeMapping(null);

            // Assert (capture by ExpectedException)
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "MyObject3 does not have an implicit conversion defined for IMyObject")]
        public void RegisterWithNonMatchingTypeThrows()
        {
            //Arrange
            var locator = new ActivatingServiceLocator();

            //Act
            locator.RegisterTypeMapping(typeof(IMyObject), typeof(MyObject3));


            // Assert (capture by ExpectedException)
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "MyObject3 must be a non-abstract type with a parameterless constructor")]
        public void RegisterWithTwoInterfacesThrows()
        {
            //Arrange
            var locator = new ActivatingServiceLocator();

            //Act
            locator.RegisterTypeMapping(typeof(IMyObject), typeof(IMyObject2));


            // Assert (capture by ExpectedException)
        }

        [TestMethod]
        public void CanRegisterWithValidTypes()
        {
            //Arrange
            ActivatingServiceLocator locator = new ActivatingServiceLocator();

            //Act
            locator.RegisterTypeMapping(typeof(IMyObject2), typeof(MyObject3));
            var instance = locator.GetInstance<IMyObject2>();

            //Assert
            Assert.IsInstanceOfType(instance, typeof(MyObject3));
        }

        [TestMethod]
        public void IsTypeRegistered_WithRegisteredService_returnsTrue()
        {
            //Arrange
            ActivatingServiceLocator locator = new ActivatingServiceLocator();

            //Act
            locator.RegisterTypeMapping(typeof(IMyObject2), typeof(MyObject3));
            bool target = locator.IsTypeRegistered(typeof (IMyObject2));

            //Assert
            Assert.IsTrue(target);
        }

        [TestMethod]
        public void IsTypeRegistered_WithKeyAndRegisteredService_returnsTrue()
        {
            //Arrange
            ActivatingServiceLocator locator = new ActivatingServiceLocator();
            string key = "foobar";

            //Act
            locator.RegisterTypeMapping(typeof(IMyObject2), typeof(MyObject3), key, InstantiationType.NewInstanceForEachRequest);
            bool target = locator.IsTypeRegistered(typeof(IMyObject2), key);

            //Assert
            Assert.IsTrue(target);
        }

        [TestMethod]
        public void IsTypeRegistered_WithoutRegisteredService_returnsFalse()
        {
            //Arrange
            ActivatingServiceLocator locator = new ActivatingServiceLocator();

            //Act
            locator.RegisterTypeMapping(typeof(IMyObject2), typeof(MyObject3));
            bool target = locator.IsTypeRegistered(typeof(IMyObject));

            //Assert
            Assert.IsFalse(target);
        }
    }

    public class MyObject : IMyObject
    {
    }

    public class MyObject2 : IMyObject
    {
    }

    public class MyObject3 : IMyObject2
    {
    }

    public interface IMyObject
    {
    }

    public interface IMyObject2
    {
    }
}