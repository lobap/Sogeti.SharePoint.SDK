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
    /// Constructs the property bag hierarchy for a sandbox environment. Contains a property bag for
    /// the web and site levels.
    /// </summary>
    public class SandboxPropertyBagHierarchy: PropertyBagHierarchy
    {
        /// <summary>
        /// Constructs a sandbox hierarchy for the provided web.
        /// </summary>
        /// <param name="web">The web to use as a basis for constructing the hierarchy.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public SandboxPropertyBagHierarchy(SPWeb web)
        {
            Validation.ArgumentNotNull(web, "web");
            BuildHierarchy(web);
        }

        /// <summary>
        /// Builds the hierarchy for the web provided.
        /// </summary>
        /// <param name="web">The web to use for creating the property bags</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void BuildHierarchy(SPWeb web)
        {
            Bags.Add(new SPWebPropertyBag(web));
            Bags.Add(new SPSitePropertyBag(web.Site));
        }
    }
}
