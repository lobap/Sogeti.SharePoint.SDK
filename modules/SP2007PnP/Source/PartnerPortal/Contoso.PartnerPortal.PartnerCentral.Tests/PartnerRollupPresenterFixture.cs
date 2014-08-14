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
using System.Linq;
using Contoso.Common.BusinessEntities;
using Contoso.Common.Repositories;
using Contoso.PartnerPortal.PartnerCentral.PartnerRollup;
using Contoso.PartnerPortal.PartnerDirectory;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contoso.PartnerPortal.PartnerCentral.Tests
{
    [TestClass]
    public class PartnerRollupPresenterFixture
    {
        [TestCleanup]
        public void Cleanup()
        {
            SharePointServiceLocator.Reset();
        }

        [TestMethod]
        public void CanLoadOpenIncidentTasks()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<IPartnerSiteDirectory, MockPartnerSiteDirectory>()
                                                                      .RegisterTypeMapping<IIncidentTaskRepository, MockIncidentTaskRepository>());

            MockPartnerRollupView  view = new MockPartnerRollupView();

            var target = new PartnerRollupPresenter(view);
            target.ReturnSearchResults();

            Assert.AreEqual(2, view.Data.Count());
            Assert.AreEqual("1", view.Data.First().Partner.PartnerId);
            Assert.AreEqual(1, view.Data.First().IncidentTasks.Count);
            Assert.AreEqual("1", view.Data.First().IncidentTasks.First().AssignedTo);
        }
    }

    public class MockIncidentTaskRepository : IIncidentTaskRepository
    {
        public IEnumerable<IncidentTask> GetAllOpenIncidentTasks()
        {
            yield return new IncidentTask()
                             {
                                 AssignedTo = "1"
                                 ,
                                 Path = "Path1"
                                 ,
                                 Priority = "Prio"
                                 ,
                                 Status = "Status"
                                 ,
                                 Title = "Title"
                             };

            yield return new IncidentTask()
                             {
                                 AssignedTo = "2"
                                 ,
                                 Path = "Path2"
                                 ,
                                 Priority = "Prio"
                                 ,
                                 Status = "Status"
                                 ,
                                 Title = "Title"
                             };
        }
    }

    public class MockPartnerSiteDirectory : IPartnerSiteDirectory
    {
        public string GetCurrentPartnerId()
        {
            throw new NotImplementedException();
        }

        public string GetPartnerSiteCollectionUrl()
        {
            throw new NotImplementedException();
        }

        public string GetPartnerSiteCollectionUrl(string partnerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PartnerSiteDirectoryEntry> GetAllPartnerSites()
        {
            yield return new PartnerSiteDirectoryEntry()
                             {
                                 PartnerId = "1"
                                 , SiteCollectionUrl = "Path1"
                                 , Title = "Title"
                             };
            yield return new PartnerSiteDirectoryEntry()
                             {
                                 PartnerId = "2"
                                 ,
                                 SiteCollectionUrl = "Path2"
                                 ,
                                 Title = "Title"
                             };
        }
    }

    public class MockPartnerRollupView : IPartnerRollupView
    {
        public IEnumerable<PartnerRollupSearchResult> Data;

        public void SetData(IEnumerable<PartnerRollupSearchResult> data)
        {
            Data = data;
        }
    }
}