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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Server;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SPG.Common.Configuration;
using TypeMock.ArrangeActAssert;

namespace Contoso.PartnerPortal.Portal.IntegrationTests
{
    [TestClass]
    public class ConfigManagerFixture
    {
        [TestCleanup]
        [TestInitialize]
        public void Cleanup()
        {
            SPSite spSite = new SPSite("http://localhost:9001/sites/pssportal");
            SPContext spContext = SPContext.GetContext(spSite.RootWeb);
            Isolate.WhenCalled(() => SPContext.Current.Site).WillReturn(spSite);
            Isolate.WhenCalled(() => SPContext.Current).WillReturn(spContext);
            
            List<string> keys = new List<string>() { "IntegrationTest-FarmLevelKey", "IntegrationTest-WebAppLevelKey", "IntegrationTest-SiteLevelKey", "IntegrationTest-WebLevelKey" };

            HierarchicalConfig hierarchicalConfig = new HierarchicalConfig();
            foreach (string key in keys)
            {
                hierarchicalConfig.RemoveKeyFromPropertyBag(key, SPContext.Current.Web);
                hierarchicalConfig.RemoveKeyFromPropertyBag(key, SPContext.Current.Site);
                hierarchicalConfig.RemoveKeyFromPropertyBag(key, SPContext.Current.Site.WebApplication);
                hierarchicalConfig.RemoveKeyFromPropertyBag(key, SPFarm.Local);
            }
         
            Isolate.CleanUp();
        }



        [TestMethod]
        public void CanSetAndGetValues()
        {
            SPSite spSite = new SPSite("http://localhost:9001/sites/pssportal");
            SPContext spContext = SPContext.GetContext(spSite.RootWeb);
            Isolate.WhenCalled(() => SPContext.Current.Site).WillReturn(spSite);
            Isolate.WhenCalled(() => SPContext.Current).WillReturn(spContext);

            //Set Values at levels of hierarchy            
            HierarchicalConfig target1 = new HierarchicalConfig();
            target1.SetInPropertyBag("IntegrationTest-FarmLevelKey", "FarmLevelValue", new SPFarmPropertyBag());
            target1.SetInPropertyBag("IntegrationTest-WebAppLevelKey", "WebAppLevelValue", new SPWebAppPropertyBag());
            target1.SetInPropertyBag("IntegrationTest-SiteLevelKey", "SiteLevelValue", new SPSitePropertyBag());
            target1.SetInPropertyBag("IntegrationTest-WebLevelKey", "WebLevelValue", new SPWebPropertyBag());

            HierarchicalConfig target2 = new HierarchicalConfig();
            Assert.AreEqual("FarmLevelValue", target2.GetByKey<string>("IntegrationTest-FarmLevelKey"));
            Assert.AreEqual("WebAppLevelValue", target2.GetByKey<string>("IntegrationTest-WebAppLevelKey"));
            Assert.AreEqual("SiteLevelValue", target2.GetByKey<string>("IntegrationTest-SiteLevelKey"));
            Assert.AreEqual("WebLevelValue", target2.GetByKey<string>("IntegrationTest-WebLevelKey"));

            Assert.AreEqual("FarmLevelValue", SPFarm.Local.Properties["IntegrationTest-FarmLevelKey"]);
            Assert.AreEqual("WebAppLevelValue", SPContext.Current.Site.WebApplication.Properties["IntegrationTest-WebAppLevelKey"]);
            Assert.AreEqual("SiteLevelValue", SPContext.Current.Site.RootWeb.AllProperties["IntegrationTest-SiteLevelKey"]);
            Assert.AreEqual("WebLevelValue", SPContext.Current.Web.AllProperties["IntegrationTest-WebLevelKey"]);
        }

        [TestMethod]
        public void CanSetValueAndOverrideAtLowerLevel()
        {
            SPSite spSite = new SPSite("http://localhost:9001/sites/pssportal");
            SPContext spContext = SPContext.GetContext(spSite.RootWeb);
            Isolate.WhenCalled(() => SPContext.Current.Site).WillReturn(spSite);
            Isolate.WhenCalled(() => SPContext.Current).WillReturn(spContext);

            //Set Values at levels of hierarchy            
            HierarchicalConfig target = new HierarchicalConfig();
            target.SetInPropertyBag("IntegrationTest-FarmLevelKey", "FarmLevelValue", new SPFarmPropertyBag());
            Assert.AreEqual("FarmLevelValue", target.GetByKey<string>("IntegrationTest-FarmLevelKey"));

            target.SetInPropertyBag("IntegrationTest-FarmLevelKey", "Over-riddenValue1", new SPWebAppPropertyBag());
            Assert.AreEqual("Over-riddenValue1", target.GetByKey<string>("IntegrationTest-FarmLevelKey"));

            target.SetInPropertyBag("IntegrationTest-FarmLevelKey", "Over-riddenValue2", new SPWebAppPropertyBag());
            Assert.AreEqual("Over-riddenValue2", target.GetByKey<string>("IntegrationTest-FarmLevelKey"));

            target.SetInPropertyBag("IntegrationTest-FarmLevelKey", "Over-riddenValue3", new SPSitePropertyBag());
            Assert.AreEqual("Over-riddenValue3", target.GetByKey<string>("IntegrationTest-FarmLevelKey"));

            target.SetInPropertyBag("IntegrationTest-FarmLevelKey", "Over-riddenValue4", new SPWebPropertyBag());
            Assert.AreEqual("Over-riddenValue4", target.GetByKey<string>("IntegrationTest-FarmLevelKey"));

        }

    }
}
