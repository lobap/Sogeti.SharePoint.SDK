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
using System.Linq;
using Microsoft.Practices.SPG.Common.ListRepository;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;
using System;

namespace Microsoft.Practices.SPG.Common.Tests.ListRepository
{
    [TestClass]
    public class ListItemFieldMapperFixture
    {
        [TestMethod]
        public void CanCreateEntityFromSPListItem()
        {
            Guid fieldId1 = Guid.NewGuid();
            Guid fieldId2 = Guid.NewGuid();

            SPListItem spListItem = Isolate.Fake.Instance<SPListItem>();
            Isolate.WhenCalled(() => spListItem[fieldId1]).WithExactArguments().WillReturn("teststring");
            Isolate.WhenCalled(() => spListItem[fieldId2]).WithExactArguments().WillReturn(123);

            
            ListItemFieldMapper<TestEntity> target = new ListItemFieldMapper<TestEntity>();
            target.AddMapping(fieldId1, "Property1String");
            target.AddMapping(fieldId2, "Property2Integer");


            TestEntity testEntity = target.CreateEntity(spListItem);

            Assert.AreEqual("teststring", testEntity.Property1String);
            Assert.AreEqual(123, testEntity.Property2Integer);
        }

        [TestMethod]
        public void CanAddMappingDirectly()
        {
            ListItemFieldMapper<TestEntity> target = new ListItemFieldMapper<TestEntity>();
            Guid fieldId = Guid.NewGuid();
            target.AddMapping(fieldId, "EntityPropertyName");

            Assert.AreEqual(1, target.FieldMappings.Count((mapping) => mapping.ListFieldId.Equals(fieldId) && mapping.EntityPropertyName == "EntityPropertyName"));
        }


        [TestMethod]
        public void CanFillSPListItemFromEntity()
        {
            Guid fieldId1 = Guid.NewGuid();
            Guid fieldId2 = Guid.NewGuid();
            TestEntity testEntity = new TestEntity() {Property1String = "teststring", Property2Integer = 321};
            SPListItem spListItem = Isolate.Fake.Instance<SPListItem>();
            object field1Value = null;
            object field2Value = null;
            Isolate.WhenCalled(() => spListItem[fieldId1] = "teststring").DoInstead((context) => field1Value = context.Parameters[1]);
            Isolate.WhenCalled(() => spListItem[fieldId2] = 321).DoInstead((context) => field2Value = context.Parameters[1]);
            

            ListItemFieldMapper<TestEntity> target = new ListItemFieldMapper<TestEntity>();
            target.AddMapping(fieldId1, "Property1String");
            target.AddMapping(fieldId2, "Property2Integer");

            target.FillSPListItemFromEntity(spListItem, testEntity);

            Assert.AreEqual("teststring", field1Value);
            Assert.AreEqual(321, field2Value);

        }

        [TestMethod]
        public void WhenCreatingEntityMissingPropertyFailsWithClearException()
        {
            Guid fieldId1 = new Guid("{95C7711D-E2B5-42f9-B0DB-4C840BED0C74}");
            SPListItem spListItem = Isolate.Fake.Instance<SPListItem>();
            Isolate.WhenCalled(() => spListItem[fieldId1]).WithExactArguments().WillReturn("teststring");
            Isolate.WhenCalled(() => spListItem.Name).WillReturn("ItemName");

            var target = new ListItemFieldMapper<TestEntity>();
            target.AddMapping(fieldId1, "NonExistingProperty");

            try
            {
                target.CreateEntity(spListItem);
                Assert.Fail();
            }
            catch (ListItemFieldMappingException ex)
            {
                Assert.AreEqual(
                    "Type 'Microsoft.Practices.SPG.Common.Tests.ListRepository.TestEntity' does not have a property 'NonExistingProperty' which was mapped to FieldID: '95c7711d-e2b5-42f9-b0db-4c840bed0c74' for SPListItem 'ItemName'."
                    , ex.Message);
            }
            catch(Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void WhenFillingItemMissingPropertyFailsWithClearException()
        {
            Guid fieldId1 = new Guid("{95C7711D-E2B5-42f9-B0DB-4C840BED0C74}");
            SPListItem spListItem = Isolate.Fake.Instance<SPListItem>();
            Isolate.WhenCalled(() => spListItem.Name).WillReturn("ItemName");

            var target = new ListItemFieldMapper<TestEntity>();
            target.AddMapping(fieldId1, "NonExistingProperty");

            try
            {
                target.FillSPListItemFromEntity(spListItem, new TestEntity());
                Assert.Fail();
            }
            catch (ListItemFieldMappingException ex)
            {
                Assert.AreEqual(
                    "Type 'Microsoft.Practices.SPG.Common.Tests.ListRepository.TestEntity' does not have a property 'NonExistingProperty' which was mapped to FieldID: '95c7711d-e2b5-42f9-b0db-4c840bed0c74' for SPListItem 'ItemName'."
                    , ex.Message);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void WhenCreatingEntityMissingFieldFailsWithClearException()
        {
            Guid fieldId1 = new Guid("{95C7711D-E2B5-42f9-B0DB-4C840BED0C74}");
            SPListItem spListItem = Isolate.Fake.Instance<SPListItem>();
            Isolate.WhenCalled(() => spListItem.Fields[fieldId1]).WithExactArguments().WillThrow(new ArgumentException());
            Isolate.WhenCalled(() => spListItem.Name).WillReturn("ItemName");

            var target = new ListItemFieldMapper<TestEntity>();
            target.AddMapping(fieldId1, "Property1String");

            try
            {
                target.CreateEntity(spListItem);
                Assert.Fail();
            }
            catch (ListItemFieldMappingException ex)
            {
                Assert.AreEqual(
                    "SPListItem 'ItemName' does not have a field with Id '95c7711d-e2b5-42f9-b0db-4c840bed0c74'"
                    + " which was mapped to property: 'Property1String' for entity 'Microsoft.Practices.SPG.Common.Tests.ListRepository.TestEntity'."
                    , ex.Message);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

    }

    class TestEntity
    {
        public string Property1String { get; set; }
        public int Property2Integer { get; set; }
    }
}