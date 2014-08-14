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
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using Contoso.Common.Repositories;
using Contoso.LOB.Services.Client.PricingProxy;
using Contoso.PartnerPortal.PartnerDirectory;
using Microsoft.Practices.SPG.Common.ServiceLocation;

// The types in the proxy have the same name as the types that are used by the app. Change the names of the proxy objects, to 
// avoid having to type the full namespace each time.  
using Discount=Contoso.Common.BusinessEntities.Discount;
using Price=Contoso.Common.BusinessEntities.Price;
using ProxyDiscount = Contoso.LOB.Services.Client.PricingProxy.Discount;
using ProxyPrice = Contoso.LOB.Services.Client.PricingProxy.Price;

namespace Contoso.LOB.Services.Client.Repositories
{
    /// <summary>
    /// Repository that retrieves pricing information from the pricing Web service. The pricing webservice requires that you pass in
    /// the partner Id to retrieve prices for a specific partner. This repository retrieves the current partner ID from the PartnerSiteDirectory.
    /// 
    /// This repository calls into the Pricing LOB Service. To do this, we've implemented the trusted facade pattern. The LOBService trusts SharePoint
    /// to provide the correct partnerID, and thus it will not do any authentication of the partner. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Exposed through the service locator.")]
    internal class PricingRepository : IPricingRepository
    {
        #region Private Fields

        private Cache cache;

        private string partnerId { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PricingRepository"/> class.
        /// </summary>
        public PricingRepository()
        {
            // Retrieve the PartnerID and Cache in the constructor. You have to do this here, because, if you want to use this
            // repository in a ListItemEventReceiver, you only have access to the context (spcontext, http context and current user)
            // in the constructor of the splistitemeventreceiver. 
            IPartnerSiteDirectory partnerSiteDirectory =
                SharePointServiceLocator.Current.GetInstance<IPartnerSiteDirectory>();
            partnerId = partnerSiteDirectory.GetCurrentPartnerId();
            cache = HttpContext.Current.Cache;
        }

        private DisposableProxy<IPricing> GetClient()
        {
            // Create a proxy that's easy to dispose. The problem with the IPricing interface is, that it doesn't implement 
            // IDisposable. This DisposableProxy helps with that. 
            PricingClient client = new Contoso.LOB.Services.Client.PricingProxy.PricingClient();

            // Use the PartnerID as the username. We're using the trusted facade pattern. The service trusts this SharePoint app, so 
            // it trusts us to pass in the correct partner information. 
            client.ClientCredentials.UserName.UserName = partnerId;

            // The methods calling GetClient() should have wrapped the call into a HostingEnvironment.Impersonate(). That causes the 
            // DefaultNetworkCredentials to contain the App Pool Account. Note, if you look at the DefaultNetworkCredentials in the debugger
            // the properties will be empty. However, the credentials will still get set. 
            client.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            return new DisposableProxy<IPricing>(client);
        }

        #endregion

        #region IPricingRepository Members

        /// <summary>
        /// Retrieve the price for a particular SKU that applies to the currently logged on user. This price includes all the discounts that have been
        /// applied to it for the current partner.
        /// </summary>
        /// <param name="sku">The SKU to retrieve pricing for.</param>
        /// <returns>The price.</returns>
        public Price GetPriceBySku(string sku)
        {
            Price price = null;

            //prices are stored per sku and partner
            IPartnerSiteDirectory partnerSiteDirectory =
                SharePointServiceLocator.Current.GetInstance<IPartnerSiteDirectory>();
            string key = string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", typeof (Price).FullName, sku,
                                       partnerSiteDirectory.GetCurrentPartnerId());

            if (cache[key] == null)
            {
                // Impersonate as the AppPool account: 
                    // As part of the trusted facade, we want to ensure that only the SharePoint server can access
                    // the WCF LOB services. In order to check that, we have added a PrincipalPermissionAttribute that demands
                    // that the calling account is part of a specific group. The app pool account is also part of that group
                    // so we need to impersonate as the app pool account. 
                using (HostingEnvironment.Impersonate())
                {
                    // Dispose of the proxy after usage, to ensure the resources are cleaned up:
                        // Creating proxy classes can potentially be a heavy operation, so you might consider
                        // creating a pool of proxy objects. 
                    using (DisposableProxy<IPricing> client = GetClient())
                    {
                        price = TransformProxyPriceToPrice(client.Proxy.GetPriceBySku(sku));
                    }
                }

                if (price != null)
                {
                    cache.Add(key, price, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5f),
                              CacheItemPriority.Normal, null);
                }
            }
            else
            {
                price = cache[key] as Price;
            }

