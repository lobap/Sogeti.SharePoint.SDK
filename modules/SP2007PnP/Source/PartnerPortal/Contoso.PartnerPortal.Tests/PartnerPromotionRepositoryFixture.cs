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
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;
using Contoso.PartnerPortal.Promotions.Repositories;

namespace Contoso.PartnerPortal.Promotions.Tests
{
    [TestClass]
    public class PartnerPromotionRepositoryFixture
    {
        //These are actually integration tests just to validate the PartnerPromotionRepository is talking to the right list.

        [TestMethod]
        [Ignore]
        public void CanGetAllPromosFromRepository()
        {
            SPSite spSite = new SPSite("http://localhost:9001/sites/pssportal");
            Isolate.WhenCalled(()=> SPContext.Current.Site).WillReturn(spSite);

            PartnerPromotionRepository partnerPromotionRepository = new PartnerPromotionRepository();
            IList<PartnerPromotionEntity> promoPages = partnerPromotionRepository.GetAllMyPromos();

            Assert.AreEqual(3, promoPages.Count);
            
        }

        [TestMethod]
        [Ignore]
        public void CanGetSpecificPromoFromRepository()
        {
            SPSite spSite = new SPSite("http://localhost:9001/sites/pssportal");
            Isolate.WhenCalled(() => SPContext.Current.Site).WillReturn(spSite);

            PartnerPromotionRepository partnerPromotionRepository = new PartnerPromotionRepository();
            PartnerPromotionEntity partnerPromotion = partnerPromotionRepository.GetBySku("2000000000");

            Assert.AreEqual("2000000000", partnerPromotion.Sku);
            Assert.AreEqual("PromoDesc", partnerPromotion.Description);
        }
    }
}