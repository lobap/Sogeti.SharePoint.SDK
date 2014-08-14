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
using System.Collections.Generic;

using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SPG.Common.ServiceLocation
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