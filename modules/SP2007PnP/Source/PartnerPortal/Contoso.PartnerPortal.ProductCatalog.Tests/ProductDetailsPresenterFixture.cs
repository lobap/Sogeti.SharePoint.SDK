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
using System.Web.UI;
using Contoso.Common.BusinessEntities;
using Contoso.Common.Repositories;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;
using Contoso.PartnerPortal.ProductCatalog.WebParts.ProductDetails;

namespace Contoso.PartnerPortal.ProductCatalog.Tests
{
    /// <summary>
    /// Summary description for ProductDetailsPresenterFixture
    /// </summary>
    [TestClass]
    public class ProductDetailsPresenterFixture
    {

        [TestCleanup]
        public void Cleanup()
        {
            Isolate.CleanUp();
            SharePointServiceLocator.ReplaceCurrentServiceLocator(null);
        }

        [TestMethod]
        public void OnViewLoadedTest()
        {
            MockProductDetailsView mockView = new MockProductDetailsView();
            
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<IProductCatalogRepository, MockProductCatalogRepository>()
                .RegisterTypeMapping<ILogger, MockLogger>());


            ProductDetailsPresenter target = new ProductDetailsPresenter(mockView);
            target.LoadProduct("123456");
            
            Assert.AreEqual("Widget", mockView.Product.Name);
            Assert.AreEqual("Widget Description", mockView.Product.LongDescription);
            Assert.AreEqual("123456", mockView.Product.Sku);
            Assert.IsTrue(mockView.DataBindCalled);
        }

        #region Mocks

        private class MockProductDetailsView : Control, IProductDetailsView
        {
            public Product product = new Product();
            public Price price = new Price();
            public bool DataBindCalled;

            #region IProductDetailsView Members

            public Product Product
            {
                get{ return this.product; }
                set { this.product = value; }
            }

            public override void DataBind()
            {
                this.DataBindCalled = true;
            }


            public Price Price
            {
                get
                {
                    return this.price;
                }
                set
                {
                    this.price = value;
                }
            }

            #endregion
        }

        private class MockProductCatalogRepository : IProductCatalogRepository
        {
            #region IProductCatalog Members

            public List<string> GetProductSkus()
            {
                throw new NotImplementedException();
            }

            public Product GetProductBySku(string sku)
            {
                return new Product 
                           { 
                               Name = "Widget", 
                               LongDescription = "Widget Description", 
                               ImagePath = "http://imagepath.jpg", 
                               Sku = "123456" 
                           };
            }

            public IEnumerable<Product> GetProductsByCategory(string categoryId)
            {
                throw new NotImplementedException();
            }

            public Category GetCategoryById(string categoryId)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Category> GetChildCategoriesByCategory(string categoryId)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Part> GetPartsByProductSku(string sku)
            {
                yield break;
            }

            public string GetCategoryProfileUrl()
            {
                throw new NotImplementedException();
            }

            #endregion

            public void Dispose()
            {
            }
        }

        #endregion
    }
}
