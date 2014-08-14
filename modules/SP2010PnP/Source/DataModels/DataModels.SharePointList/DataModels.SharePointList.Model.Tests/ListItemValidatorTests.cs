//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Collections.Generic;
using Microsoft.SharePoint.Moles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.Model.Tests
{
    [TestClass]
    public class ListItemValidatorTests
    {

        [TestMethod]
        [HostType("Moles")]
        public void MatchingFieldsExistInList_ReturnsFalse_NoMatchingFieldsInList()
        {
            //Arrange
            var validator = new ListItemValidator();
            MSPList spList = new MSPList();
            var spFields = new MSPFieldCollection();
            var lookupField = new MSPField();
            lookupField.TypeGet = () => SPFieldType.Lookup;
            spList.FieldsGet = () => spFields;
            spFields.GetFieldByInternalNameString = n => lookupField;
            var dataCollection = new MSPItemEventDataCollection();
            dataCollection.ItemGetString = (s) => "1";
            var returnedItems = new MSPListItemCollection();
            returnedItems.CountGet = () => 0;
            spList.GetItemsSPQuery = q => returnedItems;

            var fields = new List<string>();
            fields.Add("Field1");
            fields.Add("Field2");

            //Act
            bool result = validator.MatchingFieldsExistInList(spList, dataCollection, fields);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [HostType("Moles")]
        public void MatchingFieldsExistInList_ReturnsTrue_MatchingFieldsInList()
        {
            //Arrange
            var validator = new ListItemValidator();
            MSPList spList = new MSPList();
            var spFields = new MSPFieldCollection();
            var lookupField = new MSPField();
            lookupField.TypeGet = () => SPFieldType.Lookup;
            spList.FieldsGet = () => spFields;
            spFields.GetFieldByInternalNameString = n => lookupField;
            var dataCollection = new MSPItemEventDataCollection();
            dataCollection.ItemGetString = (s) => "1";
            var returnedItems = new MSPListItemCollection();
            returnedItems.CountGet = () => 1;
            spList.GetItemsSPQuery = q => returnedItems;

            var fields = new List<string>();
            fields.Add("Field1");
            fields.Add("Field2");
            //Act
            bool result = validator.MatchingFieldsExistInList(spList, dataCollection, fields);
            //Assert
            Assert.IsTrue(result);
        }




    }


}
