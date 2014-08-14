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
using Contoso.PartnerPortal.ProductCatalog.Controls;
using Microsoft.SharePoint.WebPartPages;

namespace Contoso.PartnerPortal.ProductCatalog.WebParts.RelatedParts
{
    /// <summary>
    /// The RelatedPartsWebPart shows how to use the AJAX Update Panel to asynchronously update the UI in SharePoint. 
    /// </summary>
    [Guid("65a3de36-07df-49f1-b57b-516796077d75")]
    public class RelatedPartsWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {
        private string productSku = String.Empty;
        private RelatedPartsControl relatedPartsControl;

        [ConnectionConsumer("Product Sku Connection Consumer", "IFilterValueConnection")]
        [CLSCompliant(false)]
        public void GetProductSkuFromConnectionConsumer(IFilterValues filterValues)
        {
            if (!this.Page.IsPostBack)
            {
                List<ConsumerParameter> parameters = new List<ConsumerParameter> { new ConsumerParameter("Sku", ConsumerParameterCapabilities.SupportsSingleValue | ConsumerParameterCapabilities.SupportsEmptyValue) };
                filterValues.SetConsumerParameters(new ReadOnlyCollection<ConsumerParameter>(parameters));
                this.ProductSku = filterValues.ParameterValues[0];
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // Create the view that will show the related parts. 
            this.relatedPartsControl = (RelatedPartsControl) Page.LoadControl("~/_controltemplates/Contoso/RelatedPartsControl.ascx");
            

            // Create an error visualizer that will host the related parts control. 
            // Using this constructor will add it to the page. 
            IErrorVisualizer errorVisualizer = new ErrorVisualizer(this, this.relatedPartsControl);

            this.relatedPartsControl.ErrorVisualizer = errorVisualizer;
            this.relatedPartsControl.Sku = this.ProductSku;
        }

        [WebBrowsable(true)]
        [Personalizable(PersonalizationScope.Shared)]
        [WebDisplayName("Product SKU")]
        public string ProductSku
        {
            get
            {
                return this.productSku;
                //EnsureChildControls();
                //return this.relatedPartsControl.Sku;
            }
            set
            {
                this.productSku = value;
                //EnsureChildControls();
                //this.relatedPartsControl.Sku = value;
            }
        }
    }
}
