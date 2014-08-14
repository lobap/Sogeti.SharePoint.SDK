//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// A property bag for a site. Uses the property bag on the root web to store configuration values.
    /// </summary>
    public class SPSitePropertyBag : SPWebPropertyBag
    {
        /// <summary>
        /// The prefix that's used by the SPSitePropertyBag to differentiate key's between SPSite settings and
        /// settings for the RootWeb. 
        /// </summary>
        public const string KeySuffix = "._Site_";
        private readonly SPSite site;

        /// <summary>
        /// Initializes a new instance of the <see cref="SPSitePropertyBag"/> class.
        /// </summary>
        /// <param name="site">The site.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public SPSitePropertyBag(SPSite site)
            : base(site.RootWeb)
        {
            Validation.ArgumentNotNull(site, "site");
            this.site = site;
        }

        /// <summary>
        /// Gets The config level that this SPSitePropertyBag operates on. Returns <see cref="ConfigLevel.CurrentSPSite"/>.
        /// </summary>
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
        /// <param name="key">The key provided</param>
        /// <returns>A transformed key to differentiate site from web level settings</returns>
        protected override string BuildKey(string key)
        {
            return base.BuildKey(key) + KeySuffix;
        }
    }
}