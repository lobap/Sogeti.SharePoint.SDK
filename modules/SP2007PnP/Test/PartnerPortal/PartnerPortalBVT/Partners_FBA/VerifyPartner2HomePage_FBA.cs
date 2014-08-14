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
    /// This Test Case is browse partner2 site with FBA login and validate partner home page
    /// </summary>
    public class VerifyPartner2HomePage_FBA : WebTest
    {
        string fbaURL = CustConfig.GetFBAURL;
        string fbaUserName = CustConfig.Partner2FBAUserName;
        string fbaUserPassword = CustConfig.FBAUserPassword;
        public VerifyPartner2HomePage_FBA()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {

            WebTestRequest frequest1 = new WebTestRequest(fbaURL);
            frequest1.ExpectedResponseUrl = fbaURL + "/_layouts/login.aspx?ReturnUrl=%2fsites%2fpssportal";
            ExtractHiddenFields fextractionRule1 = new ExtractHiddenFields();
            fextractionRule1.Required = true;
            fextractionRule1.HtmlDecode = true;
            fextractionRule1.ContextParameterName = "1";
            frequest1.ExtractValues += new EventHandler<ExtractionEventArgs>(fextractionRule1.Extract);
            yield return frequest1;
            frequest1 = null;

            WebTestRequest frequest3 = new WebTestRequest(fbaURL + "/_layouts/login.aspx");
            frequest3.ThinkTime = 4;
            frequest3.Method = "POST";
            frequest3.ExpectedResponseUrl = fbaURL + "/Pages/default.aspx";
            frequest3.QueryStringParameters.Add("ReturnUrl", "%2f", false, false);
            FormPostHttpBody frequest3Body = new FormPostHttpBody();
            frequest3Body.FormPostParameters.Add("__LASTFOCUS", this.Context["$HIDDEN1.__LASTFOCUS"].ToString());
            frequest3Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            frequest3Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN1.__EVENTTARGET"].ToString());
            frequest3Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            frequest3Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            frequest3Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$UserName", fbaUserName);
            frequest3Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$password", fbaUserPassword);
            frequest3Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$login", "Sign In");
            frequest3Body.FormPostParameters.Add("__spDummyText1", "");
            frequest3Body.FormPostParameters.Add("__spDummyText2", "");
            frequest3.Body = frequest3Body;

            yield return frequest3;
            frequest3 = null;

          
            WebTestRequest request1 = new WebTestRequest(fbaURL  +"/sites/partner2");
            request1.ExpectedResponseUrl = fbaURL  +"/sites/partner2/Pages/default.aspx";

            ValidationRuleFindText validationRule1 = new ValidationRuleFindText();
            validationRule1.FindText = "Partner2";
            validationRule1.IgnoreCase = true;
            validationRule1.UseRegularExpression = true;
            validationRule1.PassIfTextFound = true;
            request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);

            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule2 = new ValidationRuleFindText();
                validationRule2.FindText = "Partner Home";
                validationRule2.IgnoreCase = true;
                validationRule2.UseRegularExpression = false;
                validationRule2.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule3 = new ValidationRuleFindText();
                validationRule3.FindText = "Manage Incidents";
                validationRule3.IgnoreCase = true;
                validationRule3.UseRegularExpression = false;
                validationRule3.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule3.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule4 = new ValidationRuleFindText();
                validationRule4.FindText = "Manage Order Exceptions";
                validationRule4.IgnoreCase = true;
                validationRule4.UseRegularExpression = false;
                validationRule4.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule4.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule5 = new ValidationRuleFindText();
                validationRule5.FindText = "Product Catalog";
                validationRule5.IgnoreCase = true;
                validationRule5.UseRegularExpression = false;
                validationRule5.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule5.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule6 = new ValidationRuleFindText();
                validationRule6.FindText = "Promotions";
                validationRule6.IgnoreCase = true;
                validationRule6.UseRegularExpression = false;
                validationRule6.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule6.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule7 = new ValidationRuleFindText();
                validationRule7.FindText = "default";
                validationRule7.IgnoreCase = true;
                validationRule7.UseRegularExpression = false;
                validationRule7.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule7.Validate);
            }
          
            yield return request1;
            request1 = null;
                      
         }
    }
}
