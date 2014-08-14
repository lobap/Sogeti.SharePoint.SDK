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
using System.Security.Permissions;
using Contoso.Common.BusinessEntities;
using Contoso.Common.Repositories;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.Common.Logging;

namespace Contoso.PartnerPortal.ProductCatalog.Navigation
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class BusinessDataCatalogSiteMapProvider : SiteMapProvider
    {
        private bool isInitialized;
        private SiteMapNode rootNode;

        private string profileUrl;
        private IProductCatalogRepository productCatalogRepository;

        private ILogger logger;

        public BusinessDataCatalogSiteMapProvider()
        {
            this.logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We want to make the business data catalog sitemap provider more robust. So we're logging the exception but keep going. ")]
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes)
        {
            if (!this.isInitialized)
            {
                base.Initialize(name, attributes);

                try
                {
                    productCatalogRepository = SharePointServiceLocator.Current.GetInstance<IProductCatalogRepository>();
                    this.profileUrl = productCatalogRepository.GetCategoryProfileUrl();

                    this.isInitialized = true;
                }
                catch (Exception ex)
                {
                    this.logger.LogToOperations(ex);
                }
            }
        }

        public override SiteMapNode RootNode
        {
            get
            {
                return this.rootNode;
            }
        }

        public override SiteMapNode FindSiteMapNode(string rawUrl)
        {
            SiteMapNode node = null;

            // Only run if this SiteMapProvider was initialized properly. The initialization
            // must be performed in order to connect to the BDC to retrieve categories.
            if (this.isInitialized)
            {
                // Parse the querystring to find the Category Id.
                string queryString = rawUrl.Split("?".ToCharArray())[1];
                string[] query = queryString.Split("&".ToCharArray());
                string[] queryParts = query[0].Split("=".ToCharArray());
                string categoryId = queryParts[1];

                Category category = this.productCatalogRepository.GetCategoryById(categoryId);
                if (category != null)
                {
                    node = new SiteMapNode(this, categoryId, rawUrl, category.Name);
                }
            }

            return node;
        }

        public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
        {
            SiteMapNodeCollection childNodes = new SiteMapNodeCollection();

            // Only run if this SiteMapProvider was initialized properly. The initialization
            // must be performed in order to connect to the BDC to retrieve categories.
            if (this.isInitialized)
            {
                IEnumerable<Category> childCategories = this.productCatalogRepository.GetChildCategoriesByCategory(node.Key);
                foreach (Category child in childCategories)
                {
                    childNodes.Add(new SiteMapNode(this,
                                                   child.CategoryId,
                                                   string.Format(CultureInfo.InvariantCulture, this.profileUrl, child.CategoryId),
                                                   child.Name));
                }
            }

            return childNodes;
        }

        public override SiteMapNode GetParentNode(SiteMapNode node)
        {
            SiteMapNode parentNode = null;

            // Only run if this SiteMapProvider was initialized properly. The initialization
            // must be performed in order to connect to the BDC to retrieve categories.
            if (this.isInitialized)
            {
                Category currentCategory = this.productCatalogRepository.GetCategoryById(node.Key);
                if (currentCategory != null)
                {
                    Category parentCategory = this.productCatalogRepository.GetCategoryById(currentCategory.ParentId);
                    if (parentCategory != null)
                    {
                        parentNode = new SiteMapNode(this,
                                                     parentCategory.CategoryId,
                                                     string.Format(CultureInfo.InvariantCulture, this.profileUrl, parentCategory.CategoryId),
                                                     parentCategory.Name);

                        if (parentCategory.CategoryId == "0")
                        {
                            this.rootNode = parentNode;
                        }
                    }
                }
            }

            return parentNode;
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return RootNode;
        }
    }
}
