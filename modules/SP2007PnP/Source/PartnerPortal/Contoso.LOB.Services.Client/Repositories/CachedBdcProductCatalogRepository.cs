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
using System.Web;
using System.Web.Caching;
using Contoso.Common;
using Contoso.Common.BusinessEntities;
using Contoso.Common.Repositories;
using Microsoft.Office.Server.ApplicationRegistry.MetadataModel;
using Microsoft.Office.Server.ApplicationRegistry.Runtime;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;

namespace Contoso.LOB.Services.Client.Repositories
{
    /// <summary>
    /// Repository that retrieves product catalog information from the BDC. This repository stores the product information 
    /// in the cache, to improve loading times. 
    /// 
    /// This repository demonstrates how to cache results that are retrieved from the BDC. There is no way to apply caching in the BDC 
    /// itself, so we had to build a wrapper around it. This is that wrapper. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is exposed through the service locator")]
    internal class CachedBdcProductCatalogRepository : IProductCatalogRepository
    {
        private const string CategoryEntityName = "Category";
        private const string ContosoProductCatalogSystemName = "ContosoProductCatalogService";
        private const string PartEntityName = "Part";
        private const string ProductEntityName = "Product";
        private readonly Cache cache;

        private readonly ILogger logger;
        private Entity categoryEntityValue;
        private Entity partEntityValue;
        private LobSystemInstance productCatalogSystem;
        private Entity productEntityValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedBdcProductCatalogRepository"/> class.
        /// </summary>
        public CachedBdcProductCatalogRepository()
        {
            // Get an instance of the logger. 
            logger = SharePointServiceLocator.Current.GetInstance<ILogger>();

            // Retrieve the Cache in the constructor. You have to do this here, because, if you want to use this
            // repository in a ListItemEventReceiver, you only have access to the context (spcontext, http context and current user)
            // in the constructor of the splistitemeventreceiver. 
            cache = HttpContext.Current.Cache;
        }

        private LobSystemInstance ProductCatalogSystem
        {
            get
            {
                if (productCatalogSystem == null)
                {
                    productCatalogSystem =
                        ApplicationRegistry.GetLobSystemInstanceByName(ContosoProductCatalogSystemName);
                }

                return productCatalogSystem;
            }
        }

        private Entity ProductEntity
        {
            get
            {
                if (productEntityValue == null)
                {
                    productEntityValue = ProductCatalogSystem.GetEntities()[ProductEntityName];
                }

                return productEntityValue;
            }
        }

        private Entity CategoryEntity
        {
            get
            {
                if (categoryEntityValue == null)
                {
                    categoryEntityValue = ProductCatalogSystem.GetEntities()[CategoryEntityName];
                }

                return categoryEntityValue;
            }
        }

        private Entity PartEntity
        {
            get
            {
                if (partEntityValue == null)
                {
                    partEntityValue = ProductCatalogSystem.GetEntities()[PartEntityName];
                }

                return partEntityValue;
            }
        }

        #region IProductCatalogRepository Members

        /// <summary>
        /// Retrieve a product with specified SKU
        /// </summary>
        /// <param name="sku">The sku of the product to retrieve.</param>
        /// <returns>The product</returns>
        public Product GetProductBySku(string sku)
        {
            if (string.IsNullOrEmpty(sku))
            {
                throw new ArgumentNullException("sku");
            }

            Product product = GetCachedProduct(sku);

            if (product == null)
            {
                product = GetProductFromBDC(sku);
                SetCachedProduct(product);
            }

            return product;
        }

        /// <summary>
        /// Get all products in a particular category.
        /// </summary>
        /// <param name="categoryId">The category to get products for.</param>
        /// <returns>The products in the catalog.</returns>
        public IEnumerable<Product> GetProductsByCategory(string categoryId)
        {
            List<Product> products = GetCachedProductsForCategory(categoryId);

            if (products == null)
            {
                products = GetProductsByCategoryFromBDC(categoryId);
                if (products != null)
                    SetCachedProductsForCategory(products, categoryId);
            }

            return products;
        }

        /// <summary>
        /// Return a category with specified ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>The category.</returns>
        public Category GetCategoryById(string categoryId)
        {
            Category category = GetCachedCategory(categoryId);

            if (category == null)
            {
                if (!string.IsNullOrEmpty(categoryId))
                {
                    IEntityInstance categoryInstance = CategoryEntity.FindSpecific(categoryId, ProductCatalogSystem);

                    category = new Category();
                    category.CategoryId = (string) categoryInstance["CategoryId"];
                    category.Name = (string) categoryInstance["Name"];
                    category.ParentId = (string) categoryInstance["ParentId"];

                    SetCachedCategory(category);
                }
            }

            return category;
        }

