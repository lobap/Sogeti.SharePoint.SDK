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
    using System.Diagnostics;
    /// <summary>
    /// This test verify custom even source is using to log any errors from Partner Portal
    /// It browses wrong sku and verify the eventlog with proper source name.
    /// </summary>

    public class verifycustomeventsource : WebTest
    {
        string hostURL = CustConfig.GetHostURL;
        bool existsFlag;
        public verifycustomeventsource()
        {
            this.PreAuthenticate = true;
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            WebTestRequest request1 = new WebTestRequest(hostURL + "/sites/ProductCatalog/Product.aspx");
            request1.QueryStringParameters.Add("sku", "6000000001", false, false);
            if ((this.Context.ValidationLevel >= Microsoft.VisualStudio.TestTools.WebTesting.ValidationLevel.High))
            {
                ValidationRuleFindText validationRule1 = new ValidationRuleFindText();
                validationRule1.FindText = "Could not find product information.  ";
                validationRule1.IgnoreCase = true;
                validationRule1.UseRegularExpression = true;
                validationRule1.PassIfTextFound = true;
                request1.ValidateResponse += new EventHandler<ValidationEventArgs>(validationRule1.Validate);
            }
            yield return request1;
            request1 = null;

            //Verify eventlog
            EventLog eventLog1 = new EventLog("Application", System.Environment.MachineName.ToString(), "Contoso Partner Portal");

            foreach (EventLogEntry entry in eventLog1.Entries)
            {
                existsFlag = false;
                if (entry.Message.Contains("Sku \'6000000001\' could not be found"))
                {
                    existsFlag = true;
                    break;
                }
            }
            if (existsFlag)
                this.Outcome = Outcome.Pass;
            else
                this.Outcome = Outcome.Fail;

        }
    }
}
