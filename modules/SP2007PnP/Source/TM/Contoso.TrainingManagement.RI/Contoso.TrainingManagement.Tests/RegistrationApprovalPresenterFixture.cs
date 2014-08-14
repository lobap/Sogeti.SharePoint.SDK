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
using System.Collections.Generic;

using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TypeMock;

using Contoso.TrainingManagement.Repository;
using Contoso.TrainingManagement.Repository.BusinessEntities;
using Contoso.TrainingManagement.Forms;
using Contoso.TrainingManagement.Mocks;

namespace Contoso.TrainingManagement.Tests
{
    [TestClass]
    public class RegistrationApprovalPresenterFixture
    {
        #region Private Fields

        private ServiceLocator serviceLocator = ServiceLocator.GetInstance();

        #endregion

        #region Test Initialize

        [TestInitialize]
        public void TestInit()
        {
            ServiceLocator.Clear();
            MockTrainingCourseRepository.Clear();
            MockRegistrationRepository.Clear();
            MockRegistrationApprovalTaskRepository.Clear();
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

        #region RenderRegApprovalView

        [TestMethod]
        public void PresenterInitializesViewTextHappyCase()
        {
            var view = new MockRegistrationApprovalView();
            var presenter = new RegistrationApprovalPresenter(view);
            var mockWeb = GetMockSPWeb(true);
            var trainingCourse = new TrainingCourse() { Cost = 100, Code = "12345678", Title = "MockCourse" };
            MockTrainingCourseRepository.TrainingCourseReturnedByGet = trainingCourse;
            serviceLocator.Register<IRegistrationRepository>(typeof(MockRegistrationRepository));
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));
            serviceLocator.Register<IRegistrationApprovalTaskRepository>(typeof(MockRegistrationApprovalTaskRepository));
            MockRegistrationRepository.RegistrationReturnedByGet = new Registration();
            MockRegistrationApprovalTaskRepository.RegistrationApprovalTaskReturnedByGet = new RegistrationApprovalTask();

            bool success = presenter.RenderRegApprovalView(mockWeb, "12345");

            Assert.IsTrue(success);
            Assert.AreEqual("Registration Approval", view.HeaderTitle);
            Assert.AreEqual("Registration Approval - MockUserName", view.PageTitle);
            Assert.AreEqual("MockUserName", view.HeaderSubtitle);
            StringAssert.Contains(view.Message, "Please approve or reject the registration request by MockUserName for course: 12345678 - MockCourse.");
            StringAssert.Contains(view.Message, "The MockCourse has been requested by MockUserName.");
            StringAssert.Contains(view.Message, "The cost of this course is $100.00.");
            StringAssert.Contains(view.Message, "MockUserName can be contacted at this alias: MockUser@email.com.");
            Assert.AreEqual(2, view.Status.Count);
            Assert.IsTrue(view.Status.Contains("Approved"));
            Assert.IsTrue(view.Status.Contains("Rejected"));
            Assert.IsTrue(view.ShowConfirmationControls);
        }

        [TestMethod]
        public void RenderViewValidatesTask()
        {
            var view = new MockRegistrationApprovalView();
            var presenter = new RegistrationApprovalPresenter(view);
            var mockWeb = GetMockSPWeb(true);
            serviceLocator.Register<IRegistrationApprovalTaskRepository>(typeof(MockRegistrationApprovalTaskRepository));

            bool success = presenter.RenderRegApprovalView(mockWeb, "00000");

            Assert.IsFalse(success);
            Assert.AreEqual("Registration Approval - Failure", view.PageTitle);
            Assert.AreEqual("Failure", view.HeaderSubtitle);
            StringAssert.Contains(view.Message, "The Approval Task selected is not valid.");
            Assert.IsFalse(view.ShowConfirmationControls);
        }

