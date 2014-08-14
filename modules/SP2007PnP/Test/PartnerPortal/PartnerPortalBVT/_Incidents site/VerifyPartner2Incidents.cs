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

namespace PSSBVT
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;
    using PartnerPortalBVT.Settings;

    /// <summary>
    /// This Test verify the Incident 3 and Incident 4 in the Partner 2 Incidents site 
    /// </summary>


    public class VerifyPartner2Incidents : WebTest
    {
        string hostURL = CustConfig.GetHostURL;
        string winUserPassword = CustConfig.WINUserPassword;

        public VerifyPartner2Incidents()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            this.UserName = System.Environment.MachineName + "\\ContosoPartner2User6";
            this.Password = winUserPassword;

            // Initialize validation rules that apply to all requests in the WebTest
            //if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.Low))
            //{
            //    ValidateResponseUrl validationRule1 = new ValidateResponseUrl();
            //    this.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            //}

            WebTestRequest request8 = new WebTestRequest(hostURL + "/sites/partner2/Pages/default.aspx");
            request8.ThinkTime = 3;
            yield return request8;
            request8 = null;

            WebTestRequest request9 = new WebTestRequest(hostURL + "/sites/partner2/_layouts/viewlsts.aspx");
            request9.ThinkTime = 2;
            request9.QueryStringParameters.Add("ShowSites", "1", false, false);
            yield return request9;
            request9 = null;

            WebTestRequest request10 = new WebTestRequest(hostURL + "/sites/Partner2/incidents/");
            request10.ThinkTime = 2;
            request10.ExpectedResponseUrl = hostURL + "/sites/Partner2/incidents/default.aspx";
            yield return request10;
            request10 = null;

            WebTestRequest request11 = new WebTestRequest(hostURL + "/sites/partner2/incidents/_layouts/viewlsts.aspx");
            request11.ThinkTime = 24;
            request11.QueryStringParameters.Add("ShowSites", "1", false, false);

            //Get guid for incident 3 subsite

            ExtractText extractionRule3 = new ExtractText();
            extractionRule3.StartsWith = "<A ID=\"webUrl\" HREF=\"" + hostURL + "/sites/partner2/incidents/";
            extractionRule3.EndsWith = "/\">Incident: 3</A>";
            extractionRule3.IgnoreCase = true;
            extractionRule3.UseRegularExpression = true;
            extractionRule3.Required = true;
            //extractionRule3.ExtractRandomMatch = false;
            extractionRule3.Index = 0;
            extractionRule3.HtmlDecode = true;
            extractionRule3.ContextParameterName = "GuidIncident3";
            request11.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule3.Extract);
            yield return request11;

            //store value in local variable
            string GuidIncident3 = this.Context["GuidIncident3"].ToString();

            //extract guid if context param has other info
            if (GuidIncident3.IndexOf("/") > 0)
                GuidIncident3 = GuidIncident3.Substring(GuidIncident3.LastIndexOf("/") + 1);
            request11 = null;

            WebTestRequest request12 = new WebTestRequest(hostURL + "/sites/partner2/incidents/" + GuidIncident3 + "/");
            request12.ThinkTime = 9;
            request12.ExpectedResponseUrl = hostURL + "/sites/partner2/incidents/" + GuidIncident3 + "/default.aspx";
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule12 = new ValidationRuleFindText();
                validationRule12.FindText = "Incident: 3";
                validationRule12.IgnoreCase = true;
                validationRule12.UseRegularExpression = false;
                validationRule12.PassIfTextFound = true;
                request12.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule12.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule13 = new ValidationRuleFindText();
                validationRule13.FindText = "Incident Tasks";
                validationRule13.IgnoreCase = true;
                validationRule13.UseRegularExpression = false;
                validationRule13.PassIfTextFound = true;
                request12.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule13.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule14 = new ValidationRuleFindText();
                validationRule14.FindText = "Incident Documents";
                validationRule14.IgnoreCase = true;
                validationRule14.UseRegularExpression = false;
                validationRule14.PassIfTextFound = true;
                request12.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule14.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule15 = new ValidationRuleFindText();
                validationRule15.FindText = "Discussion Board";
                validationRule15.IgnoreCase = true;
                validationRule15.UseRegularExpression = false;
                validationRule15.PassIfTextFound = true;
                request12.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule15.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule16 = new ValidationRuleFindText();
                validationRule16.FindText = "IncidentInfo Web Part";
                validationRule16.IgnoreCase = true;
                validationRule16.UseRegularExpression = false;
                validationRule16.PassIfTextFound = true;
                request12.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule16.Validate);
            }
            yield return request12;
            request12 = null;

            WebTestRequest request13 = new WebTestRequest(hostURL + "/sites/partner2/incidents/_layouts/viewlsts.aspx");
            request13.ThinkTime = 6;
            request13.QueryStringParameters.Add("ShowSites", "1", false, false);

            //Get guid for incident 4 subsite
            ExtractText extractionRule4 = new ExtractText();
            extractionRule4.StartsWith = "<A ID=\"webUrl\" HREF=\"" + hostURL + "/sites/partner2/incidents/";
            extractionRule4.EndsWith = "/\">Incident: 4</A>";
            extractionRule4.IgnoreCase = true;
            extractionRule4.UseRegularExpression = true;
            extractionRule4.Required = true;
            //extractionRule4.ExtractRandomMatch = false;
            extractionRule4.Index = 0;
            extractionRule4.HtmlDecode = true;
            extractionRule4.ContextParameterName = "GuidIncident4";
            request13.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule4.Extract);
            yield return request13;

            //store value in local variable
            string GuidIncident4 = this.Context["GuidIncident4"].ToString();


            if (GuidIncident4.IndexOf("/") > 0)
                GuidIncident4 = GuidIncident4.Substring(GuidIncident4.LastIndexOf("/") + 1);

            request13 = null;

            WebTestRequest request14 = new WebTestRequest(hostURL + "/sites/partner2/incidents/" + GuidIncident4 + "/");
            request14.ExpectedResponseUrl = hostURL + "/sites/partner2/incidents/" + GuidIncident4 + "/default.aspx";

            //Validating the Incident 4 page
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule17 = new ValidationRuleFindText();
                validationRule17.FindText = "Incident: 4";
                validationRule17.IgnoreCase = true;
                validationRule17.UseRegularExpression = false;
                validationRule17.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule17.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule18 = new ValidationRuleFindText();
                validationRule18.FindText = "Incident Tasks";
                validationRule18.IgnoreCase = true;
                validationRule18.UseRegularExpression = false;
                validationRule18.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule18.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule19 = new ValidationRuleFindText();
                validationRule19.FindText = "Incident Documents";
                validationRule19.IgnoreCase = true;
                validationRule19.UseRegularExpression = false;
                validationRule19.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule19.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule20 = new ValidationRuleFindText();
                validationRule20.FindText = "Discussion Board";
                validationRule20.IgnoreCase = true;
                validationRule20.UseRegularExpression = false;
                validationRule20.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule20.Validate);
            }
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule21 = new ValidationRuleFindText();
                validationRule21.FindText = "IncidentInfo Web Part";
                validationRule21.IgnoreCase = true;
                validationRule21.UseRegularExpression = false;
                validationRule21.PassIfTextFound = true;
                request14.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule21.Validate);
            }
            yield return request14;
            request14 = null;


        }
    }
}
