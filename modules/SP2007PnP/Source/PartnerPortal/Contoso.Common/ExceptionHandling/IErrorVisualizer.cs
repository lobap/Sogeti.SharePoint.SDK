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

namespace Contoso.Common.ExceptionHandling
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