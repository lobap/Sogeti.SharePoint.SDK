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
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Contoso.PartnerPortal.ContextualHelp;

namespace Contoso.PartnerPortal.ContextualHelp.Tests
{
    public class MockContextualHelpView : IContextualHelpView
    {
        public string Content { get; set; }

        public MockContextualHelpView()
        {
            
        }

        #region IContextualHelpView Members

        public void SetContent(string content)
        {
            Content = content;
        }

        #endregion
    }
    /// <summary>
    ///This is a test class for ContextualHelpPresenterTest and is intended
    ///to contain all ContextualHelpPresenterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContextualHelpPresenterTest
    {
        /// <summary>
        ///A test for ContextualHelpPresenter Constructor
        ///</summary>
        [TestMethod()]
        public void ContextualHelpPresenterConstructorTest()
        {
            IContextualHelpView view = new MockContextualHelpView();
            ContextualHelpPresenter target = new ContextualHelpPresenter(view);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for SetContent
        ///</summary>
        [TestMethod()]
        public void SetContentTest()
        {
            MockContextualHelpView view = new MockContextualHelpView();
            ContextualHelpPresenter target = new ContextualHelpPresenter(view);
            string pageUrl = "/unittest";
            target.SetContent(pageUrl);
            string expected = "Test Data";
            Assert.AreEqual(expected, view.Content);
        }
    }
}
