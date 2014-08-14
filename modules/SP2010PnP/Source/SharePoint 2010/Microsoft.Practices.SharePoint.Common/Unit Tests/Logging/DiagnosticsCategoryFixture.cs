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
    /// <summary>
    /// Summary description for DiagnosticsCategoryFixture
    /// </summary>
    [TestClass]
    public class DiagnosticsCategoryFixture
    { 
        [TestMethod]
        public void CanCreateInstanceWithProperties()
        {
            //Arrange

            //Act
            var category = new DiagnosticsCategory("test",
                EventSeverity.Verbose,
                TraceSeverity.Monitorable);

            //Assert
            Assert.AreEqual<string>("test", category.Name);
            Assert.AreEqual<EventSeverity>(EventSeverity.Verbose, category.EventSeverity);
            Assert.AreEqual<TraceSeverity>(TraceSeverity.Monitorable, category.TraceSeverity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowOnNullName()
        {
            //Arrange

            //Act
            new DiagnosticsCategory(null);

            //Assert
            //throw
        }

        [TestMethod]
        public void CanTranslateToSPDiagnosticsCategory()
        {
            //Arrange
            var category = new DiagnosticsCategory("test",
                EventSeverity.Verbose,
                TraceSeverity.Monitorable);

            //Act
            var spCat = category.ToSPDiagnosticsCategory();

            //Assert
            Assert.AreEqual<string>(category.Name, spCat.Name);
            Assert.AreEqual<EventSeverity>(category.EventSeverity, spCat.EventSeverity);
            Assert.AreEqual<TraceSeverity>(category.TraceSeverity, spCat.TraceSeverity);
            Assert.AreEqual<uint>(category.Id, spCat.Id);
            Assert.AreEqual<uint>(0, spCat.MessageId);
            Assert.AreEqual<bool>(false, spCat.Hidden);
            Assert.AreEqual<bool>(true, spCat.Shadow);
        }

        [TestMethod]
        public void CanGetDefaultSPDiagnosticsCategory()
        {
            //Arrange

            //Act
            var spCat = DiagnosticsCategory.DefaultSPDiagnosticsCategory;

            //Assert
            Assert.AreEqual<string>(Constants.DefaultCategoryName, spCat.Name);
            Assert.AreEqual<string>(Constants.DefaultCategoryName, spCat.LocalizedName);
            Assert.AreEqual<TraceSeverity>(TraceSeverity.Medium, spCat.TraceSeverity);
            Assert.AreEqual<EventSeverity>(EventSeverity.Information, spCat.EventSeverity);
            Assert.AreEqual<uint>(0, spCat.Id);
            Assert.AreEqual<uint>(0, spCat.MessageId);
            Assert.IsTrue(spCat.Shadow);
            Assert.IsFalse(spCat.Hidden);
        }
    }
}
