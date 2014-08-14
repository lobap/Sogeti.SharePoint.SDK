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
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls.WebParts;
using Contoso.Common;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.Security;


namespace Contoso.PartnerPortal.ProductCatalog
{
    [Guid("68a85277-0049-49ed-8d52-fd4dbbd52f1b")]
    public class WebFeatureReceiver : SPFeatureReceiver
    {
        private SPWebConfigModification sPWebConfigModification;

        public WebFeatureReceiver()
        {
            sPWebConfigModification = new SPWebConfigModification("add[@name=\"CategorySiteMapProvider\"]",
                                                                  "configuration/system.web/siteMap/providers");
            sPWebConfigModification.Owner = GetType().Assembly.GetName().ToString();
            sPWebConfigModification.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            sPWebConfigModification.Value = @"<add name=""CategorySiteMapProvider"" description=""Provider for category navigation using Business Data Catalog"" type=""Contoso.PartnerPortal.ProductCatalog.Navigation.BusinessDataCatalogSiteMapProvider, Contoso.PartnerPortal.ProductCatalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0dcd9137292eac97"" />";
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                SPFile file = web.GetFile("Category.aspx");
                if (file.Exists)
                {
                    SPLimitedWebPartManager manager = file.GetLimitedWebPartManager(PersonalizationScope.Shared);

                    System.Web.UI.WebControls.WebParts.WebPart bdcItemBuilder = null;
                    System.Web.UI.WebControls.WebParts.WebPart categoryDetails = null;
                    System.Web.UI.WebControls.WebParts.WebPart childCategoriesWebPart = null;
                    System.Web.UI.WebControls.WebParts.WebPart productsWebPart = null;

                    foreach (System.Web.UI.WebControls.WebParts.WebPart webPart in manager.WebParts)
                    {
                        switch (webPart.Title)
                        {
                            case "Business Data Item Builder":
                                bdcItemBuilder = webPart;
                                break;
                            case "Category Details":
                                categoryDetails = webPart;
                                break;
                            case "Category List":
                                childCategoriesWebPart = webPart;
                                break;
                            case "Product List":
                                productsWebPart = webPart;
                                break;
                        }
                    }

                    ProviderConnectionPoint providerConnection = manager.GetProviderConnectionPoints(bdcItemBuilder)[0];
                    ConsumerConnectionPoint consumerConnection = manager.GetConsumerConnectionPoints(childCategoriesWebPart)[0];
                    SPWebPartConnection connection = manager.SPConnectWebParts(bdcItemBuilder, providerConnection, childCategoriesWebPart, consumerConnection);
                    manager.SPWebPartConnections.Add(connection);

                    consumerConnection = manager.GetConsumerConnectionPoints(productsWebPart)[0];
                    connection = manager.SPConnectWebParts(bdcItemBuilder, providerConnection, productsWebPart, consumerConnection);
                    manager.SPWebPartConnections.Add(connection);

                    consumerConnection = manager.GetConsumerConnectionPoints(categoryDetails)[0];
                    connection = manager.SPConnectWebParts(bdcItemBuilder, providerConnection, categoryDetails, consumerConnection);
                    manager.SPWebPartConnections.Add(connection);

                    manager.Web.Dispose();
                }

                web.Site.WebApplication.WebConfigModifications.Add(sPWebConfigModification);

                //Call Update and ApplyWebConfigModifications to save changes
                web.Site.WebApplication.Update();
                web.Site.WebApplication.WebService.ApplyWebConfigModifications();
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            web.Site.WebApplication.WebConfigModifications.Remove(sPWebConfigModification);

            //Call Update and ApplyWebConfigModifications to save changes
            web.Site.WebApplication.Update();
            web.Site.WebApplication.WebService.ApplyWebConfigModifications();
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
