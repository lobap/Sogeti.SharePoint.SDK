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
    /// A Discount that a partner can get for a particular product
    /// </summary>
    public class Discount
    {
        /// <summary>
        /// The value of the discount, in percent. 
        /// </summary>
        public decimal Value
        { 
            get; set;
        }

        /// <summary>
        /// The Id of the discount.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the product sku.
        /// </summary>
        /// <value>The product sku.</value>
        public string ProductSku
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the partner id.
        /// </summary>
        /// <value>The partner id.</value>
        public string PartnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }
    }
}