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
using Contoso.AccountingManagement.BusinessEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contoso.AccountingManagement.Data;
using Contoso.AccountingManagement.Services;

namespace Contoso.AccountingManagement.Tests
{
    /// <summary>
    /// This unit test will cover the objects in the AccountingManagement assembly
    /// </summary>
    [TestClass]
    public class AccountingManagementFixture
    {
        [TestMethod]
        public void AccountingDataSetInstanceNotNullTest()
        {
            Assert.IsNotNull(AccountingDataSet.Instance);
        }

        [TestMethod]
        public void AccountingDataSetInstanceBudgetsRowCountTest()
        {
            Assert.AreEqual(3, AccountingDataSet.Instance.Budget.Count);
        }

        [TestMethod]
        public void AccountingManagerGetBudgetPositiveTest()
        {
            AccountingManager accountingManager = new AccountingManager();
            float budget = accountingManager.GetBudget("DEP100", "TRAINING");

            Assert.AreNotEqual(float.NaN, budget);
            Assert.AreEqual(5000f, budget);
        }

        [TestMethod]
        public void AccountingManagerGetBudgetNegativeTest()
        {
            AccountingManager accountingManager = new AccountingManager();
            float budget = accountingManager.GetBudget("DUMMY", "TRAINING");

            Assert.AreEqual(float.NaN, budget);
        }

        [TestMethod]
        public void AccountManagerSaveTransactionPositiveTest()
        {
            AccountingManager accountingManager = new AccountingManager();

            Transaction transaction = new Transaction();
            transaction.Amount = 500f;
            transaction.Bucket = "UnitTest1";
            transaction.CostCenter = "UnitTest1";
            transaction.Description = "This was created by a unit test.";
            accountingManager.SaveTransaction(transaction);

            IList<Transaction> transactions = accountingManager.GetTransactions("UnitTest1", "UnitTest1");

            Assert.IsNotNull(transactions);
            Assert.AreEqual(1, transactions.Count);
            Assert.AreEqual(transaction.Amount, transactions[0].Amount);
            Assert.AreEqual(transaction.Bucket, transactions[0].Bucket);
            Assert.AreEqual(transaction.CostCenter, transactions[0].CostCenter);
            Assert.AreEqual(transaction.Description, transactions[0].Description);
        }

        [TestMethod]
        public void AccountingManagerGetTransactionsNegativeTest()
        {
            AccountingManager accountingManager = new AccountingManager();

            Transaction transaction = new Transaction();
            transaction.Amount = 500f;
            transaction.Bucket = "UnitTest2";
            transaction.CostCenter = "UnitTest2";
            transaction.Description = "This was created by a unit test.";
            accountingManager.SaveTransaction(transaction);

            IList<Transaction> transactions = accountingManager.GetTransactions("WrongCc", "WrongBkt");

            Assert.IsNotNull(transactions);
            Assert.AreEqual(0, transactions.Count);
        }

        [TestMethod]
        public void AccountManagerGetRemainingBudgetPositiveTest()
        {
            AccountingManager accountingManager = new AccountingManager();

            Transaction transaction = new Transaction();
            transaction.Amount = 500f;
            transaction.Bucket = "TRAINING";
            transaction.CostCenter = "DEP100";
            transaction.Description = "This was created by a unit test.";
            accountingManager.SaveTransaction(transaction);

            float remainingBudget = accountingManager.GetRemainingBudget("DEP100", "TRAINING");
            float budget = accountingManager.GetBudget("DEP100", "TRAINING");
            float expectedBudget = budget - transaction.Amount;

            Assert.AreEqual(expectedBudget, remainingBudget);
        }

        [TestMethod]
        public void SpecialCharactersinCostCenterOrBucketReturnNaN()
        {
            AccountingManager accountingManager = new AccountingManager();

            float actual = accountingManager.GetBudget("DEP100'", "TRAININGTest");

            Assert.AreEqual(float.NaN, actual);
        }

        [TestMethod]
        public void SaveTransactionwithNaN()
        {
            AccountingManager target = new AccountingManager();
            string costCenter = "DEP100";
            string bucket = "TRAINING";
            float expected = 5000f;
            float actual;

            actual = target.GetBudget(costCenter, bucket);
            Assert.AreEqual(expected, actual);

            //do some trans here
            Transaction transaction = new Transaction();
            transaction.CostCenter = costCenter;
            transaction.Bucket = bucket;
            transaction.Amount = float.NaN;
            transaction.Description = "Testing..";

            try
            {
                target.SaveTransaction(transaction);    
                Assert.Fail();
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Unable to save transaction due to invalid transaction amount.", ex.Message);
            }
        }
    }
}
