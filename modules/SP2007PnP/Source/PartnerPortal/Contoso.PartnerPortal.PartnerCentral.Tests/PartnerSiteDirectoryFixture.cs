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
using System.Diagnostics;
using System.Linq;
using Contoso.PartnerPortal.PartnerDirectory;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.Logging;

using TypeMock.ArrangeActAssert;

namespace Contoso.PartnerPortal.PartnerCentral.Tests
{
    /// <summary>
    ///This is a test class for PartnerSiteDirectoryTest and is intended
    ///to contain all PartnerSiteDirectoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PartnerSiteDirectoryFixture
    {
        [TestCleanup]
        public void CleanupMocks()
        {
            Isolate.CleanUp();
            SharePointServiceLocator.Reset();
        }

        /// <summary>
        ///A test for GetPartnerSiteCollectionUrl
        ///</summary>
        [TestMethod()]
        public void GetPartnerSiteCollectionUrl()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>()
                                                                      .RegisterTypeMapping<ILogger, MockLogger>(InstantiationType.AsSingleton));

            SPFarm fakeFarm = Isolate.Fake.Instance<SPFarm>();
            Isolate.WhenCalled(() => SPFarm.Local).WillReturn(fakeFarm); 
            SPSite fakeSite = Isolate.Fake.Instance<SPSite>();

            Isolate.Swap.NextInstance<SPSite>().With(fakeSite);
            SPWeb fakeWeb = fakeSite.OpenWeb();


            SPList fakeList = fakeWeb.Lists["Sites"];
            SPListItemCollection fakeItems = fakeList.GetItems(new SPQuery());
            Isolate.WhenCalled(() => fakeItems.Count).WillReturn(1);
            SPListItem fakeItem = fakeItems[0];
            Isolate.WhenCalled(() => fakeItem["URL"]).WillReturn("http://localhost, http://localhost");

            PartnerSiteDirectory target = new PartnerSiteDirectory();

            string expected = "http://localhost";
            string actual;
            actual = target.GetPartnerSiteCollectionUrl("TestPartnerId");
            MockLogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>() as MockLogger;


