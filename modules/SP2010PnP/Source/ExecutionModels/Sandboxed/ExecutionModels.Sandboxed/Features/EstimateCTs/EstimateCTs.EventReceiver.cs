//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Diagnostics;
using Microsoft.SharePoint.Administration;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;
using System.Xml;

namespace ExecutionModels.Sandboxed.Features.EstimateCTs
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("923cec3e-3013-4705-96b6-6662ccd7be4d")]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class EstimateCTsEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            Guid[] requiredFieldsToAdd = new Guid[] {   Constants.estimateStatusFieldId, 
                                                        Constants.estimateValueFieldId, 
                                                        Constants.projectLookupFieldId
                                                    };
            Guid[] optionalFieldsToAdd = new Guid[]
                                        {
                                            Constants.VendorIdFieldId, 
                                            Constants.VendorNameFieldId 
                                        };

            using (SPWeb web = site.RootWeb)
            {
                SPContentType sowContentType = web.ContentTypes[Constants.sowContentTypeId];
                if (sowContentType == null)
                {
                    sowContentType = new SPContentType(Constants.sowContentTypeId, web.ContentTypes, Constants.sowContentTypeName);
                    web.ContentTypes.Add(sowContentType);
                }

                sowContentType.DocumentTemplate = string.Concat(web.Url, Constants.sowTemplatePath);
                AddFieldsToContentType(web, sowContentType, requiredFieldsToAdd, true);
                AddFieldsToContentType(web, sowContentType, optionalFieldsToAdd, false);
                sowContentType.Update(true);

                SPContentType estimateContentType = web.ContentTypes[Constants.estimateContentTypeId];
                if (estimateContentType == null)
                {
                    estimateContentType = new SPContentType(Constants.estimateContentTypeId, web.ContentTypes, Constants.estimateContentTypeName);
                    web.ContentTypes.Add(estimateContentType);
                }

                estimateContentType.DocumentTemplate = string.Concat(web.Url, Constants.estimateTemplatePath);
                AddFieldsToContentType(web, estimateContentType, requiredFieldsToAdd, true);
                AddFieldsToContentType(web, estimateContentType, optionalFieldsToAdd, false);
                estimateContentType.Update(true);

            }
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private static void AddFieldsToContentType(SPWeb web, SPContentType contentType, Guid[] fieldIdArray, bool required)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException("contentType");
            }

            if (fieldIdArray == null)
            {
                throw new ArgumentException("fieldId");
            }

            if (web == null)
            {
                throw new ArgumentNullException("web");
            }

            foreach (Guid fieldId in fieldIdArray)
            {
                SPField field = web.AvailableFields[fieldId];

                SPFieldLink fieldLink = new SPFieldLink(field);
                fieldLink.Required = required;

                if (contentType.FieldLinks[fieldLink.Id] == null)
                {
                    contentType.FieldLinks.Add(fieldLink);
                }
            }
        }

        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
