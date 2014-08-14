//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Collections.Generic;

using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
{
    /// <summary>
    /// Interface for classes that will create and configure service locators, in such a way that
    /// they can be used by the <see cref="SharePointServiceLocator"/>. If you register
    /// an IServiceLocatorFactory in the <see cref="ServiceLocatorConfig"/>, it will use that 
    /// factory to create the service locator instance. 
    /// </summary>
    public interface IServiceLocatorFactory
    {
        /// <summary>
        /// Create the <see cref="IServiceLocator"/>
        /// </summary>
        /// <returns>The created service locator</returns>
        IServiceLocator Create();

        /// <summary>
        /// Loads the type mappings into the service locator. 
        /// </summary>
        /// <param name="serviceLocator">The service locator to load type mappings into.</param>
        /// <param name="typeMappings">The type mappings to load</param>
        void LoadTypeMappings(IServiceLocator serviceLocator, IEnumerable<TypeMapping> typeMappings);
    }
}