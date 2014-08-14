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
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Contoso.HRManagement.Services;

namespace Contoso.HRManagement.Tests
{
    /// <summary>
    /// Summary description for HRManagementFixture
    /// </summary>
    [TestClass]
    public class HRManagementFixture
    {
        [TestMethod]
        public void HRManagerGetManagerTest()
        {
            HRManager hrManager = new HRManager();
            string manager = hrManager.GetManager("testuser");
            Assert.AreEqual(String.Format(@"{0}\{1}", Environment.MachineName, "spgmanager"), manager);
        }

        [TestMethod]
        public void HRManagerGetCostCenterTest()
        {
            HRManager hrManager = new HRManager();
            string costCenter = hrManager.GetCostCenter("testuser");
            Assert.AreEqual("DEP100", costCenter);
        }

        [TestMethod]
        public void HRManagerGetCostCentersTest()
        {
            HRManager hrManager = new HRManager();
            IList<string> costCenters = hrManager.GetCostCenters();
            Assert.AreEqual(3, costCenters.Count);
            Assert.AreEqual("DEP100", costCenters[0]);
            Assert.AreEqual("DEP200", costCenters[1]);
            Assert.AreEqual("DEP300", costCenters[2]);
        }

        [TestMethod]
        public void HRManagerGetDirectReports()
        {
            HRManager hrManager = new HRManager();
            IList<string> directReports = hrManager.GetDirectReports(string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager"));
            Assert.AreEqual(string.Format(@"{0}\{1}", Environment.MachineName, "spgemployee"), directReports[0]);
        }

        [TestMethod]
        public void HRManagerGetDirectReportsWithSystemAccount()
        {
            HRManager hrManager = new HRManager();
            IList<string> directReports = hrManager.GetDirectReports(@"SHAREPOINT\system");
            Assert.AreEqual(string.Format(@"{0}\{1}", Environment.MachineName, "spgemployee"), directReports[0]);
        }

        [TestMethod]
        public void HRManagerGetDirectReportsNoDirectReports()
        {
            HRManager hrManager = new HRManager();
            IList<string> directReports = hrManager.GetDirectReports("newemployee");
            Assert.AreEqual(0, directReports.Count);
        }
    }
}
