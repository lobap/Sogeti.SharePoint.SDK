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
using System.Linq;
using Contoso.Common.BusinessEntities;
using Contoso.Common.ExceptionHandling;
using Contoso.Common.Repositories;
using Contoso.PartnerPortal.ProductCatalog.Controls;
using Microsoft.Practices.SPG.Common.ServiceLocation;

namespace Contoso.PartnerPortal.ProductCatalog.WebParts.Discounts
{
    /// <summary>
    /// Testable Logic for the <see cref="DiscountsControl"/>. 
    /// 
    /// This class is the Presenter in a Model View Presenter pattern. The presenter should hold as much of the UI logic as possible. 
    /// Also, the presenter should be testable in isloation. For example it only talks to the view through an interface. In a unit test, 
    /// the view can be replaced by a mock. 
    /// 
    /// The Discounts view & presenter shows one way to implement the Model-View-Presenter pattern, where the Presenter is an implementation 
    /// detail of the view. The advantage is, that the webpart doesn't need to know anything about the presenter. The downside is,
    /// that any data that the webpart wants to forward to the presenter, has to be forwarded through the view.
    /// 
    /// See the PartnerRollupWebPart for an example where the WebPart does know about the presenter. 
    /// </summary>
    public class DiscountsPresenter
    {
        private const string TheProductSKUWasNotDefined = "The product SKU was not defined";
        private const string NoDiscountsAvailableForSkuAndUser = "No discounts available for Sku: {0} and user: {1}";
        private readonly IDiscountsView discountsView;
        private readonly ViewExceptionHandler viewExceptionHandler;

        /// <summary>
        /// Create a DiscountPresenter.
        /// </summary>
        /// <param name="discountsView"></param>
        public DiscountsPresenter(IDiscountsView discountsView)
        {
            this.discountsView = discountsView;
            viewExceptionHandler = new ViewExceptionHandler();
        }

        /// <summary>
        /// Object that can show any technical errors that can occur. This class (a control) is created by the
        /// webpart, and forwarded by the view to the presenter. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        public IErrorVisualizer ErrorVisualizer
        {
            get;
            set;
        }

        /// <summary>
        /// Get and load the discounts into the view. 
        /// </summary>
        /// <param name="productSku"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This is an unhandled exception handler, where we need to catch all exceptions.")]
        public void LoadDiscounts(string productSku)
        {
            try
            {
                if (string.IsNullOrEmpty(productSku))
                {
                    this.viewExceptionHandler.ShowFunctionalErrorMessage(TheProductSKUWasNotDefined, this.ErrorVisualizer);
                    return;
                }

                IPricingRepository pricingRepository = SharePointServiceLocator.Current.GetInstance<IPricingRepository>();
                IEnumerable<Discount> discounts = pricingRepository.GetDiscountsBySku(productSku);

                if (!discounts.Any())
                {
                    ShowSkuNotFoundError(productSku);
                    return;
                }

                this.discountsView.Discounts = discounts;
                this.discountsView.DataBind();

            }
            catch (Exception exception)
            {
                // An unhandled exception has occurred. Make sure the exception is logged and
                // a technical error message is displayed to the user. Because no message is specified here
                // the default message is used. 
                viewExceptionHandler.HandleViewException(exception, this.ErrorVisualizer);
            }
        }

        private void ShowSkuNotFoundError(string productSku)
        {
            string errorMessage = string.Format(CultureInfo.CurrentCulture,
                                                NoDiscountsAvailableForSkuAndUser,
                                                productSku,
                                                System.Threading.Thread.CurrentPrincipal.Identity.Name);

            this.viewExceptionHandler.ShowFunctionalErrorMessage(errorMessage, this.ErrorVisualizer);
        }
    }
}
