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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;

using TypeMock.ArrangeActAssert;

using Contoso.PartnerPortal.Promotions.Repositories;
using Contoso.PartnerPortal.Promotions.WebParts.PartnerPromotions;

namespace Contoso.PartnerPortal.Promotions.Tests
{
    /// <summary>
    /// Summary description for PartnerPromotionsPresenterTest
    /// </summary>
    [TestClass]
    public class PartnerPromotionsPresenterFixture
    {
        [TestCleanup]
        public void Cleanup()
        {
            SharePointServiceLocator.Reset();
            Isolate.CleanUp();
        }
        [TestMethod]
        public void FindPromotionPagesTest()
        {
            //SPWeb fakeWeb = Isolate.Fake.Instance<SPWeb>(Members.ReturnRecursiveFakes);

            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<IPartnerPromotionRepository, MockPartnerPromotionRepository>());

            MockPartnerPromotionRepository.GetAllMyPromosRetVal = new List<PartnerPromotionEntity>();
            PartnerPromotionsPresenter presenter = new PartnerPromotionsPresenter();
            //presenter.TargetWeb = fakeWeb;

            Assert.AreSame(MockPartnerPromotionRepository.GetAllMyPromosRetVal, presenter.FindPromotionPages());
        }
    }

    class MockPartnerPromotionRepository : IPartnerPromotionRepository
    {
        public PartnerPromotionEntity GetBySku(string sku)
        {
            return GetBySkuRetVal;
        }

        public IList<PartnerPromotionEntity> GetAllMyPromos()
        {
            return GetAllMyPromosRetVal;
        }

        public static PartnerPromotionEntity GetBySkuRetVal { get; set; }
        public static IList<PartnerPromotionEntity> GetAllMyPromosRetVal { get; set; }
    }
}