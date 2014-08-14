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
using System.Collections.Generic;
using System.Diagnostics;
using Contoso.Common.Repositories;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Portal.SingleSignon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;
using TypeMock.ArrangeActAssert;
using Microsoft.Practices.SPG.Common.Logging;
using Contoso.Common;
using Contoso.PartnerPortal.Collaboration.Incident.ContentTypes.IncidentTask;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using System.ServiceModel.Description;
using Contoso.PartnerPortal.PartnerDirectory;

namespace Contoso.PartnerPortal.Incident.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class IncidentTaskReceiverFixture
    {
        [TestCleanup]
        public void Cleanup()
        {
            Isolate.CleanUp();
            SharePointServiceLocator.Reset();
        }

        [TestMethod]
        public void ItemAddingThrowsExceptionTest()
        {
            SPItemEventProperties properties = Isolate.Fake.Instance<SPItemEventProperties>(Members.ReturnRecursiveFakes);

            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                 .RegisterTypeMapping<ILogger, MockLogger>()
                                                 .RegisterTypeMapping<IIncidentManagementRepository, MockIncidentManagementService>()
                                                 .RegisterTypeMapping<IPartnerSiteDirectory, MockPartnerSiteDirectory>());

            IncidentTaskReceiver receiver = new IncidentTaskReceiver();
            receiver.ItemAdding(properties);

            Assert.IsTrue(properties.Cancel);
            Assert.AreEqual("Could not write the incident task to history. Please contact support.", properties.ErrorMessage);
            Assert.IsTrue(MockLogger.errorMessage.Contains("Service threw excpetion"));
        }
    }

    class MockLogger : BaseLogger
    {
        public static string errorMessage;

        public MockLogger()
        {
        }

        protected override void WriteToOperationsLog(string message, int eventId, EventLogEntryType severity, string category)
        {
            MockLogger.errorMessage = message;
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category)
        {
            MockLogger.errorMessage = message;
        }
    }

    class MockIncidentManagementService : IIncidentManagementRepository
    {
        public MockIncidentManagementService()
        {
            ClientCredentials = new ClientCredentials();
        }
        #region IIncidentManagement Members

        public Common.BusinessEntities.Incident GetIncident(string incidentId)
        {
            throw new NotImplementedException();
        }

        public void WriteToHistory(string message)
        {
            throw new ApplicationException("Service threw excpetion");
        }

        public void UpdateIncidentSiteStatus(string incidentId, string partner, string status)
        {
            throw new NotImplementedException();
        }

        public ClientCredentials ClientCredentials { get; set; }

        #endregion

        public void Dispose()
        {
        }
    }

    class MockPartnerSiteDirectory : IPartnerSiteDirectory
    {
        #region IPartnerSiteDirectory Members

        public string GetCurrentPartnerId()
        {
            return "partner1";
        }

        public string GetPartnerSiteCollectionUrl(string partnerId)
        {
            throw new NotImplementedException();
        }

        public string GetPartnerSiteCollectionUrl()
        {
            throw new NotImplementedException();
        }

        IEnumerable<PartnerSiteDirectoryEntry> IPartnerSiteDirectory.GetAllPartnerSites()
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<PartnerSiteDirectoryEntry> GetAllPartnerSites()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
