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
using System.Globalization;
using System.Security.Permissions;
using Contoso.Common.BusinessEntities;
using Contoso.Common.ExceptionHandling;
using Contoso.Common.Repositories;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Security;

namespace Contoso.PartnerPortal.ProductCatalog.Controls
{
    /// <summary>
    /// The View that visualizes the price for the PricingWebPart.
    /// </summary>
    public partial class PricingControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Value that will be displayed to show the price
        /// </summary>
        protected string PriceText
        {
            get; set;
        }

        /// <summary>
        /// Shows the price of a particular SKU. 
        /// 
        /// Note. We could have applied the Model View Presenter pattern here, but for simplicity, we kept this code int his class. 
        /// So this code could have been placed in a presenter, but for simplicity we put it directly in the view. 
        /// 
        /// See the DiscountsWebPart and the PartnerRollupWebPart for an example of the MVP pattern. 
        /// </summary>
        /// <param name="productSku">The SKU to display the product information for. </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This is an unhandled exception handler, where we need to catch all exceptions.")]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void ShowPricing(string productSku)
        {
            try
            {
                if (!string.IsNullOrEmpty(productSku))
                {
                    IPricingRepository pricingRepository = SharePointServiceLocator.Current.GetInstance<IPricingRepository>();
                    Price price = pricingRepository.GetPriceBySku(productSku);

                    if (price == null)
                        this.PriceText = "There is no price available for this product.";
                    else
                        this.PriceText = price.Value.ToString("C", CultureInfo.CurrentUICulture);
                }
                else
                {
                    PriceText = "The product has not been specified.";
                }
                this.DataBind();
            }
            catch (Exception ex)
            {
                // If an unknown exception occurs we want to:
                // 1. Log the error
                // 2. Display a friendly (Non technical) error message. 
                // The ViewExceptionHandler will do that for us:
                ViewExceptionHandler viewExceptionHandler = new ViewExceptionHandler();

                // In this example, we are looking for an error visualizer up in the tree and using that to display the error.
                // Find the error Visualizer (in this case, the one that was added by the PricingWebPart.cs:
                IErrorVisualizer errorVisualizer = ViewExceptionHandler.FindErrorVisualizer(this);

                // Now log the error and display a friendly error message using the error visualizer.
                viewExceptionHandler.HandleViewException(ex, errorVisualizer, string.Format(CultureInfo.CurrentUICulture, "Due to a technical problem, the pricing information for sku '{0}' could not be retrieved. Please try again later.", productSku));    
            }
        }
    }
}