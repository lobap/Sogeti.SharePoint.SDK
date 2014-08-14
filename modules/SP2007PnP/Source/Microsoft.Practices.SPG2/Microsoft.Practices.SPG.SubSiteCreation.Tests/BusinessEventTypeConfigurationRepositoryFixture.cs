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
using Microsoft.Practices.SPG.SubSiteCreation.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using TypeMock.ArrangeActAssert;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using System;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.Common.Configuration;

namespace Microsoft.Practices.SPG.SubSiteCreation.Tests
{
    public class MockConfigManager : IHierarchicalConfig
    {
        public static string ReturnValue;

        public TValue GetByKey<TValue>(string key)
        {
            return (TValue)(object)ReturnValue;
        }

        public TValue GetByKey<TValue>(string key, ConfigLevel level)
        {
            return (TValue)(object)ReturnValue;
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key, ConfigLevel level)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///This is a test class for BusinessEventTypeConfigurationRepositoryTest and is intended
    ///to contain all BusinessEventTypeConfigurationRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BusinessEventTypeConfigurationRepositoryFixture
    {
        [TestCleanup]
        public void Cleanup()
        {
            Isolate.CleanUp();
            SharePointServiceLocator.Reset();
        }

        /// <summary>
        ///A test for GetBusinessEventTypeConfiguration
        ///</summary>
        [TestMethod()]
        public void GetBusinessEventTypeConfigurationTest()
        {
            MockConfigManager.ReturnValue = "http://localhost";
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator().RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>());

            SPSite fakeSite = Isolate.Fake.Instance<SPSite>(Members.ReturnRecursiveFakes);
            Isolate.Swap.NextInstance<SPSite>().With(fakeSite);

            SPWeb fakeWeb = Isolate.Fake.Instance<SPWeb>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => fakeSite.OpenWeb()).WillReturn(fakeWeb);
            
            Guid SiteTemplateFieldId = new Guid("84267e40-7f47-4f40-b3be-4004312eb467");
            Guid BusinessEventIdKeyFieldId = new Guid("3D6B9777-FDA5-4639-9316-B39D3E060573");
            Guid TopLevelSiteRelativeUrlFieldId = new Guid("E87A0DE5-29D7-461e-9C83-67C2D291B54C");

            SPList fakeList = fakeWeb.Lists["Business Event Type Configuration"];
            SPListItemCollection fakeItems = fakeList.GetItems(new SPQuery());
            Isolate.WhenCalled(() => fakeItems.Count).WillReturn(1);

            var fakeItem = fakeItems[0];
            Isolate.WhenCalled(() => fakeItem[SiteTemplateFieldId]).WithExactArguments().WillReturn("TESTSTP");
            Isolate.WhenCalled(() => fakeItem[BusinessEventIdKeyFieldId]).WithExactArguments().WillReturn("TESTKEY");
            Isolate.WhenCalled(() => fakeItem[TopLevelSiteRelativeUrlFieldId]).WithExactArguments().WillReturn("http://testurl");

            SPFarmPropertyBag spFarmPropertyBag = Isolate.Fake.Instance<SPFarmPropertyBag>();
            Isolate.Swap.NextInstance<SPFarmPropertyBag>().With(spFarmPropertyBag);

            string businessEvent = string.Empty;
            BusinessEventTypeConfiguration expected = new BusinessEventTypeConfiguration();
            expected.BusinessEventIdKey = "TESTKEY";
            expected.SiteTemplate = "TESTSTP";
            expected.TopLevelSiteRelativeUrl = "http://testurl";

            BusinessEventTypeConfigurationRepository target = new BusinessEventTypeConfigurationRepository();
            BusinessEventTypeConfiguration actual;
            actual = target.GetBusinessEventTypeConfiguration(businessEvent);
            Assert.AreEqual(expected.BusinessEventIdKey, actual.BusinessEventIdKey);
            Assert.AreEqual(expected.SiteTemplate, actual.SiteTemplate);
            Assert.AreEqual(expected.TopLevelSiteRelativeUrl, actual.TopLevelSiteRelativeUrl);
        }

        [TestMethod]
        [ExpectedException(typeof(SubSiteCreationException), "The site containing Sub Site Creation Configuration data could not be found.")]
        public void GetBusinessEventTypeConfigurationNullSubSiteCreationConfigurationSiteProperty()
        {
            MockConfigManager.ReturnValue = string.Empty;
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator().RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>());

            SPFarmPropertyBag spFarmPropertyBag = Isolate.Fake.Instance<SPFarmPropertyBag>();
            Isolate.Swap.NextInstance<SPFarmPropertyBag>().With(spFarmPropertyBag);

            BusinessEventTypeConfigurationRepository target = new BusinessEventTypeConfigurationRepository();
            target.GetBusinessEventTypeConfiguration("");
        }

        [TestMethod]
        [ExpectedException(typeof(SubSiteCreationException), "The configuration data for the {0} event could not be found.")]
        public void GetBusinessEventTypeConfigurationNoItems()
        {
            MockConfigManager.ReturnValue = "http://localhost";
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator().RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>());
            SPSite fakeSite = Isolate.Fake.Instance<SPSite>(Members.ReturnRecursiveFakes);
            Isolate.Swap.NextInstance<SPSite>().With(fakeSite);

            SPWeb fakeWeb = Isolate.Fake.Instance<SPWeb>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => fakeSite.OpenWeb()).WillReturn(fakeWeb);

            SPFarmPropertyBag spFarmPropertyBag = Isolate.Fake.Instance<SPFarmPropertyBag>();
            Isolate.Swap.NextInstance<SPFarmPropertyBag>().With(spFarmPropertyBag);

            SPList fakeList = fakeWeb.Lists["Business Event Type Configuration"];
            SPListItemCollection fakeItems = fakeList.GetItems(new SPQuery());
            Isolate.WhenCalled(() => fakeItems.Count).WillReturn(0);

            BusinessEventTypeConfigurationRepository target = new BusinessEventTypeConfigurationRepository();
            target.GetBusinessEventTypeConfiguration("");
        }
    }
}