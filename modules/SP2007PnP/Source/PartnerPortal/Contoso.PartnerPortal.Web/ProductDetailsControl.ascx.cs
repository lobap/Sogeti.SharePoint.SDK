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
using Contoso.Common.BusinessEntities;
using Contoso.Common.ExceptionHandling;
using Contoso.PartnerPortal.ProductCatalog.WebParts.ProductDetails;

namespace Contoso.PartnerPortal.ProductCatalog.Controls
{
    /// <summary>
    /// View that displays the ProductDetails for the ProductDetailsWebPart. The logic for this view can be
    /// found in the ProductDetailsPresenter and is designed in such a way that it can be unit tested in isolation. 
    /// 
    /// This View implements the IProductDetailsView so that the ProductDetailsPresenter can communicate with this view.
    /// </summary>
    public partial class ProductDetailsControl : System.Web.UI.UserControl, IProductDetailsView
    {
        private ProductDetailsPresenter presenter;

        /// <summary>
        /// Create an instance of the <see cref="ProductDetailsControl"/>
        /// </summary>
        public ProductDetailsControl()
        {
            // Create the presenter that hosts the logic for this view.
            this.presenter = new ProductDetailsPresenter(this);
        }

        #region IProductDetailsView Members

        /// <summary>
        /// The product that will be displayed 
        /// </summary>
        public Product Product
        {
            get;
            set;
        }

        /// <summary>
        /// The price that will be displayed by this view
        /// </summary>
        public Price Price
        {
            get;
            set;
        }

        /// <summary>
        /// Control that can display any unhandled (technical) errors that can occur in this control
        /// or in the presenter. 
        /// 
        /// In this case, it's forwarded to the presenter, so the presenter can display an error message if 
        /// something goes wrong while retrieving the product information. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        public IErrorVisualizer ErrorVisualizer
        {
            get { return this.presenter.ErrorVisualizer; }
            set { this.presenter.ErrorVisualizer = value; }
        }

        #endregion

        /// <summary>
        /// Display the information for a product with the specified information into the view.
        /// </summary>
        /// <param name="sku"></param>
        public void LoadProduct(string sku)
        {
            // The logic for this method is in the presenter, so that it can be tested. 
            this.presenter.LoadProduct(sku);
        }
    }
}