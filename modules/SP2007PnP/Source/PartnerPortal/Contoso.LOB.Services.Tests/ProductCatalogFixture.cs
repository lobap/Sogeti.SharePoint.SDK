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
using System.Collections.Generic;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Contoso.LOB.Services.BusinessEntities;
using TypeMock.ArrangeActAssert;
using Contoso.LOB.Services.Security;

namespace Contoso.LOB.Services.Tests
{
    [TestClass]
    public class ProductCatalogFixture
    {
        private ISecurityHelper mockSecurityHelper;

        [TestInitialize]
        public void Init()
        {
            mockSecurityHelper = new MockSecurityHelper();
        }

        [TestCleanup]
        public void Cleanup()
        {
            mockSecurityHelper = null;
        }

        [TestMethod]
        public void GetChildCategoriesReturnsChildCategories()
        {
            TestableProductCatalog target = new TestableProductCatalog();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            Category rootCategory = target.GetCategoryById("0");
            IList<Category> childCategories = target.GetChildCategoriesByCategory(rootCategory.CategoryId);

            Assert.AreEqual(4, childCategories.Count);
            Assert.AreEqual("Medical Supplies", childCategories[0].Name);
            Assert.AreEqual("Hospital Equipment", childCategories[1].Name);
            Assert.AreEqual("Physician Equipment", childCategories[2].Name);
            Assert.AreEqual("Dental Equipment", childCategories[3].Name);
        }

        [TestMethod]
        public void CanGetSpecificCategory()
        {
            TestableProductCatalog target = new TestableProductCatalog();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            Category category = target.GetCategoryById("1");

            Assert.AreEqual("Medical Supplies", category.Name);
        }

        [TestMethod]
        public void CanGetCategories()
        {
            TestableProductCatalog target = new TestableProductCatalog();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            var categories = target.GetCategories();

            Assert.AreEqual(12, categories.Count);
        }

        [TestMethod]
        public void CanGetProductsForSpecificCategory()
        {
            TestableProductCatalog target = new TestableProductCatalog();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            IList<Product> products = target.GetProductsByCategory("8");

            Assert.AreEqual(1, products.Count);
            Assert.AreEqual("Blood Pressure Kit", products[0].Name);
        }

        [TestMethod]
        public void CanGetProductBySku()
        {
            Isolate.WhenCalled(() => HttpContext.Current.Request.Url.Scheme).WillReturn("http");
            Isolate.WhenCalled(() => HttpContext.Current.Server.MachineName).WillReturn("localhost");
            Isolate.WhenCalled(() => HttpContext.Current.Request.Url.Port).WillReturn(8585);
            Isolate.WhenCalled(() => HttpContext.Current.Request.Url.Segments).WillReturn(new string[] { "/", "Contoso.LOB.Services/", "service.svc" });
            TestableProductCatalog target = new TestableProductCatalog();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            Product product = target.GetProductBySku("1000000000");

            Assert.AreEqual("Blood Pressure Kit", product.Name);
            Assert.AreEqual("http://localhost:8585/Contoso.LOB.Services/images/bloodpressure.jpg", product.ImagePath);
            Assert.AreEqual("http://localhost:8585/Contoso.LOB.Services/images/bloodpressure.jpg", product.ThumbnailImagePath);
            Assert.AreEqual("Blood pressure kit includes cuff and easy to read dial.", product.ShortDescription);
            Assert.AreEqual("Blood pressure kit includes cuff with velcro adhesive and easy to read glow in the dark dial.", product.LongDescription);
            Assert.AreEqual("1000000000", product.Sku);
        }

        [TestMethod]
        public void GetProductBySkuWithMissingSkuReturnsNull()
        {
            TestableProductCatalog target = new TestableProductCatalog();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            Product product = target.GetProductBySku("XXXXXXXXXX");

            Assert.IsNull(product);
        }

        [TestMethod]
        public void CanGetProductSkus()
        {
            TestableProductCatalog target = new TestableProductCatalog();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            var products = target.GetProductSkus();

            Assert.AreEqual(7, products.Count);
            Assert.IsTrue(products.Contains("1000000000"));
            Assert.IsTrue(products.Contains("2000000000"));
            Assert.IsTrue(products.Contains("3000000000"));
            Assert.IsTrue(products.Contains("4000000000"));
            Assert.IsTrue(products.Contains("5000000000"));
            Assert.IsTrue(products.Contains("6000000000"));
            Assert.IsTrue(products.Contains("7000000000"));
        }
    }

    public class TestableProductCatalog : ProductCatalog
    {
        public ISecurityHelper ReplacementSecurityHelper
        {
            set
            {
                this.SecurityHelper = value;
            }
        }
    }
}
