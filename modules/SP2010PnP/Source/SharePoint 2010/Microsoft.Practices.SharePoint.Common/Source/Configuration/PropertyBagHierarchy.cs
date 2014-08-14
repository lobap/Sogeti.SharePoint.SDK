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
    /// Implements the base behavior for a property bag hierarchy.  <see cref="IPropertyBagHierarchy"/>
    /// for more inforamtion.
    /// </summary>
    public abstract class PropertyBagHierarchy: IPropertyBagHierarchy
    {
        List<IPropertyBag> bags;

        /// <summary>
        /// Constructs the property bag hierarchy
        /// </summary>
        public PropertyBagHierarchy()
        {
            this.bags = new List<IPropertyBag>();
        }

        /// <summary>
        /// The list of property bags to use for this hierarchy.
        /// </summary>
        protected List<IPropertyBag> Bags
        {
            get
            {
                return bags;
            }
        }

        /// <summary>
        /// The enumerated list of property bags to read configuration from in 
        /// order.
        /// </summary>
        public IEnumerable<IPropertyBag> PropertyBags
        {
            get
            {
                foreach (IPropertyBag bag in Bags)
                {
                    yield return bag;
                }
            }
        }

        /// <summary>
        /// Gets the property bag for the level specified.
        /// </summary>
        /// <param name="level">The level to get the property bag for</param>
        /// <returns>the property bag for the level, null if the bag is not available</returns>
        public IPropertyBag GetPropertyBagForLevel(ConfigLevel level)
        {
            foreach (IPropertyBag bag in bags)
            {
                if (bag.Level == level)
                    return bag;
            }

            return null;
        }
    }
}
