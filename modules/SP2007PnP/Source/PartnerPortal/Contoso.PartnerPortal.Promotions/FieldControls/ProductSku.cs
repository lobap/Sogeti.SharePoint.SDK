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
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web;
using Contoso.Common.Repositories;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.ApplicationRegistry.MetadataModel;
using Microsoft.Office.Server.ApplicationRegistry.Runtime;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Contoso.PartnerPortal.Promotions.Properties;

namespace Contoso.PartnerPortal.Promotions.FieldControls
{
    [CLSCompliant(false)]
    [Guid("1f5b41da-a7f5-4ee2-bdea-4fa9943c5786")]
    public class ProductSku : TextField
    {
        private const string ProductSkuRegEx = @"[0-9A-Z]{10}";
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel=true)]
        [AspNetHostingPermission(SecurityAction.LinkDemand, Level=AspNetHostingPermissionLevel.Minimal)]
        public override void Validate()
        {
            base.Validate();

            if (this.IsValid)
            {
                string sku = (string)this.Value;

                // 1. Check to make sure the product sku is not empty
                if (string.IsNullOrEmpty(sku))
                {
                    this.IsValid = false;
                    this.ErrorMessage = Resources.ProductCanNotBeEmpty;
                    return;
                }
                else
                {
                    // 2. Validate that the product sku is in the right format
                    RegexStringValidator regexStringValidator = new RegexStringValidator(ProductSkuRegEx);
                    try
                    {
                        regexStringValidator.Validate(sku);
                    }
                    catch (ArgumentException)
                    {
                        this.IsValid = false;
                        this.ErrorMessage = Resources.InvalidProductSkuFormat;
                        return;
                    }

                    // 3. Validate that the product sku exists in the product catalog system
                    IProductCatalogRepository productCatalogRepository =
                        SharePointServiceLocator.Current.GetInstance<IProductCatalogRepository>();
                    
                    if (productCatalogRepository.GetProductBySku(sku) == null)
                    {
                        this.IsValid = false;
                        this.ErrorMessage = Resources.ProductNotFound;
                        return;
                    }
                }
            }
        }
    }
}
