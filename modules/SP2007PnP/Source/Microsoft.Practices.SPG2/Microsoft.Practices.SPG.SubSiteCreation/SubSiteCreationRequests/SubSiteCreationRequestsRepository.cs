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
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SPG.Common.ListRepository;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ServiceLocation;

namespace Microsoft.Practices.SPG.SubSiteCreation
{
    /// <summary>
    /// Repository for adding subsite creation requests to the list. Each request that get's added
    /// will start the subsite creation workflow. 
    /// </summary>
    public class SubSiteCreationRequestsRepository : ISubSiteCreationRequestRepository
    {
        #region Private constants and ReadOnly fields

        ListItemFieldMapper<SubSiteCreationRequest> ListItemFieldMapper = new ListItemFieldMapper<SubSiteCreationRequest>();
        
        #endregion

        /// <summary>
        /// Create an instance of the SubSiteCreation
        /// </summary>
        public SubSiteCreationRequestsRepository()
        {
            ListItemFieldMapper.AddMapping(FieldIds.BusinessEventFieldId, Constants.BusinessEventProperty);
            ListItemFieldMapper.AddMapping(FieldIds.EventIdFieldId, Constants.EventIdProperty);
            ListItemFieldMapper.AddMapping(FieldIds.SiteCollectionUrlFieldId, Constants.SiteCollectionUrlProperty);
        }

        #region ISubSiteCreationRequestRepository Members

        /// <summary>
        /// Inserts a new item into the "Sub Site Creation Request List". Calling this method will trigger
        /// the "Sub Site Creation Workflow".
        /// </summary>
        /// <param name="request">The request entity containing necessary information to successfully run the Sub Site Creation Workflow.</param>
        public void AddSubSiteCreationRequest(SubSiteCreationRequest request)
        {
            if (request == null)
            {
                throw new SubSiteCreationException(Constants.TheSubSiteCreationRequestWasNullMessage);
            }
            else if(string.IsNullOrEmpty(request.BusinessEvent))
            {
                throw new SubSiteCreationException(string.Format(CultureInfo.CurrentCulture, Constants.ValueProvidedNullOrEmptyMessage, Constants.BusinessEventProperty));
            }
            else if(string.IsNullOrEmpty(request.EventId))
            {
                throw new SubSiteCreationException(string.Format(CultureInfo.CurrentCulture, Constants.ValueProvidedNullOrEmptyMessage, Constants.EventIdProperty));
            }
            else if(string.IsNullOrEmpty(request.SiteCollectionUrl))
            {
                throw new SubSiteCreationException(string.Format(CultureInfo.CurrentCulture, Constants.ValueProvidedNullOrEmptyMessage, Constants.SiteCollectionUrlProperty));
            }

            IHierarchicalConfig hierarchicalConfig = SharePointServiceLocator.Current.GetInstance<IHierarchicalConfig>();
            string adminWebUrl = hierarchicalConfig.GetByKey<string>(Constants.SubSiteCreationConfigSiteKey, ConfigLevel.CurrentSPFarm);
            if (string.IsNullOrEmpty(adminWebUrl))
            {
                throw new SubSiteCreationException(string.Format(CultureInfo.CurrentCulture, Constants.ConfigSiteNotFoundMessage, Constants.SubSiteCreationConfigSiteKey));
            }

            using (SPSite site = new SPSite(adminWebUrl))
            {
                using (SPWeb adminWeb = site.OpenWeb())
                {
                    SPList subSiteCreationRequests = adminWeb.Lists[Constants.SubSiteRequestsListName];
                    SPListItem subSiteCreationRequest = subSiteCreationRequests.Items.Add();
                    ListItemFieldMapper.FillSPListItemFromEntity(subSiteCreationRequest, request);
                    subSiteCreationRequest.Update();
                }
            }
        }

        #endregion
    }
}