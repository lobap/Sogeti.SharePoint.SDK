//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
{
    /// <summary>
    /// Determines how to instantiate objects from the service locator
    /// </summary>
    public enum InstantiationType
    {
        /// <summary>
        /// Create a new instance for each call to <see cref="IServiceLocator.GetInstance(System.Type)"/>. 
        /// </summary>
        NewInstanceForEachRequest,

        /// <summary>
        /// Create a singleton instance. Each call to <see cref="IServiceLocator.GetInstance(System.Type)"/> will return the same instance.
        /// </summary>
        AsSingleton
    }
}
