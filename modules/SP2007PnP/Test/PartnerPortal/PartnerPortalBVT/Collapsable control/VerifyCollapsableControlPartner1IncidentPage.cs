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

namespace PartnerPortalBVT
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;
    using PartnerPortalBVT.Settings;

    /// <summary>
    /// This Test verify the text in the Collapsable control in the Partner1 Incident page
    /// </summary>


    public class VerifyCollapsableControlPartner1IncidentPage : WebTest
    {
        string hostURL = CustConfig.GetHostURL;
        string winUserPassword = CustConfig.WINUserPassword;

        public VerifyCollapsableControlPartner1IncidentPage()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            this.UserName = System.Environment.MachineName + "\\ContosoPartner1User6";
            this.Password = winUserPassword;

            // Initialize validation rules that apply to all requests in the WebTest
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidateResponseUrl validationRule1 = new ValidateResponseUrl();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }

            WebTestRequest request1 = new WebTestRequest(hostURL + "/sites/partner1/incidents/default.aspx");
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule2 = new ValidationRuleFindText();
                validationRule2.FindText = @"Active Incident Task is a Content Query Web Part that shows all incident tasks whose task status is not complete.";
                validationRule2.IgnoreCase = true;
                validationRule2.UseRegularExpression = false;
                validationRule2.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }

            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule3 = new ValidationRuleFindText();
                validationRule3.FindText = @"The Incident Status List Web Part is a custom Web Part that uses the PortalSiteMapProvider class to also query for Incident Tasks. This Web Part shows status, created date, and last modified date. These fields not available to the Content Query Web Part.";
                validationRule3.IgnoreCase = true;
                validationRule3.UseRegularExpression = false;
                validationRule3.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule3.Validate);
            }

            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule31 = new ValidationRuleFindText();
                validationRule31.FindText = @"Both the Content Query Web Part and PortalSiteMapProvider make use the Microsoft Office SharePoint Server 2007 object cache.";
                validationRule31.IgnoreCase = true;
                validationRule31.UseRegularExpression = false;
                validationRule31.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule31.Validate);
            }
            yield return request1;
            request1 = null;
        }
    }
}
