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
    /// Entity for the SPList SubSiteCreationRequest. This list is a queue of sites to create and sites that have been created. 
    /// </summary>
    public class SubSiteCreationRequest
    {
        /// <summary>
        /// Identifies the type of business event that has occurred. This will help find the correct type of 
        /// <see cref="BusinessEventTypeConfiguration"/>. 
        /// </summary>
        public string BusinessEvent
        {
            get;
            set;
        }

        /// <summary>
        /// The Id of the business event that has occurred. 
        /// </summary>
        public string EventId
        {
            get;
            set;
        }

        /// <summary>
        /// The URL to the site collection that the that the site will be created in. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string SiteCollectionUrl
        {
            get;
            set;
        }
    }
}