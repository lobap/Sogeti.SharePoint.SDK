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
using Microsoft.Practices.SharePoint.Common.Logging;
using System.Diagnostics.Moles;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Configuration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{
    [TestClass]
    public class DiagnosticsAreaEventSourceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterAreas_ThrowOnNullAreas()
        {
            DiagnosticsAreaEventSource.RegisterAreas(null);
        }

        [TestMethod]
        [HostType("Moles")]
        public void RegisterAreas_BypassRegistrationOnEmptyCollection()
        {
            // Arrange
             string sourceName = null;
            string logName = null;

            // Act
            DiagnosticsAreaEventSource.RegisterAreas(new DiagnosticsAreaCollection());

            // Assert
            Assert.IsNull(sourceName);
            Assert.IsNull(logName);
        }

        [TestMethod]
        [HostType("Moles")]
        public void RegisterAreas_EventSourceDoesntExist()
        {
            //Arrange
            var areas = new DiagnosticsAreaCollection();
            DiagnosticsArea area = new DiagnosticsArea("test");
            areas.Add(area);

            MEventLog.SourceExistsString = (s) => false;

            string[] sourceNames = new string[2];
            int sourceCnt = 0;
            string []logNames = new string [2];
            MEventLog.CreateEventSourceStringString = (s, l) =>
            {
                if (sourceCnt < 2)
                {
                    sourceNames[sourceCnt] = s;
                    logNames[sourceCnt] = l;
                }

                sourceCnt++;
            };

            // Act
            DiagnosticsAreaEventSource.RegisterAreas(areas);

            // Assert
            Assert.IsTrue(sourceCnt == 2);
            Assert.AreEqual<string>(area.Name, sourceNames[0]);
            Assert.AreEqual<string>(Constants.EventLogName, logNames[0]);
            Assert.AreEqual<string>(DiagnosticsArea.DefaultSPDiagnosticsArea.Name, sourceNames[1]);
            Assert.AreEqual<string>(Constants.EventLogName, logNames[1]);
        }

        [TestMethod]
        [HostType("Moles")]
        public void RegisterAreas_SkipRegisterIfEventSourceDoesExist()
        {
            //Arrange
            var areas = new DiagnosticsAreaCollection();
            DiagnosticsArea area = new DiagnosticsArea("test");
            areas.Add(area);

            MEventLog.SourceExistsString = (s) => true;

            string sourceName = null;
            string logName = null;
            int registerCnt = 0;

            MEventLog.CreateEventSourceStringString = (s, l) =>
            {
                sourceName = s;
                logName = l;
                registerCnt++;
            };

            // Act
            DiagnosticsAreaEventSource.RegisterAreas(areas);

            // Assert
            Assert.IsTrue(registerCnt == 0);
            Assert.IsNull(sourceName);
            Assert.IsNull(logName);
        }

        [TestMethod]
        [HostType("Moles")]
        public void EnsureConfiguredAreasRegistered_RegistersConfiguredAreas()
        {
            //Arrange
            var areas = new DiagnosticsAreaCollection();
            var newLocator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(newLocator);
            newLocator.RegisterTypeMapping<IConfigManager, MockConfigManager>();
            var sourceNames = new List<string>();
            var logNames = new List<string>();

            DiagnosticsArea area = new DiagnosticsArea("test");
            areas.Add(area);

            MEventLog.SourceExistsString = (s) => false;

            MEventLog.CreateEventSourceStringString = (s, l) =>
            {
                sourceNames.Add(s);
                logNames.Add(l);
            };

            // Act
            DiagnosticsAreaEventSource.EnsureConfiguredAreasRegistered();

            // Assert
            Assert.AreEqual(3, sourceNames.Count);
            Assert.AreEqual(MockConfigManager.Area1Name, sourceNames[0] );
            Assert.AreEqual(MockConfigManager.Area2Name, sourceNames[1]);
            Assert.AreEqual<string>(Constants.EventLogName, logNames[0]);
            Assert.AreEqual<string>(Constants.EventLogName, logNames[1]);
            Assert.AreEqual(DiagnosticsArea.DefaultSPDiagnosticsArea.Name, sourceNames[2]);
            Assert.AreEqual<string>(Constants.EventLogName, logNames[2]);
        }
    }
}
