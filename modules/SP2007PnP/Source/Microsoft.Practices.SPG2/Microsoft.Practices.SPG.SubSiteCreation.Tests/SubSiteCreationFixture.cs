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
using Microsoft.Practices.SPG.SiteCreation.Workflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.Practices.SPG.SubSiteCreation.Workflow;

using TypeMock.ArrangeActAssert;
using System.Workflow.Activities;

namespace Microsoft.Practices.SPG.SiteCreation.Workflow.Tests
{   
    /// <summary>
    ///This is a test class for SubSiteCreationTest and is intended
    ///to contain all SubSiteCreationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SubSiteCreationTest
    {
        /// <summary>
        ///A test for codeActivity1_ExecuteCode
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Microsoft.Practices.SPG.SiteCreation.Workflow.dll")]
        public void onWorkflowActivated1_InvokedTest()
        {
            SPListItem fakeItem = Isolate.Fake.Instance<SPListItem>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => fakeItem["BusinessEvent"]).WillReturn("unittest");
            Isolate.WhenCalled(() => fakeItem["EventId"]).WillReturn("1");
            Isolate.WhenCalled(() => fakeItem["SiteCollectionUrl"]).WillReturn("http://localhost");
            SPWorkflowActivationProperties fakeProperties = Isolate.Fake.Instance<SPWorkflowActivationProperties>(Members.ReturnRecursiveFakes);
            Isolate.Swap.NextInstance<SPWorkflowActivationProperties>().With(fakeProperties);
            Isolate.WhenCalled(() => fakeProperties.Item).WillReturn(fakeItem);

            SubSiteCreationActivity_Accessor target = new SubSiteCreationActivity_Accessor();
            object sender = null;
            ExternalDataEventArgs e = null;
            target.onWorkflowActivated1_Invoked(sender, e);

            Assert.AreEqual("unittest", target.businessEvent);
            Assert.AreEqual("1", target.eventId);
            Assert.AreEqual("http://localhost", target.siteCollectionUrl);
        }
    }
}