        /// <summary>
        /// Gets the child categories with specified category ID.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public IEnumerable<Category> GetChildCategoriesByCategory(string categoryId)
        {
            List<Category> categories = GetCachedChildCategories(categoryId);

            if (categories == null)
            {
                categories = GetChildCategoriesByCategoryFromBDC(categoryId);
                SetCachedChildCategoriesForCategory(categories, categoryId);
            }

            return categories;
        }

        /// <summary>
        /// Gets the parts for a particular product specified by it's sku.
        /// </summary>
        /// <param name="sku">The sku to retrieve parts for.</param>
        /// <returns>The parts for this product.</returns>
        public IEnumerable<Part> GetPartsByProductSku(string sku)
        {
            List<Part> parts = new List<Part>();

            IEntityInstance sourceProduct = ProductEntity.FindSpecific(sku, ProductCatalogSystem);
            EntityInstanceCollection source = new EntityInstanceCollection();

            if (source != null)
            {
                source.Add(sourceProduct);
            }

            IEntityInstanceEnumerator entitiesEnumerator = PartEntity.FindAssociated(source, ProductCatalogSystem);
            while (entitiesEnumerator.MoveNext())
            {
                IEntityInstance partInstance = entitiesEnumerator.Current;

                Part part = new Part();
                part.Name = (string) partInstance["Name"];
                part.PartId = (string) partInstance["PartId"];
                parts.Add(part);
            }

            return parts;
        }

