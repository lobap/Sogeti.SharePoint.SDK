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
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;

using Contoso.TrainingManagement.Common.Constants;

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

            UpdateSiteMap(site);

            UpdateTrainingManagementContentType(site);

            UpdateListItemEventReceivers(site);
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

        #region Private Methods

        private static void UpdateSiteMap(SPSite site)
        {
            //The following call is needed to add our custom Registration Approval Application Page to the SiteMap.
            //the following call must be made a local administrator in order to work. If the user is not a local administrator
            //or if UAC is turned on while the feature is activiating, it may cause some problems. It is placed in a 
            //try...catch so that the site creation does not completely fail.
            try
            {
                site.WebApplication.Farm.Services.GetValue<SPWebService>().ApplyApplicationContentToLocalServer();
            }
            catch ( SecurityException )
            {
            }
        }

        private static void UpdateTrainingManagementContentType(SPSite site)
        {
            EnsureFields(site, ContentTypes.TrainingCourse, new Guid(Fields.TrainingCourseInstructor));
        }

        private static void EnsureFields(SPSite site, string contentTypeId, Guid fieldId)
        {
            SPContentTypeId spContentTypeId = new SPContentTypeId(contentTypeId);
            SPContentType contentType = site.RootWeb.ContentTypes[spContentTypeId];

            bool fieldLinkExists = false;
            foreach (SPFieldLink fieldLink in contentType.FieldLinks)
            {
                if (fieldLink.Id == fieldId)
                {
                    fieldLinkExists = true;
                    break;
                }
            }

            if (!fieldLinkExists)
            {
                SPField fieldToAdd = site.RootWeb.Fields[fieldId];
                contentType.FieldLinks.Add(new SPFieldLink(fieldToAdd));
                contentType.Update(true);
            }            
        }

        private static void UpdateListItemEventReceivers(SPSite site)
        {
            //Upgrade All Lists Instances with new Event Receiver Assembly for
            //Lists based on the Training Course Content Type

            string newAssembly = "Contoso.TrainingManagement, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9f4da00116c38ec5"; //our new assembly info
            string newClass = "Contoso.TrainingManagement.TrainingCourseItemEventReceiver";
            SPContentTypeId contentTypeId = new SPContentTypeId(ContentTypes.TrainingCourse); //our custom ctype

            foreach ( SPWeb web in site.AllWebs )
            {
                using ( web )
                {
                    for ( int i = 0; i < web.Lists.Count; i++ )
                    {
                        SPList list = web.Lists[i];
                        SPContentTypeId bestMatch = list.ContentTypes.BestMatch(contentTypeId);
                        if ( bestMatch.IsChildOf(contentTypeId) )
                        {
                            for ( int j = 0; j < list.EventReceivers.Count; j++ )
                            {
                                SPEventReceiverDefinition eventReceiverDefinition = list.EventReceivers[j];
                                if ( String.Compare(eventReceiverDefinition.Assembly, newAssembly, true) != 0 )
                                {
                                    list.EventReceivers.Add(eventReceiverDefinition.Type, newAssembly, newClass);
                                    eventReceiverDefinition.Delete();
                                    list.Update();
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
