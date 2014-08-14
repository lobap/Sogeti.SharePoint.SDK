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
    ///This is a test class for BusinessEventTypeConfigurationTest and is intended
    ///to contain all BusinessEventTypeConfigurationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BusinessEventTypeConfigurationFixture
    {
        /// <summary>
        ///A test for TopLevelSiteRelativeUrl
        ///</summary>
        [TestMethod()]
        public void TopLevelSiteRelativeUrlTest()
        {
            BusinessEventTypeConfiguration target = new BusinessEventTypeConfiguration();
            string expected = "test";
            string actual;
            target.TopLevelSiteRelativeUrl = expected;
            actual = target.TopLevelSiteRelativeUrl;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SiteTemplate
        ///</summary>
        [TestMethod()]
        public void SiteTemplateTest()
        {
            BusinessEventTypeConfiguration target = new BusinessEventTypeConfiguration();
            string expected = "test";
            string actual;
            target.SiteTemplate = expected;
            actual = target.SiteTemplate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BusinessEventIdKey
        ///</summary>
        [TestMethod()]
        public void BusinessEventIdKeyTest()
        {
            BusinessEventTypeConfiguration target = new BusinessEventTypeConfiguration();
            string expected = "test";
            string actual;
            target.BusinessEventIdKey = expected;
            actual = target.BusinessEventIdKey;
            Assert.AreEqual(expected, actual);
        }
    }
}