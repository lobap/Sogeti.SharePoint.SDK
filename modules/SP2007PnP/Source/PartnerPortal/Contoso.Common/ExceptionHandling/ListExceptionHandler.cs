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
using System.Diagnostics;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Security;

namespace Contoso.Common.ExceptionHandling
{
    /// <summary>
    /// Class that helps to handle exceptions in ItemEventReceivers. 
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)] 
    public class ListExceptionHandler : BaseRobustExceptionHandler
    {

        private const int defaultEventID = 0;

        /// <summary>
        /// Handle an exception in a listItemEvent. This will set the Cancel property of the <paramref name="spItemEventProperties"/>
        /// to true, set the error message on the  <paramref name="spItemEventProperties"/> and log the exception into the <see cref="ILogger"/>
        /// that's registered with the <see cref="SharePointServiceLocator"/>.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="spItemEventProperties">The ItemEventProperties that will be updated.</param>
        public void HandleListItemEventException(Exception exception, SPEventPropertiesBase spItemEventProperties)
        {
            this.HandleListItemEventException(exception, spItemEventProperties, ListItemAction.Cancel, exception.Message);
        }

        /// <summary>
        /// Handle an exception in a listItemEvent. This will set the Cancel property of the <paramref name="spItemEventProperties"/>
        /// to true, set the error message on the  <paramref name="spItemEventProperties"/> and log the exception into the <see cref="ILogger"/>
        /// that's registered with the <see cref="SharePointServiceLocator"/>.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="spItemEventProperties">The ItemEventProperties that will be updated.</param>
        /// <param name="message">Custom message that will be passed to the logger and as the exceptionmessage on the <paramref name="spItemEventProperties"/></param>
        public void HandleListItemEventException(Exception exception, SPEventPropertiesBase spItemEventProperties, string message)
        {
            this.HandleListItemEventException(exception, spItemEventProperties, ListItemAction.Cancel, message);
        }

        /// <summary>
        /// Handle an exception in a listItemEvent. This will set the Cancel property of the <paramref name="spItemEventProperties"/>
        /// to true if the <paramref name="listItemAction"/> equals <see cref="ListItemAction.Cancel"/>, set the error message on 
        /// the  <paramref name="spItemEventProperties"/> and log the exception into the <see cref="ILogger"/> that's registered 
        /// with the <see cref="SharePointServiceLocator"/>.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="spItemEventProperties">The ItemEventProperties that will be updated.</param>
        /// <param name="listItemAction">Determines if the action should be canceled.</param>
        public void HandleListItemEventException(Exception exception, SPEventPropertiesBase spItemEventProperties, ListItemAction listItemAction)
        {
            this.HandleListItemEventException(exception, spItemEventProperties, listItemAction, exception.Message);
        }

        /// <summary>
        /// Handle an exception in a listItemEvent. This will set the Cancel property of the <paramref name="spItemEventProperties"/>
        /// to true if the <paramref name="listItemAction"/> equals <see cref="ListItemAction.Cancel"/>, set the error message on 
        /// the  <paramref name="spItemEventProperties"/> and log the exception into the <see cref="ILogger"/> that's registered 
        /// with the <see cref="SharePointServiceLocator"/>.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="spItemEventProperties">The ItemEventProperties that will be updated.</param>
        /// <param name="listItemAction">Determines if the action should be canceled.</param>
        /// /// <param name="message">Custom message that will be passed to the logger and as the exceptionmessage on the <paramref name="spItemEventProperties"/></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "In this case, we are building an unhandled exception handler. The ThrowExceptionHandlingException will throw the exception for us")]
        public void HandleListItemEventException(Exception exception, SPEventPropertiesBase spItemEventProperties, ListItemAction listItemAction, string message)
        {
            try
            {
                var logger = GetLogger(exception);
                spItemEventProperties.Cancel = (ListItemAction.Cancel == listItemAction);
                spItemEventProperties.ErrorMessage = message;
                logger.LogToOperations(exception, defaultEventID, EventLogEntryType.Error, null);
            }
            catch(ExceptionHandlingException)
            {
                throw;
            }
            catch (Exception handlingException)
            {
                ThrowExceptionHandlingException(handlingException, exception);
            }
        }
    }
}