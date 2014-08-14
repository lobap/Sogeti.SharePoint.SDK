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
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

using TypeMock.ArrangeActAssert;
using System;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.Common.Configuration;

namespace Microsoft.Practices.SPG.SubSiteCreation.Tests
{
    /// <summary>
    ///This is a test class for SubSiteCreationRequestsRepositoryTest and is intended
    ///to contain all SubSiteCreationRequestsRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SubSiteCreationRequestsRepositoryFixture
    {
        [TestCleanup]
        public void TestCleanup()
        {
            Isolate.CleanUp();
            SharePointServiceLocator.Reset();
        }

        /// <summary>
        ///A test for AddSubSiteCreationRequest
        ///</summary>
        [TestMethod()]
        public void AddSubSiteCreationRequestTest()
        {
            MockConfigManager.ReturnValue = "http://localhost";
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator().RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>());

            Hashtable farmProperties = new Hashtable(1);
            farmProperties.Add("SubSiteCreationConfigurationSite", "http://localhost");
            Isolate.WhenCalled(() => SPFarm.Local.Properties).WillReturn(farmProperties);

            SPSite fakeSite = Isolate.Fake.Instance<SPSite>(Members.ReturnRecursiveFakes);
            Isolate.Swap.NextInstance<SPSite>().With(fakeSite);

            SPWeb fakeWeb = Isolate.Fake.Instance<SPWeb>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => fakeSite.OpenWeb()).WillReturn(fakeWeb);

            SPList fakeList = fakeWeb.Lists["Sub Site Creation Requests"];
            SPListItem fakeItem = fakeList.Items.Add();

            SubSiteCreationRequestsRepository target = new SubSiteCreationRequestsRepository();
            SubSiteCreationRequest request = new SubSiteCreationRequest();
            request.BusinessEvent = "unittest";
            request.EventId = "0000";
            request.SiteCollectionUrl = "testurl";
            target.AddSubSiteCreationRequest(request);

            Isolate.Verify.WasCalledWithAnyArguments(()=> fakeItem.Update());
        }

        [TestMethod()]
        [ExpectedException(typeof(SubSiteCreationException), "The site containing Sub Site Creation Configuration data could not be found in Configuration using key: unittest.")]
        public void AddSubSiteCreationRequestNullSubSiteCreationConfigurationSitePropertyTest()
        {
            MockConfigManager.ReturnValue = string.Empty;
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator().RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>());

            Hashtable farmProperties = new Hashtable(0);
            Isolate.WhenCalled(() => SPFarm.Local.Properties).WillReturn(farmProperties);

            SubSiteCreationRequestsRepository target = new SubSiteCreationRequestsRepository();
            SubSiteCreationRequest request = new SubSiteCreationRequest();
            request.BusinessEvent = "unittest";
            request.EventId = "0000";
            request.SiteCollectionUrl = "testurl";
            target.AddSubSiteCreationRequest(request);
        }

        [TestMethod]
        [ExpectedException(typeof(SubSiteCreationException), "The subsite creation request was null.")]
        public void AddSubSiteCreationRequestParameterNullTest()
        {
            SubSiteCreationRequestsRepository target = new SubSiteCreationRequestsRepository();
            target.AddSubSiteCreationRequest(null);
        }

        [TestMethod]
        [ExpectedException(typeof(SubSiteCreationException), "The value provided for the BusinessEvent was null or empty.")]
        public void AddSubSiteCreationRequestNullBusinessEventTest()
        {
            SubSiteCreationRequest request = new SubSiteCreationRequest();
            request.BusinessEvent = string.Empty;
            SubSiteCreationRequestsRepository target = new SubSiteCreationRequestsRepository();
            target.AddSubSiteCreationRequest(request);
        }

        [TestMethod]
        [ExpectedException(typeof(SubSiteCreationException), "The value provided for the EventId was null or empty.")]
        public void AddSubSiteCreationRequestNullEventIdTest()
        {
            SubSiteCreationRequest request = new SubSiteCreationRequest();
            request.BusinessEvent = "UnitTest";
            request.EventId = string.Empty;
            SubSiteCreationRequestsRepository target = new SubSiteCreationRequestsRepository();
            target.AddSubSiteCreationRequest(request);
        }

        [TestMethod]
        [ExpectedException(typeof(SubSiteCreationException), "The value provided for the SiteCollectionUrl was null or empty.")]
        public void AddSubSiteCreationRequestNullSiteCollectionUrTest()
        {
            SubSiteCreationRequest request = new SubSiteCreationRequest();
            request.BusinessEvent = "UnitTest";
            request.EventId = "UnitTest";
            request.SiteCollectionUrl = string.Empty;
            SubSiteCreationRequestsRepository target = new SubSiteCreationRequestsRepository();
            target.AddSubSiteCreationRequest(request);
        }
    }
}