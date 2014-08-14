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
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;

namespace Contoso.TrainingManagement
{
    /// <summary>
    /// The SiteFeatureReceiver class contains methods to perform during the
    /// life of the Contoso Training Management's site collection scoped
    /// feature.
    /// </summary>
    [Guid("0cb4445d-ab1a-4053-9301-50c3b2666f31")]
    public class SiteFeatureReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// The FeatureActivated method executes when the Contoso Training Management's
        /// site collection scoped feature is activated. It will sync up site map for
        /// the web application to show the registrationapproval.aspx page in the bread crumbs.
        /// </summary>
        /// <param name="properties"></param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            #region Update SiteMap

            //The following call is needed to add our custom Registration Approval Application Page to the SiteMap.
            //the following call must be made a local administrator in order to work. If the user is not a local administrator
            //or if UAC is turned on while the feature is activiating, it may cause some problems. It is placed in a 
            //try...catch so that the site creation does not completely fail.
            try
            {
                SPSite site = null;
                object parent = properties.Feature.Parent;

                if (parent is SPWeb)
                {
                    site = ((SPWeb)parent).Site;
                }
                else
                {
                    site = (SPSite)parent;
                }

                site.WebApplication.Farm.Services.GetValue<SPWebService>().ApplyApplicationContentToLocalServer();
            }
            catch (SecurityException) { }

            #endregion
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
