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
    /// Represents equivalent trace servities to SharePoint for use in the sandbox
    /// since TraceSeverity enum can't be accessed from the sandbox.
    /// </summary>
    public enum SandboxTraceSeverity
    {
               /// <summary>
        /// Writes high-level detail to the trace log file.
        /// </summary>
         High=20,   
        /// <summary>
        /// Writes medium-level detail to the trace log file
        /// </summary>
        Medium=50,
        /// <summary>
        ///  Represents an unusual code path and actions that should be monitored.  
        /// </summary>
        Monitorable=15,
        /// <summary>
        ///  Writes no trace information to the trace log file.  
        /// </summary>
        None=0,
        /// <summary>
        ///  Represents an unexpected code path and actions that should be monitored.
        /// </summary>
        Unexpected=10,
        /// <summary>
        ///  Writes low-level detail to the trace log file.   
        /// </summary>
        Verbose=100
    }
}
