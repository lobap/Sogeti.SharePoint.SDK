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
using System.Linq;
using System.Text;
using Contoso.PartnerPortal.Promotions.Repositories;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using System.Data;
using System.Web;

namespace Contoso.PartnerPortal.Promotions.WebParts.PartnerPromotions
{
    public class PartnerPromotionsPresenter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public IEnumerable<PartnerPromotionEntity> FindPromotionPages()
        {
            IPartnerPromotionRepository partnerPromotionRepository = SharePointServiceLocator.Current.GetInstance<IPartnerPromotionRepository>();
            return partnerPromotionRepository.GetAllMyPromos();

        }
    }
}
