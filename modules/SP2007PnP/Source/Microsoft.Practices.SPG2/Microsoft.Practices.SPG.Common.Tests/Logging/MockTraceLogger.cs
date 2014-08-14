//===============================================================================
// Microsoft patterns & practices
// SharePoint Guidance version 2.0
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SPG.Common.Tests.Logging
{
    class MockTraceLogger : ITraceLogger
    {
        public List<string> Messages = new List<string>();

        public string Message;
        public string Category;
        public int EventID;
        public TraceSeverity Severity;

        public void Trace(string message, int eventId, TraceSeverity severity, string category)
        {
            this.Messages.Add(message);

            this.Message = message;
            this.Category = category;
            this.EventID = eventId;
            this.Severity = severity;
        }
    }

    class MockEventLogger : IEventLogLogger
    {
        public List<string> Messages = new List<string>();

        public string Message;
        public string Category;
        public int EventID;
        public EventLogEntryType Severity;

        public void Log(string message, int eventId, EventLogEntryType severity, string category)
        {
            this.Messages.Add(message);

            this.Message = message;
            this.Category = category;
            this.EventID = eventId;
            this.Severity = severity;
        }
    }
}
