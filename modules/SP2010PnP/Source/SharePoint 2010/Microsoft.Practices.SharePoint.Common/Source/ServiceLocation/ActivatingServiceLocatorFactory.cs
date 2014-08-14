//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
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
            return new ActivatingServiceLocator();
        }


        /// <summary>
        /// Loads the type mappings.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="typeMappings">The mappings.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void LoadTypeMappings(IServiceLocator serviceLocator, IEnumerable<TypeMapping> typeMappings)
        {
            if (typeMappings == null)
                return;

            Validation.ArgumentNotNull(serviceLocator, "serviceLocator");
            Validation.TypeIsAssignable(typeof(ActivatingServiceLocator), serviceLocator.GetType(), "serviceLocator");

            ActivatingServiceLocator activatingServiceLocator = serviceLocator as ActivatingServiceLocator;

            foreach(TypeMapping typeMapping in typeMappings)
            {
                activatingServiceLocator.RegisterTypeMapping(typeMapping);
            }
        }
    }
}
