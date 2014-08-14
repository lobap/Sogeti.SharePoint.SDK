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
using System.Security.Principal;
using System.Security.Permissions;
using System.Web.Configuration;

namespace Contoso.PartnerPortal.Services.Security
{
    public class SecurityHelper : ISecurityHelper
    {
        /// <summary>
        /// This method validates that the current user belongs to the local ContosoSPTrustedAccounts group.
        /// </summary>
        public void DemandAuthorizedPermissions()
        {
            string trustedAccountsGroupName = WebConfigurationManager.AppSettings.Get("TrustedAccountGroup");
            string role = trustedAccountsGroupName.Replace("LocalMachineName", System.Environment.MachineName);

            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            PrincipalPermission principalPermission = new PrincipalPermission(null, role, true);
            principalPermission.Demand();
        }
    }
}