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

namespace ExecutionModels.Sandboxed.Features.EstimatesInstance
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("127674e5-4ce2-45f0-bc19-2b12505673d7")]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class EstimatesInstanceEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var web = properties.Feature.Parent as SPWeb;

            var estimatesList = web.GetList(string.Concat(web.ServerRelativeUrl, Constants.estimatesListLocation));

            AddContentTypeToList(Constants.sowContentTypeId, estimatesList, web);
            AddContentTypeToList(Constants.estimateContentTypeId, estimatesList, web);

            var documentContentType = estimatesList.ContentTypes[Constants.documentContentTypeName];

            if (documentContentType != null)
            {
                try
                {
                    var spContentTypeIdInList = estimatesList.ContentTypes.BestMatch(
                                                                    Constants.documentContentTypeId);
                    if (documentContentType.Id == spContentTypeIdInList)
                    {
                        estimatesList.ContentTypes.Delete(spContentTypeIdInList);
                        estimatesList.Update();
                    }
                }
                catch (SPException spException)
                {
                    //content type is in use.
                    //Add exception shielding for features.
                    System.Diagnostics.Trace.WriteLine(spException.ToString());
                }
            }
        }

        static bool ListContains(SPList list, SPContentTypeId id)
        {
            var matchId = list.ContentTypes.BestMatch(id);
            return matchId.IsChildOf(id);
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private static void AddContentTypeToList(SPContentTypeId spContentTypeId, SPList list, SPWeb web)
        {
            var contentType = web.AvailableContentTypes[spContentTypeId];

            if (contentType != null)
            {
                list.ContentTypesEnabled = true;
                list.Update();

                if (!ListContains(list, spContentTypeId))
                {
                    list.ContentTypes.Add(contentType);
                    list.Update();
                }
            }
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


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
