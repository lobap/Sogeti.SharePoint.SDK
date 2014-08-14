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
using Microsoft.SharePoint.WebPartPages;
using Contoso.PartnerPortal.ProductCatalog.Controls;

namespace Contoso.PartnerPortal.ProductCatalog.WebParts.Discounts
{
    /// <summary>
    /// The Discounts WebPart displays a list of discounts for a particular product/user
    /// 
    /// This webpart shows how to implement the Model View Presenter pattern within a webpart. 
    /// 
    /// The Discounts view & presenter shows one way to implement the Model-View-Presenter pattern, where the Presenter is an implementation 
    /// detail of the view. The advantage is, that the webpart doesn't need to know anything about the presenter. The downside is,
    /// that any data that the webpart wants to forward to the presenter, has to be forwarded through the view.
    /// 
    /// See the PartnerRollupWebPart for an example where the WebPart does know about the presenter. 
    /// </summary>
    [Guid("6a6228d9-7008-4efc-83f4-303def7a57af")]
    public class DiscountsWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {
        private DiscountsControl discountsControl;

        private string productSku;

        /// <summary>
        ///  Set the Product SKU. 
        /// </summary>
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
            }
        }

        /// <summary>
        /// Get the product SKU from the filter values. This method will be called by SharePoint if this webpart
        /// is connected to other webparts.
        /// </summary>
        /// <param name="filterValues"></param>
        [ConnectionConsumer("Product Connection Consumer")]
        public void GetProductSkuFromFilterValues(IFilterValues filterValues)
        {
            List<ConsumerParameter> parameters = new List<ConsumerParameter> { new ConsumerParameter("Sku", ConsumerParameterCapabilities.SupportsSingleValue | ConsumerParameterCapabilities.SupportsEmptyValue) };
            filterValues.SetConsumerParameters(new ReadOnlyCollection<ConsumerParameter>(parameters));
            this.ProductSku = filterValues.ParameterValues[0];
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // Create the View (an user control), that will display the list of discounts to the user
            this.discountsControl = (DiscountsControl) this.Page.LoadControl("~/_controltemplates/Contoso/DiscountsControl.ascx");

            // Add the View to an ErrorVisualizer. The ErrorVisualizer can display (technical) error messages 
            // when an unhandled exception occurs in the view. If an error message is to be displayed, the 
            // View itself will not be rendered; only the error message. 
            ErrorVisualizer errorVisualizer = new ErrorVisualizer(this, discountsControl);

            // In this case, we are forwarding the ErrorVisualizer explicitly to the view, to make
            // the error handling more robust. It is also possible for the view to look up the 
            // control tree to find the visualizer. That approach (demonstrated in the PricingWebPart)
            // requires less code, but also implies an implicit contract between the webpart and the view.
            this.discountsControl.ErrorVisualizer = errorVisualizer;
            this.discountsControl.LoadDiscounts(this.productSku);
        }
    }
}
