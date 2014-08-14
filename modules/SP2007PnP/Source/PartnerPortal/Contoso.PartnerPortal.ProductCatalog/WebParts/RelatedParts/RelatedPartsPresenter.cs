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
using System.Linq;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Contoso.Common.BusinessEntities;
using Contoso.Common.ExceptionHandling;
using Contoso.Common.Repositories;
using Contoso.PartnerPortal.ProductCatalog.Properties;

namespace Contoso.PartnerPortal.ProductCatalog.WebParts.RelatedParts
{

    /// <summary>
    /// This class holds the testable logic for the Related parts control. 
    /// 
    /// This class follows the Model - View - Presenter pattern. See the DiscountsWebPart and the PartnerRollupWebPart for 
    /// more information about this pattern. 
    /// </summary>
    public class RelatedPartsPresenter
    {
        #region Private Fields

        private readonly IRelatedPartsView view;

        #endregion

        public RelatedPartsPresenter(IRelatedPartsView view)
        {
            this.view = view;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        public IErrorVisualizer ErrorVisualizer{ get; set;}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This is an unhandled exception handler.")]
        public void LoadParts(string sku)
        {
            try
            {
                IProductCatalogRepository productCatalogRepository =
                    SharePointServiceLocator.Current.GetInstance<IProductCatalogRepository>();

                IEnumerable<Part> parts = productCatalogRepository.GetPartsByProductSku(sku);
                if (parts != null && parts.Any())
                {
                    view.Parts = parts;
                    
                }
                else
                {
                    // Show an errormessage in the view. Note, we couldn't use the ErrorVisualizer here, because
                    // the errorVisualizer is outside of the updatepanel. Only things inside the update panel of 
                    // the view will be refreshed when the LoadParts button is clicked. 
                    view.ErrorMessage = Resources.NoPartsFoundError;
                }
                view.DataBind();
            }
            catch (Exception ex)
            {
                // If an unhandled exception occurs in the view, then instruct the ErrorVisualizer to replace
                // the view with an errormessage. 
                ViewExceptionHandler viewExceptionHandler = new ViewExceptionHandler();
                viewExceptionHandler.HandleViewException(ex, ErrorVisualizer);
            }
        }
    }
}
