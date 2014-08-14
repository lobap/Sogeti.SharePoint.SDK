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
using Microsoft.SharePoint;
using TypeMock.ArrangeActAssert;
using System;
using Microsoft.SharePoint.Utilities;
using Microsoft.Practices.SPG.SubSiteCreation;

namespace Microsoft.Practices.SPG.SiteCreation.Workflow.Tests
{   
    /// <summary>
    ///This is a test class for CreateSubSiteTest and is intended
    ///to contain all CreateSubSiteTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CreateSubSiteTest
    {
        [TestCleanup]
        public void CleanupTest()
        {
            Isolate.CleanUp();
        }

        /// <summary>
        ///A test for TopLevelSiteRelativeUrl
        ///</summary>
        [TestMethod()]
        public void TopLevelSiteRelativeUrlTest()
        {
            CreateSubSiteActivity target = new CreateSubSiteActivity();
            string expected = "test";
            string actual;
            target.TopLevelSiteRelativeUrl = expected;
            actual = target.TopLevelSiteRelativeUrl;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SubSiteUrl
        ///</summary>
        [TestMethod()]
        public void SubSiteUrlTest()
        {
            CreateSubSiteActivity target = new CreateSubSiteActivity();
            string expected = "test";
            string actual;
            target.SubSiteUrl = expected;
            actual = target.SubSiteUrl;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SiteTemplateName
        ///</summary>
        [TestMethod()]
        public void SiteTemplateNameTest()
        {
            CreateSubSiteActivity target = new CreateSubSiteActivity();
            string expected = "test";
            string actual;
            target.SiteTemplateName = expected;
            actual = target.SiteTemplateName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SiteCollectionUrl
        ///</summary>
        [TestMethod()]
        public void SiteCollectionUrlTest()
        {
            CreateSubSiteActivity target = new CreateSubSiteActivity();
            string expected = "test";
            string actual;
            target.SiteCollectionUrl = expected;
            actual = target.SiteCollectionUrl;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BusinessEventIdKey
        ///</summary>
        [TestMethod()]
        public void BusinessEventIdKeyTest()
        {
            CreateSubSiteActivity target = new CreateSubSiteActivity();
            string expected = "test";
            string actual;
            target.BusinessEventIdKey = expected;
            actual = target.BusinessEventIdKey;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BusinessEventId
        ///</summary>
        [TestMethod()]
        public void BusinessEventIdTest()
        {
            CreateSubSiteActivity target = new CreateSubSiteActivity();
            string expected = "test";
            string actual;
            target.BusinessEventId = expected;
            actual = target.BusinessEventId;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BusinessEvent
        ///</summary>
        [TestMethod()]
        public void BusinessEventTest()
        {
            CreateSubSiteActivity target = new CreateSubSiteActivity();
            string expected = "test";
            string actual;
            target.BusinessEvent = expected;
            actual = target.BusinessEvent;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Microsoft.Practices.SPG.SiteCreation.Workflow.dll")]
        public void ExecuteTest()
        {
            SPSite fakeSite = Isolate.Fake.Instance<SPSite>(Members.ReturnRecursiveFakes);
            Isolate.Swap.NextInstance<SPSite>().With(fakeSite);
            SPWeb fakeTopLevelWeb = fakeSite.OpenWeb();
            Isolate.WhenCalled(() => fakeTopLevelWeb.Exists).WillReturn(true);
            SPWeb fakeWeb = fakeTopLevelWeb.Webs.Add("", "", "", uint.MinValue, "", false, false);
            Isolate.WhenCalled(() => fakeWeb.Url).WillReturn("http://localhost/unittest");
            SPPropertyBag fakePropertyBag = fakeWeb.Properties;

            CreateSubSiteActivity_Accessor target = new CreateSubSiteActivity_Accessor();
            target.BusinessEvent = "unittest";
            target.BusinessEventId = "1";
            target.BusinessEventIdKey = "unittestid";
            target.SiteCollectionUrl = "http://localhost";
            target.SiteTemplateName = "testtemplate";
            target.TopLevelSiteRelativeUrl = "/toplevelsite";

            ActivityExecutionContext executionContext = null;

            ActivityExecutionStatus expected = new ActivityExecutionStatus();
            expected = ActivityExecutionStatus.Closed;
            ActivityExecutionStatus actual;
            actual = target.Execute(executionContext);

            Isolate.Verify.WasCalledWithAnyArguments(() => fakeTopLevelWeb.Webs.Add("", "", "", uint.MinValue, "", false, false));
            Isolate.Verify.WasCalledWithExactArguments(() => fakePropertyBag["unittestid"] = "1");
            Isolate.Verify.WasCalledWithAnyArguments(()=>fakePropertyBag.Update());
            Assert.AreEqual(target.SubSiteUrl, "http://localhost/unittest");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("Microsoft.Practices.SPG.SiteCreation.Workflow.dll")]
        [ExpectedException(typeof(SubSiteCreationException), "The site template, site collection url or top level site relative url properties was not provided.")]
        public void ExecuteNullParameterValuesTest()
        {
            CreateSubSiteActivity_Accessor target = new CreateSubSiteActivity_Accessor(); 
            ActivityExecutionContext executionContext = null;
            target.Execute(executionContext);
        }

        [TestMethod()]
        [DeploymentItem("Microsoft.Practices.SPG.SiteCreation.Workflow.dll")]
        public void ExecuteEmptyTopLevelSiteRelativeUrlTest()
        {
            SPSite fakeSite = Isolate.Fake.Instance<SPSite>(Members.ReturnRecursiveFakes);
            Isolate.Swap.NextInstance<SPSite>().With(fakeSite);
            SPWeb fakeWeb = fakeSite.RootWeb.Webs.Add("", "", "", uint.MinValue, "", false, false);
            Isolate.WhenCalled(() => fakeWeb.Url).WillReturn("http://localhost/unittest");
            SPPropertyBag fakePropertyBag = fakeWeb.Properties;

            CreateSubSiteActivity_Accessor target = new CreateSubSiteActivity_Accessor();
            target.BusinessEvent = "unittest";
            target.BusinessEventId = "1";
            target.BusinessEventIdKey = "unittestid";
            target.SiteCollectionUrl = "http://localhost";
            target.SiteTemplateName = "testtemplate";

            ActivityExecutionContext executionContext = null;

            ActivityExecutionStatus expected = new ActivityExecutionStatus();
            expected = ActivityExecutionStatus.Closed;
            ActivityExecutionStatus actual;
            actual = target.Execute(executionContext);

            Isolate.Verify.WasCalledWithAnyArguments(() => fakeSite.RootWeb.Webs.Add("", "", "", uint.MinValue, "", false, false));
            Isolate.Verify.WasCalledWithExactArguments(() => fakePropertyBag["unittestid"] = "1");
            Isolate.Verify.WasCalledWithAnyArguments(() => fakePropertyBag.Update());
            Assert.AreEqual(target.SubSiteUrl, "http://localhost/unittest");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("Microsoft.Practices.SPG.SiteCreation.Workflow.dll")]
        public void ExecuteNullTopLevelSiteTest()
        {
            SPSite fakeSite = Isolate.Fake.Instance<SPSite>(Members.ReturnRecursiveFakes);
            Isolate.Swap.NextInstance<SPSite>().With(fakeSite);
            SPWeb fakeTopLevelWeb = fakeSite.RootWeb.Webs.Add("", "", "", uint.MinValue, "", false, false);
            SPWeb fakeWeb = fakeTopLevelWeb.Webs.Add("", "", "", uint.MinValue, "", false, false);
            Isolate.WhenCalled(() => fakeWeb.Url).WillReturn("http://localhost/unittest");
            SPPropertyBag fakePropertyBag = fakeWeb.Properties;

            CreateSubSiteActivity_Accessor target = new CreateSubSiteActivity_Accessor();
            target.BusinessEvent = "unittest";
            target.BusinessEventId = "1";
            target.BusinessEventIdKey = "unittestid";
            target.SiteCollectionUrl = "http://localhost";
            target.SiteTemplateName = "testtemplate";
            target.TopLevelSiteRelativeUrl = "/toplevelsite";

            ActivityExecutionContext executionContext = null;

            ActivityExecutionStatus expected = new ActivityExecutionStatus();
            expected = ActivityExecutionStatus.Closed;
            ActivityExecutionStatus actual;
            actual = target.Execute(executionContext);

            Isolate.Verify.WasCalledWithAnyArguments(() => fakeSite.RootWeb.Webs.Add("", "", "", uint.MinValue, "", false, false));
            Isolate.Verify.WasCalledWithAnyArguments(() => fakeTopLevelWeb.Webs.Add("", "", "", uint.MinValue, "", false, false));
            Isolate.Verify.WasCalledWithExactArguments(() => fakePropertyBag["unittestid"] = "1");
            Isolate.Verify.WasCalledWithAnyArguments(() => fakePropertyBag.Update());
            Assert.AreEqual(target.SubSiteUrl, "http://localhost/unittest");
            Assert.AreEqual(expected, actual);
        }
    }
}
