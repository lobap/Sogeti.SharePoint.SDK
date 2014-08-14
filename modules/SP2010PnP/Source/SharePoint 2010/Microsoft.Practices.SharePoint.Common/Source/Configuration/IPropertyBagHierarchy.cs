//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Collections.Generic;
namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Represents an interface for a container of property bags.  The container
    /// is responsible for ordering the property bags as well in hierarchical 
    /// sequence.  The first bag in the PropertyBags is the lowest in the hierachy.
    /// </summary>
    public interface IPropertyBagHierarchy
    {
        /// <summary>
        /// Retrieves the property bag for a specific level in the hierarchy.
        /// </summary>
        /// <param name="level">The level of the property bag to get</param>
        /// <returns>The property bag for the level, null if the property bag is not available.</returns>
        IPropertyBag GetPropertyBagForLevel(ConfigLevel level);

        /// <summary>
        /// Gets the enumeration for the property bags in the hierarchy, in order of lowest to 
        /// highest.  The number and types of property bags available will depend upon the context.
        /// </summary>
        IEnumerable<IPropertyBag> PropertyBags { get; }
    }
}
