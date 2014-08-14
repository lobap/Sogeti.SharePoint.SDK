//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{
    /// <summary>
    /// Summary description for DiagnosticsAreaCollectionFixture
    /// </summary>
    [TestClass]
    public class DiagnosticsAreaCollectionTests
    {
        string TestArea1Name = "{51362770-8740-4A2C-B933-2D4D2453342B}";
        string TestArea2Name = "{AA4EF4B8-60A2-40DE-897B-24A614A81C08}";
        string TestCategory11Name = "{2929FC81-C386-4C66-B9B9-2948EBAF5324}";
        string TestCategory12Name = "{45EC03D1-AE34-44EA-A610-C1DF1CD17C34}";
        string TestCategory21Name = "{88DFA3E5-01CF-40A4-887B-F0E37851CA51}";
        string TestCategory22Name = "{19C16C6D-349C-4770-9435-8A54F357FF88}";


        [TestMethod]
        public void CTOR_WithNoConfigurationSet()
        {
            //Arrange
            var config = new MockConfigManager();
            config.Clear();

            //Act
            var target = new DiagnosticsAreaCollection(config);

            //Assert
            Assert.IsTrue(target.Count == 0);

        }

        [TestMethod]
        public void CTOR_WithConfiguration()
        {
            //Arrange
            var expected = new MockConfigManager();
  
            //Act
            var target = new DiagnosticsAreaCollection(expected);

            //Assert
            Assert.IsTrue(target.Count == expected.Areas.Count);

            for (int i = 0; i < target.Count; i++)
                Assert.AreEqual<DiagnosticsArea>(target[i], expected.Areas[i]);
        }

        [TestMethod]
        public void CTOR_WithConfigurationMgr_WithoutLogginConfigurationSucceeds()
        {
            //Arrange
            var configMgr = new SIConfigManager();
            var propBag = new BIPropertyBag();
            configMgr.ContainsKeyInPropertyBagStringIPropertyBag = (key, bag) => false;
            configMgr.GetPropertyBagConfigLevel = (configLevel) => propBag;

            //Act
            var target = new DiagnosticsAreaCollection(configMgr);

            //Assert
            Assert.IsTrue(target.Count == 0);
        }

        [TestMethod]
        public void CTOR_WithNullConfigurationManager_ThrowsArgumentNullException()
        {
            //Arrange
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                var target = new DiagnosticsAreaCollection(null);
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert 
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        public void Indexer_FindByStringIndexer()
        {
            //Arrange
            const string AreaName = "{E6C24B3D-8A81-4E0A-AAC9-85E5A5B9C09A}";
            var target = new DiagnosticsAreaCollection();
            var expected = new DiagnosticsArea(AreaName);
            target.Add(expected);

            //Act
            DiagnosticsArea foundArea = target[AreaName];

            //Assert
            Assert.IsNotNull(foundArea);
            Assert.AreEqual<DiagnosticsArea>(expected, foundArea);
        }

        [TestMethod]
        public void Indexer_WithMissingKey_ReturnsNull()
        {
            //Arrange

            //Act
            var target = new DiagnosticsAreaCollection();

            //Assert
            Assert.IsNull(target["{E6C24B3D-8A81-4E0A-AAC9-85E5A5B9C09A}"]);
        }

        [TestMethod]
        public void Indexer_WithNullName_ThrowsArgumentNullException()
        {
            //Arrange
            var target = new DiagnosticsAreaCollection();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                DiagnosticsArea area = target[null];
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert 
            Assert.IsTrue(expectedExceptionThrown, "Indexer with null index did not throw an exception");
        }

        [TestMethod]
        public void Add_WithDefaultConstructor()
        {
            //Arrange
            var target = new DiagnosticsAreaCollection();
            var expected = new DiagnosticsArea(TestsConstants.TestGuidName);

            //Act
            target.Add(expected);

            //Assert
            Assert.AreEqual<DiagnosticsArea>(target[0], expected);
        }

        [TestMethod]
        public void Add_WithConfigMgrConstructorWithoutConfigData()
        {
            //Arrange
            var configMgr = new SIConfigManager();
            var propBag = new BIPropertyBag();
            configMgr.ContainsKeyInPropertyBagStringIPropertyBag = (key, bag) => false;
            configMgr.GetPropertyBagConfigLevel = (level) => propBag;

            var target = new DiagnosticsAreaCollection(configMgr);
            var expected = new DiagnosticsArea(TestsConstants.TestGuidName);

            //Act
            target.Add(expected);

            //Assert
            Assert.AreEqual<DiagnosticsArea>(target[0], expected);
        }

        [TestMethod]
        public void Add_AreaWithConfigMgrConstructorWithConfigData()
        {
            //Arrange
            var configMgr = new MockConfigManager();
            var target = new DiagnosticsAreaCollection(configMgr);
            var expected = new DiagnosticsArea(TestsConstants.TestGuidName);

            //Act
            target.Add(expected);

            //Assert
            Assert.AreEqual<DiagnosticsArea>(target[TestsConstants.TestGuidName], expected);
        }


        [TestMethod]
        public void Add_DuplicateArea_ThrowsInvalidOperationException()
        {
            //Arrange
            var target = new DiagnosticsAreaCollection();
            var area = new DiagnosticsArea(MockConfigManager.Area1Name);
            target.Add(area);
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target.Add(area);
            }
            catch (InvalidOperationException)
            {
                expectedExceptionThrown = true;
            }

            //Assert
            Assert.IsTrue(expectedExceptionThrown);

        }

        [TestMethod]
        public void Add_NullArea_ThrowsArgumentNullException()
        {
            //Arrange
            var target = new DiagnosticsAreaCollection();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target.Add(null);
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert
            Assert.IsTrue(expectedExceptionThrown, "add with null area did not throw exception");
        }

        [TestMethod]
        public void Add_AreaWithNullName_ThrowsArgumentNullException()
        {
            //Arrange
            var target = new DiagnosticsAreaCollection();
            var area = new DiagnosticsArea();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target.Add(area);
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            Assert.IsTrue(expectedExceptionThrown, "Adding area with a null area name did not throw exception");
        }

        [TestMethod]
        public void Add_WithDefaultArea_ThrowsInvalidOperationException()
        {
            //Arrange
            bool expectedExceptionThrown = false;
            var target = new DiagnosticsAreaCollection();
            var area = new DiagnosticsArea(Constants.DefaultAreaName);

            //Act
            try
            {
                target.Add(area);
            }
            catch (InvalidOperationException)
            {
                expectedExceptionThrown = true;
            }

            Assert.IsTrue(expectedExceptionThrown, "adding default area to areas collection failed");
        }

        [TestMethod]
        public void Update_ByIndexer()
        {
            //Arrange
            var config = new MockConfigManager();
            var target = new DiagnosticsAreaCollection(config);
            var expected = new DiagnosticsArea(MockConfigManager.Area1Name);
            var category = new DiagnosticsCategory(Guid.NewGuid().ToString());
            expected.DiagnosticsCategories.Add(category);
            var originalArea = target[MockConfigManager.Area1Name];
            int index = target.IndexOf(originalArea);

            //Act
            target[index] = expected;

            //Assert
            Assert.IsTrue(target[index] == expected);
            Assert.IsTrue(target[index] != originalArea);
            Assert.IsTrue(target[index].DiagnosticsCategories.Count == 1);
            Assert.AreEqual(target[index].DiagnosticsCategories[0], category);
        }

        [TestMethod]
        public void Update_ByIndexerWithNullValue_ThrowsArgumentNullException()
        {
            //Arrange
            var config = new MockConfigManager();
            var target = new DiagnosticsAreaCollection(config);
            var originalArea = target[MockConfigManager.Area1Name];
            int index = target.IndexOf(originalArea);
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target[index] = null;
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert 
            Assert.IsTrue(expectedExceptionThrown);

        }

        [TestMethod]
        public void Update_ByIndexerWithNullAreaName_ThrowsArgumentNullException()
        {
            //Arrange
            var config = new MockConfigManager();
            var target = new DiagnosticsAreaCollection(config);
            var originalArea = target[MockConfigManager.Area1Name];
            int index = target.IndexOf(originalArea);
            var area = new DiagnosticsArea();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target[index] = area;
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert 
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        public void Indexer_UpdateWithDefaultArea_ThrowsInvalidOperationException()
        {
            //Arrange
            bool expectedExceptionThrown = false;
            var target = new DiagnosticsAreaCollection();
            var area = new DiagnosticsArea(Constants.DefaultAreaName);
            target.Add(new DiagnosticsArea("foobar"));

            //Act
            try
            {
                target[0] = area;
            }
            catch (InvalidOperationException)
            {
                expectedExceptionThrown = true;
            }

            Assert.IsTrue(expectedExceptionThrown, "updating to default area to areas collection failed");
        }

        [TestMethod]
        public void Update_ByStringIndexer()
        {
            //Arrange
            var config = new MockConfigManager();
            var target = new DiagnosticsAreaCollection(config);
            var expected = new DiagnosticsArea(MockConfigManager.Area1Name);
            var category = new DiagnosticsCategory(Guid.NewGuid().ToString());
            expected.DiagnosticsCategories.Add(category);

            //Act
            target[MockConfigManager.Area1Name] = expected;

            //Assert
            Assert.AreEqual<DiagnosticsArea>(target[MockConfigManager.Area1Name], expected);
            Assert.IsTrue(target[MockConfigManager.Area1Name].DiagnosticsCategories.Count == 1);
            Assert.AreEqual(target[MockConfigManager.Area1Name].DiagnosticsCategories[0], category);
        }

        [TestMethod]
        public void Update_ByStringIndexerWithNullKey_ThrowsArgumentNullException()
        {
            //Arrange
            var config = new MockConfigManager();
            var target = new DiagnosticsAreaCollection(config);
            var area = new DiagnosticsArea(TestsConstants.TestGuidName);
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target[null] = area;
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert
            Assert.IsTrue(expectedExceptionThrown);

        }
        
        [TestMethod]
        public void Update_ByStringIndexerWithNullValue_ThrowsArgumentNotNullException()
        {
            //Arrange
            var config = new MockConfigManager();
            var target = new DiagnosticsAreaCollection(config);
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target[MockConfigManager.Area1Name] = null;
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert 
            Assert.IsTrue(expectedExceptionThrown);

        }

        [TestMethod]
        public void Update_ByStringIndexerWithNullAreaName_ThrowsArgumentNullException()
        {
            //Arrange
            var config = new MockConfigManager();
            var target = new DiagnosticsAreaCollection(config);
            var area = new DiagnosticsArea();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target[MockConfigManager.Area1Name] = area;
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            //Assert 
            Assert.IsTrue(expectedExceptionThrown, "did not throw exception with null area name");
        }

        [TestMethod]
        public void Update_ByStringIndexerWithDuplicateArea_ThrowsInvalidOperationException()
        {
            //Arrange
            var config = new MockConfigManager();
            var target = new DiagnosticsAreaCollection(config);
            var area = new DiagnosticsArea(MockConfigManager.Area2Name);
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                target[MockConfigManager.Area1Name] = area;
            }
            catch (InvalidOperationException)
            {
                expectedExceptionThrown = true;
            }

            //Assert 
            Assert.IsTrue(expectedExceptionThrown, "exception not thrown with duplicate area added");
        }

        [TestMethod]
        public void Remove_AreaRemoveDefault()
        {
            //Arrange
            MockConfigManager config = new MockConfigManager();
            DiagnosticsAreaCollection areas = new DiagnosticsAreaCollection(config);

            DiagnosticsArea area = new DiagnosticsArea(MockConfigManager.Area2Name);
            DiagnosticsCategory category = new DiagnosticsCategory(MockConfigManager.Area2Category1Name);
            area.DiagnosticsCategories.Add(category);
            int areasCount = areas.Count;
            DiagnosticsArea originalArea = areas[MockConfigManager.Area2Name];

            //Act
            bool isRemoved = areas.Remove(originalArea);

            //Assert
            Assert.IsTrue(isRemoved);
            Assert.IsNull(areas[MockConfigManager.Area2Name]);
        }


        [TestMethod]
        public void Remove_NullAreaDoesntThrowException()
        {
            //Arrange
            MockConfigManager config = new MockConfigManager();
            DiagnosticsAreaCollection areas = new DiagnosticsAreaCollection(config);

            //Act
            areas.Remove(null);

            // Assert - no action required, it should be a no-op
        }


        [TestMethod]
        public void Save_AreasConfiguration()
        {
            //Arrange
            MockConfigManager config = new MockConfigManager();
            DiagnosticsAreaCollection areas = new DiagnosticsAreaCollection(config);

            //Act
            areas.SaveConfiguration();

            //Assert
            Assert.IsTrue(config.SaveCount == 1);
        }

        //The following two test methods show the same tests using stubs and behaved types for comparison.
        [TestMethod]
        public void Save_AreasConfigurationWithStubs()
        {
            //Arrange
            int saveCount = 0;
            var propBag = new BIPropertyBag();

            var config = new SIConfigManager
            {
                ContainsKeyInPropertyBagStringIPropertyBag = (key, bag) =>
                {
                    return false;
                },
                SetInPropertyBagStringObjectIPropertyBag = (key, o, bag) =>
                {
                    if (key == Constants.AreasConfigKey)
                        saveCount++;
                },

                GetPropertyBagConfigLevel = (level) => propBag,
            };

            DiagnosticsAreaCollection areas = new DiagnosticsAreaCollection(config);

            //Act
            areas.SaveConfiguration();

            //Assert
            Assert.IsTrue(saveCount == 1);
        }

        [TestMethod]
        public void Save_AreasConfiguration_WithBehavedTypes()
        {
            //Arrange
            var config = new MockLoggingConfigMgr();

            DiagnosticsAreaCollection areas = new DiagnosticsAreaCollection(config);

            //Act
            areas.SaveConfiguration();

            //Assert
            Assert.IsTrue(config.SaveCount == 1);
        }

  
        [TestMethod]
        public void SaveConfiguration_WithDefaultConstructor_ThrowsInvalidOperationException()
        {
            //Arrange
            DiagnosticsAreaCollection areas = new DiagnosticsAreaCollection();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                areas.SaveConfiguration();
            }
            catch (InvalidOperationException)
            {
                expectedExceptionThrown = true;
            }

            Assert.IsTrue(expectedExceptionThrown);
        }



        [TestMethod]
        public void SerializeDeserialize_WithAreasAndCategories()
        {
            //Arrange
            var target = new ConfigSettingSerializer();
            var expected = new DiagnosticsAreaCollection();


            var area1 = new DiagnosticsArea(TestArea1Name);
            area1.DiagnosticsCategories.Add(new DiagnosticsCategory(TestCategory11Name, EventSeverity.ErrorCritical, TraceSeverity.Medium));
            area1.DiagnosticsCategories.Add(new DiagnosticsCategory(TestCategory12Name, EventSeverity.Information, TraceSeverity.Verbose));
            var area2 = new DiagnosticsArea(TestArea2Name);
            area2.DiagnosticsCategories.Add(new DiagnosticsCategory(TestCategory21Name, EventSeverity.Information, TraceSeverity.Monitorable));
            area2.DiagnosticsCategories.Add(new DiagnosticsCategory(TestCategory22Name, EventSeverity.Warning, TraceSeverity.Medium));
            expected.Add(area1);
            expected.Add(area2);

            //Act
            string stringValue = target.Serialize(expected.GetType(), expected);
            var conversionResult = target.Deserialize(expected.GetType(), stringValue) as DiagnosticsAreaCollection;

            //Assert
            Assert.AreEqual(expected.Count, conversionResult.Count);
            Assert.AreEqual(expected[0].DiagnosticsCategories.Count, conversionResult[0].DiagnosticsCategories.Count);
            Assert.AreEqual(expected[0].DiagnosticsCategories[0].Name, conversionResult[0].DiagnosticsCategories[0].Name);
            Assert.AreEqual(expected[0].DiagnosticsCategories[1].Name, conversionResult[0].DiagnosticsCategories[1].Name);
            Assert.AreEqual(expected[0].DiagnosticsCategories[0].EventSeverity, conversionResult[0].DiagnosticsCategories[0].EventSeverity);
            Assert.AreEqual(expected[0].DiagnosticsCategories[0].TraceSeverity, conversionResult[0].DiagnosticsCategories[0].TraceSeverity);
            Assert.AreEqual(expected[0].DiagnosticsCategories[1].EventSeverity, conversionResult[0].DiagnosticsCategories[1].EventSeverity);
            Assert.AreEqual(expected[0].DiagnosticsCategories[1].TraceSeverity, conversionResult[0].DiagnosticsCategories[1].TraceSeverity);
            Assert.AreEqual(expected[1].DiagnosticsCategories.Count, conversionResult[1].DiagnosticsCategories.Count);
            Assert.AreEqual(expected[1].DiagnosticsCategories[0].Name, conversionResult[1].DiagnosticsCategories[0].Name);
            Assert.AreEqual(expected[1].DiagnosticsCategories[1].Name, conversionResult[1].DiagnosticsCategories[1].Name);
            Assert.AreEqual(expected[1].DiagnosticsCategories[0].EventSeverity, conversionResult[1].DiagnosticsCategories[0].EventSeverity);
            Assert.AreEqual(expected[1].DiagnosticsCategories[0].TraceSeverity, conversionResult[1].DiagnosticsCategories[0].TraceSeverity);
            Assert.AreEqual(expected[1].DiagnosticsCategories[1].EventSeverity, conversionResult[1].DiagnosticsCategories[1].EventSeverity);
            Assert.AreEqual(expected[1].DiagnosticsCategories[1].TraceSeverity, conversionResult[1].DiagnosticsCategories[1].TraceSeverity);
        }

    }
}
