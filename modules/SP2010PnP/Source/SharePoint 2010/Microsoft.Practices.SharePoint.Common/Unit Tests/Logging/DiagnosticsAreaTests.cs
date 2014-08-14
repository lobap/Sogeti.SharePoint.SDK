//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SharePoint.Common.Tests;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{
    /// <summary>
    /// Summary description for DiagnosticsAreaFixture
    /// </summary>
    [TestClass]
    public class DiagnosticsAreaTests
    {
        [TestMethod]
        public void CreateDefaultConstructorSucceeds()
        {
            //Arrange

            //Act
            var area = new DiagnosticsArea();

            // Assert
            Assert.AreEqual<string>(null, area.Name);
            Assert.AreEqual<int>(0, area.DiagnosticsCategories.Count);
        }

        [TestMethod]
        public void CreateNameConstructorSucceeds()
        {
            //Arrange
            const string areaName = TestsConstants.TestGuidName;
            //Act
            var area = new DiagnosticsArea(areaName);

            // Assert
            Assert.AreEqual<string>(areaName, area.Name);
            Assert.AreEqual<int>(0, area.DiagnosticsCategories.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateNameConstructorWithNullNameThrowsArgumentNullException()
        {
            //Arrange


            //Act
            var area = new DiagnosticsArea(null);

            // Assert caught by exception
        }

        [TestMethod]
        public void CreateNameAndValuesConstructorSucceeds()
        {
            string testCategory = TestsConstants.TestGuidName;

            //Arrange
            var diagnosticsCategories = new DiagnosticsCategoryCollection();
            diagnosticsCategories.Add(new DiagnosticsCategory(testCategory, 
                Microsoft.SharePoint.Administration.EventSeverity.Error, 
                Microsoft.SharePoint.Administration.TraceSeverity.Medium));

            //Act
            var area = new DiagnosticsArea("name", diagnosticsCategories);

            // Assert
            Assert.AreEqual<string>("name", area.Name);
            Assert.AreEqual<int>(1, area.DiagnosticsCategories.Count);
            Assert.AreEqual<int>(1, diagnosticsCategories.Count);
            Assert.IsTrue(area.DiagnosticsCategories[0].Name == testCategory);
            Assert.IsTrue(area.DiagnosticsCategories[0].EventSeverity == Microsoft.SharePoint.Administration.EventSeverity.Error);
            Assert.IsTrue(area.DiagnosticsCategories[0].TraceSeverity == Microsoft.SharePoint.Administration.TraceSeverity.Medium);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateNameAndValuesConstructorWithNullNameThrowsArgumentNullException()
        {
            //Arrange
            var diagnosticsCategories = new DiagnosticsCategoryCollection();

            //Act
            var area = new DiagnosticsArea(null, diagnosticsCategories);

            // Assert caught by exception

        }


        [TestMethod]
        public void CreateNameAndValuesConstructorWithNullAreasSucceeds()
        {
            //Arrange
             
            //Act
            var area = new DiagnosticsArea("name", null);

            // Assert
            Assert.AreEqual<string>("name", area.Name);
            Assert.AreEqual<int>(0, area.DiagnosticsCategories.Count);
        }


        [TestMethod]
        public void GetAreaNameSucceeds()
        {
            //Arrange
            var expected = TestsConstants.TestGuidName;
            var area = new DiagnosticsArea(expected);

            //Act
            string result = area.Name;

            // Assert
            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void SetAreaNameSucceeds()
        {
            //Arrange
            var expected = TestsConstants.TestGuidName;
            var area = new DiagnosticsArea();

            //Act
            area.Name = expected;

            // Assert
            Assert.AreEqual(expected, area.Name);

        } 

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetAreaNameToNullThrowsArgumentNullException()
        {
            //Arrange
            var area = new DiagnosticsArea("test name");

            //Act
            area.Name = null;

            // Assert (capture by ExpectedException)

        }
                

        [TestMethod]
        public void TranslateToSPDiagnosticsAreaSucceeds()
        {
            //Arrange
  
            var testCategory = "{0FE74678-8734-4355-AC62-45D8D211C3E6}";
            var testArea = "{DBAB3052-B89B-4D26-BB67-EF489C44CA57}";
            var area = new DiagnosticsArea(testArea);
            var category = new DiagnosticsCategory(testCategory);
            area.DiagnosticsCategories.Add(category);

            //Act
            var spArea = area.ToSPDiagnosticsArea();

            //Assert
            Assert.AreEqual<string>(spArea.Name, testArea);
            Assert.AreEqual<string>(spArea.LocalizedName, testArea);
        }


        [TestMethod]
        public void GetDefaultAreaSucceeds()
        {
            //Arrange

            //Act
            var spArea = DiagnosticsArea.DefaultSPDiagnosticsArea;

            //Assert
            Assert.AreEqual<string>(Constants.DefaultAreaName, spArea.Name);
        }
    }

  
}
