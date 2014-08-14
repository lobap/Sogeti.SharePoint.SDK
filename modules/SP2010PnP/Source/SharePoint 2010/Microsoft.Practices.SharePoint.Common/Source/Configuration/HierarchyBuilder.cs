//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Responsible for building the hierarchy of property bags for
    /// the current context.
    /// </summary>
    public class HierarchyBuilder
    {
        /// <summary>
        /// Constructs the hierarchy using the SPWeb as the starting
        /// point for the operating context.
        /// </summary>
        /// <param name="web">The web to use as a basis for the hierarchy</param>
        /// <returns>A <see cref="IPropertyBagHierarchy"/> that contains the property bag hierarchy to use</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static IPropertyBagHierarchy GetHierarchy(SPWeb web)
        {
            IPropertyBagHierarchy hierarchyStack = null;

            if (SharePointEnvironment.InSandbox)
            {
                if (web == null && SharePointEnvironment.CanAccessFarmConfig)
                {
                    return new SandboxFarmPropertyBagHierarchy();
                }

                Validation.ArgumentNotNull(web, "web");

                if (SharePointEnvironment.CanAccessFarmConfig)
                {
                    hierarchyStack = new SandboxWithProxyPropertyBagHierarchy(web);
                }
                else
                {
                    hierarchyStack = new SandboxPropertyBagHierarchy(web);
                }
            }
            else
            {
                if (web != null)
                {
                    hierarchyStack = new FullTrustPropertyBagHierarchy(web);
                }
                else
                {
                    hierarchyStack = GetFarmHierarchy();
                }
            }
            return hierarchyStack;
        }

        
        static IPropertyBagHierarchy GetFarmHierarchy()
        {
            if (SPFarm.Local != null)
            {
                return new FarmPropertyBagHierarchy(SPFarm.Local);
            }
            else
            {
                throw new NoSharePointContextException(Properties.Resources.SPFarmNotFound);
            }
        }
    }
}
