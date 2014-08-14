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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SharePoint.Common.Logging.Moles;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Moles;
using Microsoft.SharePoint.Behaviors;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.Practices.SharePoint.Common.Logging.LoggerProxy;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;

namespace Microsoft.Practices.SharePoint.Common.Tests.Proxies
{
    [TestClass]
    public class TracingOperationTests
    {
        [TestMethod]
        public void Execute_WithNullArguments_ThrowsArgumentNullException()
        {
            //Arrange
            var operation = new TracingOperation();
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                operation.Execute(null);
            }
            catch (ArgumentNullException)
            {
                expectedExceptionThrown = true;
            }

            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_WithNoMessageInProxyArgs_ReturnsNullArgumentException()
        {
            //Arrange
            var args = new TracingOperationArgs();
            args.Message = null;
            args.EventId = -1;
            args.Category = "foobar";
            args.Severity = (int)SandboxTraceSeverity.High;
            var operation = new TracingOperation();
            BSPFarm.SetLocal();

            //Act
            object target = operation.Execute(args);

            //Assert
            Assert.IsInstanceOfType(target, typeof(ArgumentNullException));
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_WithWrongProxyArgsType_ReturnsArgumentException()
        {
            //Arrange
            var args = new LoggingOperationArgs();
            args.Message = "foobar";
            args.EventId = -1;
            args.Category = "foobar";
            args.Severity = (int)SandboxEventSeverity.Error;
            var operation = new TracingOperation();
            BSPFarm.SetLocal();

            //Act
            object target = operation.Execute(args);

            //Assert
            Assert.IsInstanceOfType(target, typeof(ArgumentException));
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_TracesMessage()
        {
            //Arrange
            var args = new TracingOperationArgs();
            Guid createGuid = Guid.Empty;
            args.Message = TestsConstants.TestGuidName;
            args.EventId = -99;
            args.Category = TestsConstants.AreasCategories;
            args.Severity = (int)SandboxTraceSeverity.High;
            args.SiteID = TestsConstants.TestGuid;

            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator());
            ((ActivatingServiceLocator)SharePointServiceLocator.GetCurrent())
                .RegisterTypeMapping<ILogger, TestTraceLogger>(InstantiationType.AsSingleton);

            var operation = new TracingOperation();
            BSPFarm.SetLocal();
            MSPSite site;
            BSPWeb bweb = new BSPWeb();
            var mweb = new MSPWeb(bweb)
            {
                NameGet = () => "foo.bar",
            };


            MSPSite.ConstructorGuid = (instance, g) =>
            {
                site = new MSPSite(instance)
                {
                    Dispose = () => { },
                    IDGet = () => TestsConstants.TestGuid,
                    RootWebGet = () => mweb
                };

                createGuid = g;
            };


            //Act
            object target = operation.Execute(args);

            //Assert
            var logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>() as TestTraceLogger;

            Assert.IsNotInstanceOfType(target, typeof(Exception));
            Assert.AreEqual(TestsConstants.TestGuid, createGuid);
            Assert.IsTrue(logger.Message.Contains(args.Message));
            Assert.AreEqual(logger.Category, TestsConstants.AreasCategories);
            Assert.AreEqual(logger.EventId, args.EventId);
            Assert.AreEqual(logger.Severity, SandboxTraceSeverity.High);
        }


        [TestMethod]
        [HostType("Moles")]
        public void Execute_TracesMessage_WithoutSeverity()
        {
            //Arrange
            var args = new TracingOperationArgs();
            Guid createGuid = Guid.Empty;
            args.Message = TestsConstants.TestGuidName;
            args.EventId = -99;
            args.Category = TestsConstants.AreasCategories;
            args.Severity = null;
            args.SiteID = TestsConstants.TestGuid;

            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator());
            ((ActivatingServiceLocator)SharePointServiceLocator.GetCurrent())
                .RegisterTypeMapping<ILogger, TestTraceLogger>(InstantiationType.AsSingleton);

            var operation = new TracingOperation();
            BSPFarm.SetLocal();
            MSPSite site;
            BSPWeb bweb = new BSPWeb();
            var mweb = new MSPWeb(bweb)
            {
                NameGet = () => "foo.bar",
            };


            MSPSite.ConstructorGuid = (instance, g) =>
            {
                site = new MSPSite(instance)
                {
                    Dispose = () => { },
                    IDGet = () => TestsConstants.TestGuid,
                    RootWebGet = () => mweb
                };

                createGuid = g;
            };


            //Act
            object target = operation.Execute(args);

            //Assert
            var logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>() as TestTraceLogger;

            Assert.AreEqual(TestsConstants.TestGuid, createGuid);
            Assert.IsTrue(logger.Message.Contains(args.Message));
            Assert.AreEqual(logger.Category, TestsConstants.AreasCategories);
            Assert.AreEqual(logger.EventId, args.EventId);
        }

        public class TestTraceLogger : SILogger
        {
            public string Message { get; private set; }
            public int EventId { get; private set; }
            public string Category { get; private set; }
            public SandboxTraceSeverity Severity { get; private set; }

            public TestTraceLogger()
            {
                TraceToDeveloperStringInt32SandboxTraceSeverityString = (message, eventId, eventSeverity, category) =>
                {
                    this.Message = message;
                    this.EventId = eventId;
                    this.Category = category;
                    this.Severity = eventSeverity;
                };

                TraceToDeveloperStringInt32String = (message, eventId, category) =>
                {
                    this.Message = message;
                    this.EventId = eventId;
                    this.Category = category;
                };
            }
        }
    }
}