        [TestMethod]
        public void RenderViewValidatesRegistration()
        {
            var view = new MockRegistrationApprovalView();
            var presenter = new RegistrationApprovalPresenter(view);
            var mockWeb = GetMockSPWeb(false);
            serviceLocator.Register<IRegistrationRepository>(typeof(MockRegistrationRepository));
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));
            serviceLocator.Register<IRegistrationApprovalTaskRepository>(typeof (MockRegistrationApprovalTaskRepository));
            MockRegistrationApprovalTaskRepository.RegistrationApprovalTaskReturnedByGet = new RegistrationApprovalTask();

            bool success = presenter.RenderRegApprovalView(mockWeb, "00000");

            Assert.IsFalse(success);
            Assert.AreEqual("Registration Approval - Failure", view.PageTitle);
            Assert.AreEqual("Failure", view.HeaderSubtitle);
            StringAssert.Contains(view.Message, "The Registration associated with the selected Approval Task is not valid.");
            Assert.IsFalse(view.ShowConfirmationControls);
        }

        [TestMethod]
        public void RenderViewValidatesCourse()
        {
            var view = new MockRegistrationApprovalView();
            var presenter = new RegistrationApprovalPresenter(view);
            var mockWeb = GetMockSPWeb(true);
            serviceLocator.Register<IRegistrationRepository>(typeof (MockRegistrationRepository));
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));
            serviceLocator.Register<IRegistrationApprovalTaskRepository>(typeof (MockRegistrationApprovalTaskRepository));
            MockRegistrationRepository.RegistrationReturnedByGet = new Registration();
            MockRegistrationApprovalTaskRepository.RegistrationApprovalTaskReturnedByGet = new RegistrationApprovalTask();

            bool success = presenter.RenderRegApprovalView(mockWeb, "00000");

            Assert.IsFalse(success);
            Assert.AreEqual("Registration Approval - Failure", view.PageTitle);
            Assert.AreEqual("Failure", view.HeaderSubtitle);
            StringAssert.Contains(view.Message, "The Course associated with the selected Approval Task is not valid.");
            Assert.IsFalse(view.ShowConfirmationControls);
        }

        [TestMethod]
        public void RenderViewValidatesUser()
        {
            var view = new MockRegistrationApprovalView();
            var presenter = new RegistrationApprovalPresenter(view);
            var mockWeb = GetMockSPWeb(false);
            serviceLocator.Register<IRegistrationRepository>(typeof(MockRegistrationRepository));
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));
            serviceLocator.Register<IRegistrationApprovalTaskRepository>(typeof(MockRegistrationApprovalTaskRepository));
            MockRegistrationRepository.RegistrationReturnedByGet = new Registration();
            MockTrainingCourseRepository.TrainingCourseReturnedByGet = new TrainingCourse();
            MockRegistrationApprovalTaskRepository.RegistrationApprovalTaskReturnedByGet = new RegistrationApprovalTask();
            
            bool success = presenter.RenderRegApprovalView(mockWeb, "00000");

            Assert.IsFalse(success);
            Assert.AreEqual("Registration Approval - Failure", view.PageTitle);
            Assert.AreEqual("Failure", view.HeaderSubtitle);
            StringAssert.Contains(view.Message, "The Employee associated with the selected Approval Task is not valid.");
            Assert.IsFalse(view.ShowConfirmationControls);
        }

        #endregion

        #region ProcessApproval

        [TestMethod]
        public void ProcessApprovalUpdatesRegistration()
        {
            var view = new MockRegistrationApprovalView();
            var presenter = new RegistrationApprovalPresenter(view);
            var mockWeb = GetMockSPWeb(true);
            var registration = new Registration();
            MockRegistrationRepository.RegistrationReturnedByGet = registration;
            MockRegistrationApprovalTaskRepository.RegistrationApprovalTaskReturnedByGet = new RegistrationApprovalTask();
            serviceLocator.Register<IRegistrationRepository>(typeof(MockRegistrationRepository));
            serviceLocator.Register<IRegistrationApprovalTaskRepository>(typeof (MockRegistrationApprovalTaskRepository));

            bool success = presenter.ProcessApproval(mockWeb, "12345", "Approved");

            Assert.IsTrue(success);
            Assert.AreSame(registration, MockRegistrationRepository.UpdateCalledWithRegistrationParam);
            Assert.AreEqual("Approved", registration.RegistrationStatus);            
        }

        [TestMethod]
        public void ProcessApprovalFailureReturnsFalse()
        {
            var view = new MockRegistrationApprovalView();
            var presenter = new RegistrationApprovalPresenter(view);
            var mockWeb = GetMockSPWeb(true);
            var registration = new Registration();
            MockRegistrationRepository.RegistrationReturnedByGet = registration;
            serviceLocator.Register<IRegistrationRepository>(typeof(MockRegistrationRepository));
            serviceLocator.Register<IRegistrationApprovalTaskRepository>(typeof(MockRegistrationApprovalTaskRepository));

            bool success = presenter.ProcessApproval(mockWeb, "12345", "Approved");

            Assert.IsFalse(success);
            Assert.AreNotSame(registration, MockRegistrationRepository.UpdateCalledWithRegistrationParam);
            Assert.IsNull(registration.RegistrationStatus);
        }

        #endregion

        #endregion

        #region SharePoint Mocks

        private SPWeb GetMockSPWeb(bool userFound)
        {
            var web = RecorderManager.CreateMockedObject<SPWeb>();
            var user = RecorderManager.CreateMockedObject<SPUser>();

            if (!userFound) user = null;

            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                SPUser spUser = web.SiteUsers.GetByID(12345);
                recorder.Return(user);
            }

            if (user != null)
            {
                using (RecordExpectations recorder = RecorderManager.StartRecording())
                {
                    string userName = user.Name;
                    recorder.Return("MockUserName").RepeatAlways();
                    string userEmail = user.LoginName;
                    recorder.Return("MockUser@email.com").RepeatAlways();
                }
            }
            return web;
        }

        #endregion

        #region Private Mocks

        private class MockRegistrationApprovalView : IRegistrationApprovalView
        {
            public MockRegistrationApprovalView()
            {
                Status = new List<string>();
            }
            public string PageTitle
            {
                get;
                set;
            }

            public string HeaderTitle
            {
                get;
                set;
            }

            public string HeaderSubtitle
            {
                get;
                set;
            }

            public string Message
            {
                get;
                set;
            }

            public IList<string> Status
            {
                get;
                set;
            }

            public bool ShowConfirmationControls
            {
                get;
                set;
            }
        }

        #endregion
    }    
}
