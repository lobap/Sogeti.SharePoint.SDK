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
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.SharePoint.Administration.Moles;
using System.Collections.Specialized;
using System.Collections;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Tests.Behaviors;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SharePoint.Common.Logging;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public partial class SPFarmPropertyBagTests
    {
        [TestInitialize]
        public void Init()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(
                new ActivatingServiceLocator()
                .RegisterTypeMapping<ILogger, MockLogger>(InstantiationType.AsSingleton));
        }

        [TestCleanup]
        public void Cleanup()
        {
            SharePointServiceLocator.Reset();
        }

        [TestMethod]
        [HostType("Moles")]
        public void ctor_WithNoSharePointContext()
        {
            //Arrange
            MSPFarm.LocalGet = () => null;
            bool expectedExceptionThrown = false;

            //Act
            try
            {
                var target = new SPFarmPropertyBag();
            }
            catch (NoSharePointContextException)
            {
                expectedExceptionThrown = true;
            }
            Assert.IsTrue(expectedExceptionThrown);
        }

        [TestMethod]
        [HostType("Moles")]
        public void AddAndContains()
        {
            SPFarmPropertyBag.ClearCache();
            var f = new BSPConfiguredFarm();
            var fssPO = new MSPPersistedObject(f.SettingStore);
            fssPO.Update = () =>
            {
            };

            string key = "key";
            string value = "value";
            var target = new SPFarmPropertyBag();
            IPropertyBagTest.AddContains(target, key, value);
        }


        [TestMethod]
        [HostType("Moles")]
        public void AddAndRemove()
        {
            // Arrange
            SPFarmPropertyBag.ClearCache();
            var f = new BSPConfiguredFarm();
            var fssPO = new MSPPersistedObject(f.SettingStore);
            fssPO.Update = () =>
            {
            };

            string key = "foo";

            //  create a farm with property 'key'
            f.SettingStore.Settings[key] = "fred";
            
            // Act
            var target = new SPFarmPropertyBag();
            var containsBeforeCondition = target.Contains(key);
            target.Remove(key);
            var result = target.Contains(key);

            // Assert
            Assert.IsFalse(result);
            Assert.IsTrue(containsBeforeCondition);
        }
    }
}
