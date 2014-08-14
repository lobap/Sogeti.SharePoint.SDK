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
    class TestableBaseLogger : BaseLogger
    {
        public class LoggedMessage
        {
            public string Message { get; set; }
            public int EventId { get; set; }
            public TraceSeverity TraceSeverity { get; set; }
            public EventSeverity EventSeverity { get; set; }
            public string Category { get; set; }
            public SandboxEventSeverity SandboxEventSeverity { get; set; }
            public SandboxTraceSeverity SandboxTraceSeverity { get; set; }
        }

        public System.Collections.Generic.List<LoggedMessage> Messages = new System.Collections.Generic.List<LoggedMessage>();

        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category)
        {
            var messageToAdd = new LoggedMessage();

            messageToAdd.Message = message;
            messageToAdd.EventId = eventId;
            messageToAdd.TraceSeverity = severity;
            messageToAdd.Category = category;
            Messages.Add(messageToAdd);
        }

        protected override void WriteToOperationsLog(string message, int eventId, EventSeverity severity, string category)
        {
            var messageToAdd = new LoggedMessage();

            messageToAdd.Message = message;
            messageToAdd.EventId = eventId;
            messageToAdd.EventSeverity = severity;
            messageToAdd.Category = category;
            Messages.Add(messageToAdd);
        }

        protected override void WriteToOperationsLog(string message, int eventId, SandboxEventSeverity severity, string category)
        {
            var messageToAdd = new LoggedMessage();
            messageToAdd.Message = message;
            messageToAdd.EventId = eventId;
            messageToAdd.SandboxEventSeverity = severity;
            messageToAdd.Category = category;
            Messages.Add(messageToAdd);
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, SandboxTraceSeverity severity, string category)
        {
            var messageToAdd = new LoggedMessage();
            messageToAdd.Message = message;
            messageToAdd.EventId = eventId;
            messageToAdd.SandboxTraceSeverity = severity;
            messageToAdd.Category = category;
            Messages.Add(messageToAdd);
        }

        protected override void WriteToOperationsLog(string message, int eventId, string category)
        {
            var messageToAdd = new LoggedMessage();
            messageToAdd.Message = message;
            messageToAdd.EventId = eventId;
            messageToAdd.Category = category;
            Messages.Add(messageToAdd);
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, string category)
        {
            var messageToAdd = new LoggedMessage();
            messageToAdd.Message = message;
            messageToAdd.EventId = eventId;
            messageToAdd.Category = category;
            Messages.Add(messageToAdd);
        }
    }
}
