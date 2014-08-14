//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ExecutionModels.Sandboxed.Proxy.AggregateView;
using ExecutionModels.Sandboxed.Proxy.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExecutionModels.Sandboxed.Proxy.Tests
{
    [TestClass]
    public class AggregateViewPresenterTests
    {
        [TestMethod]
        public void SetSiteData_SetsDataTableIntoView()
        {
            var view = new MockAggregateView();
            var model = new MockEstimateService();
            model.Data = new DataTable();
            var presenter = new AggregateViewPresenter(view, model);

            presenter.SetSiteData();

            Assert.AreSame(view.Data, model.Data);
        }

        [TestMethod]
        public void SetSiteData_HandlesException()
        {
            // Arrange
            var view = new MockAggregateView();
            var model = new MockEstimateService(new InvalidOperationException("test"));
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