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
using System.Diagnostics;
using System.Threading;
using System.Web;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Moles;
using Microsoft.Practices.SharePoint.Common.Logging.Moles;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{
    [TestClass]
    public class EventLogLoggerTests
    {
        //no unit tests since only implements a simple pass thru.  This was done in order to maximize testability of the code by pushing the
        //code down into the DiagnosticsService.  Since SharePoint owned the base class, it made implementing an interface for the class in
        // a robust way difficult and therefore could not isolate the service with service locator.
    }
}
