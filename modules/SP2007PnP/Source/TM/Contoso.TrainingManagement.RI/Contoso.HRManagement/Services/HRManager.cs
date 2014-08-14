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

namespace Contoso.HRManagement.Services
{
    /// <summary>
    /// The HRManager is responsible for providing HR related
    /// information such as an employee's manager.
    /// </summary>
    public class HRManager : IHRManager
    {
        #region IHRManager Members

        /// <summary>
        /// The GetManager method returns a user's manager.
        /// </summary>
        /// <param name="userName">the username of the employee</param>
        /// <returns>the name of the employee's manager</returns>
        public string GetManager(string userName)
        {
            return String.Format(@"{0}\{1}", Environment.MachineName, "spgmanager");            
        }

        /// <summary>
        /// The GetCostCenter method returns a user's cost center.
        /// </summary>
        /// <param name="userName">the username of the employee</param>
        /// <returns>the name of the employee's cost center</returns>
        public string GetCostCenter(string userName)
        {
            return "DEP100";
        }

        /// <summary>
        /// The GetCostCenters method returns all cost center.
        /// </summary>
        /// <returns>all cost center names</returns>
        public IList<string> GetCostCenters()
        {
            IList<string> costCenters = new List<string>();
            costCenters.Add("DEP100");
            costCenters.Add("DEP200");
            costCenters.Add("DEP300");
            return costCenters;
        }

        /// <summary>
        /// The GetDirectReports method returns all of the direct reports of a user.
        /// </summary>
        /// <param name="userName">the username of the manager</param>
        /// <returns>all of the manager's direct reports</returns>
        public IList<string> GetDirectReports(string userName)
        {
            IList<string> directReports = new List<string>();

            if ( userName == string.Format(@"{0}\{1}", Environment.MachineName, "spgmanager") || userName == @"SHAREPOINT\system" )
            {
                directReports.Add(string.Format(@"{0}\{1}", Environment.MachineName, "spgemployee"));
            }

            return directReports;
        }

        #endregion
    }
}
