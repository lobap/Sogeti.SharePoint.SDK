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
using System.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;

using Contoso.TrainingManagement.Repository;
using Contoso.TrainingManagement.Repository.BusinessEntities;
using Contoso.TrainingManagement.Mocks;

namespace Contoso.TrainingManagement.IntegrationTests
{
    /// <summary>
    /// Summary description for TrainingCourseItemEventReceiver
    /// </summary>
    [TestClass]
    public class TrainingCourseItemEventReceiverFixture
    {
        #region Private Fields

        private ServiceLocator serviceLocator = ServiceLocator.GetInstance();
        private Guid webId;
        private readonly string siteUrl = ConfigurationManager.AppSettings["SiteUrl"];

        #endregion

        #region Test Initialize

        [TestInitialize]
        public void TestInit()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs.Add(Guid.NewGuid().ToString(), "", "", 1033, "CONTOSOTRAINING#0", false, false))
                {
                    webId = web.ID;
                }
            }

            MockTrainingCourseRepository.Clear();
        }

        #endregion

        #region Test Cleanup

        [TestCleanup]
        public void TestCleanup()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                site.AllWebs[webId].Delete();
            }
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
            TrainingCourse course = new TrainingCourse
                                        {
                                            Title = "My Title",
                                            Code = "12345678",
                                            EnrollmentDate = DateTime.Today,
                                            StartDate = DateTime.Today.AddDays(1),
                                            EndDate = DateTime.Today.AddDays(2),
                                            Cost = 100
                                        };

            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    TrainingCourseRepository repository = new TrainingCourseRepository();
                    int id = repository.Add(course, web);

                    //Cleanup
                    repository.Delete(id, web);
                }
            }
        }

        /// <summary>
        /// Adding course with invalid course code
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SPException), "The Course Code must be 8 characters long.\r\n")]
        public void AddingCourseWithInvalidCourseCodeCancelsWithError()
        {
            TrainingCourse course = new TrainingCourse
                                        {
                                            Title = "My Title",
                                            Code = "1234567",
                                            EnrollmentDate = DateTime.Today,
                                            StartDate = DateTime.Today.AddDays(1),
                                            EndDate = DateTime.Today.AddDays(2),
                                            Cost = 100
                                        };

            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    TrainingCourseRepository repository = new TrainingCourseRepository();
                    repository.Add(course, web);
                }
            }
        }

        /// <summary>
        /// Adding course with invalid enrollment date
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SPException), "The Enrollment Deadline Date can not be before today's date.\r\n")]
        public void AddingCourseWithInvalidEnrollmentDateCancelsWithError()
        {
            //Setup our mock so that our enrollment date is invalid
            TrainingCourse course = new TrainingCourse
                                        {
                                            Title = "My Title",
                                            Code = "12345678",
                                            EnrollmentDate = DateTime.Today.AddDays(-1),
                                            StartDate = DateTime.Today.AddDays(1),
                                            EndDate = DateTime.Today.AddDays(2),
                                            Cost = 100
                                        };


            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    TrainingCourseRepository repository = new TrainingCourseRepository();
                    repository.Add(course, web);
                }
            }
        }

        /// <summary>
        /// Adding course with invalid start date
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SPException), "The Start Date can not be before the Enrollment Deadline Date.\r\n")]
        public void AddingCourseWithInvalidStartDateCancelsWithError()
        {
            TrainingCourse course = new TrainingCourse
                                        {
                                            Title = "My Title",
                                            Code = "12345678",
                                            EnrollmentDate = DateTime.Today,
                                            StartDate = DateTime.Today.AddDays(-1),
                                            EndDate = DateTime.Today.AddDays(2),
                                            Cost = 100
                                        };

            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    TrainingCourseRepository repository = new TrainingCourseRepository();
                    repository.Add(course, web);
                }
            }
        }

        /// <summary>
        /// Adding course with invalid end date
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SPException), "The End Date can not be before the Start Date.\r\n")]
        public void AddingCourseWithInvalidEndDateCancelsWithError()
        {
            //Setup our mock so that our end date is invalid
            TrainingCourse course = new TrainingCourse
                                        {
                                            Title = "My Title",
                                            Code = "12345678",
                                            EnrollmentDate = DateTime.Today,
                                            StartDate = DateTime.Today.AddDays(1),
                                            EndDate = DateTime.Today.AddDays(-2),
                                            Cost = 100
                                        };

            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    TrainingCourseRepository repository = new TrainingCourseRepository();
                    repository.Add(course, web);
                }
            }
        }


        /// <summary>
        /// Adding course with exisiting course code
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SPException), "The Course Code is already in use.\r\n")]
        public void AddingCourseWithExisitingCodeCancelsWithError()
        {
            TrainingCourse course = new TrainingCourse
                                        {
                                            Title = "My Title",
                                            Code = "12345678",
                                            EnrollmentDate = DateTime.Today,
                                            StartDate = DateTime.Today.AddDays(1),
                                            EndDate = DateTime.Today.AddDays(2),
                                            Cost = 100
                                        };

            //Setup our mock so that the courses count will be 1
            TrainingCourse course2 = new TrainingCourse
                                         {
                                             Title = "My Title",
                                             Code = "12345678",
                                             EnrollmentDate = DateTime.Today,
                                             StartDate = DateTime.Today.AddDays(1),
                                             EndDate = DateTime.Today.AddDays(2),
                                             Cost = 100
                                         };

            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    TrainingCourseRepository repository = new TrainingCourseRepository();
                    int id = 0;

                    try
                    {
                        id = repository.Add(course, web);
                        repository.Add(course2, web);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        repository.Delete(id, web);
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(SPException), "Negative values are not allowed for Cost.\r\n")]
        public void AddingCourseWithNegativeCostCancelsWithError()
        {
            TrainingCourse course = new TrainingCourse
                                        {
                                            Title = "My Title",
                                            Code = "12345679",
                                            EnrollmentDate = DateTime.Today,
                                            StartDate = DateTime.Today.AddDays(1),
                                            EndDate = DateTime.Today.AddDays(2),
                                            Cost = -100
                                        };

            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    TrainingCourseRepository repository = new TrainingCourseRepository();
                    repository.Add(course, web);
                }
            }
        }

        #endregion

        #region ItemUpdating

        /// <summary>
        /// Positive Test for the Training Courses ItemUpdating event handler
        /// </summary>
        [TestMethod]
        public void ItemUpdatingPositiveTest()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange
                    TrainingCourseRepository repository = new TrainingCourseRepository();

                    TrainingCourse course = new TrainingCourse
                                                {
                                                    Title = "My Title",
                                                    Code = "12345678",
                                                    EnrollmentDate = DateTime.Today,
                                                    StartDate = DateTime.Today.AddDays(1),
                                                    EndDate = DateTime.Today.AddDays(2),
                                                    Cost = 100
                                                };

                    int id = repository.Add(course, web);

                    #endregion

                    course.Title = "New Title";
                    repository.Update(course, web);

                    #region Cleanup

                    repository.Delete(id, web);

                    #endregion
                }
            }
        }

        /// <summary>
        /// Updating course with invalid course code
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SPException), "The Course Code must be 8 characters long.\r\n")]
        public void UpdatingCourseWithInvalidCourseCodeCancelsWithError()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    TrainingCourseRepository repository = new TrainingCourseRepository();

                    TrainingCourse course = new TrainingCourse
                                                {
                                                    Title = "My Title",
                                                    Code = "12345678",
                                                    EnrollmentDate = DateTime.Today,
                                                    StartDate = DateTime.Today.AddDays(1),
                                                    EndDate = DateTime.Today.AddDays(2),
                                                    Cost = 100
                                                };

                    course.Id = repository.Add(course, web);

                    #endregion

                    try
                    {
                        course.Code = "1234567";
                        repository.Update(course, web);
                    }
                    catch (SPException ex)
                    {
                        throw;
                    }
                    finally
                    {
                        #region Cleanup

                        repository.Delete(course.Id, web);

                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// Updating course with invalid start date
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SPException), "The Start Date can not be before the Enrollment Deadline Date.\r\n")]
        public void UpdatingCourseWithInvalidStartDateCancelsWithError()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    TrainingCourseRepository repository = new TrainingCourseRepository();

                    TrainingCourse course = new TrainingCourse
                                                {
                                                    Title = "My Title",
                                                    Code = "12345678",
                                                    EnrollmentDate = DateTime.Today,
                                                    StartDate = DateTime.Today.AddDays(1),
                                                    EndDate = DateTime.Today.AddDays(2),
                                                    Cost = 100
                                                };

                    course.Id = repository.Add(course, web);

                    #endregion

                    try
                    {
                        course.StartDate = course.EnrollmentDate.AddDays(-1);
                        repository.Update(course, web);
                    }
                    catch (SPException ex)
                    {
                        throw;
                    }
                    finally
                    {
                        #region Cleanup

                        repository.Delete(course.Id, web);

                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// Updating course with invalid end date
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SPException), "The End Date can not be before the Start Date.\r\n")]
        public void UpdatingCourseWithInvalidEndDateCancelsWithError()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    TrainingCourseRepository repository = new TrainingCourseRepository();

                    TrainingCourse course = new TrainingCourse
                                                {
                                                    Title = "My Title",
                                                    Code = "12345678",
                                                    EnrollmentDate = DateTime.Today,
                                                    StartDate = DateTime.Today.AddDays(1),
                                                    EndDate = DateTime.Today.AddDays(2),
                                                    Cost = 100
                                                };

                    course.Id = repository.Add(course, web);

                    #endregion

                    try
                    {
                        course.EndDate = course.StartDate.AddDays(-1);
                        repository.Update(course, web);
                    }
                    catch (SPException ex)
                    {
                        throw;
                    }
                    finally
                    {
                        #region Cleanup

                        repository.Delete(course.Id, web);

                        #endregion
                    }
                }
            }
        }


        /// <summary>
        /// Updating course with existing course code
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SPException), "The Course Code is already in use.\r\n")]
        public void UpdatingCourseWithExisitingCodeCancelsWithError()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    TrainingCourseRepository repository = new TrainingCourseRepository();

                    TrainingCourse course = new TrainingCourse
                                                {
                                                    Title = "My Title",
                                                    Code = "12345678",
                                                    EnrollmentDate = DateTime.Today,
                                                    StartDate = DateTime.Today.AddDays(1),
                                                    EndDate = DateTime.Today.AddDays(2),
                                                    Cost = 100
                                                };

                    course.Id = repository.Add(course, web);

                    TrainingCourse course2 = new TrainingCourse
                                                 {
                                                     Title = "My Title",
                                                     Code = "98765432",
                                                     EnrollmentDate = DateTime.Today,
                                                     StartDate = DateTime.Today.AddDays(1),
                                                     EndDate = DateTime.Today.AddDays(2),
                                                     Cost = 100
                                                 };

                    course2.Id = repository.Add(course2, web);

                    #endregion

                    try
                    {
                        course.Code = "98765432";
                        repository.Update(course, web);
                    }
                    catch (SPException ex)
                    {
                        throw;
                    }
                    finally
                    {
                        #region Cleanup

                        repository.Delete(course.Id, web);
                        repository.Delete(course2.Id, web);

                        #endregion
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(SPException), "Negative values are not allowed for Cost.\r\n")]
        public void UpdatingCourseWithNegativeCostCancelsWithError()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    TrainingCourseRepository repository = new TrainingCourseRepository();

                    TrainingCourse course = new TrainingCourse
                                                {
                                                    Title = "My Title",
                                                    Code = "12345678",
                                                    EnrollmentDate = DateTime.Today,
                                                    StartDate = DateTime.Today.AddDays(1),
                                                    EndDate = DateTime.Today.AddDays(2),
                                                    Cost = 100
                                                };

                    course.Id = repository.Add(course, web);

                    #endregion

                    try
                    {
                        course.Cost = -100f;
                        repository.Update(course, web);
                    }
                    catch (SPException ex)
                    {
                        throw;
                    }
                    finally
                    {
                        #region Cleanup

                        repository.Delete(course.Id, web);

                        #endregion
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}