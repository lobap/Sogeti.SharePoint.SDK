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

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
{
    /// <summary>
    /// Event args to use to notify a tpye mapping has changed for a service locator
    /// (added programmatically)
    /// </summary>
    public class TypeMappingChangedArgs: EventArgs
    {
        /// <summary>
        /// The details about the changed type mapping.
        /// </summary>
        public TypeMapping Mapping {get; set;}
    }
}
