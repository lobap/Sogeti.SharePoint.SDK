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
using System.Security.Permissions;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Contoso.PartnerPortal.ContextualHelp
{
    [Guid("92a99385-aefc-4718-98c2-1ac541c55a6e")]
    public class ContextualHelpFeatureReceiver : SPFeatureReceiver
    {
        #region Private Constants

        private const string ContextualHelpMasterPageUrl = "_catalogs/masterpage/contextualHelp.master";

        #endregion

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel=true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            if (web.Site.RootWeb.ServerRelativeUrl.EndsWith("/", StringComparison.CurrentCultureIgnoreCase))
            {
                web.MasterUrl = web.Site.RootWeb.ServerRelativeUrl + ContextualHelpMasterPageUrl;
                web.CustomMasterUrl = web.Site.RootWeb.ServerRelativeUrl + ContextualHelpMasterPageUrl;
            }
            else
            {
                web.MasterUrl = web.Site.RootWeb.ServerRelativeUrl + "/" + ContextualHelpMasterPageUrl;
                web.CustomMasterUrl = web.Site.RootWeb.ServerRelativeUrl + "/" + ContextualHelpMasterPageUrl;
            }

            web.Update();
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
