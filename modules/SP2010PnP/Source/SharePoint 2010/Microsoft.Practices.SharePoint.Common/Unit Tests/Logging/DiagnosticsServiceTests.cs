//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.SharePoint.Moles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using System.Diagnostics.Moles;
using System.Collections.Generic;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{
    #region Testable mocks

    public class TestableDiagnosticServicesDefaultCateory : DiagnosticsService
    {
        public override SPDiagnosticsCategory DefaultCategory
        {
            get { return DiagnosticsCategory.DefaultSPDiagnosticsCategory; }
        }
    }

    public class TestableDiagnosticServicesLogging : DiagnosticsService
    {
        public const EventSeverity CatEventSeverity = EventSeverity.Error;
        public const TraceSeverity CatTraceSeverity = TraceSeverity.Monitorable;

        public override SPDiagnosticsCategory FindCategory(string categoryPath)
        {
            return new SPDiagnosticsCategory(categoryPath, CatTraceSeverity, CatEventSeverity);
        }
    }

    public class TestableProvideAreas : DiagnosticsService
    {
        public IEnumerable<SPDiagnosticsArea> TestProvideAreas()
        {
            return base.ProvideAreas();
        }
    }


    #endregion

    /// <summary>
    /// Summary description for DiagnosticsServiceFixture
    /// </summary>
    [TestClass]    
    public class DiagnosticsServiceTests
    {
        [TestInitialize]
        [TestCleanup]
        public void CleanUp()
        {
            SharePointServiceLocator.Reset();
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetLocalReturnsServiceSucceeds()
        {
            //Arrange
            DiagnosticsService expected = null;
            MSPDiagnosticsServiceBase.GetLocal<DiagnosticsService>(() => { Assert.IsTrue(expected == null);  expected = new DiagnosticsService(); return expected; });

            //Act
            var target = DiagnosticsService.Local;

            //Assert
            Assert.IsNotNull(expected);
            Assert.AreEqual<DiagnosticsService>(expected, target);
        }

        //Removing this unit test because it is executing an obsolete method. Compiler warning CS0618
        //[TestMethod]
        //[HostType("Moles")]
        //public void RegisterSucceeds()
        //{
        //    //Arrange  
        //    bool registeredCalled = false;


        //    MSPDiagnosticsServiceBase.ConstructorStringSPFarm = (instance, name, thefarm) =>
        //        {
        //            new MSPPersistedObject(instance)
        //            {
        //                UpdateBoolean = (val) => registeredCalled = true
        //            };
        //        };

        //    //Act
        //    DiagnosticsService.Register();

        //    Assert.IsTrue(registeredCalled);
        //}

        [TestMethod]
        [HostType("Moles")]
        public void UnRegisterSucceeds()
        {
            //Arrange
            SPService target = null;

            BSPFarm.SetLocal();
            MSPDiagnosticsServiceBase.GetLocal<DiagnosticsService>(() => new DiagnosticsService());
            MSPService.AllInstances.Delete = (b) => target = b;

            //Act
            DiagnosticsService.Unregister();

            //Assert
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GetType().Equals(typeof(DiagnosticsService)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindCategoryWithNullNameThrowsArgumentNullException()
        {
            //Arrange
            var expected = new TestableDiagnosticServicesDefaultCateory();

            //Act
            SPDiagnosticsCategory target = expected.FindCategory(null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FindCategoryWithEmptyNameThrowsArgumentException()
        {
            //Arrange
            var expected = new TestableDiagnosticServicesDefaultCateory();

            //Act
            SPDiagnosticsCategory target = expected.FindCategory(string.Empty);
        }

        [TestMethod]
        public void FindCategoryWithValidCategorySucceeds()
        {
            //Arrange
            var replaceLocator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);

            var target = new DiagnosticsService();
            var config = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>() as MockConfigManager;

            string category11Name = string.Format("{0}/{1}", MockConfigManager.Area1Name, MockConfigManager.Area1Category1Name);
            string category21Name = string.Format("{0}/{1}", MockConfigManager.Area2Name, MockConfigManager.Area2Category1Name);
            string category12Name = string.Format("{0}/{1}", MockConfigManager.Area1Name, MockConfigManager.Area1Category2Name);
            string category22Name = string.Format("{0}/{1}", MockConfigManager.Area2Name, MockConfigManager.Area2Category2Name);

            //Act
            SPDiagnosticsCategory category11 = target.FindCategory(category11Name);
            SPDiagnosticsCategory category12 = target.FindCategory(category12Name);
            SPDiagnosticsCategory category21 = target.FindCategory(category21Name);
            SPDiagnosticsCategory category22 = target.FindCategory(category22Name);

            // Assert
            Assert.AreEqual( MockConfigManager.Area1Category1Name, category11.Name);
            Assert.AreEqual(MockConfigManager.Area1Category2Name, category12.Name);
            Assert.AreEqual(MockConfigManager.Area2Category1Name, category21.Name);
            Assert.AreEqual(MockConfigManager.Area2Category2Name, category22.Name);
            Assert.AreEqual(MockConfigManager.Area1Name, category11.Area.Name);
            Assert.AreEqual(MockConfigManager.Area1Name, category12.Area.Name);
            Assert.AreEqual(MockConfigManager.Area2Name, category21.Area.Name);
            Assert.AreEqual(MockConfigManager.Area2Name, category22.Area.Name);
            Assert.IsTrue(config.LoadedCount == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(LoggingException))]
        public void FindCategoryWithCategoryNameFormatErrorThrowsLoggingException()
        {
            //Arrange
            var target = new DiagnosticsService();

            //Act
            target.FindCategory("badCategory");

            // Assert (capture by ExpectedException)
        }

        [TestMethod]
        public void FindCategoryWithMissingCategoryReturnsNull()
        {
            //Arrange
            string testAreaGuid = "{848D873D-EA3C-445A-874A-E80310EC50D4}";
            var replaceLocator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
            var target = new DiagnosticsService();
            string category11Name = string.Format("{0}/{1}", testAreaGuid, TestsConstants.TestGuidName);

            //Act
            SPDiagnosticsCategory category = target.FindCategory(category11Name);

            // Assert
            Assert.IsNull(category);
        }

        [TestMethod]
        public void ProvideAreasSucceeds()
        {
            //Arrange
            var target = new TestableProvideAreas();
            var replaceLocator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
 
            //Act
            IEnumerable<SPDiagnosticsArea> result = target.TestProvideAreas();

            // Assert
            List<SPDiagnosticsArea> list = new List<SPDiagnosticsArea>(result);

            Assert.IsTrue(list.Count == 3);
            Assert.AreEqual(MockConfigManager.Area1Name, list[0].Name);
            Assert.AreEqual(MockConfigManager.Area2Name, list[1].Name);
            Assert.AreEqual(target.DefaultCategory.Area.Name, list[2].Name); 
            Assert.IsTrue(list[0].Categories[MockConfigManager.Area1Category1Name] != null);
            Assert.IsTrue(list[0].Categories[MockConfigManager.Area1Category2Name] != null);
            Assert.IsTrue(list[1].Categories[MockConfigManager.Area2Category1Name] != null);
            Assert.IsTrue(list[1].Categories[MockConfigManager.Area2Category2Name] != null);
            Assert.IsTrue(list[2].Categories[target.DefaultCategory.Name] != null);

        }
        

        [TestMethod]
        [HostType("Moles")]
        public void LogToEventLogSucceeds()
        {
            //Arrange
            var target = new TestableDiagnosticServicesLogging();
            int idTOTest = 99;
            int idWritten = -1;
            SPDiagnosticsCategory categoryWritten = null;
            var severitySent = EventSeverity.Information;
            var severityWritten = EventSeverity.Error;
            string messageWritten = null;
            string testMessage = Guid.NewGuid().ToString();

            MSPDiagnosticsServiceBase.AllInstances.WriteEventUInt16SPDiagnosticsCategoryEventSeverityStringObjectArray 
                = (instance, ID, category, severity, str1, obj) =>
            {
                        messageWritten = str1;
                        idWritten = ID;
                        categoryWritten = category;
                        severityWritten = severity;
            };

            //Act
            target.LogEvent(testMessage, idTOTest, EventSeverity.Information, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual<EventSeverity>(severitySent, severityWritten);
            Assert.IsTrue(messageWritten.EndsWith(testMessage));
            Assert.AreEqual<int>(idWritten, idTOTest);
            Assert.AreEqual<string>(categoryWritten.Name, TestsConstants.AreasCategories);
        }

        [TestMethod]
        [HostType("Moles")]
        public void LogToEventLogNoSeveritySucceeds()
        {
            //Arrange
            var target = new TestableDiagnosticServicesLogging();
            int idTOTest = 99;
            int idWritten = -1;
            SPDiagnosticsCategory categoryWritten = null;
            var severityWritten = EventSeverity.Error;
            string messageWritten = null;
            string testMessage = Guid.NewGuid().ToString();

            MSPDiagnosticsServiceBase.AllInstances.WriteEventUInt16SPDiagnosticsCategoryEventSeverityStringObjectArray
                = (instance, ID, category, severity, str1, obj) =>
                {
                    messageWritten = str1;
                    idWritten = ID;
                    categoryWritten = category;
                    severityWritten = severity;
                };

            //Act
            target.LogEvent(testMessage, idTOTest, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual<EventSeverity>(TestableDiagnosticServicesLogging.CatEventSeverity, severityWritten);
            Assert.IsTrue(messageWritten.EndsWith(testMessage));
            Assert.AreEqual<int>(idWritten, idTOTest);
            Assert.AreEqual<string>(categoryWritten.Name, TestsConstants.AreasCategories);
        }

        [TestMethod]
        [HostType("Moles")]
        public void GetDefaultCategorySucceeds()
        {
            //Arrange
            var target = new DiagnosticsService();
            MSPFarm.LocalGet = () => new MSPFarm();
            var replaceLocator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockLoggingConfigMgr>(InstantiationType.AsSingleton);
 
            //Act
            SPDiagnosticsCategory defaultCat = target.DefaultCategory;

            //Assert
            Assert.IsTrue(defaultCat.Area.Name == Constants.DefaultAreaName);
            Assert.IsTrue(defaultCat.Name == Constants.DefaultCategoryName);
        }


        [TestMethod]
        [HostType("Moles")]
        public void LogWithoutContextSucceeds()
        {            
            //Arrange
            bool eventLogWritten = false;
            MSPContext.CurrentGet = () => null;
            MSPFarm.LocalGet = () => new MSPFarm();
            var target = new TestableDiagnosticServicesLogging();
 
            MSPDiagnosticsServiceBase.AllInstances.
                WriteEventUInt16SPDiagnosticsCategoryEventSeverityStringObjectArray = (diagnosticsServiceBase,
                                                                                  ID,
                                                                                  category, 
                                                                                  severity,
                                                                                  str1, obj) =>
            {
                eventLogWritten = true;
            };

            //Act
            target.LogEvent("Message", 99, EventSeverity.Information, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(eventLogWritten);
        }

        [TestMethod]
        [HostType("Moles")]
        public void LogEventWithEmptyStringDefaultCategorySucceeds()
        {
            //Arrange
            var replaceLocator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);
            var service = new DiagnosticsService();
            SPDiagnosticsCategory target = null;
            bool eventLogWritten = false;

            MSPDiagnosticsServiceBase.AllInstances.WriteEventUInt16SPDiagnosticsCategoryEventSeverityStringObjectArray
                 = (diagnosticsServiceBase, ID, category, severity, str1, obj) =>
            {
                eventLogWritten = true;
                target = category;
            };

            //Act
            service.LogEvent("Message", 99, EventSeverity.Information, string.Empty);

            //Assert
            Assert.IsTrue(eventLogWritten);
            Assert.AreEqual<SPDiagnosticsCategory>(service.DefaultCategory, target);
        }

        [TestMethod]
        [HostType("Moles")]
        public void LogEventWithNullDefaultCategorySucceeds()
        {
            //Arrange
            var replaceLocator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);
            var service = new DiagnosticsService();
            SPDiagnosticsCategory target = null;
            bool eventLogWritten = false;

            MSPDiagnosticsServiceBase.AllInstances.WriteEventUInt16SPDiagnosticsCategoryEventSeverityStringObjectArray
                 = (diagnosticsServiceBase, ID, category, severity, str1, obj) =>
                 {
                     eventLogWritten = true;
                     target = category;
                 };

            //Act
            service.LogEvent("Message", 99, EventSeverity.Information, null);

            //Assert
            Assert.IsTrue(eventLogWritten);
            Assert.AreEqual<SPDiagnosticsCategory>(service.DefaultCategory, target);
        }

        [TestMethod]
        [ExpectedException(typeof(LoggingException))]
        public void LogWithInvalidCategoryThrowsLoggingException()
        {
            //Arrange
            var replaceLocator = new ActivatingServiceLocator();
            const string invalidCategory = TestsConstants.TestGuidName;     // should be of form area/category
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);
            var target = new DiagnosticsService();

            //Act
            target.LogEvent("Message", 99, EventSeverity.Information, invalidCategory);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(LoggingException))]
        public void LogWithMissingCategoryThrowsLoggingException()
        {
            //Arrange
            var replaceLocator = new ActivatingServiceLocator();
            const string testArea = "{649D6544-ED41-46D1-ABC6-72B9D94C3D4C}";
            string missingCategory = string.Format("{0}/{1}", testArea, TestsConstants.TestGuidName);     // should be of form area/category
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);
            var target = new DiagnosticsService();

            //Act
            target.LogEvent("Message", 99, EventSeverity.Information, missingCategory);

            //Assert
        }

        [TestMethod]
        [HostType("Moles")]
        public void TraceSucceeds()
        {
            //Arrange
            var target = new TestableDiagnosticServicesLogging();
            int idTOTest = 99;
            uint idWritten = 0;
            SPDiagnosticsCategory categoryWritten = null;
            var severitySent = TraceSeverity.Medium;
            var severityWritten = TraceSeverity.Verbose;
            string messageWritten = null;
            string testMessage = Guid.NewGuid().ToString();

            MSPDiagnosticsServiceBase.AllInstances.WriteTraceUInt32SPDiagnosticsCategoryTraceSeverityStringObjectArray 
                = (instance, ID, category, severity, str1, obj) =>
            {
                messageWritten = str1;
                idWritten = ID;
                categoryWritten = category;
                severityWritten = severity;
            };

            //Act
            target.LogTrace(testMessage, idTOTest, severitySent, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual<TraceSeverity>(severitySent, severityWritten);
            Assert.IsTrue(messageWritten.EndsWith(testMessage));
            Assert.AreEqual<uint>(idWritten, (uint)idTOTest);
            Assert.AreEqual<string>(categoryWritten.Name, TestsConstants.AreasCategories);          
         }


        [TestMethod]
        [HostType("Moles")]
        public void TraceWithoutContextSucceeds()
        {
            //Arrange
            bool traceLogWritten = false;
            MSPContext.CurrentGet = () => null;
            MSPFarm.LocalGet = () => new MSPFarm();
            var target = new TestableDiagnosticServicesLogging();

            MSPDiagnosticsServiceBase.AllInstances.WriteTraceUInt32SPDiagnosticsCategoryTraceSeverityStringObjectArray
                = (diagnosticsServiceBase, ID, category, severity, str1, obj) =>
            {
                traceLogWritten = true;
            };

            //Act
            target.LogTrace("Message", 99, TraceSeverity.Verbose, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(traceLogWritten);
        }

        [TestMethod]
        [HostType("Moles")]
        public void TraceWithEmptyStringDefaultCategorySucceeds()
        {
            //Arrange
            var replaceLocator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);
            var service = new DiagnosticsService();
            bool traceLogWritten = false;
            SPDiagnosticsCategory target = null;

            MSPDiagnosticsServiceBase.AllInstances.WriteTraceUInt32SPDiagnosticsCategoryTraceSeverityStringObjectArray
                = (diagnosticsServiceBase, ID, category, severity, str1, obj) =>
            {
                traceLogWritten = true;
                target = category;
            };

            //Act
            service.LogTrace("Message", 99, TraceSeverity.Verbose, string.Empty);
  
            //Assert
            Assert.IsTrue(traceLogWritten);
            Assert.AreEqual<SPDiagnosticsCategory>(service.DefaultCategory, target);
        }

        [TestMethod]
        [HostType("Moles")]
        public void TraceWithNullDefaultCategorySucceeds()
        {
            //Arrange
            var replaceLocator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);
            var service = new DiagnosticsService();
            bool traceLogWritten = false;
            SPDiagnosticsCategory target = null;

            MSPDiagnosticsServiceBase.AllInstances.WriteTraceUInt32SPDiagnosticsCategoryTraceSeverityStringObjectArray
                = (diagnosticsServiceBase, ID, category, severity, str1, obj) =>
                {
                    traceLogWritten = true;
                    target = category;
                };

            //Act
            service.LogTrace("Message", 99, TraceSeverity.Verbose, null);

            //Assert
            Assert.IsTrue(traceLogWritten);
            Assert.AreEqual<SPDiagnosticsCategory>(service.DefaultCategory, target);
        }

        [TestMethod]
        [HostType("Moles")]
        public void TraceNoSeveritySucceeds()
        {
            //Arrange
            var target = new TestableDiagnosticServicesLogging();
            int idTOTest = 99;
            uint idWritten = 1024;
            SPDiagnosticsCategory categoryWritten = null;
            var severityWritten = TraceSeverity.Medium;
            string messageWritten = null;
            string testMessage = Guid.NewGuid().ToString();

            MSPDiagnosticsServiceBase.AllInstances.WriteTraceUInt32SPDiagnosticsCategoryTraceSeverityStringObjectArray
                = (instance, ID, category, severity, str1, obj) =>
                {
                    messageWritten = str1;
                    idWritten = ID;
                    categoryWritten = category;
                    severityWritten = severity;
                };

            //Act
            target.LogTrace(testMessage, idTOTest, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual<TraceSeverity>(TestableDiagnosticServicesLogging.CatTraceSeverity, severityWritten);
            Assert.IsTrue(messageWritten.EndsWith(testMessage));
            Assert.AreEqual((int) idWritten, idTOTest);
            Assert.AreEqual<string>(categoryWritten.Name, TestsConstants.AreasCategories);
        }

        [TestMethod]
        [ExpectedException(typeof(LoggingException))]
        public void TraceWithInvalidCategoryThrowsLoggingException()
        {
            //Arrange
            var replaceLocator = new ActivatingServiceLocator();
            const string invalidCategory = TestsConstants.TestGuidName;     // should be of form area/category
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);
            var target = new DiagnosticsService();

            //Act
            target.LogTrace("Message", 99, TraceSeverity.Medium, invalidCategory);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(LoggingException))]
        public void TraceWithMissingCategoryThrowsLoggingException()
        {
            //Arrange
            var replaceLocator = new ActivatingServiceLocator();
            const string testArea = "{649D6544-ED41-46D1-ABC6-72B9D94C3D4C}";
            string missingCategory = string.Format("{0}/{1}", testArea, TestsConstants.TestGuidName);     // should be of form area/category
            SharePointServiceLocator.ReplaceCurrentServiceLocator(replaceLocator);
            replaceLocator.RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton);
            replaceLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>(InstantiationType.AsSingleton);
            var target = new DiagnosticsService();

            //Act
            target.LogTrace("Message", 99, TraceSeverity.Medium, missingCategory);

            //Assert
        }
    }
}
