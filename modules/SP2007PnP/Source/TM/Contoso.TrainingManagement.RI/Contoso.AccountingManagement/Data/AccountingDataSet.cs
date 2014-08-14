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
namespace Contoso.AccountingManagement.Data
{
    public partial class AccountingDataSet
    {
        #region Private Fields

        private static AccountingDataSet instance;
        private static readonly object lockObject = new object();

        #endregion

        #region Public Properties

        public static AccountingDataSet Instance
        {
            get
            {
                lock ( lockObject )
                {
                    if ( instance == null )
                    {
                        instance = CreateInstance();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Methods

        private static AccountingDataSet CreateInstance()
        {
            AccountingDataSet accountingDataSet = new AccountingDataSet();
            
            accountingDataSet.Budget.AddBudgetRow("DEP100", "TRAINING", 5000f);
            accountingDataSet.Budget.AddBudgetRow("DEP200", "TRAINING", 8000f);
            accountingDataSet.Budget.AddBudgetRow("DEP300", "TRAINING", 8000f);

            return accountingDataSet;
        }

        #endregion
    }
}
