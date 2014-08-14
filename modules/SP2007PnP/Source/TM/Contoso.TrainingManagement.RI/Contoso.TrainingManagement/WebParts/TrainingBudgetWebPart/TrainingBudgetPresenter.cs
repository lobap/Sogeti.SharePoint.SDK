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

using Contoso.AccountingManagement.Services;
using Contoso.HRManagement.Services;

namespace Contoso.TrainingManagement.WebParts
{
    /// <summary>
    /// The TrainingBudgetPresenter handles the presentation
    /// logic for the Training Budget WebPart.
    /// </summary>
    public class TrainingBudgetPresenter
    {
        #region Private Fields

        private readonly ITrainingBudgetView view;
        private readonly IAccountingManager acct;
        private readonly IHRManager hr;

        #endregion

        #region Constructor

        public TrainingBudgetPresenter(ITrainingBudgetView view)
        {
            ServiceLocator serviceLocator = ServiceLocator.GetInstance();

            this.view = view;
            this.acct = serviceLocator.Get<IAccountingManager>();
            this.hr = serviceLocator.Get<IHRManager>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populates the cost centers control of the view.
        /// </summary>
        public void SetCostCenterSource()
        {
            IList<string> costCenters = hr.GetCostCenters();
            view.CostCenters = costCenters;                                 
        }

        /// <summary>
        /// Populates the budget and transaction controls of the view.
        /// </summary>
        public void SetTransactionSource()
        {
            string costCenter = view.CostCenter;

            view.TotalBudget = acct.GetBudget(costCenter, "Training");
            view.RemainingBudget = acct.GetRemainingBudget(costCenter, "Training");
            view.Transactions = acct.GetTransactions(costCenter, "Training");
        }

        #endregion
    }
}
