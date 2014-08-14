//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.SharePoint.UserCode;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using System.Globalization;
using Microsoft.Practices.SharePoint.Common.LoggerProxy.Properties;
using Microsoft.SharePoint;

namespace Microsoft.Practices.SharePoint.Common.Logging.LoggerProxy
{
    /// <summary>
    /// Implements the full trust proxy for tracing to the ULS.
    /// </summary>
    public class TracingOperation : SPProxyOperation
    {
        /// <summary>
        /// Implements the full trust proxy for tracing information to the ULS from the sandbox.
        /// </summary>
        /// <param name="args">The arguments for the operation, must be of type <see cref="TracingOperationArgs"/></param>
        /// <returns>null if successful, or an exception representing an error if an error occurred</returns>
        public override object Execute(SPProxyOperationArgs args)
        {
            Validation.ArgumentNotNull(args, "args");

            try
            {
                var proxyArgs = args as TracingOperationArgs;

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

                string sandboxMessage = null;

                if (proxyArgs.SiteID != null)
                {
                    using (SPSite site = new SPSite((Guid)proxyArgs.SiteID))
                    {
                        sandboxMessage = string.Format(CultureInfo.CurrentCulture, Resources.SandboxTraceMessage, site.ID, site.RootWeb.Name, proxyArgs.Message);
                    }
                }
                else
                {
                    sandboxMessage = string.Format(CultureInfo.CurrentCulture, Resources.NoSiteContextTraceMessage, proxyArgs.Message);
                }
                
                if (proxyArgs.Severity == null)
                {
                    logger.TraceToDeveloper(sandboxMessage, proxyArgs.EventId, proxyArgs.Category);
                }
                else
                {
                    var severity = (SandboxTraceSeverity)proxyArgs.Severity;
                    logger.TraceToDeveloper(sandboxMessage, proxyArgs.EventId, severity, proxyArgs.Category);
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
