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
namespace Contoso.LOB.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.IO;
    using System.Reflection;
    
    using Contoso.LOB.Services.BusinessEntities;
    using System.Collections;
    using System.Configuration; 

    public partial class ProductCatalogDataSet : ICatalogStore
    {
        private static ProductCatalogDataSet _instance = new ProductCatalogDataSet();
        private IList<Product> _products;
        private IList<Category> _categories;
        private IList<Part> _parts;

        static ProductCatalogDataSet()
        {
            _instance.LoadXml();
        }

        public static ProductCatalogDataSet Instance
        {
            get
            {
                return _instance;
            }
        }

        private void LoadXml()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["productCatalogSampleDataFile"]))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("Contoso.LOB.Services.App_Data.ProductCatalogDataSet.xml"))
                {
                    this.ReadXml(stream);
                }
            }
            else
            {
                this.ReadXml(ConfigurationManager.AppSettings["productCatalogSampleDataFile"]);
            }
        }

        #region ICatalogStore Members

        public IEnumerable<Contoso.LOB.Services.BusinessEntities.Product> Products
        {
            get
            {
                if (this._products == null)
                {
                    this._products = new List<Product>();
                    foreach (ProductRow productRow in this.Product)
                    {
                        Product product = new Product();
                        product.CategoryId = productRow.CategoryId.ToString();
                        product.ImagePath = productRow.ImagePath;
                        product.LongDescription = productRow.LongDescription;
                        product.Name = productRow.Name;
                        product.ShortDescription = productRow.ShortDescription;
                        product.Sku = productRow.SKU;
                        product.ThumbnailImagePath = productRow.ThumbnailImagePath;
                        this._products.Add(product);
                    }
                }

                return this._products;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Contoso.LOB.Services.BusinessEntities.Category> Categories
        {
            get
            {
                if (this._categories == null)
                {
                    this._categories = new List<Category>();
                    foreach (CategoryRow categoryRow in this.Category)
                    {
                        Category category = new Category();
                        category.CategoryId = categoryRow.Id.ToString();
                        category.Name = categoryRow.Name;
                        category.ParentId = categoryRow.ParentId;
                        this._categories.Add(category);
                    }
                }

                return this._categories;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Part> Parts
        {
            get
            {
                if (this._parts == null)
                {
                    this._parts = new List<Part>();
                    foreach (PartRow partRow in this.Part)
                    {
                        Part part = new Part();
                        part.PartId = partRow.Id.ToString();
                        part.Name = partRow.Name;
                        this._parts.Add(part);
                    }
                }

                return this._parts;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
