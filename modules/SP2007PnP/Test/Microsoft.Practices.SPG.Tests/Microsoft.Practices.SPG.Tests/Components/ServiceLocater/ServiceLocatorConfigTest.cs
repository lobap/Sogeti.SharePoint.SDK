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
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SPG.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SPG.Tests
{


    /// <summary>
    ///This is a test class for ServiceLocatorConfigTest and is intended
    ///to contain all ServiceLocatorConfigTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceLocatorConfigTest
    {



        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for GetType
        ///</summary>
        [TestMethod()]
        public void SerLocConfigRemoveTypeMappings1()
        {
            IEnumerable<TypeMapping> actual;
            ServiceLocatorConfig target = new ServiceLocatorConfig(); 
            target.RegisterTypeMapping<IConfig, ConfigTest>();
            target.RegisterTypeMapping<IConfig, ConfigTest>("key");
            SharePointServiceLocator.Reset();
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IConfig>(), typeof(ConfigTest));

            actual = target.GetTypeMappings();


            target.RemoveTypeMappings<IConfig>();
            SharePointServiceLocator.Reset();
            Assert.AreEqual(actual.Count() - 2, target.GetTypeMappings().Count());


        }

        /// <summary>
        ///A test for GetType
        ///</summary>
        [TestMethod()]
        public void SerLocConfigRemoveTypeMappingKey()
        {
            List<TypeMapping> actual;
            ServiceLocatorConfig target = new ServiceLocatorConfig(); 

            target.RegisterTypeMapping<IConfig, ConfigTest>();
            target.RegisterTypeMapping<IConfig, ConfigTest>("key");
            SharePointServiceLocator.Reset();
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IConfig>(), typeof(ConfigTest));

            actual = new List<TypeMapping>(target.GetTypeMappings());

            target.RemoveTypeMapping<IConfig>("key");
            Assert.AreEqual(actual.Count - 1, target.GetTypeMappings().Count());


        }

        /// <summary>
        ///A test for GetType
        ///</summary>
        [TestMethod()]
        public void SerLocConfigRemoveTypeMappings()
        {
            List<TypeMapping> actual;
            ServiceLocatorConfig target = new ServiceLocatorConfig(); 

            target.RegisterTypeMapping<IConfig, ConfigTest>();
            SharePointServiceLocator.Reset();
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IConfig>(), typeof(ConfigTest));
            actual = new List<TypeMapping>(target.GetTypeMappings());

            target.RemoveTypeMappings<IConfig>();
            SharePointServiceLocator.Reset();
            Assert.AreEqual(actual.Count - 1, target.GetTypeMappings().Count());


        }


        /// <summary>
        ///A test for RegisterTypeMapping
        ///</summary>
        [Ignore]
        [TestMethod()]
        public void SerLocConfigRegisterTypeMappingTestHelper()
        {
            int beforeCount;
            List<TypeMapping> actual;
            ServiceLocatorConfig target = new ServiceLocatorConfig();
            actual = new List<TypeMapping>(target.GetTypeMappings());
            beforeCount = actual.Count;
            target.RegisterTypeMapping<IConfig, ConfigTest>();
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IConfig>(), typeof(ConfigTest));
            actual = new List<TypeMapping>(target.GetTypeMappings());
            
            Assert.AreEqual(beforeCount + 1, actual.Count);

        }

        [TestMethod()]
        public void SerLocConfigRegisterTypeMappingOverwritekey()
        {
            List<TypeMapping> actual;
            ServiceLocatorConfig target = new ServiceLocatorConfig();
            int beforeCount;
            target.RegisterTypeMapping<IConfig, ConfigTest>("key");
            actual = new List<TypeMapping>(target.GetTypeMappings());
            
            beforeCount = actual.Count;
            target.RegisterTypeMapping<IConfig, ConfigTest1>("key");
            SharePointServiceLocator.Reset();
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IConfig>("key"), typeof(ConfigTest1));
            actual = new List<TypeMapping>(target.GetTypeMappings());
            
            Assert.AreEqual(beforeCount, actual.Count);

        }


        /// <summary>
        ///A test for GetTypeMappings
        ///</summary>
        [Ignore]
        [TestMethod()]
        public void SerLocConfigGetTypeMappingsTest()
        {
            ServiceLocatorConfig target = new ServiceLocatorConfig();
            List<TypeMapping> actual;
            actual = new List<TypeMapping>(target.GetTypeMappings());
            Assert.IsTrue(actual.Count > 0);
            

        }


    }
    interface IConfig
    {
    }
    public class ConfigTest : IConfig
    {





    }
    public class ConfigTest1 : IConfig
    {


    }


}
