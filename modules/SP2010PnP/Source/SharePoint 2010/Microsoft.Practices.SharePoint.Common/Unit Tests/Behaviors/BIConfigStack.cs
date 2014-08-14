//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.SharePoint.Common.Configuration.Moles;
using Microsoft.Practices.SharePoint.Common.Configuration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Behaviors
{
    public class BIConfigStack: IPropertyBagHierarchy
    {
        public List<IPropertyBag> Bags = new List<IPropertyBag>();

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

        public IPropertyBag GetPropertyBagForLevel(ConfigLevel level)
        {
            int i = 0;

            while (i < Bags.Count && Bags[i].Level != level)
                i++;

            if (i == Bags.Count)
                return null;

            return Bags[i];
        }
    }
}
