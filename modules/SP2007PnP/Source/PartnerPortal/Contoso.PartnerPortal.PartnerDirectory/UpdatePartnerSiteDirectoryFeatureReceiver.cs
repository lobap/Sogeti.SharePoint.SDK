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
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.WebControls;
using Microsoft.SharePoint.WebPartPages;

using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ListRepository;
using Microsoft.Practices.SPG.Common.ServiceLocation;

using Contoso.PartnerPortal.PartnerDirectory.Properties;

namespace Contoso.PartnerPortal.PartnerDirectory
{
    [Guid("e541660c-c063-4c97-8706-dc486914dc27")]
    public class UpdatePartnerSiteDirectoryFeatureReceiver : SPFeatureReceiver
    {
        private readonly Guid titleFieldId = new Guid(Constants.TitleFieldId);

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            // The Site Directory used for the Partner Directory requires a new field to store Partner Id.
            // The field is provisioned in the Site Collection scoped feature, but should be added to the exisitng
            // Sites list here.
            SPList sitesList = AddPartnerFieldToSiteDirectory(web);

            // The Site Directory makes available a set of Tabbed pages for things like Top Sites, Site Map and Categories
            // We will add a new Tabbed page to easily see only sites for Partners.
            CreatePartnerTabbedPage(web, sitesList);

            // Also register this site as the Partner Site Directory in config settings
            SetPartnerSiteDirectoryInConfig(web);

            // Register the type mapping for the Partner Site Directory Repository
            ServiceLocatorConfig typeMappings = new ServiceLocatorConfig();
            typeMappings.RegisterTypeMapping<IPartnerSiteDirectory, PartnerSiteDirectory>();
        }

        private void CreatePartnerTabbedPage(SPWeb web, SPList sitesList)
        {
            // Create a new page for listing Partner Site Collections
            if (PublishingWeb.IsPublishingWeb(web))
            {
                PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                PublishingPageCollection availablePages = pubWeb.GetPublishingPages();
                PublishingPage partnerSitesPage = availablePages[Constants.PartnerSitesPageUrl];

                if (partnerSitesPage == null)
                {
                    // The Partner Sites page should be based on the same layout as the top sites page.
                    PublishingPage topSitesPage = availablePages[Constants.TopSitesPageUrl];
                    partnerSitesPage = availablePages.Add(Constants.PartnerSitesFileName, topSitesPage.Layout);
                    partnerSitesPage.ListItem[titleFieldId] = Constants.PartnerSitesTile;
                    // We can not use the Field Id of the the Description field, because it is not static.
                    partnerSitesPage.ListItem[Constants.DescriptionField] = Resources.PartnerSitePageDescription;
                    partnerSitesPage.Update();
                    SPFile partnerSitesPageFile = web.GetFile(partnerSitesPage.Url);

                    AddListViewWebPartToPage(sitesList, partnerSitesPage, partnerSitesPageFile);

                    // Add a tab to the Tabs control.
                    SPListItem newTab = web.Lists[Constants.TabsList].Items.Add();
                    newTab[Constants.TabNameField] = Constants.PartnersTabName;
                    newTab[Constants.PageField] = Constants.PartnerSitesFileName;
                    newTab[Constants.TooltipField] = Constants.PartnerSitesTooltip;
                    newTab.Update();
                }
            }
        }

        private static void AddListViewWebPartToPage(SPList sitesList, PublishingPage partnerSitesPage, SPFile partnerSitesPageFile)
        {
            ListViewWebPart listViewWebPart = new ListViewWebPart();
            listViewWebPart.ViewType = ViewType.None;
            listViewWebPart.ListName = sitesList.ID.ToString("B").ToUpper(CultureInfo.CurrentCulture);
            listViewWebPart.ViewGuid = sitesList.Views[Constants.PartnerSitesView].ID.ToString("B").ToUpper(CultureInfo.CurrentCulture);
            listViewWebPart.Title = Constants.PartnerSitesTile;
            listViewWebPart.ChromeType = System.Web.UI.WebControls.WebParts.PartChromeType.None;

            SPLimitedWebPartManager webPartManager =
                partnerSitesPageFile.GetLimitedWebPartManager(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);
            webPartManager.AddWebPart(listViewWebPart, Constants.LeftColumnZone, 1);
            webPartManager.Web.Dispose();
            partnerSitesPage.CheckIn(Constants.CheckInComment);
        }

        private static SPList AddPartnerFieldToSiteDirectory(SPWeb web)
        {
            SPList sitesList = web.Lists[Constants.SitesList];
            if (sitesList != null)
            {
                try
                {
                    sitesList.Fields.Add(web.Site.RootWeb.Fields[FieldIds.PartnerFieldId]);

                    CreatePartnersCustomView(sitesList);
                }
                catch (SPException sharePointException)
                {
                    ILogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
                    logger.LogToOperations(sharePointException);
                }
            }
            return sitesList;
        }

        private static void CreatePartnersCustomView(SPList sitesList)
        {
            StringCollection viewFields = new StringCollection();
            viewFields.Add(Constants.TitlewURL);
            viewFields.Add(Constants.PartnerDirectoryPartnerField);

            string query = Constants.QueryForPartnerSites;

            sitesList.Views.Add(Constants.PartnerSitesView, viewFields, query, 100, true, false, SPViewCollection.SPViewType.Html, false);
        }

        private static void SetPartnerSiteDirectoryInConfig(SPWeb web)
        {
            IConfigManager hierarchicalConfig = SharePointServiceLocator.Current.GetInstance<IConfigManager>();
            hierarchicalConfig.SetInPropertyBag(Constants.PartnerSiteDirectoryUrlConfigKey
                , web.Site.MakeFullUrl(Constants.PartnerDirectoryRelativeUrl), SPFarm.Local);
        }

        #region Unused

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }

        #endregion
    }
}
