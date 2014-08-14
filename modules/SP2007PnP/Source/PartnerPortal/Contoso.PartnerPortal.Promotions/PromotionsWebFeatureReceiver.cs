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
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebPartPages;

using Microsoft.Practices.SPG.Common.ListRepository;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Contoso.PartnerPortal.PartnerDirectory;
using Contoso.PartnerPortal.Promotions.Repositories;

namespace Contoso.PartnerPortal.Promotions
{
    [Guid("2c988455-7e87-4a52-a81d-2756121ff663")]
    public class PromotionsWebFeatureReceiver : SPFeatureReceiver
    {
        #region Private Constants

        private const string DefaultFileName = "default.aspx";
        private const string PromotionPageContentType = "PromotionPage";
        private const string PagesList = "Pages";
        private const string ProductSkuFieldId = "a933192c-4430-4814-97d5-f99b23ace03e";
        private const string PromotionNameFieldId = "351cd1fd-efd2-4db7-ac9d-c669149adce5";
        private const string LinkFilenameFieldId = "5cc6dc79-3710-4374-b433-61cb4a686c12";
        private const string PromotionOnlyView = "PromotionsOnlyView";
        private const string PromotionsTitle = "Promotions";
        private const string TopColumnZone = "TopColumnZone";
        private const string CheckInComment = "Adding ListViewWebPart to default page via PromotionsWebFeatureReceiver.";

        #endregion

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb currentWeb = properties.Feature.Parent as SPWeb;

            // The Promotions feature should only be activated on a Publishing Site.
            if (PublishingWeb.IsPublishingWeb(currentWeb))
            {
                PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(currentWeb);
                SPFile defaultPage = publishingWeb.DefaultPage;
                // If the default page doesn't exist, it needs to be created first.
                if (defaultPage == null)
                {
                    defaultPage = CreateNewWelcomePage(currentWeb, publishingWeb, defaultPage);
                }
                else
                {
                    defaultPage.CheckOut();
                }

                try
                {
                    // Create a custom View that displays only Promotion Pages.
                    SPList pagesList = currentWeb.Lists[PagesList];
                    SPView promotionsOnlyView = CreatePromotionsOnlyView(currentWeb, pagesList);

                    // Add a list view Web part to display Promotions Only
                    AddListViewWebPartToPage(defaultPage, pagesList, promotionsOnlyView);
                }
                finally
                {
                    defaultPage.CheckIn(CheckInComment);
                    defaultPage.Publish(CheckInComment);
                }

                // Save the URL of the site where this feature is being activated. This settings will be used at runtime.
                SetUrlInConfig(currentWeb);

                // Register the Promotion
                RegisterPromotionRepositoryTypeMapping();
            }

        }

        private static void RegisterPromotionRepositoryTypeMapping()
        {
            ServiceLocatorConfig typeMappings = new ServiceLocatorConfig();
            typeMappings.RegisterTypeMapping<IPartnerPromotionRepository, PartnerPromotionRepository>();
        }

        private static void SetUrlInConfig(SPWeb currentWeb)
        {
            // The PartnerPromotionRepository needs the URL of the sites that stores the PartnerPromotions. 
            // Store the URL of the Web that this feature is activated on as the parner promotion URL. 
            IConfigManager hierarchicalConfig = SharePointServiceLocator.Current.GetInstance<IConfigManager>();
            hierarchicalConfig.SetInPropertyBag(PartnerPromotionRepository.PartnerPromotionRepositoryUrlConfigKey, currentWeb.Url, SPFarm.Local);
        }

        private static void AddListViewWebPartToPage(SPFile defaultPage, SPList pagesList, SPView promotionsOnlyView)
        {
            //Add ListViewWebPart instance to default page configured with above view
            SPLimitedWebPartManager webPartManager = defaultPage.GetLimitedWebPartManager(PersonalizationScope.Shared);
            ListViewWebPart listViewWebPart = new ListViewWebPart();
            listViewWebPart.Title = PromotionsTitle;
            listViewWebPart.ViewType = ViewType.None;
            listViewWebPart.ViewGuid = promotionsOnlyView.ID.ToString("B").ToUpper(CultureInfo.CurrentCulture);
            listViewWebPart.ListName = pagesList.ID.ToString("B").ToUpper(CultureInfo.CurrentCulture);

            webPartManager.AddWebPart(listViewWebPart, TopColumnZone, 0);
            webPartManager.Web.Dispose();
        }

        private static SPView CreatePromotionsOnlyView(SPWeb currentWeb, SPList pagesList)
        {
            //Create View that only shows PromotionPage content type instances
            CAMLQueryBuilder queryBuilder = new CAMLQueryBuilder();
            queryBuilder.FilterByContentType(PromotionPageContentType);
            string queryBuilderBuildQuery = queryBuilder.Build().Query;

            //Add fields to list
            
            SPField productSkuField = currentWeb.Site.RootWeb.Fields[new Guid(ProductSkuFieldId)];
            SPField promotionNameField = currentWeb.Site.RootWeb.Fields[new Guid(PromotionNameFieldId)];
            if (!pagesList.Fields.ContainsField(productSkuField.InternalName))
            {
                pagesList.Fields.Add(productSkuField);
            }
            if (!pagesList.Fields.ContainsField(promotionNameField.InternalName))
            {
                pagesList.Fields.Add(promotionNameField);
            }

            //Add fields to view using FieldId GUIDs
            StringCollection viewFields = new StringCollection();
            viewFields.Add(currentWeb.Site.RootWeb.Fields[new Guid(LinkFilenameFieldId)].InternalName);//LinkFilename)
            viewFields.Add(productSkuField.InternalName);//ProductSkuField
            viewFields.Add(promotionNameField.InternalName);//PromotionNameField

            SPView promotionsOnlyView = pagesList.Views.Add(PromotionOnlyView, viewFields, queryBuilderBuildQuery, 100, true, false);
            promotionsOnlyView.Update();

            return promotionsOnlyView;
        }

        private static SPFile CreateNewWelcomePage(SPWeb currentWeb, PublishingWeb publishingWeb, SPFile defaultPage)
        {
            PageLayout welcomeLayout = publishingWeb.GetAvailablePageLayouts(ContentTypeId.WelcomePage)[0];
            PublishingPage welcomePage = publishingWeb.GetPublishingPages().Add(DefaultFileName, welcomeLayout);
            defaultPage = currentWeb.GetFile(welcomePage.Url);
            publishingWeb.DefaultPage = defaultPage;
            publishingWeb.Update();
            //defaultPage.MoveTo("default.aspx", true);
            return defaultPage;
        }

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
    }
}
