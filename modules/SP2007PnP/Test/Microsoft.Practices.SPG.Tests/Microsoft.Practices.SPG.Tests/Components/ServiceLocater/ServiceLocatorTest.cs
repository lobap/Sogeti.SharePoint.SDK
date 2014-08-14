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
using System.Collections.Generic;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.Configuration;
using System.Linq;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SPG.Tests
{


    /// <summary>
    ///This is a test class for ServiceLocatorTest and is intended
    ///to contain all ServiceLocatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceLocatorTest
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
        ///A test for Current
        ///</summary>
        [TestMethod()]
        public void ServiceLocatorCurrentTest()
        {
            IServiceLocator actual;
            actual = SharePointServiceLocator.Current;
            Assert.IsInstanceOfType(actual.GetInstance<ILogger>(), typeof(SharePointLogger));
            Assert.IsInstanceOfType(actual.GetInstance<ITraceLogger>(), typeof(TraceLogger));
            Assert.IsInstanceOfType(actual.GetInstance<IEventLogLogger>(), typeof(EventLogLogger));
            Assert.IsInstanceOfType(actual.GetInstance<IConfigManager>(), typeof(HierarchicalConfig));
            Assert.IsInstanceOfType(actual.GetInstance<IHierarchicalConfig>(), typeof(HierarchicalConfig));

        }

        /// <summary>
        ///A test for SetServiceLocator
        ///</summary>
        [TestMethod()]
        public void SetServiceLocatorTest()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(SharePointServiceLocator.Current);

            ServiceLocatorConfig target = new ServiceLocatorConfig();
            target.RegisterTypeMapping<IFirstType, FirstTest1>();
            target.RegisterTypeMapping<IFirstType, FirstTest1>("First");
            target.RegisterTypeMapping<IFirstType, FirstTest2>("Second");
            SharePointServiceLocator.Reset();

                                                                                           
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IFirstType>(), typeof(FirstTest1));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IFirstType>("First"), typeof(FirstTest1));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IFirstType>("Second"), typeof(FirstTest2));


        }
        /// <summary>
        ///A test for SetServiceLocator
        ///</summary>
        [TestMethod()]
        public void SetServiceLocatorTest2()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(SharePointServiceLocator.Current);

                ServiceLocatorConfig target = new ServiceLocatorConfig();
            target.RegisterTypeMapping<IFirstType, FirstTest1>();
            target.RegisterTypeMapping<IFirstType, FirstTest1>("First");
            target.RegisterTypeMapping<IFirstType, FirstTest2>("Second");
            SharePointServiceLocator.Reset();                                                                           
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IFirstType>(), typeof(FirstTest1));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IFirstType>("First"), typeof(FirstTest1));            

            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<ILogger>(), typeof(SharePointLogger));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<ITraceLogger>(), typeof(TraceLogger));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IEventLogLogger>(), typeof(EventLogLogger));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IConfigManager>(), typeof(HierarchicalConfig));


        }

        /// <summary>
        ///A test for ReSet
        ///</summary>
        [TestMethod()]
        public void ServiceLocatorReSetTest()
        {
            SharePointServiceLocator.Reset();
             ServiceLocatorConfig target = new ServiceLocatorConfig();
            target.RegisterTypeMapping<IFirstType, FirstTest1>();
            target.RegisterTypeMapping<IFirstType, FirstTest1>("First");
            target.RegisterTypeMapping<IFirstType, FirstTest2>("Second");
            SharePointServiceLocator.Reset();                                                                               
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IFirstType>(), typeof(FirstTest1));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IFirstType>("First"), typeof(FirstTest1));
            
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<ILogger>(), typeof(SharePointLogger));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<ITraceLogger>(), typeof(TraceLogger));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IEventLogLogger>(), typeof(EventLogLogger));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IConfigManager>(), typeof(HierarchicalConfig));

            Assert.AreEqual(3, SharePointServiceLocator.Current.GetAllInstances<IFirstType>().Count());
            Assert.AreEqual(1, SharePointServiceLocator.Current.GetAllInstances<ILogger>().Count());
                        
            SharePointServiceLocator.Reset();            
            Assert.AreEqual(3, SharePointServiceLocator.Current.GetAllInstances<IFirstType>().Count());
            Assert.AreEqual(1, SharePointServiceLocator.Current.GetAllInstances<ILogger>().Count());


        }

        /// <summary>
        ///A test for ReSet
        ///</summary>        
        [TestMethod()]
        [Ignore()]
        //TODO: expected Exception
        public void NullInstanceTest()
        {
            SharePointServiceLocator.Reset();
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IFirstType>(), null);



        }

    }
    interface IFirstType
    { }
    public class FirstTest1 : IFirstType
    {
    }
    public class FirstTest2 : IFirstType
    {
    }
    interface ISecondType
    {
    }
    public class SecondTest1 : ISecondType
    { }
    public class SecondTest2 : ISecondType
    {
    }

}
