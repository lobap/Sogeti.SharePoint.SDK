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
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.SubSiteCreation;
using TypeMock.ArrangeActAssert;
using Contoso.PartnerPortal.PartnerDirectory;

namespace Contoso.PartnerPortal.Portal.Tests
{
    

    /// <summary>
    ///This is a test class for PartnerRedirectPresenterTest and is intended
    ///to contain all PartnerRedirectPresenterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PartnerRedirectPresenterFixture
    {
        [TestCleanup]
        public void CleanupTest()
        {
            Isolate.CleanUp();
            SharePointServiceLocator.Reset();
        }

        /// <summary>
        ///A test for Redirect
        ///</summary>
        [TestMethod()]
        public void RedirectToHomeTest()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<IPartnerSiteDirectory, MockPartnerSiteDirectory>());

            HttpContext fakeContext = Isolate.Fake.Instance<HttpContext>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => HttpContext.Current).WillReturn(fakeContext);
            MockHttpResponse mockHttpResponse = new MockHttpResponse();
            Isolate.Swap.CallsOn(HttpContext.Current.Response).WithCallsTo(mockHttpResponse);

            PartnerRedirectController target = new PartnerRedirectController();
            string queryParamValue = "home";
            target.Redirect(queryParamValue);
            Assert.AreEqual("/MySite/MyPage", mockHttpResponse.RedirectUrl);
        }

        [TestMethod]
        public void RedirectToBusinessEventTest()
        {
            MockBusinessEventTypeConfiguration.ReturnValue = new BusinessEventTypeConfiguration { TopLevelSiteRelativeUrl = "incident" };
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<IBusinessEventTypeConfigurationRepository, MockBusinessEventTypeConfiguration>()
                .RegisterTypeMapping<IPartnerSiteDirectory, MockPartnerSiteDirectory>());

            HttpContext fakeContext = Isolate.Fake.Instance<HttpContext>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => HttpContext.Current).WillReturn(fakeContext);
            MockHttpResponse mockHttpResponse = new MockHttpResponse();
            Isolate.Swap.CallsOn(HttpContext.Current.Response).WithCallsTo(mockHttpResponse);

            SPSite fakeSite = Isolate.Fake.Instance<SPSite>(Members.ReturnRecursiveFakes);
            Isolate.Swap.NextInstance<SPSite>().With(fakeSite);
            SPWeb fakeWeb = fakeSite.OpenWeb("");
            Isolate.WhenCalled(() => fakeWeb.Exists).WillReturn(true);
            Isolate.WhenCalled(() => fakeWeb.Url).WillReturn("http://localhost/incidents");

            TestablePartnerRedirectController target = new TestablePartnerRedirectController();
            string queryParamValue = "incident";
            target.Redirect(queryParamValue);
            Assert.AreEqual("/incidents", mockHttpResponse.RedirectUrl);
        }

        [TestMethod]
        public void RedirectToBusinessEventNoTopLevelSite()
        {
            MockBusinessEventTypeConfiguration.ReturnValue = new BusinessEventTypeConfiguration { TopLevelSiteRelativeUrl = "incident" };
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<IBusinessEventTypeConfigurationRepository, MockBusinessEventTypeConfiguration>()
                .RegisterTypeMapping<IPartnerSiteDirectory, MockPartnerSiteDirectory>());

            HttpContext fakeContext = Isolate.Fake.Instance<HttpContext>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => HttpContext.Current).WillReturn(fakeContext);
            MockHttpResponse mockHttpResponse = new MockHttpResponse();
            Isolate.Swap.CallsOn(HttpContext.Current.Response).WithCallsTo(mockHttpResponse);

            SPSite fakeSite = Isolate.Fake.Instance<SPSite>(Members.ReturnRecursiveFakes);
            Isolate.Swap.NextInstance<SPSite>().With(fakeSite);
            SPWeb fakeWeb = fakeSite.OpenWeb("");
            Isolate.WhenCalled(() => fakeWeb.Exists).WillReturn(false);

            TestablePartnerRedirectController target = new TestablePartnerRedirectController();
            string queryParamValue = "incident";
            target.Redirect(queryParamValue);
            Assert.AreEqual("/MySite/MyPage", mockHttpResponse.RedirectUrl);
        }
    }

    public class MockBusinessEventTypeConfiguration : IBusinessEventTypeConfigurationRepository
    {
        public static BusinessEventTypeConfiguration ReturnValue;
        public MockBusinessEventTypeConfiguration()
        {

        }

        #region IBusinessEventTypeConfigurationRepository Members

        public BusinessEventTypeConfiguration GetBusinessEventTypeConfiguration(string businessEvent)
        {
            return ReturnValue;
        }

        #endregion
    }

    class TestablePartnerRedirectController : PartnerRedirectController
    {
        protected override void RunElevated(SPSecurity.CodeToRunElevated method)
        {
            method.Invoke();
        }
    }

    class MockHttpResponse
    {
        public string RedirectUrl
        {
            get;
            set;
        }

        void Redirect(string url)
        {
            RedirectUrl = url;
        }
    }

    public class MockPartnerSiteDirectory : IPartnerSiteDirectory
    {
        public MockPartnerSiteDirectory()
        {

        }

        #region IPartnerSiteDirectory Members

        public string GetCurrentPartnerId()
        {
            throw new NotImplementedException();
        }

        public string GetPartnerSiteCollectionUrl()
        {
            return "http://localhost/MySite/MyPage";
        }

        public string GetPartnerSiteCollectionUrl(string partnerId)
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