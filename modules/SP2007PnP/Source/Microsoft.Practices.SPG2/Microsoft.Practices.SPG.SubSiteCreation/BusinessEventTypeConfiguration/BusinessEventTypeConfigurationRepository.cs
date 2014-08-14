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

using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Microsoft.Practices.SPG.Common.ListRepository;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ServiceLocation;

namespace Microsoft.Practices.SPG.SubSiteCreation
{
    /// <summary>
    /// Class that provides centralized list access to the BusinessEventTypeConfiguration SharePoint list.
    /// This class follows the repository pattern in that it allows the consumer to work with strongly typed
    /// BusinessEventTypeConfiguration instances instead of weakly typed SPListItems.
    /// </summary>
    public class BusinessEventTypeConfigurationRepository : IBusinessEventTypeConfigurationRepository
    {
        private const string ConfigSiteNotFoundMessage = "The site containing Sub Site Creation Configuration data could not be found in Configuration using key: {0}.";
        private const string ConfigDataNotFoundMessage = "The configuration data for the {0} event could not be found.";

        readonly ListItemFieldMapper<BusinessEventTypeConfiguration> ListItemFieldMapper = new ListItemFieldMapper<BusinessEventTypeConfiguration>();

        /// <summary>
        /// Creates the Repository instance. 
        /// </summary>
        public BusinessEventTypeConfigurationRepository()
        {
            ListItemFieldMapper.AddMapping(FieldIds.SiteTemplateFieldId, "SiteTemplate");
            ListItemFieldMapper.AddMapping(FieldIds.BusinessEventIdKeyFieldId, "BusinessEventIdKey");
            ListItemFieldMapper.AddMapping(FieldIds.TopLevelSiteRelativeUrlFieldId, "TopLevelSiteRelativeUrl");
        }

        #region IBusinessEventTypeConfigurationRepository Members

        /// <summary>
        /// Retrieve the configuration for the specified business event. 
        /// </summary>
        /// <param name="businessEvent">The business event to get the configuration for</param>
        /// <returns>The business event's configuration. </returns>
        public BusinessEventTypeConfiguration GetBusinessEventTypeConfiguration(string businessEvent)
        {
            IHierarchicalConfig hierarchicalConfig = SharePointServiceLocator.Current.GetInstance<IHierarchicalConfig>();
            string adminWebUrl = hierarchicalConfig.GetByKey<string>(Constants.SubSiteCreationConfigSiteKey, ConfigLevel.CurrentSPFarm);
            if (string.IsNullOrEmpty(adminWebUrl))
            {
                throw new SubSiteCreationException(string.Format(CultureInfo.CurrentCulture, ConfigSiteNotFoundMessage, Constants.SubSiteCreationConfigSiteKey));
            }

            using (SPSite site = new SPSite(adminWebUrl))
            {
                using (SPWeb adminWeb = site.OpenWeb())
                {
                    SPList businessEventSiteTemplateList = adminWeb.Lists[Constants.BusinessEventTypeConfigListName];
                    CAMLQueryBuilder camlQueryBuilder = new CAMLQueryBuilder();
                    camlQueryBuilder.AddEqual(FieldIds.BusinessEventFieldId, businessEvent);

                    SPListItemCollection items = businessEventSiteTemplateList.GetItems(camlQueryBuilder.Build());

                    if (items.Count > 0)
                    {
                        return ListItemFieldMapper.CreateEntity(items[0]);
                    }
                    else
                    {
                        throw new SubSiteCreationException(string.Format(CultureInfo.CurrentCulture, ConfigDataNotFoundMessage, businessEvent));
                    }                    
                }
            }
        }

        #endregion
    }
}