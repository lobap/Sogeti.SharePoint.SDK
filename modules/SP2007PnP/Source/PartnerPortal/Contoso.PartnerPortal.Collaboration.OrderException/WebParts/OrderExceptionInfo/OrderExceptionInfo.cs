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

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

using System.Text;

namespace Contoso.PartnerPortal.Collaboration.OrderException.WebParts
{
    [Guid("610fc132-c155-49f4-8bcb-413aedde0735")]
    public class OrderExceptionInfo : System.Web.UI.WebControls.WebParts.WebPart
    {
        public OrderExceptionInfo()
        {
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // The OrderExceptionInfo is a simple Web part that displays some static information.
            // It's sole purpose is to demonstrate the ability to use different site definitions/site templates
            // for the subsite creation pattern. For a LOB connected Web part, please see the Incident Info Web Part.
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("<H1>Order Exception Info</H1>");
            contentBuilder.AppendLine("<H3>Order Exception #13415</H3>");
            contentBuilder.AppendLine("<H3>Super Magnet Supply Shortage</H3>");
            contentBuilder.AppendLine("<p>There is an uxpected shortage in new super magnets, and may cause delays in shipments to partner 1</p>");
            
            LiteralControl content = new LiteralControl();
            content.Text = contentBuilder.ToString();

            this.Controls.Add(content);
        }
    }
}
