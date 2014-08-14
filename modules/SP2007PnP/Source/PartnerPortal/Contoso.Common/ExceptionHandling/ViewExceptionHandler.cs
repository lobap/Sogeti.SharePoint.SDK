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
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web.UI;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Contoso.Common.ExceptionHandling
{
    /// <summary>
    /// Class that helps with exception handling in Views, user controls, Webparts and other controls. 
    /// 
    /// It will log an exception and try to display a 'friendly' error message on the page, without crashing the 
    /// whole page. If it can't display the error message, it will just crash the whole page (default behavior), 
    /// but still log the error. 
    /// </summary>
    public class ViewExceptionHandler : BaseRobustExceptionHandler
    {
        private const int defaultEventID = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewExceptionHandler"/> class.
        /// </summary>
        public ViewExceptionHandler()
        {
            
        }

        /// <summary>
        /// Handle an exception in a view. This method will log the error using the ILogger that's registered in 
        /// the <see cref="SharePointServiceLocator"/> and will show the error in the <paramref name="errorVisualizer"/>
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="errorVisualizer">The error visualizer that will show the errormessage.</param>
        /// <param name="eventId">The EventId to log the error under.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "In this case, we are building an unhandled exception handler. The ThrowExceptionHandlingException will throw the exception for us")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)] 
        public void HandleViewException(Exception exception, IErrorVisualizer errorVisualizer, int eventId)
        {
            try
            {
                ILogger logger = GetLogger(exception);
                logger.LogToOperations(exception, eventId, EventLogEntryType.Error, null);

                EnsureErrorVisualizer(errorVisualizer, exception);
                errorVisualizer.ShowDefaultErrorMessage();
            }
            catch (ExceptionHandlingException)
            {
                throw;
            }
            catch (Exception handlingException)
            {
                this.ThrowExceptionHandlingException(handlingException, exception);
            }
        }

        /// <summary>
        /// Handle an exception in a view. This method will log the error using the ILogger that's registered in 
        /// the <see cref="SharePointServiceLocator"/> and will show the error in the <paramref name="errorVisualizer"/>
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="errorVisualizer">The error visualizer that will show the errormessage.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void HandleViewException(Exception exception, IErrorVisualizer errorVisualizer)
        {
            HandleViewException(exception, errorVisualizer, defaultEventID);
        }

        /// <summary>
        /// Handle an exception in a view. This method will log the error using the ILogger that's registered in 
        /// the <see cref="SharePointServiceLocator"/> and will show the error in the <paramref name="errorVisualizer"/>
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="errorVisualizer">The error visualizer that will show the errormessage.</param>
        /// <param name="customErrorMessage">Custom error message to display to the user. </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)] 
        public void HandleViewException(Exception exception, IErrorVisualizer errorVisualizer, string customErrorMessage)
        {
            HandleViewException(exception, errorVisualizer, customErrorMessage, defaultEventID);
        }

        /// <summary>
        /// Handle an exception in a view. This method will log the error using the ILogger that's registered in 
        /// the <see cref="SharePointServiceLocator"/> and will show the error in the <paramref name="errorVisualizer"/>
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="errorVisualizer">The error visualizer that will show the errormessage.</param>
        /// <param name="eventId">The EventId to log the error under.</param>
        /// <param name="customErrorMessage">Custom error message to display to the user. </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This is an unhandled exception handler, so we need to catch Exception.")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void HandleViewException(Exception exception, IErrorVisualizer errorVisualizer, string customErrorMessage, int eventId)
        {
            try
            {
                ILogger logger = GetLogger(exception);
                logger.LogToOperations(exception, eventId, EventLogEntryType.Error, customErrorMessage);

                EnsureErrorVisualizer(errorVisualizer, exception);
                errorVisualizer.ShowErrorMessage(customErrorMessage);
            }
            catch(ExceptionHandlingException)
            {
                throw;
            }
            catch(Exception handlingException)
            {
                this.ThrowExceptionHandlingException(handlingException, exception);
            }
        }

        private void EnsureErrorVisualizer(IErrorVisualizer errorVisualizer, Exception originalException)
        {
            if (errorVisualizer == null)
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture, "IErrorVisualizer was not found.");
                throw new ExceptionHandlingException(
                    BuildExceptionHandlingErrorMessage(null, originalException, errorMessage)
                    , originalException);
            }
        }

        /// <summary>
        /// Try to find an <see cref="IErrorVisualizer"/> in the parent controls.
        /// </summary>
        /// <param name="control">The control to take as starting point to find an IErrorVisualizer.</param>
        /// <returns>An errorVisualiser if it or it's parent is one. Null if it can't find it. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        public static IErrorVisualizer FindErrorVisualizer(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            Control currentControl = control;
            do
            {
                IErrorVisualizer errorVisualizer = currentControl as IErrorVisualizer;

                if (errorVisualizer != null)
                    return errorVisualizer;

                currentControl = currentControl.Parent;
            } while (currentControl != null);

            return null;
        }

        /// <summary>
        /// Show a functional error message in the error visualiser. This functional error will only be logged to the
        /// trace log. Not to the EventLog. 
        /// </summary>
        /// <param name="errorMessage">The error message to display in the error visualiser.</param>
        /// <param name="errorVisualizer">The error visualiser to display the error message in. </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "In this case, we are building an unhandled exception handler. The ThrowExceptionHandlingException will throw the exception for us")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)] 
        public void ShowFunctionalErrorMessage(string errorMessage, IErrorVisualizer errorVisualizer)
        {
            try
            {
                if (errorVisualizer == null) throw new ArgumentNullException("errorVisualizer");

                ILogger logger = GetLogger(null);
                logger.TraceToDeveloper(errorMessage);
                errorVisualizer.ShowErrorMessage(errorMessage);
            }
            catch(ExceptionHandlingException)
            {
                throw;
            }
            catch (Exception handlingException)
            {
                this.ThrowExceptionHandlingException(handlingException, null);
            }
        }
    }
}