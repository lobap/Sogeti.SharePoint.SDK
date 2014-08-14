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
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;
using System.Data;
using System.Web;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

using Contoso.PartnerPortal.Promotions.Controls;

namespace Contoso.PartnerPortal.Promotions.WebParts.PartnerPromotions
{
    [Guid("6058a309-fda6-4675-9b8e-f485531c47a4")]
    public class PartnerPromotionsWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            PartnerPromotionsControl control = (PartnerPromotionsControl)Page.LoadControl("~/_controltemplates/Contoso/PartnerPromotionsControl.ascx");
            PartnerPromotionsPresenter presenter = new PartnerPromotionsPresenter();
            control.PartnerPromotions = presenter.FindPromotionPages();

            this.Controls.Add(control);
        }
    }
}
