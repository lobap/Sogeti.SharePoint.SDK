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
    /// This Test verify the OrderExceptionSite for the Partner1 Extranet user
    /// step1 : Open Extranet Login Page
    /// step2 : Type User name as Contosopartner1user2 and Password P2ssw0rd$
    /// Step3 : Validate the OrderExceptionSite page.
    /// </summary>


    public class Z_VerifyExtranetOrderExceptionPartner1 : WebTest
    {

        string fbaURL = CustConfig.GetFBAURL;
        string fbaUserName = CustConfig.Partner1FBAUserName;
        string fbaUserPassword = CustConfig.FBAUserPassword;


        public Z_VerifyExtranetOrderExceptionPartner1()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
           //Thread wait to complete workflow
            System.Threading.Thread.Sleep(60000);
            // Initialize validation rules that apply to all requests in the WebTest
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidateResponseUrl validationRule1 = new ValidateResponseUrl();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }

            WebTestRequest request1 = new WebTestRequest(fbaURL + "/_layouts/login.aspx");
            request1.ThinkTime = 17;
            request1.QueryStringParameters.Add("ReturnUrl", "%2f", false, false);
            ExtractHiddenFields extractionRule1 = new ExtractHiddenFields();
            extractionRule1.Required = true;
            extractionRule1.HtmlDecode = true;
            extractionRule1.ContextParameterName = "1";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1.Extract);
            yield return request1;
            request1 = null;

            WebTestRequest request2 = new WebTestRequest(fbaURL + "/_layouts/login.aspx");
            request2.ThinkTime = 6;
            request2.Method = "POST";
            request2.ExpectedResponseUrl = fbaURL + "/sites/partner1/Pages/default.aspx";
            request2.QueryStringParameters.Add("ReturnUrl", "%2f", false, false);
            FormPostHttpBody request2Body = new FormPostHttpBody();
            request2Body.FormPostParameters.Add("__LASTFOCUS", this.Context["$HIDDEN1.__LASTFOCUS"].ToString());
            request2Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request2Body.FormPostParameters.Add("__EVENTTARGET", this.Context["$HIDDEN1.__EVENTTARGET"].ToString());
            request2Body.FormPostParameters.Add("__EVENTARGUMENT", this.Context["$HIDDEN1.__EVENTARGUMENT"].ToString());
            request2Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request2Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$UserName", fbaUserName);
            request2Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$password", fbaUserPassword);
            request2Body.FormPostParameters.Add("ctl00$PlaceHolderMain$login$login", "Sign In");
            request2Body.FormPostParameters.Add("__spDummyText1", "");
            request2Body.FormPostParameters.Add("__spDummyText2", "");
            request2.Body = request2Body;
            yield return request2;
            request2 = null;

            WebTestRequest request3 = new WebTestRequest(fbaURL + "/sites/partner1/_layouts/viewlsts.aspx");
            request3.ThinkTime = 3;
            request3.QueryStringParameters.Add("ShowSites", "1", false, false);
            yield return request3;
            request3 = null;

            WebTestRequest request4 = new WebTestRequest(fbaURL + "/sites/partner1/orderexceptions/");
            request4.ThinkTime = 3;
            request4.ExpectedResponseUrl = fbaURL + "/sites/partner1/orderexceptions/default.aspx";
            yield return request4;
            request4 = null;

            WebTestRequest request5 = new WebTestRequest(fbaURL + "/sites/partner1/orderexceptions/_layouts/viewlsts.aspx");
            request5.ThinkTime = 5;

            // Get GUID of orderException Site
            request5.QueryStringParameters.Add("ShowSites", "1", false, false);
            ExtractText extractionRule2 = new ExtractText();
            extractionRule2.StartsWith = "<A ID=\"webIcon\" HREF=\"" + fbaURL + "/sites/partner1/OrderExceptions/";
            extractionRule2.EndsWith = "/\">OrderException: cvbn23</A>";
            extractionRule2.IgnoreCase = true;
            extractionRule2.UseRegularExpression = true;
            extractionRule2.Required = true;
            extractionRule2.Index = 0;
            extractionRule2.HtmlDecode = true;
            extractionRule2.ContextParameterName = "GuidOrderExceptionPartner1";
            request5.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule2.Extract);
            yield return request5;

            //store value in local variable
            string GuidOrderExceptionPartner1 = this.Context["GuidOrderExceptionPartner1"].ToString();

            if (GuidOrderExceptionPartner1.IndexOf("/") > 0)
                GuidOrderExceptionPartner1 = GuidOrderExceptionPartner1.Substring(GuidOrderExceptionPartner1.LastIndexOf("/") + 1);

            request5 = null;

            WebTestRequest request6 = new WebTestRequest(fbaURL + "/sites/partner1/orderexceptions/" + GuidOrderExceptionPartner1 + "/");
            request6.ExpectedResponseUrl = fbaURL + "/sites/partner1/orderexceptions/" + GuidOrderExceptionPartner1 + "/default.aspx";
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule2 = new ValidationRuleFindText();
                validationRule2.FindText = "OrderException: cvbn23";
                validationRule2.IgnoreCase = true;
                validationRule2.UseRegularExpression = false;
                validationRule2.PassIfTextFound = true;
                request6.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule3 = new ValidationRuleFindText();
                validationRule3.FindText = "Discussion instance";
                validationRule3.IgnoreCase = true;
                validationRule3.UseRegularExpression = false;
                validationRule3.PassIfTextFound = true;
                request6.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule3.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule4 = new ValidationRuleFindText();
                validationRule4.FindText = "Tasks instance";
                validationRule4.IgnoreCase = true;
                validationRule4.UseRegularExpression = false;
                validationRule4.PassIfTextFound = true;
                request6.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule4.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule5 = new ValidationRuleFindText();
                validationRule5.FindText = "Documents instance";
                validationRule5.IgnoreCase = true;
                validationRule5.UseRegularExpression = false;
                validationRule5.PassIfTextFound = true;
                request6.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule5.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule6 = new ValidationRuleFindText();
                validationRule6.FindText = "Order Exception Info Web Part";
                validationRule6.IgnoreCase = true;
                validationRule6.UseRegularExpression = false;
                validationRule6.PassIfTextFound = true;
                request6.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule6.Validate);
            }
            yield return request6;
            request6 = null;
        }
    }
}
