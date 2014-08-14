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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contoso.LOB.Services.BusinessEntities;
using Contoso.LOB.Services.Security;

namespace Contoso.LOB.Services.Tests
{   
    /// <summary>
    ///This is a test class for PricingTest and is intended
    ///to contain all PricingTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PricingFixture
    {
        private MockSecurityHelper mockSecurityHelper;

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
        /// <summary>
        ///A test for GetPrice
        ///</summary>
        [TestMethod()]
        public void GetPriceForKnownPartnerAppliesDiscounts()
        {
            mockSecurityHelper.UserToReturn = "ContosoPartner1";

            TestablePricing target = new TestablePricing();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            Price expected = new Price();
            expected.ProductSku = "1000000000";
            expected.PartnerId = "ContosoPartner1";
            expected.Value = 159.995M; // 319.99 / 2

            Price actual;
            actual = target.GetPriceBySku(expected.ProductSku);

            Assert.AreEqual(expected.ProductSku, actual.ProductSku);
            Assert.AreEqual(expected.PartnerId, actual.PartnerId);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void GetPriceForUnknownPartnerReturnsBasePrice()
        {
            mockSecurityHelper.UserToReturn = "999";

            TestablePricing target = new TestablePricing();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            Price expected = new Price();
            expected.ProductSku = "1000000000";
            expected.PartnerId = "999";
            expected.Value = 319.99M;

            Price actual;
            actual = target.GetPriceBySku(expected.ProductSku);

            Assert.AreEqual(expected.ProductSku, actual.ProductSku);
            Assert.AreEqual(expected.PartnerId, actual.PartnerId);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void GetPricePriceNotFoundTest()
        {
            mockSecurityHelper.UserToReturn = "ContosoPartner1";

            TestablePricing target = new TestablePricing();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            Price actual = target.GetPriceBySku("99999");

            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for GetDiscountsByProductIdTest
        ///</summary>
        [TestMethod()]
        public void GetDiscountsByProductSkuTest()
        {
            mockSecurityHelper.UserToReturn = "ContosoPartner1";

            TestablePricing target = new TestablePricing();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            int expectedCount = 1;
            string expectedId = "1";
            string expectedPartnerId = "ContosoPartner1";
            string expectedProductSku = "1000000000";
            string expectedName = "2 for 1";
            decimal expectedValue = 50M;

            IList<Discount> actual;
            actual = target.GetDiscountsBySku("1000000000");

            Assert.AreEqual(expectedCount, actual.Count);
            Assert.AreEqual(expectedId, actual[0].Id);
            Assert.AreEqual(expectedPartnerId, actual[0].PartnerId);
            Assert.AreEqual(expectedProductSku, actual[0].ProductSku);
            Assert.AreEqual(expectedName, actual[0].Name);
            Assert.AreEqual(expectedValue, actual[0].Value);
        }

        [TestMethod]
        public void GetDiscountsByProductDiscountsNotFoundTest()
        {
            mockSecurityHelper.UserToReturn = "ContosoPartner1";

            TestablePricing target = new TestablePricing();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            int expectedCount = 0;

            IList<Discount> actual;
            actual = target.GetDiscountsBySku("9999");

            Assert.AreEqual(expectedCount, actual.Count);
        }

        /// <summary>
        ///A test for GetDiscountByNameTest
        ///</summary>
        [TestMethod()]
        public void GetDiscountByNameTest()
        {
            mockSecurityHelper.UserToReturn = "ContosoPartner1";

            TestablePricing target = new TestablePricing();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            string expectedId = "1";
            string expectedPartnerId = "ContosoPartner1";
            string expectedProductSku = "1000000000";
            string expectedName = "2 for 1";
            decimal expectedValue = 50M;

            Discount actual;
            actual = target.GetDiscountByName("2 for 1");

            Assert.AreEqual(expectedId, actual.Id);
            Assert.AreEqual(expectedPartnerId, actual.PartnerId);
            Assert.AreEqual(expectedProductSku, actual.ProductSku);
            Assert.AreEqual(expectedName, actual.Name);
            Assert.AreEqual(expectedValue, actual.Value);
        }

        [TestMethod]
        public void GetDiscountByNameDiscountNotFoundTest()
        {
            mockSecurityHelper.UserToReturn = "ContosoPartner1";

            TestablePricing target = new TestablePricing();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            Discount actual;
            actual = target.GetDiscountByName("kajdfljad");

            Assert.AreEqual(null, actual);
        }

        /// <summary>
        ///A test for GetDiscountByIdTest
        ///</summary>
        [TestMethod()]
        public void GetDiscountByIdTest()
        {
            mockSecurityHelper.UserToReturn = "ContosoPartner1";

            TestablePricing target = new TestablePricing();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            string expectedId = "1";
            string expectedPartnerId = "ContosoPartner1";
            string expectedProductSku = "1000000000";
            string expectedName = "2 for 1";

            Discount actual;
            actual = target.GetDiscountById("1");

            Assert.AreEqual(expectedId, actual.Id);
            Assert.AreEqual(expectedPartnerId, actual.PartnerId);
            Assert.AreEqual(expectedProductSku, actual.ProductSku);
            Assert.AreEqual(expectedName, actual.Name);
        }

        [TestMethod]
        public void GetDiscountByIdDiscountNotFoundTest()
        {
            mockSecurityHelper.UserToReturn = "ContosoPartner1";

            TestablePricing target = new TestablePricing();
            target.ReplacementSecurityHelper = mockSecurityHelper;

            Discount actual;
            actual = target.GetDiscountById("9999");

            Assert.AreEqual(null, actual);
        }
    }

    public class MockSecurityHelper : ISecurityHelper
    {
        public string UserToReturn;

        #region IPricingSecurityHelper Members

        public string GetPartnerId()
        {
            return UserToReturn;
        }

        public void DemandAuthorizedPermissions()
        {
        }

        #endregion
    }

    public class TestablePricing : Pricing
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
