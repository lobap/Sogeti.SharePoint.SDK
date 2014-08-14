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
using Contoso.PartnerPortal.ContextualHelp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contoso.PartnerPortal.ContextualHelp.Tests
{
    /// <summary>
    ///This is a test class for HelpContentTest and is intended
    ///to contain all HelpContentTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HelpContentTest
    {
        /// <summary>
        ///A test for Url
        ///</summary>
        [TestMethod()]
        public void UrlTest()
        {
            HelpContent target = new HelpContent();
            string expected = "testvalue";
            string actual;
            target.Url = expected;
            actual = target.Url;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Content
        ///</summary>
        [TestMethod()]
        public void ContentTest()
        {
            HelpContent target = new HelpContent(); 
            string expected = "testvalue";
            string actual;
            target.Content = expected;
            actual = target.Content;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for HelpContent Constructor
        ///</summary>
        [TestMethod()]
        public void HelpContentConstructorTest()
        {
            HelpContent target = new HelpContent();
            Assert.IsNotNull(target);
        }
    }
}
