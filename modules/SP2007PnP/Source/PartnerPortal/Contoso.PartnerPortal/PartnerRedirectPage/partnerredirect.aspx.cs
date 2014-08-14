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
using System.Text;
using System.Web.UI;
using System.Threading;
using System.Web;
using System.Data;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SPG.Common.ServiceLocation;

using Contoso.Common;
using Contoso.PartnerPortal.PartnerDirectory;
using Microsoft.Practices.SPG.SubSiteCreation;

namespace Contoso.PartnerPortal
{
    public class PartnerRedirectPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string queryParamValue = HttpContext.Current.Request.QueryString["page"];

            PartnerRedirectController controller = new PartnerRedirectController();
            controller.Redirect(queryParamValue);
        }
    }
}