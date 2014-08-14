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
using Contoso.LOB.Services.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contoso.LOB.Services.BusinessEntities;
using System.Collections.Generic;

namespace Contoso.LOB.Services.Tests
{
    
    
    /// <summary>
    ///This is a test class for IncidentManagementTest and is intended
    ///to contain all IncidentManagementTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IncidentManagementFixture
    {
        private ISecurityHelper mockSecurityHelper;

        [TestInitialize]
        public void Init()
        {
            mockSecurityHelper = new MockSecurityHelper();
        }        

        [TestCleanup]
        public void Cleanup()
        {
            mockSecurityHelper = null;
        }

        /// <summary>
        ///A test for GetIncident
        ///</summary>
        [TestMethod()]
        public void GetIncidentTest()
        {
            TestableIncidentManagement target = new TestableIncidentManagement(); 
            target.ReplacementSecurityHelper = mockSecurityHelper;
            string incidentId = "1"; 
            Incident expected = new Incident(); 
            expected.Id = "1";
            expected.Description = "Installation issues";
            expected.Product = "X-ray machine";
            expected.Status = "Open";
            expected.Partner = "ContosoPartner1";
            expected.History = new List<string>(1);
            expected.History.Add("Incident created.");

            Incident actual;
            actual = target.GetIncident(incidentId);
            
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Product, actual.Product);
            Assert.AreEqual(expected.Status, actual.Status);
            Assert.AreEqual(expected.Partner, actual.Partner);
            Assert.AreEqual(expected.History[0], actual.History[0]);
        }
    }

    public class TestableIncidentManagement : IncidentManagement
    {
        public ISecurityHelper ReplacementSecurityHelper
        {
            set
            {
                this.SecurityHelper = value;
            }
        }
    }
}
