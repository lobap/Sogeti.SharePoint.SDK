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

namespace Microsoft.Practices.SharePoint.Common.ListRepository
{
    /// <summary>
    /// Class that holds filter expressions that can be used by the <see cref="CAMLQueryBuilder"/>. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CAML")]
    public class CAMLFilter
    {
        /// <summary>
        /// The filter expression to use when building a query. 
        /// </summary>
        public string FilterExpression { get; set; }
    }
}
