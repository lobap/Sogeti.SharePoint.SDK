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
using Microsoft.Practices.SharePoint.Common.Logging.LoggerProxy;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.SharePoint.Moles;
using Microsoft.SharePoint.Behaviors;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.Tests.Logging;
using Microsoft.Practices.SharePoint.Common.Logging.Moles;

namespace Microsoft.Practices.SharePoint.Common.Tests.Proxies
{
    [TestClass]
    public class LoggingOperationTests
    {
        [TestMethod]
        public void Execute_WithNullArguments_ThrowsArgumentNullException()
        {
            //Arrange
            var operation = new LoggingOperation();
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
            var args = new LoggingOperationArgs();
            args.Message = null;
            args.EventId = -1;
            args.Category = "foobar";
            args.Severity = (int) EventSeverity.Error;
            var operation = new LoggingOperation();
            BSPFarm.SetLocal();

            //Act
            object target =  operation.Execute(args);

            //Assert
            Assert.IsInstanceOfType(target, typeof(ArgumentNullException));
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_WithWrongProxyArgsType_ReturnsArgumentException()
        {
            //Arrange
            var args = new TracingOperationArgs();
            args.Message = "foobar";
            args.EventId = -1;
            args.Category = "foobar";
            args.Severity = (int)EventSeverity.Error;
            var operation = new LoggingOperation();
            BSPFarm.SetLocal();

            //Act
            object target = operation.Execute(args);

            //Assert
            Assert.IsInstanceOfType(target, typeof(ArgumentException));
        }

        [TestMethod]
        [HostType("Moles")]
        public void Execute_LogsMessage()
        {
            //Arrange
            var args = new LoggingOperationArgs();
            Guid createGuid = Guid.Empty;
            args.Message = TestsConstants.TestGuidName;
            args.EventId = -99;
            args.Category = TestsConstants.AreasCategories;
            args.Severity = (int)SandboxEventSeverity.Error;
            args.SiteID = TestsConstants.TestGuid;
  
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator());
            ((ActivatingServiceLocator)SharePointServiceLocator.GetCurrent())
                .RegisterTypeMapping<ILogger, TestLogger>(InstantiationType.AsSingleton);

            var operation = new LoggingOperation();
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
                        IDGet = ()=> TestsConstants.TestGuid,
                        RootWebGet = ()=>mweb
                    };
                    
                    createGuid = g;
                };


            //Act
            object target = operation.Execute(args);

            //Assert
            var logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>() as TestLogger;

            Assert.AreEqual(TestsConstants.TestGuid, createGuid);
            Assert.IsTrue(logger.Message.Contains(args.Message));
            Assert.AreEqual(logger.Category, TestsConstants.AreasCategories);
            Assert.AreEqual(logger.EventId, args.EventId);
            Assert.AreEqual(logger.Severity, SandboxEventSeverity.Error);
        }


        [TestMethod]
        [HostType("Moles")]
        public void Execute_LogsMessage_WithoutSeverity()
        {
            //Arrange
            var args = new LoggingOperationArgs();
            Guid createGuid = Guid.Empty;
            args.Message = TestsConstants.TestGuidName;
            args.EventId = -99;
            args.Category = TestsConstants.AreasCategories;
            args.Severity = null;
            args.SiteID = TestsConstants.TestGuid;

            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator());
            ((ActivatingServiceLocator)SharePointServiceLocator.GetCurrent())
                .RegisterTypeMapping<ILogger, TestLogger>(InstantiationType.AsSingleton);

            var operation = new LoggingOperation();
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
            var logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>() as TestLogger;

            Assert.AreEqual(TestsConstants.TestGuid, createGuid);
            Assert.IsTrue(logger.Message.Contains(args.Message));
            Assert.AreEqual(logger.Category, TestsConstants.AreasCategories);
            Assert.AreEqual(logger.EventId, args.EventId);
        }

        public class TestLogger: SILogger
        {
            public string Message {get; private set;}
            public int EventId {get; private set;}
            public string Category {get; private set;}
            public SandboxEventSeverity Severity {get; private set;}

            public TestLogger()
            {
                 LogToOperationsStringInt32SandboxEventSeverityString = (message, eventId, eventSeverity, category) =>
                {
                    this.Message = message;
                    this.EventId = eventId;
                    this.Category = category;
                    this.Severity = eventSeverity;
                };

                 LogToOperationsStringInt32String = (message, eventId, category) =>
                     {
                         this.Message = message;
                         this.EventId = eventId;
                         this.Category = category;
                     };
            }

        }
    }
}
