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
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

using TypeMock;

using Contoso.TrainingManagement.Repository;
using Contoso.TrainingManagement.Repository.BusinessEntities;
using Contoso.AccountingManagement.BusinessEntities;
using Contoso.AccountingManagement.Services;
using Contoso.HRManagement.Services;
using Contoso.TrainingManagement.Common.Constants;
using Contoso.TrainingManagement.Mocks;
using Contoso.TrainingManagement.Workflows.RegistrationApproval;

namespace Contoso.TrainingManagement.Tests
{
    /// <summary>
    /// Summary description for RegistrationApprovalControllerFixture
    /// </summary>
    [TestClass]
    public class RegistrationApprovalControllerFixture
    {
        #region Private Fields

        private ConfigurationService configurationService = ConfigurationService.GetInstance();
        private ServiceLocator serviceLocator = null;

        #endregion

        #region Test Initialize

        [TestInitialize]
        public void TestInit()
        {
            configurationService.Clear();
            serviceLocator = ServiceLocator.GetInstance();
            ServiceLocator.Clear();
            MockListItemRepository.Clear();
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

        #region PopulateManagerApprovalTask

        [TestMethod]
        public void PopulateManagerApprovalTask()
        {
            Registration registration = new Registration()
            {
                Id = 1,
                Title = "TestTitle",
                CourseId = 1,
                UserId = 1,
                RegistrationStatus = "Pending"
            };
            MockRegistrationRepository.RegistrationReturnedByGet = registration;

            TrainingCourse trainingCourse = new TrainingCourse()
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Code = "TestCode",
                Cost = 123f,
                EnrollmentDate = DateTime.Today,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2)
            };
            MockTrainingCourseRepository.TrainingCourseReturnedByGet = trainingCourse;

