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
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Web;
using System.IdentityModel.Tokens;

namespace Contoso.LOB.Services.Security
{
    /// <summary>
    /// This class explicitly does NOT validate the username and password, because it's used in the Trusted
    /// FAcade pattern: http://msdn.microsoft.com/en-us/library/aa355058.aspx
    /// 
    /// For the trusted facade pattern, SharePoint just forwards the PartnerID as the userName. The LOB Service
    /// trusts SharePoint to forward the correct partner id. It does validate that the server that's calling the 
    /// service is trusted through. 
    /// </summary>
    public class NoOpUserNamePasswordValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            // Don't validate the username and password. 

            // This implementation follows the trusted facade pattern:
            // http://msdn.microsoft.com/en-us/library/aa355058.aspx

            // In short, this webservice is not exposed publicly, but can only be accessed
            // by trusted clients. In our scenario, SharePoint is that trusted client. 
            // It authenticates the users, so this LOB Service trusts SharePoint to provide
            // it with the required credentials. 

        }
    }
}