        /// <summary>
        /// Get the URL that shows the Categories.
        /// </summary>
        /// <returns></returns>
        public string GetCategoryProfileUrl()
        {
            return CategoryEntity.GetActions()["View Profile"].Url;
        }

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private string GetProductCacheKey(string sku)
        {
            return typeof (Product).FullName + sku;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private string GetProductsInCategoryCacheKey(string categoryId)
        {
            return typeof (Product).FullName + categoryId;
        }

        private Product GetCachedProduct(string sku)
        {
            Product product = product = cache[GetProductCacheKey(sku)] as Product;
            return product;
        }

        private void SetCachedProduct(Product product)
        {
            if (product != null)
            {
                cache.Add(GetProductCacheKey(product.Sku), product, null, Cache.NoAbsoluteExpiration,
                          TimeSpan.FromMinutes(5f), CacheItemPriority.Normal, null);
            }
        }

        private List<Product> GetCachedProductsForCategory(string categoryId)
        {
            List<Product> products = null;
            List<string> productsInCategory = cache[GetProductsInCategoryCacheKey(categoryId)] as List<string>;

            if (productsInCategory != null)
            {
                products = new List<Product>();
                foreach (string sku in productsInCategory)
                {
                    Product product = GetProductBySku(sku);
                    if (product != null)
                    {
                        products.Add(product);
                    }
                    else
                    {
                        // The list of cached products for this category contains a product id, that could not be found. 
                        // This would indicate a problem with the caching stragety. 
                        string errorMessage = string.Format(CultureInfo.CurrentUICulture,
                                                            "There is problem in the cache: Sku '{0}' was listed in category {1} but could not be retrieved.", sku, categoryId);
                        logger.TraceToDeveloper(errorMessage, (int) EventLogEventId.SkuNotFound);
                    }
                }
            }
            return products;
        }

        private void SetCachedProductsForCategory(List<Product> products, string categoryId)
        {
            List<string> productsInCategory = new List<string>();

            foreach (Product product in products)
            {
                productsInCategory.Add(product.Sku);
                SetCachedProduct(product);
            }

            cache.Add(GetProductsInCategoryCacheKey(categoryId), productsInCategory, null, Cache.NoAbsoluteExpiration,
                      TimeSpan.FromMinutes(5f), CacheItemPriority.Normal, null);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private string GetCategoryCacheKey(string categoryId)
        {
            return typeof (Category).FullName + categoryId;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private string GetChildCategoriesCacheKey(string categoryId)
        {
            return typeof (Category).FullName + categoryId + ".children";
        }


        private Category GetCachedCategory(string categoryId)
        {
            Category category = null;

            string key = GetCategoryCacheKey(categoryId);

            if (cache[key] != null)
            {
                category = cache[key] as Category;
            }
            return category;
        }

        private void SetCachedCategory(Category category)
        {
            cache.Add(GetCategoryCacheKey(category.CategoryId), category, null, Cache.NoAbsoluteExpiration,
                      TimeSpan.FromMinutes(5f), CacheItemPriority.Normal, null);
        }

        private void SetCachedChildCategoriesForCategory(List<Category> categories, string categoryId)
        {
            List<string> children = new List<string>();

            foreach (Category child in categories)
            {
                children.Add(child.CategoryId);
                SetCachedCategory(child);
            }

            cache.Add(GetChildCategoriesCacheKey(categoryId), children, null, Cache.NoAbsoluteExpiration,
                      TimeSpan.FromMinutes(5f), CacheItemPriority.Normal, null);
        }

        private List<Category> GetCachedChildCategories(string categoryId)
        {
            List<Category> children = null;
            List<string> childrenInCategory = cache[GetChildCategoriesCacheKey(categoryId)] as List<string>;

            if (childrenInCategory != null)
            {
                children = new List<Category>();
                foreach (string childCategoryId in childrenInCategory)
                {
                    Category child = GetCategoryById(childCategoryId);
                    if (child != null)
                    {
                        children.Add(child);
                    }
                    else
                    {
                        // The list of cached child categories indicates that a child category should be available
                        // but the category could not be found. 
                        string errorMessage = string.Format(CultureInfo.CurrentUICulture,
                                                            "There is problem in the cache: Category '{0}' ommitted from children of category {1}.",
                                                            childCategoryId, categoryId);
                        logger.TraceToDeveloper(errorMessage, (int) EventLogEventId.SkuNotFound);
                    }
                }
            }
            return children;
        }


        private Product GetProductFromBDC(string sku)
        {
            Product product;
            try
            {
                IEntityInstance productInstance = ProductEntity.FindSpecific(sku, ProductCatalogSystem);

                product = new Product();
                product.CategoryId = (string) productInstance["CategoryId"];
                product.ImagePath = (string) productInstance["ImagePath"];
                product.LongDescription = (string) productInstance["LongDescription"];
                product.ShortDescription = (string) productInstance["ShortDescription"];
                product.Name = (string) productInstance["Name"];
                product.Sku = (string) productInstance["Sku"];
                product.ThumbnailImagePath = (string) productInstance["ThumbnailImagePath"];
            }
            catch (ObjectNotFoundException ex) 
            {
                string errorMessage = string.Format(CultureInfo.CurrentUICulture, "Sku '{0}' could not be found.", sku);
                logger.LogToOperations(ex, errorMessage, (int) EventLogEventId.SkuNotFound);
                return null;
            }
            return product;
        }

        private List<Product> GetProductsByCategoryFromBDC(string categoryId)
        {
            List<Product> products = new List<Product>();

            IEntityInstance sourceCategory = CategoryEntity.FindSpecific(categoryId, ProductCatalogSystem);
            EntityInstanceCollection source = new EntityInstanceCollection();
            if (source != null)
            {
                source.Add(sourceCategory);
            }

            IEntityInstanceEnumerator entitiesEnumerator = ProductEntity.FindAssociated(source, ProductCatalogSystem);
            while (entitiesEnumerator.MoveNext())
            {
                IEntityInstance productInstance = entitiesEnumerator.Current;

                Product product = new Product();
                product.CategoryId = (string) productInstance["CategoryId"];
                product.ImagePath = (string) productInstance["ImagePath"];
                product.LongDescription = (string) productInstance["LongDescription"];
                product.ShortDescription = (string) productInstance["ShortDescription"];
                product.Name = (string) productInstance["Name"];
                product.Sku = (string) productInstance["Sku"];
                product.ThumbnailImagePath = (string) productInstance["ThumbnailImagePath"];

                products.Add(product);
                SetCachedProduct(product);
            }

            return products;
        }

        public string GetProfileUrl()
        {
            return ProductEntity.GetActions()["View Profile"].Url;
        }


        private List<Category> GetChildCategoriesByCategoryFromBDC(string categoryId)
        {
            List<Category> categories = new List<Category>();

            if (!string.IsNullOrEmpty(categoryId))
            {
                IEntityInstance sourceCategory = CategoryEntity.FindSpecific(categoryId, ProductCatalogSystem);
                EntityInstanceCollection source = new EntityInstanceCollection();
                if (sourceCategory != null)
                {
                    source.Add(sourceCategory);
                }

                IEntityInstanceEnumerator instanceEnumerator = CategoryEntity.FindAssociated(source,
                                                                                             ProductCatalogSystem);
                while (instanceEnumerator.MoveNext())
                {
                    IEntityInstance categoryInstance = instanceEnumerator.Current;
                    Category category = new Category();
                    category.CategoryId = (string) categoryInstance["CategoryId"];
                    category.Name = (string) categoryInstance["Name"];
                    category.ParentId = (string) categoryInstance["ParentId"];

                    categories.Add(category);

                    string key = typeof (Category).FullName + categoryId;
                    cache.Add(key, categories, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5f),
                              CacheItemPriority.Normal, null);
                }
            }

            return categories;
        }
    }
}