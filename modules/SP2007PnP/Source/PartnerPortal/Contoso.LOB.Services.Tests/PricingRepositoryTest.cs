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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contoso.Common.Tests
{   
    /// <summary>
    ///This is a test class for PricingRepositoryTest and is intended
    ///to contain all PricingRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PricingRepositoryTest
    {
        ///// <summary>
        /////A test for GetPriceBySku
        /////</summary>
        //[TestMethod()]
        //[Ignore]
        //public void GetPriceBySkuTest()
        //{
        //    Cache cache = new Cache();
        //    MockPricingClient mockClient = new MockPricingClient();
        //    PricingRepository target = new PricingRepository(cache, mockClient);
        //    string sku = "1";
        //    Price expected = new Price();
        //    expected.ProductSku = "1";
        //    List<Price> cachedPrices = new List<Price>();
        //    cachedPrices.Add(expected);
        //    CacheItemRemovedCallback onRemove = new CacheItemRemovedCallback(this.RemovedCallback);
        //    cache.Add("PartnerPrices", cachedPrices, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, onRemove);

        //    Price actual;
        //    actual = target.GetPriceBySku(sku);
        //    Assert.AreEqual(expected, actual);
        //}

    }
}
