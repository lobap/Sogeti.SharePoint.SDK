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
using Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Workflow.ComponentModel;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.SubSiteCreation;

namespace Microsoft.Practices.SPG.SiteCreation.Workflow.Tests
{   
    /// <summary>
    ///This is a test class for ResolveSiteTemplateTest and is intended
    ///to contain all ResolveSiteTemplateTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ResolveSiteTemplateTest
    {
        [TestCleanup]
        public void CleanupTest()
        {
            SharePointServiceLocator.Reset();
        }

        /// <summary>
        ///A test for TopLevelSiteRelativeUrl
        ///</summary>
        [TestMethod()]
        public void TopLevelSiteRelativeUrlTest()
        {
            ResolveSiteTemplateActivity target = new ResolveSiteTemplateActivity();
            string expected = "test";
            string actual;
            target.TopLevelSiteRelativeUrl = expected;
            actual = target.TopLevelSiteRelativeUrl;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SiteTemplateName
        ///</summary>
        [TestMethod()]
        public void SiteTemplateNameTest()
        {
            ResolveSiteTemplateActivity target = new ResolveSiteTemplateActivity();
            string expected = "test";
            string actual;
            target.SiteTemplateName = expected;
            actual = target.SiteTemplateName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BusinessEventName
        ///</summary>
        [TestMethod()]
        public void BusinessEventNameTest()
        {
            ResolveSiteTemplateActivity target = new ResolveSiteTemplateActivity(); 
            string expected = "test";
            string actual;
            target.BusinessEventName = expected;
            actual = target.BusinessEventName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BusinessEventIdKey
        ///</summary>
        [TestMethod()]
        public void BusinessEventIdKeyTest()
        {
            ResolveSiteTemplateActivity target = new ResolveSiteTemplateActivity();
            string expected = "test";
            string actual;
            target.BusinessEventIdKey = expected;
            actual = target.BusinessEventIdKey;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Microsoft.Practices.SPG.SiteCreation.Workflow.dll")]
        public void ExecuteTest()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator().RegisterTypeMapping<IBusinessEventTypeConfigurationRepository, MockBusinessEventTypeConfigurationRepository>());

            ResolveSiteTemplateActivity_Accessor target = new ResolveSiteTemplateActivity_Accessor();
            target.BusinessEventName = "unittest";
            
            ActivityExecutionContext executionContext = null;
            ActivityExecutionStatus expected = ActivityExecutionStatus.Closed;
            ActivityExecutionStatus actual;
            actual = target.Execute(executionContext);            
            
            Assert.AreEqual(expected, actual);
            Assert.AreEqual("unittesttemplate", target.SiteTemplateName);
            Assert.AreEqual("unittestkey", target.BusinessEventIdKey);
            Assert.AreEqual("/unittest", target.TopLevelSiteRelativeUrl);
        }

        private class MockBusinessEventTypeConfigurationRepository : IBusinessEventTypeConfigurationRepository
        {
            #region IBusinessEventTypeConfigurationRepository Members

            public BusinessEventTypeConfiguration GetBusinessEventTypeConfiguration(string businessEvent)
            {
                BusinessEventTypeConfiguration configuration = new BusinessEventTypeConfiguration();
                configuration.BusinessEventIdKey = "unittestkey";
                configuration.SiteTemplate = "unittesttemplate";
                configuration.TopLevelSiteRelativeUrl = "/unittest";

                return configuration;
            }

            #endregion
        }
    }
}
