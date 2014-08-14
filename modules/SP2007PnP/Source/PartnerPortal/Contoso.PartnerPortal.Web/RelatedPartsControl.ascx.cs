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
using Contoso.Common.BusinessEntities;
using Contoso.Common.ExceptionHandling;
using Contoso.PartnerPortal.ProductCatalog.WebParts.RelatedParts;

namespace Contoso.PartnerPortal.ProductCatalog.Controls
{
    /// <summary>
    /// This class implements the View for the Related Parts WebPart.
    /// 
    /// This class follows the Model - View - Presenter pattern. See the DiscountsWebPart and the PartnerRollupWebPart for 
    /// more information about this pattern. 
    /// </summary>
    public partial class RelatedPartsControl : System.Web.UI.UserControl , IRelatedPartsView
    {
        private RelatedPartsPresenter presenter;

        public RelatedPartsControl()
        {
            presenter = new RelatedPartsPresenter(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        public IErrorVisualizer ErrorVisualizer
        {
            get { return this.presenter.ErrorVisualizer; }
            set { this.presenter.ErrorVisualizer = value;}
        }

        public IEnumerable<Part> Parts
        {
            get
            {
                return this.PartsRepeater as IEnumerable<Part>;
            }
            set
            {
                this.PartsRepeater.DataSource = value;
            }
        }

        /// <summary>
        /// The product SKU is stored in the ViewState. The reason for this is, that the <see cref="LoadPartsButton_Click"/> event
        /// fires before the SKU is set by SharePoint. By setting this value in the ViewState, we don't have to wait for SharePoint
        /// to set this property. 
        /// </summary>
        public string Sku
        {
            get
            {
                return this.ViewState["ProductSKU"] as string;
            }
            set
            {
                this.ViewState["ProductSKU"] = value;
            }
        }

        /// <summary>
        /// Errormessage, that will be shown. This value is set by the Presenter. 
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// This event will fire asynchronously if the user clicks the LoadParts button. Forward this click to the
        /// presenter. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadPartsButton_Click(object sender, EventArgs e)
        {
            this.presenter.LoadParts(this.Sku);
        }
    }
}