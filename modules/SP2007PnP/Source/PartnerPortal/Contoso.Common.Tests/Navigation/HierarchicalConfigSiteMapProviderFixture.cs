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
using System.Web;
using Contoso.Common.Navigation;
using Contoso.Common.Tests.ExceptionHandling;
using Contoso.Common.Tests.Properties;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.SPG.Common.ServiceLocation;

namespace Contoso.Common.Tests.Navigation
{
    [TestClass]
    public class HierarchicalConfigSiteMapProviderFixture
    {
        [TestMethod]
        public void CanTranslateXmlIntoSiteMapNodes()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>().RegisterTypeMapping<ILogger, MockLogger>());

            MockConfigManager.GetByKeyReturnValue = Resources.SiteMapXml;

            HierarchicalConfigSiteMapProvider target = new HierarchicalConfigSiteMapProvider();

            SiteMapNode rootNode = target.BuildSiteMap();
            Assert.AreEqual("/pssportalroot", rootNode.Url);
            Assert.AreEqual(4, rootNode.ChildNodes.Count);
            SiteMapNode firstChild = rootNode.ChildNodes[0];
            Assert.AreEqual(2, firstChild.ChildNodes.Count);

        }

        [TestMethod]
        public void RootNodeEqualsBuildSiteMap()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<IHierarchicalConfig, MockConfigManager>().RegisterTypeMapping<ILogger, MockLogger>());

            MockConfigManager.GetByKeyReturnValue = Resources.SiteMapXml;

            HierarchicalConfigSiteMapProvider target = new HierarchicalConfigSiteMapProvider();

            SiteMapNode rootNode = target.RootNode;
            Assert.AreEqual("/pssportalroot", rootNode.Url);
            Assert.AreEqual(4, rootNode.ChildNodes.Count);
            SiteMapNode firstChild = rootNode.ChildNodes[0]; //Partner Home
            Assert.AreEqual(2, firstChild.ChildNodes.Count); 
        }

    }

    class MockConfigManager : IHierarchicalConfig
    {

        public static string GetByKeyReturnValue { get; set; }

        public bool ContainsKeyInPropertyBag(string key, SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetByKey<TValue>(string key)
        {
            return (TValue) (object) GetByKeyReturnValue;
        }

        public TValue GetByKey<TValue>(string key, ConfigLevel level)
        {
            return (TValue)(object)GetByKeyReturnValue;
        }


        public void Remove(string key, IPropertyBag propertyBag)
        {
            throw new NotImplementedException();
        }

        public void RemoveKeyFromPropertyBag(string key, SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public void RemoveKeyFromPropertyBag(string key, SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, SPFarm propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyInPropertyBag(string key, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public void RemoveKeyFromPropertyBag(string key, SPFarm propertyBag)
        {
            throw new NotImplementedException();
        }

        public void RemoveKeyFromPropertyBag(string key, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetByKey<TValue>(string key, IPropertyBag propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetByKey<TValue>(string key, SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetByKey<TValue>(string key, SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetByKey<TValue>(string key, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetByKey<TValue>(string key, SPFarm propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetByKey(string key, object value, IPropertyBag propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public void SetInPropertyBag(string key, object value, SPFarm propertyBag)
        {
            throw new NotImplementedException();
        }

        public IPropertyBag GetDefaultPropertyBag()
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key, ConfigLevel level)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPWeb propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPSite propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPWebApplication propertyBag)
        {
            throw new NotImplementedException();
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPFarm propertyBag)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key, IPropertyBag propertyBag)
        {
            throw new NotImplementedException();
        }
    }
}