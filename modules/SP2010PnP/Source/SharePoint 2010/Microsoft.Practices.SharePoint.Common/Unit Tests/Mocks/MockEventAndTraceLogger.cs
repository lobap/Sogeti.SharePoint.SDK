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
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{
    class MockEventAndTraceLogger : ITraceLogger, IEventLogLogger
    {
        public class LogMessage
        {
            public LogMessage(string message, int eventId, TraceSeverity severity, string category)
            {
                this.Message = message;
                this.EventId = eventId;
                this.TraceSeverity = severity;
                this.Category = category;
            }

            public LogMessage(string message, int eventId, EventSeverity severity, string category)
            {
                this.Message = message;
                this.EventId = eventId;
                this.EventSeverity = severity;
                this.Category = category;
            }

            public string Message { get; private set; }
            public int EventId { get; private set; }
            public TraceSeverity TraceSeverity { get; private set; }
            public string Category { get; private set; }
            public EventSeverity EventSeverity { get; private set; }
        }

        public readonly List<LogMessage> Messages;

        public MockEventAndTraceLogger()
        {
            this.Messages = new List<LogMessage>();
        }

        public void Trace(string message, int eventId, TraceSeverity severity, string category)
        {
            var msg = new LogMessage(message, eventId, severity, category);
            this.Messages.Add(msg);
        }

        public void Log(string message, int eventId, EventSeverity severity, string category)
        {
            var msg = new LogMessage(message, eventId, severity, category);
            this.Messages.Add(msg);
        }


        public void Trace(string message, int eventId, string category)
        {
            throw new NotImplementedException();
        }


        public void Log(string message, int eventId, string category)
        {
            var msg = new LogMessage(message, eventId, EventSeverity.ErrorCritical, category);
            this.Messages.Add(msg);
        }
    }
}
