//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace ExecutionModels.Common.ExceptionHandling
{
    /// <summary>
    /// Interface for controls that can display exception messages. This interface is used by 
    /// the ViewExceptionHandler to display error messages after an exception has occurred
    /// in a webpart.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualiser", Justification = "Visualiser is the most common spelling for this thing.")]
    public interface IErrorVisualizer
    {
        /// <summary>
        /// Show a default error message. This should be a friendly, non technical error message, that tells the end user something is wrong. 
        /// </summary>
        void ShowDefaultErrorMessage();

        /// <summary>
        /// Show a cusotm error message. This should be a friendly, non technical error message, that tells the end user something is wrong. 
        /// </summary>
        /// <param name="errorMessage">The error message to display. This should be a friendly, non technical error message, that tells the end user something is wrong. </param>
        void ShowErrorMessage(string errorMessage);
    }
}