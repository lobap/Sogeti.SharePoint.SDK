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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Practices.SPG.Common.Tests.ServiceLocation
{
    /// <summary>
    /// Summary description for ServiceLocatorFixture
    /// </summary>
    [TestClass]
    public class ActivatingServiceLocatorFixture
    {

        [TestMethod]
        public void CanRegisterAndResolveTypeMapping()
        {
            var target = new ActivatingServiceLocator();
            target.RegisterTypeMapping<IMyObject, MyObject>();

            var result = target.GetInstance<IMyObject>();

            Assert.IsInstanceOfType(result, typeof(MyObject));
        }

        [TestMethod]
        public void LastRegistrationWins()
        {
            var target = new ActivatingServiceLocator();
            target.RegisterTypeMapping<IMyObject, MyObject>();
            target.RegisterTypeMapping<IMyObject, MyObject2>();

            Assert.IsInstanceOfType(target.GetInstance<IMyObject>(), typeof(MyObject2));
        }

        [TestMethod]
        [ExpectedException(typeof(ActivationException))]
        public void GetWithoutKeyThrows()
        {
            var target = new ActivatingServiceLocator();
            target.RegisterTypeMapping<IMyObject, MyObject>("key1");
            
            var result = target.GetInstance<IMyObject>();
            Assert.IsInstanceOfType(result, typeof(MyObject));
        }

        [TestMethod]
        public void CanRegisterMultipleTypes()
        {
            var target = new ActivatingServiceLocator();
            target.RegisterTypeMapping<IMyObject, MyObject>("key1");
            target.RegisterTypeMapping<IMyObject, MyObject2>("key2");

            var result = target.GetInstance<IMyObject>("key1");
            Assert.IsInstanceOfType(result, typeof(MyObject));
        }

        [TestMethod]
        public void CanResolveAll()
        {
            var target = new ActivatingServiceLocator();
            target.RegisterTypeMapping<IMyObject, MyObject>("key1");
            target.RegisterTypeMapping<IMyObject, MyObject2>("key2");

            var result = target.GetAllInstances<IMyObject>();
            Assert.IsInstanceOfType(result.First(), typeof(MyObject));
            Assert.IsInstanceOfType(result.Skip(1).First(), typeof(MyObject2));
        }

        [TestMethod]
        public void CanResolveByKey()
        {
            var target = new ActivatingServiceLocator();
            target.RegisterTypeMapping<IMyObject, MyObject>("key1");
            target.RegisterTypeMapping<IMyObject, MyObject2>("key2");

            var result = target.GetInstance<IMyObject>("key2");
            Assert.IsInstanceOfType(result, typeof(MyObject2));
        }


        [TestMethod]
        public void CanRegisterSingleton()
        {
            ActivatingServiceLocator locator = new ActivatingServiceLocator();
            locator.RegisterTypeMapping(TypeMapping.Create<IMyObject, MyObject>(InstantiationType.AsSingleton));

            Assert.AreSame(locator.GetInstance<IMyObject>(), locator.GetInstance<IMyObject>());
        }

        [TestMethod]
        public void ResolveAllRetrievesSingletons()
        {
            ActivatingServiceLocator locator = new ActivatingServiceLocator();
            locator.RegisterTypeMapping(TypeMapping.Create<IMyObject, MyObject>("key1", InstantiationType.AsSingleton));
            locator.RegisterTypeMapping(TypeMapping.Create<IMyObject, MyObject2>("key2", InstantiationType.AsSingleton));

            Assert.AreSame(locator.GetInstance<IMyObject>("key1"), locator.GetInstance<IMyObject>("key1"));
            var all1 = locator.GetAllInstances<IMyObject>();
            var all2 = locator.GetAllInstances<IMyObject>();
            Assert.AreEqual(all1.First(), all2.First());
            Assert.AreEqual(all1.Skip(1).First(), all2.Skip(1).First());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RegisterWithNullImplThrows()
        {
            ActivatingServiceLocator locator = new ActivatingServiceLocator();
            locator.RegisterTypeMapping(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "MyObject3 does not have an implicit conversion defined for IMyObject")]
        public void RegisterWithNonMatchingTypeThrows()
        {
            ActivatingServiceLocator locator = new ActivatingServiceLocator();
            locator.RegisterTypeMapping(typeof(IMyObject), typeof(MyObject3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "MyObject3 must be a non-abstract type with a parameterless constructor")]
        public void RegisterWithTwoInterfacesThrows()
        {
            ActivatingServiceLocator locator = new ActivatingServiceLocator();
            locator.RegisterTypeMapping(typeof(IMyObject), typeof(IMyObject2));
        }

        [TestMethod]
        public void CanRegisterWithValidTypes()
        {
            ActivatingServiceLocator locator = new ActivatingServiceLocator();
            locator.RegisterTypeMapping(typeof(IMyObject2), typeof(MyObject3));

            var instance = locator.GetInstance<IMyObject2>();
            Assert.IsInstanceOfType(instance, typeof(MyObject3));
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