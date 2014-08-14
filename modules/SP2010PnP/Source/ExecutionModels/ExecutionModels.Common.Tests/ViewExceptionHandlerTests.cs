//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace ExecutionModels.Common.Tests
{
    using System;
    using System.Web.UI;
    using ExecutionModels.Common.ExceptionHandling;
    using ExecutionModels.ExceptionHandling;
    using Microsoft.Practices.SharePoint.Common.Logging;
    using Microsoft.Practices.SharePoint.Common.ServiceLocation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ViewExceptionHandlerTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ActivatingServiceLocator locator = new ActivatingServiceLocator();
            locator.RegisterTypeMapping<ILogger, MockLogger>(InstantiationType.AsSingleton);
            SharePointServiceLocator.ReplaceCurrentServiceLocator(locator);
        }

        [TestCleanup]
        public void Cleanup()
        {
            SharePointServiceLocator.Reset();
        }

        [TestMethod]
        public void HandleViewException_LogsAndShowsDefaultExceptionMessage()
        {
            // Arrange
            var exceptionHandler = new ViewExceptionHandler();
            Exception exception = new Exception("Unhandled exception");
            MockErrorVisualizingView mockErrorVisualizingView = new MockErrorVisualizingView();
            
            // Act
            exceptionHandler.HandleViewException(exception, mockErrorVisualizingView);

            // Assert
            Assert.IsTrue(mockErrorVisualizingView.DefaultErrorMessageShown);
            MockLogger logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>() as MockLogger;
            Assert.AreEqual("Unhandled exception", logger.ErrorMessage);
        }

        [TestMethod]
        public void HandleViewException_SwapsMessage()
        {
            // Arrange
            var exceptionHandler = new ViewExceptionHandler();
            var exception = new Exception("Unhandled exception");
            var mockErrorVisualizingView = new MockErrorVisualizingView();
            
            // Act
            exceptionHandler.HandleViewException(exception, mockErrorVisualizingView, "Something went wrong");

            // Assert
            Assert.AreEqual("Something went wrong", mockErrorVisualizingView.ErrorMessage);
            MockLogger logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>() as MockLogger;
            Assert.AreEqual("Unhandled exception", logger.ErrorMessage);
        }


        [TestMethod]
        public void FindErrorVisualizer_FindsIErrorVisualizerInParents()
        {
            // Arrange
            var child = new Control();
            var parent = new Control();
            var controlToFind = new MockErrorVisualizingView();

            // Act
            controlToFind.Controls.Add(parent);
            parent.Controls.Add(child);
            var foundControl = ViewExceptionHandler.FindErrorVisualizer(child);

            // Assert
            Assert.AreSame(controlToFind, foundControl);
        }

        [TestMethod]
        public void FindErrorVisualizer_ReturnsNull_WhenNoViewErrorIsFound()
        {
            // Arrange
            var child = new Control();
            var parent = new Control();
            var controlNotToFind = new Control();

            // Act
            controlNotToFind.Controls.Add(parent);
            parent.Controls.Add(child);
            var foundControl = ViewExceptionHandler.FindErrorVisualizer(child);

            // Assert
            Assert.IsNull(foundControl);
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void HandleViewException_ThrowsExceptionHandlingException_WhenErrorVisualizaerHasProblems()
        {
            // Arrange
            var errorVisualizer = new MockErrorVisualizerThatThrowsError();
            var exceptionHandler = new ViewExceptionHandler();

            // Act
            exceptionHandler.HandleViewException(new Exception("OriginalMessage"), errorVisualizer);

            // Assert - throw
        }

        #region Mocks

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
        #endregion
    }
}