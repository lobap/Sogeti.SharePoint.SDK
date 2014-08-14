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
using System.Web;

using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.SubSiteCreation;
using Microsoft.SharePoint;
using Contoso.PartnerPortal.PartnerDirectory;

namespace Contoso.PartnerPortal
{
    /// <summary>
    /// Controller class that determines to what URL to redirect to, when the user hits the partner redirect page. 
    /// </summary>
    public class PartnerRedirectController
    {
        public void Redirect(string queryStringParameterValue)
        {
            IPartnerSiteDirectory partnerSiteDirectory = SharePointServiceLocator.Current.GetInstance<IPartnerSiteDirectory>();
            string siteCollectionUrl = partnerSiteDirectory.GetPartnerSiteCollectionUrl();

            // The default behavior should be to take you the partner's site collection's root Web.
            Uri redirectUrl = new Uri(siteCollectionUrl);

            if (!string.IsNullOrEmpty(siteCollectionUrl))
            {

                redirectUrl = GetPartnerHomePageUrl(siteCollectionUrl, queryStringParameterValue, redirectUrl);
            }

            HttpContext.Current.Response.Redirect(redirectUrl.PathAndQuery);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private Uri GetPartnerHomePageUrl(string siteCollectionUrl, string queryStringParameterValue, Uri redirectUrl)
        {
            // If the querystring parameter equals 'home', then the user is redirected to it's home page. 
            if (!string.IsNullOrEmpty(queryStringParameterValue) && 
                !string.Equals(queryStringParameterValue, "home", StringComparison.OrdinalIgnoreCase))
            {
                IBusinessEventTypeConfigurationRepository businessEventConfigRepository
                    = SharePointServiceLocator.Current.GetInstance<IBusinessEventTypeConfigurationRepository>();

                BusinessEventTypeConfiguration businessEventConfig = null;
                RunElevated(delegate
                {
                    businessEventConfig = businessEventConfigRepository.GetBusinessEventTypeConfiguration(queryStringParameterValue);
                });

                if (businessEventConfig != null)
                {
                    using (SPSite site = new SPSite(siteCollectionUrl))
                    {
                        using (SPWeb web = site.OpenWeb(businessEventConfig.TopLevelSiteRelativeUrl))
                        {
                            if (web.Exists)
                            {
                                redirectUrl = new Uri(web.Url);
                            }
                        }
                    }
                }
            }
            return redirectUrl;
        }

        /// <summary>
        /// Runs the method in an elevated SharePoint context. 
        /// </summary>
        /// <param name="method">method to run</param>
        protected virtual void RunElevated(SPSecurity.CodeToRunElevated method)
        {
            SPSecurity.RunWithElevatedPrivileges(method);
        }
    }
}
