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

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
{
    /// <summary>
    /// Exception that is thrown if the service locator is ran from a place where the SharePoint context
    /// could not be retrieved. For example, SPFarm.Local == null. 
    /// </summary>
    [Serializable]
    public class NoSharePointContextException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSharePointContextException"/> class.
        /// </summary>
        public NoSharePointContextException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSharePointContextException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NoSharePointContextException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSharePointContextException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public NoSharePointContextException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSharePointContextException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected NoSharePointContextException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
