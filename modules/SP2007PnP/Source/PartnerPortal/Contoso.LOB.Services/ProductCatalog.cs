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
using System.ServiceModel.Activation;
using Contoso.LOB.Services.BusinessEntities;
using Contoso.LOB.Services.Contracts;
using Contoso.LOB.Services.Data;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Globalization;
using Contoso.LOB.Services.Security;

namespace Contoso.LOB.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ProductCatalog : IProductCatalog
    {
        private const string SleepAmout = "sleepAmount";
        private const string Product_ProductPart = "Product_ProductPart";
        private const string Part_ProductPart = "Part_ProductPart";
        #region Private Fields

        private ProductCatalogDataSet catalogStore;

        #endregion

        #region Constructors

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public ProductCatalog()
        {
            this.SecurityHelper = new SecurityHelper();
            catalogStore = ProductCatalogDataSet.Instance;
        }


        #endregion

        #region Methods

        #region Product

        private static Product CreateProduct(ProductCatalogDataSet.ProductRow row)
        {
            string urlToVirtualDir = string.Empty;
            HttpContext currentHttpContext = HttpContext.Current;
            if (currentHttpContext != null)
            {
                urlToVirtualDir = string.Format(CultureInfo.CurrentCulture, "{0}://{1}:{2}", currentHttpContext.Request.Url.Scheme,
                                                currentHttpContext.Server.MachineName, currentHttpContext.Request.Url.Port);
                
                //Include the URL segments except for the last one.
                for(int segmentIndex=0; segmentIndex<currentHttpContext.Request.Url.Segments.Length-1; segmentIndex++)
                {
                    urlToVirtualDir += currentHttpContext.Request.Url.Segments[segmentIndex];
                }
            }

            Product product = new Product();
            product.CategoryId = row.CategoryId.ToString(CultureInfo.CurrentCulture);
            product.ImagePath = string.Format(CultureInfo.CurrentCulture, "{0}{1}", urlToVirtualDir, row.ImagePath);
            product.LongDescription = row.LongDescription;
            product.Name = row.Name;
            product.ShortDescription = row.ShortDescription;
            product.Sku = row.SKU;
            product.ThumbnailImagePath = string.Format(CultureInfo.CurrentCulture, "{0}{1}", urlToVirtualDir, row.ThumbnailImagePath);
            return product;
        }

        public IList<Product> GetProductsByCategory(string categoryId)
        {
            SecurityHelper.DemandAuthorizedPermissions();

            List<Product> products = new List<Product>();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get(SleepAmout), CultureInfo.CurrentCulture));

            ProductCatalogDataSet.ProductRow[] match =
                catalogStore.Product.Select(string.Format(CultureInfo.CurrentCulture, "CategoryId = {0}", categoryId)) as ProductCatalogDataSet.ProductRow[];

            if (match.Length > 0)
            {
                foreach (ProductCatalogDataSet.ProductRow productRow in match)
                {
                    products.Add(CreateProduct(productRow));
                }
            }

            return products;
        }

        public Product GetProductBySku(string sku)
        {
            SecurityHelper.DemandAuthorizedPermissions();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get(SleepAmout), CultureInfo.CurrentCulture));

            if (sku.StartsWith("BAD", StringComparison.CurrentCultureIgnoreCase))
                throw new ArgumentException("This SKU is very BAD!");

            ProductCatalogDataSet.ProductRow[] match =
                catalogStore.Product.Select(string.Format(CultureInfo.CurrentCulture, "Sku = '{0}'", sku)) as ProductCatalogDataSet.ProductRow[];

            if (match.Length > 0)
            {
                return CreateProduct(match[0]);
            }

            return null;
        }

        public IEnumerable<Product> GetProducts()
        {
            SecurityHelper.DemandAuthorizedPermissions();

            List<Product> products = new List<Product>();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get(SleepAmout), CultureInfo.CurrentCulture));

            foreach (ProductCatalogDataSet.ProductRow productRow in catalogStore.Product)
            {
                products.Add(CreateProduct(productRow));
            }

            return products;
        }

        public IList<string> GetProductSkus()
        {
            SecurityHelper.DemandAuthorizedPermissions();

            List<string> productSkus = new List<string>();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get(SleepAmout), CultureInfo.CurrentCulture));

            foreach (ProductCatalogDataSet.ProductRow productRow in catalogStore.Product)
            {
                productSkus.Add(productRow.SKU);
            }

            return productSkus;
        }

        #endregion

        #region Category

        private static Category CreateCategory(ProductCatalogDataSet.CategoryRow row)
        {
            Category category = new Category();
            category.CategoryId = row.Id.ToString(CultureInfo.CurrentCulture);
            category.Name = row.Name;
            category.ParentId = row.ParentId;

            return category;
        }

        public Category GetCategoryById(string categoryId)
        {
            SecurityHelper.DemandAuthorizedPermissions();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get(SleepAmout), CultureInfo.CurrentCulture));

            ProductCatalogDataSet.CategoryRow[] match =
                catalogStore.Category.Select(string.Format(CultureInfo.CurrentCulture, "Id = {0}", categoryId)) as ProductCatalogDataSet.CategoryRow[];

            if (match.Length > 0)
            {
                return CreateCategory(match[0]);
            }

            return null;
        }

        public IList<Category> GetChildCategoriesByCategory(string categoryId)
        {
            SecurityHelper.DemandAuthorizedPermissions();

            List<Category> childCategories = new List<Category>();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get(SleepAmout), CultureInfo.CurrentCulture));
            ProductCatalogDataSet.CategoryRow[] match =
                catalogStore.Category.Select(string.Format(CultureInfo.CurrentCulture, "Id > 0 AND ParentId = {0}", categoryId)) as ProductCatalogDataSet.CategoryRow[];

            foreach (ProductCatalogDataSet.CategoryRow categoryRow in match)
            {
                childCategories.Add(CreateCategory(categoryRow));
            }

            return childCategories;
        }

        public IList<Category> GetCategories()
        {
            SecurityHelper.DemandAuthorizedPermissions();

            List<Category> categories = new List<Category>();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get(SleepAmout), CultureInfo.CurrentCulture));

            foreach (ProductCatalogDataSet.CategoryRow categoryRow in catalogStore.Category)
            {
                categories.Add(CreateCategory(categoryRow));
            }

            return categories;
        }

        #endregion

        #region Part

        public IEnumerable<Part> GetPartsByProductSku(string productSku)
        {
            SecurityHelper.DemandAuthorizedPermissions();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get(SleepAmout), CultureInfo.CurrentCulture));

            List<Part> parts = new List<Part>();
            ProductCatalogDataSet.ProductRow[] match =
                catalogStore.Product.Select(string.Format(CultureInfo.CurrentCulture, "Sku = '{0}'", productSku)) as ProductCatalogDataSet.ProductRow[];

            if (match.Length > 0)
            {
                ProductCatalogDataSet.ProductPartRow[] prodPartRows
                    = match[0].GetChildRows(Product_ProductPart) as ProductCatalogDataSet.ProductPartRow[];

                foreach (ProductCatalogDataSet.ProductPartRow prodPartRow in prodPartRows)
                {
                    ProductCatalogDataSet.PartRow partRow = prodPartRow.GetParentRow(Part_ProductPart) as ProductCatalogDataSet.PartRow;
                    Part part = new Part();
                    part.Name = partRow.Name;
                    part.PartId = partRow.Id.ToString(CultureInfo.CurrentCulture);
                    parts.Add(part);
                }
            }

            //Place a sleep in here to fake a slow service
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get(SleepAmout), CultureInfo.CurrentCulture));

            return parts;
        }

        #endregion

        #endregion

        protected ISecurityHelper SecurityHelper { get; set; }
    }
}