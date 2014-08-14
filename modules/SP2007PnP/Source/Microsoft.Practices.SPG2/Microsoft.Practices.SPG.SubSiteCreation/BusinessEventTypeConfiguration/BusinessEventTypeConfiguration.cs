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
    /// Entity that holds the configuration for business event types. When a site is created for 
    /// a particular business event type, this configuration entity determines what template to 
    /// use and what the URL of the top level site is. 
    /// 
    /// This entity maps to items in the BusinessEventTypeConfiguration SPList. 
    /// </summary>
    public class BusinessEventTypeConfiguration
    {
        /// <summary>
        /// The name of the site template to use when creating a site for a particular business event type
        /// </summary>
        public string SiteTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// This key stores the business Event ID in the PropertyBag of the SPWeb that's created by this workflow. Use this key
        /// to read or write the Business Event ID in the PropertyBag of the SPWeb. 
        /// </summary>
        public string BusinessEventIdKey
        {
            get;
            set;
        }

        /// <summary>
        /// URL to the top level site. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "The SharePoint API requires an string.")]
        public string TopLevelSiteRelativeUrl
        {
            get;
            set;
        }
    }
}