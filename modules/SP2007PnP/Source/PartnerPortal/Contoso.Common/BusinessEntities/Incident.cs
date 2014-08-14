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
using System.Web;
using System.Runtime.Serialization;

namespace Contoso.Common.BusinessEntities
{
    /// <summary>
    /// Represents an incident for a partner and a product. 
    /// </summary>
    public class Incident
    {
        /// <summary>
        /// The Id of the incident
        /// </summary>
        public string Id 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>The product.</value>
        public string Product
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the partner.
        /// </summary>
        /// <value>The partner.</value>
        public string Partner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the history of this incident. 
        /// </summary>
        /// <value>The history.</value>
        public IEnumerable<string> History
        {
            get;
            set;
        }
    }
}