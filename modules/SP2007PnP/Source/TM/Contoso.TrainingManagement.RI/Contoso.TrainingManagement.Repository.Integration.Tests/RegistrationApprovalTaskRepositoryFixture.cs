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

using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Contoso.TrainingManagement.Repository.BusinessEntities;
using Contoso.TrainingManagement.Common.Constants;

namespace Contoso.TrainingManagement.Repository.IntegrationTests
{
    /// <summary>
    /// Summary description for RegistrationApprovalTaskRepositoryFixture_2
    /// </summary>
    [TestClass]
    public class RegistrationApprovalTaskRepositoryFixture_2
    {
        #region Private Fields

        private Guid webId;
        private readonly string siteUrl = ConfigurationManager.AppSettings["SiteUrl"];

        #endregion

        #region Test Setup

        [TestInitialize]
        public void SetupSite()
        {
            using (SPSite site = new SPSite(siteUrl))
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
                using(SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    RegistrationApprovalTask task = new RegistrationApprovalTask();
                    task.Title = "TestTitle";
                    task.WorkflowItemId = 5;

                    #endregion

                    #region Act

                    RegistrationApprovalTaskRepository target = new RegistrationApprovalTaskRepository();
                    int id = target.Add(task, web);

                    #endregion

                    #region Assert

                    Assert.IsTrue(id > 0);

                    #endregion

                    #region Cleanup

                    target.Delete(id, web);

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

                    RegistrationApprovalTask taskAdded = new RegistrationApprovalTask();
                    taskAdded.Title = "TestTitle";
                    taskAdded.WorkflowItemId = 5;
                    RegistrationApprovalTaskRepository target = new RegistrationApprovalTaskRepository();
                    int taskAddedId = target.Add(taskAdded, web);

                    #endregion

                    #region Act

                    RegistrationApprovalTask taskFound = target.Get(taskAddedId, web);
                    
                    #endregion

                    #region Assert

                    Assert.IsNotNull(taskFound);
                    Assert.AreEqual(taskAddedId, taskFound.Id);
                    Assert.AreEqual(taskAdded.Title, taskFound.Title);
                    Assert.AreEqual(taskAdded.WorkflowItemId, taskFound.WorkflowItemId);

                    #endregion

                    #region Cleanup

                    target.Delete(taskAddedId, web);

                    #endregion
                }
            }
        }

        [TestMethod]
        public void Update_IdAndItemIsValid_ItemUpdated()
        {
            using(SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    RegistrationApprovalTask taskAdded = new RegistrationApprovalTask();
                    taskAdded.Title = "TestTitle";
                    taskAdded.WorkflowItemId = 5;
                    RegistrationApprovalTaskRepository target = new RegistrationApprovalTaskRepository();
                    int taskAddedId = target.Add(taskAdded, web);

                    #endregion

                    #region Act

                    taskAdded.Id = taskAddedId;
                    taskAdded.Title = "NewTitle";
                    taskAdded.WorkflowItemId = 6;
                    target.Update(taskAdded, web);

                    #endregion

                    #region Assert

                    RegistrationApprovalTask taskFound = target.Get(taskAddedId, web);
                    Assert.AreEqual("NewTitle", taskFound.Title);
                    Assert.AreEqual(6, taskFound.WorkflowItemId);

                    #endregion

                    #region Cleanup

                    target.Delete(taskAddedId, web);

                    #endregion
                }
            }
        }

        [TestMethod]
        public  void Delete_IdIsValid_ItemDeleted()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    RegistrationApprovalTask taskAdded = new RegistrationApprovalTask();
                    taskAdded.Title = "TestTitle";
                    taskAdded.WorkflowItemId = 5;
                    RegistrationApprovalTaskRepository target = new RegistrationApprovalTaskRepository();
                    int taskAddedId = target.Add(taskAdded, web);

                    int itemCount = web.Lists[Lists.RegistrationApprovalTasks].ItemCount;

                    #endregion

                    #region Act

                    target.Delete(taskAddedId, web);

                    #endregion

                    #region Assert

                    Assert.AreEqual(itemCount - 1, web.Lists[Lists.RegistrationApprovalTasks].ItemCount);

                    #endregion
                }
            }
        }
    }
}