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
using System.Text;

namespace Microsoft.Practices.SPG.SubSiteCreation
{
    /// <summary>
    /// Constants that are used for the Site Creation. 
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The name of the config key that holds the name of the SubSite creation Configuration list. 
        /// </summary>
        public const string SubSiteCreationConfigSiteKey = "SubSiteCreationConfigurationSite";

        /// <summary>
        /// The name of the list for subsite creation rqeuests
        /// </summary>
        public const string SubSiteRequestsListName = "Sub Site Creation Requests";

        /// <summary>
        /// The name of the list that holds the business event types. 
        /// </summary>
        public const string BusinessEventTypeConfigListName = "Business Event Type Configuration";

        /// <summary>
        /// The name of the business event property.
        /// </summary>
        public const string BusinessEventProperty = "BusinessEvent";

        /// <summary>
        /// The name of the event Id property.
        /// </summary>
        public const string EventIdProperty = "EventId";

        /// <summary>
        /// The name of the site collection URL property.
        /// </summary>
        public const string SiteCollectionUrlProperty = "SiteCollectionUrl";

        /// <summary>
        /// The error message to return when the subsite creation configuration site was not found.
        /// </summary>
        internal const string ConfigSiteNotFoundMessage = "The site containing Sub Site Creation Configuration data could not be found in Configuration using key: {0}.";
        
        /// <summary>
        /// The error message to return when the subsite creation request was null.
        /// </summary>
        internal const string TheSubSiteCreationRequestWasNullMessage = "The subsite creation request was null.";

        /// <summary>
        /// The error message to return when the subsite creation request contains invalid information.
        /// </summary>
        internal const string ValueProvidedNullOrEmptyMessage = "The value provided for the {0} was null or empty.";        
    }
}