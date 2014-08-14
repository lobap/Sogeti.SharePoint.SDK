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
using System.Web;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.Common.Tests.Mocks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace Microsoft.Practices.SPG.Common.Tests.ServiceLocation
{
    [TestClass]
    public class SharePointServiceLocatorFixture
    {
        [TestCleanup]
        public void Cleanup()
        {
            Isolate.CleanUp();
            SharePointServiceLocator.Reset();
        }

        [TestMethod]
        public void CanChangeServiceLocator()
        {
            var mock = new MockServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(mock);

            Assert.AreEqual(mock, SharePointServiceLocator.Current);

        }

        [TestMethod]
        public void ServiceLocatorIsLoadedWithTypes()
        {
            var typeMappings = new List<TypeMapping>
                                                       {
                                                           TypeMapping.Create<ISomething, Something>("key")
                                                       };

            SetupConfigToReturnTypeMappings(typeMappings);
            
            Assert.IsInstanceOfType(SharePointServiceLocator.Current, typeof(ActivatingServiceLocator));

            ActivatingServiceLocator target = SharePointServiceLocator.Current as ActivatingServiceLocator;
            Assert.IsTrue(target.IsTypeRegistered<ISomething>());
        }

        [TestMethod]
        public void CanRegisterDifferentServiceLocator()
        {
            var typeMappings = new List<TypeMapping>
                                                       {
                                                           TypeMapping.Create<IServiceLocatorFactory, MockServiceLocatorFactory>()
                                                       };

            SetupConfigToReturnTypeMappings(typeMappings);

            Assert.IsInstanceOfType(SharePointServiceLocator.Current, typeof(MockServiceLocator));

        }

        [TestMethod]
        public void DefaultMappingsAreAdded()
        {
            Isolate.WhenCalled(()=>SPContext.Current).ReturnRecursiveFake();
            SPWebPropertyBag prop = new SPWebPropertyBag();
            Isolate.Swap.NextInstance<SPWebPropertyBag>().With(prop);
            Isolate.WhenCalled(() => prop.GetParent()).WillReturn(null);
            var typeMappings = new List<TypeMapping>();

            SetupConfigToReturnTypeMappings(typeMappings);

            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<ILogger>(), typeof(SharePointLogger));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<ITraceLogger>(), typeof(TraceLogger));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IEventLogLogger>(), typeof(EventLogLogger));
            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<IHierarchicalConfig>(), typeof(HierarchicalConfig));
        }

        [TestMethod]
        public void CanOverrideDefaultTypemapping()
        {
            var typeMappings = new List<TypeMapping>
                                   {
                                       TypeMapping.Create<ILogger, MockLogger>()
                                   };

            SetupConfigToReturnTypeMappings(typeMappings);

            Assert.IsInstanceOfType(SharePointServiceLocator.Current.GetInstance<ILogger>(), typeof(MockLogger));

        }

        [TestMethod]
        [ExpectedException(typeof(NoSharePointContextException))]
        public void CallingServiceLocatorWithoutSharePointContextFails()
        {
            Isolate.WhenCalled(() => SPFarm.Local).WillReturn(null);
            SharePointServiceLocator.Reset();
            Assert.IsNull(SPFarm.Local, "SPFarm should be local after this message");
            var target = SharePointServiceLocator.Current;

            Assert.Fail(target.ToString());
            
        }

        [TestMethod]
        public void TheServiceLocatorFromCommonServiceLocatorThrowsClearErrorMessage()
        {
            Isolate.WhenCalled(() => SPFarm.Local).WillReturn(Isolate.Fake.Instance<SPFarm>());
            ServiceLocatorConfig fakeConfig = Isolate.Fake.Instance<ServiceLocatorConfig>();
            Isolate.Swap.NextInstance<ServiceLocatorConfig>().With(fakeConfig);
            Isolate.WhenCalled(() => fakeConfig.GetTypeMappings()).WillReturn(null);
            SharePointServiceLocator.Reset();
            var sl = SharePointServiceLocator.Current;

            try
            {
                var sl1 = ServiceLocator.Current;
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                Assert.AreEqual(ex.Message,
                                "ServiceLocator.Current is not supported. Use SharePointServiceLocator.Current instead.");
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        private void SetupConfigToReturnTypeMappings(List<TypeMapping> typeMappings)
        {
            Isolate.WhenCalled(() => SPFarm.Local).WillReturn(Isolate.Fake.Instance<SPFarm>());
            Isolate.WhenCalled(() => HttpContext.Current).WillReturn(Isolate.Fake.Instance<HttpContext>());

            var fakeConfig = Isolate.Fake.Instance<ServiceLocatorConfig>();
            Isolate.Swap.NextInstance<ServiceLocatorConfig>().With(fakeConfig);
            Isolate.WhenCalled(() => fakeConfig.GetTypeMappings()).WillReturn(typeMappings);
        }

        private class MockServiceLocator : IServiceLocator
        {
            public List<TypeMapping> TypeMappings = new List<TypeMapping>();

            public object GetService(Type serviceType)
            {
                return null;
            }

            public object GetInstance(Type serviceType)
            {
                return null;
            }

            public object GetInstance(Type serviceType, string key)
            {
                return null;
            }

            public IEnumerable<object> GetAllInstances(Type serviceType)
            {
                yield break;
            }

            public TService GetInstance<TService>()
            {
                return default(TService);
            }

            public TService GetInstance<TService>(string key)
            {
                return default(TService);
            }

            public IEnumerable<TService> GetAllInstances<TService>()
            {
                yield break;
            }
        }

        private class MockServiceLocatorFactory : IServiceLocatorFactory
        {
            public IServiceLocator Create()
            {
                return new MockServiceLocator();
            }

            public void LoadTypeMappings(IServiceLocator serviceLocator, IEnumerable<TypeMapping> typeMappings)
            {
                
            }
        }


    }
}