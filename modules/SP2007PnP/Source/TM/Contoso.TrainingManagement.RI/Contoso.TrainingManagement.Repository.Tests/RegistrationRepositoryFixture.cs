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

using TypeMock;

using Contoso.TrainingManagement.Repository.BusinessEntities;
using Contoso.TrainingManagement.Common.Constants;
using Contoso.TrainingManagement.Mocks;

namespace Contoso.TrainingManagement.Repository.Tests
{
    /// <summary>
    /// Summary description for RegistrationRepositoryFixture
    /// </summary>
    [TestClass]
    public class RegistrationRepositoryFixture
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
            SPWeb web = this.RecordAddReturnSPWeb();
            SPListItem item = this.RecordAddSPListItem();
            Registration registration = new Registration();
            registration.Title = "UnitTest";
            registration.CourseId = 1234;
            registration.UserId = 100;
            registration.RegistrationStatus = "Pending";
            MockListItemRepository.SPListItemReturnedByGet = item;
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            RegistrationRepository repository = new RegistrationRepository();

            int id = repository.Add(registration, web);

            Assert.AreEqual(1, id);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanGetByCourseIdUserId()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPListItem item = this.RecordGetReturnSPListItem();
            Registration registration = new Registration();
            MockListItemRepository.SPListItemReturnedByGet = item;
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            RegistrationRepository repository = new RegistrationRepository();

            registration = repository.Get(1, 1, web);

            Assert.IsNotNull(registration);
            Assert.AreEqual<int>(1, registration.Id);
            Assert.AreEqual<string>("UNITTEST", registration.Title);
            Assert.AreEqual<int>(1, registration.CourseId);
            Assert.AreEqual<int>(1, registration.UserId);
            Assert.AreEqual<string>("Pending", registration.RegistrationStatus);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanGetById()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPListItem item = this.RecordGetReturnSPListItem();
            Registration registration = new Registration();
            MockListItemRepository.SPListItemReturnedByGet = item;
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            RegistrationRepository repository = new RegistrationRepository();

            registration = repository.Get(1, web);

            Assert.IsNotNull(registration);
            Assert.AreEqual<int>(1, registration.Id);
            Assert.AreEqual<string>("UNITTEST", registration.Title);
            Assert.AreEqual<int>(1, registration.CourseId);
            Assert.AreEqual<int>(1, registration.UserId);
            Assert.AreEqual<string>("Pending", registration.RegistrationStatus);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanUpdate()
        {
            SPWeb web = this.RecordUpdateReturnSPWeb();
            Registration registration = new Registration();
            registration.Id = 1;
            registration.Title = "UnitTest";
            registration.CourseId = 1234;
            registration.UserId = 100;
            registration.RegistrationStatus = "Pending";
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            RegistrationRepository repository = new RegistrationRepository();

            repository.Update(registration, web);

            Assert.IsTrue(MockListItemRepository.CalledUpdateOrDelete);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanDelete()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            RegistrationRepository repository = new RegistrationRepository();

            repository.Delete(1, web);

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

        private SPWeb RecordAddReturnSPWeb()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPUser user = RecorderManager.CreateMockedObject<SPUser>();

            using ( RecordExpectations recorder = RecorderManager.StartRecording() )
            {
                recorder.ExpectAndReturn(web.SiteUsers.GetByID(1), user);
            }

            return web;
        }

        private SPListItem RecordGetReturnSPListItem()
        {
            SPListItem item = RecorderManager.CreateMockedObject<SPListItem>();

            using ( RecordExpectations recorder = RecorderManager.StartRecording() )
            {
                recorder.ExpectAndReturn(item[new Guid(Fields.Id)], 1);
                recorder.ExpectAndReturn(item[new Guid(Fields.Title)], "UNITTEST");
                recorder.ExpectAndReturn(item[new Guid(Fields.CourseId)], 1);
                recorder.ExpectAndReturn(item[new Guid(Fields.UserId)], 1);
                recorder.ExpectAndReturn(item[new Guid(Fields.RegistrationStatus)], "Pending");
            }

            return item;
        }

        private SPWeb RecordUpdateReturnSPWeb()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPUser user = RecorderManager.CreateMockedObject<SPUser>();

            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                recorder.ExpectAndReturn(web.SiteUsers.GetByID(1), user);
            }

            return web;
        }

        #endregion
    }
}