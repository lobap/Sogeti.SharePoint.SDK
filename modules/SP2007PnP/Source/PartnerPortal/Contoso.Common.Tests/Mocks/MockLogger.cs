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
using System.Diagnostics;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.SharePoint.Administration;

namespace Contoso.Common.Tests.ExceptionHandling
{
    public class MockLogger : BaseLogger
    {
        public string ErrorMessage;

        public MockLogger()
        {
        }


        protected override void WriteToOperationsLog(string message, int eventId, EventLogEntryType severity, string category)
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
    }
}