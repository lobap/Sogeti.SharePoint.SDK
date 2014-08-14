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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;

using TypeMock;

using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Repository.Tests
{
    /// <summary>
    /// Summary description for BaseSPListItemManager
    /// </summary>
    [TestClass]
    public class BaseEntityRepositoryFixture
    {
        [TestMethod]
        public void CanGetFieldName()
        {
            SPWeb web = this.RecordGetFieldName();

            MockBaseEntityRepository entityRepository = new MockBaseEntityRepository();
            string fieldName = entityRepository.GetFieldName(new Guid(), web);

            Assert.AreEqual("UnitTestField", fieldName);
        }

        private SPWeb RecordGetFieldName()
        {
            SPWeb web = RecorderManager.CreateMockedObject<SPWeb>();

            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                recorder.ExpectAndReturn(web.Lists[""].Fields[new Guid()].InternalName, "UnitTestField");
            }

            return web;
        }

        private class MockBaseEntityRepository : BaseEntityRepository<Registration>
        {
            protected override string ListName
            {
                get { return ""; }
            }

            protected override Dictionary<Guid, object> GatherParameters(Registration entity, SPWeb web)
            {
                throw new NotImplementedException();
            }

            protected override Registration PopulateEntity(SPListItem item)
            {
                throw new NotImplementedException();
            }
        }
    }
}