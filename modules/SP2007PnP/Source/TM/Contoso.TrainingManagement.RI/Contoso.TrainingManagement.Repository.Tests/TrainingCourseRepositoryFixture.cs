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
    /// Summary description for TrainingCourseRepositoryFixture
    /// </summary>
    [TestClass]
    public class TrainingCourseRepositoryFixture
    {
        #region Private Fields

        private ConfigurationService configurationService = ConfigurationService.GetInstance();
        private ServiceLocator serviceLocator = null;

        #endregion

        #region Initialize

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
            TrainingCourse course = new TrainingCourse();
            course.Title = "TestTitle";
            course.Code = "TestCode";
            course.Description = "TestDescription";
            course.EnrollmentDate = DateTime.Today;
            course.StartDate = DateTime.Today.AddDays(1);
            course.EndDate = DateTime.Today.AddDays(2);
            course.Cost = 123f;
            MockListItemRepository.SPListItemReturnedByGet = item;
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            TrainingCourseRepository repository = new TrainingCourseRepository();

            int id = repository.Add(course, web);

            Assert.AreEqual(1, id);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanGetById()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPListItem item = this.RecordGetReturnSPListItem();
            TrainingCourse course = new TrainingCourse();
            MockListItemRepository.SPListItemReturnedByGet = item;
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            TrainingCourseRepository repository = new TrainingCourseRepository();            

            course = repository.Get(1, web);

            Assert.IsNotNull(course);
            Assert.AreEqual<int>(1, course.Id);
            Assert.AreEqual<string>("TestTitle", course.Title);
            Assert.AreEqual<string>("TestCode", course.Code);
            Assert.AreEqual<string>("TestDescription", course.Description);
            Assert.AreEqual<DateTime>(DateTime.Today, course.EnrollmentDate);
            Assert.AreEqual<DateTime>(DateTime.Today.AddDays(1), course.StartDate);
            Assert.AreEqual<DateTime>(DateTime.Today.AddDays(2), course.EndDate);
            Assert.AreEqual<float>(123f, course.Cost);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanGetByCourseCode()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPListItem item = this.RecordGetReturnSPListItem();      
            TrainingCourse course = new TrainingCourse();
            MockListItemRepository.SPListItemReturnedByGet = item;
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            TrainingCourseRepository repository = new TrainingCourseRepository(); 

            course = repository.Get("TestCode", web);

            Assert.IsNotNull(course);
            Assert.AreEqual<int>(1, course.Id);
            Assert.AreEqual<string>("TestTitle", course.Title);
            Assert.AreEqual<string>("TestCode", course.Code);
            Assert.AreEqual<string>("TestDescription", course.Description);
            Assert.AreEqual<DateTime>(DateTime.Today, course.EnrollmentDate);
            Assert.AreEqual<DateTime>(DateTime.Today.AddDays(1), course.StartDate);
            Assert.AreEqual<DateTime>(DateTime.Today.AddDays(2), course.EndDate);
            Assert.AreEqual<float>(123f, course.Cost);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanUpdate()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            TrainingCourse course = new TrainingCourse();
            course.Id = 1;
            course.Title = "TestTitle";
            course.Code = "TestCode";
            course.Description = "TestDescription";
            course.EnrollmentDate = DateTime.Today;
            course.StartDate = DateTime.Today.AddDays(1);
            course.EndDate = DateTime.Today.AddDays(2);
            course.Cost = 123f;
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            TrainingCourseRepository repository = new TrainingCourseRepository();

            repository.Update(course, web);

            Assert.IsTrue(MockListItemRepository.CalledUpdateOrDelete);
            MockManager.Verify();
        }

        [TestMethod]
        public void CanDelete()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            TrainingCourse course = new TrainingCourse();
            serviceLocator.Register<IListItemRepository>(typeof(MockListItemRepository));
            TrainingCourseRepository repository = new TrainingCourseRepository(); 

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

        private SPListItem RecordGetReturnSPListItem()
        {
            SPListItem item = RecorderManager.CreateMockedObject<SPListItem>();

            using ( RecordExpectations recorder = RecorderManager.StartRecording() )
            {                               
                recorder.ExpectAndReturn(item[new Guid(Fields.Id)], 1);
                recorder.ExpectAndReturn(item[new Guid(Fields.Title)], "TestTitle");
                recorder.ExpectAndReturn(item[new Guid(Fields.TrainingCourseCode)], "TestCode");
                recorder.ExpectAndReturn(item[new Guid(Fields.TrainingCourseDescription)], "TestDescription");
                recorder.ExpectAndReturn(item[new Guid(Fields.TrainingCourseEnrollmentDate)], DateTime.Today);
                recorder.ExpectAndReturn(item[new Guid(Fields.TrainingCourseStartDate)], DateTime.Today.AddDays(1));
                recorder.ExpectAndReturn(item[new Guid(Fields.TrainingCourseEndDate)], DateTime.Today.AddDays(2));
                recorder.ExpectAndReturn(item[new Guid(Fields.TrainingCourseCost)], 123f);
            }

            return item;
        }

        #endregion
    }
}