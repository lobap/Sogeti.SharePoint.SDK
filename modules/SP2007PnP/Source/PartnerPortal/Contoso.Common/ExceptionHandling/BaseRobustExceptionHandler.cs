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
using System.Linq;
using System.Text;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using System.Security.Permissions;
using Microsoft.SharePoint.Security;

namespace Contoso.Common.ExceptionHandling
{
    /// <summary>
    /// Base class for exception handers. This class makes it easier to make exception handling robust
    /// If an exception occurs while handling an exception, 
    /// </summary>
    public abstract class BaseRobustExceptionHandler
    {
        private ILogger logger;

        /// <summary>
        /// Gets the logger from the service locator. If the logger cannot be found, the resulting exception exception is
        /// handled in such a way to prevent losing the original exception. 
        /// </summary>
        /// <param name="originalException">The original exception.</param>
        /// <returns>A loggin implementation</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected virtual ILogger GetLogger(Exception originalException)
        {
            try
            {
                if (this.logger == null)
                {
                    this.logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
                }
                return this.logger;
            }
            catch (Exception handlingException)
            {
                if (originalException == null)
                {
                    throw;
                }

                ThrowExceptionHandlingException(handlingException, originalException);
            }
            return null;
        }

        /// <summary>
        /// Handles an exception that might occur while handling a different exception. This will build and throw
        /// an exception that holds information about the original information, but also about why the exception
        /// handling failed. 
        /// 
        /// The main purpose of this stragety is to avoid losing the original exception information. 
        /// </summary>
        /// <param name="handlingException">The exception that occurred while handling the <paramref name="originalException"/>.</param>
        /// <param name="originalException">The original exception, that kicked off the error handling.</param>
        protected virtual void ThrowExceptionHandlingException(Exception handlingException, Exception originalException)
        {
            ExceptionHandlingException exception = new ExceptionHandlingException(
                BuildExceptionHandlingErrorMessage(handlingException, originalException, null), 
                originalException);

            exception.HandlingException = handlingException;

            throw exception;
        }

        /// <summary>
        /// Build an exception message that describes that the exception handling failed, what the original exception was
        /// and what failed int he exception handling. 
        /// </summary>
        /// <param name="handlingException">The exception that occurred while handling the <paramref name="originalException"/>.</param>
        /// <param name="originalException">The original exception, that kicked off the error handling.</param>
        /// <param name="additionalErrorMessage">Any additional messages that might be helpful. </param>
        /// <returns>The exception message. </returns>
        protected virtual string BuildExceptionHandlingErrorMessage(Exception handlingException, Exception originalException, string additionalErrorMessage)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("An exception occurred while handling another exception.");
            if (additionalErrorMessage != null)
            {
                builder.AppendLine(additionalErrorMessage);
            }
            builder.AppendFormat("\tThe original exception was: '{0}'\r\n", originalException.Message);
            if (handlingException != null)
            {
                builder.AppendFormat("\tThe handling exception was: '{0}'\r\n", handlingException.Message);
            }
            builder.AppendLine("Please also check the inner exception property and the HandlingException property for more information.");

            return builder.ToString();
        }
    }
}