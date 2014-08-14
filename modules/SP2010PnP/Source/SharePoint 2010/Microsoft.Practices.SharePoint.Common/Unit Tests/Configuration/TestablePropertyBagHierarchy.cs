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
using Microsoft.Practices.SharePoint.Common.Configuration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    public class TestablePropertyBagHierarchy: PropertyBagHierarchy
    {
        public TestablePropertyBagHierarchy()
        {
        }

        public void AddPropertyBag(IPropertyBag bag)
        {
            base.Bags.Add(bag);
        }

    }
}
