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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.SPG.SubSiteCreation.Tests
{
    /// <summary>
    ///This is a test class for SubSiteCreationRequestTest and is intended
    ///to contain all SubSiteCreationRequestTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SubSiteCreationRequestFixture
    {
        /// <summary>
        ///A test for SiteCollectionUrl
        ///</summary>
        [TestMethod()]
        public void SiteCollectionUrlTest()
        {
            SubSiteCreationRequest target = new SubSiteCreationRequest();
            string expected = "test";
            string actual;
            target.SiteCollectionUrl = expected;
            actual = target.SiteCollectionUrl;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for EventId
        ///</summary>
        [TestMethod()]
        public void EventIdTest()
        {
            SubSiteCreationRequest target = new SubSiteCreationRequest();
            string expected = "test";
            string actual;
            target.EventId = expected;
            actual = target.EventId;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BusinessEvent
        ///</summary>
        [TestMethod()]
        public void BusinessEventTest()
        {
            SubSiteCreationRequest target = new SubSiteCreationRequest();
            string expected = "test";
            string actual;
            target.BusinessEvent = expected;
            actual = target.BusinessEvent;
            Assert.AreEqual(expected, actual);
        }
    }
}