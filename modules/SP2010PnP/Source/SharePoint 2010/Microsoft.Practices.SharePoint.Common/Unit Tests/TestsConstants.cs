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

namespace Microsoft.Practices.SharePoint.Common.Tests
{
    static class TestsConstants
    {
        public static readonly string AreasCategories = string.Concat(
            "Area1",
            Microsoft.Practices.SharePoint.Common.Constants.CategoryPathSeparator,
            "Category1");

        public const string TestGuidName = "{599B31FB-C914-4ACD-901A-D0E2C1F34609}";

        public static readonly Guid TestGuid = new Guid(TestGuidName);
    }
}
