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
    using System.Threading;

    /// <summary>
    /// This Test verify the creation of Incident 3 in the Partner 2 Incidents site.
    /// step1 : Create Incident 3
    /// step2 : Validate the creation of Incident 3 in the Partner 2 Incidents site.
    /// Step3 : Validate the Incident 3 Page
    /// </summary>

    public class _VerifyCreateIncident_3 : WebTest
    {
        string hostURL = CustConfig.GetHostURL;
        string serviceURL = CustConfig.GetLOBWebURL;

        public _VerifyCreateIncident_3()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            // Initialize validation rules that apply to all requests in the WebTest

            WebTestRequest request1 = new WebTestRequest(serviceURL + "/NewIncidentSite.aspx");
            request1.ThinkTime = 6;
            ExtractHiddenFields extractionRule1 = new ExtractHiddenFields();
            extractionRule1.Required = true;
            extractionRule1.HtmlDecode = true;
            extractionRule1.ContextParameterName = "1";
            request1.ExtractValues += new EventHandler<ExtractionEventArgs>(extractionRule1.Extract);
            yield return request1;
            request1 = null;

            WebTestRequest request2 = new WebTestRequest(serviceURL + "/NewIncidentSite.aspx");
            request2.ThinkTime = 20;
            request2.Method = "POST";
            FormPostHttpBody request2Body = new FormPostHttpBody();
            request2Body.FormPostParameters.Add("__VIEWSTATE", this.Context["$HIDDEN1.__VIEWSTATE"].ToString());
            request2Body.FormPostParameters.Add("__EVENTVALIDATION", this.Context["$HIDDEN1.__EVENTVALIDATION"].ToString());
            request2Body.FormPostParameters.Add("incidentsDropDownList", "3");
            request2Body.FormPostParameters.Add("createSite", "Create Site");
            request2.Body = request2Body;
            yield return request2;
            request2 = null;

            WebTestRequest request3 = new WebTestRequest(hostURL + "/sites/partner2/default.aspx");
            request3.ThinkTime = 5;
            yield return request3;
            request3 = null;
            //sleep sometime to get workflow done
            //Thread.Sleep(30000);
            WebTestRequest request4 = new WebTestRequest(hostURL + "/sites/partner2/_layouts/viewlsts.aspx");
            request4.ThinkTime = 3;
            request4.QueryStringParameters.Add("ShowSites", "1", false, false);
            yield return request4;
            request4 = null;

            WebTestRequest request5 = new WebTestRequest(hostURL + "/sites/partner2/incidents/");
            request5.ThinkTime = 3;
            request5.ExpectedResponseUrl = hostURL + "/sites/partner2/incidents/default.aspx";
            yield return request5;
            request5 = null;

            WebTestRequest request6 = new WebTestRequest(hostURL + "/sites/partner2/incidents/_layouts/viewlsts.aspx");
            request6.ThinkTime = 4;
            request6.QueryStringParameters.Add("ShowSites", "1", false, false);

        }
    }
}