            serviceLocator.Register<IRegistrationRepository>(typeof(MockRegistrationRepository));
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));

            SPWorkflowTaskProperties taskProperties = new SPWorkflowTaskProperties();
            SPWeb web = CreateMockSPWeb(SPWebMockType.UserAndManager);
            SPListItem item = CreateMockSPListItem(SPListItemMockType.Populate, string.Empty);

            Controller controller = new Controller();
            controller.PopulateManagerApprovalTaskProperties(taskProperties, web, item);

            Assert.AreEqual(String.Format(@"{0}\{1}", Environment.MachineName, "spgmanager").ToLower(), taskProperties.AssignedTo.ToLower());     
            Assert.AreEqual(string.Format("Approve new registration request from {0} for {1}.", "TestUser", "TestCode"), taskProperties.Title);
            MockManager.Verify();
        }

        #endregion

        #region IsManagerApprovalTaskComplete

        [TestMethod]
        public void IsManagerApprovalTaskComplete()
        {
            SPListItem item = CreateMockSPListItem(SPListItemMockType.StatusCheck, "Approved");

            bool isComplete = Controller.IsManagerApprovalTaskComplete(item);

            Assert.IsTrue(isComplete);
            MockManager.Verify();
        }

        [TestMethod]
        public void IsManagerApprovalTaskCompleteRejected()
        {
            SPListItem item = CreateMockSPListItem(SPListItemMockType.StatusCheck, "Rejected");

            bool isComplete = Controller.IsManagerApprovalTaskComplete(item);

            Assert.IsTrue(isComplete);
            MockManager.Verify();
        }

        [TestMethod]
        public void IsManagerApprovalTaskNotComplete()
        {
            SPListItem item = CreateMockSPListItem(SPListItemMockType.StatusCheck, "Pending");

            bool isComplete = Controller.IsManagerApprovalTaskComplete(item);

            Assert.IsFalse(isComplete);
            MockManager.Verify();
        }

        #endregion

        #region IsManagerApprovalTaskApproved

        [TestMethod]
        public void IsManagerApprovalTaskApproved()
        {
            SPListItem item = CreateMockSPListItem(SPListItemMockType.StatusCheck, "Approved");

            bool isApproved = Controller.IsManagerApprovalTaskApproved(item);

            Assert.IsTrue(isApproved);
            MockManager.Verify();
        }

        [TestMethod]
        public void IsManagerApprovalTaskNotApproved()
        {
            SPListItem item = CreateMockSPListItem(SPListItemMockType.StatusCheck, "Rejected");

            bool isApproved = Controller.IsManagerApprovalTaskApproved(item);

            Assert.IsFalse(isApproved);
            MockManager.Verify();
        }

        [TestMethod]
        public void IsManagerApprovalTaskNotApprovedAndIsPending()
        {
            SPListItem item = CreateMockSPListItem(SPListItemMockType.StatusCheck, "Pending");

            bool isApproved = Controller.IsManagerApprovalTaskApproved(item);

            Assert.IsFalse(isApproved);
            MockManager.Verify();
        }

        #endregion

        #region ChargeAccounting

        [TestMethod]
        public void ChargeAccounting()
        {
            Registration registration = new Registration()
            {
                Id = 1,
                Title = "TestTitle",
                CourseId = 1,
                UserId = 1,
                RegistrationStatus = "Pending"
            };
            MockRegistrationRepository.RegistrationReturnedByGet = registration;

            MockTrainingCourseRepository mockTrainingCourseRepository = new MockTrainingCourseRepository();
            TrainingCourse trainingCourse = new TrainingCourse()
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Code = "TestCode",
                Cost = 123f,
                EnrollmentDate = DateTime.Today,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2)
            };
            MockTrainingCourseRepository.TrainingCourseReturnedByGet = trainingCourse;
            
            serviceLocator.Register<IRegistrationRepository>(typeof(MockRegistrationRepository));
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));
            serviceLocator.Register<IAccountingManager>(typeof(MockAccountingManager));

            SPWeb web = CreateMockSPWeb(SPWebMockType.UserOnly);
            SPListItem item = CreateMockSPListItem(SPListItemMockType.Populate, string.Empty);

            Controller controller = new Controller();
            controller.ChargeAccounting(web, item);

            Transaction savedTransaction = MockAccountingManager.savedTransaction;
            Assert.AreEqual(123f, savedTransaction.Amount);
            Assert.AreEqual("DEP100", savedTransaction.CostCenter);
            Assert.AreEqual("Training", savedTransaction.Bucket);
            Assert.AreEqual(string.Format("{0} training course registration by {1}.", "TestTitle", "TestUser"), savedTransaction.Description);
            MockManager.Verify();
        }

        #endregion

        #endregion

        #region SharePoint Mock Setup

        private enum SPListItemMockType
        {
            Populate,
            StatusCheck
        }

        private enum SPWebMockType
        {
            UserAndManager,
            UserOnly
        }

        private SPWeb CreateMockSPWeb(SPWebMockType mockType)
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPUser user = RecorderManager.CreateMockedObject<SPUser>();
            SPUser manager = RecorderManager.CreateMockedObject<SPUser>();

            switch ( mockType )
            {
                case SPWebMockType.UserAndManager:                    
                    using ( RecordExpectations recorder = RecorderManager.StartRecording() )
                    {
                        recorder.ExpectAndReturn(web.SiteUsers.GetByID(1), user);
                        recorder.ExpectAndReturn(user.LoginName, @"domain\alias").RepeatAlways();
                        recorder.ExpectAndReturn(web.SiteUsers[1], manager);
                        recorder.ExpectAndReturn(manager.LoginName, string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager"));
                        recorder.ExpectAndReturn(user.Name, "TestUser").RepeatAlways();
                    }
                    break;
                case SPWebMockType.UserOnly:
                    using ( RecordExpectations recorder = RecorderManager.StartRecording() )
                    {
                        recorder.ExpectAndReturn(web.SiteUsers.GetByID(1), user);
                        recorder.ExpectAndReturn(user.LoginName, @"domain\alias").RepeatAlways();
                        recorder.ExpectAndReturn(user.Name, "TestUser").RepeatAlways();
                    }
                    break;
                default:
                    using ( RecordExpectations recorder = RecorderManager.StartRecording() )
                    {
                        recorder.ExpectAndReturn(web.SiteUsers.GetByID(1), user);
                        recorder.ExpectAndReturn(user.LoginName, @"domain\alias").RepeatAlways();
                        recorder.ExpectAndReturn(web.SiteUsers[1], manager);
                        recorder.ExpectAndReturn(manager.LoginName, string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager"));
                        recorder.ExpectAndReturn(user.Name, "TestUser").RepeatAlways();
                    }
                    break;
        }

            return web;
        }

        private SPListItem CreateMockSPListItem(SPListItemMockType mockType, string status)
        {
            SPListItem listItem = RecorderManager.CreateMockedObject<SPListItem>();

            switch ( mockType )
            {
                case SPListItemMockType.Populate:
                    using ( RecordExpectations recorder = RecorderManager.StartRecording() )
                    {
                        recorder.ExpectAndReturn(listItem.ID, 1);
                    }
                    break;
                case SPListItemMockType.StatusCheck:
                    using ( RecordExpectations recorder = RecorderManager.StartRecording() )
                    {
                        recorder.ExpectAndReturn(listItem[new Guid(Fields.RegistrationStatus)], status);
                    }
                    break;                
                default:
                    using ( RecordExpectations recorder = RecorderManager.StartRecording() )
                    {
                        recorder.ExpectAndReturn(listItem.ID, 1);
                    }
                    break;
            }
            
            return listItem;
        }

        #endregion
    }
}
