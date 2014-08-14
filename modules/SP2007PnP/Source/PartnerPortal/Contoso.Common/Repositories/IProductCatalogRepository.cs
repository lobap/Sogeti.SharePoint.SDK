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
    /// Interface for the repository that retrieves information from the product catalog. 
    /// </summary>
    public interface IProductCatalogRepository
    {
        /// <summary>
        /// Retrieve a product with specified SKU
        /// </summary>
        /// <param name="sku">The sku of the product to retrieve.</param>
        /// <returns>The product</returns>
        Product GetProductBySku(string sku);

        /// <summary>
        /// Get all products in a particular category. 
        /// </summary>
        /// <param name="categoryId">The category to get products for. </param>
        /// <returns>The products in the catalog.</returns>
        IEnumerable<Product> GetProductsByCategory(string categoryId);
        
        /// <summary>
        /// Return a category with specified ID. 
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>The category. </returns>
        Category GetCategoryById(string categoryId);


        /// <summary>
        /// Gets the child categories with specified category ID.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        IEnumerable<Category> GetChildCategoriesByCategory(string categoryId);


        /// <summary>
        /// Gets the parts for a particular product specified by it's sku.
        /// </summary>
        /// <param name="sku">The sku to retrieve parts for.</param>
        /// <returns>The parts for this product. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ByProduct")]
        IEnumerable<Part> GetPartsByProductSku(string sku);

        /// <summary>
        /// Get the URL that shows the Categories. 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "It's easier to work with strings in this case.")]
        string GetCategoryProfileUrl();
    }
}