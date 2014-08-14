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
using System.Configuration;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Contoso.TrainingManagement.Common.Constants;


namespace Contoso.TrainingManagement.Repository.IntegrationTests
{
    /// <summary>
    /// Summary description for ListItemRepositoryFixture_2
    /// </summary>
    [TestClass]
    public class ListItemRepositoryFixture_2
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
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    ListItemRepository listItemRepository = new ListItemRepository();
                    Dictionary<Guid, object> fields = new Dictionary<Guid, object>();
                    fields.Add(new Guid(Fields.CourseId), 1);
                    fields.Add(new Guid(Fields.UserId), 1);
                    SPListItem newItem;

                    #endregion

                    #region Act

                    newItem = listItemRepository.Add(web, Lists.Registrations, fields);

                    #endregion

                    #region Assert

                    Assert.IsNotNull(newItem);
                    Assert.AreEqual(1, newItem[new Guid(Fields.CourseId)]);
                    Assert.AreEqual(1, newItem[new Guid(Fields.UserId)]);

                    #endregion

                    #region Cleanup

                    int id = (int)newItem[new Guid(Fields.Id)];
                    listItemRepository.Delete(web, Lists.Registrations, id);

                    #endregion
                }
            }
        }

        [TestMethod]
        public void Get_QueryIsValid_ItemFound()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    ListItemRepository listItemRepository = new ListItemRepository();
                    Dictionary<Guid, object> fields = new Dictionary<Guid, object>();
                    fields.Add(new Guid(Fields.CourseId), 1);
                    fields.Add(new Guid(Fields.UserId), 1);
                    SPListItem targetItem = listItemRepository.Add(web, Lists.Registrations, fields);

                    #endregion

                    #region Act

                    StringBuilder queryBuilder = new StringBuilder("<Where>");
                    queryBuilder.Append(string.Format("<Eq><FieldRef Name='ID'/>"));
                    queryBuilder.Append(string.Format("<Value Type='Integer'>{0}</Value></Eq>", targetItem[new Guid(Fields.Id)]));
                    queryBuilder.Append("</Where>");
                    SPQuery query = new SPQuery();
                    query.Query = queryBuilder.ToString();
                    SPListItem foundItem = listItemRepository.Get(web, Lists.Registrations, query);

                    #endregion

                    #region Assert

                    Assert.IsNotNull(foundItem);
                    Assert.AreEqual(targetItem[new Guid(Fields.UserId)], foundItem[new Guid(Fields.UserId)]);
                    Assert.AreEqual(targetItem[new Guid(Fields.CourseId)], foundItem[new Guid(Fields.CourseId)]);

                    #endregion

                    #region Cleanup

                    int id = (int)targetItem[new Guid(Fields.Id)];
                    listItemRepository.Delete(web, Lists.Registrations, id);

                    #endregion
                }
            }
        }

        [TestMethod]
        public void Get_ValidListName_ItemFound()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange
                    
                    ListItemRepository listItemRepository = new ListItemRepository();
                    Dictionary<Guid, object> fields = new Dictionary<Guid, object>();
                    fields.Add(new Guid(Fields.CourseId), 999);
                    fields.Add(new Guid(Fields.UserId), 999);
                    SPListItem targetItem1 = listItemRepository.Add(web, Lists.Registrations, fields);

                    fields.Clear();
                    fields.Add(new Guid(Fields.CourseId), 998);
                    fields.Add(new Guid(Fields.UserId), 998);
                    SPListItem targetItem2 = listItemRepository.Add(web, Lists.Registrations, fields);

                    List<SPListItem> targetItems = new List<SPListItem>() {targetItem1, targetItem2};

                    #endregion

                    #region Act

                    SPListItemCollection foundItems = listItemRepository.Get(web, Lists.Registrations);

                    #endregion

                    #region Assert

                    Assert.IsNotNull(foundItems);
                    Assert.AreEqual(2, foundItems.Count);
                    for (int i = 0; i < foundItems.Count; i++)
                    {

                        Assert.AreEqual(targetItems[i][new Guid(Fields.Id)], foundItems[i][new Guid(Fields.Id)]);
                        Assert.AreEqual(targetItems[i][new Guid(Fields.CourseId)], foundItems[i][new Guid(Fields.CourseId)]);
                        Assert.AreEqual(targetItems[i][new Guid(Fields.UserId)], foundItems[i][new Guid(Fields.UserId)]);
                    }   
                
                    #endregion

                    #region Cleanup

                    foreach (SPListItem item in targetItems)
                    {
                        listItemRepository.Delete(web, Lists.Registrations, (int)item[new Guid(Fields.Id)]);
                    }

                    #endregion
                }
            }
        }

        [TestMethod]
        public void Update_IdAndItemIsValid_ItemUpdated()
        {
            using (SPSite site = new SPSite(siteUrl))
            {
                using(SPWeb web = site.AllWebs[webId])
                {
                    #region Arrange

                    ListItemRepository listItemRepository = new ListItemRepository();
                    Dictionary<Guid, object> fields = new Dictionary<Guid, object>();
                    fields.Add(new Guid(Fields.CourseId), 1);
                    fields.Add(new Guid(Fields.UserId), 1);
                    SPListItem targetItem = listItemRepository.Add(web, Lists.Registrations, fields);

                    #endregion

                    #region Act
                    
                    fields.Clear();
                    fields.Add(new Guid(Fields.CourseId), 999);
                    fields.Add(new Guid(Fields.UserId), 999);
                    listItemRepository.Update(web, Lists.Registrations,(int)targetItem[new Guid(Fields.Id)], fields);

                    #endregion

                    #region Assert

                    StringBuilder queryBuilder = new StringBuilder("<Where>");
                    queryBuilder.Append(string.Format("<Eq><FieldRef Name='ID'/>"));
                    queryBuilder.Append(string.Format("<Value Type='Integer'>{0}</Value></Eq>", targetItem[new Guid(Fields.Id)]));
                    queryBuilder.Append("</Where>");
                    SPQuery query = new SPQuery();
                    query.Query = queryBuilder.ToString();
                    SPListItem foundItem = listItemRepository.Get(web, Lists.Registrations, query);

                    Assert.AreEqual(999, foundItem[new Guid(Fields.CourseId)]);
                    Assert.AreEqual(999, foundItem[new Guid(Fields.UserId)]);

                    #endregion

                    #region Cleanup

                    listItemRepository.Delete(web, Lists.Registrations, (int)targetItem[new Guid(Fields.Id)]);

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

                    ListItemRepository listItemRepository = new ListItemRepository();
                    Dictionary<Guid, object> fields = new Dictionary<Guid, object>();
                    fields.Add(new Guid(Fields.CourseId), 1);
                    fields.Add(new Guid(Fields.UserId), 1);
                    SPListItem targetItem = listItemRepository.Add(web, Lists.Registrations, fields);

                    #endregion

                    #region Act

                    listItemRepository.Delete(web, Lists.Registrations, (int)targetItem[new Guid(Fields.Id)]);

                    #endregion

                    #region Assert

                    SPListItemCollection items = listItemRepository.Get(web, Lists.Registrations);
                    Assert.AreEqual(0, items.Count);

                    #endregion
                }
            }
        }
    }
}