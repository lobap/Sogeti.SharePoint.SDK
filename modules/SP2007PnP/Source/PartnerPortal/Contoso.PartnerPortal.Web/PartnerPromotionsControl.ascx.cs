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

using Contoso.PartnerPortal.Promotions.WebParts.PartnerPromotions;
using Contoso.PartnerPortal.Promotions.Repositories;

namespace Contoso.PartnerPortal.Promotions.Controls
{
    public partial class PartnerPromotionsControl : System.Web.UI.UserControl, IPartnerPromotionsView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Repeater1.DataBind();
        }

        #region IPartnerPromotionsView Members

        public IEnumerable<PartnerPromotionEntity> PartnerPromotions
        {
            get
            {
                return this.Repeater1.DataSource as IEnumerable<PartnerPromotionEntity>;
            }
            set
            {
                this.Repeater1.DataSource = value;
            }
        }

        #endregion
    }
}