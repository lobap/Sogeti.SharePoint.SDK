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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls.WebParts;
using Contoso.Common.ExceptionHandling;
using Microsoft.SharePoint.WebPartPages;
using Contoso.PartnerPortal.ProductCatalog.Controls;

namespace Contoso.PartnerPortal.ProductCatalog.WebParts.ProductDetails
{
    [Guid("d2cc0f8d-0404-483a-b8bb-85bd1d2cd180")]
    public class ProductDetailsWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {
        #region Private Fields

        ProductDetailsControl productDetailsControl;
        string productSku;

        #endregion
        
        public ProductDetailsWebPart()
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
            }
        }

        [ConnectionConsumer("Product Sku Connection Consumer", "IFilterValueConnection" )]
        [CLSCompliant(false)]
        public void GetProductSkuFromFilterValues(IFilterValues filterValues)
        {
            List<ConsumerParameter> parameters = new List<ConsumerParameter>();
            parameters.Add(new ConsumerParameter("Sku", ConsumerParameterCapabilities.SupportsSingleValue | ConsumerParameterCapabilities.SupportsEmptyValue));
            filterValues.SetConsumerParameters(new ReadOnlyCollection<ConsumerParameter>(parameters));
            this.ProductSku = filterValues.ParameterValues[0];
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // Create a control that will display any errors that might occur in the ProductDetailsControl
            ErrorVisualizer errorVisualizer = new ErrorVisualizer();

            this.Controls.Add(errorVisualizer);
            // Add the ProductDetailsControl to the host. This way, if an error has to be rendered, the host can
            // prevent the ProductDetailsControl from being displayed. 
            this.productDetailsControl = (ProductDetailsControl)Page.LoadControl("~/_controltemplates/Contoso/ProductDetailsControl.ascx");
            this.productDetailsControl.ErrorVisualizer = errorVisualizer;
            errorVisualizer.Controls.Add(this.productDetailsControl); 
            this.productDetailsControl.LoadProduct(this.productSku);
        }
    }
}