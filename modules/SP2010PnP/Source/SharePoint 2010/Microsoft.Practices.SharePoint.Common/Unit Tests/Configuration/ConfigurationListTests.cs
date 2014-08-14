//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    using Common.Configuration;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Behaviors;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConfigurationListTests
    {
        [TestMethod]
        [HostType("Moles")]
        public void EnsureConfigurationList_WithExistingListNew()
        {
            // Arrange
            BSPContentType.Prepare();
            BSPFieldLink.Prepare();

            var listUrl = string.Format("/Lists/{0}", ConfigurationList.ConfigListName);
            var site = new BSPSite();
            var rootWeb = site.SetRootWeb();
            rootWeb.Fields.SetAll(new SPField[] {
                new BSPField { Id = ConfigurationList.SettingKeyFieldId },
                new BSPField { Id = ConfigurationList.SettingValueFieldId },
                new BSPField { Id = ConfigurationList.SettingWebIdFieldId },
            });
            rootWeb.ServerRelativeUrl = "/";
            rootWeb.ContentTypes.SetEmpty();
            rootWeb.ContentTypes.ReadOnly = false;

            var list = rootWeb.Lists.SetOne();
            list.Title = ConfigurationList.ConfigListName;
            list.Fields.SetOne(new BSPField { Id = ConfigurationList.SettingWebIdFieldId });
            list.Hidden = true;
            list.FieldIndexes.SetEmpty();
            list.Url = listUrl;
            list.ContentTypes.SetEmpty();

            // Act and Assert
            EnsureConfigurationListForAnySite(site);
        }

        public static void EnsureConfigurationListForAnySite(SPSite site)
        {
            // Act
            ConfigurationList.EnsureConfigurationList(site);

            // Assert
            var rootWeb = site.RootWeb;
            var list = rootWeb.Lists[ConfigurationList.ConfigListName];
            Assert.IsNotNull(list);
        }

    }
}
