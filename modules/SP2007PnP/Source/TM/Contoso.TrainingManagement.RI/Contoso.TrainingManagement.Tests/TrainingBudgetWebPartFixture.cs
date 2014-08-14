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
using Contoso.AccountingManagement.Services;
using Contoso.HRManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contoso.TrainingManagement.Mocks;
using Contoso.TrainingManagement.WebParts;

namespace Contoso.TrainingManagement.Tests
{
    /// <summary>
    /// Summary description for TrainingBudgetWebPartFixture
    /// </summary>
    [TestClass]
    public class TrainingBudgetWebPartFixture
    {
        #region Private Fields

        private TrainingBudgetMockView mockView;
        private ServiceLocator serviceLocator = ServiceLocator.GetInstance();

        #endregion

        #region Test Prep

        [TestInitialize()]
        public void TestInit()
        {
            ServiceLocator.Clear();
            this.mockView = new TrainingBudgetMockView();
        }

        #endregion

        #region Test Cleanup

        [TestCleanup]
        public void TestCleanup()
        {
            serviceLocator.Reset();
        }

        #endregion

        #region Test Methods

        #region SetCostCenterSource

        /// <summary>
        /// Populates list of cost centers
        /// </summary>
        [TestMethod]
        public void SetCostCenterSourceTest()
        {            
            serviceLocator.Register<IAccountingManager>(typeof(MockAccountingManager));
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));
            
            TrainingBudgetPresenter presenter = new TrainingBudgetPresenter(this.mockView);

            presenter.SetCostCenterSource();

            //assert cost centers
            Assert.IsNotNull(this.mockView.CostCenters);
            Assert.AreEqual<int>(2, this.mockView.CostCenters.Count);
        }

        #endregion

        #region SetTransactionSource

        /// <summary>
        /// Without transaction data
        /// </summary>
        [TestMethod]
        public void SetTransactionSourceTest()
        {
            this.mockView.CostCenter = "DEP100";

            serviceLocator.Register<IAccountingManager>(typeof(MockAccountingManager));
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));
            MockAccountingManager.totalBudget = 5000f;
            MockAccountingManager.remainingBudget = 5000f;
            MockAccountingManager.transactionCount = 0;
            TrainingBudgetPresenter presenter = new TrainingBudgetPresenter(this.mockView);

            presenter.SetTransactionSource();

            //assert total budget
            Assert.AreNotEqual(float.NaN, this.mockView.TotalBudget);
            Assert.AreEqual(5000f, this.mockView.TotalBudget);

            //assert remaining budget
            Assert.AreNotEqual(float.NaN, this.mockView.RemainingBudget);
            Assert.AreEqual(5000f, this.mockView.RemainingBudget);

            //assert transactions
            Assert.IsNotNull(this.mockView.Transactions);
            Assert.AreEqual<int>(0, this.mockView.Transactions.Count);
        }

        /// <summary>
        /// With transaction data
        /// </summary>
        [TestMethod]
        public void SetDataSourceAfterTransactionTest()
        {
            this.mockView.CostCenter = "DEP100";

            serviceLocator.Register<IAccountingManager>(typeof(MockAccountingManager));
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));
            MockAccountingManager.totalBudget = 5000f;
            MockAccountingManager.remainingBudget = 4500f;
            MockAccountingManager.transactionCount = 1;
            TrainingBudgetPresenter presenter = new TrainingBudgetPresenter(this.mockView);

            presenter.SetTransactionSource();

            //assert total budget
            Assert.AreNotEqual(float.NaN, this.mockView.TotalBudget);
            Assert.AreEqual(5000f, this.mockView.TotalBudget);

            //assert remaining budget
            Assert.AreNotEqual(float.NaN, this.mockView.RemainingBudget);
            Assert.AreEqual(4500f, this.mockView.RemainingBudget);

            //assert transactions
            Assert.AreEqual<int>(1, this.mockView.Transactions.Count);
            Assert.AreEqual<float>(500f, this.mockView.Transactions[0].Amount);
            Assert.AreEqual<string>("Training", this.mockView.Transactions[0].Bucket);
            Assert.AreEqual<string>(this.mockView.CostCenter, this.mockView.Transactions[0].CostCenter);
            Assert.AreEqual<string>("UNITTEST", this.mockView.Transactions[0].Description);
        }

        /// <summary>
        /// There was a DropDownList ViewState issue in the web part
        /// </summary>
        [TestMethod]
        public void EmptyCostCenterTest()
        {
            this.mockView.CostCenter = String.Empty;

            serviceLocator.Register<IAccountingManager>(typeof(MockAccountingManager));
            serviceLocator.Register<IHRManager>(typeof(MockHRManager));
            MockAccountingManager.totalBudget = float.NaN;
            MockAccountingManager.remainingBudget = float.NaN;
            MockAccountingManager.transactionCount = 0;
            TrainingBudgetPresenter presenter = new TrainingBudgetPresenter(this.mockView);

            presenter.SetTransactionSource();

            //assert total budget
            Assert.AreEqual(float.NaN, this.mockView.TotalBudget);

            //assert remaining budget
            Assert.AreEqual(float.NaN, this.mockView.RemainingBudget);

            //assert transactions
            Assert.IsNotNull(this.mockView.Transactions);
            Assert.AreEqual<int>(0, this.mockView.Transactions.Count);
        }

        #endregion

        #endregion

        #region Private Mocks

        private class TrainingBudgetMockView : ITrainingBudgetView
        {
            #region Private Fields

            private string costCenter;
            private IList<string> costCenters;
            private float totalBudget;
            private float remainingBudget;
            private IList<Transaction> transactions;

            #endregion

            #region ITrainingBudgetView Members

            public string CostCenter
            {
                get
                {
                    return costCenter;
                }
                set
                {
                    this.costCenter = value;
                }
            }

            public IList<string> CostCenters
            {
                get
                {
                    return this.costCenters;
                }
                set
                {
                    this.costCenters = value;
                }                
            }

            public IList<Transaction> Transactions
            {
                get
                {
                    return this.transactions;
                }
                set
                {
                    this.transactions = value;
                }
            }

            public float TotalBudget
            {
                get
                {
                    return this.totalBudget;
                }
                set
                {
                    this.totalBudget = value;
                }
            }

            public float RemainingBudget
            {
                get
                {
                    return this.remainingBudget;
                }
                set
                {
                    this.remainingBudget = value;
                }
            }

            #endregion
        }

        #endregion
    }
}
