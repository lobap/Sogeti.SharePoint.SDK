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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.SharePoint.Common.Tests.Configuration
{
    public partial class IPropertyBagTest
    {
        public static  void AddContains(IPropertyBag target, string key, string value)
        {
            target[key] = value;
            Assert.IsTrue(target.Contains(key));
            Assert.IsTrue(value == target[key]);
        }
    }
}
