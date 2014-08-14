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
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.SharePoint.Moles;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SharePoint.Common.Tests.Behaviors;
using Microsoft.Practices.SharePoint.Common.Logging.Moles;
using Microsoft.Practices.SharePoint.Common.Logging;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public class SPWebAppPropertyBagTests
    {
        [TestInitialize]
        public void Initialize()
        {
            var logger = new MockLogger();
            var locator = new ActivatingServiceLocator();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(locator);
            locator.RegisterTypeMapping<ILogger, MockLogger>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            SharePointServiceLocator.Reset();
        }

        [TestMethod]
        [HostType("Moles")]
        public void AddAndContains()
        {
            //Arrange
            SPWebAppPropertyBag.ClearCache();
            MSPPersistedObject webPO;
            WebAppSettingStore wss; 
            var webApp = new BSPConfiguredWebApp();
            wss = new WebAppSettingStore();
            webPO = new MSPPersistedObject((SPPersistedObject)webApp.Instance);
            webPO.GetChildString<WebAppSettingStore>(
                (s) => 
                    {
                        return wss;
                    });
            var wssPO = new MSPPersistedObject(wss);
            wssPO.Update = () => 
            { 
            };


            string key = "key";
            string value = "value";
            var target = new SPWebAppPropertyBag(webApp);
            IPropertyBagTest.AddContains(target, key, value);
        }


        [TestMethod]
        [HostType("Moles")]
        public void AddAndRemove()
        {
            //Arrange
            SPWebAppPropertyBag.ClearCache();
            MSPPersistedObject webPO;
            WebAppSettingStore wss;
            string key = "flintstone";
    
            var webApp = new BSPConfiguredWebApp();
            wss = new WebAppSettingStore();
            wss.Settings[key] = "fred";
            webPO = new MSPPersistedObject((SPPersistedObject)webApp.Instance);
            webPO.GetChildString<WebAppSettingStore>((s) => wss);
            var wssPO = new MSPPersistedObject(wss);
            wssPO.Update = () =>
            {
            };

            // Act
            var target = new SPWebAppPropertyBag(webApp);
            var containsBeforeCondition = target.Contains(key);
            target.Remove(key);
            var result = target.Contains(key);

            // Assert
            Assert.IsFalse(result);
            Assert.IsTrue(containsBeforeCondition);
        }
    }
}
