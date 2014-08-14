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
    /// This Test verify the Partner 1 and Partner 2 Order Exception sites
    /// </summary>


    public class Z_VerifyOrderExceptionSitePartner1 : WebTest
    {
        string hostURL = CustConfig.GetHostURL;
        string winUserName = CustConfig.Partner1WinUserName;
        string winUserPassword = CustConfig.WINUserPassword;

        public Z_VerifyOrderExceptionSitePartner1()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            this.UserName = winUserName;
            this.Password = winUserPassword;

            //Thread wait to complete workflow
            System.Threading.Thread.Sleep(20000);

            // Initialize validation rules that apply to all requests in the WebTest
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidateResponseUrl validationRule1 = new ValidateResponseUrl();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }

            WebTestRequest request1 = new WebTestRequest(hostURL + "/sites/partner1/Pages/default.aspx");
            request1.ThinkTime = 10;
            yield return request1;
            request1 = null;

            WebTestRequest request2 = new WebTestRequest(hostURL + "/sites/partner1/_layouts/viewlsts.aspx");
            request2.ThinkTime = 3;
            request2.QueryStringParameters.Add("ShowSites", "1", false, false);
            yield return request2;
            request2 = null;

            WebTestRequest request3 = new WebTestRequest(hostURL + "/sites/partner1/orderexceptions/");
            request3.ThinkTime = 7;
            request3.ExpectedResponseUrl = hostURL + "/sites/partner1/orderexceptions/default.aspx";
            yield return request3;
            request3 = null;

            WebTestRequest request4 = new WebTestRequest(hostURL + "/sites/partner1/orderexceptions/_layouts/viewlsts.aspx");
            request4.ThinkTime = 22;
            request4.QueryStringParameters.Add("ShowSites", "1", false, false);

            // Get GUID of orderException Site
            ExtractText extractionRule1 = new ExtractText();
            extractionRule1.StartsWith = "<A ID=\"webIcon\" HREF=\"" + hostURL + "/sites/partner1/OrderExceptions/";
            extractionRule1.EndsWith = "/\">OrderException: cvbn23</A>";
            extractionRule1.IgnoreCase = true;
            extractionRule1.UseRegularExpression = true;
            extractionRule1.Required = true;
            extractionRule1.Index = 0;
            extractionRule1.HtmlDecode = true;
            extractionRule1.ContextParameterName = "GuidOrderExceptionPartner1";
            request4.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1.Extract);
            yield return request4;

            //store value in local variable
            string GuidOrderExceptionPartner1 = this.Context["GuidOrderExceptionPartner1"].ToString();

            if (GuidOrderExceptionPartner1.IndexOf("/") > 0)
                GuidOrderExceptionPartner1 = GuidOrderExceptionPartner1.Substring(GuidOrderExceptionPartner1.LastIndexOf("/") + 1);

            request4 = null;

            WebTestRequest request5 = new WebTestRequest(hostURL + "/sites/partner1/orderexceptions/" + GuidOrderExceptionPartner1 + "/");
            request5.ThinkTime = 19;
            request5.ExpectedResponseUrl = hostURL + "/sites/partner1/orderexceptions/" + GuidOrderExceptionPartner1 + "/default.aspx";
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule2 = new ValidationRuleFindText();
                validationRule2.FindText = "OrderException: cvbn23";
                validationRule2.IgnoreCase = true;
                validationRule2.UseRegularExpression = false;
                validationRule2.PassIfTextFound = true;
                request5.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule3 = new ValidationRuleFindText();
                validationRule3.FindText = "Discussion instance";
                validationRule3.IgnoreCase = true;
                validationRule3.UseRegularExpression = false;
                validationRule3.PassIfTextFound = true;
                request5.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule3.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule4 = new ValidationRuleFindText();
                validationRule4.FindText = "Tasks instance";
                validationRule4.IgnoreCase = true;
                validationRule4.UseRegularExpression = false;
                validationRule4.PassIfTextFound = true;
                request5.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule4.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule5 = new ValidationRuleFindText();
                validationRule5.FindText = "Documents instance";
                validationRule5.IgnoreCase = true;
                validationRule5.UseRegularExpression = false;
                validationRule5.PassIfTextFound = true;
                request5.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule5.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule6 = new ValidationRuleFindText();
                validationRule6.FindText = "Order Exception Info Web Part";
                validationRule6.IgnoreCase = true;
                validationRule6.UseRegularExpression = false;
                validationRule6.PassIfTextFound = true;
                request5.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule6.Validate);
            }
            yield return request5;
            request5 = null;

            
        }
    }
}
