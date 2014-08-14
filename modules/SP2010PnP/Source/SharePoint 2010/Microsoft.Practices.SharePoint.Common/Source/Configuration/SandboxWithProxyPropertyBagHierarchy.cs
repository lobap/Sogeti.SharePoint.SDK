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
    /// A property bag for use in the sandbox that contains read only property bags for the farm
    /// and web application.   Contains a property bag for the web, site, readonly web app, and
    /// read only farm.
    /// </summary>
    public class SandboxWithProxyPropertyBagHierarchy: PropertyBagHierarchy
    {
        /// <summary>
        /// Constructs the property bag for sandbox with proxy.
        /// </summary>
        /// <param name="web">The web to use as a basis for creating property bags</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public SandboxWithProxyPropertyBagHierarchy(SPWeb web)
        {
            Validation.ArgumentNotNull(web, "web");
            BuildHierarchy(web);
        }

        /// <summary>
        /// Builds the hierarchy of property bags for the provided web.
        /// </summary>
        /// <param name="web">The web to use as a basis for creating the property bags</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void BuildHierarchy(SPWeb web)
        {
            Bags.Add(new SPWebPropertyBag(web));
            Bags.Add(new SPSitePropertyBag(web.Site));
            Bags.Add(new SandboxWebAppPropertyBag(web.Site.ID));
            Bags.Add(new SandboxFarmPropertyBag());
        }
    }
}
