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
using ExecutionModels.Sandboxed.AggregateView;
using System.Data;
using Microsoft.SharePoint;

namespace ExecutionModels.Sandboxed.Tests
{
    [TestClass]
    public class AggregateViewPresenterTests
    {
        [TestMethod]
        public void SetSiteData_PopulatesViewDataWithModel()
        {
            // Arrange
            var view = new MockAggregateView();
            var model = new MockEstimatesQueryService();
            var presenter = new AggregateViewPresenter(view, model);

            // Act
            presenter.SetSiteData();

            // Assert
            Assert.AreEqual<DataTable>(view.Data, model.Data);
        }

        [TestMethod]
        public void SetSiteData_HandlesException()
        {
            // Arrange
            var view = new MockAggregateView();
            var model = new MockEstimatesQueryService(new InvalidOperationException("test"));
            var presenter = new AggregateViewPresenter(view, model);
            var errorVisualizer = new MockErrorVisualizer();
            presenter.ErrorVisualizer = errorVisualizer;
            presenter.ExceptionHandler = new MockViewExceptionHandler();

            // Act
            presenter.SetSiteData();

            // Assert
            Assert.AreEqual<string>("test", errorVisualizer.ErrorMessage);
        }
    }
}
