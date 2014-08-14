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
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.UserCode;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using System.Globalization;
using Microsoft.Practices.SharePoint.Common.LoggerProxy.Properties;

namespace Microsoft.Practices.SharePoint.Common.Logging.LoggerProxy
{
    /// <summary>
    /// Provides the full trust proxy operation for logging
    /// </summary>
    public class LoggingOperation : SPProxyOperation
    {
        /// <summary>
        /// Implements the full trust proxy for logging from the sandbox to the event log.
        /// </summary>
        /// <param name="args">The arguments for logging, must be of type <see cref="LoggingOperationArgs"/></param>
        /// <returns>null if no error occurred, otherwise the exception representing the error condition</returns>
        public override object Execute(SPProxyOperationArgs args)
        {
            Validation.ArgumentNotNull(args, "args");

            try
            {
                var proxyArgs = args as LoggingOperationArgs;

                if (proxyArgs == null)
                {
                    return new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidProxyArgProvided,
                        args.GetType().AssemblyQualifiedName, typeof(LoggingOperationArgs).AssemblyQualifiedName));
                }
                if (proxyArgs.Message == null)
                {
                    return new ArgumentNullException("proxyArgs.Message");
                }

                ILogger logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>();
                string sandboxMessage = proxyArgs.Message;

                if (proxyArgs.SiteID != null)
                {
                    using (SPSite site = new SPSite((Guid)proxyArgs.SiteID))
                    {
                        sandboxMessage = string.Format(CultureInfo.CurrentCulture, Resources.SandboxLogMessage, site.ID, site.RootWeb.Name, proxyArgs.Message);
                    }
                }
                else
                {
                    sandboxMessage = string.Format(CultureInfo.CurrentCulture, Resources.NoSiteContextLogMessage, proxyArgs.Message);
                }


                if (proxyArgs.Severity == null)
                {
                    logger.LogToOperations(sandboxMessage, proxyArgs.EventId, proxyArgs.Category);
                }
                else
                {
                    var severity = (SandboxEventSeverity)proxyArgs.Severity;
                    logger.LogToOperations(sandboxMessage, proxyArgs.EventId, severity, proxyArgs.Category);
                }

                return null;
            }
            catch (Exception exception)
            {
                return exception;
            }
        }
    }
}
