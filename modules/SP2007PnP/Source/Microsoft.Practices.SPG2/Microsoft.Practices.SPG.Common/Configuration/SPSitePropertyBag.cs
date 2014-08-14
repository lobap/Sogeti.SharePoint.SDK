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
using System.Security.Permissions;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Configuration
{
    /// <summary>
    /// A class that wraps the PropertyBag of an SPSite. 
    /// </summary>
    [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class SPSitePropertyBag : SPWebPropertyBag
    {
        /// <summary>
        /// The prefix that's used by the SPSitePropertyBag to differentiate key's between SPSite settings and
        /// settings for the RootWeb. 
        /// </summary>
        internal const string KeyPrefix = "Site_";

        private readonly SPSite site;
        private IPropertyBag parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SPSitePropertyBag"/> class.
        /// </summary>
        /// <param name="site">The site.</param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        internal SPSitePropertyBag(SPSite site)
            : base(site.RootWeb)
        {
            if (site == null) throw new ArgumentNullException("site");
            this.site = site;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SPSitePropertyBag"/> class.
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public SPSitePropertyBag()
        {
            if (SPContext.Current == null)
            {
                throw new NoSharePointContextException("This code should run in a SharePoint context. SPContext.Current is null.");
            }

            this.site = SPContext.Current.Site;
            this.SpWeb = this.site.RootWeb;
        }

        /// <summary>
        /// Gets the parent property bag. This will be the SPWebApplication.
        /// </summary>
        /// <returns></returns>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public override IPropertyBag GetParent()
        {
            if (parent == null)
            {
                parent = new SPWebAppPropertyBag(site.WebApplication);
            }

            return parent;
        }

        /// <summary>
        /// Gets The config level that this SPSitePropertyBag operates on. Returns <see cref="ConfigLevel.CurrentSPSite"/>.
        /// </summary>
        /// <value>The level.</value>
        public override ConfigLevel Level
        {
            get
            {
                return ConfigLevel.CurrentSPSite;
            }
        }

        /// <summary>
        /// Changes the key to make sure that the items stored at rootweb level don't override
        /// the items at the current Web level. This can otherwise occur if the root Web == the current Web. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override string BuildKey(string key)
        {
            return KeyPrefix + base.BuildKey(key);
        }

    }
}