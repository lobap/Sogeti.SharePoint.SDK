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
using System.ServiceModel.Activation;
using System.Web.Configuration;
using Contoso.LOB.Services.BusinessEntities;
using Contoso.LOB.Services.Contracts;
using Contoso.LOB.Services.Data;
using Contoso.LOB.Services.Security;
using System.Threading;
using System.Globalization;

namespace Contoso.LOB.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Pricing : IPricing
    {
        #region Private Fields

        private PricingDataSet pricingDataSet = PricingDataSet.Instance;

        #endregion

        #region Constructors

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public Pricing()
        {
            this.SecurityHelper = new SecurityHelper();
        }

        #endregion

        #region IPricing Members

        public Price GetPriceBySku(string sku)
        {
            this.SecurityHelper.DemandAuthorizedPermissions();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get("sleepAmount"), CultureInfo.CurrentCulture));

            Price price = null;
            PricingDataSet.PricingRow[] match = this.pricingDataSet.Pricing.Select(string.Format(CultureInfo.CurrentCulture, "ProductSku = '{0}'", sku)) as PricingDataSet.PricingRow[];
            if (match.Length > 0)
            {
                price = new Price();
                price.PartnerId = this.SecurityHelper.GetPartnerId(); 
                price.ProductSku = sku;
                price.Value = CalculatePrice(match[0].Price, sku);
            }

            return price;
        }

        private decimal CalculatePrice(decimal originalPrice, string sku)
        {
            decimal price = originalPrice;

            IEnumerable<Discount> discounts = GetDiscountsBySku(sku);
            {
                foreach(Discount discount in discounts)
                {
                    price = price - (price/100*discount.Value);
                }
            }
            return price;
        }

        public Discount GetDiscountById(string id)
        {
            SecurityHelper.DemandAuthorizedPermissions();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get("sleepAmount"), CultureInfo.CurrentCulture));

            Discount discount = null;
            PricingDataSet.DiscountRow[] match = this.pricingDataSet.Discount.Select(string.Format(CultureInfo.CurrentCulture, "Id = {0}", id)) as PricingDataSet.DiscountRow[];
            
            if (match.Length > 0)
            {
                discount = CreateDiscount(match[0]);
            }

            return discount;
        }

        private static Discount CreateDiscount(PricingDataSet.DiscountRow discountRow)
        {
            Discount discount = new Discount();
            discount.Id = discountRow.Id.ToString(CultureInfo.CurrentCulture);
            discount.PartnerId = discountRow.PartnerId;
            discount.ProductSku = discountRow.ProductSku;
            discount.Name = discountRow.Name;
            discount.Value = discountRow.Value;

            return discount;
        }

        public Discount GetDiscountByName(string name)
        {
            SecurityHelper.DemandAuthorizedPermissions();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get("sleepAmount"), CultureInfo.CurrentCulture));

            Discount discount = null;
            PricingDataSet.DiscountRow[] match = this.pricingDataSet.Discount.Select(string.Format(CultureInfo.CurrentCulture, "Name = '{0}'", name)) as PricingDataSet.DiscountRow[];
            if (match.Length > 0)
            {
                discount = CreateDiscount(match[0]);
            }

            return discount;
        }

        public IList<Discount> GetDiscountsBySku(string sku)
        {
            SecurityHelper.DemandAuthorizedPermissions();

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get("sleepAmount"), CultureInfo.CurrentCulture));

            List<Discount> discounts = new List<Discount>();

            string query = string.Format(CultureInfo.CurrentCulture, "ProductSku = '{0}' AND PartnerId = '{1}'", sku, this.SecurityHelper.GetPartnerId());
            PricingDataSet.DiscountRow[] match = this.pricingDataSet.Discount.Select(query) as PricingDataSet.DiscountRow[];
            foreach (PricingDataSet.DiscountRow row in match)
            {
                discounts.Add(CreateDiscount(row));
            }

            return discounts;
        }

        #endregion
        
        protected ISecurityHelper SecurityHelper { get; set; }
    }
}
