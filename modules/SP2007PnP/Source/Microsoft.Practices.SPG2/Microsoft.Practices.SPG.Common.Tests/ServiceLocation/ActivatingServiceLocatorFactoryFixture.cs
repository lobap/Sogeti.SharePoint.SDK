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
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.SPG.Common.Tests.ServiceLocation
{
    [TestClass]
    public class ActivatingServiceLocatorFactoryFixture
    {
        [TestMethod]
        public void FactoryCreatesServiceLocator()
        {
            var target = new ActivatingServiceLocatorFactory();

            ActivatingServiceLocator serviceLocator = target.Create() as ActivatingServiceLocator;

            Assert.IsNotNull(serviceLocator);

        }

        [TestMethod]
        public void CanLoadTypeMappings()
        {
            var target = new ActivatingServiceLocatorFactory();

            List<TypeMapping> typeMappings = new List<TypeMapping>();
            typeMappings.Add(TypeMapping.Create<ISomething, Something>());

            var serviceLocator = new ActivatingServiceLocator();

            target.LoadTypeMappings(serviceLocator, typeMappings);

            Assert.IsTrue(serviceLocator.IsTypeRegistered<ISomething>());
        }

        [TestMethod]
        public void TypeMappingsNullDoesNotThrow()
        {
            var target = new ActivatingServiceLocatorFactory();
            target.LoadTypeMappings(new ActivatingServiceLocator(), null);
        }
    }

}
