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

using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SPG.Common.ServiceLocation
{
    /// <summary>
    /// Class that will create an activating service locator and load it up with type mappings. 
    /// </summary>
    public class ActivatingServiceLocatorFactory : IServiceLocatorFactory
    {
        /// <summary>
        /// Create the <see cref="IServiceLocator"/> and loads it with type mappings. 
        /// </summary>
        /// <returns>The created service locator</returns>
        public IServiceLocator Create()
        {
            ActivatingServiceLocator serviceLocator = new ActivatingServiceLocator();

            return serviceLocator;
        }


        /// <summary>
        /// Loads the type mappings.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="typeMappings">The mappings.</param>
        public void LoadTypeMappings(IServiceLocator serviceLocator, IEnumerable<TypeMapping> typeMappings)
        {
            if (serviceLocator == null)
            {
                throw new ArgumentNullException("serviceLocator");
            }

            ActivatingServiceLocator activatingServiceLocator = serviceLocator as ActivatingServiceLocator;

            if (activatingServiceLocator == null)
            {
                throw new ArgumentException("serviceLocator must be a ActivatingServiceLocator, not a " + serviceLocator.GetType().Name, "serviceLocator");
            }

            if (typeMappings == null)
            {
                return;
            }

            foreach(TypeMapping typeMapping in typeMappings)
            {
                activatingServiceLocator.RegisterTypeMapping(typeMapping);
            }
        }

    }
}
