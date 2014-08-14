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
using Contoso.Common.ExceptionHandling;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using Contoso.PartnerPortal.PartnerCentral.PartnerRollup;
using Contoso.PartnerPortal.PartnerCentral.Controls;

namespace Contoso.PartnerPortal.PartnerCentral.WebParts.PartnerRollup
{
    /// <summary>
    /// This class rolls up all the open incidents for for all partners. 
    /// 
    /// It demonstrates how to use Search, to find information in many different lists in many different locations. 
    /// 
    /// This class also shows a variation of the Model View Presenter pattern. In this variation, the WebPart knows about the presenter.
    /// The advantage of this is, that the webpart can talk directly to the presenter and doesn't have to forward information through 
    /// the View. The downside is, that the webpart now has to know about both the presenter AND the View. 
    /// 
    /// Another variation of the MVP pattern is explained in the DiscountsWebPart. 
    /// 
    /// </summary>
    [Guid("5bfc33be-50bd-4a8b-af3c-513ff0041dd3")]
    public class PartnerRollupWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {
        PartnerRollupPresenter presenter;

        public PartnerRollupWebPart()
        {
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            ErrorVisualizer errorVisualizer = new ErrorVisualizer();
            this.Controls.Add(errorVisualizer);

            // Create the View and also create the presetner and associate them together. 
            PartnerRollupControl control = (PartnerRollupControl)Page.LoadControl("~/_controltemplates/Contoso/PartnerRollupControl.ascx");
            errorVisualizer.Controls.Add(control);

            presenter = new PartnerRollupPresenter(control);

            // In this variation of the MVP pattern, the Webpart can talk directly to the presenter. 
            presenter.ReturnSearchResults();
        }
    }
}
