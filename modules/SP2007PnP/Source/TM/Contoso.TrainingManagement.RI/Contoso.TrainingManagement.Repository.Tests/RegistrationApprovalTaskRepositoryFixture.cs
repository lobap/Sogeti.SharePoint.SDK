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
using Contoso.TrainingManagement.Repository.BusinessEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;
using TypeMock;

using Contoso.TrainingManagement.Common.Constants;
using Contoso.TrainingManagement.Mocks;

namespace Contoso.TrainingManagement.Repository.Tests
{
    /// <summary>
    /// Summary description for RegistrationApprovalTaskRepositoryFixture
    /// </summary>
    [TestClass]
    public class RegistrationApprovalTaskRepositoryFixture
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
            MockManager.ClearAll();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void CanAdd()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPListItem item = this.RecordAddSPListItem();
            RegistrationApprovalTask registrationApprovalTask = new RegistrationApprovalTask();
            registrationApprovalTask.Title = "TestTitle";
            registrationApprovalTask.WorkflowItemId = 5;
            MockListItemRepository.SPListItemReturnedByGet = item;
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            RegistrationApprovalTaskRepository registrationApprovalTaskRepository = new RegistrationApprovalTaskRepository();

            int id = registrationApprovalTaskRepository.Add(registrationApprovalTask, web);

            Assert.AreEqual(1, id);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanGetById()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPListItem item = this.RecordGetReturnSPListItem();
            RegistrationApprovalTask registrationApprovalTask = new RegistrationApprovalTask();
            MockListItemRepository.SPListItemReturnedByGet = item;
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            RegistrationApprovalTaskRepository registrationApprovalTaskRepository = new RegistrationApprovalTaskRepository();

            registrationApprovalTask = registrationApprovalTaskRepository.Get(1, web);

            Assert.IsNotNull(registrationApprovalTask);
            Assert.AreEqual<int>(1, registrationApprovalTask.Id);
            Assert.AreEqual<string>("TestTitle", registrationApprovalTask.Title);
            Assert.AreEqual<int>(5, registrationApprovalTask.WorkflowItemId);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanUpdate()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            RegistrationApprovalTask registrationApprovalTask = new RegistrationApprovalTask();
            registrationApprovalTask.Id = 1;
            registrationApprovalTask.Title = "TestTitle";
            registrationApprovalTask.WorkflowItemId = 5;
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            RegistrationApprovalTaskRepository registrationApprovalTaskRepository = new RegistrationApprovalTaskRepository();

            registrationApprovalTaskRepository.Update(registrationApprovalTask, web);

            Assert.IsTrue(MockListItemRepository.CalledUpdateOrDelete);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanDelete()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            RegistrationApprovalTask registrationApprovalTask = new RegistrationApprovalTask();
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            RegistrationApprovalTaskRepository registrationApprovalTaskRepository = new RegistrationApprovalTaskRepository();

            registrationApprovalTaskRepository.Delete(1, web);

            Assert.IsTrue(MockListItemRepository.CalledUpdateOrDelete);
            MockManager.Verify();
        }

        #endregion

        #region SharePoint Mocks

        private SPListItem RecordAddSPListItem()
        {
            SPListItem item = RecorderManager.CreateMockedObject<SPListItem>();

            using ( RecordExpectations recorder = RecorderManager.StartRecording() )
            {
                recorder.ExpectAndReturn(item[new Guid(Fields.Id)], 1);
            }

            return item;
        }

        private SPListItem RecordGetReturnSPListItem()
        {
            SPListItem item = RecorderManager.CreateMockedObject<SPListItem>();

            using ( RecordExpectations recorder = RecorderManager.StartRecording() )
            {
                recorder.ExpectAndReturn(item[new Guid(Fields.Id)], 1);
                recorder.ExpectAndReturn(item[new Guid(Fields.Title)], "TestTitle");
                recorder.ExpectAndReturn(item[new Guid(Fields.WorkflowItemId)], 5);
            }

            return item;
        }

        #endregion
    }
}
