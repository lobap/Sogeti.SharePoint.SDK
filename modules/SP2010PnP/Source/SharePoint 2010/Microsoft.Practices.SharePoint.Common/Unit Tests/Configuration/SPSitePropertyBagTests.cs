//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Administration.Behaviors;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint.Moles;
using Microsoft.SharePoint.Behaviors;
using Microsoft.SharePoint.Administration.Moles;
using Microsoft.SharePoint.Utilities.Moles;
using System.Collections.Specialized;
using Microsoft.SharePoint.Behaviors.Utilities;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    [TestClass]
    public class SPSitePropertyBagTests
    {
        [TestMethod]
        [HostType("Moles")]
        public void AddAndContains()
        {
            MSPSite site = SetupSite();
            string key = "key";
            string value = "value";

            var target = new SPSitePropertyBag(site);
            IPropertyBagTest.AddContains(target, key, value);
        }


        [TestMethod]
        [HostType("Moles")]
        public void AddAndRemove()
        {
            // Arrange
            MSPSite site = SetupSite();
             string key = "foo";
            string value =  "fred";
            var target = new SPSitePropertyBag(site); 
            target[key] = value;

            // Act
            var containsBeforeCondition = target.Contains(key);
            target.Remove(key);
            var result = target.Contains(key);

            // Assert
            Assert.IsFalse(result);
            Assert.IsTrue(containsBeforeCondition);
        }


        [TestMethod]
        [HostType("Moles")]
        public void ctor_WithSite_CanAdd()
        {
            // Arrange
            MSPSite site = SetupSite();

            string key = "foo";
            string value = "fred";
            var target = new SPSitePropertyBag(site);
            target[key] = value;

            // Act
            var containsBeforeCondition = target.Contains(key);
            target.Remove(key);
            var result = target.Contains(key);

            // Assert
            Assert.IsFalse(result);
            Assert.IsTrue(containsBeforeCondition);
        }

        [TestMethod]
        [HostType("Moles")]
        public void SetKey_SuffixIsAdded()
        {
            // Arrange
            SPSite site = SetupSite();
            string key = "foo";
            string value = "fred";
            var target = new SPSitePropertyBag(site);
            target[key] = value;

            // Act
            target[key] = value;
            string fullKey = key + SPSitePropertyBag.KeySuffix;
            bool containsKey = site.RootWeb.AllProperties.ContainsKey(fullKey);
            bool doesNotContainKey = site.RootWeb.AllProperties.ContainsKey(key); 

            // Assert
            Assert.IsTrue(containsKey);
            Assert.IsFalse(doesNotContainKey);
        }


        private MSPSite SetupSite()
        {
            var bag = new System.Collections.Hashtable();

            MSPWeb web = new MSPWeb()
            {
                AllPropertiesGet = () => bag,
                SetPropertyObjectObject = (key, value) => bag[key] = value,
                GetPropertyObject = (key) => bag[key],
                DeletePropertyObject = (key) => bag.Remove(key),
                AddPropertyObjectObject = (key, value) => bag.Add(key, value),
                Update = () => { }
            };

            MSPSite site = new MSPSite()
            {
                RootWebGet = () => web,
            };

            return site;
        }
    }
}
