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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;
using TypeMock;

using Contoso.TrainingManagement;
using Contoso.TrainingManagement.Common.Constants;
using Contoso.TrainingManagement.Repository;

namespace Contoso.TrainingManagement.Repository.Tests
{
    /// <summary>
    /// Unit Tests for the ListManager class
    /// </summary>
    [TestClass]
    public class ListItemRepositoryFixture
    {
        #region Test Cleanup

        [TestCleanup]
        public void ClearMocks()
        {
            MockManager.ClearAll();
        }

        #endregion

        #region Test Methods

        /// <summary>
        /// Testing the add of a SPListItem
        /// </summary>
        [TestMethod]
        public void Add_ItemIsValid_ItemAdded()
        {
            SPWeb web = this.RecordWebForAdd();
            ListItemRepository listItemRepository = new ListItemRepository();
            Dictionary<Guid, object> fields = new Dictionary<Guid, object>();
            fields.Add(new Guid(Fields.CourseId), 1);
            fields.Add(new Guid(Fields.UserId), 1);
            SPListItem newItem = null;

            newItem = listItemRepository.Add(web, "Registrations", fields);

            Assert.IsNotNull(newItem);
            MockManager.Verify();
        }

        /// <summary>
        /// Testing the get of a SPListItem
        /// </summary>
        [TestMethod]
        public void Get_QueryIsValid_ItemFound()
        {
            SPWeb web = this.RecordWebForGet();
            ListItemRepository listItemRepository = new ListItemRepository();
            SPQuery query = new SPQuery();
            SPListItem item = null;

            item = listItemRepository.Get(web, "Registrations", query);

            Assert.IsNotNull(item);
            MockManager.Verify();
        }

        /// <summary>
        /// Testing the get of SPListItemCollection
        /// </summary>
        [TestMethod]
        public void Get_ValidListName_ItemsFound()
        {
            SPWeb web = this.RecordWebForGetAll();
            ListItemRepository listItemRepository = new ListItemRepository();
            SPQuery query = new SPQuery();
            SPListItemCollection items = null;

            items = listItemRepository.Get(web, "Registrations");

            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Count);
            MockManager.Verify();
        }

        /// <summary>
        /// Testing the update of a SPListItem
        /// </summary>
        [TestMethod]
        public void Update_IdAndItemIsValid_ItemUpdated()
        {
            SPWeb web = this.RecordWebForUpdate();
            ListItemRepository listItemRepository = new ListItemRepository();
            Dictionary<Guid, object> fields = new Dictionary<Guid, object>();
            fields.Add(new Guid(Fields.CourseId), 1);
            fields.Add(new Guid(Fields.UserId), 1);

            listItemRepository.Update(web, "Registrations", 1, fields);

            MockManager.Verify();
        }

        [TestMethod]
        public void Delete_IdIsValid_ItemDeleted()
        {
            SPWeb web = this.RecordWebForDelete();
            ListItemRepository listItemRepository = new ListItemRepository();
            SPQuery query = new SPQuery();

            listItemRepository.Delete(web, "Registrations", 1);

            MockManager.Verify();
        }

        #endregion

        #region Mock Setup

        private SPWeb RecordWebForAdd()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPList list = RecorderManager.CreateMockedObject<SPList>();
            SPListItem item = RecorderManager.CreateMockedObject<SPListItem>();

            using ( RecordExpectations recorder = RecorderManager.StartRecording() )
            {
                recorder.ExpectAndReturn(web.Lists["Registrations"], list).RepeatAlways();
                recorder.ExpectAndReturn(list.Items.Add(), item);
                item[new Guid(Fields.CourseId)] = 1;
                recorder.CheckArguments();
                item[new Guid(Fields.UserId)] = 1;
                recorder.CheckArguments();
                //recorder.ExpectAndReturn(web.AllowUnsafeUpdates, false);
                //web.AllowUnsafeUpdates = true;
                item.Update();
                //web.AllowUnsafeUpdates = false;
            }

            return web;
        }

        private SPWeb RecordWebForGet()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPList list = RecorderManager.CreateMockedObject<SPList>();
            SPListItemCollection itemCollection = RecorderManager.CreateMockedObject<SPListItemCollection>();
            SPListItem item = RecorderManager.CreateMockedObject<SPListItem>();

            using ( RecordExpectations recorder = RecorderManager.StartRecording() )
            {
                recorder.ExpectAndReturn(web.Lists["Registrations"], list).RepeatAlways();
                recorder.ExpectAndReturn(list.GetItems(new SPQuery()), itemCollection).RepeatAlways();
                recorder.ExpectAndReturn(itemCollection.Count, 1);
                recorder.ExpectAndReturn(itemCollection[0], item);
            }

            return web;
        }

        private SPWeb RecordWebForGetAll()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPList list = RecorderManager.CreateMockedObject<SPList>();
            SPListItemCollection itemCollection = RecorderManager.CreateMockedObject<SPListItemCollection>();

            using ( RecordExpectations recorder = RecorderManager.StartRecording() )
            {
                recorder.ExpectAndReturn(web.Lists["Registrations"], list).RepeatAlways();
                recorder.ExpectAndReturn(list.Items, itemCollection).RepeatAlways();
                recorder.ExpectAndReturn(itemCollection.Count, 2);
            }

            return web;
        }

        private SPWeb RecordWebForUpdate()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPList list = RecorderManager.CreateMockedObject<SPList>();
            SPListItemCollection itemCollection = RecorderManager.CreateMockedObject<SPListItemCollection>();
            SPListItem item = RecorderManager.CreateMockedObject<SPListItem>();

            using ( RecordExpectations recorder = RecorderManager.StartRecording() )
            {
                recorder.ExpectAndReturn(web.Lists["Registrations"], list).RepeatAlways();
                recorder.ExpectAndReturn(list.GetItems(new SPQuery()), itemCollection).RepeatAlways();
                recorder.ExpectAndReturn(itemCollection.Count, 1);
                recorder.ExpectAndReturn(itemCollection[0], item);
                item[new Guid(Fields.CourseId)] = 1;
                recorder.CheckArguments();
                item[new Guid(Fields.UserId)] = 1;
                recorder.CheckArguments();
                //recorder.ExpectAndReturn(web.AllowUnsafeUpdates, false);
                //web.AllowUnsafeUpdates = true;
                item.Update();
                //web.AllowUnsafeUpdates = false;
            }

            return web;
        }

        private SPWeb RecordWebForDelete()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();
            SPList list = RecorderManager.CreateMockedObject<SPList>();
            SPListItemCollection itemCollection = RecorderManager.CreateMockedObject<SPListItemCollection>();
            SPListItem item = RecorderManager.CreateMockedObject<SPListItem>();

            using ( RecordExpectations recorder = RecorderManager.StartRecording() )
            {
                recorder.ExpectAndReturn(web.Lists["Registrations"], list).RepeatAlways();
                recorder.ExpectAndReturn(list.GetItems(new SPQuery()), itemCollection).RepeatAlways();
                recorder.ExpectAndReturn(itemCollection.Count, 1);
                recorder.ExpectAndReturn(itemCollection[0], item);
                //recorder.ExpectAndReturn(web.AllowUnsafeUpdates, false);
                //web.AllowUnsafeUpdates = true;
                item.Delete();
                //web.AllowUnsafeUpdates = false;
            }

            return web;
        }

        #endregion
    }
}