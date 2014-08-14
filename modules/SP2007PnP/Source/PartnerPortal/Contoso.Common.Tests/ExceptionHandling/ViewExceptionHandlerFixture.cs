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
using System.Web.UI;
using Contoso.Common.ExceptionHandling;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contoso.Common.Tests.ExceptionHandling
{
    [TestClass]
    public class ViewExceptionHandlerFixture
    {

        [TestInitialize]
        public void TestInitialize()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<ILogger, MockLogger>(InstantiationType.AsSingleton));
        }

        [TestCleanup]
        public void Cleanup()
        {
            SharePointServiceLocator.Reset();
        }

        [TestMethod]
        public void HandleViewExceptionShouldLogAndShowDefaultExceptionMessage()
        {
            var exceptionHandler = new ViewExceptionHandler();
            Exception exception = new Exception("Unhandled exception");
            MockErrorVisualizingView mockErrorVisualizingView = new MockErrorVisualizingView();
            exceptionHandler.HandleViewException(exception, mockErrorVisualizingView);

            Assert.IsTrue(mockErrorVisualizingView.DefaultErrorMessageShown);
            MockLogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>() as MockLogger;
            Assert.AreEqual("Unhandled exception", logger.ErrorMessage);
        }

        [TestMethod]
        public void HandleViewExceptionCanSwapMessage()
        {
            var exceptionHandler = new ViewExceptionHandler();
            Exception exception = new Exception("Unhandled exception");
            MockErrorVisualizingView mockErrorVisualizingView = new MockErrorVisualizingView();
            exceptionHandler.HandleViewException(exception, mockErrorVisualizingView, "Something went wrong");

            Assert.AreEqual("Something went wrong", mockErrorVisualizingView.ErrorMessage);
            MockLogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>() as MockLogger;
            Assert.AreEqual("Unhandled exception", logger.ErrorMessage);
        }


        [TestMethod]
        public void CanFindIViewErrorMessageInParents()
        {
            Control child = new Control();
            Control parent = new Control();
            MockErrorVisualizingView controlToFind = new MockErrorVisualizingView();

            controlToFind.Controls.Add(parent);
            parent.Controls.Add(child);

            var foundControl = ViewExceptionHandler.FindErrorVisualizer(child);

            Assert.AreSame(controlToFind, foundControl);
        }

        [TestMethod]
        public void FindIViewErrorWillREturnNullWhenNoViewErrorIsFound()
        {
            Control child = new Control();
            Control parent = new Control();
            Control controlNotToFind = new Control();

            controlNotToFind.Controls.Add(parent);
            parent.Controls.Add(child);

            var foundControl = ViewExceptionHandler.FindErrorVisualizer(child);

            Assert.IsNull(foundControl);
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void HandlingExceptionIsRobust()
        {
            MockErrorVisualizerThatThrowsError errorVisualizer = new MockErrorVisualizerThatThrowsError();

            var exceptionHandler = new ViewExceptionHandler();
            exceptionHandler.HandleViewException(new Exception("OriginalMessage"), errorVisualizer);
        }

        class MockControl : Control
        {
        }

        public class MockErrorVisualizerThatThrowsError : IErrorVisualizer
        {
            public void ShowDefaultErrorMessage()
            {
                throw new ArgumentException("MyMessage");
            }

            public void ShowErrorMessage(string errorMessage)
            {
                throw new ArgumentException("MyMessage");
            }
        }

        public class MockErrorVisualizingView : Control, IErrorVisualizer
        {
            public bool DefaultErrorMessageShown;
            public string ErrorMessage;

            public void ShowDefaultErrorMessage()
            {
                DefaultErrorMessageShown = true;
            }

            public void ShowErrorMessage(string errorMessage)
            {
                ErrorMessage = errorMessage;
            }
        }

    }
}