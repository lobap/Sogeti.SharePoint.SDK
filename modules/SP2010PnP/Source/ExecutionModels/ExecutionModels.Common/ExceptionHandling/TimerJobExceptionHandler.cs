//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Security.Permissions;
using ExecutionModels.ExceptionHandling;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace ExecutionModels.Common.ExceptionHandling
{
    /// <summary>
    /// Class that helps with exception handling in TimerJobs. 
    /// 
    /// It will log an exception and try to display a 'friendly' error message on the page, without crashing the 
    /// whole page. If it can't display the error message, it will just crash the whole page (default behavior), 
    /// but still log the error. 
    /// </summary>
    public class TimerJobExceptionHandler : BaseRobustExceptionHandler
    {
        private const int defaultEventID = 0;

        /// <summary>
        /// Handle an exception in a TimerJob. This method will log the error using the ILogger that's registered in 
        /// the <see cref="SharePointServiceLocator"/>.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="eventId">The EventId to log the error under.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void HandleTimerJobException(Exception exception, int eventId)
        {
            try
            {
                ILogger logger = GetLogger(exception);
                logger.LogToOperations(exception, eventId, SandboxEventSeverity.Error, null);
            }
            catch (ExceptionHandlingException)
            {
                throw;
            }
            catch (Exception handlingException)
            {
                ThrowExceptionHandlingException(handlingException, exception);
            }
        }

        /// <summary>
        /// Handle an exception in a TimerJob. This method will log the error using the ILogger that's registered in 
        /// the <see cref="SharePointServiceLocator"/>.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void HandleTimerJobException(Exception exception)
        {
            HandleTimerJobException(exception, defaultEventID);
        }

        /// <summary>
        /// Handle an exception in a TimerJob. This method will log the error using the ILogger that's registered in 
        /// the <see cref="SharePointServiceLocator"/>.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="customErrorMessage">Custom error message to display to the user. </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void HandleTimerJobException(Exception exception, string customErrorMessage)
        {
            HandleTimerJobException(exception, customErrorMessage, defaultEventID);
        }

        /// <summary>
        /// Handle an exception in a TimerJob. This method will log the error using the ILogger that's registered in 
        /// the <see cref="SharePointServiceLocator"/>.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="errorVisualizer">The error visualizer that will show the errormessage.</param>
        /// <param name="eventId">The EventId to log the error under.</param>
        /// <param name="customErrorMessage">Custom error message to display to the user. </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void HandleTimerJobException(Exception exception, string customErrorMessage, int eventId)
        {
            try
            {
                ILogger logger = GetLogger(exception);
                logger.LogToOperations(exception, eventId, SandboxEventSeverity.Error, customErrorMessage);
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
    }
}