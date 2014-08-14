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
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contoso.Common.Tests
{
    [TestClass]
    public class ServiceLocatorFixture
    {
        [TestMethod]
        public void CanResolveLogger()
        {
            var logger = ServiceLocator.Current.GetInstance<ILogger>();
            Assert.IsInstanceOfType(logger, typeof(Logger));
        }

    }
}
