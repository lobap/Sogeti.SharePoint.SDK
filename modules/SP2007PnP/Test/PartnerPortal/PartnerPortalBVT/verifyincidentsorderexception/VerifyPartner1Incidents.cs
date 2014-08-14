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
    /// This Test verify the Incident 1 and Incident 2 in the Partner 1 Incidents site 
    /// </summary>


    public class Z_VerifyPartner1Incidents : WebTest
    {
        string hostURL = CustConfig.GetHostURL;


        public Z_VerifyPartner1Incidents()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {

            //Thread wait to complete workflow
            System.Threading.Thread.Sleep(20000);

            // Initialize validation rules that apply to all requests in the WebTest
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            {
                ValidateResponseUrl validationRule1 = new ValidateResponseUrl();
                this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }

            WebTestRequest request1 = new WebTestRequest(hostURL + "/sites/partner1/Pages/default.aspx");
            request1.ThinkTime = 3;
            yield return request1;
            request1 = null;

            WebTestRequest request2 = new WebTestRequest(hostURL + "/sites/partner1/_layouts/viewlsts.aspx");
            request2.ThinkTime = 2;
            request2.QueryStringParameters.Add("ShowSites", "1", false, false);
            yield return request2;
            request2 = null;

            WebTestRequest request3 = new WebTestRequest(hostURL + "/sites/partner1/incidents/");
            request3.ThinkTime = 3;
            request3.ExpectedResponseUrl = hostURL + "/sites/partner1/incidents/default.aspx";
            yield return request3;
            request3 = null;

            WebTestRequest request4 = new WebTestRequest(hostURL + "/sites/partner1/incidents/_layouts/viewlsts.aspx");
            request4.ThinkTime = 13;
            request4.QueryStringParameters.Add("ShowSites", "1", false, false);

            //Get guid for incident 1 subsite
            ExtractText extractionRule1 = new ExtractText();
            extractionRule1.StartsWith = "<A ID=\"webUrl\" HREF=\"" + hostURL + "/sites/partner1/incidents/";
            extractionRule1.EndsWith = "/\">Incident: 1</A>";
            extractionRule1.IgnoreCase = true;
            extractionRule1.UseRegularExpression = true;
            extractionRule1.Required = true;
            extractionRule1.Index = 0;
            extractionRule1.HtmlDecode = true;
            extractionRule1.ContextParameterName = "GuidIncident1";
            request4.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1.Extract);
            yield return request4;

            //store value in local veriable
            string GuidIncident1 = this.Context["GuidIncident1"].ToString();

            request4 = null;

            WebTestRequest request5 = new WebTestRequest(hostURL + "/sites/partner1/incidents/" + GuidIncident1 + "/");
            request5.ThinkTime = 8;
            request5.ExpectedResponseUrl = hostURL + "/sites/partner1/incidents/" + GuidIncident1 + "/default.aspx";
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule2 = new ValidationRuleFindText();
                validationRule2.FindText = "Incident: 1";
                validationRule2.IgnoreCase = true;
                validationRule2.UseRegularExpression = false;
                validationRule2.PassIfTextFound = true;
                request5.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule2.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule3 = new ValidationRuleFindText();
                validationRule3.FindText = "Incident Documents";
                validationRule3.IgnoreCase = true;
                validationRule3.UseRegularExpression = false;
                validationRule3.PassIfTextFound = true;
                request5.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule3.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule4 = new ValidationRuleFindText();
                validationRule4.FindText = "Incident Tasks";
                validationRule4.IgnoreCase = true;
                validationRule4.UseRegularExpression = false;
                validationRule4.PassIfTextFound = true;
                request5.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule4.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule5 = new ValidationRuleFindText();
                validationRule5.FindText = "Discussion Board";
                validationRule5.IgnoreCase = true;
                validationRule5.UseRegularExpression = false;
                validationRule5.PassIfTextFound = true;
                request5.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule5.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule6 = new ValidationRuleFindText();
                validationRule6.FindText = "IncidentInfo Web Part";
                validationRule6.IgnoreCase = true;
                validationRule6.UseRegularExpression = false;
                validationRule6.PassIfTextFound = true;
                request5.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule6.Validate);
            }
            yield return request5;
            request5 = null;

            WebTestRequest request6 = new WebTestRequest(hostURL + "/sites/partner1/incidents/_layouts/viewlsts.aspx");
            request6.ThinkTime = 20;
            request6.QueryStringParameters.Add("ShowSites", "1", false, false);

            //Get guid for incident 2 subsite
            ExtractText extractionRule2 = new ExtractText();
            extractionRule2.StartsWith = "<A ID=\"webUrl\" HREF=\"" + hostURL + "/sites/partner1/incidents/";
            extractionRule2.EndsWith = "/\">Incident: 2</A>";
            extractionRule2.IgnoreCase = true;
            extractionRule2.UseRegularExpression = true;
            extractionRule2.Required = true;
            extractionRule2.Index = 0;
            extractionRule2.HtmlDecode = true;
            extractionRule2.ContextParameterName = "GuidIncident2";
            request6.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule2.Extract);
            yield return request6;

            //store value in local veriable
            string GuidIncident2 = this.Context["GuidIncident2"].ToString();

            //extract guid if context parameter has other information
            if (GuidIncident2.IndexOf("/") > 0)
                GuidIncident2 = GuidIncident2.Substring(GuidIncident2.LastIndexOf("/") + 1);

            request6 = null;

            WebTestRequest request7 = new WebTestRequest(hostURL + "/sites/partner1/incidents/" + GuidIncident2 + "/");
            request7.ThinkTime = 25;
            request7.ExpectedResponseUrl = hostURL + "/sites/partner1/incidents/" + GuidIncident2 + "/default.aspx";
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule7 = new ValidationRuleFindText();
                validationRule7.FindText = "Incident: 2";
                validationRule7.IgnoreCase = true;
                validationRule7.UseRegularExpression = false;
                validationRule7.PassIfTextFound = true;
                request7.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule7.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule8 = new ValidationRuleFindText();
                validationRule8.FindText = "Incident Tasks";
                validationRule8.IgnoreCase = true;
                validationRule8.UseRegularExpression = false;
                validationRule8.PassIfTextFound = true;
                request7.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule8.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule9 = new ValidationRuleFindText();
                validationRule9.FindText = "Incident Documents";
                validationRule9.IgnoreCase = true;
                validationRule9.UseRegularExpression = false;
                validationRule9.PassIfTextFound = true;
                request7.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule9.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule10 = new ValidationRuleFindText();
                validationRule10.FindText = "Discussion Board";
                validationRule10.IgnoreCase = true;
                validationRule10.UseRegularExpression = false;
                validationRule10.PassIfTextFound = true;
                request7.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule10.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule11 = new ValidationRuleFindText();
                validationRule11.FindText = "IncidentInfo Web Part";
                validationRule11.IgnoreCase = true;
                validationRule11.UseRegularExpression = false;
                validationRule11.PassIfTextFound = true;
                request7.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule11.Validate);
            }
            yield return request7;
            request7 = null;


        }
    }
}
