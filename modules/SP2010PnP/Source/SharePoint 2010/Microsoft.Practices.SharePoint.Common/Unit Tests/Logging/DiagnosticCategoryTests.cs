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
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{
    [TestClass]
    public partial class DiagnosticCategoryTests
    {

        [TestMethod]
        public void CreateDiagnosticCategory()
        {
            //Arrange
            string guidName = TestsConstants.TestGuidName;

            //Act
            var category = new DiagnosticsCategory();
            category.Name = guidName;
            category.TraceSeverity = TraceSeverity.Monitorable;
            category.EventSeverity = EventSeverity.ErrorCritical;

            //Assert
            Assert.AreEqual(guidName, category.Name);
            Assert.AreEqual(TraceSeverity.Monitorable, category.TraceSeverity);
            Assert.AreEqual(EventSeverity.ErrorCritical, category.EventSeverity);
        }

        [TestMethod]
        public void CreateDiagnosticCategoryWithSeverity()
        {
            //Arrange
            string guidName = TestsConstants.TestGuidName;

            //Act
            var category = new DiagnosticsCategory(guidName, EventSeverity.ErrorCritical, TraceSeverity.Monitorable);

            //Assert
            Assert.AreEqual(guidName, category.Name);
            Assert.AreEqual(TraceSeverity.Monitorable, category.TraceSeverity);
            Assert.AreEqual(EventSeverity.ErrorCritical, category.EventSeverity);
        }

        [TestMethod]
        public void CreateDiagnosticCategoryWithName()
        {
            //Arrange
            string guidName = TestsConstants.TestGuidName;

            //Act
            var category = new DiagnosticsCategory(guidName);

            //Assert
            Assert.AreEqual(guidName, category.Name);
            Assert.AreEqual( DiagnosticsCategory.DefaultTraceSeverity, category.TraceSeverity);
            Assert.AreEqual(DiagnosticsCategory.DefaultEventSeverity, category.EventSeverity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDiagnosticCategoryWithNullNameThrowsArgumentNullException()
        {
            //Arrange
 
            //Act
            var category = new DiagnosticsCategory(null);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDiagnosticCategoryWithNullNameAndSeverityThrowsArgumentNullException()
        {
            //Arrange

            //Act
            var category = new DiagnosticsCategory(null, EventSeverity.ErrorCritical, TraceSeverity.Monitorable);

            //Assert
        }

        [TestMethod]
        public void SetDiagnosticCategoryNameSucceeds()
        {
            //Arrange
            var category = new DiagnosticsCategory();
            
            //Act
            category.Name = TestsConstants.TestGuidName;

            //Assert
            Assert.AreEqual(TestsConstants.TestGuidName, category.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetDiagnosticCategoryNameWithNullThrowsArgumentNullException()
        {
            //Arrange
            var category = new DiagnosticsCategory();

            //Act
            category.Name = null;

            //Assert
        }

        [TestMethod]
        public void SetDiagnosticCategoryIdSucceeds()
        {
            //Arrange
            var target = new DiagnosticsCategory();
            uint expected = 2001;

            //Act
            target.Id = expected;

            //Assert
            Assert.AreEqual(expected, target.Id);
        }

 
    }
}
