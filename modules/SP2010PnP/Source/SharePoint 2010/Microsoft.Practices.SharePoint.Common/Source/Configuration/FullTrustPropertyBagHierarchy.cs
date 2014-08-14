//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Represents the property bag hierarchy for a full trust configuration.  Contains a
    /// SPWebPropertyBag, SPSitePropertyBag, SPWebApPropertyBag, and SPFarmPropertyBag.
    /// </summary>
    public class FullTrustPropertyBagHierarchy : PropertyBagHierarchy
    {
        /// <summary>
        /// Constructs the farm hierarchy for a full trust configuration.
        /// </summary>
        /// <param name="web">The web to use to build the hierarchy</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public FullTrustPropertyBagHierarchy(SPWeb web)
        {
            Validation.ArgumentNotNull(web, "web");
            BuildHierarchy(web);
        }

        /// <summary>
        /// Builds the hierarchy for the provided web.  The web, site, and web application, and farm are
        /// property bags are constructed from this web using web.Site, web.Site.WebApplication, and 
        /// web.Site.WebApplication.Farm.
        /// </summary>
        /// <param name="web">The SPWeb to use to construct the hierarchy</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void BuildHierarchy(SPWeb web)
        {
            Bags.Add(new SPWebPropertyBag(web));
            Bags.Add(new SPSitePropertyBag(web.Site));
            Bags.Add(new SPWebAppPropertyBag(web.Site.WebApplication));
            Bags.Add(new SPFarmPropertyBag(web.Site.WebApplication.Farm));
        }
    }
}
