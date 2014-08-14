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
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ListRepository;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;

namespace Contoso.PartnerPortal.Promotions.Repositories
{
    public class PartnerPromotionRepository : IPartnerPromotionRepository
    {
        private const string ContentTypeName = "PromotionPage";
        private const string ListName = "Pages";

        /// <summary>
        /// The URL of the site that contains the partner promotions.
        /// </summary>
        public const string PartnerPromotionRepositoryUrlConfigKey =
            "Contoso.PartnerPortal.Promotions.Repositories.PartnerPromotionSiteUrl";

        private readonly Guid ProductSkuFieldId = new Guid("a933192c-4430-4814-97d5-f99b23ace03e");
        private readonly Guid PromotionDescriptionFieldId = new Guid("aaed1aca-3f64-4b13-8e87-863ebc659432");
        private readonly Guid PromotionImageFieldId = new Guid("729c0fb2-62c8-4a91-86c5-5ace6e8265fb");
        private readonly Guid PromotionNameFieldId = new Guid("351cd1fd-efd2-4db7-ac9d-c669149adce5");
        private readonly Guid WindowsMediaFieldId = new Guid("E38B18B2-B680-4601-9C46-B05E230F5605");

        private ListItemFieldMapper<PartnerPromotionEntity> ListItemFieldMapper =
            new ListItemFieldMapper<PartnerPromotionEntity>();

        private SPList promotionsList;
        private string promotionsWebUrl;

        public PartnerPromotionRepository()
        {
            // Get the URL of the Web that holds the partnerpromotions. This value is set by the PromotionsWebFeatureReceiver when
            // this feature is activated on a particular Web. 
            IHierarchicalConfig hierarchicalConfig = SharePointServiceLocator.Current.GetInstance<IHierarchicalConfig>();
            string partnerPromotionWebUrl = hierarchicalConfig.GetByKey<string>(PartnerPromotionRepositoryUrlConfigKey);

            Initialize(partnerPromotionWebUrl);

            //Important to retrieve ID value of SPListItem so that 
            //modifications to entity can be persisted back into SPListItem
            ListItemFieldMapper.AddMapping(ProductSkuFieldId, "Sku");
            ListItemFieldMapper.AddMapping(PromotionImageFieldId, "Image");
            ListItemFieldMapper.AddMapping(PromotionNameFieldId, "PromotionName");
            ListItemFieldMapper.AddMapping(PromotionDescriptionFieldId, "Description");
            ListItemFieldMapper.AddMapping(WindowsMediaFieldId, "Media");
        }

        #region IPartnerPromotionRepository Members

        public PartnerPromotionEntity GetBySku(string sku)
        {
            CAMLQueryBuilder camlQueryBuilder = new CAMLQueryBuilder();
            camlQueryBuilder.AddEqual("ProductSkuField", sku);

            SPListItemCollection collection = promotionsList.GetItems(camlQueryBuilder.Build());
            if (collection != null && collection.Count > 0)
            {
                SPListItem firstListItem = collection[0];
                PartnerPromotionEntity partnerPromotion = ListItemFieldMapper.CreateEntity(firstListItem);
                partnerPromotion.PromotionUrl = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}",
                                                              promotionsWebUrl, ListName, firstListItem["LinkFilename"]);

                return partnerPromotion;
            }

            return null;
        }

        public IList<PartnerPromotionEntity> GetAllMyPromos()
        {
            List<PartnerPromotionEntity> partnerPromotions = new List<PartnerPromotionEntity>();

            CAMLQueryBuilder camlQueryBuilder = new CAMLQueryBuilder();
            camlQueryBuilder.FilterByContentType(ContentTypeName);
            SPQuery spQuery = camlQueryBuilder.Build();
            SPListItemCollection collection = promotionsList.GetItems(spQuery);

            foreach (SPListItem item in collection)
            {
                PartnerPromotionEntity partnerPromotion = ListItemFieldMapper.CreateEntity(item);
                partnerPromotion.PromotionUrl = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}",
                                                              promotionsWebUrl, ListName, item["LinkFilename"]);
                partnerPromotions.Add(partnerPromotion);
            }

            return partnerPromotions;
        }

        #endregion

        private void Initialize(string siteUrl)
        {
            using (SPSite spSite = new SPSite(siteUrl))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    promotionsWebUrl = new Uri(spWeb.Url).AbsolutePath;
                    promotionsList = spWeb.Lists[ListName];
                }
            }
        }
    }
}