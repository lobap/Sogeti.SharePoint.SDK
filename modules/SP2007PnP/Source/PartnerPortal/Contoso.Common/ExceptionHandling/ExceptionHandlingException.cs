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
using System.Runtime.Serialization;

namespace Contoso.Common.ExceptionHandling
{
    /// <summary>
    /// Exception that can occur if the exception handling fails. The inner Exception will hold the original exception
    /// where the HandlingException will hold the message about why the exception handling failed. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
    public class ExceptionHandlingException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingException"/> class.
        /// </summary>
        public ExceptionHandlingException()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ExceptionHandlingException(string message) : base(message)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ExceptionHandlingException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected ExceptionHandlingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private Exception handlingException;

        /// <summary>
        /// Exception that occurred while handling an other exception. 
        /// </summary>
        public Exception HandlingException
        {
            get { return handlingException; }
            set
            {
                handlingException = value;
                this.Data["HandlingException"] = value.ToString();
            }
        }
    }
}