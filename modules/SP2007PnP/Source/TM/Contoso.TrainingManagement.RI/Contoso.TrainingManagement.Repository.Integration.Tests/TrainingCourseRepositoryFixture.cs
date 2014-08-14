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

using Contoso.TrainingManagement.Common.Constants;
using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Repository.IntegrationTests
{
    /// <summary>
    /// Summary description for TrainingCourseRepositoryFixture_2
    /// </summary>
    [TestClass]
    public class TrainingCourseRepositoryFixture_2
    {
        #region Private Fields

        private Guid webId;
        private readonly string siteUrl = ConfigurationManager.AppSettings["SiteUrl"];

        #endregion

        #region Test Setup

        [TestInitialize]
        public void SetupSite()
        {
            using (SPSite site = new SPSite((siteUrl)))
            {
                using (SPWeb web = site.AllWebs.Add(Guid.NewGuid().ToString(), "", "", 1033, "CONTOSOTRAINING#0", false, false))
                {
                    webId = web.ID;
                }
            }
        }

        #endregion

        #region Test Cleanup

        [TestCleanup]
        public void CleanupSite()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                site.AllWebs[webId].Delete();
            }
        }

        #endregion

        [TestMethod]
        public void Add_ItemIsValid_ItemAdded()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    TrainingCourse targetItem = new TrainingCourse();
                    targetItem.Title = "TestTitle";
                    targetItem.Code = "TestCode";
                    targetItem.Description = "TestDescription";
                    targetItem.EnrollmentDate = DateTime.Today;
                    targetItem.StartDate = DateTime.Today.AddDays(1);
                    targetItem.EndDate = DateTime.Today.AddDays(2);
                    targetItem.Cost = 123f;
                    
                    #endregion

                    #region Act

                    TrainingCourseRepository target = new TrainingCourseRepository();
                    int targetItemId = target.Add(targetItem, web);

                    #endregion

                    #region Assert

                    Assert.IsTrue(targetItemId > 0);

                    #endregion

                    #region Cleanup

                    target.Delete(targetItemId, web);

                    #endregion
                }
            }
        }

        [TestMethod]
        public void Get_IdIsValid_ItemFound()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    TrainingCourse targetItem = new TrainingCourse();
                    targetItem.Title = "TestTitle";
                    targetItem.Code = "TestCode";
                    targetItem.Description = "TestDescription";
                    targetItem.EnrollmentDate = DateTime.Today;
                    targetItem.StartDate = DateTime.Today.AddDays(1);
                    targetItem.EndDate = DateTime.Today.AddDays(2);
                    targetItem.Cost = 123f;
                    TrainingCourseRepository target = new TrainingCourseRepository();
                    int targetItemId = target.Add(targetItem, web);
                    
                    #endregion

                    #region Act

                    TrainingCourse foundItem = target.Get(targetItemId, web);

                    #endregion

                    #region Assert

                    Assert.IsNotNull(foundItem);
                    Assert.AreEqual(targetItemId, foundItem.Id);
                    Assert.AreEqual(targetItem.Title, foundItem.Title);
                    Assert.AreEqual(targetItem.Code, foundItem.Code);
                    Assert.AreEqual(targetItem.Description, foundItem.Description);
                    Assert.AreEqual(targetItem.EnrollmentDate, foundItem.EnrollmentDate);
                    Assert.AreEqual(targetItem.StartDate, foundItem.StartDate);
                    Assert.AreEqual(targetItem.EndDate, foundItem.EndDate);
                    Assert.AreEqual(targetItem.Cost, foundItem.Cost);

                    #endregion

                    #region Cleanup

                    target.Delete(targetItemId, web);

                    #endregion
                }
            }
        }

        [TestMethod]
        public void Get_CourseCodeIsValid_ItemFound()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    TrainingCourse targetItem = new TrainingCourse();
                    targetItem.Title = "TestTitle";
                    targetItem.Code = "TestCode";
                    targetItem.Description = "TestDescription";
                    targetItem.EnrollmentDate = DateTime.Today;
                    targetItem.StartDate = DateTime.Today.AddDays(1);
                    targetItem.EndDate = DateTime.Today.AddDays(2);
                    targetItem.Cost = 123f;
                    TrainingCourseRepository target = new TrainingCourseRepository();
                    int targetItemId = target.Add(targetItem, web);
                    
                    #endregion

                    #region Act

                    TrainingCourse foundItem = target.Get("TestCode", web);

                    #endregion

                    #region Assert

                    Assert.IsNotNull(foundItem);
                    Assert.AreEqual(targetItemId, foundItem.Id);
                    Assert.AreEqual(targetItem.Title, foundItem.Title);
                    Assert.AreEqual(targetItem.Code, foundItem.Code);
                    Assert.AreEqual(targetItem.Description, foundItem.Description);
                    Assert.AreEqual(targetItem.EnrollmentDate, foundItem.EnrollmentDate);
                    Assert.AreEqual(targetItem.StartDate, foundItem.StartDate);
                    Assert.AreEqual(targetItem.EndDate, foundItem.EndDate);
                    Assert.AreEqual(targetItem.Cost, foundItem.Cost);

                    #endregion

                    #region Cleanup

                    target.Delete(targetItemId, web);

                    #endregion
                }
            }
        }

        [TestMethod]
        public void Update_IdAndItemIsValid_ItemUpdated()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    TrainingCourse targetItem = new TrainingCourse();
                    targetItem.Title = "TestTitle";
                    targetItem.Code = "TestCode";
                    targetItem.Description = "TestDescription";
                    targetItem.EnrollmentDate = DateTime.Today;
                    targetItem.StartDate = DateTime.Today.AddDays(1);
                    targetItem.EndDate = DateTime.Today.AddDays(2);
                    targetItem.Cost = 123f;
                    TrainingCourseRepository target = new TrainingCourseRepository();
                    targetItem.Id = target.Add(targetItem, web);

                    #endregion

                    #region Act

                    targetItem.Title = "NewTitle";
                    targetItem.Code = "_NewCode";
                    targetItem.Description = "TestDescription";
                    targetItem.EnrollmentDate = DateTime.Today.AddDays(1);
                    targetItem.StartDate = DateTime.Today.AddDays(2);
                    targetItem.EndDate = DateTime.Today.AddDays(3);
                    targetItem.Cost = 456f;
                    target.Update(targetItem, web);

                    #endregion

                    #region Assert

                    TrainingCourse trainingCourseFound = target.Get(targetItem.Id, web);
                    Assert.IsNotNull(trainingCourseFound);
                    Assert.AreEqual(targetItem.Id, trainingCourseFound.Id);
                    Assert.AreEqual(targetItem.Title, trainingCourseFound.Title);
                    Assert.AreEqual(targetItem.Code, trainingCourseFound.Code);
                    Assert.AreEqual(targetItem.Description, trainingCourseFound.Description);
                    Assert.AreEqual(targetItem.EnrollmentDate, trainingCourseFound.EnrollmentDate);
                    Assert.AreEqual(targetItem.StartDate, trainingCourseFound.StartDate);
                    Assert.AreEqual(targetItem.EndDate, trainingCourseFound.EndDate);
                    Assert.AreEqual(targetItem.Cost, trainingCourseFound.Cost);
                    
                    #endregion

                    #region Cleanup

                    target.Delete(targetItem.Id, web);

                    #endregion
                }
            }
        }

        [TestMethod]
        public void Delete_IdIsValid_ItemDeleted()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    TrainingCourse targetItem = new TrainingCourse();
                    targetItem.Title = "TestTitle";
                    targetItem.Code = "TestCode";
                    targetItem.Description = "TestDescription";
                    targetItem.EnrollmentDate = DateTime.Today;
                    targetItem.StartDate = DateTime.Today.AddDays(1);
                    targetItem.EndDate = DateTime.Today.AddDays(2);
                    targetItem.Cost = 123f;
                    TrainingCourseRepository target = new TrainingCourseRepository();
                    targetItem.Id = target.Add(targetItem, web);

                    int itemCount = web.Lists[Lists.TrainingCourses].ItemCount;

                    #endregion

                    #region Act

                    target.Delete(targetItem.Id, web);

                    #endregion

                    #region Assert

                    Assert.AreEqual(itemCount - 1, web.Lists[Lists.TrainingCourses].ItemCount);

                    #endregion
                }
            }
        }
    }
}