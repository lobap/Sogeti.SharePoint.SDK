//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Runtime.Serialization;

namespace ExecutionModels.ExceptionHandling
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
        public ExceptionHandlingException(string message)
            : base(message)
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