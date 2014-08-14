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
using Contoso.Common.BusinessEntities;
using Contoso.Common.ExceptionHandling;
using Contoso.Common.Repositories;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Contoso.PartnerPortal.ProductCatalog.Properties;

namespace Contoso.PartnerPortal.ProductCatalog.WebParts.ProductDetails
{
    /// <summary>
    /// Testable logic to display product details. 
    /// 
    /// This class follows the Model - View - Presenter pattern. See the DiscountsWebPart and the PartnerRollupWebPart for 
    /// more information about this pattern. 
    /// </summary>
    public class ProductDetailsPresenter
    {
        #region Private Fields

        private IProductDetailsView view;
        private ILogger logger;

        #endregion

        #region Constructors

        public ProductDetailsPresenter(IProductDetailsView view)
        {
            this.view = view;
            this.logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        public IErrorVisualizer ErrorVisualizer
        {
            get;
            set;
        }

        #endregion


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This is an unhandled exception handler, where we need to catch all exceptions.")]
        public void LoadProduct(string sku)
        {
            try
            {
                // Write a trace message, that might help in debugging a particular problem. Trace messages should be directed
                // towards developers, and can contain deep technical information. In contrast, Eventlog messages are targeted to operations
                // and should be more actionable. 
                logger.TraceToDeveloper(Resources.StartLoadingMessage);

                // Get the product catalog from the ServiceLocator. Because this presenter only knows about the IProductCatalog 
                // interface, it can be tested in isolation. In the unit tests, you'll see a MockProductDatalogRepository being used.
                // The actual implementation is provided by the Contoso.LOB.Services.Client project. In the it's WebAppFeatureReceiver
                // You'll see how the actual ProductCatalogRepository is registered with the ServiceLocatorConfig. 
                IProductCatalogRepository productCatalogRepository =
                    SharePointServiceLocator.Current.GetInstance<IProductCatalogRepository>();

                Product product = productCatalogRepository.GetProductBySku(sku);

                if (product == null || product.Sku == null)
                {
                    // Show an error message to the user. 
                    new ViewExceptionHandler().ShowFunctionalErrorMessage(Resources.CouldNotFindProductInformation,
                                                                          this.ErrorVisualizer);
                }
                else
                {
                    this.view.Product = product;
                    this.view.DataBind();
                }

                logger.TraceToDeveloper(Resources.EndLoadProductMessage
                    );
            }
            catch(Exception ex)
            {
                // If something goes wrong, make sure the error gets logged
                // and a non technical message is displayed to the user
                new ViewExceptionHandler().HandleViewException(ex, this.ErrorVisualizer,
                    Contoso.PartnerPortal.ProductCatalog.Properties.Resources.ProductDetailsErrorMessage);
            }
        }
    }
}