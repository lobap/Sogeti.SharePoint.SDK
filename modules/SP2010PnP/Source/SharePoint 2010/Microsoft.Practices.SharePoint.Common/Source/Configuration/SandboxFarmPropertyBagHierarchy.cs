//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// The property bag hierarchy for a farm only context. Contains a single
    /// property bag in the hierarchy, SPFarmPropertyBag.
    /// </summary>
    public class SandboxFarmPropertyBagHierarchy : PropertyBagHierarchy
    {
        /// <summary>
        /// Constructs the FarmPropertyBagHierarchy
        /// </summary>
        public SandboxFarmPropertyBagHierarchy()
        {
            Bags.Add(new SandboxFarmPropertyBag());
        }
    }
}
