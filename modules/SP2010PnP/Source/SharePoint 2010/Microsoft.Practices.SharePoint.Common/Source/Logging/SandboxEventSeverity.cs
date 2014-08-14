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

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// This enumeration represents the non-oboslete levels for
    /// SharePoint EventSeverity accessible from the sandbox.  Values match
    /// values declared for EventSeverity
    /// </summary>
    public enum SandboxEventSeverity
    {
        /// <summary>
        ///  Indicates no event entries are written. 
        /// </summary>
        None = 0, 
        /// <summary>
        ///  Indicates a problem state that needs the immediate attention of an administrator. 
        /// </summary>
        ErrorCritical=30, 
         /// <summary>
         /// Indicates a problem state requiring attention by a site administrator.
         /// </summary>
         Error=40,
        /// <summary>
        /// Indicates conditions that are not immediately significant but that may eventually cause failure. 
        /// </summary>
         Warning=50,
        /// <summary>
        ///  Contains noncritical information provided for the administrator. 
        /// </summary>
        Information=80, 
        /// <summary>
        /// Contains detailed information provided for the administrator
        /// </summary>
        Verbose=100  
    }
}