            return price;
        }

        /// <summary>
        /// Get all the discounts that apply for this particular sku and for the currently logged on user.
        /// </summary>
        /// <param name="sku">The SKU to retrieve pricing information for.</param>
        /// <returns>The discounts that have been applied.</returns>
        public IEnumerable<Discount> GetDiscountsBySku(string sku)
        {
            IPartnerSiteDirectory partnerSiteDirectory =
                SharePointServiceLocator.Current.GetInstance<IPartnerSiteDirectory>();

            string key = string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", typeof (List<Discount>).FullName,
                                       sku, partnerSiteDirectory.GetCurrentPartnerId());

            List<Discount> discounts = cache[key] as List<Discount>;

            if (discounts == null)
            {
                discounts = new List<Discount>();

                // Impersonate as the AppPool account: 
                // As part of the trusted facade, we want to ensure that only the SharePoint server can access
                // the WCF LOB services. In order to check that, we have added a PrincipalPermissionAttribute that demands
                // that the calling account is part of a specific group. The app pool account is also part of that group
                // so we need to impersonate as the app pool account. 
                using (HostingEnvironment.Impersonate())
                {
                    // Dispose of the proxy after usage, to ensure the resources are cleaned up. 
                    // Creating proxy classes can potentially be a heavy operation, so you might consider
                    // creating a pool of proxy objects. 
                    using (DisposableProxy<IPricing> client = GetClient())
                    {
                        ProxyDiscount[] proxyDiscounts = client.Proxy.GetDiscountsBySku(sku);
                        foreach (ProxyDiscount proxyDiscount in proxyDiscounts)
                        {
                            discounts.Add(TransformProxyDiscountToDiscount(proxyDiscount));
                        }
                    }
                }

                cache.Add(key, discounts, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5f),
                          CacheItemPriority.Normal, null);
            }

            return discounts;
        }

        #endregion

        /// <summary>
        /// Transform a pricing object from the service into a pricing object that's used internally by the app.
        /// 
        /// This decouples the implementation of the services from the implementation of the application, so both can evolve seperately. 
        /// </summary>
        /// <param name="price">The price to create.</param>
        /// <returns>The created price. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private Price TransformProxyPriceToPrice(ProxyPrice price)
        {
            Price newPrice = new Price();
            newPrice.PartnerId = price.PartnerId;
            newPrice.ProductSku = price.ProductSku;
            newPrice.Value = price.Value;

            return newPrice;
        }

        /// <summary>
        /// Transform a discount object from the service into a discount object that's used by the application. 
        /// 
        /// This decouples the implementation of the services from the implementation of the application, so both can evolve seperately. 
        /// </summary>
        /// <param name="discount">The discount from the proxy</param>
        /// <returns>Discount object to be used by the app. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private Discount TransformProxyDiscountToDiscount(ProxyDiscount discount)
        {
            Discount newDiscount = new Discount();
            newDiscount.Id = discount.Id;
            newDiscount.Name = discount.Name;
            newDiscount.PartnerId = discount.PartnerId;
            newDiscount.ProductSku = discount.ProductSku;
            newDiscount.Value = discount.Value;

            return newDiscount;
        }
    }
}