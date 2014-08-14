//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace ExecutionModels.Common.Tests
{
    using Microsoft.Practices.SharePoint.Common.Logging;
    using Microsoft.SharePoint.Administration;

    public class MockLogger : BaseLogger
    {
        public string ErrorMessage;

        public MockLogger()
        {
        }

        protected override void WriteToOperationsLog(string message, int eventId, EventSeverity severity, string category)
        {
            ErrorMessage = message;
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category)
        {
            ErrorMessage = message; 
        }

        protected override string BuildExceptionMessage(System.Exception exception, string customErrorMessage)
        {
            return exception.Message;
        }

        protected override void WriteToOperationsLog(string message, int eventId, string category)
        {
            throw new System.NotImplementedException();
        }

        protected override void WriteToOperationsLog(string message, int eventId, SandboxEventSeverity severity, string category)
        {
            ErrorMessage = message;
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, string category)
        {
            throw new System.NotImplementedException();
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, SandboxTraceSeverity severity, string category)
        {
            throw new System.NotImplementedException();
        }
    }
}