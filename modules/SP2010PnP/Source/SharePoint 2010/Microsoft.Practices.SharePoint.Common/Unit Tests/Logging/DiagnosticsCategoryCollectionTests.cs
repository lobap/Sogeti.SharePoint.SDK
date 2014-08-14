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
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.Tests;
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{
    /// <summary>
    /// Summary description for DiagnosticsCategoryCollectionFixture
    /// </summary>
    [TestClass]
    public class DiagnosticsCategoryCollectionTests
    {
        [TestMethod]
        public void ConstructCategoryCollectionSucceeds()
        {
            //Arrange

            //Act
            var target = new DiagnosticsCategoryCollection();

            //Assert
            Assert.IsTrue(target.Count == 0);
        }

        [TestMethod]
        public void ConstructCategoryCollectionWithListSucceeds()
        {
            //Arrange
            var addCategories = new List<DiagnosticsCategory>();
            string category2Name = "{2FC195E5-8102-449A-B42E-2FEC841E30ED}";
            var expected1 = new DiagnosticsCategory(TestsConstants.TestGuidName);
            var expected2 = new DiagnosticsCategory(category2Name);

            addCategories.Add(expected1);
            addCategories.Add(expected2);

            //Act
            var target = new DiagnosticsCategoryCollection(addCategories);

            //Assert
            Assert.IsTrue(target.Count == 2);
            Assert.AreEqual<DiagnosticsCategory>(expected1, target[0]);
            Assert.AreEqual<DiagnosticsCategory>(expected2, target[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructCategoryWithNullListThrowsArgumentNullException()
        {
            //Arrange
 
            //Act
            var target = new DiagnosticsCategoryCollection(null);

            //Assert caught by exception
        }


        [TestMethod]
        public void AddCategorySucceeds()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            var target = new DiagnosticsCategoryCollection();
            var expected = new DiagnosticsCategory(CategoryName);

            //Act
            target.Add(expected); 
            
            //Assert
            Assert.AreEqual<DiagnosticsCategory>(expected, target[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddCategoryWithNullCategoryThrowsArgumentNullException()
        {
             //Arrange
            var target = new DiagnosticsCategoryCollection();

            //Act
            target.Add(null); 
            
            //Assert caught by exception.    
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddCategoryWithNullNameThrowsArgumentNullException()
        {
            //Arrange
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory();

            //Act
            target.Add(category);

            //Assert caught by exception.    
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDuplicateCategoryThrowsInvalidOperationException()
        {
            //Arrange
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(TestsConstants.TestGuidName);
            var dupCategory = new DiagnosticsCategory(TestsConstants.TestGuidName);
            target.Add(category);

            //Act
            target.Add(dupCategory);

            //Assert caught by exception.    
        }


        [TestMethod]
        public void GetCategoryByNameSucceeds()
        {
            //Arrange
            const string Category1Name = TestsConstants.TestGuidName;
            const string Category2Name = "{351C28A4-2E3C-4C98-82CD-2B6FEEE3A12C}";
            const string Category3Name = "{238C3E82-E8FD-429B-85F7-8052CC1232B1}";
            var target = new DiagnosticsCategoryCollection();
            var expected1 = new DiagnosticsCategory(Category1Name);
            var expected2 = new DiagnosticsCategory(Category2Name);
            var expected3 = new DiagnosticsCategory(Category3Name);
            target.Add(expected1);
            target.Add(expected2);
            target.Add(expected3);

            //Act
            DiagnosticsCategory foundCategory1 = target[TestsConstants.TestGuidName];
            DiagnosticsCategory foundCategory2 = target[Category2Name];
            DiagnosticsCategory foundCategory3 = target[Category3Name];

            //Assert
            Assert.AreEqual<DiagnosticsCategory>(expected1, foundCategory1);
            Assert.AreEqual<DiagnosticsCategory>(expected2, foundCategory2);
            Assert.AreEqual<DiagnosticsCategory>(expected3, foundCategory3);
        }

        [TestMethod]
        public void GetMissingCategoryReturnsNullSucceeds()
        {
            //Arrange
            var target = new DiagnosticsCategoryCollection();

            //Act
            DiagnosticsCategory foundCategory = target["test"];

            //Assert
            Assert.IsNull(foundCategory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCategoryWithNullKeyThrowsArgumentNullException()
        {
            //Arrange
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(TestsConstants.TestGuidName);
            target.Add(category);

            //Act
            DiagnosticsCategory foundCategory = target[null];

            //Assert caught by exception.
        }


         [TestMethod]
         public void SetCategoryWithIndexSucceeds()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            const string newCategoryName = "{351C28A4-2E3C-4C98-82CD-2B6FEEE3A12C}";
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(CategoryName);
            target.Add(category);
            var expected = new DiagnosticsCategory(newCategoryName);
            
            //Act
            target[0] = expected;

            //Assert
            Assert.AreEqual<DiagnosticsCategory>(expected, target[0]);
        }


         [TestMethod]
         public void SetSameCategoryAtSameIndexSucceeds()
         {
             //Arrange
             const string CategoryName = TestsConstants.TestGuidName;
             var target = new DiagnosticsCategoryCollection();
             var category = new DiagnosticsCategory(CategoryName, EventSeverity.ErrorCritical, TraceSeverity.High);
             target.Add(category);
             DiagnosticsCategory expected = new DiagnosticsCategory(TestsConstants.TestGuidName, EventSeverity.Information, TraceSeverity.Medium);

             //Act
             target[0] = expected;

             //Assert 
             Assert.AreEqual<DiagnosticsCategory>(target[0], expected);
         }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCategoryWithNullThrowsInvalidOperationException()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(CategoryName);
            target.Add(category);

            //Act
            target[0] = null;

            //Assert caught by exeption.

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCategoryWithNullCategoryNameThrowsInvalidOperationException()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(CategoryName);
            target.Add(category);
            var nullNameCategory = new DiagnosticsCategory();

            //Act
            target[0] = nullNameCategory;

            //Assert caught by exeption.
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetDuplicateCategoryThrowsInvalidOperationException()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(CategoryName, EventSeverity.ErrorCritical, TraceSeverity.High);
            target.Add(category);
            DiagnosticsCategory newCategory = new DiagnosticsCategory(TestsConstants.TestGuidName, EventSeverity.Information, TraceSeverity.Medium);
            target.Add(newCategory);

            //Act
            target[0] = newCategory;

            //Assert caught by exception
        }


        [TestMethod]
        public void SetCategoryWithStringIndexSucceeds()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            const string newCategoryName = "{351C28A4-2E3C-4C98-82CD-2B6FEEE3A12C}";
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(CategoryName);
            target.Add(category);
            var expected = new DiagnosticsCategory(newCategoryName);

            //Act
            target[TestsConstants.TestGuidName] = expected;

            //Assert
            Assert.AreEqual<DiagnosticsCategory>(expected, target[newCategoryName]);
        }


        [TestMethod]
        public void SetSameCategoryAtSameStringIndexSucceeds()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(CategoryName, EventSeverity.ErrorCritical, TraceSeverity.High);
            target.Add(new DiagnosticsCategory("test1", EventSeverity.ErrorCritical, TraceSeverity.High));
            target.Add(category);
            DiagnosticsCategory expected = new DiagnosticsCategory(TestsConstants.TestGuidName, EventSeverity.Information, TraceSeverity.Medium);

            //Act
            target[TestsConstants.TestGuidName] = expected;

            //Assert 
            Assert.AreEqual<DiagnosticsCategory>(target[TestsConstants.TestGuidName], expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCategoryWithNullWithStringIndexThrowsInvalidOperationException()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(CategoryName);
            target.Add(category);

            //Act
            target[TestsConstants.TestGuidName] = null;

            //Assert caught by exeption.

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCategoryWithStringIndexWithNullCategoryNameThrowsInvalidOperationException()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(CategoryName);
            target.Add(category);
            var nullNameCategory = new DiagnosticsCategory();

            //Act
            target[TestsConstants.TestGuidName] = nullNameCategory;

            //Assert caught by exeption.
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetDuplicateCategoryWithStringIndexThrowsInvalidOperationException()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(CategoryName, EventSeverity.ErrorCritical, TraceSeverity.High);
            target.Add(category);
            DiagnosticsCategory expected = new DiagnosticsCategory(TestsConstants.TestGuidName, EventSeverity.Information, TraceSeverity.Medium);
            target.Add(expected);

            //Act
            target[TestsConstants.TestGuidName] = expected;

            //Assert 
            Assert.AreEqual<DiagnosticsCategory>(target[TestsConstants.TestGuidName], expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetWithStringIndexWithNullKeyThrowsArgumentNullException()
        {
            //Arrange
            const string CategoryName = TestsConstants.TestGuidName;
            var target = new DiagnosticsCategoryCollection();
            var category = new DiagnosticsCategory(CategoryName, EventSeverity.ErrorCritical, TraceSeverity.High);
            target.Add(category);
            DiagnosticsCategory newCategory = new DiagnosticsCategory(TestsConstants.TestGuidName, EventSeverity.Information, TraceSeverity.Medium);

            //Act
            target[null] = newCategory;

            //Assert caught by exception.
        }
    }
}
