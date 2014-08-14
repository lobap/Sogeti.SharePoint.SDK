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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls.WebParts;
using Contoso.Common.ExceptionHandling;
using Contoso.PartnerPortal.ProductCatalog.Controls;
using Microsoft.SharePoint.WebPartPages;

namespace Contoso.PartnerPortal.ProductCatalog.WebParts.Pricing
{
    /// <summary>
    /// The pricing webpart shows the price for a particular product. 
    /// </summary>
    [Guid("7a154334-274a-4e0a-8dd1-422a81593448")]
    public class PricingWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {
        private PricingControl pricingControl;
        private string productSku;

        public PricingWebPart()
        {
        }

        [WebBrowsable(true)]
        [Personalizable(PersonalizationScope.Shared)]
        [WebDisplayName("Product Sku")]
        public string ProductSku
        {
            get
            {
                return productSku;
            }
            set
            {
                productSku = value;

                //// The SKU property can either be set directly or through a webpart connection. 
                //// By using this pattern here, we make sure that the correct price is always displayed. 
                //EnsureChildControls();
                //this.pricingControl.ShowPricing(productSku);
            }
        }

        [ConnectionConsumer("Product Connection Consumer")]
        public void GetProductSkuFromConnectionProvider(IFilterValues filterValues)
        {
            List<ConsumerParameter> parameters = new List<ConsumerParameter> { new ConsumerParameter("Sku", ConsumerParameterCapabilities.SupportsSingleValue | ConsumerParameterCapabilities.SupportsEmptyValue) };
            filterValues.SetConsumerParameters(new ReadOnlyCollection<ConsumerParameter>(parameters));
            this.ProductSku = filterValues.ParameterValues[0];
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            ErrorVisualizer errorVisualizer = new ErrorVisualizer();
            this.Controls.Add(errorVisualizer);

            this.pricingControl = (PricingControl)Page.LoadControl("~/_controltemplates/Contoso/PricingControl.ascx");
            errorVisualizer.Controls.Add(this.pricingControl);
            this.pricingControl.ShowPricing(productSku);
        }
    }
}