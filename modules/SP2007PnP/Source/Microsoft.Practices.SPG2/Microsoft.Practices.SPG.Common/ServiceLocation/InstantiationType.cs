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
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SPG.Common.ServiceLocation
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
