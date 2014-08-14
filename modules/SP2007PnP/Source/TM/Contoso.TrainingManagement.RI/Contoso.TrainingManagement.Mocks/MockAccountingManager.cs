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
using Contoso.AccountingManagement.BusinessEntities;
using Contoso.AccountingManagement.Services;

namespace Contoso.TrainingManagement.Mocks
{
    public class MockAccountingManager : IAccountingManager
    {
        public static Transaction savedTransaction;
        public static float totalBudget = 5000f;
        public static float remainingBudget = 5000f;
        public static int transactionCount = 0;

        #region IAccountingManager Members

        public float GetBudget(string costCenter, string bucket)
        {
            return totalBudget;
        }

        public float GetRemainingBudget(string costCenter, string bucket)
        {
            return remainingBudget;
        }

        public IList<Transaction> GetTransactions(string costCenter, string bucket)
        {
            IList<Transaction> transactions = new List<Transaction>();

            for (int i = 0; i < transactionCount; i++)
            {
                Transaction transaction = new Transaction();
                transaction.Amount = 500f;
                transaction.Bucket = bucket;
                transaction.CostCenter = costCenter;
                transaction.Description = "UNITTEST";

                transactions.Add(transaction);
            }

            return transactions;
        }

        public void SaveTransaction(Transaction transaction)
        {
            savedTransaction = transaction;
        }

        #endregion
    }
}
