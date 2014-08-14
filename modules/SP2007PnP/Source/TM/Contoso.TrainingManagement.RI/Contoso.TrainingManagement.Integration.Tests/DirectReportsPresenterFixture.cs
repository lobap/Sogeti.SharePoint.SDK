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
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;

using Contoso.HRManagement.Services;
using Contoso.TrainingManagement.Mocks;
using Contoso.TrainingManagement.ControlTemplates;

namespace Contoso.TrainingManagement.IntegrationTests
{
    /// <summary>
    /// Summary description for DirectReportsPresenterFixture
    /// </summary>
    [TestClass]
    public class DirectReportsPresenterFixture
    {
        #region Private Fields

        private MockDirectReportsView mockView;
        private readonly ServiceLocator serviceLocator = ServiceLocator.GetInstance();
        private readonly string siteUrl = ConfigurationManager.AppSettings["SiteUrl"];
        private SPWeb web;

        #endregion

        #region Test Prep

        [TestInitialize()]
        public void TestInit()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                web = site.AllWebs.Add(Guid.NewGuid().ToString(), "", "", 1033, "CONTOSOTRAINING#0", false, false);
            }

            ServiceLocator.Clear();
            mockView = new MockDirectReportsView();
        }

        #endregion

        #region Test Cleanup

        [TestCleanup]
        public void TestCleanup()
        {
            web.Delete();
            web.Dispose();

            serviceLocator.Reset();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void SetDirectReportsTest()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            mockView.Web = web;
            mockView.LoginName = string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager");
            mockView.ShowLogin = false;

            DirectReportsPresenter presenter = new DirectReportsPresenter(mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");
            
            //assert direct reports
            Assert.IsNotNull(mockView.DirectReports); 
            Assert.AreEqual("/_layouts/userdisp.aspx?ID=", mockView.UserDisplayUrl);
            Assert.AreEqual("&Source=http://localhost/training/manage.aspx", mockView.SourceUrl);
            Assert.AreEqual(1, mockView.DirectReports.Count);
            Assert.AreEqual("You have 1 direct report(s).", mockView.DirectReportsMessage);
        }

        [TestMethod]
        public void SetDirectReportsTestWithShowLogin()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            mockView.Web = web;
            mockView.LoginName = string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager");
            mockView.ShowLogin = true;

            DirectReportsPresenter presenter = new DirectReportsPresenter(mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");

            //assert direct reports
            Assert.IsNotNull(mockView.DirectReports);
            Assert.AreEqual("/_layouts/userdisp.aspx?ID=", mockView.UserDisplayUrl);
            Assert.AreEqual("&Source=http://localhost/training/manage.aspx", mockView.SourceUrl);
            Assert.AreEqual(1, mockView.DirectReports.Count);
            Assert.AreEqual("You have 1 direct report(s).", mockView.DirectReportsMessage);
        }

        [TestMethod]
        public void NoDirectReportsFound()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            mockView.Web = web;
            mockView.LoginName = string.Format(@"{0}\{1}", Environment.MachineName, "spgemployee");
            mockView.ShowLogin = false;

            DirectReportsPresenter presenter = new DirectReportsPresenter(mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");

            //assert error message
            Assert.IsNull(mockView.DirectReports);
            Assert.AreEqual("No direct reports were found.", mockView.DirectReportsMessage);
        }

        [TestMethod]
        public void NoDirectReportsFoundInvalidUser()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            mockView.Web = web;
            mockView.LoginName = string.Format(@"{0}\{1}", Environment.MachineName, "newmanager");
            mockView.ShowLogin = false;

            DirectReportsPresenter presenter = new DirectReportsPresenter(mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");

            //assert error message
            Assert.IsNull(mockView.DirectReports);
            Assert.AreEqual("No direct reports were found.", mockView.DirectReportsMessage);
        }

        [TestMethod]
        public void NoDirectReportsFoundNullWeb()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            //Web not being set here 
            this.mockView.LoginName = string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager");
            this.mockView.ShowLogin = false;

            DirectReportsPresenter presenter = new DirectReportsPresenter(this.mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");

            //assert error message
            Assert.IsNull(this.mockView.DirectReports);
            Assert.AreEqual("An unexpected error occurred.", this.mockView.DirectReportsMessage);
        }

        [TestMethod]
        public void NoDirectReportsFoundNullLoginName()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            this.mockView.Web = web;
            //LoginName not being set here 
            this.mockView.ShowLogin = false;

            DirectReportsPresenter presenter = new DirectReportsPresenter(this.mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");

            //assert error message
            Assert.IsNull(this.mockView.DirectReports);
            Assert.AreEqual("An unexpected error occurred.", this.mockView.DirectReportsMessage);
        }

        #endregion

        #region Private Mocks

        private class MockDirectReportsView : IDirectReportsView
        {
            #region IDirectReportsView Members

            public Dictionary<int, string> DirectReports
            {
                get;
                set;                
            }

            public string UserDisplayUrl
            {
                get;
                set;
            }

            public string SourceUrl
            {
                get;
                set;
            }

            public string DirectReportsMessage
            {
                get;
                set;
            }

            public SPWeb Web
            {
                get;
                set;
            }

            public string LoginName
            {
                get;
                set;
            }

            public bool ShowLogin
            {
                get;
                set;
            }

            #endregion
        }

        #endregion
    }
}