            Assert.AreEqual(logger.loggedMessage, string.Format("PartnerSiteDirectory FindPartnerMappingForCurrentPartner CAML: <Where><Eq><FieldRef ID='{0}'/><Value Type='Text'>TestPartnerId</Value></Eq></Where>", FieldIds.PartnerFieldId));
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A positive test for GetCurrentPartnerId
        ///</summary>
        [TestMethod]
        public void GetCurrentPartnerIdReturnPartnerId()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>()
                                                                      .RegisterTypeMapping<ILogger, MockLogger>());
            SPFarm fakeFarm = Isolate.Fake.Instance<SPFarm>();
            Isolate.WhenCalled(() => SPFarm.Local).WillReturn(fakeFarm); 
            Isolate.WhenCalled(() => ServerContext.GetContext(new SPSite(null))).WillReturn(null);
            Isolate.WhenCalled(() => SPContext.Current.Site).WillReturn(null);
            UserProfileManager fakeUserProfileManager = Isolate.Fake.Instance<UserProfileManager>();
            Isolate.Swap.NextInstance<UserProfileManager>().With(fakeUserProfileManager);
            UserProfile fakeUserProfile = Isolate.Fake.Instance<UserProfile>();
            Isolate.WhenCalled(() => fakeUserProfileManager.GetUserProfile(string.Empty)).WillReturn(fakeUserProfile);
            Isolate.WhenCalled(() => SPContext.Current.Web.CurrentUser.LoginName).WillReturn("User1");
            Isolate.WhenCalled(() => fakeUserProfileManager.UserExists("User1")).WithExactArguments().WillReturn(true);
            Isolate.WhenCalled(() => fakeUserProfile["PartnerId"].Value).WillReturn("TestPartnerId");


            PartnerSiteDirectory target = new PartnerSiteDirectory();
            string actualPartnerId = target.GetCurrentPartnerId();

            Assert.AreEqual("TestPartnerId", actualPartnerId);
        }

        /// <summary>
        ///A negative test for GetCurrentPartnerId
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(PartnerNotFoundException))]
        public void GetCurrentPartnerIdThrowsPartnerNotFoundException()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>()
                                                                      .RegisterTypeMapping<ILogger, MockLogger>());
            SPFarm fakeFarm = Isolate.Fake.Instance<SPFarm>();
            Isolate.WhenCalled(() => SPFarm.Local).WillReturn(fakeFarm);
            Isolate.WhenCalled(() => SPContext.Current.Web.CurrentUser.LoginName).WillReturn("testuser");
            UserProfileManager fakeUserProfileManager = Isolate.Fake.Instance<UserProfileManager>();
            Isolate.Swap.NextInstance<UserProfileManager>().With(fakeUserProfileManager);
            Isolate.WhenCalled(() => fakeUserProfileManager.GetUserProfile(new Guid())).WillThrow(new Exception());

            PartnerSiteDirectory target = new PartnerSiteDirectory();
            target.GetCurrentPartnerId();

        }

        [TestMethod]
        [ExpectedException(typeof(NoSharePointContextException))]
        public void GetCurrentPartnerIdShouldOnlyRunInSharePointContext()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>());

            PartnerSiteDirectory target = new PartnerSiteDirectory();
            target.GetCurrentPartnerId();
        }

        /// <summary>
        ///A test for PartnerSiteDirectory Constructor
        ///</summary>
        [TestMethod()]
        public void PartnerSiteDirectoryConstructorTest()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>());

            SPFarm fakeFarm = Isolate.Fake.Instance<SPFarm>();
            Isolate.WhenCalled(() => SPFarm.Local).WillReturn(fakeFarm);

            PartnerSiteDirectory target = new PartnerSiteDirectory();
            Assert.IsNotNull(target);
        }

        [TestMethod]
        public void GetAllPartnerSites()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>()
                                                                      .RegisterTypeMapping<ILogger, MockLogger>());

            SPFarm fakeFarm = Isolate.Fake.Instance<SPFarm>();
            Isolate.WhenCalled(() => SPFarm.Local).WillReturn(fakeFarm);

            //Not sure why SPFarm.local is returning null, therefore making these next two lines necessary.
            //Seems to be related to faking the call to GetItems.
            SPFarmPropertyBag farmPropertyBag = new SPFarmPropertyBag();
            Isolate.Swap.NextInstance<SPFarmPropertyBag>().With(farmPropertyBag);

            SPSite fakeSite = Isolate.Fake.Instance<SPSite>(Members.ReturnRecursiveFakes);
            Isolate.Swap.NextInstance<SPSite>().With(fakeSite);
            SPWeb fakeWeb = fakeSite.OpenWeb();
            SPList fakeList = fakeWeb.Lists["Sites"];
            SPListItemCollection fakeItems = Isolate.Fake.Instance<SPListItemCollection>(Members.ReturnRecursiveFakes);
            SPListItem fakeItem = Isolate.Fake.Instance<SPListItem>();
            Isolate.WhenCalled(() => fakeItems[0]).WillReturn(fakeItem);
            Isolate.WhenCalled(() => fakeList.GetItems(new SPQuery())).WillReturn(fakeItems);
            Isolate.WhenCalled(() => fakeItem["PartnerDirectoryPartnerField"]).WillReturn("TestPartner");
            Isolate.WhenCalled(() => fakeItem["URL"]).WillReturn("http://localhost");
            Isolate.WhenCalled(() => fakeItem["Title"]).WillReturn("Unit Test");

            PartnerSiteDirectory target = new PartnerSiteDirectory();
            var partnerSites = target.GetAllPartnerSites();
            Assert.AreEqual(1, partnerSites.Count());
            Assert.AreEqual("TestPartner", partnerSites.First().PartnerId);
            Assert.AreEqual("http://localhost", partnerSites.First().SiteCollectionUrl);
            Assert.AreEqual("Unit Test", partnerSites.First().Title);
        }
    }

    class MockConfigManager : IHierarchicalConfig
    {
        public TValue GetByKey<TValue>(string key)
        {
            return (TValue)(object)"http://localhost";
        }

        public TValue GetByKey<TValue>(string key, ConfigLevel level)
        {
            return (TValue)(object)"http://localhost";
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key, ConfigLevel level)
        {
            throw new NotImplementedException();
        }
    }

    class MockLogger : BaseLogger
    {
        internal string loggedMessage;
        protected override void WriteToOperationsLog(string message, int eventId, EventLogEntryType severity, string category)
        {
            loggedMessage = message;
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category)
        {
            loggedMessage = message;
        }
    }
}