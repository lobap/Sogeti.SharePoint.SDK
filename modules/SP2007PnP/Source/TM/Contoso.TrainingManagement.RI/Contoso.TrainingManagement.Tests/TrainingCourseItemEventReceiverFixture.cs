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
using Contoso.TrainingManagement.Repository;
using Contoso.TrainingManagement.Repository.BusinessEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;

using Microsoft.SharePoint;

using Contoso.TrainingManagement.Common.Constants;
using Contoso.TrainingManagement.Mocks;

namespace Contoso.TrainingManagement.Tests
{
    /// <summary>
    /// Summary description for TrainingCourseItemEventReceiver
    /// </summary>
    [TestClass]
    public class TrainingCourseItemEventReceiverFixture
    {
        #region Private Fields

        private ServiceLocator serviceLocator = ServiceLocator.GetInstance();

        #endregion

        [TestInitialize]
        public void TestInit()
        {
            MockTrainingCourseRepository.Clear();
            ServiceLocator.Clear();
        }

        #region Test Cleanup

        [TestCleanup]
        public void ClearMocks()
        {
            MockManager.ClearAll();
        }

        #endregion

        #region Test Methods

        #region ItemAdding

        /// <summary>
        /// Positive test for the Training Courses ItemAdding event handler
        /// </summary>
        [TestMethod]
        public void ItemAddingPositiveTest()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that our vaildations pass
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345678",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemAdding(spItemEventProperties);

            //Assert that the cancel did not get set
            Assert.IsFalse(spItemEventProperties.Cancel);
        }

        /// <summary>
        /// Adding course with invalid course code
        /// </summary>
        [TestMethod]
        public void AddingCourseWithInvalidCourseCodeCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that our course code is invalid
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "1234",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemAdding(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "The Course Code must be 8 characters long.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }

        /// <summary>
        /// Adding course with invalid enrollment date
        /// </summary>
        [TestMethod]
        public void AddingCourseWithInvalidEnrollmentDateCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that our enrollment date is invalid
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345678",
                                                                                               DateTime.Today.AddDays(-1),
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemAdding(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "The Enrollment Deadline Date can not be before today's date.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }

        /// <summary>
        /// Adding course with invalid start date
        /// </summary>
        [TestMethod]
        public void AddingCourseWithInvalidStartDateCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that our start date is invalid
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345678",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(-1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemAdding(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "The Start Date can not be before the Enrollment Deadline Date.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }

        /// <summary>
        /// Adding course with invalid end date
        /// </summary>
        [TestMethod]
        public void AddingCourseWithInvalidEndDateCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that our end date is invalid
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345678",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(-2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemAdding(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "The End Date can not be before the Start Date.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }


        /// <summary>
        /// Adding course with exisiting course code
        /// </summary>
        [TestMethod]
        public void AddingCourseWithExisitingCodeCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));
            MockTrainingCourseRepository.TrainingCourseReturnedByGet = new TrainingCourse();

            //Setup our mock so that the courses count will be 1
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345678",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemAdding(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "The Course Code is already in use.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }

        [TestMethod]
        public void AddingCourseWithNegativeCostCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that course count returns 1
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345679",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               -100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemAdding(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "Negative values are not allowed for Cost.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }

        #endregion

        #region ItemUpdating

        /// <summary>
        /// Positive Test for the Training Courses ItemUpdating event handler
        /// </summary>
        [TestMethod]
        public void ItemUpdatingPositiveTest()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that it pass all validations
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345678",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemUpdating(spItemEventProperties);

            Assert.IsFalse(spItemEventProperties.Cancel);
        }

        /// <summary>
        /// Updating course with invalid course code
        /// </summary>
        [TestMethod]
        public void UpdatingCourseWithInvalidCourseCodeCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that the course code is invalid
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "1234",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemUpdating(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "The Course Code must be 8 characters long.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }

        /// <summary>
        /// Updating course with invalid start date
        /// </summary>
        [TestMethod]
        public void UpdatingCourseWithInvalidStartDateCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that the start date is invalid
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345678",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(-1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemUpdating(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "The Start Date can not be before the Enrollment Deadline Date.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }

        /// <summary>
        /// Updating course with invalid end date
        /// </summary>
        [TestMethod]
        public void UpdatingCourseWithInvalidEndDateCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that the end date is invalid
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345678",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(-2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemUpdating(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "The End Date can not be before the Start Date.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }


        /// <summary>
        /// Updating course with existing course code
        /// </summary>
        [TestMethod]
        public void UpdatingCourseWithExisitingCodeCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));
            MockTrainingCourseRepository.TrainingCourseReturnedByGet = new TrainingCourse();

            //Setup our mock so that course count returns 1
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345679",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemUpdating(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "The Course Code is already in use.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }



        [TestMethod]
        public void UpdatingCourseWithNegativeCostCancelsWithError()
        {
            serviceLocator.Register<ITrainingCourseRepository>(typeof(MockTrainingCourseRepository));

            //Setup our mock so that course count returns 1
            SPItemEventProperties spItemEventProperties = this.CreateMockSpItemEventProperties("My Title",
                                                                                               "12345679",
                                                                                               DateTime.Today,
                                                                                               DateTime.Today.AddDays(1),
                                                                                               DateTime.Today.AddDays(2),
                                                                                               -100);

            //Call our event receiver with our mocked SPItemEventProperties
            TrainingCourseItemEventReceiver receiver = new TrainingCourseItemEventReceiver();
            receiver.ItemUpdating(spItemEventProperties);

            StringAssert.Contains(spItemEventProperties.ErrorMessage, "Negative values are not allowed for Cost.");
            Assert.IsTrue(spItemEventProperties.Cancel);
        }

        #endregion

        #endregion

        #region Mock Setup

        private SPItemEventProperties CreateMockSpItemEventProperties(string title, string code, DateTime enrollmentDate, DateTime startDate, DateTime endDate, float courseCost)
        {
            //Create any mock objects we'll need here
            SPItemEventProperties spItemEventProperties = RecorderManager.CreateMockedObject<SPItemEventProperties>();
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPItemEventDataCollection afterProperties = RecorderManager.CreateMockedObject<SPItemEventDataCollection>();

            //record our main expectations
            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                object obj = spItemEventProperties.AfterProperties;
                recorder.Return(afterProperties).RepeatAlways();

                afterProperties["Title"] = string.Empty;
                afterProperties["TrainingCourseCode"] = string.Empty;

                spItemEventProperties.OpenWeb();
                recorder.Return(web);

                obj = spItemEventProperties.ListItem[new Guid(Fields.TrainingCourseCode)];
                recorder.Return("12345678").RepeatAlways();
            }

            //Record our expectations for our AfterProperties collection
            MockHelper.RecordSPItemEventDataCollection(afterProperties, "Title", title);
            MockHelper.RecordSPItemEventDataCollection(afterProperties, "TrainingCourseCode", code);
            MockHelper.RecordSPItemEventDataCollection(afterProperties, "TrainingCourseEnrollmentDate", enrollmentDate);
            MockHelper.RecordSPItemEventDataCollection(afterProperties, "TrainingCourseStartDate", startDate);
            MockHelper.RecordSPItemEventDataCollection(afterProperties, "TrainingCourseEndDate", endDate);
            MockHelper.RecordSPItemEventDataCollection(afterProperties, "TrainingCourseCost", courseCost);

            return spItemEventProperties;
        }

        #endregion
    }
}