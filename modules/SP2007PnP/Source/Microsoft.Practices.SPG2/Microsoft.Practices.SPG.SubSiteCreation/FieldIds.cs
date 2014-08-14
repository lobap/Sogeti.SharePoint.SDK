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
    /// Definition of the field id's that are used by the subsite creation process
    /// </summary>
    public static class FieldIds
    {
        /// <summary>
        /// The name of business event that has occurred.
        /// </summary>
        public static readonly Guid BusinessEventFieldId = new Guid("3c9699e9-2f9b-4cd6-845e-76c7e58d129a");

        /// <summary>
        /// The name of the site template that should be used to create a site. 
        /// </summary>
        public static readonly Guid SiteTemplateFieldId = new Guid("84267e40-7f47-4f40-b3be-4004312eb467");

        /// <summary>
        /// The ID that uniquely identifies the business event that has occurred.  
        /// </summary>
        public static readonly Guid EventIdFieldId = new Guid("7106DC30-31D9-420d-969F-24A22B7AB7CD");

        /// <summary>
        /// The URL of the site collection that will contain the newly created site. 
        /// </summary>
        public static readonly Guid SiteCollectionUrlFieldId = new Guid("6304BC2B-1AE4-45a3-95F9-6207AD8BF265");

        /// <summary>
        /// The identification key of business event that will be used to associate the subsite with a specific business event. For example "IncidentId" or "OrderExceptionId".
        /// </summary>
        public static readonly Guid BusinessEventIdKeyFieldId = new Guid("3D6B9777-FDA5-4639-9316-B39D3E060573");

        /// <summary>
        /// URL for the Top Level site.
        /// </summary>
        public static readonly Guid TopLevelSiteRelativeUrlFieldId = new Guid("E87A0DE5-29D7-461e-9C83-67C2D291B54C");
    }
}
