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
    public class FarmPropertyBagHierarchy: PropertyBagHierarchy
    {
        /// <summary>
        /// Constructs the FarmPropertyBagHierarchy
        /// </summary>
        /// <param name="farm">The farm to create the hierarchy for</param>
        public FarmPropertyBagHierarchy(SPFarm farm)
        {
            Validation.ArgumentNotNull(farm, "farm");
            BuildHierarchy(farm);
        }

        /// <summary>
        /// Builds the heirarchy for the farm only context.  
        /// </summary>
        /// <param name="farm">The farm to use</param>
        private void BuildHierarchy(SPFarm farm)
        {
            Bags.Add(new SPFarmPropertyBag(farm));
        }
    }
}
