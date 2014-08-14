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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contoso.Common.ExceptionHandling;
using Contoso.Common.Repositories;
using System.Text;
using Contoso.Common.BusinessEntities;
using Contoso.PartnerPortal.ProductCatalog.WebParts.Discounts;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;

namespace Contoso.PartnerPortal.ProductCatalog.Controls
{
    /// <summary>
    /// This is the View for the <see cref="DiscountsWebPart"/>. It displays a list of <see cref="Discount"/>s
    /// for a specified sku for the current user. 
    /// 
    /// This class is the View in a Model View Presenter pattern. The view is responsible for rendering the UI for the user
    /// but it should have as little logic as possible, because the view is very hard to unit test. 
    /// 
    /// The logic for this view can be found in the <see cref="DiscountsPresenter"/>, and is designed in such a 
    /// way to be Unit Testable. To that end, this view also implements the <see cref="IDiscountsView"/> 
    /// that the <see cref="DiscountsPresenter"/> uses. 
    /// 
    /// The Discounts view & presenter shows one way to implement the Model-View-Presenter pattern, where the Presenter is an implementation 
    /// detail of the view. The advantage is, that the webpart doesn't need to know anything about the presenter. The downside is,
    /// that any data that the webpart wants to forward to the presenter, has to be forwarded through the view.
    /// 
    /// See the PartnerRollupWebPart for an example where the WebPart does know about the presenter. 
    /// </summary>
    public partial class DiscountsControl : System.Web.UI.UserControl, IDiscountsView
    {
        private DiscountsPresenter discountsPresenter;

        public DiscountsControl()
        {
            this.discountsPresenter = new DiscountsPresenter(this);
        }

        /// <summary>
        /// Load the discounts for a particular sku and the current user.
        /// </summary>
        /// <param name="productSku">The sku to load</param>
        public void LoadDiscounts(string productSku)
        {
            // Create the presenter and display the discounts
            this.discountsPresenter.LoadDiscounts(productSku);
        }

        /// <summary>
        /// The Error Visualizer that can display technical errors. This value
        /// is immediately forwarded to the presenter, so it can use it to display
        /// exceptions as well. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        public IErrorVisualizer ErrorVisualizer
        {
            get { return this.discountsPresenter.ErrorVisualizer; }
            set { this.discountsPresenter.ErrorVisualizer = value; }
        }

        #region IDiscountsView Members

        /// <summary>
        /// The discounts to display in the view. This value will be forwared to the datasourcer of the discountsrepeater.
        /// </summary>
        public IEnumerable<Discount> Discounts
        {
            get
            {
                return this.DiscountsRepeater.DataSource as IEnumerable<Discount>;
            }
            set
            {
                this.DiscountsRepeater.DataSource = value;
            }
        }

        #endregion

    }
}