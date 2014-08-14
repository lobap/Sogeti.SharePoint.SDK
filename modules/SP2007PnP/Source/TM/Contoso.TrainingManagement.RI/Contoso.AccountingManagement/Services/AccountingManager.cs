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
using System.Data;
using System.Text;

using Contoso.AccountingManagement.Data;
using Contoso.AccountingManagement.BusinessEntities;

namespace Contoso.AccountingManagement.Services
{
    /// <summary>
    /// The AccountingManager class will return accounting information such as total budget,
    /// remaining budget and all transactions related to a Cost Center
    /// </summary>
    public class AccountingManager : IAccountingManager
    {
        #region Private Properties

        private static readonly object lockObject = new object();

        #endregion

        #region IAccountingManager Members

        /// <summary>
        /// The GetBudget method returns the total budget of a Cost Center for a specific bucket
        /// </summary>
        /// <param name="costCenter">The costCenter is an organization or department</param>
        /// <param name="bucket">The bucket is the category</param>
        /// <returns>the total budget</returns>
        public float GetBudget(string costCenter, string bucket)
        {
            float budget = float.NaN;

            string filterExpression = string.Format("CostCenter = '{0}' and Bucket = '{1}'", costCenter.Replace("'", "''"), bucket.Replace("'", "''"));

            DataRow[] rows = null;

            lock (AccountingManager.lockObject)
            {
                rows = AccountingDataSet.Instance.Budget.Select(filterExpression);
            }

            if (rows != null && rows.Length > 0)
            {
                budget = (float)rows[0]["Amount"];
            }

            return budget;
        }

        /// <summary>
        /// The GetRemainingBudget budget returns the amount of money remaining in the budget
        /// for a specific Cost Center by bucket.
        /// </summary>
        /// <param name="costCenter">The costCenter is an organization or department</param>
        /// <param name="bucket">The bucket is the category</param>
        /// <returns>remaining budget</returns>
        public float GetRemainingBudget(string costCenter, string bucket)
        {
            float remainingBudget = float.NaN;

            float budget = this.GetBudget(costCenter, bucket);
            if (!float.IsNaN(budget))
            {
                remainingBudget = budget;

                IList<Transaction> transactions = this.GetTransactions(costCenter, bucket);
                foreach (Transaction transaction in transactions)
                {
                    remainingBudget -= transaction.Amount;
                }
            }

            return remainingBudget;
        }

        /// <summary>
        /// The GetTransactions method returns a list of transactions made
        /// for a cost center and bucket
        /// </summary>
        /// <param name="costCenter">the cost center is an organization or department</param>
        /// <param name="bucket">the bucket is the category</param>
        /// <returns></returns>
        public IList<Transaction> GetTransactions(string costCenter, string bucket)
        {
            IList<Transaction> transactions = new List<Transaction>();

            string filterExpression = string.Format("CostCenter = '{0}' and Bucket = '{1}'", costCenter.Replace("'", "''"), bucket.Replace("'", "''"));

            DataRow[] rows = null;

            lock (AccountingManager.lockObject)
            {
                rows = AccountingDataSet.Instance.Transaction.Select(filterExpression);
            }

            foreach (AccountingDataSet.TransactionRow row in rows)
            {
                Transaction transaction = new Transaction();
                transaction.Amount = row.Amount;
                transaction.Bucket = row.Bucket;
                transaction.CostCenter = row.CostCenter;
                transaction.Description = row.Description;

                transactions.Add(transaction);
            }

            return transactions;
        }

        /// <summary>
        /// The SaveTransaction will save a specific transaction for a cost center
        /// </summary>
        /// <param name="transaction">A transaction object</param>
        public void SaveTransaction(Transaction transaction)
        {
            ValidateTransaction(transaction);

            lock (AccountingManager.lockObject)
            {
                AccountingDataSet.TransactionRow row = AccountingDataSet.Instance.Transaction.NewTransactionRow();

                row.Amount = transaction.Amount;
                row.Bucket = transaction.Bucket;
                row.CostCenter = transaction.CostCenter;
                row.Description = transaction.Description;

                AccountingDataSet.Instance.Transaction.AddTransactionRow(row);
                AccountingDataSet.Instance.AcceptChanges();
            }
        }

        private static void ValidateTransaction(Transaction transaction)
        {
            StringBuilder errorMessage = new StringBuilder();

            if (float.IsNaN(transaction.Amount))
                errorMessage.AppendLine("Unable to save transaction due to invalid transaction amount.");

            if (errorMessage.Length > 0)
                throw new ArgumentException(errorMessage.ToString().Trim());
        }

        #endregion
    }
}
