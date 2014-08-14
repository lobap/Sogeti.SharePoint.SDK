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
    /// This Test case verify the Partner central page.
    /// </summary>



    public class VerifyPartnerCentral : WebTest
    {
        string hostURL = CustConfig.GetHostURL;
       

        public VerifyPartnerCentral()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
         

            // Initialize validation rules that apply to all requests in the WebTest
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidateResponseUrl validationRule1 = new ValidateResponseUrl();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }

            WebTestRequest request1 = new WebTestRequest(hostURL + "/sites/PartnerCentral/default.aspx");
            request1.ThinkTime = 24;
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule2 = new ValidationRuleFindText();
                validationRule2.FindText = "Team Site";
                validationRule2.IgnoreCase = true;
                validationRule2.UseRegularExpression = false;
                validationRule2.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }
           
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule4 = new ValidationRuleFindText();
                validationRule4.FindText = "Open Tasks in System";
                validationRule4.IgnoreCase = true;
                validationRule4.UseRegularExpression = false;
                validationRule4.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule4.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule5 = new ValidationRuleFindText();
                validationRule5.FindText = "ContosoPartner1";
                validationRule5.IgnoreCase = true;
                validationRule5.UseRegularExpression = false;
                validationRule5.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule5.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule6 = new ValidationRuleFindText();
                validationRule6.FindText = "ContosoPartner2";
                validationRule6.IgnoreCase = true;
                validationRule6.UseRegularExpression = false;
                validationRule6.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule6.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule7 = new ValidationRuleFindText();
                validationRule7.FindText = "Builtin\\Users";
                validationRule7.IgnoreCase = true;
                validationRule7.UseRegularExpression = false;
                validationRule7.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule7.Validate);
            }
            yield return request1;
            request1 = null;

            WebTestRequest request2 = new WebTestRequest(hostURL + "/sites/PartnerCentral/_layouts/viewlsts.aspx");
            request2.QueryStringParameters.Add("ShowSites", "1", false, false);
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule8 = new ValidationRuleFindText();
                validationRule8.FindText = "PartnerDirectory";
                validationRule8.IgnoreCase = true;
                validationRule8.UseRegularExpression = false;
                validationRule8.PassIfTextFound = true;
                request2.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule8.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule9 = new ValidationRuleFindText();
                validationRule9.FindText = "SPGSUBSITE";
                validationRule9.IgnoreCase = true;
                validationRule9.UseRegularExpression = false;
                validationRule9.PassIfTextFound = true;
                request2.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule9.Validate);
            }
            yield return request2;
            request2 = null;
        }
    }
}
