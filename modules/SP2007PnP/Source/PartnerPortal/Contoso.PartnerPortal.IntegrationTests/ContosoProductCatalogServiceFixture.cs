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
using Contoso.PartnerPortal.IntegrationTests.ProductCatalogProxy;
using Microsoft.Office.Server.ApplicationRegistry.Infrastructure;
using Microsoft.Office.Server.ApplicationRegistry.MetadataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Office.Server.ApplicationRegistry.Runtime;

namespace Contoso.PartnerPortal.Portal.IntegrationTests
{
    /// <summary>
    /// Summary description for ContosoProductCatalogServiceFixture
    /// </summary>
    [TestClass]
    public class ContosoProductCatalogServiceFixture
    {
        [TestMethod]
        public void ContosoProductCatalogServiceIsRegistered()
        {
            SqlSessionProvider.Instance().SetSharedResourceProviderToUse("ContosoSSP");
            
            NamedLobSystemInstanceDictionary sysInstances = ApplicationRegistry.GetLobSystemInstances();
            
            Assert.AreEqual(1, sysInstances.Count);
            Assert.IsNotNull(sysInstances["ContosoProductCatalogService"]);
        }

        [TestMethod]
        public void ContosoProductCatalogServiceHasCatalogAndProductEntities()
        {
            SqlSessionProvider.Instance().SetSharedResourceProviderToUse("ContosoSSP");
            NamedLobSystemInstanceDictionary sysInstances = ApplicationRegistry.GetLobSystemInstances();
            LobSystemInstance lobSystemInstance = sysInstances["ContosoProductCatalogService"];

            NamedEntityDictionary entities = lobSystemInstance.GetEntities();

            Assert.AreEqual(3, entities.Count);
            Assert.IsNotNull(entities["Category"]);
            Assert.IsNotNull(entities["Product"]);
            Assert.IsNotNull(entities["Part"]);
        }

        [TestMethod]
        public void CategoryCanBeFound()
        {
            SqlSessionProvider.Instance().SetSharedResourceProviderToUse("ContosoSSP");
            NamedLobSystemInstanceDictionary sysInstances = ApplicationRegistry.GetLobSystemInstances();
            LobSystemInstance lobSystemInstance = sysInstances["ContosoProductCatalogService"];
            NamedEntityDictionary entities = lobSystemInstance.GetEntities();
            Entity categoryEntity = entities["Category"];

            IEntityInstance categoryInstance = categoryEntity.FindSpecific("0", lobSystemInstance);

            Assert.AreEqual("Root Category", categoryInstance["Name"]);
            Assert.AreEqual("0", categoryInstance["CategoryId"]);
            Assert.AreEqual(string.Empty, categoryInstance["ParentId"]);

            categoryInstance = categoryEntity.FindSpecific("1", lobSystemInstance);

            Assert.AreEqual("Medical Supplies", categoryInstance["Name"]);
            Assert.AreEqual("1", categoryInstance["CategoryId"]);
            Assert.AreEqual("0", categoryInstance["ParentId"]);
        }

        [TestMethod]
        public void ProductsCanBeFound()
        {
            SqlSessionProvider.Instance().SetSharedResourceProviderToUse("ContosoSSP");
            NamedLobSystemInstanceDictionary sysInstances = ApplicationRegistry.GetLobSystemInstances();
            LobSystemInstance lobSystemInstance = sysInstances["ContosoProductCatalogService"];
            NamedEntityDictionary entities = lobSystemInstance.GetEntities();
            Entity productEntity = entities["Product"];

            IEntityInstance productInstance = productEntity.FindSpecific("1000000000", lobSystemInstance);

            Assert.AreEqual("Blood Pressure Kit", productInstance["Name"]);
            Assert.AreEqual("Blood pressure kit includes cuff with velcro adhesive and easy to read glow in the dark dial.", productInstance["LongDescription"]);
            Assert.AreEqual("Blood pressure kit includes cuff and easy to read dial.", productInstance["ShortDescription"]);
            Assert.AreEqual("1000000000", productInstance["Sku"]);
            Assert.AreEqual("8", productInstance["CategoryId"]);
            Assert.IsTrue(productInstance["ImagePath"].ToString().EndsWith("images/bloodpressure.jpg"));
            Assert.IsTrue(productInstance["ThumbnailImagePath"].ToString().EndsWith("images/bloodpressure.jpg"));

            productInstance = productEntity.FindSpecific("2000000000", lobSystemInstance);

            Assert.AreEqual("Gurney", productInstance["Name"]);
            Assert.AreEqual("Gurney includes rubber wheels and extra padding. Meets most federal safety requirements.", productInstance["LongDescription"]);
            Assert.AreEqual("Gurney includes rubber wheels and extra padding.", productInstance["ShortDescription"]);
            Assert.AreEqual("2000000000", productInstance["Sku"]);
            Assert.AreEqual("10", productInstance["CategoryId"]);
            Assert.IsTrue(productInstance["ImagePath"].ToString().EndsWith("images/gurney.jpg"));
            Assert.IsTrue(productInstance["ThumbnailImagePath"].ToString().EndsWith("images/gurney.jpg"));
        }

        [TestMethod]
        public void PartsCanBeFound()
        {
            ProductCatalogClient client = new ProductCatalogClient();
            IList<Part> parts = client.GetPartsByProductSku("1000000000");

            Assert.AreEqual(2, parts.Count);
        }
    }
}
