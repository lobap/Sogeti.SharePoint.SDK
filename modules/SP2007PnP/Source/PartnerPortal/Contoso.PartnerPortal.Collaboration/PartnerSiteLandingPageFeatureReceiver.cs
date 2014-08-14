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
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebPartPages;

using Contoso.PartnerPortal.Collaboration.Properties;
using Contoso.PartnerPortal.Promotions.WebParts.PartnerPromotions;

namespace Contoso.PartnerPortal.Collaboration
{
    [Guid("0d4249b5-6c67-466a-8c5a-a49cb7aff73e")]
    [CLSCompliant(false)]
    public class PartnerSiteLandingPageFeatureReceiver :SPFeatureReceiver
    {
        #region Private constants and readonly fields

        private const string welcomePageLayoutContentTypeId = "0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF390064DEA0F50FC8C147B0B6EA0636C4A7D4007c0f498f3f5141118c22eb5fec11ffd3";
        private const string PartnerLandingPageUrl = "Pages/default.aspx";
        private const string PartnerLandingPageFileName = "default.aspx";
        private const string PartnerLandingPageTile = "Welcome";
        private const string DescriptionField = "Description";
        private readonly Guid titleFieldId = new Guid("fa564e0f-0c70-4ab9-b863-0177e6ddd247");
        private readonly Guid pageContentFieldId = new Guid("f55c4d88-1f2e-4ad9-aaa8-819af4ee7ee8");
        private readonly Guid partnerSpecificContentFieldId = new Guid("c27d0096-3238-4579-a18b-fc964ab823de");

        #endregion
    
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel=true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                if (PublishingWeb.IsPublishingWeb(web))
                {
                    PublishingWeb pubWeb = PublishingWeb.GetPublishingWeb(web);
                    PublishingPage page = pubWeb.GetPublishingPages()[PartnerLandingPageUrl];

                    if (page == null)
                    {
                        SPContentTypeId contentTypeId = new SPContentTypeId(welcomePageLayoutContentTypeId);
                        PageLayout[] layouts = pubWeb.GetAvailablePageLayouts(contentTypeId);
                        PageLayout welcomePageLayout = layouts[0];

                        page = pubWeb.GetPublishingPages().Add(PartnerLandingPageFileName, welcomePageLayout);
                    }
                    else
                    {
                        page.CheckOut();
                    }
                                                
                    page.ListItem[titleFieldId] = PartnerLandingPageTile;
                    page.ListItem[DescriptionField] = Resources.PageDescription;
                        page.ListItem[pageContentFieldId] = Resources.PageContent;
                    page.ListItem[partnerSpecificContentFieldId] = Resources.PartnerSpecificContent;
                    page.Update();

                    SPFile welcomeFile = web.GetFile(page.Url);
                    pubWeb.DefaultPage = welcomeFile;
                    pubWeb.Update();

                    page.CheckIn(Resources.CheckInValue);                    
                }
            }
        }
        
        #region Unused

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }

        #endregion
    }
}
