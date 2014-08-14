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
using Contoso.Common.BusinessEntities;

namespace Contoso.Common.Repositories
{
    /// <summary>
    /// Interface for the repository that retrieves pricing information. 
    /// </summary>
    public interface IPricingRepository
    {
        /// <summary>
        /// Retrieve the price for a particular SKU that applies to the currently logged on user. This price includes all the discounts that have been
        /// applied to it for the current partner. 
        /// </summary>
        /// <param name="sku">The SKU to retrieve pricing for. </param>
        /// <returns>The price. </returns>
        Price GetPriceBySku(string sku);

        /// <summary>
        /// Get all the discounts that apply for this particular sku and for the currently logged on user. 
        /// </summary>
        /// <param name="sku">The SKU to retrieve pricing information for. </param>
        /// <returns>The discounts that have been applied. </returns>
        IEnumerable<Discount> GetDiscountsBySku(string sku);
    }
}