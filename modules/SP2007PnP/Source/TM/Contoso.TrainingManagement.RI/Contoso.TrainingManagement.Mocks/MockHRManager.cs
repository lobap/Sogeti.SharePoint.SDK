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
using Contoso.HRManagement.Services;

namespace Contoso.TrainingManagement.Mocks
{
    public class MockHRManager : IHRManager
    {
        #region IHRManager Members

        public string GetManager(string userName)
        {
            return String.Format(@"{0}\{1}", Environment.MachineName, "spgmanager");
        }

        public string GetCostCenter(string userName)
        {
            return "DEP100";
        }

        public IList<string> GetCostCenters()
        {
            IList<string> costCenters = new List<string>() { "DEP100", "DEP200" };
            return costCenters;
        }

        public IList<string> GetDirectReports(string userName)
        {
            IList<string> directReports = new List<string>();

            if ( userName == string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager") )
            {
                directReports.Add(string.Format(@"{0}\{1}", Environment.MachineName, "spgemployee"));
            }

            return directReports;
        }

        #endregion
    }
}
