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
using Microsoft.Practices.SharePoint.Common;

namespace ExecutionModels.FullTrust.Features.Approved_Estimates
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("e6ccc284-a68b-4029-acee-8293a21ac34b")]
    public class Approved_EstimatesEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;

            using (SPWeb web = site.RootWeb)
            {
                SPList approvedEstimatesList = web.GetList(string.Concat(web.ServerRelativeUrl, Constants.approvedEstimatesListLocation));

                SPContentType sowContentType = AddContentTypeToList(Constants.sowContentTypeId, approvedEstimatesList, web);
                SPContentType estimateContentType = AddContentTypeToList(Constants.estimateContentTypeId, approvedEstimatesList, web);
                
                Guid[] fieldsToAdd = new Guid[] 
                { 
                    Constants.estimateStatusFieldId, 
                    Constants.estimateValueFieldId, 
                    Constants.projectLookupFieldId 
                };
                AddFieldsToContentType(web, sowContentType, fieldsToAdd);
                AddFieldsToContentType(web, estimateContentType, fieldsToAdd);

                if (approvedEstimatesList.ContentTypes[Constants.documentContentTypeId] != null)
                {
                    SPContentTypeId spContentTypeIdInList = approvedEstimatesList.ContentTypes.BestMatch(
                                                                        Constants.documentContentTypeId);

                    approvedEstimatesList.ContentTypes.Delete(spContentTypeIdInList);
                    approvedEstimatesList.Update();
                }
            }
        }

        private static SPContentType AddContentTypeToList(SPContentTypeId spContentTypeId, SPList list, SPWeb web)
        {
            SPContentType contentType = web.ContentTypes[spContentTypeId];
            
            if (contentType != null)
            {
                list.ContentTypesEnabled = true;
                list.Update();
                
                if (list.ContentTypes[spContentTypeId] == null)
                {
                    string IdInCollection =
                        list.ContentTypes.BestMatch(spContentTypeId).ToString();

                    if (!IdInCollection.Contains(spContentTypeId.ToString()))
                    {
                        list.ContentTypes.Add(contentType);
                        list.Update();
                    }
                }
            }
            return contentType;
        }

        private static void AddFieldsToContentType(SPWeb web, SPContentType contentType, Guid[] fieldIdArray)
        {
            Validation.ArgumentNotNull(web, "web");
            Validation.ArgumentNotNull(contentType, "contentType");
            Validation.ArgumentNotNull(fieldIdArray, "fieldIdArray");

            foreach (Guid fieldId in fieldIdArray)
            {
                if (web.AvailableFields.Contains(fieldId))
                {
                    SPField field = web.AvailableFields[fieldId];
                    SPFieldLink fieldLink = new SPFieldLink(field);
                    if (contentType.FieldLinks[fieldLink.Id] == null)
                    {
                        contentType.FieldLinks.Add(fieldLink);
                        contentType.Update();
                    }
                }
            }
        }
    }
}
