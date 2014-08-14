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
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.SPG.AJAXSupport.UnitTests
{
    [TestClass]
    public class ModificationFixture
    {
        [TestMethod]
        public void ModificationOwnerIsNameOfAssembly()
        {
            Modification modification = new Modification(null, null, null, 0, SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode, false);

            Assert.AreEqual("Microsoft.Practices.SPG.AJAXSupport", modification.GetOwner()  );
        }

        [TestMethod]
        public void GetNameReturnsXPathOfNodeNameAndAttributes()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>()
                                                        {
                                                            {"attrib1", "value1"},
                                                            {"attrib2", "value2"}
                                                        };

            Modification modification = new Modification("nodeName", attributes, null, 0, SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode, false);

            Assert.AreEqual("nodeName[@attrib1='value1'][@attrib2='value2']", modification.GetName());
        }

        [TestMethod]
        public void GetValueReturnsXMLOfNodeNameAndAttributes()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>()
                                                        {
                                                            {"attrib1", "value1"},
                                                            {"attrib2", "value2"}
                                                        };

            Modification modification = new Modification("nodeName", attributes, null, 0, SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode, false);

            Assert.AreEqual("<nodeName attrib1='value1' attrib2='value2' />", modification.GetValue());
        }
	
    }
}
