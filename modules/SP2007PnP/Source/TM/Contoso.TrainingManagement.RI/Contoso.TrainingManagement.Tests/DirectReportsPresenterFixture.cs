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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;
using TypeMock;

using Contoso.HRManagement.Services;
using Contoso.TrainingManagement.Mocks;
using Contoso.TrainingManagement.ControlTemplates;

namespace Contoso.TrainingManagement.Tests
{
    /// <summary>
    /// Summary description for DirectReportsPresenterFixture
    /// </summary>
    [TestClass]
    public class DirectReportsPresenterFixture
    {
        #region Private Fields

        private MockDirectReportsView mockView;
        private ServiceLocator serviceLocator = ServiceLocator.GetInstance();

        #endregion

        #region Test Prep

        [TestInitialize()]
        public void TestInit()
        {
            ServiceLocator.Clear();
            this.mockView = new MockDirectReportsView();
        }

        #endregion

        #region Test Cleanup

        [TestCleanup]
        public void TestCleanup()
        {
            serviceLocator.Reset();
            MockManager.ClearAll();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void SetDirectReportsTest()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            SPWeb web = CreateMockSPWeb(false, true);
            this.mockView.Web = web;
            this.mockView.LoginName = string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager");
            this.mockView.ShowLogin = false;

            DirectReportsPresenter presenter = new DirectReportsPresenter(this.mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");
            
            //assert direct reports
            Assert.IsNotNull(this.mockView.DirectReports); 
            Assert.AreEqual("/_layouts/userdisp.aspx?ID=", this.mockView.UserDisplayUrl);
            Assert.AreEqual("&Source=http://localhost/training/manage.aspx", this.mockView.SourceUrl);
            Assert.AreEqual<int>(1, this.mockView.DirectReports.Count);
            Assert.AreEqual("SPG Employee", this.mockView.DirectReports[1]);
            Assert.AreEqual("You have 1 direct report(s).", this.mockView.DirectReportsMessage);
            MockManager.Verify();
        }

        [TestMethod]
        public void SetDirectReportsTestWithShowLogin()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            SPWeb web = CreateMockSPWeb(false, true);
            this.mockView.Web = web;
            this.mockView.LoginName = string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager");
            this.mockView.ShowLogin = true;

            DirectReportsPresenter presenter = new DirectReportsPresenter(this.mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");

            //assert direct reports
            Assert.IsNotNull(this.mockView.DirectReports);
            Assert.AreEqual("/_layouts/userdisp.aspx?ID=", this.mockView.UserDisplayUrl);
            Assert.AreEqual("&Source=http://localhost/training/manage.aspx", this.mockView.SourceUrl);
            Assert.AreEqual<int>(1, this.mockView.DirectReports.Count);
            Assert.AreEqual("SPG Employee (" + string.Format(@"{0}\{1}", Environment.MachineName, "spgemployee") + ")", this.mockView.DirectReports[1]);
            Assert.AreEqual("You have 1 direct report(s).", this.mockView.DirectReportsMessage);
            MockManager.Verify();
        }

        [TestMethod]
        public void NoDirectReportsFound()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            SPWeb web = CreateMockSPWeb(true, true);
            this.mockView.Web = web;
            this.mockView.LoginName = string.Format(@"{0}\{1}", Environment.MachineName, "newemployee");
            this.mockView.ShowLogin = false;

            DirectReportsPresenter presenter = new DirectReportsPresenter(this.mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");

            //assert error message
            Assert.IsNull(this.mockView.DirectReports);
            Assert.AreEqual("No direct reports were found.", this.mockView.DirectReportsMessage);
            MockManager.Verify();
        }

        [TestMethod]
        public void NoDirectReportsFoundInvalidUser()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            SPWeb web = CreateMockSPWeb(false, false);
            this.mockView.Web = web;
            this.mockView.LoginName = string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager");
            this.mockView.ShowLogin = false;

            DirectReportsPresenter presenter = new DirectReportsPresenter(this.mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");

            //assert error message
            Assert.IsNull(this.mockView.DirectReports);
            Assert.AreEqual("No direct reports were found.", this.mockView.DirectReportsMessage);
            MockManager.Verify();
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
            MockManager.Verify();
        }

        [TestMethod]
        public void NoDirectReportsFoundNullLoginName()
        {
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            SPWeb web = CreateMockSPWeb(true, false);
            this.mockView.Web = web;
            //LoginName not being set here 
            this.mockView.ShowLogin = false;

            DirectReportsPresenter presenter = new DirectReportsPresenter(this.mockView);

            presenter.SetDirectReportsSource("http://localhost/training/manage.aspx");

            //assert error message
            Assert.IsNull(this.mockView.DirectReports);
            Assert.AreEqual("An unexpected error occurred.", this.mockView.DirectReportsMessage);
            MockManager.Verify();
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

        #region SharePoint Mock Setup

        private SPWeb CreateMockSPWeb(bool blank, bool validUser)
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();

            if ( !blank )
            {
                SPUserCollection users = RecorderManager.CreateMockedObject<SPUserCollection>();

                if ( validUser )
                {
                    SPUser user = RecorderManager.CreateMockedObject<SPUser>();

                    using ( RecordExpectations recorder = RecorderManager.StartRecording() )
                    {
                        recorder.ExpectAndReturn(web.SiteUsers.GetCollection(new string[] { }), users);
                        recorder.ExpectAndReturn(users.Count, 1);
                        recorder.ExpectAndReturn(users[0].LoginName, string.Format(@"{0}\{1}", Environment.MachineName, "spgemployee"));
                        recorder.ExpectAndReturn(users[0], user);
                        recorder.ExpectAndReturn(user.ID, 1).RepeatAlways();
                        recorder.ExpectAndReturn(user.Name, "SPG Employee").RepeatAlways();
                        recorder.ExpectAndReturn(user.LoginName, string.Format(@"{0}\{1}", Environment.MachineName, "spgemployee")).RepeatAlways();
                    }
                }
                else
                {
                    using ( RecordExpectations recorder = RecorderManager.StartRecording() )
                    {
                        recorder.ExpectAndReturn(web.SiteUsers.GetCollection(new string[] { }), users);
                        recorder.ExpectAndReturn(users.Count, 0);
                    }
                }
            }

            return web;
        }

        #endregion
    }
}
