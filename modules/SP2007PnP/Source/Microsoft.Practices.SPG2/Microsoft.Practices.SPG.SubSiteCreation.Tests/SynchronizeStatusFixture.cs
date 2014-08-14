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
using Microsoft.SharePoint.Utilities;

namespace Microsoft.Practices.SPG.SiteCreation.Workflow.Tests
{
    /// <summary>
    ///This is a test class for SynchronizeStatusTest and is intended
    ///to contain all SynchronizeStatusTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SynchronizeStatusTest
    {
        /// <summary>
        ///A test for TargetWebUrl
        ///</summary>
        [TestMethod()]
        public void TargetWebUrlTest()
        {
            SynchronizeStatusActivity target = new SynchronizeStatusActivity();
            string expected = "test";
            string actual;
            target.TargetWebUrl = expected;
            actual = target.TargetWebUrl;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Status
        ///</summary>
        [TestMethod()]
        public void StatusTest()
        {
            SynchronizeStatusActivity target = new SynchronizeStatusActivity();
            string expected = "test";
            string actual;
            target.Status = expected;
            actual = target.Status;
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
            SPWeb fakeWeb = fakeSite.OpenWeb();
            SPPropertyBag fakePropertyBag = fakeWeb.Properties;

            SynchronizeStatusActivity_Accessor target = new SynchronizeStatusActivity_Accessor();
            target.Status = "active";
            target.TargetWebUrl = "http://localhost";


            ActivityExecutionContext executionContext = null;
            ActivityExecutionStatus expected = ActivityExecutionStatus.Closed;
            ActivityExecutionStatus actual;
            actual = target.Execute(executionContext);

            Isolate.Verify.WasCalledWithExactArguments(() => fakePropertyBag["Status"] = "active");
            Isolate.Verify.WasCalledWithAnyArguments(() => fakePropertyBag.Update());
            Assert.AreEqual(expected, actual);            
        }
    }
}
